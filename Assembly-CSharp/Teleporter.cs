using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000781 RID: 1921
public class Teleporter : KMonoBehaviour
{
	// Token: 0x17000389 RID: 905
	// (get) Token: 0x06003432 RID: 13362 RVA: 0x0011CFD3 File Offset: 0x0011B1D3
	// (set) Token: 0x06003433 RID: 13363 RVA: 0x0011CFDB File Offset: 0x0011B1DB
	[Serialize]
	public int teleporterID { get; private set; }

	// Token: 0x06003434 RID: 13364 RVA: 0x0011CFE4 File Offset: 0x0011B1E4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06003435 RID: 13365 RVA: 0x0011CFEC File Offset: 0x0011B1EC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Teleporters.Add(this);
		this.SetTeleporterID(0);
		base.Subscribe<Teleporter>(-801688580, Teleporter.OnLogicValueChangedDelegate);
	}

	// Token: 0x06003436 RID: 13366 RVA: 0x0011D018 File Offset: 0x0011B218
	private void OnLogicValueChanged(object data)
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		List<int> list = new List<int>();
		int num = 0;
		int num2 = Mathf.Min(this.ID_LENGTH, component.inputPorts.Count);
		for (int i = 0; i < num2; i++)
		{
			int logicUICell = component.inputPorts[i].GetLogicUICell();
			LogicCircuitNetwork networkForCell = logicCircuitManager.GetNetworkForCell(logicUICell);
			int item = (networkForCell != null) ? networkForCell.OutputValue : 1;
			list.Add(item);
		}
		foreach (int num3 in list)
		{
			num = (num << 1 | num3);
		}
		this.SetTeleporterID(num);
	}

	// Token: 0x06003437 RID: 13367 RVA: 0x0011D0E8 File Offset: 0x0011B2E8
	protected override void OnCleanUp()
	{
		Components.Teleporters.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06003438 RID: 13368 RVA: 0x0011D0FB File Offset: 0x0011B2FB
	public bool HasTeleporterTarget()
	{
		return this.FindTeleportTarget() != null;
	}

	// Token: 0x06003439 RID: 13369 RVA: 0x0011D109 File Offset: 0x0011B309
	public bool IsValidTeleportTarget(Teleporter from_tele)
	{
		return from_tele.teleporterID == this.teleporterID && this.operational.IsOperational;
	}

	// Token: 0x0600343A RID: 13370 RVA: 0x0011D128 File Offset: 0x0011B328
	public Teleporter FindTeleportTarget()
	{
		List<Teleporter> list = new List<Teleporter>();
		foreach (object obj in Components.Teleporters)
		{
			Teleporter teleporter = (Teleporter)obj;
			if (teleporter.IsValidTeleportTarget(this) && teleporter != this)
			{
				list.Add(teleporter);
			}
		}
		Teleporter result = null;
		if (list.Count > 0)
		{
			result = list.GetRandom<Teleporter>();
		}
		return result;
	}

	// Token: 0x0600343B RID: 13371 RVA: 0x0011D1B0 File Offset: 0x0011B3B0
	public void SetTeleporterID(int ID)
	{
		this.teleporterID = ID;
		foreach (object obj in Components.Teleporters)
		{
			((Teleporter)obj).Trigger(-1266722732, null);
		}
	}

	// Token: 0x0600343C RID: 13372 RVA: 0x0011D214 File Offset: 0x0011B414
	public void SetTeleportTarget(Teleporter target)
	{
		this.teleportTarget.Set(target);
	}

	// Token: 0x0600343D RID: 13373 RVA: 0x0011D224 File Offset: 0x0011B424
	public void TeleportObjects()
	{
		Teleporter teleporter = this.teleportTarget.Get();
		int widthInCells = base.GetComponent<Building>().Def.WidthInCells;
		int num = base.GetComponent<Building>().Def.HeightInCells - 1;
		Vector3 position = base.transform.GetPosition();
		if (teleporter != null)
		{
			ListPool<ScenePartitionerEntry, Teleporter>.PooledList pooledList = ListPool<ScenePartitionerEntry, Teleporter>.Allocate();
			GameScenePartitioner.Instance.GatherEntries((int)position.x - widthInCells / 2 + 1, (int)position.y - num / 2 + 1, widthInCells, num, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
			int cell = Grid.PosToCell(teleporter);
			foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
			{
				GameObject gameObject = (scenePartitionerEntry.obj as Pickupable).gameObject;
				Vector3 vector = gameObject.transform.GetPosition() - position;
				MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
				if (component != null)
				{
					new EmoteChore(component.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, "anim_interacts_portal_kanim", Telepad.PortalBirthAnim, null);
				}
				else
				{
					vector += Vector3.up;
				}
				gameObject.transform.SetLocalPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move) + vector);
			}
			pooledList.Recycle();
		}
		TeleportalPad.StatesInstance smi = this.teleportTarget.Get().GetSMI<TeleportalPad.StatesInstance>();
		smi.sm.doTeleport.Trigger(smi);
		this.teleportTarget.Set(null);
	}

	// Token: 0x04001EDC RID: 7900
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001EDE RID: 7902
	[Serialize]
	public Ref<Teleporter> teleportTarget = new Ref<Teleporter>();

	// Token: 0x04001EDF RID: 7903
	public int ID_LENGTH = 4;

	// Token: 0x04001EE0 RID: 7904
	private static readonly EventSystem.IntraObjectHandler<Teleporter> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<Teleporter>(delegate(Teleporter component, object data)
	{
		component.OnLogicValueChanged(data);
	});
}
