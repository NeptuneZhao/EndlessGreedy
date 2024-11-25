using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x020006AF RID: 1711
public class CreatureDeliveryPoint : StateMachineComponent<CreatureDeliveryPoint.SMInstance>
{
	// Token: 0x06002B13 RID: 11027 RVA: 0x000F1F34 File Offset: 0x000F0134
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.fetches = new List<FetchOrder2>();
		TreeFilterable component = base.GetComponent<TreeFilterable>();
		component.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(component.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
		base.GetComponent<Storage>().SetOffsets(this.deliveryOffsets);
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x06002B14 RID: 11028 RVA: 0x000F1F98 File Offset: 0x000F0198
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		base.Subscribe<CreatureDeliveryPoint>(-905833192, CreatureDeliveryPoint.OnCopySettingsDelegate);
		base.Subscribe<CreatureDeliveryPoint>(643180843, CreatureDeliveryPoint.RefreshCreatureCountDelegate);
		this.critterCapacity = base.GetComponent<BaggableCritterCapacityTracker>();
		BaggableCritterCapacityTracker baggableCritterCapacityTracker = this.critterCapacity;
		baggableCritterCapacityTracker.onCountChanged = (System.Action)Delegate.Combine(baggableCritterCapacityTracker.onCountChanged, new System.Action(this.RebalanceFetches));
		this.critterCapacity.RefreshCreatureCount(null);
		this.logicPorts = base.GetComponent<LogicPorts>();
		if (this.logicPorts != null)
		{
			this.logicPorts.Subscribe(-801688580, new Action<object>(this.OnLogicChanged));
		}
	}

	// Token: 0x06002B15 RID: 11029 RVA: 0x000F2050 File Offset: 0x000F0250
	private void OnLogicChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == "CritterDropOffInput")
		{
			if (logicValueChanged.newValue > 0)
			{
				this.RebalanceFetches();
				return;
			}
			this.ClearFetches();
		}
	}

	// Token: 0x06002B16 RID: 11030 RVA: 0x000F2091 File Offset: 0x000F0291
	[Obsolete]
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.critterCapacity != null && this.creatureLimit > 0)
		{
			this.critterCapacity.creatureLimit = this.creatureLimit;
			this.creatureLimit = -1;
		}
	}

	// Token: 0x06002B17 RID: 11031 RVA: 0x000F20C4 File Offset: 0x000F02C4
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		if (gameObject.GetComponent<CreatureDeliveryPoint>() == null)
		{
			return;
		}
		this.RebalanceFetches();
	}

	// Token: 0x06002B18 RID: 11032 RVA: 0x000F20F7 File Offset: 0x000F02F7
	private void OnFilterChanged(HashSet<Tag> tags)
	{
		this.ClearFetches();
		this.RebalanceFetches();
	}

	// Token: 0x06002B19 RID: 11033 RVA: 0x000F2108 File Offset: 0x000F0308
	private void ClearFetches()
	{
		for (int i = this.fetches.Count - 1; i >= 0; i--)
		{
			this.fetches[i].Cancel("clearing all fetches");
		}
		this.fetches.Clear();
	}

	// Token: 0x06002B1A RID: 11034 RVA: 0x000F2150 File Offset: 0x000F0350
	private void RebalanceFetches()
	{
		if (!this.LogicEnabled())
		{
			return;
		}
		HashSet<Tag> tags = base.GetComponent<TreeFilterable>().GetTags();
		ChoreType creatureFetch = Db.Get().ChoreTypes.CreatureFetch;
		Storage component = base.GetComponent<Storage>();
		int num = this.critterCapacity.creatureLimit - this.critterCapacity.storedCreatureCount;
		int count = this.fetches.Count;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		for (int i = this.fetches.Count - 1; i >= 0; i--)
		{
			if (this.fetches[i].IsComplete())
			{
				this.fetches.RemoveAt(i);
				num2++;
			}
		}
		int num6 = 0;
		for (int j = 0; j < this.fetches.Count; j++)
		{
			if (!this.fetches[j].InProgress)
			{
				num6++;
			}
		}
		if (num6 == 0 && this.fetches.Count < num)
		{
			FetchOrder2 fetchOrder = new FetchOrder2(creatureFetch, tags, FetchChore.MatchCriteria.MatchID, GameTags.Creatures.Deliverable, null, component, 1f, Operational.State.Operational, 0);
			fetchOrder.validateRequiredTagOnTagChange = true;
			fetchOrder.Submit(new Action<FetchOrder2, Pickupable>(this.OnFetchComplete), false, new Action<FetchOrder2, Pickupable>(this.OnFetchBegun));
			this.fetches.Add(fetchOrder);
			num3++;
		}
		int num7 = this.fetches.Count - num;
		for (int k = this.fetches.Count - 1; k >= 0; k--)
		{
			if (num7 <= 0)
			{
				break;
			}
			if (!this.fetches[k].InProgress)
			{
				this.fetches[k].Cancel("fewer creatures in room");
				this.fetches.RemoveAt(k);
				num7--;
				num4++;
			}
		}
		while (num7 > 0 && this.fetches.Count > 0)
		{
			this.fetches[this.fetches.Count - 1].Cancel("fewer creatures in room");
			this.fetches.RemoveAt(this.fetches.Count - 1);
			num7--;
			num5++;
		}
	}

	// Token: 0x06002B1B RID: 11035 RVA: 0x000F2367 File Offset: 0x000F0567
	private void OnFetchComplete(FetchOrder2 fetchOrder, Pickupable fetchedItem)
	{
		this.RebalanceFetches();
	}

	// Token: 0x06002B1C RID: 11036 RVA: 0x000F236F File Offset: 0x000F056F
	private void OnFetchBegun(FetchOrder2 fetchOrder, Pickupable fetchedItem)
	{
		this.RebalanceFetches();
	}

	// Token: 0x06002B1D RID: 11037 RVA: 0x000F2377 File Offset: 0x000F0577
	protected override void OnCleanUp()
	{
		base.smi.StopSM("OnCleanUp");
		TreeFilterable component = base.GetComponent<TreeFilterable>();
		component.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Remove(component.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
		base.OnCleanUp();
	}

	// Token: 0x06002B1E RID: 11038 RVA: 0x000F23B8 File Offset: 0x000F05B8
	public bool LogicEnabled()
	{
		return this.logicPorts == null || !this.logicPorts.IsPortConnected("CritterDropOffInput") || this.logicPorts.GetInputValue("CritterDropOffInput") == 1;
	}

	// Token: 0x040018B8 RID: 6328
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x040018B9 RID: 6329
	[MyCmpReq]
	public BaggableCritterCapacityTracker critterCapacity;

	// Token: 0x040018BA RID: 6330
	[Obsolete]
	[Serialize]
	private int creatureLimit = 20;

	// Token: 0x040018BB RID: 6331
	public CellOffset[] deliveryOffsets = new CellOffset[1];

	// Token: 0x040018BC RID: 6332
	public CellOffset spawnOffset = new CellOffset(0, 0);

	// Token: 0x040018BD RID: 6333
	private List<FetchOrder2> fetches;

	// Token: 0x040018BE RID: 6334
	public bool playAnimsOnFetch;

	// Token: 0x040018BF RID: 6335
	private LogicPorts logicPorts;

	// Token: 0x040018C0 RID: 6336
	private static readonly EventSystem.IntraObjectHandler<CreatureDeliveryPoint> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<CreatureDeliveryPoint>(delegate(CreatureDeliveryPoint component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x040018C1 RID: 6337
	private static readonly EventSystem.IntraObjectHandler<CreatureDeliveryPoint> RefreshCreatureCountDelegate = new EventSystem.IntraObjectHandler<CreatureDeliveryPoint>(delegate(CreatureDeliveryPoint component, object data)
	{
		component.critterCapacity.RefreshCreatureCount(data);
	});

	// Token: 0x020014A1 RID: 5281
	public class SMInstance : GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.GameInstance
	{
		// Token: 0x06008B99 RID: 35737 RVA: 0x003373DA File Offset: 0x003355DA
		public SMInstance(CreatureDeliveryPoint master) : base(master)
		{
		}
	}

	// Token: 0x020014A2 RID: 5282
	public class States : GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint>
	{
		// Token: 0x06008B9A RID: 35738 RVA: 0x003373E4 File Offset: 0x003355E4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.operational.waiting;
			this.root.Update("RefreshCreatureCount", delegate(CreatureDeliveryPoint.SMInstance smi, float dt)
			{
				smi.master.critterCapacity.RefreshCreatureCount(null);
			}, UpdateRate.SIM_1000ms, false).EventHandler(GameHashes.OnStorageChange, new StateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State.Callback(CreatureDeliveryPoint.States.DropAllCreatures));
			this.unoperational.EventTransition(GameHashes.LogicEvent, this.operational, (CreatureDeliveryPoint.SMInstance smi) => smi.master.LogicEnabled());
			this.operational.EventTransition(GameHashes.LogicEvent, this.unoperational, (CreatureDeliveryPoint.SMInstance smi) => !smi.master.LogicEnabled());
			this.operational.waiting.EnterTransition(this.operational.interact_waiting, (CreatureDeliveryPoint.SMInstance smi) => smi.master.playAnimsOnFetch);
			this.operational.interact_waiting.WorkableStartTransition((CreatureDeliveryPoint.SMInstance smi) => smi.master.GetComponent<Storage>(), this.operational.interact_delivery);
			this.operational.interact_delivery.PlayAnim("working_pre").QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.operational.interact_waiting);
		}

		// Token: 0x06008B9B RID: 35739 RVA: 0x0033755C File Offset: 0x0033575C
		public static void DropAllCreatures(CreatureDeliveryPoint.SMInstance smi)
		{
			Storage component = smi.master.GetComponent<Storage>();
			if (component.IsEmpty())
			{
				return;
			}
			List<GameObject> items = component.items;
			int count = items.Count;
			Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(smi.transform.GetPosition()), smi.master.spawnOffset), Grid.SceneLayer.Creatures);
			for (int i = count - 1; i >= 0; i--)
			{
				GameObject gameObject = items[i];
				component.Drop(gameObject, true);
				gameObject.transform.SetPosition(position);
				gameObject.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
			}
			smi.master.critterCapacity.RefreshCreatureCount(null);
		}

		// Token: 0x04006A82 RID: 27266
		public CreatureDeliveryPoint.States.OperationalState operational;

		// Token: 0x04006A83 RID: 27267
		public GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State unoperational;

		// Token: 0x020024C8 RID: 9416
		public class OperationalState : GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State
		{
			// Token: 0x0400A336 RID: 41782
			public GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State waiting;

			// Token: 0x0400A337 RID: 41783
			public GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State interact_waiting;

			// Token: 0x0400A338 RID: 41784
			public GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State interact_delivery;
		}
	}
}
