using System;
using STRINGS;
using UnityEngine;

// Token: 0x020009F2 RID: 2546
public class JungleGasPlant : StateMachineComponent<JungleGasPlant.StatesInstance>
{
	// Token: 0x060049BE RID: 18878 RVA: 0x001A6016 File Offset: 0x001A4216
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060049BF RID: 18879 RVA: 0x001A6029 File Offset: 0x001A4229
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x04003059 RID: 12377
	[MyCmpReq]
	private ReceptacleMonitor rm;

	// Token: 0x0400305A RID: 12378
	[MyCmpReq]
	private Growing growing;

	// Token: 0x0400305B RID: 12379
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x0400305C RID: 12380
	[MyCmpReq]
	private ElementEmitter elementEmitter;

	// Token: 0x020019FD RID: 6653
	public class StatesInstance : GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.GameInstance
	{
		// Token: 0x06009EA3 RID: 40611 RVA: 0x0037928B File Offset: 0x0037748B
		public StatesInstance(JungleGasPlant master) : base(master)
		{
		}
	}

	// Token: 0x020019FE RID: 6654
	public class States : GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant>
	{
		// Token: 0x06009EA4 RID: 40612 RVA: 0x00379294 File Offset: 0x00377494
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.alive.seed_grow;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Enter(delegate(JungleGasPlant.StatesInstance smi)
			{
				if (smi.master.rm.Replanted && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
					return;
				}
				smi.GoTo(this.alive.seed_grow);
			});
			GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter(delegate(JungleGasPlant.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).TagTransition(GameTags.Entombed, this.alive.seed_grow, true).EventTransition(GameHashes.TooColdWarning, this.alive.seed_grow, null).EventTransition(GameHashes.TooHotWarning, this.alive.seed_grow, null).TagTransition(GameTags.Uprooted, this.dead, false);
			this.alive.InitializeStates(this.masterTarget, this.dead);
			this.alive.seed_grow.QueueAnim("seed_grow", false, null).EventTransition(GameHashes.AnimQueueComplete, this.alive.idle, null).EventTransition(GameHashes.Wilt, this.alive.wilting, (JungleGasPlant.StatesInstance smi) => smi.master.wiltCondition.IsWilting());
			this.alive.idle.EventTransition(GameHashes.Wilt, this.alive.wilting, (JungleGasPlant.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).EventTransition(GameHashes.Grow, this.alive.grown, (JungleGasPlant.StatesInstance smi) => smi.master.growing.IsGrown()).PlayAnim("idle_loop", KAnim.PlayMode.Loop);
			this.alive.grown.DefaultState(this.alive.grown.pre).EventTransition(GameHashes.Wilt, this.alive.wilting, (JungleGasPlant.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).Enter(delegate(JungleGasPlant.StatesInstance smi)
			{
				smi.master.elementEmitter.SetEmitting(true);
			}).Exit(delegate(JungleGasPlant.StatesInstance smi)
			{
				smi.master.elementEmitter.SetEmitting(false);
			});
			this.alive.grown.pre.PlayAnim("grow", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.grown.idle);
			this.alive.grown.idle.PlayAnim("idle_bloom_loop", KAnim.PlayMode.Loop);
			this.alive.wilting.pre.DefaultState(this.alive.wilting.pre).PlayAnim("wilt_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.wilting.idle).EventTransition(GameHashes.WiltRecover, this.alive.wilting.pst, (JungleGasPlant.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
			this.alive.wilting.idle.PlayAnim("idle_wilt_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.wilting.pst, (JungleGasPlant.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
			this.alive.wilting.pst.PlayAnim("wilt_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.idle);
		}

		// Token: 0x04007AFD RID: 31485
		public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State blocked_from_growing;

		// Token: 0x04007AFE RID: 31486
		public JungleGasPlant.States.AliveStates alive;

		// Token: 0x04007AFF RID: 31487
		public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State dead;

		// Token: 0x020025DD RID: 9693
		public class AliveStates : GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.PlantAliveSubState
		{
			// Token: 0x0400A895 RID: 43157
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State seed_grow;

			// Token: 0x0400A896 RID: 43158
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State idle;

			// Token: 0x0400A897 RID: 43159
			public JungleGasPlant.States.WiltingState wilting;

			// Token: 0x0400A898 RID: 43160
			public JungleGasPlant.States.GrownState grown;

			// Token: 0x0400A899 RID: 43161
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State destroy;
		}

		// Token: 0x020025DE RID: 9694
		public class GrownState : GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State
		{
			// Token: 0x0400A89A RID: 43162
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State pre;

			// Token: 0x0400A89B RID: 43163
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State idle;
		}

		// Token: 0x020025DF RID: 9695
		public class WiltingState : GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State
		{
			// Token: 0x0400A89C RID: 43164
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State pre;

			// Token: 0x0400A89D RID: 43165
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State idle;

			// Token: 0x0400A89E RID: 43166
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State pst;
		}
	}
}
