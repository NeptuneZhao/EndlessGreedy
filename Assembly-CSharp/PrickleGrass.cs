using System;
using STRINGS;
using UnityEngine;

// Token: 0x020009F8 RID: 2552
public class PrickleGrass : StateMachineComponent<PrickleGrass.StatesInstance>
{
	// Token: 0x060049E7 RID: 18919 RVA: 0x001A68F3 File Offset: 0x001A4AF3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<PrickleGrass>(1309017699, PrickleGrass.SetReplantedTrueDelegate);
	}

	// Token: 0x060049E8 RID: 18920 RVA: 0x001A690C File Offset: 0x001A4B0C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060049E9 RID: 18921 RVA: 0x001A691F File Offset: 0x001A4B1F
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x04003074 RID: 12404
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04003075 RID: 12405
	[MyCmpReq]
	private EntombVulnerable entombVulnerable;

	// Token: 0x04003076 RID: 12406
	public bool replanted;

	// Token: 0x04003077 RID: 12407
	public EffectorValues positive_decor_effect = new EffectorValues
	{
		amount = 1,
		radius = 5
	};

	// Token: 0x04003078 RID: 12408
	public EffectorValues negative_decor_effect = new EffectorValues
	{
		amount = -1,
		radius = 5
	};

	// Token: 0x04003079 RID: 12409
	private static readonly EventSystem.IntraObjectHandler<PrickleGrass> SetReplantedTrueDelegate = new EventSystem.IntraObjectHandler<PrickleGrass>(delegate(PrickleGrass component, object data)
	{
		component.replanted = true;
	});

	// Token: 0x02001A0E RID: 6670
	public class StatesInstance : GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.GameInstance
	{
		// Token: 0x06009EFB RID: 40699 RVA: 0x0037ACC7 File Offset: 0x00378EC7
		public StatesInstance(PrickleGrass smi) : base(smi)
		{
		}
	}

	// Token: 0x02001A0F RID: 6671
	public class States : GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass>
	{
		// Token: 0x06009EFC RID: 40700 RVA: 0x0037ACD0 File Offset: 0x00378ED0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grow;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).ToggleTag(GameTags.PreventEmittingDisease).Enter(delegate(PrickleGrass.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (PrickleGrass.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (PrickleGrass.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (PrickleGrass.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.AreaElementSafeChanged, this.alive, (PrickleGrass.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(PrickleGrass.StatesInstance smi)
			{
				if (smi.master.replanted && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
				}
			}).PlayAnim("grow_seed", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive, null);
			GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State state2 = this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.idle);
			string name2 = CREATURES.STATUSITEMS.IDLE.NAME;
			string tooltip2 = CREATURES.STATUSITEMS.IDLE.TOOLTIP;
			string icon2 = "";
			StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
			NotificationType notification_type2 = NotificationType.Neutral;
			bool allow_multiples2 = false;
			main = Db.Get().StatusItemCategories.Main;
			state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main);
			this.alive.idle.EventTransition(GameHashes.Wilt, this.alive.wilting, (PrickleGrass.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).PlayAnim("idle", KAnim.PlayMode.Loop).Enter(delegate(PrickleGrass.StatesInstance smi)
			{
				smi.master.GetComponent<DecorProvider>().SetValues(smi.master.positive_decor_effect);
				smi.master.GetComponent<DecorProvider>().Refresh();
				smi.master.AddTag(GameTags.Decoration);
			});
			this.alive.wilting.PlayAnim("wilt1", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.idle, null).ToggleTag(GameTags.PreventEmittingDisease).Enter(delegate(PrickleGrass.StatesInstance smi)
			{
				smi.master.GetComponent<DecorProvider>().SetValues(smi.master.negative_decor_effect);
				smi.master.GetComponent<DecorProvider>().Refresh();
				smi.master.RemoveTag(GameTags.Decoration);
			});
		}

		// Token: 0x04007B29 RID: 31529
		public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State grow;

		// Token: 0x04007B2A RID: 31530
		public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State blocked_from_growing;

		// Token: 0x04007B2B RID: 31531
		public PrickleGrass.States.AliveStates alive;

		// Token: 0x04007B2C RID: 31532
		public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State dead;

		// Token: 0x020025E6 RID: 9702
		public class AliveStates : GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.PlantAliveSubState
		{
			// Token: 0x0400A8BA RID: 43194
			public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State idle;

			// Token: 0x0400A8BB RID: 43195
			public PrickleGrass.States.WiltingState wilting;
		}

		// Token: 0x020025E7 RID: 9703
		public class WiltingState : GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State
		{
			// Token: 0x0400A8BC RID: 43196
			public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State wilting_pre;

			// Token: 0x0400A8BD RID: 43197
			public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State wilting;

			// Token: 0x0400A8BE RID: 43198
			public GameStateMachine<PrickleGrass.States, PrickleGrass.StatesInstance, PrickleGrass, object>.State wilting_pst;
		}
	}
}
