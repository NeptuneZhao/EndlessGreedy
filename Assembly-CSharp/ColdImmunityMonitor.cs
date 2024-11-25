using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000971 RID: 2417
public class ColdImmunityMonitor : GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance>
{
	// Token: 0x060046D8 RID: 18136 RVA: 0x00195270 File Offset: 0x00193470
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.DefaultState(this.idle.feelingFine).TagTransition(GameTags.FeelingCold, this.cold, false).ParamTransition<float>(this.coldCountdown, this.cold, GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.IsGTZero);
		this.idle.feelingFine.DoNothing();
		this.idle.leftWithDesireToWarmupAfterBeingCold.Enter(new StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(ColdImmunityMonitor.UpdateWarmUpCell)).Update(new Action<ColdImmunityMonitor.Instance, float>(ColdImmunityMonitor.UpdateWarmUpCell), UpdateRate.RENDER_1000ms, false).ToggleChore(new Func<ColdImmunityMonitor.Instance, Chore>(ColdImmunityMonitor.CreateRecoverFromChillyBonesChore), this.idle.feelingFine, this.idle.feelingFine);
		this.cold.DefaultState(this.cold.exiting).TagTransition(GameTags.FeelingWarm, this.idle, false).ToggleAnims("anim_idle_cold_kanim", 0f).ToggleAnims("anim_loco_run_cold_kanim", 0f).ToggleAnims("anim_loco_walk_cold_kanim", 0f).ToggleExpression(Db.Get().Expressions.Cold, null).ToggleThought(Db.Get().Thoughts.Cold, null).ToggleEffect("ColdAir").Enter(new StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(ColdImmunityMonitor.UpdateWarmUpCell)).Update(new Action<ColdImmunityMonitor.Instance, float>(ColdImmunityMonitor.UpdateWarmUpCell), UpdateRate.RENDER_1000ms, false).ToggleChore(new Func<ColdImmunityMonitor.Instance, Chore>(ColdImmunityMonitor.CreateRecoverFromChillyBonesChore), this.idle, this.cold);
		this.cold.exiting.EventHandlerTransition(GameHashes.EffectAdded, this.idle, new Func<ColdImmunityMonitor.Instance, object, bool>(ColdImmunityMonitor.HasImmunityEffect)).TagTransition(GameTags.FeelingCold, this.cold.idle, false).ToggleStatusItem(Db.Get().DuplicantStatusItems.ExitingCold, null).ParamTransition<float>(this.coldCountdown, this.idle.leftWithDesireToWarmupAfterBeingCold, GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.IsZero).Update(new Action<ColdImmunityMonitor.Instance, float>(ColdImmunityMonitor.ColdTimerUpdate), UpdateRate.SIM_200ms, false).Exit(new StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(ColdImmunityMonitor.ClearTimer));
		this.cold.idle.Enter(new StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(ColdImmunityMonitor.ResetColdTimer)).ToggleStatusItem(Db.Get().DuplicantStatusItems.Cold, (ColdImmunityMonitor.Instance smi) => smi).TagTransition(GameTags.FeelingCold, this.cold.exiting, true);
	}

	// Token: 0x060046D9 RID: 18137 RVA: 0x001954F4 File Offset: 0x001936F4
	public static bool OnEffectAdded(ColdImmunityMonitor.Instance smi, object data)
	{
		return true;
	}

	// Token: 0x060046DA RID: 18138 RVA: 0x001954F7 File Offset: 0x001936F7
	public static void ClearTimer(ColdImmunityMonitor.Instance smi)
	{
		smi.sm.coldCountdown.Set(0f, smi, false);
	}

	// Token: 0x060046DB RID: 18139 RVA: 0x00195511 File Offset: 0x00193711
	public static void ResetColdTimer(ColdImmunityMonitor.Instance smi)
	{
		smi.sm.coldCountdown.Set(5f, smi, false);
	}

	// Token: 0x060046DC RID: 18140 RVA: 0x0019552C File Offset: 0x0019372C
	public static void ColdTimerUpdate(ColdImmunityMonitor.Instance smi, float dt)
	{
		float value = Mathf.Clamp(smi.ColdCountdown - dt, 0f, 5f);
		smi.sm.coldCountdown.Set(value, smi, false);
	}

	// Token: 0x060046DD RID: 18141 RVA: 0x00195565 File Offset: 0x00193765
	private static void UpdateWarmUpCell(ColdImmunityMonitor.Instance smi, float dt)
	{
		smi.UpdateWarmUpCell();
	}

	// Token: 0x060046DE RID: 18142 RVA: 0x0019556D File Offset: 0x0019376D
	private static void UpdateWarmUpCell(ColdImmunityMonitor.Instance smi)
	{
		smi.UpdateWarmUpCell();
	}

	// Token: 0x060046DF RID: 18143 RVA: 0x00195578 File Offset: 0x00193778
	public static bool HasImmunityEffect(ColdImmunityMonitor.Instance smi, object data)
	{
		Effects component = smi.GetComponent<Effects>();
		return component != null && component.HasEffect("WarmTouch");
	}

	// Token: 0x060046E0 RID: 18144 RVA: 0x001955A2 File Offset: 0x001937A2
	private static Chore CreateRecoverFromChillyBonesChore(ColdImmunityMonitor.Instance smi)
	{
		return new RecoverFromColdChore(smi.master);
	}

	// Token: 0x04002E25 RID: 11813
	private const float EFFECT_DURATION = 5f;

	// Token: 0x04002E26 RID: 11814
	public ColdImmunityMonitor.IdleStates idle;

	// Token: 0x04002E27 RID: 11815
	public ColdImmunityMonitor.ColdStates cold;

	// Token: 0x04002E28 RID: 11816
	public StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.FloatParameter coldCountdown;

	// Token: 0x02001905 RID: 6405
	public class ColdStates : GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400782E RID: 30766
		public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x0400782F RID: 30767
		public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State exiting;

		// Token: 0x04007830 RID: 30768
		public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State resetChore;
	}

	// Token: 0x02001906 RID: 6406
	public class IdleStates : GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04007831 RID: 30769
		public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State feelingFine;

		// Token: 0x04007832 RID: 30770
		public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State leftWithDesireToWarmupAfterBeingCold;
	}

	// Token: 0x02001907 RID: 6407
	public new class Instance : GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x06009AEF RID: 39663 RVA: 0x0036E661 File Offset: 0x0036C861
		// (set) Token: 0x06009AF0 RID: 39664 RVA: 0x0036E669 File Offset: 0x0036C869
		public ColdImmunityProvider.Instance NearestImmunityProvider { get; private set; }

		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x06009AF1 RID: 39665 RVA: 0x0036E672 File Offset: 0x0036C872
		// (set) Token: 0x06009AF2 RID: 39666 RVA: 0x0036E67A File Offset: 0x0036C87A
		public int WarmUpCell { get; private set; }

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x06009AF3 RID: 39667 RVA: 0x0036E683 File Offset: 0x0036C883
		public float ColdCountdown
		{
			get
			{
				return base.smi.sm.coldCountdown.Get(this);
			}
		}

		// Token: 0x06009AF4 RID: 39668 RVA: 0x0036E69B File Offset: 0x0036C89B
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06009AF5 RID: 39669 RVA: 0x0036E6A4 File Offset: 0x0036C8A4
		public override void StartSM()
		{
			this.navigator = base.gameObject.GetComponent<Navigator>();
			base.StartSM();
		}

		// Token: 0x06009AF6 RID: 39670 RVA: 0x0036E6C0 File Offset: 0x0036C8C0
		public void UpdateWarmUpCell()
		{
			int myWorldId = this.navigator.GetMyWorldId();
			int warmUpCell = Grid.InvalidCell;
			int num = int.MaxValue;
			ColdImmunityProvider.Instance nearestImmunityProvider = null;
			foreach (StateMachine.Instance instance in Components.EffectImmunityProviderStations.Items.FindAll((StateMachine.Instance t) => t is ColdImmunityProvider.Instance))
			{
				ColdImmunityProvider.Instance instance2 = instance as ColdImmunityProvider.Instance;
				if (instance2.GetMyWorldId() == myWorldId)
				{
					int maxValue = int.MaxValue;
					int bestAvailableCell = instance2.GetBestAvailableCell(this.navigator, out maxValue);
					if (maxValue < num)
					{
						num = maxValue;
						nearestImmunityProvider = instance2;
						warmUpCell = bestAvailableCell;
					}
				}
			}
			this.NearestImmunityProvider = nearestImmunityProvider;
			this.WarmUpCell = warmUpCell;
		}

		// Token: 0x04007835 RID: 30773
		private Navigator navigator;
	}
}
