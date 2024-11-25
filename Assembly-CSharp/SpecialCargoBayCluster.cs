using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003C7 RID: 967
public class SpecialCargoBayCluster : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>
{
	// Token: 0x0600141B RID: 5147 RVA: 0x0006E6F0 File Offset: 0x0006C8F0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.close;
		this.close.DefaultState(this.close.idle);
		this.close.closing.Target(this.Door).PlayAnim("close").OnAnimQueueComplete(this.close.idle).Target(this.masterTarget);
		this.close.idle.Target(this.Door).PlayAnim("close_idle").ParamTransition<bool>(this.IsDoorOpen, this.open.opening, GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.IsTrue).Target(this.masterTarget);
		this.close.cloud.Target(this.Door).PlayAnim("play_cloud").OnAnimQueueComplete(this.close.idle).Target(this.masterTarget);
		this.open.DefaultState(this.close.idle);
		this.open.opening.Target(this.Door).PlayAnim("open").OnAnimQueueComplete(this.open.idle).Target(this.masterTarget);
		this.open.idle.Target(this.Door).PlayAnim("open_idle").Enter(new StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State.Callback(SpecialCargoBayCluster.DropInventory)).Enter(new StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State.Callback(SpecialCargoBayCluster.CloseDoorAutomatically)).ParamTransition<bool>(this.IsDoorOpen, this.close.closing, GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.IsFalse).Target(this.masterTarget);
	}

	// Token: 0x0600141C RID: 5148 RVA: 0x0006E89E File Offset: 0x0006CA9E
	public static void CloseDoorAutomatically(SpecialCargoBayCluster.Instance smi)
	{
		smi.CloseDoorAutomatically();
	}

	// Token: 0x0600141D RID: 5149 RVA: 0x0006E8A6 File Offset: 0x0006CAA6
	public static void DropInventory(SpecialCargoBayCluster.Instance smi)
	{
		smi.DropInventory();
	}

	// Token: 0x04000B74 RID: 2932
	public const string DOOR_METER_TARGET_NAME = "fg_meter_target";

	// Token: 0x04000B75 RID: 2933
	public const string TRAPPED_CRITTER_PIVOT_SYMBOL_NAME = "critter";

	// Token: 0x04000B76 RID: 2934
	public const string LOOT_SYMBOL_NAME = "loot";

	// Token: 0x04000B77 RID: 2935
	public const string DEATH_CLOUD_ANIM_NAME = "play_cloud";

	// Token: 0x04000B78 RID: 2936
	private const string OPEN_DOOR_ANIM_NAME = "open";

	// Token: 0x04000B79 RID: 2937
	private const string CLOSE_DOOR_ANIM_NAME = "close";

	// Token: 0x04000B7A RID: 2938
	private const string OPEN_DOOR_IDLE_ANIM_NAME = "open_idle";

	// Token: 0x04000B7B RID: 2939
	private const string CLOSE_DOOR_IDLE_ANIM_NAME = "close_idle";

	// Token: 0x04000B7C RID: 2940
	public SpecialCargoBayCluster.OpenStates open;

	// Token: 0x04000B7D RID: 2941
	public SpecialCargoBayCluster.CloseStates close;

	// Token: 0x04000B7E RID: 2942
	public StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.BoolParameter IsDoorOpen;

	// Token: 0x04000B7F RID: 2943
	public StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.TargetParameter Door;

	// Token: 0x02001150 RID: 4432
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005FBC RID: 24508
		public Vector2 trappedOffset = new Vector2(0f, -0.3f);
	}

	// Token: 0x02001151 RID: 4433
	public class OpenStates : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State
	{
		// Token: 0x04005FBD RID: 24509
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State opening;

		// Token: 0x04005FBE RID: 24510
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State idle;
	}

	// Token: 0x02001152 RID: 4434
	public class CloseStates : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State
	{
		// Token: 0x04005FBF RID: 24511
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State closing;

		// Token: 0x04005FC0 RID: 24512
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State idle;

		// Token: 0x04005FC1 RID: 24513
		public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State cloud;
	}

	// Token: 0x02001153 RID: 4435
	public new class Instance : GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.GameInstance
	{
		// Token: 0x06007F20 RID: 32544 RVA: 0x0030C049 File Offset: 0x0030A249
		public void PlayDeathCloud()
		{
			if (base.IsInsideState(base.sm.close.idle))
			{
				this.GoTo(base.sm.close.cloud);
			}
		}

		// Token: 0x06007F21 RID: 32545 RVA: 0x0030C079 File Offset: 0x0030A279
		public void CloseDoor()
		{
			base.sm.IsDoorOpen.Set(false, base.smi, false);
		}

		// Token: 0x06007F22 RID: 32546 RVA: 0x0030C094 File Offset: 0x0030A294
		public void OpenDoor()
		{
			base.sm.IsDoorOpen.Set(true, base.smi, false);
		}

		// Token: 0x06007F23 RID: 32547 RVA: 0x0030C0B0 File Offset: 0x0030A2B0
		public Instance(IStateMachineTarget master, SpecialCargoBayCluster.Def def) : base(master, def)
		{
			this.buildingAnimController = base.GetComponent<KBatchedAnimController>();
			this.doorMeter = new MeterController(this.buildingAnimController, "fg_meter_target", "close_idle", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
			this.doorAnimController = this.doorMeter.meterController;
			KBatchedAnimTracker componentInChildren = this.doorAnimController.GetComponentInChildren<KBatchedAnimTracker>();
			componentInChildren.forceAlwaysAlive = true;
			componentInChildren.matchParentOffset = true;
			base.sm.Door.Set(this.doorAnimController.gameObject, base.smi, false);
			Storage[] components = base.gameObject.GetComponents<Storage>();
			this.critterStorage = components[0];
			this.sideProductStorage = components[1];
			base.Subscribe(1655598572, new Action<object>(this.OnLaunchConditionChanged));
		}

		// Token: 0x06007F24 RID: 32548 RVA: 0x0030C175 File Offset: 0x0030A375
		public void CloseDoorAutomatically()
		{
			this.CloseDoor();
		}

		// Token: 0x06007F25 RID: 32549 RVA: 0x0030C17D File Offset: 0x0030A37D
		public override void StartSM()
		{
			base.StartSM();
		}

		// Token: 0x06007F26 RID: 32550 RVA: 0x0030C188 File Offset: 0x0030A388
		private void OnLaunchConditionChanged(object obj)
		{
			if (this.rocketModuleCluster.CraftInterface != null)
			{
				Clustercraft component = this.rocketModuleCluster.CraftInterface.GetComponent<Clustercraft>();
				if (component != null && component.Status == Clustercraft.CraftStatus.Launching)
				{
					this.CloseDoor();
				}
			}
		}

		// Token: 0x06007F27 RID: 32551 RVA: 0x0030C1D4 File Offset: 0x0030A3D4
		public void DropInventory()
		{
			List<GameObject> list = new List<GameObject>();
			List<GameObject> list2 = new List<GameObject>();
			foreach (GameObject gameObject in this.critterStorage.items)
			{
				if (gameObject != null)
				{
					Baggable component = gameObject.GetComponent<Baggable>();
					if (component != null)
					{
						component.keepWrangledNextTimeRemovedFromStorage = true;
					}
				}
			}
			Storage storage = this.critterStorage;
			bool vent_gas = false;
			bool dump_liquid = false;
			List<GameObject> collect_dropped_items = list;
			storage.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
			Storage storage2 = this.sideProductStorage;
			bool vent_gas2 = false;
			bool dump_liquid2 = false;
			collect_dropped_items = list2;
			storage2.DropAll(vent_gas2, dump_liquid2, default(Vector3), true, collect_dropped_items);
			foreach (GameObject gameObject2 in list)
			{
				KBatchedAnimController component2 = gameObject2.GetComponent<KBatchedAnimController>();
				Vector3 storePositionForCritter = this.GetStorePositionForCritter(gameObject2);
				gameObject2.transform.SetPosition(storePositionForCritter);
				component2.SetSceneLayer(Grid.SceneLayer.Creatures);
				component2.Play("trussed", KAnim.PlayMode.Loop, 1f, 0f);
			}
			foreach (GameObject gameObject3 in list2)
			{
				KBatchedAnimController component3 = gameObject3.GetComponent<KBatchedAnimController>();
				Vector3 storePositionForDrops = this.GetStorePositionForDrops();
				gameObject3.transform.SetPosition(storePositionForDrops);
				component3.SetSceneLayer(Grid.SceneLayer.Ore);
			}
		}

		// Token: 0x06007F28 RID: 32552 RVA: 0x0030C364 File Offset: 0x0030A564
		public Vector3 GetCritterPositionOffet(GameObject critter)
		{
			KBatchedAnimController component = critter.GetComponent<KBatchedAnimController>();
			Vector3 zero = Vector3.zero;
			zero.x = base.def.trappedOffset.x - component.Offset.x;
			zero.y = base.def.trappedOffset.y - component.Offset.y;
			return zero;
		}

		// Token: 0x06007F29 RID: 32553 RVA: 0x0030C3C8 File Offset: 0x0030A5C8
		public Vector3 GetStorePositionForCritter(GameObject critter)
		{
			Vector3 critterPositionOffet = this.GetCritterPositionOffet(critter);
			bool flag;
			return this.buildingAnimController.GetSymbolTransform("critter", out flag).GetColumn(3) + critterPositionOffet;
		}

		// Token: 0x06007F2A RID: 32554 RVA: 0x0030C408 File Offset: 0x0030A608
		public Vector3 GetStorePositionForDrops()
		{
			bool flag;
			return this.buildingAnimController.GetSymbolTransform("loot", out flag).GetColumn(3);
		}

		// Token: 0x04005FC2 RID: 24514
		public MeterController doorMeter;

		// Token: 0x04005FC3 RID: 24515
		private Storage critterStorage;

		// Token: 0x04005FC4 RID: 24516
		private Storage sideProductStorage;

		// Token: 0x04005FC5 RID: 24517
		private KBatchedAnimController buildingAnimController;

		// Token: 0x04005FC6 RID: 24518
		private KBatchedAnimController doorAnimController;

		// Token: 0x04005FC7 RID: 24519
		[MyCmpGet]
		private RocketModuleCluster rocketModuleCluster;
	}
}
