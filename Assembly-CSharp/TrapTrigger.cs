using System;
using UnityEngine;

// Token: 0x02000B38 RID: 2872
public class TrapTrigger : KMonoBehaviour
{
	// Token: 0x060055B6 RID: 21942 RVA: 0x001E9FF0 File Offset: 0x001E81F0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameObject gameObject = base.gameObject;
		this.SetTriggerCell(Grid.PosToCell(gameObject));
		foreach (GameObject gameObject2 in this.storage.items)
		{
			this.SetStoredPosition(gameObject2);
			KBoxCollider2D component = gameObject2.GetComponent<KBoxCollider2D>();
			if (component != null)
			{
				component.enabled = true;
			}
		}
	}

	// Token: 0x060055B7 RID: 21943 RVA: 0x001EA078 File Offset: 0x001E8278
	public void SetTriggerCell(int cell)
	{
		HandleVector<int>.Handle handle = this.partitionerEntry;
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Trap", base.gameObject, cell, GameScenePartitioner.Instance.trapsLayer, new Action<object>(this.OnCreatureOnTrap));
	}

	// Token: 0x060055B8 RID: 21944 RVA: 0x001EA0D0 File Offset: 0x001E82D0
	public void SetStoredPosition(GameObject go)
	{
		if (go == null)
		{
			return;
		}
		KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
		Vector3 vector = Grid.CellToPosCBC(Grid.PosToCell(base.transform.GetPosition()), Grid.SceneLayer.BuildingBack);
		if (this.addTrappedAnimationOffset)
		{
			vector.x += this.trappedOffset.x - component.Offset.x;
			vector.y += this.trappedOffset.y - component.Offset.y;
		}
		else
		{
			vector.x += this.trappedOffset.x;
			vector.y += this.trappedOffset.y;
		}
		go.transform.SetPosition(vector);
		go.GetComponent<Pickupable>().UpdateCachedCell(Grid.PosToCell(vector));
		component.SetSceneLayer(Grid.SceneLayer.BuildingFront);
	}

	// Token: 0x060055B9 RID: 21945 RVA: 0x001EA1A8 File Offset: 0x001E83A8
	public void OnCreatureOnTrap(object data)
	{
		if (!base.enabled)
		{
			return;
		}
		if (!this.storage.IsEmpty())
		{
			return;
		}
		Trappable trappable = (Trappable)data;
		if (trappable.HasTag(GameTags.Stored))
		{
			return;
		}
		if (trappable.HasTag(GameTags.Trapped))
		{
			return;
		}
		if (trappable.HasTag(GameTags.Creatures.Bagged))
		{
			return;
		}
		bool flag = false;
		foreach (Tag tag in this.trappableCreatures)
		{
			if (trappable.HasTag(tag))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		this.storage.Store(trappable.gameObject, true, false, true, false);
		this.SetStoredPosition(trappable.gameObject);
		base.Trigger(-358342870, trappable.gameObject);
	}

	// Token: 0x060055BA RID: 21946 RVA: 0x001EA262 File Offset: 0x001E8462
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x04003830 RID: 14384
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04003831 RID: 14385
	public Tag[] trappableCreatures;

	// Token: 0x04003832 RID: 14386
	public Vector2 trappedOffset = Vector2.zero;

	// Token: 0x04003833 RID: 14387
	public bool addTrappedAnimationOffset = true;

	// Token: 0x04003834 RID: 14388
	[MyCmpReq]
	private Storage storage;
}
