using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000986 RID: 2438
public class HeatImmunityMonitor : GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance>
{
	// Token: 0x06004734 RID: 18228 RVA: 0x001976C8 File Offset: 0x001958C8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.DefaultState(this.idle.feelingFine).TagTransition(GameTags.FeelingWarm, this.warm, false).ParamTransition<float>(this.heatCountdown, this.warm, GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.IsGTZero);
		this.idle.feelingFine.DoNothing();
		this.idle.leftWithDesireToCooldownAfterBeingWarm.Enter(new StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(HeatImmunityMonitor.UpdateShelterCell)).Update(new Action<HeatImmunityMonitor.Instance, float>(HeatImmunityMonitor.UpdateShelterCell), UpdateRate.RENDER_1000ms, false).ToggleChore(new Func<HeatImmunityMonitor.Instance, Chore>(HeatImmunityMonitor.CreateRecoverFromOverheatChore), this.idle.feelingFine, this.idle.feelingFine);
		this.warm.DefaultState(this.warm.exiting).TagTransition(GameTags.FeelingCold, this.idle, false).ToggleAnims("anim_idle_hot_kanim", 0f).ToggleAnims("anim_loco_run_hot_kanim", 0f).ToggleAnims("anim_loco_walk_hot_kanim", 0f).ToggleExpression(Db.Get().Expressions.Hot, null).ToggleThought(Db.Get().Thoughts.Hot, null).ToggleEffect("WarmAir").Enter(new StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(HeatImmunityMonitor.UpdateShelterCell)).Update(new Action<HeatImmunityMonitor.Instance, float>(HeatImmunityMonitor.UpdateShelterCell), UpdateRate.RENDER_1000ms, false).ToggleChore(new Func<HeatImmunityMonitor.Instance, Chore>(HeatImmunityMonitor.CreateRecoverFromOverheatChore), this.idle, this.warm);
		this.warm.exiting.EventHandlerTransition(GameHashes.EffectAdded, this.idle, new Func<HeatImmunityMonitor.Instance, object, bool>(HeatImmunityMonitor.HasImmunityEffect)).TagTransition(GameTags.FeelingWarm, this.warm.idle, false).ToggleStatusItem(Db.Get().DuplicantStatusItems.ExitingHot, null).ParamTransition<float>(this.heatCountdown, this.idle.leftWithDesireToCooldownAfterBeingWarm, GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.IsZero).Update(new Action<HeatImmunityMonitor.Instance, float>(HeatImmunityMonitor.HeatTimerUpdate), UpdateRate.SIM_200ms, false).Exit(new StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(HeatImmunityMonitor.ClearTimer));
		this.warm.idle.Enter(new StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(HeatImmunityMonitor.ResetHeatTimer)).ToggleStatusItem(Db.Get().DuplicantStatusItems.Hot, (HeatImmunityMonitor.Instance smi) => smi).TagTransition(GameTags.FeelingWarm, this.warm.exiting, true);
	}

	// Token: 0x06004735 RID: 18229 RVA: 0x0019794C File Offset: 0x00195B4C
	public static bool OnEffectAdded(HeatImmunityMonitor.Instance smi, object data)
	{
		return true;
	}

	// Token: 0x06004736 RID: 18230 RVA: 0x0019794F File Offset: 0x00195B4F
	public static void ClearTimer(HeatImmunityMonitor.Instance smi)
	{
		smi.sm.heatCountdown.Set(0f, smi, false);
	}

	// Token: 0x06004737 RID: 18231 RVA: 0x00197969 File Offset: 0x00195B69
	public static void ResetHeatTimer(HeatImmunityMonitor.Instance smi)
	{
		smi.sm.heatCountdown.Set(5f, smi, false);
	}

	// Token: 0x06004738 RID: 18232 RVA: 0x00197984 File Offset: 0x00195B84
	public static void HeatTimerUpdate(HeatImmunityMonitor.Instance smi, float dt)
	{
		float value = Mathf.Clamp(smi.HeatCountdown - dt, 0f, 5f);
		smi.sm.heatCountdown.Set(value, smi, false);
	}

	// Token: 0x06004739 RID: 18233 RVA: 0x001979BD File Offset: 0x00195BBD
	private static void UpdateShelterCell(HeatImmunityMonitor.Instance smi, float dt)
	{
		smi.UpdateShelterCell();
	}

	// Token: 0x0600473A RID: 18234 RVA: 0x001979C5 File Offset: 0x00195BC5
	private static void UpdateShelterCell(HeatImmunityMonitor.Instance smi)
	{
		smi.UpdateShelterCell();
	}

	// Token: 0x0600473B RID: 18235 RVA: 0x001979D0 File Offset: 0x00195BD0
	public static bool HasImmunityEffect(HeatImmunityMonitor.Instance smi, object data)
	{
		Effects component = smi.GetComponent<Effects>();
		return component != null && component.HasEffect("RefreshingTouch");
	}

	// Token: 0x0600473C RID: 18236 RVA: 0x001979FA File Offset: 0x00195BFA
	private static Chore CreateRecoverFromOverheatChore(HeatImmunityMonitor.Instance smi)
	{
		return new RecoverFromHeatChore(smi.master);
	}

	// Token: 0x04002E79 RID: 11897
	private const float EFFECT_DURATION = 5f;

	// Token: 0x04002E7A RID: 11898
	public HeatImmunityMonitor.IdleStates idle;

	// Token: 0x04002E7B RID: 11899
	public HeatImmunityMonitor.WarmStates warm;

	// Token: 0x04002E7C RID: 11900
	public StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.FloatParameter heatCountdown;

	// Token: 0x02001940 RID: 6464
	public class WarmStates : GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040078ED RID: 30957
		public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x040078EE RID: 30958
		public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State exiting;

		// Token: 0x040078EF RID: 30959
		public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State resetChore;
	}

	// Token: 0x02001941 RID: 6465
	public class IdleStates : GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040078F0 RID: 30960
		public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State feelingFine;

		// Token: 0x040078F1 RID: 30961
		public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State leftWithDesireToCooldownAfterBeingWarm;
	}

	// Token: 0x02001942 RID: 6466
	public new class Instance : GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x06009BDF RID: 39903 RVA: 0x003713A7 File Offset: 0x0036F5A7
		// (set) Token: 0x06009BE0 RID: 39904 RVA: 0x003713AF File Offset: 0x0036F5AF
		public HeatImmunityProvider.Instance NearestImmunityProvider { get; private set; }

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x06009BE1 RID: 39905 RVA: 0x003713B8 File Offset: 0x0036F5B8
		// (set) Token: 0x06009BE2 RID: 39906 RVA: 0x003713C0 File Offset: 0x0036F5C0
		public int ShelterCell { get; private set; }

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06009BE3 RID: 39907 RVA: 0x003713C9 File Offset: 0x0036F5C9
		public float HeatCountdown
		{
			get
			{
				return base.smi.sm.heatCountdown.Get(this);
			}
		}

		// Token: 0x06009BE4 RID: 39908 RVA: 0x003713E1 File Offset: 0x0036F5E1
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06009BE5 RID: 39909 RVA: 0x003713EA File Offset: 0x0036F5EA
		public override void StartSM()
		{
			this.navigator = base.gameObject.GetComponent<Navigator>();
			base.StartSM();
		}

		// Token: 0x06009BE6 RID: 39910 RVA: 0x00371404 File Offset: 0x0036F604
		public void UpdateShelterCell()
		{
			int myWorldId = this.navigator.GetMyWorldId();
			int shelterCell = Grid.InvalidCell;
			int num = int.MaxValue;
			HeatImmunityProvider.Instance nearestImmunityProvider = null;
			foreach (StateMachine.Instance instance in Components.EffectImmunityProviderStations.Items.FindAll((StateMachine.Instance t) => t is HeatImmunityProvider.Instance))
			{
				HeatImmunityProvider.Instance instance2 = instance as HeatImmunityProvider.Instance;
				if (instance2.GetMyWorldId() == myWorldId)
				{
					int maxValue = int.MaxValue;
					int bestAvailableCell = instance2.GetBestAvailableCell(this.navigator, out maxValue);
					if (maxValue < num)
					{
						num = maxValue;
						nearestImmunityProvider = instance2;
						shelterCell = bestAvailableCell;
					}
				}
			}
			this.NearestImmunityProvider = nearestImmunityProvider;
			this.ShelterCell = shelterCell;
		}

		// Token: 0x040078F4 RID: 30964
		private Navigator navigator;
	}
}
