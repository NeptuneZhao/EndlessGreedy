using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000984 RID: 2436
public class GermExposureMonitor : GameStateMachine<GermExposureMonitor, GermExposureMonitor.Instance>
{
	// Token: 0x06004727 RID: 18215 RVA: 0x00197250 File Offset: 0x00195450
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.Update(delegate(GermExposureMonitor.Instance smi, float dt)
		{
			smi.OnInhaleExposureTick(dt);
		}, UpdateRate.SIM_1000ms, true).EventHandler(GameHashes.EatCompleteEater, delegate(GermExposureMonitor.Instance smi, object obj)
		{
			smi.OnEatComplete(obj);
		}).EventHandler(GameHashes.SicknessAdded, delegate(GermExposureMonitor.Instance smi, object data)
		{
			smi.OnSicknessAdded(data);
		}).EventHandler(GameHashes.SicknessCured, delegate(GermExposureMonitor.Instance smi, object data)
		{
			smi.OnSicknessCured(data);
		}).EventHandler(GameHashes.SleepFinished, delegate(GermExposureMonitor.Instance smi)
		{
			smi.OnSleepFinished();
		});
	}

	// Token: 0x06004728 RID: 18216 RVA: 0x0019733D File Offset: 0x0019553D
	public static float GetContractionChance(float rating)
	{
		return 0.5f - 0.5f * (float)Math.Tanh(0.25 * (double)rating);
	}

	// Token: 0x02001938 RID: 6456
	public enum ExposureState
	{
		// Token: 0x040078CC RID: 30924
		None,
		// Token: 0x040078CD RID: 30925
		Contact,
		// Token: 0x040078CE RID: 30926
		Exposed,
		// Token: 0x040078CF RID: 30927
		Contracted,
		// Token: 0x040078D0 RID: 30928
		Sick
	}

	// Token: 0x02001939 RID: 6457
	public class ExposureStatusData
	{
		// Token: 0x040078D1 RID: 30929
		public ExposureType exposure_type;

		// Token: 0x040078D2 RID: 30930
		public GermExposureMonitor.Instance owner;
	}

	// Token: 0x0200193A RID: 6458
	[SerializationConfig(MemberSerialization.OptIn)]
	public new class Instance : GameStateMachine<GermExposureMonitor, GermExposureMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009BA7 RID: 39847 RVA: 0x003700C0 File Offset: 0x0036E2C0
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.sicknesses = master.GetComponent<MinionModifiers>().sicknesses;
			this.primaryElement = master.GetComponent<PrimaryElement>();
			this.traits = master.GetComponent<Traits>();
			this.lastDiseaseSources = new Dictionary<HashedString, GermExposureMonitor.Instance.DiseaseSourceInfo>();
			this.lastExposureTime = new Dictionary<HashedString, float>();
			this.inhaleExposureTick = new Dictionary<HashedString, GermExposureMonitor.Instance.InhaleTickInfo>();
			GameClock.Instance.Subscribe(-722330267, new Action<object>(this.OnNightTime));
			OxygenBreather component = base.GetComponent<OxygenBreather>();
			if (component != null)
			{
				OxygenBreather oxygenBreather = component;
				oxygenBreather.onSimConsume = (Action<Sim.MassConsumedCallback>)Delegate.Combine(oxygenBreather.onSimConsume, new Action<Sim.MassConsumedCallback>(this.OnAirConsumed));
			}
		}

		// Token: 0x06009BA8 RID: 39848 RVA: 0x00370198 File Offset: 0x0036E398
		public override void StartSM()
		{
			base.StartSM();
			this.RefreshStatusItems();
		}

		// Token: 0x06009BA9 RID: 39849 RVA: 0x003701A8 File Offset: 0x0036E3A8
		public override void StopSM(string reason)
		{
			GameClock.Instance.Unsubscribe(-722330267, new Action<object>(this.OnNightTime));
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				Guid guid;
				this.statusItemHandles.TryGetValue(exposureType.germ_id, out guid);
				guid = base.GetComponent<KSelectable>().RemoveStatusItem(guid, false);
			}
			base.StopSM(reason);
		}

		// Token: 0x06009BAA RID: 39850 RVA: 0x00370214 File Offset: 0x0036E414
		public void OnEatComplete(object obj)
		{
			Edible edible = (Edible)obj;
			HandleVector<int>.Handle handle = GameComps.DiseaseContainers.GetHandle(edible.gameObject);
			if (handle != HandleVector<int>.InvalidHandle)
			{
				DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(handle);
				if (header.diseaseIdx != 255)
				{
					Disease disease = Db.Get().Diseases[(int)header.diseaseIdx];
					float num = edible.unitsConsumed / (edible.unitsConsumed + edible.Units);
					int num2 = Mathf.CeilToInt((float)header.diseaseCount * num);
					GameComps.DiseaseContainers.ModifyDiseaseCount(handle, -num2);
					KPrefabID component = edible.GetComponent<KPrefabID>();
					this.InjectDisease(disease, num2, component.PrefabID(), Sickness.InfectionVector.Digestion);
				}
			}
		}

		// Token: 0x06009BAB RID: 39851 RVA: 0x003702C4 File Offset: 0x0036E4C4
		public void OnAirConsumed(Sim.MassConsumedCallback mass_cb_info)
		{
			if (mass_cb_info.diseaseIdx != 255)
			{
				Disease disease = Db.Get().Diseases[(int)mass_cb_info.diseaseIdx];
				this.InjectDisease(disease, mass_cb_info.diseaseCount, ElementLoader.elements[(int)mass_cb_info.elemIdx].tag, Sickness.InfectionVector.Inhalation);
			}
		}

		// Token: 0x06009BAC RID: 39852 RVA: 0x00370318 File Offset: 0x0036E518
		public void OnInhaleExposureTick(float dt)
		{
			foreach (KeyValuePair<HashedString, GermExposureMonitor.Instance.InhaleTickInfo> keyValuePair in this.inhaleExposureTick)
			{
				if (keyValuePair.Value.inhaled)
				{
					keyValuePair.Value.inhaled = false;
					keyValuePair.Value.ticks++;
				}
				else
				{
					keyValuePair.Value.ticks = Mathf.Max(0, keyValuePair.Value.ticks - 1);
				}
			}
		}

		// Token: 0x06009BAD RID: 39853 RVA: 0x003703B8 File Offset: 0x0036E5B8
		public void TryInjectDisease(byte disease_idx, int count, Tag source, Sickness.InfectionVector vector)
		{
			if (disease_idx != 255)
			{
				Disease disease = Db.Get().Diseases[(int)disease_idx];
				this.InjectDisease(disease, count, source, vector);
			}
		}

		// Token: 0x06009BAE RID: 39854 RVA: 0x003703E9 File Offset: 0x0036E5E9
		public float GetGermResistance()
		{
			return Db.Get().Attributes.GermResistance.Lookup(base.gameObject).GetTotalValue();
		}

		// Token: 0x06009BAF RID: 39855 RVA: 0x0037040C File Offset: 0x0036E60C
		public float GetResistanceToExposureType(ExposureType exposureType, float overrideExposureTier = -1f)
		{
			float num = overrideExposureTier;
			if (num == -1f)
			{
				num = this.GetExposureTier(exposureType.germ_id);
			}
			num = Mathf.Clamp(num, 1f, 3f);
			float num2 = GERM_EXPOSURE.EXPOSURE_TIER_RESISTANCE_BONUSES[(int)num - 1];
			float totalValue = Db.Get().Attributes.GermResistance.Lookup(base.gameObject).GetTotalValue();
			return (float)exposureType.base_resistance + totalValue + num2;
		}

		// Token: 0x06009BB0 RID: 39856 RVA: 0x00370478 File Offset: 0x0036E678
		public int AssessDigestedGerms(ExposureType exposure_type, int count)
		{
			int exposure_threshold = exposure_type.exposure_threshold;
			int val = count / exposure_threshold;
			return MathUtil.Clamp(1, 3, val);
		}

		// Token: 0x06009BB1 RID: 39857 RVA: 0x00370498 File Offset: 0x0036E698
		public bool AssessInhaledGerms(ExposureType exposure_type)
		{
			GermExposureMonitor.Instance.InhaleTickInfo inhaleTickInfo;
			this.inhaleExposureTick.TryGetValue(exposure_type.germ_id, out inhaleTickInfo);
			if (inhaleTickInfo == null)
			{
				inhaleTickInfo = new GermExposureMonitor.Instance.InhaleTickInfo();
				this.inhaleExposureTick[exposure_type.germ_id] = inhaleTickInfo;
			}
			if (!inhaleTickInfo.inhaled)
			{
				float exposureTier = this.GetExposureTier(exposure_type.germ_id);
				inhaleTickInfo.inhaled = true;
				return inhaleTickInfo.ticks >= GERM_EXPOSURE.INHALE_TICK_THRESHOLD[(int)exposureTier];
			}
			return false;
		}

		// Token: 0x06009BB2 RID: 39858 RVA: 0x00370510 File Offset: 0x0036E710
		public void InjectDisease(Disease disease, int count, Tag source, Sickness.InfectionVector vector)
		{
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				if (disease.id == exposureType.germ_id && count > exposureType.exposure_threshold && this.HasMinExposurePeriodElapsed(exposureType.germ_id) && this.IsExposureValidForTraits(exposureType))
				{
					Sickness sickness = (exposureType.sickness_id != null) ? Db.Get().Sicknesses.Get(exposureType.sickness_id) : null;
					if (sickness == null || sickness.infectionVectors.Contains(vector))
					{
						GermExposureMonitor.ExposureState exposureState = this.GetExposureState(exposureType.germ_id);
						float exposureTier = this.GetExposureTier(exposureType.germ_id);
						if (exposureState == GermExposureMonitor.ExposureState.None || exposureState == GermExposureMonitor.ExposureState.Contact)
						{
							float contractionChance = GermExposureMonitor.GetContractionChance(this.GetResistanceToExposureType(exposureType, -1f));
							this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.Contact);
							if (contractionChance > 0f)
							{
								this.lastDiseaseSources[disease.id] = new GermExposureMonitor.Instance.DiseaseSourceInfo(source, vector, contractionChance, base.transform.GetPosition());
								if (exposureType.infect_immediately)
								{
									this.InfectImmediately(exposureType);
								}
								else
								{
									bool flag = true;
									bool flag2 = vector == Sickness.InfectionVector.Inhalation;
									bool flag3 = vector == Sickness.InfectionVector.Digestion;
									int num = 1;
									if (flag2)
									{
										flag = this.AssessInhaledGerms(exposureType);
									}
									if (flag3)
									{
										num = this.AssessDigestedGerms(exposureType, count);
									}
									if (flag)
									{
										if (flag2)
										{
											this.inhaleExposureTick[exposureType.germ_id].ticks = 0;
										}
										this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.Exposed);
										this.SetExposureTier(exposureType.germ_id, (float)num);
										float amount = Mathf.Clamp01(contractionChance);
										GermExposureTracker.Instance.AddExposure(exposureType, amount);
									}
								}
							}
						}
						else if (exposureState == GermExposureMonitor.ExposureState.Exposed && exposureTier < 3f)
						{
							float contractionChance2 = GermExposureMonitor.GetContractionChance(this.GetResistanceToExposureType(exposureType, -1f));
							if (contractionChance2 > 0f)
							{
								this.lastDiseaseSources[disease.id] = new GermExposureMonitor.Instance.DiseaseSourceInfo(source, vector, contractionChance2, base.transform.GetPosition());
								if (!exposureType.infect_immediately)
								{
									bool flag4 = true;
									bool flag5 = vector == Sickness.InfectionVector.Inhalation;
									bool flag6 = vector == Sickness.InfectionVector.Digestion;
									int num2 = 1;
									if (flag5)
									{
										flag4 = this.AssessInhaledGerms(exposureType);
									}
									if (flag6)
									{
										num2 = this.AssessDigestedGerms(exposureType, count);
									}
									if (flag4)
									{
										if (flag5)
										{
											this.inhaleExposureTick[exposureType.germ_id].ticks = 0;
										}
										this.SetExposureTier(exposureType.germ_id, this.GetExposureTier(exposureType.germ_id) + (float)num2);
										float amount2 = Mathf.Clamp01(GermExposureMonitor.GetContractionChance(this.GetResistanceToExposureType(exposureType, -1f)) - contractionChance2);
										GermExposureTracker.Instance.AddExposure(exposureType, amount2);
									}
								}
							}
						}
					}
				}
			}
			this.RefreshStatusItems();
		}

		// Token: 0x06009BB3 RID: 39859 RVA: 0x003707D4 File Offset: 0x0036E9D4
		public GermExposureMonitor.ExposureState GetExposureState(string germ_id)
		{
			GermExposureMonitor.ExposureState result;
			this.exposureStates.TryGetValue(germ_id, out result);
			return result;
		}

		// Token: 0x06009BB4 RID: 39860 RVA: 0x003707F4 File Offset: 0x0036E9F4
		public float GetExposureTier(string germ_id)
		{
			float value = 1f;
			this.exposureTiers.TryGetValue(germ_id, out value);
			return Mathf.Clamp(value, 1f, 3f);
		}

		// Token: 0x06009BB5 RID: 39861 RVA: 0x00370826 File Offset: 0x0036EA26
		public void SetExposureState(string germ_id, GermExposureMonitor.ExposureState exposure_state)
		{
			this.exposureStates[germ_id] = exposure_state;
			this.RefreshStatusItems();
		}

		// Token: 0x06009BB6 RID: 39862 RVA: 0x0037083B File Offset: 0x0036EA3B
		public void SetExposureTier(string germ_id, float tier)
		{
			tier = Mathf.Clamp(tier, 0f, 3f);
			this.exposureTiers[germ_id] = tier;
			this.RefreshStatusItems();
		}

		// Token: 0x06009BB7 RID: 39863 RVA: 0x00370862 File Offset: 0x0036EA62
		public void ContractGerms(string germ_id)
		{
			DebugUtil.DevAssert(this.GetExposureState(germ_id) == GermExposureMonitor.ExposureState.Exposed, "Duplicant is contracting a sickness but was never exposed to it!", null);
			this.SetExposureState(germ_id, GermExposureMonitor.ExposureState.Contracted);
		}

		// Token: 0x06009BB8 RID: 39864 RVA: 0x00370884 File Offset: 0x0036EA84
		public void OnSicknessAdded(object sickness_instance_data)
		{
			SicknessInstance sicknessInstance = (SicknessInstance)sickness_instance_data;
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				if (exposureType.sickness_id == sicknessInstance.Sickness.Id)
				{
					this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.Sick);
				}
			}
		}

		// Token: 0x06009BB9 RID: 39865 RVA: 0x003708D8 File Offset: 0x0036EAD8
		public void OnSicknessCured(object sickness_instance_data)
		{
			SicknessInstance sicknessInstance = (SicknessInstance)sickness_instance_data;
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				if (exposureType.sickness_id == sicknessInstance.Sickness.Id)
				{
					this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.None);
				}
			}
		}

		// Token: 0x06009BBA RID: 39866 RVA: 0x0037092C File Offset: 0x0036EB2C
		private bool IsExposureValidForTraits(ExposureType exposure_type)
		{
			if (exposure_type.required_traits != null && exposure_type.required_traits.Count > 0)
			{
				foreach (string trait_id in exposure_type.required_traits)
				{
					if (!this.traits.HasTrait(trait_id))
					{
						return false;
					}
				}
			}
			if (exposure_type.excluded_traits != null && exposure_type.excluded_traits.Count > 0)
			{
				foreach (string trait_id2 in exposure_type.excluded_traits)
				{
					if (this.traits.HasTrait(trait_id2))
					{
						return false;
					}
				}
			}
			if (exposure_type.excluded_effects != null && exposure_type.excluded_effects.Count > 0)
			{
				Effects component = base.master.GetComponent<Effects>();
				foreach (string effect_id in exposure_type.excluded_effects)
				{
					if (component.HasEffect(effect_id))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06009BBB RID: 39867 RVA: 0x00370A78 File Offset: 0x0036EC78
		private bool HasMinExposurePeriodElapsed(string germ_id)
		{
			float num;
			this.lastExposureTime.TryGetValue(germ_id, out num);
			return num == 0f || GameClock.Instance.GetTime() - num > 540f;
		}

		// Token: 0x06009BBC RID: 39868 RVA: 0x00370AB8 File Offset: 0x0036ECB8
		private void RefreshStatusItems()
		{
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				Guid guid;
				this.contactStatusItemHandles.TryGetValue(exposureType.germ_id, out guid);
				Guid guid2;
				this.statusItemHandles.TryGetValue(exposureType.germ_id, out guid2);
				GermExposureMonitor.ExposureState exposureState = this.GetExposureState(exposureType.germ_id);
				if (guid2 == Guid.Empty && (exposureState == GermExposureMonitor.ExposureState.Exposed || exposureState == GermExposureMonitor.ExposureState.Contracted) && !string.IsNullOrEmpty(exposureType.sickness_id))
				{
					guid2 = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExposedToGerms, new GermExposureMonitor.ExposureStatusData
					{
						exposure_type = exposureType,
						owner = this
					});
				}
				else if (guid2 != Guid.Empty && exposureState != GermExposureMonitor.ExposureState.Exposed && exposureState != GermExposureMonitor.ExposureState.Contracted)
				{
					guid2 = base.GetComponent<KSelectable>().RemoveStatusItem(guid2, false);
				}
				this.statusItemHandles[exposureType.germ_id] = guid2;
				if (guid == Guid.Empty && exposureState == GermExposureMonitor.ExposureState.Contact)
				{
					if (!string.IsNullOrEmpty(exposureType.sickness_id))
					{
						guid = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ContactWithGerms, new GermExposureMonitor.ExposureStatusData
						{
							exposure_type = exposureType,
							owner = this
						});
					}
				}
				else if (guid != Guid.Empty && exposureState != GermExposureMonitor.ExposureState.Contact)
				{
					guid = base.GetComponent<KSelectable>().RemoveStatusItem(guid, false);
				}
				this.contactStatusItemHandles[exposureType.germ_id] = guid;
			}
		}

		// Token: 0x06009BBD RID: 39869 RVA: 0x00370C2B File Offset: 0x0036EE2B
		private void OnNightTime(object data)
		{
			this.UpdateReports();
		}

		// Token: 0x06009BBE RID: 39870 RVA: 0x00370C34 File Offset: 0x0036EE34
		private void UpdateReports()
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.DiseaseStatus, (float)this.primaryElement.DiseaseCount, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.GERMS, "{0}", base.master.name), base.master.gameObject.GetProperName());
		}

		// Token: 0x06009BBF RID: 39871 RVA: 0x00370C88 File Offset: 0x0036EE88
		public void InfectImmediately(ExposureType exposure_type)
		{
			if (exposure_type.infection_effect != null)
			{
				base.master.GetComponent<Effects>().Add(exposure_type.infection_effect, true);
			}
			if (exposure_type.sickness_id != null)
			{
				string lastDiseaseSource = this.GetLastDiseaseSource(exposure_type.germ_id);
				SicknessExposureInfo exposure_info = new SicknessExposureInfo(exposure_type.sickness_id, lastDiseaseSource);
				this.sicknesses.Infect(exposure_info);
			}
		}

		// Token: 0x06009BC0 RID: 39872 RVA: 0x00370CE4 File Offset: 0x0036EEE4
		public void OnSleepFinished()
		{
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				if (!exposureType.infect_immediately && exposureType.sickness_id != null)
				{
					GermExposureMonitor.ExposureState exposureState = this.GetExposureState(exposureType.germ_id);
					if (exposureState == GermExposureMonitor.ExposureState.Exposed)
					{
						this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.None);
					}
					if (exposureState == GermExposureMonitor.ExposureState.Contracted)
					{
						this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.Sick);
						string lastDiseaseSource = this.GetLastDiseaseSource(exposureType.germ_id);
						SicknessExposureInfo exposure_info = new SicknessExposureInfo(exposureType.sickness_id, lastDiseaseSource);
						this.sicknesses.Infect(exposure_info);
					}
					this.SetExposureTier(exposureType.germ_id, 0f);
				}
			}
		}

		// Token: 0x06009BC1 RID: 39873 RVA: 0x00370D84 File Offset: 0x0036EF84
		public string GetLastDiseaseSource(string id)
		{
			GermExposureMonitor.Instance.DiseaseSourceInfo diseaseSourceInfo;
			string result;
			if (this.lastDiseaseSources.TryGetValue(id, out diseaseSourceInfo))
			{
				switch (diseaseSourceInfo.vector)
				{
				case Sickness.InfectionVector.Contact:
					result = DUPLICANTS.DISEASES.INFECTIONSOURCES.SKIN;
					break;
				case Sickness.InfectionVector.Digestion:
					result = string.Format(DUPLICANTS.DISEASES.INFECTIONSOURCES.FOOD, diseaseSourceInfo.sourceObject.ProperName());
					break;
				case Sickness.InfectionVector.Inhalation:
					result = string.Format(DUPLICANTS.DISEASES.INFECTIONSOURCES.AIR, diseaseSourceInfo.sourceObject.ProperName());
					break;
				default:
					result = DUPLICANTS.DISEASES.INFECTIONSOURCES.UNKNOWN;
					break;
				}
			}
			else
			{
				result = DUPLICANTS.DISEASES.INFECTIONSOURCES.UNKNOWN;
			}
			return result;
		}

		// Token: 0x06009BC2 RID: 39874 RVA: 0x00370E20 File Offset: 0x0036F020
		public Vector3 GetLastExposurePosition(string germ_id)
		{
			GermExposureMonitor.Instance.DiseaseSourceInfo diseaseSourceInfo;
			if (this.lastDiseaseSources.TryGetValue(germ_id, out diseaseSourceInfo))
			{
				return diseaseSourceInfo.position;
			}
			return base.transform.GetPosition();
		}

		// Token: 0x06009BC3 RID: 39875 RVA: 0x00370E54 File Offset: 0x0036F054
		public float GetExposureWeight(string id)
		{
			float exposureTier = this.GetExposureTier(id);
			GermExposureMonitor.Instance.DiseaseSourceInfo diseaseSourceInfo;
			if (this.lastDiseaseSources.TryGetValue(id, out diseaseSourceInfo))
			{
				return diseaseSourceInfo.factor * exposureTier;
			}
			return 0f;
		}

		// Token: 0x040078D3 RID: 30931
		[Serialize]
		public Dictionary<HashedString, GermExposureMonitor.Instance.DiseaseSourceInfo> lastDiseaseSources;

		// Token: 0x040078D4 RID: 30932
		[Serialize]
		public Dictionary<HashedString, float> lastExposureTime;

		// Token: 0x040078D5 RID: 30933
		private Dictionary<HashedString, GermExposureMonitor.Instance.InhaleTickInfo> inhaleExposureTick;

		// Token: 0x040078D6 RID: 30934
		private Sicknesses sicknesses;

		// Token: 0x040078D7 RID: 30935
		private PrimaryElement primaryElement;

		// Token: 0x040078D8 RID: 30936
		private Traits traits;

		// Token: 0x040078D9 RID: 30937
		[Serialize]
		private Dictionary<string, GermExposureMonitor.ExposureState> exposureStates = new Dictionary<string, GermExposureMonitor.ExposureState>();

		// Token: 0x040078DA RID: 30938
		[Serialize]
		private Dictionary<string, float> exposureTiers = new Dictionary<string, float>();

		// Token: 0x040078DB RID: 30939
		private Dictionary<string, Guid> statusItemHandles = new Dictionary<string, Guid>();

		// Token: 0x040078DC RID: 30940
		private Dictionary<string, Guid> contactStatusItemHandles = new Dictionary<string, Guid>();

		// Token: 0x020025C0 RID: 9664
		[Serializable]
		public class DiseaseSourceInfo
		{
			// Token: 0x0600C00C RID: 49164 RVA: 0x003DBE4A File Offset: 0x003DA04A
			public DiseaseSourceInfo(Tag sourceObject, Sickness.InfectionVector vector, float factor, Vector3 position)
			{
				this.sourceObject = sourceObject;
				this.vector = vector;
				this.factor = factor;
				this.position = position;
			}

			// Token: 0x0400A81B RID: 43035
			public Tag sourceObject;

			// Token: 0x0400A81C RID: 43036
			public Sickness.InfectionVector vector;

			// Token: 0x0400A81D RID: 43037
			public float factor;

			// Token: 0x0400A81E RID: 43038
			public Vector3 position;
		}

		// Token: 0x020025C1 RID: 9665
		public class InhaleTickInfo
		{
			// Token: 0x0400A81F RID: 43039
			public bool inhaled;

			// Token: 0x0400A820 RID: 43040
			public int ticks;
		}
	}
}
