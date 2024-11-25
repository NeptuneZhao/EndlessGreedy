using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000C20 RID: 3104
[AddComponentMenu("KMonoBehaviour/scripts/CreatureFeeder")]
public class CreatureFeeder : KMonoBehaviour
{
	// Token: 0x06005F2B RID: 24363 RVA: 0x00235E51 File Offset: 0x00234051
	protected override void OnSpawn()
	{
		this.storages = base.GetComponents<Storage>();
		Components.CreatureFeeders.Add(this.GetMyWorldId(), this);
		base.Subscribe<CreatureFeeder>(-1452790913, CreatureFeeder.OnAteFromStorageDelegate);
	}

	// Token: 0x06005F2C RID: 24364 RVA: 0x00235E81 File Offset: 0x00234081
	protected override void OnCleanUp()
	{
		Components.CreatureFeeders.Remove(this.GetMyWorldId(), this);
	}

	// Token: 0x06005F2D RID: 24365 RVA: 0x00235E94 File Offset: 0x00234094
	private void OnAteFromStorage(object data)
	{
		if (string.IsNullOrEmpty(this.effectId))
		{
			return;
		}
		(data as GameObject).GetComponent<Effects>().Add(this.effectId, true);
	}

	// Token: 0x06005F2E RID: 24366 RVA: 0x00235EBC File Offset: 0x002340BC
	public bool StoragesAreEmpty()
	{
		foreach (Storage storage in this.storages)
		{
			if (!(storage == null) && storage.Count > 0)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005F2F RID: 24367 RVA: 0x00235EF7 File Offset: 0x002340F7
	public Vector2I GetTargetFeederCell()
	{
		return Grid.CellToXY(Grid.OffsetCell(Grid.PosToCell(this), this.feederOffset));
	}

	// Token: 0x04004002 RID: 16386
	public Storage[] storages;

	// Token: 0x04004003 RID: 16387
	public string effectId;

	// Token: 0x04004004 RID: 16388
	public CellOffset feederOffset = CellOffset.none;

	// Token: 0x04004005 RID: 16389
	private static readonly EventSystem.IntraObjectHandler<CreatureFeeder> OnAteFromStorageDelegate = new EventSystem.IntraObjectHandler<CreatureFeeder>(delegate(CreatureFeeder component, object data)
	{
		component.OnAteFromStorage(data);
	});
}
