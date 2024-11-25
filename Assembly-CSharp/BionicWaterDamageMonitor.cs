using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200096C RID: 2412
public class BionicWaterDamageMonitor : GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>
{
	// Token: 0x060046B0 RID: 18096 RVA: 0x001944C4 File Offset: 0x001926C4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.safe;
		this.safe.Transition(this.threat, new StateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.Transition.ConditionCallback(BionicWaterDamageMonitor.IsThreatened), UpdateRate.SIM_200ms);
		this.threat.Transition(this.safe, GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.Not(new StateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.Transition.ConditionCallback(BionicWaterDamageMonitor.IsThreatened)), UpdateRate.SIM_200ms).Update(new Action<BionicWaterDamageMonitor.Instance, float>(BionicWaterDamageMonitor.ApplyDebuff), UpdateRate.SIM_200ms, false).Exit(new StateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.State.Callback(BionicWaterDamageMonitor.ClearNotification)).DefaultState(this.threat.idle).ToggleAnims("anim_bionic_hits_kanim", 3f);
		this.threat.idle.ParamTransition<float>(this.DamageCooldown, this.threat.damage, new StateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.Parameter<float>.Callback(BionicWaterDamageMonitor.IsTimeToZap)).ToggleReactable(new Func<BionicWaterDamageMonitor.Instance, Reactable>(BionicWaterDamageMonitor.ZapReactable)).Update(new Action<BionicWaterDamageMonitor.Instance, float>(BionicWaterDamageMonitor.DamageTimerUpdate), UpdateRate.SIM_200ms, false).Exit(new StateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.State.Callback(BionicWaterDamageMonitor.ResetDamageCooldown));
		this.threat.damage.Enter(new StateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.State.Callback(BionicWaterDamageMonitor.ApplyDamage)).Enter(new StateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.State.Callback(BionicWaterDamageMonitor.PlayDamageNotification)).GoTo(this.threat.idle).DoNotification((BionicWaterDamageMonitor.Instance smi) => smi.CreateLiquidDamageNotification());
	}

	// Token: 0x060046B1 RID: 18097 RVA: 0x0019462B File Offset: 0x0019282B
	private static Reactable ZapReactable(BionicWaterDamageMonitor.Instance smi)
	{
		return smi.GetReactable();
	}

	// Token: 0x060046B2 RID: 18098 RVA: 0x00194633 File Offset: 0x00192833
	private static bool IsThreatened(BionicWaterDamageMonitor.Instance smi)
	{
		return BionicWaterDamageMonitor.IsFloorWetWithIntolerantSubstance(smi);
	}

	// Token: 0x060046B3 RID: 18099 RVA: 0x0019463B File Offset: 0x0019283B
	private static bool IsTimeToZap(BionicWaterDamageMonitor.Instance smi, float time)
	{
		return time > smi.def.ZapInterval;
	}

	// Token: 0x060046B4 RID: 18100 RVA: 0x0019464B File Offset: 0x0019284B
	private static void ApplyDebuff(BionicWaterDamageMonitor.Instance smi, float dt)
	{
		smi.effects.Add("WaterDamage", true);
	}

	// Token: 0x060046B5 RID: 18101 RVA: 0x0019465F File Offset: 0x0019285F
	private static void ApplyDamage(BionicWaterDamageMonitor.Instance smi)
	{
		smi.ApplyDamage();
	}

	// Token: 0x060046B6 RID: 18102 RVA: 0x00194667 File Offset: 0x00192867
	private static void ResetDamageCooldown(BionicWaterDamageMonitor.Instance smi)
	{
		smi.sm.DamageCooldown.Set(0f, smi, false);
	}

	// Token: 0x060046B7 RID: 18103 RVA: 0x00194681 File Offset: 0x00192881
	private static void PlayDamageNotification(BionicWaterDamageMonitor.Instance smi)
	{
		smi.PlayDamageNotification();
	}

	// Token: 0x060046B8 RID: 18104 RVA: 0x00194689 File Offset: 0x00192889
	private static void ClearNotification(BionicWaterDamageMonitor.Instance smi)
	{
		smi.ClearNotification(null);
	}

	// Token: 0x060046B9 RID: 18105 RVA: 0x00194694 File Offset: 0x00192894
	private static void DamageTimerUpdate(BionicWaterDamageMonitor.Instance smi, float dt)
	{
		float damageCooldown = smi.DamageCooldown;
		smi.sm.DamageCooldown.Set(damageCooldown + dt, smi, false);
	}

	// Token: 0x060046BA RID: 18106 RVA: 0x001946C0 File Offset: 0x001928C0
	private static bool IsFloorWetWithIntolerantSubstance(BionicWaterDamageMonitor.Instance smi)
	{
		int num = Grid.PosToCell(smi);
		return Grid.IsValidCell(num) && Grid.Element[num].IsLiquid && !smi.kpid.HasTag(GameTags.HasAirtightSuit) && smi.def.IsElementIntolerable(Grid.Element[num].id);
	}

	// Token: 0x060046BB RID: 18107 RVA: 0x00194715 File Offset: 0x00192915
	private static string GetZapAnimName(BionicWaterDamageMonitor.Instance smi)
	{
		if (smi.GetComponent<Navigator>().CurrentNavType != NavType.Ladder)
		{
			return "zapped";
		}
		return "ladder_zapped";
	}

	// Token: 0x04002E12 RID: 11794
	public const string EFFECT_NAME = "WaterDamage";

	// Token: 0x04002E13 RID: 11795
	public GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.State safe;

	// Token: 0x04002E14 RID: 11796
	public BionicWaterDamageMonitor.DamageStates threat;

	// Token: 0x04002E15 RID: 11797
	public StateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.FloatParameter DamageCooldown;

	// Token: 0x020018F5 RID: 6389
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06009AAE RID: 39598 RVA: 0x0036DEA8 File Offset: 0x0036C0A8
		public bool IsElementIntolerable(SimHashes element)
		{
			for (int i = 0; i < this.IntolerantToElements.Length; i++)
			{
				if (this.IntolerantToElements[i] == element)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040077FB RID: 30715
		public readonly SimHashes[] IntolerantToElements = new SimHashes[]
		{
			SimHashes.Water,
			SimHashes.DirtyWater,
			SimHashes.SaltWater,
			SimHashes.Brine
		};

		// Token: 0x040077FC RID: 30716
		public float DamagePointsTakenPerShock = 20f;

		// Token: 0x040077FD RID: 30717
		public float ZapInterval = 15f;
	}

	// Token: 0x020018F6 RID: 6390
	public class DamageStates : GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.State
	{
		// Token: 0x040077FE RID: 30718
		public GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.State idle;

		// Token: 0x040077FF RID: 30719
		public GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.State damage;
	}

	// Token: 0x020018F7 RID: 6391
	public new class Instance : GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.GameInstance
	{
		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06009AB1 RID: 39601 RVA: 0x0036DF13 File Offset: 0x0036C113
		public float DamageCooldown
		{
			get
			{
				return base.sm.DamageCooldown.Get(this);
			}
		}

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x06009AB2 RID: 39602 RVA: 0x0036DF26 File Offset: 0x0036C126
		public bool IsAffectedByWaterDamage
		{
			get
			{
				return this.effects.HasEffect("WaterDamage");
			}
		}

		// Token: 0x06009AB3 RID: 39603 RVA: 0x0036DF38 File Offset: 0x0036C138
		public Instance(IStateMachineTarget master, BionicWaterDamageMonitor.Def def) : base(master, def)
		{
			this.health = base.GetComponent<Health>();
			this.effects = base.GetComponent<Effects>();
		}

		// Token: 0x06009AB4 RID: 39604 RVA: 0x0036DF5C File Offset: 0x0036C15C
		public void ApplyDamage()
		{
			this.health.Damage(base.def.DamagePointsTakenPerShock);
			int num = Grid.PosToCell(base.gameObject);
			if (Grid.IsValidCell(num))
			{
				this.lastElementDamagedBy = (base.smi.def.IsElementIntolerable(Grid.Element[num].id) ? Grid.Element[num] : null);
			}
		}

		// Token: 0x06009AB5 RID: 39605 RVA: 0x0036DFC4 File Offset: 0x0036C1C4
		public Reactable GetReactable()
		{
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, Db.Get().Emotes.Minion.WaterDamage.Id, Db.Get().ChoreTypes.WaterDamageZap, 0f, base.def.ZapInterval, float.PositiveInfinity, 0f);
			Emote waterDamage = Db.Get().Emotes.Minion.WaterDamage;
			selfEmoteReactable.SetEmote(waterDamage);
			selfEmoteReactable.preventChoreInterruption = true;
			return selfEmoteReactable;
		}

		// Token: 0x06009AB6 RID: 39606 RVA: 0x0036E04C File Offset: 0x0036C24C
		public Notification CreateLiquidDamageNotification()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			return new Notification(MISC.NOTIFICATIONS.BIONICLIQUIDDAMAGE.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.BIONICLIQUIDDAMAGE.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + component.GetProperName(), true, 0f, new Notification.ClickCallback(this.OnNotificationClicked), base.gameObject, null, true, false, false);
		}

		// Token: 0x06009AB7 RID: 39607 RVA: 0x0036E0BC File Offset: 0x0036C2BC
		private void OnNotificationClicked(object data)
		{
			if (data != null)
			{
				GameObject gameObject = (GameObject)data;
				if (gameObject != null)
				{
					GameUtil.FocusCamera(gameObject.transform, true);
				}
			}
		}

		// Token: 0x06009AB8 RID: 39608 RVA: 0x0036E0E8 File Offset: 0x0036C2E8
		public void ClearNotification(Notifier _notifier = null)
		{
			Notifier notifier = (_notifier == null) ? base.gameObject.AddOrGet<Notifier>() : _notifier;
			if (this.lastNotificationPlayed != null)
			{
				notifier.Remove(this.lastNotificationPlayed);
				this.lastNotificationPlayed = null;
			}
		}

		// Token: 0x06009AB9 RID: 39609 RVA: 0x0036E128 File Offset: 0x0036C328
		public void PlayDamageNotification()
		{
			Notifier notifier = base.gameObject.AddOrGet<Notifier>();
			this.ClearNotification(notifier);
			Notification notification = this.CreateLiquidDamageNotification();
			notifier.Add(notification, "");
			this.lastNotificationPlayed = notification;
		}

		// Token: 0x04007800 RID: 30720
		public Effects effects;

		// Token: 0x04007801 RID: 30721
		private Health health;

		// Token: 0x04007802 RID: 30722
		public Element lastElementDamagedBy;

		// Token: 0x04007803 RID: 30723
		private Notification lastNotificationPlayed;

		// Token: 0x04007804 RID: 30724
		[MyCmpGet]
		public KPrefabID kpid;
	}
}
