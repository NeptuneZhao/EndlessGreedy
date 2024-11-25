using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F4F RID: 3919
	[SerializationConfig(MemberSerialization.OptIn)]
	public class SicknessInstance : ModifierInstance<Sickness>, ISaveLoadable
	{
		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x0600788A RID: 30858 RVA: 0x002FAEA0 File Offset: 0x002F90A0
		public Sickness Sickness
		{
			get
			{
				return this.modifier;
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x0600788B RID: 30859 RVA: 0x002FAEA8 File Offset: 0x002F90A8
		public float TotalCureSpeedMultiplier
		{
			get
			{
				AttributeInstance attributeInstance = Db.Get().Attributes.DiseaseCureSpeed.Lookup(this.smi.master.gameObject);
				AttributeInstance attributeInstance2 = this.modifier.cureSpeedBase.Lookup(this.smi.master.gameObject);
				float num = 1f;
				if (attributeInstance != null)
				{
					num *= attributeInstance.GetTotalValue();
				}
				if (attributeInstance2 != null)
				{
					num *= attributeInstance2.GetTotalValue();
				}
				return num;
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x0600788C RID: 30860 RVA: 0x002FAF1C File Offset: 0x002F911C
		public bool IsDoctored
		{
			get
			{
				if (base.gameObject == null)
				{
					return false;
				}
				AttributeInstance attributeInstance = Db.Get().Attributes.DoctoredLevel.Lookup(base.gameObject);
				return attributeInstance != null && attributeInstance.GetTotalValue() > 0f;
			}
		}

		// Token: 0x0600788D RID: 30861 RVA: 0x002FAF67 File Offset: 0x002F9167
		public SicknessInstance(GameObject game_object, Sickness disease) : base(game_object, disease)
		{
		}

		// Token: 0x0600788E RID: 30862 RVA: 0x002FAF71 File Offset: 0x002F9171
		[OnDeserialized]
		private void OnDeserialized()
		{
			this.InitializeAndStart();
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x0600788F RID: 30863 RVA: 0x002FAF79 File Offset: 0x002F9179
		// (set) Token: 0x06007890 RID: 30864 RVA: 0x002FAF81 File Offset: 0x002F9181
		public SicknessExposureInfo ExposureInfo
		{
			get
			{
				return this.exposureInfo;
			}
			set
			{
				this.exposureInfo = value;
				this.InitializeAndStart();
			}
		}

		// Token: 0x06007891 RID: 30865 RVA: 0x002FAF90 File Offset: 0x002F9190
		private void InitializeAndStart()
		{
			Sickness disease = this.modifier;
			Func<List<Notification>, object, string> tooltip = delegate(List<Notification> notificationList, object data)
			{
				string text = "";
				for (int i = 0; i < notificationList.Count; i++)
				{
					Notification notification = notificationList[i];
					string arg = (string)notification.tooltipData;
					text += string.Format(DUPLICANTS.DISEASES.NOTIFICATION_TOOLTIP, notification.NotifierName, disease.Name, arg);
					if (i < notificationList.Count - 1)
					{
						text += "\n";
					}
				}
				return text;
			};
			string name = disease.Name;
			string title = name;
			NotificationType type = (disease.severity <= Sickness.Severity.Minor) ? NotificationType.BadMinor : NotificationType.Bad;
			object sourceInfo = this.exposureInfo.sourceInfo;
			this.notification = new Notification(title, type, tooltip, sourceInfo, true, 0f, null, null, null, true, false, false);
			this.statusItem = new StatusItem(disease.Id, disease.Name, DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.TEMPLATE, "", (disease.severity <= Sickness.Severity.Minor) ? StatusItem.IconType.Info : StatusItem.IconType.Exclamation, (disease.severity <= Sickness.Severity.Minor) ? NotificationType.BadMinor : NotificationType.Bad, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItem.resolveTooltipCallback = new Func<string, object, string>(this.ResolveString);
			if (this.smi != null)
			{
				this.smi.StopSM("refresh");
			}
			this.smi = new SicknessInstance.StatesInstance(this);
			this.smi.StartSM();
		}

		// Token: 0x06007892 RID: 30866 RVA: 0x002FB0A8 File Offset: 0x002F92A8
		private string ResolveString(string str, object data)
		{
			if (this.smi == null)
			{
				global::Debug.LogWarning("Attempting to resolve string when smi is null");
				return str;
			}
			KSelectable component = base.gameObject.GetComponent<KSelectable>();
			str = str.Replace("{Descriptor}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DESCRIPTOR, Strings.Get("STRINGS.DUPLICANTS.DISEASES.SEVERITY." + this.modifier.severity.ToString().ToUpper()), Strings.Get("STRINGS.DUPLICANTS.DISEASES.TYPE." + this.modifier.sicknessType.ToString().ToUpper())));
			str = str.Replace("{Infectee}", component.GetProperName());
			str = str.Replace("{InfectionSource}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.INFECTION_SOURCE, this.exposureInfo.sourceInfo));
			if (this.modifier.severity <= Sickness.Severity.Minor)
			{
				str = str.Replace("{Duration}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DURATION, GameUtil.GetFormattedCycles(this.GetInfectedTimeRemaining(), "F1", false)));
			}
			else if (this.modifier.severity == Sickness.Severity.Major)
			{
				str = str.Replace("{Duration}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DURATION, GameUtil.GetFormattedCycles(this.GetInfectedTimeRemaining(), "F1", false)));
				if (!this.IsDoctored)
				{
					str = str.Replace("{Doctor}", DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.BEDREST);
				}
				else
				{
					str = str.Replace("{Doctor}", DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DOCTORED);
				}
			}
			else if (this.modifier.severity >= Sickness.Severity.Critical)
			{
				if (!this.IsDoctored)
				{
					str = str.Replace("{Duration}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.FATALITY, GameUtil.GetFormattedCycles(this.GetFatalityTimeRemaining(), "F1", false)));
					str = str.Replace("{Doctor}", DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DOCTOR_REQUIRED);
				}
				else
				{
					str = str.Replace("{Duration}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DURATION, GameUtil.GetFormattedCycles(this.GetInfectedTimeRemaining(), "F1", false)));
					str = str.Replace("{Doctor}", DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DOCTORED);
				}
			}
			List<Descriptor> symptoms = this.modifier.GetSymptoms();
			string text = "";
			foreach (Descriptor descriptor in symptoms)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "\n";
				}
				text = text + "    • " + descriptor.text;
			}
			str = str.Replace("{Symptoms}", text);
			str = Regex.Replace(str, "{[^}]*}", "");
			return str;
		}

		// Token: 0x06007893 RID: 30867 RVA: 0x002FB370 File Offset: 0x002F9570
		public float GetInfectedTimeRemaining()
		{
			return this.modifier.SicknessDuration * (1f - this.smi.sm.percentRecovered.Get(this.smi)) / this.TotalCureSpeedMultiplier;
		}

		// Token: 0x06007894 RID: 30868 RVA: 0x002FB3A6 File Offset: 0x002F95A6
		public float GetFatalityTimeRemaining()
		{
			return this.modifier.fatalityDuration * (1f - this.smi.sm.percentDied.Get(this.smi));
		}

		// Token: 0x06007895 RID: 30869 RVA: 0x002FB3D5 File Offset: 0x002F95D5
		public float GetPercentCured()
		{
			if (this.smi == null)
			{
				return 0f;
			}
			return this.smi.sm.percentRecovered.Get(this.smi);
		}

		// Token: 0x06007896 RID: 30870 RVA: 0x002FB400 File Offset: 0x002F9600
		public void SetPercentCured(float pct)
		{
			this.smi.sm.percentRecovered.Set(pct, this.smi, false);
		}

		// Token: 0x06007897 RID: 30871 RVA: 0x002FB420 File Offset: 0x002F9620
		public void Cure()
		{
			this.smi.Cure();
		}

		// Token: 0x06007898 RID: 30872 RVA: 0x002FB42D File Offset: 0x002F962D
		public override void OnCleanUp()
		{
			if (this.smi != null)
			{
				this.smi.StopSM("DiseaseInstance.OnCleanUp");
				this.smi = null;
			}
		}

		// Token: 0x06007899 RID: 30873 RVA: 0x002FB44E File Offset: 0x002F964E
		public StatusItem GetStatusItem()
		{
			return this.statusItem;
		}

		// Token: 0x0600789A RID: 30874 RVA: 0x002FB456 File Offset: 0x002F9656
		public List<Descriptor> GetDescriptors()
		{
			return this.modifier.GetSicknessSourceDescriptors();
		}

		// Token: 0x04005A17 RID: 23063
		[Serialize]
		private SicknessExposureInfo exposureInfo;

		// Token: 0x04005A18 RID: 23064
		private SicknessInstance.StatesInstance smi;

		// Token: 0x04005A19 RID: 23065
		private StatusItem statusItem;

		// Token: 0x04005A1A RID: 23066
		private Notification notification;

		// Token: 0x0200233C RID: 9020
		private struct CureInfo
		{
			// Token: 0x04009E23 RID: 40483
			public string name;

			// Token: 0x04009E24 RID: 40484
			public float multiplier;
		}

		// Token: 0x0200233D RID: 9021
		public class StatesInstance : GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.GameInstance
		{
			// Token: 0x0600B5FE RID: 46590 RVA: 0x003C8DB2 File Offset: 0x003C6FB2
			public StatesInstance(SicknessInstance master) : base(master)
			{
			}

			// Token: 0x0600B5FF RID: 46591 RVA: 0x003C8DBC File Offset: 0x003C6FBC
			public void UpdateProgress(float dt)
			{
				float delta_value = dt * base.master.TotalCureSpeedMultiplier / base.master.modifier.SicknessDuration;
				base.sm.percentRecovered.Delta(delta_value, base.smi);
				if (base.master.modifier.fatalityDuration > 0f)
				{
					if (!base.master.IsDoctored)
					{
						float delta_value2 = dt / base.master.modifier.fatalityDuration;
						base.sm.percentDied.Delta(delta_value2, base.smi);
						return;
					}
					base.sm.percentDied.Set(0f, base.smi, false);
				}
			}

			// Token: 0x0600B600 RID: 46592 RVA: 0x003C8E70 File Offset: 0x003C7070
			public void Infect()
			{
				Sickness modifier = base.master.modifier;
				this.componentData = modifier.Infect(base.gameObject, base.master, base.master.exposureInfo);
				if (PopFXManager.Instance != null)
				{
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, string.Format(DUPLICANTS.DISEASES.INFECTED_POPUP, modifier.Name), base.gameObject.transform, 1.5f, true);
				}
			}

			// Token: 0x0600B601 RID: 46593 RVA: 0x003C8EF4 File Offset: 0x003C70F4
			public void Cure()
			{
				Sickness modifier = base.master.modifier;
				base.gameObject.GetComponent<Modifiers>().sicknesses.Cure(modifier);
				modifier.Cure(base.gameObject, this.componentData);
				if (PopFXManager.Instance != null)
				{
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, string.Format(DUPLICANTS.DISEASES.CURED_POPUP, modifier.Name), base.gameObject.transform, 1.5f, true);
				}
				if (!string.IsNullOrEmpty(modifier.recoveryEffect))
				{
					Effects component = base.gameObject.GetComponent<Effects>();
					if (component)
					{
						component.Add(modifier.recoveryEffect, true);
					}
				}
			}

			// Token: 0x0600B602 RID: 46594 RVA: 0x003C8FAD File Offset: 0x003C71AD
			public SicknessExposureInfo GetExposureInfo()
			{
				return base.master.ExposureInfo;
			}

			// Token: 0x04009E25 RID: 40485
			private object[] componentData;
		}

		// Token: 0x0200233E RID: 9022
		public class States : GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance>
		{
			// Token: 0x0600B603 RID: 46595 RVA: 0x003C8FBC File Offset: 0x003C71BC
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.infected;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.infected.Enter("Infect", delegate(SicknessInstance.StatesInstance smi)
				{
					smi.Infect();
				}).DoNotification((SicknessInstance.StatesInstance smi) => smi.master.notification).Update("UpdateProgress", delegate(SicknessInstance.StatesInstance smi, float dt)
				{
					smi.UpdateProgress(dt);
				}, UpdateRate.SIM_200ms, false).ToggleStatusItem((SicknessInstance.StatesInstance smi) => smi.master.GetStatusItem(), (SicknessInstance.StatesInstance smi) => smi).ParamTransition<float>(this.percentRecovered, this.cured, GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.IsGTOne).ParamTransition<float>(this.percentDied, this.fatality_pre, GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.IsGTOne);
				this.cured.Enter("Cure", delegate(SicknessInstance.StatesInstance smi)
				{
					smi.master.Cure();
				});
				this.fatality_pre.Update("DeathByDisease", delegate(SicknessInstance.StatesInstance smi, float dt)
				{
					DeathMonitor.Instance smi2 = smi.master.gameObject.GetSMI<DeathMonitor.Instance>();
					if (smi2 != null)
					{
						smi2.Kill(Db.Get().Deaths.FatalDisease);
						smi.GoTo(this.fatality);
					}
				}, UpdateRate.SIM_200ms, false);
				this.fatality.DoNothing();
			}

			// Token: 0x04009E26 RID: 40486
			public StateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.FloatParameter percentRecovered;

			// Token: 0x04009E27 RID: 40487
			public StateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.FloatParameter percentDied;

			// Token: 0x04009E28 RID: 40488
			public GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State infected;

			// Token: 0x04009E29 RID: 40489
			public GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State cured;

			// Token: 0x04009E2A RID: 40490
			public GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State fatality_pre;

			// Token: 0x04009E2B RID: 40491
			public GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State fatality;
		}
	}
}
