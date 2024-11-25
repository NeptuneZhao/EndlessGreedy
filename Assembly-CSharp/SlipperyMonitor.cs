using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200099F RID: 2463
public class SlipperyMonitor : GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>
{
	// Token: 0x060047C2 RID: 18370 RVA: 0x0019AD54 File Offset: 0x00198F54
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.safe;
		this.safe.EventTransition(GameHashes.NavigationCellChanged, this.unsafeCell, new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(SlipperyMonitor.IsStandingOnASlipperyCell));
		this.unsafeCell.EventTransition(GameHashes.NavigationCellChanged, this.safe, GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Not(new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(SlipperyMonitor.IsStandingOnASlipperyCell))).DefaultState(this.unsafeCell.atRisk);
		this.unsafeCell.atRisk.EventTransition(GameHashes.EquipmentChanged, this.unsafeCell.immune, new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(this.IsImmuneToSlipperySurfaces)).EventTransition(GameHashes.EffectAdded, this.unsafeCell.immune, new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(this.IsImmuneToSlipperySurfaces)).DefaultState(this.unsafeCell.atRisk.idle);
		this.unsafeCell.atRisk.idle.EventHandlerTransition(GameHashes.NavigationCellChanged, this.unsafeCell.atRisk.slip, new Func<SlipperyMonitor.Instance, object, bool>(SlipperyMonitor.RollDTwenty));
		this.unsafeCell.atRisk.slip.ToggleReactable(new Func<SlipperyMonitor.Instance, Reactable>(this.GetReactable)).ScheduleGoTo(8f, this.unsafeCell.atRisk.idle);
		this.unsafeCell.immune.EventTransition(GameHashes.EquipmentChanged, this.unsafeCell.atRisk, GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Not(new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(this.IsImmuneToSlipperySurfaces))).EventTransition(GameHashes.EffectRemoved, this.unsafeCell.atRisk, GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Not(new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(this.IsImmuneToSlipperySurfaces)));
	}

	// Token: 0x060047C3 RID: 18371 RVA: 0x0019AEF9 File Offset: 0x001990F9
	public bool IsImmuneToSlipperySurfaces(SlipperyMonitor.Instance smi)
	{
		return smi.IsImmune;
	}

	// Token: 0x060047C4 RID: 18372 RVA: 0x0019AF01 File Offset: 0x00199101
	public Reactable GetReactable(SlipperyMonitor.Instance smi)
	{
		return smi.CreateReactable();
	}

	// Token: 0x060047C5 RID: 18373 RVA: 0x0019AF0C File Offset: 0x0019910C
	private static bool IsStandingOnASlipperyCell(SlipperyMonitor.Instance smi)
	{
		int num = Grid.PosToCell(smi);
		int num2 = Grid.OffsetCell(num, 0, -1);
		return (Grid.IsValidCell(num) && Grid.Element[num].IsSlippery) || (Grid.IsValidCell(num2) && Grid.Element[num2].IsSlippery);
	}

	// Token: 0x060047C6 RID: 18374 RVA: 0x0019AF57 File Offset: 0x00199157
	private static bool RollDTwenty(SlipperyMonitor.Instance smi, object o)
	{
		return UnityEngine.Random.value <= 0.05f;
	}

	// Token: 0x04002EEC RID: 12012
	public const string EFFECT_NAME = "Slipped";

	// Token: 0x04002EED RID: 12013
	public const float SLIP_FAIL_TIMEOUT = 8f;

	// Token: 0x04002EEE RID: 12014
	public const float PROBABILITY_OF_SLIP = 0.05f;

	// Token: 0x04002EEF RID: 12015
	public GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State safe;

	// Token: 0x04002EF0 RID: 12016
	public SlipperyMonitor.UnsafeCellState unsafeCell;

	// Token: 0x0200197D RID: 6525
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200197E RID: 6526
	public class UnsafeCellState : GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State
	{
		// Token: 0x040079A1 RID: 31137
		public SlipperyMonitor.RiskStates atRisk;

		// Token: 0x040079A2 RID: 31138
		public GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State immune;
	}

	// Token: 0x0200197F RID: 6527
	public class RiskStates : GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State
	{
		// Token: 0x040079A3 RID: 31139
		public GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State idle;

		// Token: 0x040079A4 RID: 31140
		public GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State slip;
	}

	// Token: 0x02001980 RID: 6528
	public new class Instance : GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.GameInstance
	{
		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06009CD8 RID: 40152 RVA: 0x003734B5 File Offset: 0x003716B5
		public bool IsImmune
		{
			get
			{
				return this.effects.HasEffect("Slipped") || this.effects.HasImmunityTo(this.effect);
			}
		}

		// Token: 0x06009CD9 RID: 40153 RVA: 0x003734DC File Offset: 0x003716DC
		public Instance(IStateMachineTarget master, SlipperyMonitor.Def def) : base(master, def)
		{
			this.effects = base.GetComponent<Effects>();
			this.effect = Db.Get().effects.Get("Slipped");
		}

		// Token: 0x06009CDA RID: 40154 RVA: 0x0037350C File Offset: 0x0037170C
		public SlipperyMonitor.SlipReactable CreateReactable()
		{
			return new SlipperyMonitor.SlipReactable(this);
		}

		// Token: 0x040079A5 RID: 31141
		private Effect effect;

		// Token: 0x040079A6 RID: 31142
		public Effects effects;
	}

	// Token: 0x02001981 RID: 6529
	public class SlipReactable : Reactable
	{
		// Token: 0x06009CDB RID: 40155 RVA: 0x00373514 File Offset: 0x00371714
		public SlipReactable(SlipperyMonitor.Instance _smi) : base(_smi.gameObject, "Slip", Db.Get().ChoreTypes.Slip, 1, 1, false, 0f, 0f, 8f, 0f, ObjectLayer.NumLayers)
		{
			this.smi = _smi;
		}

		// Token: 0x06009CDC RID: 40156 RVA: 0x00373568 File Offset: 0x00371768
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (new_reactor == null)
			{
				return false;
			}
			if (this.gameObject != new_reactor)
			{
				return false;
			}
			if (this.smi == null)
			{
				return false;
			}
			Navigator component = new_reactor.GetComponent<Navigator>();
			return !(component == null) && component.CurrentNavType != NavType.Tube && component.CurrentNavType != NavType.Ladder && component.CurrentNavType != NavType.Pole;
		}

		// Token: 0x06009CDD RID: 40157 RVA: 0x003735DC File Offset: 0x003717DC
		protected override void InternalBegin()
		{
			this.startTime = Time.time;
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(Assets.GetAnim("anim_slip_kanim"), 1f);
			component.Play("slip_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("slip_loop", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("slip_pst", KAnim.PlayMode.Once, 1f, 0f);
			this.reactor.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.Slippering, null);
		}

		// Token: 0x06009CDE RID: 40158 RVA: 0x0037368A File Offset: 0x0037188A
		public override void Update(float dt)
		{
			if (Time.time - this.startTime > 4.3f)
			{
				base.Cleanup();
				this.ApplyEffect();
			}
		}

		// Token: 0x06009CDF RID: 40159 RVA: 0x003736AB File Offset: 0x003718AB
		public void ApplyEffect()
		{
			this.smi.effects.Add("Slipped", true);
		}

		// Token: 0x06009CE0 RID: 40160 RVA: 0x003736C4 File Offset: 0x003718C4
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
				if (component != null)
				{
					this.reactor.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.Slippering, false);
					component.RemoveAnimOverrides(Assets.GetAnim("anim_slip_kanim"));
				}
			}
		}

		// Token: 0x06009CE1 RID: 40161 RVA: 0x0037372A File Offset: 0x0037192A
		protected override void InternalCleanup()
		{
		}

		// Token: 0x040079A7 RID: 31143
		private SlipperyMonitor.Instance smi;

		// Token: 0x040079A8 RID: 31144
		private float startTime;

		// Token: 0x040079A9 RID: 31145
		private const string ANIM_FILE_NAME = "anim_slip_kanim";

		// Token: 0x040079AA RID: 31146
		private const float DURATION = 4.3f;
	}
}
