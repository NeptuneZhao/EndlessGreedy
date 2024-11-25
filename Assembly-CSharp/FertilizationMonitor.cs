using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000809 RID: 2057
public class FertilizationMonitor : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>
{
	// Token: 0x060038D9 RID: 14553 RVA: 0x001361E4 File Offset: 0x001343E4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.wild;
		base.serializable = StateMachine.SerializeType.Never;
		this.wild.ParamTransition<GameObject>(this.fertilizerStorage, this.unfertilizable, (FertilizationMonitor.Instance smi, GameObject p) => p != null);
		this.unfertilizable.Enter(delegate(FertilizationMonitor.Instance smi)
		{
			if (smi.AcceptsFertilizer())
			{
				smi.GoTo(this.replanted.fertilized);
			}
		});
		this.replanted.Enter(delegate(FertilizationMonitor.Instance smi)
		{
			ManualDeliveryKG[] components = smi.gameObject.GetComponents<ManualDeliveryKG>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].Pause(false, "replanted");
			}
			smi.UpdateFertilization(0.033333335f);
		}).Target(this.fertilizerStorage).EventHandler(GameHashes.OnStorageChange, delegate(FertilizationMonitor.Instance smi)
		{
			smi.UpdateFertilization(0.2f);
		}).Target(this.masterTarget);
		this.replanted.fertilized.DefaultState(this.replanted.fertilized.decaying).TriggerOnEnter(this.ResourceRecievedEvent, null);
		this.replanted.fertilized.decaying.DefaultState(this.replanted.fertilized.decaying.normal).ToggleAttributeModifier("Consuming", (FertilizationMonitor.Instance smi) => smi.consumptionRate, null).ParamTransition<bool>(this.hasCorrectFertilizer, this.replanted.fertilized.absorbing, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue).Update("Decaying", delegate(FertilizationMonitor.Instance smi, float dt)
		{
			if (smi.Starved())
			{
				smi.GoTo(this.replanted.starved);
			}
		}, UpdateRate.SIM_200ms, false);
		this.replanted.fertilized.decaying.normal.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.fertilized.decaying.wrongFert, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue);
		this.replanted.fertilized.decaying.wrongFert.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.fertilized.decaying.normal, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsFalse);
		this.replanted.fertilized.absorbing.DefaultState(this.replanted.fertilized.absorbing.normal).ParamTransition<bool>(this.hasCorrectFertilizer, this.replanted.fertilized.decaying, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsFalse).ToggleAttributeModifier("Absorbing", (FertilizationMonitor.Instance smi) => smi.absorptionRate, null).Enter(delegate(FertilizationMonitor.Instance smi)
		{
			smi.StartAbsorbing();
		}).EventHandler(GameHashes.Wilt, delegate(FertilizationMonitor.Instance smi)
		{
			smi.StopAbsorbing();
		}).EventHandler(GameHashes.WiltRecover, delegate(FertilizationMonitor.Instance smi)
		{
			smi.StartAbsorbing();
		}).Exit(delegate(FertilizationMonitor.Instance smi)
		{
			smi.StopAbsorbing();
		});
		this.replanted.fertilized.absorbing.normal.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.fertilized.absorbing.wrongFert, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue);
		this.replanted.fertilized.absorbing.wrongFert.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.fertilized.absorbing.normal, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsFalse);
		this.replanted.starved.DefaultState(this.replanted.starved.normal).TriggerOnEnter(this.ResourceDepletedEvent, null).ParamTransition<bool>(this.hasCorrectFertilizer, this.replanted.fertilized, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue);
		this.replanted.starved.normal.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.starved.wrongFert, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue);
		this.replanted.starved.wrongFert.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.starved.normal, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsFalse);
	}

	// Token: 0x04002230 RID: 8752
	public StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.TargetParameter fertilizerStorage;

	// Token: 0x04002231 RID: 8753
	public StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.BoolParameter hasCorrectFertilizer;

	// Token: 0x04002232 RID: 8754
	public StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.BoolParameter hasIncorrectFertilizer;

	// Token: 0x04002233 RID: 8755
	public GameHashes ResourceRecievedEvent = GameHashes.Fertilized;

	// Token: 0x04002234 RID: 8756
	public GameHashes ResourceDepletedEvent = GameHashes.Unfertilized;

	// Token: 0x04002235 RID: 8757
	public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State wild;

	// Token: 0x04002236 RID: 8758
	public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State unfertilizable;

	// Token: 0x04002237 RID: 8759
	public FertilizationMonitor.ReplantedStates replanted;

	// Token: 0x020016FF RID: 5887
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06009443 RID: 37955 RVA: 0x0035BBA0 File Offset: 0x00359DA0
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

		// Token: 0x0400716A RID: 29034
		public Tag wrongFertilizerTestTag;

		// Token: 0x0400716B RID: 29035
		public PlantElementAbsorber.ConsumeInfo[] consumedElements;
	}

	// Token: 0x02001700 RID: 5888
	public class VariableFertilizerStates : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State
	{
		// Token: 0x0400716C RID: 29036
		public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State normal;

		// Token: 0x0400716D RID: 29037
		public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State wrongFert;
	}

	// Token: 0x02001701 RID: 5889
	public class FertilizedStates : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State
	{
		// Token: 0x0400716E RID: 29038
		public FertilizationMonitor.VariableFertilizerStates decaying;

		// Token: 0x0400716F RID: 29039
		public FertilizationMonitor.VariableFertilizerStates absorbing;

		// Token: 0x04007170 RID: 29040
		public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State wilting;
	}

	// Token: 0x02001702 RID: 5890
	public class ReplantedStates : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State
	{
		// Token: 0x04007171 RID: 29041
		public FertilizationMonitor.FertilizedStates fertilized;

		// Token: 0x04007172 RID: 29042
		public FertilizationMonitor.VariableFertilizerStates starved;
	}

	// Token: 0x02001703 RID: 5891
	public new class Instance : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.GameInstance, IWiltCause
	{
		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x06009448 RID: 37960 RVA: 0x0035BC88 File Offset: 0x00359E88
		public float total_fertilizer_available
		{
			get
			{
				return this.total_available_mass;
			}
		}

		// Token: 0x06009449 RID: 37961 RVA: 0x0035BC90 File Offset: 0x00359E90
		public Instance(IStateMachineTarget master, FertilizationMonitor.Def def) : base(master, def)
		{
			this.AddAmounts(base.gameObject);
			this.MakeModifiers();
			master.Subscribe(1309017699, new Action<object>(this.SetStorage));
		}

		// Token: 0x0600944A RID: 37962 RVA: 0x0035BCCF File Offset: 0x00359ECF
		public virtual StatusItem GetStarvedStatusItem()
		{
			return Db.Get().CreatureStatusItems.NeedsFertilizer;
		}

		// Token: 0x0600944B RID: 37963 RVA: 0x0035BCE0 File Offset: 0x00359EE0
		public virtual StatusItem GetIncorrectFertStatusItem()
		{
			return Db.Get().CreatureStatusItems.WrongFertilizer;
		}

		// Token: 0x0600944C RID: 37964 RVA: 0x0035BCF1 File Offset: 0x00359EF1
		public virtual StatusItem GetIncorrectFertStatusItemMajor()
		{
			return Db.Get().CreatureStatusItems.WrongFertilizerMajor;
		}

		// Token: 0x0600944D RID: 37965 RVA: 0x0035BD04 File Offset: 0x00359F04
		protected virtual void AddAmounts(GameObject gameObject)
		{
			Amounts amounts = gameObject.GetAmounts();
			this.fertilization = amounts.Add(new AmountInstance(Db.Get().Amounts.Fertilization, gameObject));
		}

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x0600944E RID: 37966 RVA: 0x0035BD39 File Offset: 0x00359F39
		public WiltCondition.Condition[] Conditions
		{
			get
			{
				return new WiltCondition.Condition[]
				{
					WiltCondition.Condition.Fertilized
				};
			}
		}

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x0600944F RID: 37967 RVA: 0x0035BD48 File Offset: 0x00359F48
		public string WiltStateString
		{
			get
			{
				string result = "";
				if (base.smi.IsInsideState(base.smi.sm.replanted.fertilized.decaying.wrongFert))
				{
					result = this.GetIncorrectFertStatusItemMajor().resolveStringCallback(CREATURES.STATUSITEMS.WRONGFERTILIZERMAJOR.NAME, this);
				}
				else if (base.smi.IsInsideState(base.smi.sm.replanted.fertilized.absorbing.wrongFert))
				{
					result = this.GetIncorrectFertStatusItem().resolveStringCallback(CREATURES.STATUSITEMS.WRONGFERTILIZER.NAME, this);
				}
				else if (base.smi.IsInsideState(base.smi.sm.replanted.starved))
				{
					result = this.GetStarvedStatusItem().resolveStringCallback(CREATURES.STATUSITEMS.NEEDSFERTILIZER.NAME, this);
				}
				else if (base.smi.IsInsideState(base.smi.sm.replanted.starved.wrongFert))
				{
					result = this.GetIncorrectFertStatusItemMajor().resolveStringCallback(CREATURES.STATUSITEMS.WRONGFERTILIZERMAJOR.NAME, this);
				}
				return result;
			}
		}

		// Token: 0x06009450 RID: 37968 RVA: 0x0035BE7C File Offset: 0x0035A07C
		protected virtual void MakeModifiers()
		{
			this.consumptionRate = new AttributeModifier(Db.Get().Amounts.Fertilization.deltaAttribute.Id, -0.16666667f, CREATURES.STATS.FERTILIZATION.CONSUME_MODIFIER, false, false, true);
			this.absorptionRate = new AttributeModifier(Db.Get().Amounts.Fertilization.deltaAttribute.Id, 1.6666666f, CREATURES.STATS.FERTILIZATION.ABSORBING_MODIFIER, false, false, true);
		}

		// Token: 0x06009451 RID: 37969 RVA: 0x0035BEF8 File Offset: 0x0035A0F8
		public void SetStorage(object obj)
		{
			this.storage = (Storage)obj;
			base.sm.fertilizerStorage.Set(this.storage, base.smi);
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
					manualDeliveryKG.enabled = true;
				}
			}
		}

		// Token: 0x06009452 RID: 37970 RVA: 0x0035BFC4 File Offset: 0x0035A1C4
		public virtual bool AcceptsFertilizer()
		{
			PlantablePlot component = base.sm.fertilizerStorage.Get(this).GetComponent<PlantablePlot>();
			return component != null && component.AcceptsFertilizer;
		}

		// Token: 0x06009453 RID: 37971 RVA: 0x0035BFF9 File Offset: 0x0035A1F9
		public bool Starved()
		{
			return this.fertilization.value == 0f;
		}

		// Token: 0x06009454 RID: 37972 RVA: 0x0035C010 File Offset: 0x0035A210
		public void UpdateFertilization(float dt)
		{
			if (base.def.consumedElements == null)
			{
				return;
			}
			if (this.storage == null)
			{
				return;
			}
			bool value = true;
			bool value2 = false;
			List<GameObject> items = this.storage.items;
			for (int i = 0; i < base.def.consumedElements.Length; i++)
			{
				PlantElementAbsorber.ConsumeInfo consumeInfo = base.def.consumedElements[i];
				float num = 0f;
				for (int j = 0; j < items.Count; j++)
				{
					GameObject gameObject = items[j];
					if (gameObject.HasTag(consumeInfo.tag))
					{
						num += gameObject.GetComponent<PrimaryElement>().Mass;
					}
					else if (gameObject.HasTag(base.def.wrongFertilizerTestTag))
					{
						value2 = true;
					}
				}
				this.total_available_mass = num;
				float totalValue = base.gameObject.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
				if (num < consumeInfo.massConsumptionRate * totalValue * dt)
				{
					value = false;
					break;
				}
			}
			base.sm.hasCorrectFertilizer.Set(value, base.smi, false);
			base.sm.hasIncorrectFertilizer.Set(value2, base.smi, false);
		}

		// Token: 0x06009455 RID: 37973 RVA: 0x0035C150 File Offset: 0x0035A350
		public void StartAbsorbing()
		{
			if (this.absorberHandle.IsValid())
			{
				return;
			}
			if (base.def.consumedElements == null || base.def.consumedElements.Length == 0)
			{
				return;
			}
			GameObject gameObject = base.smi.gameObject;
			float totalValue = base.gameObject.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
			PlantElementAbsorber.ConsumeInfo[] array = new PlantElementAbsorber.ConsumeInfo[base.def.consumedElements.Length];
			for (int i = 0; i < base.def.consumedElements.Length; i++)
			{
				PlantElementAbsorber.ConsumeInfo consumeInfo = base.def.consumedElements[i];
				consumeInfo.massConsumptionRate *= totalValue;
				array[i] = consumeInfo;
			}
			this.absorberHandle = Game.Instance.plantElementAbsorbers.Add(this.storage, array);
		}

		// Token: 0x06009456 RID: 37974 RVA: 0x0035C225 File Offset: 0x0035A425
		public void StopAbsorbing()
		{
			if (!this.absorberHandle.IsValid())
			{
				return;
			}
			this.absorberHandle = Game.Instance.plantElementAbsorbers.Remove(this.absorberHandle);
		}

		// Token: 0x04007173 RID: 29043
		public AttributeModifier consumptionRate;

		// Token: 0x04007174 RID: 29044
		public AttributeModifier absorptionRate;

		// Token: 0x04007175 RID: 29045
		protected AmountInstance fertilization;

		// Token: 0x04007176 RID: 29046
		private Storage storage;

		// Token: 0x04007177 RID: 29047
		private HandleVector<int>.Handle absorberHandle = HandleVector<int>.InvalidHandle;

		// Token: 0x04007178 RID: 29048
		private float total_available_mass;
	}
}
