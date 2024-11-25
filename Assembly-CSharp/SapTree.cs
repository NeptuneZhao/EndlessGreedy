using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020009FA RID: 2554
public class SapTree : GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>
{
	// Token: 0x060049F1 RID: 18929 RVA: 0x001A6A34 File Offset: 0x001A4C34
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.alive;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State state = this.dead;
		string name = CREATURES.STATUSITEMS.DEAD.NAME;
		string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).ToggleTag(GameTags.PreventEmittingDisease).Enter(delegate(SapTree.StatesInstance smi)
		{
			GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
			smi.master.Trigger(1623392196, null);
			smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
		});
		this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.normal);
		this.alive.normal.DefaultState(this.alive.normal.idle).EventTransition(GameHashes.Wilt, this.alive.wilting, (SapTree.StatesInstance smi) => smi.wiltCondition.IsWilting()).Update(delegate(SapTree.StatesInstance smi, float dt)
		{
			smi.CheckForFood();
		}, UpdateRate.SIM_1000ms, false);
		GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State state2 = this.alive.normal.idle.PlayAnim("idle", KAnim.PlayMode.Loop);
		string name2 = CREATURES.STATUSITEMS.IDLE.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.IDLE.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main).ParamTransition<bool>(this.hasNearbyEnemy, this.alive.normal.attacking_pre, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsTrue).ParamTransition<float>(this.storedSap, this.alive.normal.oozing, (SapTree.StatesInstance smi, float p) => p >= smi.def.stomachSize).ParamTransition<GameObject>(this.foodItem, this.alive.normal.eating, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsNotNull);
		this.alive.normal.eating.PlayAnim("eat_pre", KAnim.PlayMode.Once).QueueAnim("eat_loop", true, null).Update(delegate(SapTree.StatesInstance smi, float dt)
		{
			smi.EatFoodItem(dt);
		}, UpdateRate.SIM_1000ms, false).ParamTransition<GameObject>(this.foodItem, this.alive.normal.eating_pst, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsNull).ParamTransition<float>(this.storedSap, this.alive.normal.eating_pst, (SapTree.StatesInstance smi, float p) => p >= smi.def.stomachSize);
		this.alive.normal.eating_pst.PlayAnim("eat_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.normal.idle);
		this.alive.normal.oozing.PlayAnim("ooze_pre", KAnim.PlayMode.Once).QueueAnim("ooze_loop", true, null).Update(delegate(SapTree.StatesInstance smi, float dt)
		{
			smi.Ooze(dt);
		}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.storedSap, this.alive.normal.oozing_pst, (SapTree.StatesInstance smi, float p) => p <= 0f).ParamTransition<bool>(this.hasNearbyEnemy, this.alive.normal.oozing_pst, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsTrue);
		this.alive.normal.oozing_pst.PlayAnim("ooze_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.normal.idle);
		this.alive.normal.attacking_pre.PlayAnim("attacking_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.normal.attacking);
		this.alive.normal.attacking.PlayAnim("attacking_loop", KAnim.PlayMode.Once).Enter(delegate(SapTree.StatesInstance smi)
		{
			smi.DoAttack();
		}).OnAnimQueueComplete(this.alive.normal.attacking_cooldown);
		this.alive.normal.attacking_cooldown.PlayAnim("attacking_pst", KAnim.PlayMode.Once).QueueAnim("attack_cooldown", true, null).ParamTransition<bool>(this.hasNearbyEnemy, this.alive.normal.attacking_done, GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.IsFalse).ScheduleGoTo((SapTree.StatesInstance smi) => smi.def.attackCooldown, this.alive.normal.attacking);
		this.alive.normal.attacking_done.PlayAnim("attack_to_idle", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.normal.idle);
		this.alive.wilting.PlayAnim("withered", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.normal, null).ToggleTag(GameTags.PreventEmittingDisease);
	}

	// Token: 0x0400307C RID: 12412
	public SapTree.AliveStates alive;

	// Token: 0x0400307D RID: 12413
	public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State dead;

	// Token: 0x0400307E RID: 12414
	private StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.TargetParameter foodItem;

	// Token: 0x0400307F RID: 12415
	private StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.BoolParameter hasNearbyEnemy;

	// Token: 0x04003080 RID: 12416
	private StateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.FloatParameter storedSap;

	// Token: 0x02001A14 RID: 6676
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007B30 RID: 31536
		public Vector2I foodSenseArea;

		// Token: 0x04007B31 RID: 31537
		public float massEatRate;

		// Token: 0x04007B32 RID: 31538
		public float kcalorieToKGConversionRatio;

		// Token: 0x04007B33 RID: 31539
		public float stomachSize;

		// Token: 0x04007B34 RID: 31540
		public float oozeRate;

		// Token: 0x04007B35 RID: 31541
		public List<Vector3> oozeOffsets;

		// Token: 0x04007B36 RID: 31542
		public Vector2I attackSenseArea;

		// Token: 0x04007B37 RID: 31543
		public float attackCooldown;
	}

	// Token: 0x02001A15 RID: 6677
	public class AliveStates : GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.PlantAliveSubState
	{
		// Token: 0x04007B38 RID: 31544
		public SapTree.NormalStates normal;

		// Token: 0x04007B39 RID: 31545
		public SapTree.WiltingState wilting;
	}

	// Token: 0x02001A16 RID: 6678
	public class NormalStates : GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State
	{
		// Token: 0x04007B3A RID: 31546
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State idle;

		// Token: 0x04007B3B RID: 31547
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State eating;

		// Token: 0x04007B3C RID: 31548
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State eating_pst;

		// Token: 0x04007B3D RID: 31549
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State oozing;

		// Token: 0x04007B3E RID: 31550
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State oozing_pst;

		// Token: 0x04007B3F RID: 31551
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State attacking_pre;

		// Token: 0x04007B40 RID: 31552
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State attacking;

		// Token: 0x04007B41 RID: 31553
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State attacking_cooldown;

		// Token: 0x04007B42 RID: 31554
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State attacking_done;
	}

	// Token: 0x02001A17 RID: 6679
	public class WiltingState : GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State
	{
		// Token: 0x04007B43 RID: 31555
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State wilting_pre;

		// Token: 0x04007B44 RID: 31556
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State wilting;

		// Token: 0x04007B45 RID: 31557
		public GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.State wilting_pst;
	}

	// Token: 0x02001A18 RID: 6680
	public class StatesInstance : GameStateMachine<SapTree, SapTree.StatesInstance, IStateMachineTarget, SapTree.Def>.GameInstance
	{
		// Token: 0x06009F11 RID: 40721 RVA: 0x0037B094 File Offset: 0x00379294
		public StatesInstance(IStateMachineTarget master, SapTree.Def def) : base(master, def)
		{
			Vector2I vector2I = Grid.PosToXY(base.gameObject.transform.GetPosition());
			Vector2I vector2I2 = new Vector2I(vector2I.x - def.attackSenseArea.x / 2, vector2I.y);
			this.attackExtents = new Extents(vector2I2.x, vector2I2.y, def.attackSenseArea.x, def.attackSenseArea.y);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("SapTreeAttacker", this, this.attackExtents, GameScenePartitioner.Instance.objectLayers[0], new Action<object>(this.OnMinionChanged));
			Vector2I vector2I3 = new Vector2I(vector2I.x - def.foodSenseArea.x / 2, vector2I.y);
			this.feedExtents = new Extents(vector2I3.x, vector2I3.y, def.foodSenseArea.x, def.foodSenseArea.y);
		}

		// Token: 0x06009F12 RID: 40722 RVA: 0x0037B18F File Offset: 0x0037938F
		protected override void OnCleanUp()
		{
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		}

		// Token: 0x06009F13 RID: 40723 RVA: 0x0037B1A4 File Offset: 0x003793A4
		public void EatFoodItem(float dt)
		{
			Pickupable pickupable = base.sm.foodItem.Get(this).GetComponent<Pickupable>().Take(base.def.massEatRate * dt);
			if (pickupable != null)
			{
				float mass = pickupable.GetComponent<Edible>().Calories * 0.001f * base.def.kcalorieToKGConversionRatio;
				Util.KDestroyGameObject(pickupable.gameObject);
				PrimaryElement component = base.GetComponent<PrimaryElement>();
				this.storage.AddLiquid(SimHashes.Resin, mass, component.Temperature, byte.MaxValue, 0, true, false);
				base.sm.storedSap.Set(this.storage.GetMassAvailable(SimHashes.Resin.CreateTag()), this, false);
			}
		}

		// Token: 0x06009F14 RID: 40724 RVA: 0x0037B25C File Offset: 0x0037945C
		public void Ooze(float dt)
		{
			float num = Mathf.Min(base.sm.storedSap.Get(this), dt * base.def.oozeRate);
			if (num <= 0f)
			{
				return;
			}
			int index = Mathf.FloorToInt(GameClock.Instance.GetTime() % (float)base.def.oozeOffsets.Count);
			this.storage.DropSome(SimHashes.Resin.CreateTag(), num, false, true, base.def.oozeOffsets[index], true, false);
			base.sm.storedSap.Set(this.storage.GetMassAvailable(SimHashes.Resin.CreateTag()), this, false);
		}

		// Token: 0x06009F15 RID: 40725 RVA: 0x0037B30C File Offset: 0x0037950C
		public void CheckForFood()
		{
			ListPool<ScenePartitionerEntry, SapTree>.PooledList pooledList = ListPool<ScenePartitionerEntry, SapTree>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(this.feedExtents, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
			foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
			{
				Pickupable pickupable = scenePartitionerEntry.obj as Pickupable;
				if (pickupable.GetComponent<Edible>() != null)
				{
					base.sm.foodItem.Set(pickupable.gameObject, this, false);
					pooledList.Recycle();
					return;
				}
			}
			base.sm.foodItem.Set(null, this);
			pooledList.Recycle();
		}

		// Token: 0x06009F16 RID: 40726 RVA: 0x0037B3C8 File Offset: 0x003795C8
		public bool DoAttack()
		{
			int num = this.weapon.AttackArea(base.transform.GetPosition());
			base.sm.hasNearbyEnemy.Set(num > 0, this, false);
			return true;
		}

		// Token: 0x06009F17 RID: 40727 RVA: 0x0037B404 File Offset: 0x00379604
		private void OnMinionChanged(object obj)
		{
			if (obj as GameObject != null)
			{
				base.sm.hasNearbyEnemy.Set(true, this, false);
			}
		}

		// Token: 0x04007B46 RID: 31558
		[MyCmpReq]
		public WiltCondition wiltCondition;

		// Token: 0x04007B47 RID: 31559
		[MyCmpReq]
		public EntombVulnerable entombVulnerable;

		// Token: 0x04007B48 RID: 31560
		[MyCmpReq]
		private Storage storage;

		// Token: 0x04007B49 RID: 31561
		[MyCmpReq]
		private Weapon weapon;

		// Token: 0x04007B4A RID: 31562
		private HandleVector<int>.Handle partitionerEntry;

		// Token: 0x04007B4B RID: 31563
		private Extents feedExtents;

		// Token: 0x04007B4C RID: 31564
		private Extents attackExtents;
	}
}
