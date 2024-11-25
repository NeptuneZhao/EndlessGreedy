using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000813 RID: 2067
public class IrrigationMonitor : GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>
{
	// Token: 0x06003935 RID: 14645 RVA: 0x00137DD8 File Offset: 0x00135FD8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.wild;
		base.serializable = StateMachine.SerializeType.Never;
		this.wild.ParamTransition<GameObject>(this.resourceStorage, this.unfertilizable, (IrrigationMonitor.Instance smi, GameObject p) => p != null);
		this.unfertilizable.Enter(delegate(IrrigationMonitor.Instance smi)
		{
			if (smi.AcceptsLiquid())
			{
				smi.GoTo(this.replanted.irrigated);
			}
		});
		this.replanted.Enter(delegate(IrrigationMonitor.Instance smi)
		{
			ManualDeliveryKG[] components = smi.gameObject.GetComponents<ManualDeliveryKG>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].Pause(false, "replanted");
			}
			smi.UpdateIrrigation(0.033333335f);
		}).Target(this.resourceStorage).EventHandler(GameHashes.OnStorageChange, delegate(IrrigationMonitor.Instance smi)
		{
			smi.UpdateIrrigation(0.2f);
		}).Target(this.masterTarget);
		this.replanted.irrigated.DefaultState(this.replanted.irrigated.absorbing).TriggerOnEnter(this.ResourceRecievedEvent, null);
		this.replanted.irrigated.absorbing.DefaultState(this.replanted.irrigated.absorbing.normal).ParamTransition<bool>(this.hasCorrectLiquid, this.replanted.starved, GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.IsFalse).ToggleAttributeModifier("Absorbing", (IrrigationMonitor.Instance smi) => smi.absorptionRate, null).Enter(delegate(IrrigationMonitor.Instance smi)
		{
			smi.UpdateAbsorbing(true);
		}).EventHandler(GameHashes.TagsChanged, delegate(IrrigationMonitor.Instance smi)
		{
			smi.UpdateAbsorbing(true);
		}).Exit(delegate(IrrigationMonitor.Instance smi)
		{
			smi.UpdateAbsorbing(false);
		});
		this.replanted.irrigated.absorbing.normal.ParamTransition<bool>(this.hasIncorrectLiquid, this.replanted.irrigated.absorbing.wrongLiquid, GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.IsTrue);
		this.replanted.irrigated.absorbing.wrongLiquid.ParamTransition<bool>(this.hasIncorrectLiquid, this.replanted.irrigated.absorbing.normal, GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.IsFalse);
		this.replanted.starved.DefaultState(this.replanted.starved.normal).TriggerOnEnter(this.ResourceDepletedEvent, null).ParamTransition<bool>(this.enoughCorrectLiquidToRecover, this.replanted.irrigated.absorbing, (IrrigationMonitor.Instance smi, bool p) => p && this.hasCorrectLiquid.Get(smi)).ParamTransition<bool>(this.hasCorrectLiquid, this.replanted.irrigated.absorbing, (IrrigationMonitor.Instance smi, bool p) => p && this.enoughCorrectLiquidToRecover.Get(smi));
		this.replanted.starved.normal.ParamTransition<bool>(this.hasIncorrectLiquid, this.replanted.starved.wrongLiquid, GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.IsTrue);
		this.replanted.starved.wrongLiquid.ParamTransition<bool>(this.hasIncorrectLiquid, this.replanted.starved.normal, GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.IsFalse);
	}

	// Token: 0x04002269 RID: 8809
	public StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.TargetParameter resourceStorage;

	// Token: 0x0400226A RID: 8810
	public StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.BoolParameter hasCorrectLiquid;

	// Token: 0x0400226B RID: 8811
	public StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.BoolParameter hasIncorrectLiquid;

	// Token: 0x0400226C RID: 8812
	public StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.BoolParameter enoughCorrectLiquidToRecover;

	// Token: 0x0400226D RID: 8813
	public GameHashes ResourceRecievedEvent = GameHashes.LiquidResourceRecieved;

	// Token: 0x0400226E RID: 8814
	public GameHashes ResourceDepletedEvent = GameHashes.LiquidResourceEmpty;

	// Token: 0x0400226F RID: 8815
	public GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State wild;

	// Token: 0x04002270 RID: 8816
	public GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State unfertilizable;

	// Token: 0x04002271 RID: 8817
	public IrrigationMonitor.ReplantedStates replanted;

	// Token: 0x02001716 RID: 5910
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06009494 RID: 38036 RVA: 0x0035CEC4 File Offset: 0x0035B0C4
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			if (this.consumedElements.Length != 0)
			{
				List<Descriptor> list = new List<Descriptor>();
				float preModifiedAttributeValue = obj.GetComponent<Modifiers>().GetPreModifiedAttributeValue(Db.Get().PlantAttributes.FertilizerUsageMod);
				foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in this.consumedElements)
				{
					float num = consumeInfo.massConsumptionRate * preModifiedAttributeValue;
					list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.IDEAL_FERTILIZER, consumeInfo.tag.ProperName(), GameUtil.GetFormattedMass(-num, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.IDEAL_FERTILIZER, consumeInfo.tag.ProperName(), GameUtil.GetFormattedMass(num, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
				}
				return list;
			}
			return null;
		}

		// Token: 0x040071AA RID: 29098
		public Tag wrongIrrigationTestTag;

		// Token: 0x040071AB RID: 29099
		public PlantElementAbsorber.ConsumeInfo[] consumedElements;
	}

	// Token: 0x02001717 RID: 5911
	public class VariableIrrigationStates : GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State
	{
		// Token: 0x040071AC RID: 29100
		public GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State normal;

		// Token: 0x040071AD RID: 29101
		public GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State wrongLiquid;
	}

	// Token: 0x02001718 RID: 5912
	public class Irrigated : GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State
	{
		// Token: 0x040071AE RID: 29102
		public IrrigationMonitor.VariableIrrigationStates absorbing;
	}

	// Token: 0x02001719 RID: 5913
	public class ReplantedStates : GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State
	{
		// Token: 0x040071AF RID: 29103
		public IrrigationMonitor.Irrigated irrigated;

		// Token: 0x040071B0 RID: 29104
		public IrrigationMonitor.VariableIrrigationStates starved;
	}

	// Token: 0x0200171A RID: 5914
	public new class Instance : GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.GameInstance, IWiltCause
	{
		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x06009499 RID: 38041 RVA: 0x0035CFAC File Offset: 0x0035B1AC
		public float total_fertilizer_available
		{
			get
			{
				return this.total_available_mass;
			}
		}

		// Token: 0x0600949A RID: 38042 RVA: 0x0035CFB4 File Offset: 0x0035B1B4
		public Instance(IStateMachineTarget master, IrrigationMonitor.Def def) : base(master, def)
		{
			this.AddAmounts(base.gameObject);
			this.MakeModifiers();
			master.Subscribe(1309017699, new Action<object>(this.SetStorage));
		}

		// Token: 0x0600949B RID: 38043 RVA: 0x0035CFF3 File Offset: 0x0035B1F3
		public virtual StatusItem GetStarvedStatusItem()
		{
			return Db.Get().CreatureStatusItems.NeedsIrrigation;
		}

		// Token: 0x0600949C RID: 38044 RVA: 0x0035D004 File Offset: 0x0035B204
		public virtual StatusItem GetIncorrectLiquidStatusItem()
		{
			return Db.Get().CreatureStatusItems.WrongIrrigation;
		}

		// Token: 0x0600949D RID: 38045 RVA: 0x0035D015 File Offset: 0x0035B215
		public virtual StatusItem GetIncorrectLiquidStatusItemMajor()
		{
			return Db.Get().CreatureStatusItems.WrongIrrigationMajor;
		}

		// Token: 0x0600949E RID: 38046 RVA: 0x0035D028 File Offset: 0x0035B228
		protected virtual void AddAmounts(GameObject gameObject)
		{
			Amounts amounts = gameObject.GetAmounts();
			this.irrigation = amounts.Add(new AmountInstance(Db.Get().Amounts.Irrigation, gameObject));
		}

		// Token: 0x0600949F RID: 38047 RVA: 0x0035D060 File Offset: 0x0035B260
		protected virtual void MakeModifiers()
		{
			this.consumptionRate = new AttributeModifier(Db.Get().Amounts.Irrigation.deltaAttribute.Id, -0.16666667f, CREATURES.STATS.IRRIGATION.CONSUME_MODIFIER, false, false, true);
			this.absorptionRate = new AttributeModifier(Db.Get().Amounts.Irrigation.deltaAttribute.Id, 1.6666666f, CREATURES.STATS.IRRIGATION.ABSORBING_MODIFIER, false, false, true);
		}

		// Token: 0x060094A0 RID: 38048 RVA: 0x0035D0DC File Offset: 0x0035B2DC
		public static void DumpIncorrectFertilizers(Storage storage, GameObject go)
		{
			if (storage == null)
			{
				return;
			}
			if (go == null)
			{
				return;
			}
			IrrigationMonitor.Instance smi = go.GetSMI<IrrigationMonitor.Instance>();
			PlantElementAbsorber.ConsumeInfo[] consumed_infos = null;
			if (smi != null)
			{
				consumed_infos = smi.def.consumedElements;
			}
			IrrigationMonitor.Instance.DumpIncorrectFertilizers(storage, consumed_infos, false);
			FertilizationMonitor.Instance smi2 = go.GetSMI<FertilizationMonitor.Instance>();
			PlantElementAbsorber.ConsumeInfo[] consumed_infos2 = null;
			if (smi2 != null)
			{
				consumed_infos2 = smi2.def.consumedElements;
			}
			IrrigationMonitor.Instance.DumpIncorrectFertilizers(storage, consumed_infos2, true);
		}

		// Token: 0x060094A1 RID: 38049 RVA: 0x0035D140 File Offset: 0x0035B340
		private static void DumpIncorrectFertilizers(Storage storage, PlantElementAbsorber.ConsumeInfo[] consumed_infos, bool validate_solids)
		{
			if (storage == null)
			{
				return;
			}
			for (int i = storage.items.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = storage.items[i];
				if (!(gameObject == null))
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					if (!(component == null) && !(gameObject.GetComponent<ElementChunk>() == null))
					{
						if (validate_solids)
						{
							if (!component.Element.IsSolid)
							{
								goto IL_C1;
							}
						}
						else if (!component.Element.IsLiquid)
						{
							goto IL_C1;
						}
						bool flag = false;
						KPrefabID component2 = component.GetComponent<KPrefabID>();
						if (consumed_infos != null)
						{
							foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in consumed_infos)
							{
								if (component2.HasTag(consumeInfo.tag))
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							storage.Drop(gameObject, true);
						}
					}
				}
				IL_C1:;
			}
		}

		// Token: 0x060094A2 RID: 38050 RVA: 0x0035D21C File Offset: 0x0035B41C
		public void SetStorage(object obj)
		{
			this.storage = (Storage)obj;
			base.sm.resourceStorage.Set(this.storage, base.smi);
			IrrigationMonitor.Instance.DumpIncorrectFertilizers(this.storage, base.smi.gameObject);
			foreach (ManualDeliveryKG manualDeliveryKG in base.smi.gameObject.GetComponents<ManualDeliveryKG>())
			{
				bool flag = false;
				foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in base.def.consumedElements)
				{
					if (manualDeliveryKG.RequestedItemTag == consumeInfo.tag)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					manualDeliveryKG.SetStorage(this.storage);
					manualDeliveryKG.enabled = !this.storage.gameObject.GetComponent<PlantablePlot>().has_liquid_pipe_input;
				}
			}
		}

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x060094A3 RID: 38051 RVA: 0x0035D2FC File Offset: 0x0035B4FC
		public WiltCondition.Condition[] Conditions
		{
			get
			{
				return new WiltCondition.Condition[]
				{
					WiltCondition.Condition.Irrigation
				};
			}
		}

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x060094A4 RID: 38052 RVA: 0x0035D308 File Offset: 0x0035B508
		public string WiltStateString
		{
			get
			{
				string result = "";
				if (base.smi.IsInsideState(base.smi.sm.replanted.irrigated.absorbing.wrongLiquid))
				{
					result = this.GetIncorrectLiquidStatusItem().resolveStringCallback(CREATURES.STATUSITEMS.WRONGIRRIGATION.NAME, this);
				}
				else if (base.smi.IsInsideState(base.smi.sm.replanted.starved.wrongLiquid))
				{
					result = this.GetIncorrectLiquidStatusItemMajor().resolveStringCallback(CREATURES.STATUSITEMS.WRONGIRRIGATIONMAJOR.NAME, this);
				}
				else if (base.smi.IsInsideState(base.smi.sm.replanted.starved))
				{
					result = this.GetStarvedStatusItem().resolveStringCallback(CREATURES.STATUSITEMS.NEEDSIRRIGATION.NAME, this);
				}
				return result;
			}
		}

		// Token: 0x060094A5 RID: 38053 RVA: 0x0035D3EC File Offset: 0x0035B5EC
		public virtual bool AcceptsLiquid()
		{
			PlantablePlot component = base.sm.resourceStorage.Get(this).GetComponent<PlantablePlot>();
			return component != null && component.AcceptsIrrigation;
		}

		// Token: 0x060094A6 RID: 38054 RVA: 0x0035D421 File Offset: 0x0035B621
		public bool Starved()
		{
			return this.irrigation.value == 0f;
		}

		// Token: 0x060094A7 RID: 38055 RVA: 0x0035D438 File Offset: 0x0035B638
		public void UpdateIrrigation(float dt)
		{
			if (base.def.consumedElements == null)
			{
				return;
			}
			Storage storage = base.sm.resourceStorage.Get<Storage>(base.smi);
			bool flag = true;
			bool value = false;
			bool flag2 = true;
			if (storage != null)
			{
				List<GameObject> items = storage.items;
				for (int i = 0; i < base.def.consumedElements.Length; i++)
				{
					float num = 0f;
					PlantElementAbsorber.ConsumeInfo consumeInfo = base.def.consumedElements[i];
					for (int j = 0; j < items.Count; j++)
					{
						GameObject gameObject = items[j];
						if (gameObject.HasTag(consumeInfo.tag))
						{
							num += gameObject.GetComponent<PrimaryElement>().Mass;
						}
						else if (gameObject.HasTag(base.def.wrongIrrigationTestTag))
						{
							value = true;
						}
					}
					this.total_available_mass = num;
					float totalValue = base.gameObject.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
					if (num < consumeInfo.massConsumptionRate * totalValue * dt)
					{
						flag = false;
						break;
					}
					if (num < consumeInfo.massConsumptionRate * totalValue * (dt * 30f))
					{
						flag2 = false;
						break;
					}
				}
			}
			else
			{
				flag = false;
				flag2 = false;
				value = false;
			}
			base.sm.hasCorrectLiquid.Set(flag, base.smi, false);
			base.sm.hasIncorrectLiquid.Set(value, base.smi, false);
			base.sm.enoughCorrectLiquidToRecover.Set(flag2 && flag, base.smi, false);
		}

		// Token: 0x060094A8 RID: 38056 RVA: 0x0035D5CC File Offset: 0x0035B7CC
		public void UpdateAbsorbing(bool allow)
		{
			bool flag = allow && !base.smi.gameObject.HasTag(GameTags.Wilting);
			if (flag != this.absorberHandle.IsValid())
			{
				if (flag)
				{
					if (base.def.consumedElements == null || base.def.consumedElements.Length == 0)
					{
						return;
					}
					float totalValue = base.gameObject.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
					PlantElementAbsorber.ConsumeInfo[] array = new PlantElementAbsorber.ConsumeInfo[base.def.consumedElements.Length];
					for (int i = 0; i < base.def.consumedElements.Length; i++)
					{
						PlantElementAbsorber.ConsumeInfo consumeInfo = base.def.consumedElements[i];
						consumeInfo.massConsumptionRate *= totalValue;
						array[i] = consumeInfo;
					}
					this.absorberHandle = Game.Instance.plantElementAbsorbers.Add(this.storage, array);
					return;
				}
				else
				{
					this.absorberHandle = Game.Instance.plantElementAbsorbers.Remove(this.absorberHandle);
				}
			}
		}

		// Token: 0x040071B1 RID: 29105
		public AttributeModifier consumptionRate;

		// Token: 0x040071B2 RID: 29106
		public AttributeModifier absorptionRate;

		// Token: 0x040071B3 RID: 29107
		protected AmountInstance irrigation;

		// Token: 0x040071B4 RID: 29108
		private float total_available_mass;

		// Token: 0x040071B5 RID: 29109
		private Storage storage;

		// Token: 0x040071B6 RID: 29110
		private HandleVector<int>.Handle absorberHandle = HandleVector<int>.InvalidHandle;
	}
}
