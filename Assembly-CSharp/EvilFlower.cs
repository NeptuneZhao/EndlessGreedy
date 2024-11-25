using System;
using STRINGS;
using UnityEngine;

// Token: 0x020009F0 RID: 2544
public class EvilFlower : StateMachineComponent<EvilFlower.StatesInstance>
{
	// Token: 0x060049B6 RID: 18870 RVA: 0x001A5D70 File Offset: 0x001A3F70
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<EvilFlower>(1309017699, EvilFlower.SetReplantedTrueDelegate);
	}

	// Token: 0x060049B7 RID: 18871 RVA: 0x001A5D89 File Offset: 0x001A3F89
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060049B8 RID: 18872 RVA: 0x001A5D9C File Offset: 0x001A3F9C
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x0400304B RID: 12363
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x0400304C RID: 12364
	[MyCmpReq]
	private EntombVulnerable entombVulnerable;

	// Token: 0x0400304D RID: 12365
	public bool replanted;

	// Token: 0x0400304E RID: 12366
	public EffectorValues positive_decor_effect = new EffectorValues
	{
		amount = 1,
		radius = 5
	};

	// Token: 0x0400304F RID: 12367
	public EffectorValues negative_decor_effect = new EffectorValues
	{
		amount = -1,
		radius = 5
	};

	// Token: 0x04003050 RID: 12368
	private static readonly EventSystem.IntraObjectHandler<EvilFlower> SetReplantedTrueDelegate = new EventSystem.IntraObjectHandler<EvilFlower>(delegate(EvilFlower component, object data)
	{
		component.replanted = true;
	});

	// Token: 0x020019FA RID: 6650
	public class StatesInstance : GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.GameInstance
	{
		// Token: 0x06009E99 RID: 40601 RVA: 0x00378F5B File Offset: 0x0037715B
		public StatesInstance(EvilFlower smi) : base(smi)
		{
		}
	}

	// Token: 0x020019FB RID: 6651
	public class States : GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower>
	{
		// Token: 0x06009E9A RID: 40602 RVA: 0x00378F64 File Offset: 0x00377164
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grow;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).TriggerOnEnter(GameHashes.BurstEmitDisease, null).ToggleTag(GameTags.PreventEmittingDisease).Enter(delegate(EvilFlower.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (EvilFlower.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (EvilFlower.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (EvilFlower.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(EvilFlower.StatesInstance smi)
			{
				if (smi.master.replanted && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
				}
			}).PlayAnim("grow_seed", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive, null);
			GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State state2 = this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.idle);
			string name2 = CREATURES.STATUSITEMS.IDLE.NAME;
			string tooltip2 = CREATURES.STATUSITEMS.IDLE.TOOLTIP;
			string icon2 = "";
			StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
			NotificationType notification_type2 = NotificationType.Neutral;
			bool allow_multiples2 = false;
			main = Db.Get().StatusItemCategories.Main;
			state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main);
			this.alive.idle.EventTransition(GameHashes.Wilt, this.alive.wilting, (EvilFlower.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).PlayAnim("idle", KAnim.PlayMode.Loop).Enter(delegate(EvilFlower.StatesInstance smi)
			{
				smi.master.GetComponent<DecorProvider>().SetValues(smi.master.positive_decor_effect);
				smi.master.GetComponent<DecorProvider>().Refresh();
				smi.master.AddTag(GameTags.Decoration);
			});
			this.alive.wilting.PlayAnim("wilt1", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.idle, null).ToggleTag(GameTags.PreventEmittingDisease).Enter(delegate(EvilFlower.StatesInstance smi)
			{
				smi.master.GetComponent<DecorProvider>().SetValues(smi.master.negative_decor_effect);
				smi.master.GetComponent<DecorProvider>().Refresh();
				smi.master.RemoveTag(GameTags.Decoration);
			});
		}

		// Token: 0x04007AF8 RID: 31480
		public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State grow;

		// Token: 0x04007AF9 RID: 31481
		public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State blocked_from_growing;

		// Token: 0x04007AFA RID: 31482
		public EvilFlower.States.AliveStates alive;

		// Token: 0x04007AFB RID: 31483
		public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State dead;

		// Token: 0x020025DA RID: 9690
		public class AliveStates : GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.PlantAliveSubState
		{
			// Token: 0x0400A88B RID: 43147
			public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State idle;

			// Token: 0x0400A88C RID: 43148
			public EvilFlower.States.WiltingState wilting;
		}

		// Token: 0x020025DB RID: 9691
		public class WiltingState : GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State
		{
			// Token: 0x0400A88D RID: 43149
			public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State wilting_pre;

			// Token: 0x0400A88E RID: 43150
			public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State wilting;

			// Token: 0x0400A88F RID: 43151
			public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State wilting_pst;
		}
	}
}
