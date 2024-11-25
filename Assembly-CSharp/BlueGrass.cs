using System;
using STRINGS;
using UnityEngine;

// Token: 0x020009EC RID: 2540
public class BlueGrass : StateMachineComponent<BlueGrass.StatesInstance>
{
	// Token: 0x06004998 RID: 18840 RVA: 0x001A56FC File Offset: 0x001A38FC
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06004999 RID: 18841 RVA: 0x001A5714 File Offset: 0x001A3914
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x0600499A RID: 18842 RVA: 0x001A5727 File Offset: 0x001A3927
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x0600499B RID: 18843 RVA: 0x001A572F File Offset: 0x001A392F
	protected override void OnPrefabInit()
	{
		base.Subscribe<BlueGrass>(1309017699, BlueGrass.OnReplantedDelegate);
		base.OnPrefabInit();
	}

	// Token: 0x0600499C RID: 18844 RVA: 0x001A5748 File Offset: 0x001A3948
	private void OnReplanted(object data = null)
	{
		this.SetConsumptionRate();
	}

	// Token: 0x0600499D RID: 18845 RVA: 0x001A5750 File Offset: 0x001A3950
	public void SetConsumptionRate()
	{
		if (this.receptacleMonitor.Replanted)
		{
			this.elementConsumer.consumptionRate = 0.002f;
			return;
		}
		this.elementConsumer.consumptionRate = 0.0005f;
	}

	// Token: 0x0400302A RID: 12330
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x0400302B RID: 12331
	[MyCmpReq]
	private ElementConsumer elementConsumer;

	// Token: 0x0400302C RID: 12332
	[MyCmpReq]
	private ReceptacleMonitor receptacleMonitor;

	// Token: 0x0400302D RID: 12333
	[MyCmpReq]
	private Growing growing;

	// Token: 0x0400302E RID: 12334
	private static readonly EventSystem.IntraObjectHandler<BlueGrass> OnReplantedDelegate = new EventSystem.IntraObjectHandler<BlueGrass>(delegate(BlueGrass component, object data)
	{
		component.OnReplanted(data);
	});

	// Token: 0x020019F1 RID: 6641
	public class StatesInstance : GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.GameInstance
	{
		// Token: 0x06009E79 RID: 40569 RVA: 0x00378142 File Offset: 0x00376342
		public StatesInstance(BlueGrass master) : base(master)
		{
		}
	}

	// Token: 0x020019F2 RID: 6642
	public class States : GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass>
	{
		// Token: 0x06009E7A RID: 40570 RVA: 0x0037814C File Offset: 0x0037634C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grow;
			GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter(delegate(BlueGrass.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (BlueGrass.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (BlueGrass.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (BlueGrass.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(BlueGrass.StatesInstance smi)
			{
				if (smi.master.receptacleMonitor.HasReceptacle() && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
					return;
				}
				smi.GoTo(this.alive);
			});
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.growing).Enter(delegate(BlueGrass.StatesInstance smi)
			{
				smi.master.SetConsumptionRate();
			});
			this.alive.growing.EventTransition(GameHashes.Wilt, this.alive.wilting, (BlueGrass.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).Enter(delegate(BlueGrass.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(true);
			}).Exit(delegate(BlueGrass.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(false);
			}).EventTransition(GameHashes.Grow, this.alive.fullygrown, (BlueGrass.StatesInstance smi) => smi.master.growing.IsGrown());
			this.alive.fullygrown.EventTransition(GameHashes.Wilt, this.alive.wilting, (BlueGrass.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).EventTransition(GameHashes.HarvestComplete, this.alive.growing, null);
			this.alive.wilting.EventTransition(GameHashes.WiltRecover, this.alive.growing, (BlueGrass.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
		}

		// Token: 0x04007AE4 RID: 31460
		public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State grow;

		// Token: 0x04007AE5 RID: 31461
		public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State blocked_from_growing;

		// Token: 0x04007AE6 RID: 31462
		public BlueGrass.States.AliveStates alive;

		// Token: 0x04007AE7 RID: 31463
		public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State dead;

		// Token: 0x020025D2 RID: 9682
		public class AliveStates : GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.PlantAliveSubState
		{
			// Token: 0x0400A85B RID: 43099
			public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State growing;

			// Token: 0x0400A85C RID: 43100
			public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State fullygrown;

			// Token: 0x0400A85D RID: 43101
			public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State wilting;
		}
	}
}
