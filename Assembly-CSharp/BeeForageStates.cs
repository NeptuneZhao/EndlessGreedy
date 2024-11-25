﻿using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000BC RID: 188
public class BeeForageStates : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>
{
	// Token: 0x06000354 RID: 852 RVA: 0x0001BD68 File Offset: 0x00019F68
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.collect.findTarget;
		GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.FORAGINGMATERIAL.NAME;
		string tooltip = CREATURES.STATUSITEMS.FORAGINGMATERIAL.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Exit(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(BeeForageStates.UnreserveTarget)).Exit(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(BeeForageStates.DropAll));
		this.collect.findTarget.Enter(delegate(BeeForageStates.Instance smi)
		{
			BeeForageStates.FindTarget(smi);
			smi.targetHive = smi.master.GetComponent<Bee>().FindHiveInRoom();
			if (smi.targetHive != null)
			{
				if (smi.forageTarget != null)
				{
					BeeForageStates.ReserveTarget(smi);
					smi.GoTo(this.collect.forage.moveToTarget);
					return;
				}
				if (Grid.IsValidCell(smi.targetMiningCell))
				{
					smi.GoTo(this.collect.mine.moveToTarget);
					return;
				}
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.collect.forage.moveToTarget.MoveTo(new Func<BeeForageStates.Instance, int>(BeeForageStates.GetOreCell), this.collect.forage.pickupTarget, this.behaviourcomplete, false);
		this.collect.forage.pickupTarget.PlayAnim("pickup_pre").Enter(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(BeeForageStates.PickupComplete)).OnAnimQueueComplete(this.storage.moveToHive);
		this.collect.mine.moveToTarget.MoveTo((BeeForageStates.Instance smi) => smi.targetMiningCell, this.collect.mine.mineTarget, this.behaviourcomplete, false);
		this.collect.mine.mineTarget.PlayAnim("mining_pre").QueueAnim("mining_loop", false, null).QueueAnim("mining_pst", false, null).Enter(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(BeeForageStates.MineTarget)).OnAnimQueueComplete(this.storage.moveToHive);
		this.storage.Enter(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(this.HoldOre)).Exit(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(this.DropOre));
		this.storage.moveToHive.Enter(delegate(BeeForageStates.Instance smi)
		{
			if (!smi.targetHive)
			{
				smi.targetHive = smi.master.GetComponent<Bee>().FindHiveInRoom();
			}
			if (!smi.targetHive)
			{
				smi.GoTo(this.storage.dropMaterial);
			}
		}).MoveTo((BeeForageStates.Instance smi) => Grid.OffsetCell(Grid.PosToCell(smi.targetHive.transform.GetPosition()), smi.hiveCellOffset), this.storage.storeMaterial, this.behaviourcomplete, false);
		this.storage.storeMaterial.PlayAnim("deposit").Exit(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(BeeForageStates.StoreOre)).OnAnimQueueComplete(this.behaviourcomplete.pre);
		this.storage.dropMaterial.Enter(delegate(BeeForageStates.Instance smi)
		{
			smi.GoTo(this.behaviourcomplete);
		}).Exit(new StateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State.Callback(BeeForageStates.DropAll));
		this.behaviourcomplete.DefaultState(this.behaviourcomplete.pst);
		this.behaviourcomplete.pre.PlayAnim("spawn").OnAnimQueueComplete(this.behaviourcomplete.pst);
		this.behaviourcomplete.pst.BehaviourComplete(GameTags.Creatures.WantsToForage, false);
	}

	// Token: 0x06000355 RID: 853 RVA: 0x0001C054 File Offset: 0x0001A254
	private static void FindTarget(BeeForageStates.Instance smi)
	{
		if (BeeForageStates.FindOre(smi))
		{
			return;
		}
		BeeForageStates.FindMineableCell(smi);
	}

	// Token: 0x06000356 RID: 854 RVA: 0x0001C068 File Offset: 0x0001A268
	private void HoldOre(BeeForageStates.Instance smi)
	{
		GameObject gameObject = smi.GetComponent<Storage>().FindFirst(smi.def.oreTag);
		if (!gameObject)
		{
			return;
		}
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		KAnim.Build.Symbol source_symbol = gameObject.GetComponent<KBatchedAnimController>().CurrentAnim.animFile.build.symbols[0];
		component.GetComponent<SymbolOverrideController>().AddSymbolOverride(smi.oreSymbolHash, source_symbol, 5);
		component.SetSymbolVisiblity(smi.oreSymbolHash, true);
		component.SetSymbolVisiblity(smi.oreLegSymbolHash, true);
		component.SetSymbolVisiblity(smi.noOreLegSymbolHash, false);
	}

	// Token: 0x06000357 RID: 855 RVA: 0x0001C0F7 File Offset: 0x0001A2F7
	private void DropOre(BeeForageStates.Instance smi)
	{
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		component.SetSymbolVisiblity(smi.oreSymbolHash, false);
		component.SetSymbolVisiblity(smi.oreLegSymbolHash, false);
		component.SetSymbolVisiblity(smi.noOreLegSymbolHash, true);
	}

	// Token: 0x06000358 RID: 856 RVA: 0x0001C128 File Offset: 0x0001A328
	private static void PickupComplete(BeeForageStates.Instance smi)
	{
		if (!smi.forageTarget)
		{
			global::Debug.LogWarningFormat("PickupComplete forageTarget {0} is null", new object[]
			{
				smi.forageTarget
			});
			return;
		}
		BeeForageStates.UnreserveTarget(smi);
		int num = Grid.PosToCell(smi.forageTarget);
		if (smi.forageTarget_cell != num)
		{
			global::Debug.LogWarningFormat("PickupComplete forageTarget {0} moved {1} != {2}", new object[]
			{
				smi.forageTarget,
				num,
				smi.forageTarget_cell
			});
			smi.forageTarget = null;
			return;
		}
		if (smi.forageTarget.HasTag(GameTags.Stored))
		{
			global::Debug.LogWarningFormat("PickupComplete forageTarget {0} was stored by {1}", new object[]
			{
				smi.forageTarget,
				smi.forageTarget.storage
			});
			smi.forageTarget = null;
			return;
		}
		smi.forageTarget = EntitySplitter.Split(smi.forageTarget, 10f, null);
		smi.GetComponent<Storage>().Store(smi.forageTarget.gameObject, false, false, true, false);
	}

	// Token: 0x06000359 RID: 857 RVA: 0x0001C224 File Offset: 0x0001A424
	private static void MineTarget(BeeForageStates.Instance smi)
	{
		Storage storage = smi.master.GetComponent<Storage>();
		HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(delegate(Sim.MassConsumedCallback mass_cb_info, object data)
		{
			if (mass_cb_info.mass > 0f)
			{
				storage.AddOre(ElementLoader.elements[(int)mass_cb_info.elemIdx].id, mass_cb_info.mass, mass_cb_info.temperature, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount, false, true);
			}
		}, null, "BeetaMine");
		SimMessages.ConsumeMass(smi.cellToMine, Grid.Element[smi.cellToMine].id, smi.def.amountToMine, 1, handle.index);
	}

	// Token: 0x0600035A RID: 858 RVA: 0x0001C29C File Offset: 0x0001A49C
	private static void StoreOre(BeeForageStates.Instance smi)
	{
		if (smi.targetHive.IsNullOrDestroyed())
		{
			smi.GoTo(smi.sm.storage.dropMaterial);
		}
		else
		{
			smi.master.GetComponent<Storage>().Transfer(smi.targetHive.GetComponent<Storage>(), false, false);
		}
		smi.forageTarget = null;
		smi.forageTarget_cell = Grid.InvalidCell;
		smi.targetHive = null;
	}

	// Token: 0x0600035B RID: 859 RVA: 0x0001C304 File Offset: 0x0001A504
	private static void DropAll(BeeForageStates.Instance smi)
	{
		smi.GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
	}

	// Token: 0x0600035C RID: 860 RVA: 0x0001C32C File Offset: 0x0001A52C
	private static bool FindMineableCell(BeeForageStates.Instance smi)
	{
		smi.targetMiningCell = Grid.InvalidCell;
		MineableCellQuery mineableCellQuery = PathFinderQueries.mineableCellQuery.Reset(smi.def.oreTag, 20);
		smi.GetComponent<Navigator>().RunQuery(mineableCellQuery);
		if (mineableCellQuery.result_cells.Count > 0)
		{
			smi.targetMiningCell = mineableCellQuery.result_cells[UnityEngine.Random.Range(0, mineableCellQuery.result_cells.Count)];
			foreach (Direction d in MineableCellQuery.DIRECTION_CHECKS)
			{
				int cellInDirection = Grid.GetCellInDirection(smi.targetMiningCell, d);
				if (Grid.IsValidCell(cellInDirection) && Grid.IsSolidCell(cellInDirection) && Grid.Element[cellInDirection].tag == smi.def.oreTag)
				{
					smi.cellToMine = cellInDirection;
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x0600035D RID: 861 RVA: 0x0001C424 File Offset: 0x0001A624
	private static bool FindOre(BeeForageStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		Vector3 position = smi.transform.GetPosition();
		Pickupable forageTarget = null;
		int num = 100;
		Extents extents = new Extents((int)position.x, (int)position.y, 15);
		ListPool<ScenePartitionerEntry, BeeForageStates>.PooledList pooledList = ListPool<ScenePartitionerEntry, BeeForageStates>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		Element element = ElementLoader.GetElement(smi.def.oreTag);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			Pickupable pickupable = scenePartitionerEntry.obj as Pickupable;
			if (!(pickupable == null) && !(pickupable.PrimaryElement == null) && pickupable.PrimaryElement.Element == element && !pickupable.KPrefabID.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				int navigationCost = component.GetNavigationCost(Grid.PosToCell(pickupable));
				if (navigationCost != -1 && navigationCost < num)
				{
					forageTarget = pickupable;
					num = navigationCost;
				}
			}
		}
		pooledList.Recycle();
		smi.forageTarget = forageTarget;
		smi.forageTarget_cell = (smi.forageTarget ? Grid.PosToCell(smi.forageTarget) : Grid.InvalidCell);
		return smi.forageTarget != null;
	}

	// Token: 0x0600035E RID: 862 RVA: 0x0001C574 File Offset: 0x0001A774
	private static void ReserveTarget(BeeForageStates.Instance smi)
	{
		GameObject gameObject = smi.forageTarget ? smi.forageTarget.gameObject : null;
		if (gameObject != null)
		{
			DebugUtil.Assert(!gameObject.HasTag(GameTags.Creatures.ReservedByCreature));
			gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x0600035F RID: 863 RVA: 0x0001C5C4 File Offset: 0x0001A7C4
	private static void UnreserveTarget(BeeForageStates.Instance smi)
	{
		GameObject go = smi.forageTarget ? smi.forageTarget.gameObject : null;
		if (smi.forageTarget != null)
		{
			go.RemoveTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x06000360 RID: 864 RVA: 0x0001C606 File Offset: 0x0001A806
	private static int GetOreCell(BeeForageStates.Instance smi)
	{
		global::Debug.Assert(smi.forageTarget);
		global::Debug.Assert(smi.forageTarget_cell != Grid.InvalidCell);
		return smi.forageTarget_cell;
	}

	// Token: 0x0400025B RID: 603
	private const int MAX_NAVIGATE_DISTANCE = 100;

	// Token: 0x0400025C RID: 604
	private const string oreSymbol = "snapto_thing";

	// Token: 0x0400025D RID: 605
	private const string oreLegSymbol = "legBeeOre";

	// Token: 0x0400025E RID: 606
	private const string noOreLegSymbol = "legBeeNoOre";

	// Token: 0x0400025F RID: 607
	public BeeForageStates.CollectionBehaviourStates collect;

	// Token: 0x04000260 RID: 608
	public BeeForageStates.StorageBehaviourStates storage;

	// Token: 0x04000261 RID: 609
	public BeeForageStates.ExitStates behaviourcomplete;

	// Token: 0x02000FE9 RID: 4073
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06007ACE RID: 31438 RVA: 0x00302A57 File Offset: 0x00300C57
		public Def(Tag tag, float amount_to_mine)
		{
			this.oreTag = tag;
			this.amountToMine = amount_to_mine;
		}

		// Token: 0x04005BBE RID: 23486
		public Tag oreTag;

		// Token: 0x04005BBF RID: 23487
		public float amountToMine;
	}

	// Token: 0x02000FEA RID: 4074
	public new class Instance : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.GameInstance
	{
		// Token: 0x06007ACF RID: 31439 RVA: 0x00302A70 File Offset: 0x00300C70
		public Instance(Chore<BeeForageStates.Instance> chore, BeeForageStates.Def def) : base(chore, def)
		{
			this.oreSymbolHash = new KAnimHashedString("snapto_thing");
			this.oreLegSymbolHash = new KAnimHashedString("legBeeOre");
			this.noOreLegSymbolHash = new KAnimHashedString("legBeeNoOre");
			base.smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(base.smi.oreSymbolHash, false);
			base.smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(base.smi.oreLegSymbolHash, false);
			base.smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(base.smi.noOreLegSymbolHash, true);
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToForage);
		}

		// Token: 0x04005BC0 RID: 23488
		public int targetMiningCell = Grid.InvalidCell;

		// Token: 0x04005BC1 RID: 23489
		public int cellToMine = Grid.InvalidCell;

		// Token: 0x04005BC2 RID: 23490
		public Pickupable forageTarget;

		// Token: 0x04005BC3 RID: 23491
		public int forageTarget_cell = Grid.InvalidCell;

		// Token: 0x04005BC4 RID: 23492
		public KPrefabID targetHive;

		// Token: 0x04005BC5 RID: 23493
		public KAnimHashedString oreSymbolHash;

		// Token: 0x04005BC6 RID: 23494
		public KAnimHashedString oreLegSymbolHash;

		// Token: 0x04005BC7 RID: 23495
		public KAnimHashedString noOreLegSymbolHash;

		// Token: 0x04005BC8 RID: 23496
		public CellOffset hiveCellOffset = new CellOffset(1, 1);
	}

	// Token: 0x02000FEB RID: 4075
	public class ForageBehaviourStates : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State
	{
		// Token: 0x04005BC9 RID: 23497
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State moveToTarget;

		// Token: 0x04005BCA RID: 23498
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State pickupTarget;
	}

	// Token: 0x02000FEC RID: 4076
	public class MiningBehaviourStates : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State
	{
		// Token: 0x04005BCB RID: 23499
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State moveToTarget;

		// Token: 0x04005BCC RID: 23500
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State mineTarget;
	}

	// Token: 0x02000FED RID: 4077
	public class CollectionBehaviourStates : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State
	{
		// Token: 0x04005BCD RID: 23501
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State findTarget;

		// Token: 0x04005BCE RID: 23502
		public BeeForageStates.ForageBehaviourStates forage;

		// Token: 0x04005BCF RID: 23503
		public BeeForageStates.MiningBehaviourStates mine;
	}

	// Token: 0x02000FEE RID: 4078
	public class StorageBehaviourStates : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State
	{
		// Token: 0x04005BD0 RID: 23504
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State moveToHive;

		// Token: 0x04005BD1 RID: 23505
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State storeMaterial;

		// Token: 0x04005BD2 RID: 23506
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State dropMaterial;
	}

	// Token: 0x02000FEF RID: 4079
	public class ExitStates : GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State
	{
		// Token: 0x04005BD3 RID: 23507
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State pre;

		// Token: 0x04005BD4 RID: 23508
		public GameStateMachine<BeeForageStates, BeeForageStates.Instance, IStateMachineTarget, BeeForageStates.Def>.State pst;
	}
}
