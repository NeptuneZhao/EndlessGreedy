using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020005AD RID: 1453
[AddComponentMenu("KMonoBehaviour/scripts/RoomTracker")]
public class RoomTracker : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x1700018C RID: 396
	// (get) Token: 0x060022A2 RID: 8866 RVA: 0x000C0EC5 File Offset: 0x000BF0C5
	// (set) Token: 0x060022A3 RID: 8867 RVA: 0x000C0ECD File Offset: 0x000BF0CD
	public Room room { get; private set; }

	// Token: 0x060022A4 RID: 8868 RVA: 0x000C0ED8 File Offset: 0x000BF0D8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		global::Debug.Assert(!string.IsNullOrEmpty(this.requiredRoomType) && this.requiredRoomType != Db.Get().RoomTypes.Neutral.Id, "RoomTracker must have a requiredRoomType!");
		base.Subscribe<RoomTracker>(144050788, RoomTracker.OnUpdateRoomDelegate);
		this.FindAndSetRoom();
	}

	// Token: 0x060022A5 RID: 8869 RVA: 0x000C0F3C File Offset: 0x000BF13C
	public void FindAndSetRoom()
	{
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(base.gameObject));
		if (cavityForCell != null && cavityForCell.room != null)
		{
			this.OnUpdateRoom(cavityForCell.room);
			return;
		}
		this.OnUpdateRoom(null);
	}

	// Token: 0x060022A6 RID: 8870 RVA: 0x000C0F83 File Offset: 0x000BF183
	public bool IsInCorrectRoom()
	{
		return this.room != null && this.room.roomType.Id == this.requiredRoomType;
	}

	// Token: 0x060022A7 RID: 8871 RVA: 0x000C0FAC File Offset: 0x000BF1AC
	public bool SufficientBuildLocation(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		if (this.requirement == RoomTracker.Requirement.Required || this.requirement == RoomTracker.Requirement.CustomRequired)
		{
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			if (((cavityForCell != null) ? cavityForCell.room : null) == null)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060022A8 RID: 8872 RVA: 0x000C0FF8 File Offset: 0x000BF1F8
	private void OnUpdateRoom(object data)
	{
		this.room = (Room)data;
		if (this.room != null && !(this.room.roomType.Id != this.requiredRoomType))
		{
			this.statusItemGuid = base.GetComponent<KSelectable>().RemoveStatusItem(this.statusItemGuid, false);
			return;
		}
		switch (this.requirement)
		{
		case RoomTracker.Requirement.TrackingOnly:
			this.statusItemGuid = base.GetComponent<KSelectable>().RemoveStatusItem(this.statusItemGuid, false);
			return;
		case RoomTracker.Requirement.Recommended:
			this.statusItemGuid = base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.RequiredRoom, Db.Get().BuildingStatusItems.NotInRecommendedRoom, this.requiredRoomType);
			return;
		case RoomTracker.Requirement.Required:
			this.statusItemGuid = base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.RequiredRoom, Db.Get().BuildingStatusItems.NotInRequiredRoom, this.requiredRoomType);
			return;
		case RoomTracker.Requirement.CustomRecommended:
		case RoomTracker.Requirement.CustomRequired:
			this.statusItemGuid = base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.RequiredRoom, Db.Get().BuildingStatusItems.Get(this.customStatusItemID), this.requiredRoomType);
			return;
		default:
			return;
		}
	}

	// Token: 0x060022A9 RID: 8873 RVA: 0x000C1134 File Offset: 0x000BF334
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (!string.IsNullOrEmpty(this.requiredRoomType))
		{
			string name = Db.Get().RoomTypes.Get(this.requiredRoomType).Name;
			switch (this.requirement)
			{
			case RoomTracker.Requirement.Recommended:
			case RoomTracker.Requirement.CustomRecommended:
				list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.PREFERS_ROOM, name), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.PREFERS_ROOM, name), Descriptor.DescriptorType.Requirement, false));
				break;
			case RoomTracker.Requirement.Required:
			case RoomTracker.Requirement.CustomRequired:
				list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.REQUIRESROOM, name), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESROOM, name), Descriptor.DescriptorType.Requirement, false));
				break;
			}
		}
		return list;
	}

	// Token: 0x04001387 RID: 4999
	public RoomTracker.Requirement requirement;

	// Token: 0x04001388 RID: 5000
	public string requiredRoomType;

	// Token: 0x04001389 RID: 5001
	public string customStatusItemID;

	// Token: 0x0400138A RID: 5002
	private Guid statusItemGuid;

	// Token: 0x0400138C RID: 5004
	private static readonly EventSystem.IntraObjectHandler<RoomTracker> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<RoomTracker>(delegate(RoomTracker component, object data)
	{
		component.OnUpdateRoom(data);
	});

	// Token: 0x020013A8 RID: 5032
	public enum Requirement
	{
		// Token: 0x0400676C RID: 26476
		TrackingOnly,
		// Token: 0x0400676D RID: 26477
		Recommended,
		// Token: 0x0400676E RID: 26478
		Required,
		// Token: 0x0400676F RID: 26479
		CustomRecommended,
		// Token: 0x04006770 RID: 26480
		CustomRequired
	}
}
