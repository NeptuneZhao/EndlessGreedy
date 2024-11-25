using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005F2 RID: 1522
[AddComponentMenu("KMonoBehaviour/scripts/Uncoverable")]
public class Uncoverable : KMonoBehaviour
{
	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x060024EF RID: 9455 RVA: 0x000CEC90 File Offset: 0x000CCE90
	public bool IsUncovered
	{
		get
		{
			return this.hasBeenUncovered;
		}
	}

	// Token: 0x060024F0 RID: 9456 RVA: 0x000CEC98 File Offset: 0x000CCE98
	private bool IsAnyCellShowing()
	{
		int rootCell = Grid.PosToCell(this);
		return !this.occupyArea.TestArea(rootCell, null, Uncoverable.IsCellBlockedDelegate);
	}

	// Token: 0x060024F1 RID: 9457 RVA: 0x000CECC1 File Offset: 0x000CCEC1
	private static bool IsCellBlocked(int cell, object data)
	{
		return Grid.Element[cell].IsSolid && !Grid.Foundation[cell];
	}

	// Token: 0x060024F2 RID: 9458 RVA: 0x000CECE1 File Offset: 0x000CCEE1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060024F3 RID: 9459 RVA: 0x000CECEC File Offset: 0x000CCEEC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.IsAnyCellShowing())
		{
			this.hasBeenUncovered = true;
		}
		if (!this.hasBeenUncovered)
		{
			base.GetComponent<KSelectable>().IsSelectable = false;
			Extents extents = this.occupyArea.GetExtents();
			this.partitionerEntry = GameScenePartitioner.Instance.Add("Uncoverable.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		}
	}

	// Token: 0x060024F4 RID: 9460 RVA: 0x000CED60 File Offset: 0x000CCF60
	private void OnSolidChanged(object data)
	{
		if (this.IsAnyCellShowing() && !this.hasBeenUncovered && this.partitionerEntry.IsValid())
		{
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
			this.hasBeenUncovered = true;
			base.GetComponent<KSelectable>().IsSelectable = true;
			Notification notification = new Notification(MISC.STATUSITEMS.BURIEDITEM.NOTIFICATION, NotificationType.Good, new Func<List<Notification>, object, string>(Uncoverable.OnNotificationToolTip), this, true, 0f, null, null, null, true, false, false);
			base.gameObject.AddOrGet<Notifier>().Add(notification, "");
		}
	}

	// Token: 0x060024F5 RID: 9461 RVA: 0x000CEDF0 File Offset: 0x000CCFF0
	private static string OnNotificationToolTip(List<Notification> notifications, object data)
	{
		Uncoverable cmp = (Uncoverable)data;
		return MISC.STATUSITEMS.BURIEDITEM.NOTIFICATION_TOOLTIP.Replace("{Uncoverable}", cmp.GetProperName());
	}

	// Token: 0x060024F6 RID: 9462 RVA: 0x000CEE19 File Offset: 0x000CD019
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x040014EA RID: 5354
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x040014EB RID: 5355
	[Serialize]
	private bool hasBeenUncovered;

	// Token: 0x040014EC RID: 5356
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040014ED RID: 5357
	private static readonly Func<int, object, bool> IsCellBlockedDelegate = (int cell, object data) => Uncoverable.IsCellBlocked(cell, data);
}
