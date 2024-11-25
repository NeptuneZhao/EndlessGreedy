using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;

// Token: 0x02000968 RID: 2408
public class BionicOilMonitor : GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>
{
	// Token: 0x0600468A RID: 18058 RVA: 0x00193708 File Offset: 0x00191908
	private static Effect CreateFreshOilEffectVariation(string id, float stressBonus, float moralBonus)
	{
		Effect effect = new Effect("FreshOil_" + id, DUPLICANTS.MODIFIERS.FRESHOIL.NAME, DUPLICANTS.MODIFIERS.FRESHOIL.TOOLTIP, 4800f, true, true, false, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, moralBonus, DUPLICANTS.MODIFIERS.FRESHOIL.NAME, false, false, true));
		effect.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, stressBonus, DUPLICANTS.MODIFIERS.FRESHOIL.NAME, false, false, true));
		return effect;
	}

	// Token: 0x0600468B RID: 18059 RVA: 0x001937B4 File Offset: 0x001919B4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.offline;
		this.root.Exit(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.RemoveBaseOilDeltaModifier));
		this.offline.EventTransition(GameHashes.BionicOnline, this.online, new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Transition.ConditionCallback(BionicOilMonitor.IsBionicOnline)).Enter(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.RemoveBaseOilDeltaModifier));
		this.online.EventTransition(GameHashes.BionicOffline, this.offline, GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Not(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Transition.ConditionCallback(BionicOilMonitor.IsBionicOnline))).Enter(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.AddBaseOilDeltaModifier)).DefaultState(this.online.idle);
		this.online.idle.EnterTransition(this.online.seeking, new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Transition.ConditionCallback(BionicOilMonitor.WantsOilChange)).OnSignal(this.OilValueChanged, this.online.seeking, new Func<BionicOilMonitor.Instance, bool>(BionicOilMonitor.WantsOilChange));
		this.online.seeking.OnSignal(this.OilFilledSignal, this.online.idle).OnSignal(this.OilValueChanged, this.online.idle, new Func<BionicOilMonitor.Instance, bool>(BionicOilMonitor.HasDecentAmountOfOil)).DefaultState(this.online.seeking.hasOil).ToggleUrge(Db.Get().Urges.OilRefill);
		this.online.seeking.hasOil.EnterTransition(this.online.seeking.noOil, GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Not(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Transition.ConditionCallback(BionicOilMonitor.HasAnyAmountOfOil))).OnSignal(this.OilRanOutSignal, this.online.seeking.noOil).ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicWantsOilChange, null);
		this.online.seeking.noOil.ToggleEffect("NoLubrication");
	}

	// Token: 0x0600468C RID: 18060 RVA: 0x0019399E File Offset: 0x00191B9E
	public static bool IsBionicOnline(BionicOilMonitor.Instance smi)
	{
		return smi.IsOnline;
	}

	// Token: 0x0600468D RID: 18061 RVA: 0x001939A6 File Offset: 0x00191BA6
	public static bool HasAnyAmountOfOil(BionicOilMonitor.Instance smi)
	{
		return smi.CurrentOilMass > 0f;
	}

	// Token: 0x0600468E RID: 18062 RVA: 0x001939B5 File Offset: 0x00191BB5
	public static bool HasDecentAmountOfOil(BionicOilMonitor.Instance smi)
	{
		return smi.CurrentOilPercentage > 0.2f;
	}

	// Token: 0x0600468F RID: 18063 RVA: 0x001939C4 File Offset: 0x00191BC4
	public static bool WantsOilChange(BionicOilMonitor.Instance smi)
	{
		return smi.CurrentOilPercentage <= 0.2f;
	}

	// Token: 0x06004690 RID: 18064 RVA: 0x001939D6 File Offset: 0x00191BD6
	public static void AddBaseOilDeltaModifier(BionicOilMonitor.Instance smi)
	{
		smi.SetBaseDeltaModifierActiveState(true);
	}

	// Token: 0x06004691 RID: 18065 RVA: 0x001939DF File Offset: 0x00191BDF
	public static void RemoveBaseOilDeltaModifier(BionicOilMonitor.Instance smi)
	{
		smi.SetBaseDeltaModifierActiveState(false);
	}

	// Token: 0x06004693 RID: 18067 RVA: 0x001939F0 File Offset: 0x00191BF0
	// Note: this type is marked as 'beforefieldinit'.
	static BionicOilMonitor()
	{
		Dictionary<SimHashes, Effect> dictionary = new Dictionary<SimHashes, Effect>();
		dictionary[SimHashes.CrudeOil] = BionicOilMonitor.CreateFreshOilEffectVariation(SimHashes.CrudeOil.ToString(), -0.016666668f, 3f);
		dictionary[SimHashes.PhytoOil] = BionicOilMonitor.CreateFreshOilEffectVariation(SimHashes.PhytoOil.ToString(), -0.008333334f, 2f);
		BionicOilMonitor.LUBRICANT_TYPE_EFFECT = dictionary;
	}

	// Token: 0x04002DEB RID: 11755
	public static Dictionary<SimHashes, Effect> LUBRICANT_TYPE_EFFECT;

	// Token: 0x04002DEC RID: 11756
	public const float OIL_CAPACITY = 200f;

	// Token: 0x04002DED RID: 11757
	public const float OIL_TANK_DURATION = 6000f;

	// Token: 0x04002DEE RID: 11758
	public const float OIL_REFILL_TRESHOLD = 0.2f;

	// Token: 0x04002DEF RID: 11759
	public const string NO_OIL_EFFECT_NAME = "NoLubrication";

	// Token: 0x04002DF0 RID: 11760
	public GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State offline;

	// Token: 0x04002DF1 RID: 11761
	public BionicOilMonitor.OnlineStates online;

	// Token: 0x04002DF2 RID: 11762
	public StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Signal OilFilledSignal;

	// Token: 0x04002DF3 RID: 11763
	public StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Signal OilRanOutSignal;

	// Token: 0x04002DF4 RID: 11764
	public StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Signal OilValueChanged;

	// Token: 0x020018E0 RID: 6368
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020018E1 RID: 6369
	public class WantsOilChangeState : GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State
	{
		// Token: 0x040077BD RID: 30653
		public GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State hasOil;

		// Token: 0x040077BE RID: 30654
		public GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State noOil;
	}

	// Token: 0x020018E2 RID: 6370
	public class OnlineStates : GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State
	{
		// Token: 0x040077BF RID: 30655
		public GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State idle;

		// Token: 0x040077C0 RID: 30656
		public BionicOilMonitor.WantsOilChangeState seeking;
	}

	// Token: 0x020018E3 RID: 6371
	public new class Instance : GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.GameInstance
	{
		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06009A3D RID: 39485 RVA: 0x0036CBB8 File Offset: 0x0036ADB8
		public bool IsOnline
		{
			get
			{
				return this.batterySMI != null && this.batterySMI.IsOnline;
			}
		}

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06009A3E RID: 39486 RVA: 0x0036CBCF File Offset: 0x0036ADCF
		public bool HasOil
		{
			get
			{
				return this.CurrentOilMass > 0f;
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06009A3F RID: 39487 RVA: 0x0036CBDE File Offset: 0x0036ADDE
		public float CurrentOilPercentage
		{
			get
			{
				return this.CurrentOilMass / this.oilAmount.GetMax();
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x06009A40 RID: 39488 RVA: 0x0036CBF2 File Offset: 0x0036ADF2
		public float CurrentOilMass
		{
			get
			{
				if (this.oilAmount != null)
				{
					return this.oilAmount.value;
				}
				return 0f;
			}
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x06009A42 RID: 39490 RVA: 0x0036CC16 File Offset: 0x0036AE16
		// (set) Token: 0x06009A41 RID: 39489 RVA: 0x0036CC0D File Offset: 0x0036AE0D
		public AmountInstance oilAmount { get; private set; }

		// Token: 0x06009A43 RID: 39491 RVA: 0x0036CC20 File Offset: 0x0036AE20
		public Instance(IStateMachineTarget master, BionicOilMonitor.Def def) : base(master, def)
		{
			this.oilAmount = Db.Get().Amounts.BionicOil.Lookup(base.gameObject);
			AmountInstance oilAmount = this.oilAmount;
			oilAmount.OnMaxValueReached = (System.Action)Delegate.Combine(oilAmount.OnMaxValueReached, new System.Action(this.OnOilTankFilled));
			AmountInstance oilAmount2 = this.oilAmount;
			oilAmount2.OnMinValueReached = (System.Action)Delegate.Combine(oilAmount2.OnMinValueReached, new System.Action(this.OnOilRanOut));
			AmountInstance oilAmount3 = this.oilAmount;
			oilAmount3.OnValueChanged = (Action<float>)Delegate.Combine(oilAmount3.OnValueChanged, new Action<float>(this.OnOilValueChanged));
			this.batterySMI = base.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
		}

		// Token: 0x06009A44 RID: 39492 RVA: 0x0036CD0C File Offset: 0x0036AF0C
		public override void StartSM()
		{
			base.StartSM();
		}

		// Token: 0x06009A45 RID: 39493 RVA: 0x0036CD14 File Offset: 0x0036AF14
		private void OnOilTankFilled()
		{
			base.sm.OilFilledSignal.Trigger(this);
		}

		// Token: 0x06009A46 RID: 39494 RVA: 0x0036CD27 File Offset: 0x0036AF27
		private void OnOilRanOut()
		{
			base.sm.OilRanOutSignal.Trigger(this);
		}

		// Token: 0x06009A47 RID: 39495 RVA: 0x0036CD3A File Offset: 0x0036AF3A
		private void OnOilValueChanged(float delta)
		{
			base.sm.OilValueChanged.Trigger(this);
		}

		// Token: 0x06009A48 RID: 39496 RVA: 0x0036CD4D File Offset: 0x0036AF4D
		public void SetOilMassValue(float value)
		{
			this.oilAmount.SetValue(value);
		}

		// Token: 0x06009A49 RID: 39497 RVA: 0x0036CD5C File Offset: 0x0036AF5C
		public void SetBaseDeltaModifierActiveState(bool isActive)
		{
			MinionModifiers component = base.GetComponent<MinionModifiers>();
			if (isActive)
			{
				bool flag = false;
				int count = component.attributes.Get(this.BaseOilDeltaModifier.AttributeId).Modifiers.Count;
				for (int i = 0; i < count; i++)
				{
					if (component.attributes.Get(this.BaseOilDeltaModifier.AttributeId).Modifiers[i] == this.BaseOilDeltaModifier)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					component.attributes.Add(this.BaseOilDeltaModifier);
					return;
				}
			}
			else
			{
				component.attributes.Remove(this.BaseOilDeltaModifier);
			}
		}

		// Token: 0x06009A4A RID: 39498 RVA: 0x0036CDF5 File Offset: 0x0036AFF5
		public void RefillOil(float amount)
		{
			this.oilAmount.SetValue(this.CurrentOilMass + amount);
			this.OnOilTankFilled();
		}

		// Token: 0x040077C1 RID: 30657
		private BionicBatteryMonitor.Instance batterySMI;

		// Token: 0x040077C2 RID: 30658
		private AttributeModifier BaseOilDeltaModifier = new AttributeModifier(Db.Get().Amounts.BionicOil.deltaAttribute.Id, -0.033333335f, BionicMinionConfig.NAME, false, false, true);
	}
}
