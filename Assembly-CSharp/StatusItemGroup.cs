using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020005C3 RID: 1475
public class StatusItemGroup
{
	// Token: 0x06002351 RID: 9041 RVA: 0x000C52BB File Offset: 0x000C34BB
	public IEnumerator<StatusItemGroup.Entry> GetEnumerator()
	{
		return this.items.GetEnumerator();
	}

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x06002352 RID: 9042 RVA: 0x000C52CD File Offset: 0x000C34CD
	// (set) Token: 0x06002353 RID: 9043 RVA: 0x000C52D5 File Offset: 0x000C34D5
	public GameObject gameObject { get; private set; }

	// Token: 0x06002354 RID: 9044 RVA: 0x000C52DE File Offset: 0x000C34DE
	public StatusItemGroup(GameObject go)
	{
		this.gameObject = go;
	}

	// Token: 0x06002355 RID: 9045 RVA: 0x000C5312 File Offset: 0x000C3512
	public void SetOffset(Vector3 offset)
	{
		this.offset = offset;
		Game.Instance.SetStatusItemOffset(this.gameObject.transform, offset);
	}

	// Token: 0x06002356 RID: 9046 RVA: 0x000C5334 File Offset: 0x000C3534
	public StatusItemGroup.Entry GetStatusItem(StatusItemCategory category)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].category == category)
			{
				return this.items[i];
			}
		}
		return StatusItemGroup.Entry.EmptyEntry;
	}

	// Token: 0x06002357 RID: 9047 RVA: 0x000C5380 File Offset: 0x000C3580
	public Guid SetStatusItem(StatusItemCategory category, StatusItem item, object data = null)
	{
		if (item != null && item.allowMultiples)
		{
			throw new ArgumentException(item.Name + " allows multiple instances of itself to be active so you must access it via its handle");
		}
		if (category == null)
		{
			throw new ArgumentException("SetStatusItem requires a category.");
		}
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].category == category)
			{
				if (this.items[i].item == item)
				{
					this.Log("Set (exists in category)", item, this.items[i].id, category);
					return this.items[i].id;
				}
				this.Log("Set->Remove existing in category", item, this.items[i].id, category);
				this.RemoveStatusItem(this.items[i].id, false);
			}
		}
		if (item != null)
		{
			Guid guid = this.AddStatusItem(item, data, category);
			this.Log("Set (new)", item, guid, category);
			return guid;
		}
		this.Log("Set (failed)", item, Guid.Empty, category);
		return Guid.Empty;
	}

	// Token: 0x06002358 RID: 9048 RVA: 0x000C549B File Offset: 0x000C369B
	public void SetStatusItem(Guid guid, StatusItemCategory category, StatusItem new_item, object data = null)
	{
		this.RemoveStatusItem(guid, false);
		if (new_item != null)
		{
			this.AddStatusItem(new_item, data, category);
		}
	}

	// Token: 0x06002359 RID: 9049 RVA: 0x000C54B4 File Offset: 0x000C36B4
	public bool HasStatusItem(StatusItem status_item)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].item.Id == status_item.Id)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600235A RID: 9050 RVA: 0x000C5500 File Offset: 0x000C3700
	public bool HasStatusItemID(string status_item_id)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].item.Id == status_item_id)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600235B RID: 9051 RVA: 0x000C5544 File Offset: 0x000C3744
	public Guid AddStatusItem(StatusItem item, object data = null, StatusItemCategory category = null)
	{
		if (this.gameObject == null || (!item.allowMultiples && this.HasStatusItem(item)))
		{
			return Guid.Empty;
		}
		if (!item.allowMultiples)
		{
			using (List<StatusItemGroup.Entry>.Enumerator enumerator = this.items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.item.Id == item.Id)
					{
						throw new ArgumentException("Tried to add " + item.Id + " multiples times which is not permitted.");
					}
				}
			}
		}
		StatusItemGroup.Entry entry = new StatusItemGroup.Entry(item, category, data);
		if (item.shouldNotify)
		{
			entry.notification = new Notification(item.notificationText, item.notificationType, new Func<List<Notification>, object, string>(StatusItemGroup.OnToolTip), item, false, 0f, item.notificationClickCallback, data, null, true, false, false);
			this.gameObject.AddOrGet<Notifier>().Add(entry.notification, "");
		}
		if (item.ShouldShowIcon())
		{
			Game.Instance.AddStatusItem(this.gameObject.transform, item);
			Game.Instance.SetStatusItemOffset(this.gameObject.transform, this.offset);
		}
		this.items.Add(entry);
		if (this.OnAddStatusItem != null)
		{
			this.OnAddStatusItem(entry, category);
		}
		return entry.id;
	}

	// Token: 0x0600235C RID: 9052 RVA: 0x000C56B4 File Offset: 0x000C38B4
	public Guid RemoveStatusItem(StatusItem status_item, bool immediate = false)
	{
		if (status_item.allowMultiples)
		{
			throw new ArgumentException(status_item.Name + " allows multiple instances of itself to be active so it must be released via an instance handle");
		}
		int i = 0;
		while (i < this.items.Count)
		{
			if (this.items[i].item.Id == status_item.Id)
			{
				Guid id = this.items[i].id;
				if (id == Guid.Empty)
				{
					return id;
				}
				this.RemoveStatusItemInternal(id, i, immediate);
				return id;
			}
			else
			{
				i++;
			}
		}
		return Guid.Empty;
	}

	// Token: 0x0600235D RID: 9053 RVA: 0x000C574C File Offset: 0x000C394C
	public Guid RemoveStatusItem(Guid guid, bool immediate = false)
	{
		if (guid == Guid.Empty)
		{
			return guid;
		}
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].id == guid)
			{
				this.RemoveStatusItemInternal(guid, i, immediate);
				return guid;
			}
		}
		return Guid.Empty;
	}

	// Token: 0x0600235E RID: 9054 RVA: 0x000C57A8 File Offset: 0x000C39A8
	private void RemoveStatusItemInternal(Guid guid, int itemIdx, bool immediate)
	{
		StatusItemGroup.Entry entry = this.items[itemIdx];
		this.items.RemoveAt(itemIdx);
		if (entry.notification != null)
		{
			this.gameObject.GetComponent<Notifier>().Remove(entry.notification);
		}
		if (entry.item.ShouldShowIcon() && Game.Instance != null)
		{
			Game.Instance.RemoveStatusItem(this.gameObject.transform, entry.item);
		}
		if (this.OnRemoveStatusItem != null)
		{
			this.OnRemoveStatusItem(entry, immediate);
		}
	}

	// Token: 0x0600235F RID: 9055 RVA: 0x000C5836 File Offset: 0x000C3A36
	private static string OnToolTip(List<Notification> notifications, object data)
	{
		return ((StatusItem)data).notificationTooltipText + notifications.ReduceMessages(true);
	}

	// Token: 0x06002360 RID: 9056 RVA: 0x000C584F File Offset: 0x000C3A4F
	public void Destroy()
	{
		if (Game.IsQuitting())
		{
			return;
		}
		while (this.items.Count > 0)
		{
			this.RemoveStatusItem(this.items[0].id, false);
		}
	}

	// Token: 0x06002361 RID: 9057 RVA: 0x000C5880 File Offset: 0x000C3A80
	[Conditional("ENABLE_LOGGER")]
	private void Log(string action, StatusItem item, Guid guid)
	{
	}

	// Token: 0x06002362 RID: 9058 RVA: 0x000C5882 File Offset: 0x000C3A82
	private void Log(string action, StatusItem item, Guid guid, StatusItemCategory category)
	{
	}

	// Token: 0x0400141F RID: 5151
	private List<StatusItemGroup.Entry> items = new List<StatusItemGroup.Entry>();

	// Token: 0x04001420 RID: 5152
	public Action<StatusItemGroup.Entry, StatusItemCategory> OnAddStatusItem;

	// Token: 0x04001421 RID: 5153
	public Action<StatusItemGroup.Entry, bool> OnRemoveStatusItem;

	// Token: 0x04001423 RID: 5155
	private Vector3 offset = new Vector3(0f, 0f, 0f);

	// Token: 0x020013BA RID: 5050
	public struct Entry : IComparable<StatusItemGroup.Entry>, IEquatable<StatusItemGroup.Entry>
	{
		// Token: 0x0600881B RID: 34843 RVA: 0x0032D75D File Offset: 0x0032B95D
		public Entry(StatusItem item, StatusItemCategory category, object data)
		{
			this.id = Guid.NewGuid();
			this.item = item;
			this.data = data;
			this.category = category;
			this.notification = null;
		}

		// Token: 0x0600881C RID: 34844 RVA: 0x0032D786 File Offset: 0x0032B986
		public string GetName()
		{
			return this.item.GetName(this.data);
		}

		// Token: 0x0600881D RID: 34845 RVA: 0x0032D799 File Offset: 0x0032B999
		public void ShowToolTip(ToolTip tooltip_widget, TextStyleSetting property_style)
		{
			this.item.ShowToolTip(tooltip_widget, this.data, property_style);
		}

		// Token: 0x0600881E RID: 34846 RVA: 0x0032D7AE File Offset: 0x0032B9AE
		public void SetIcon(Image image)
		{
			this.item.SetIcon(image, this.data);
		}

		// Token: 0x0600881F RID: 34847 RVA: 0x0032D7C2 File Offset: 0x0032B9C2
		public int CompareTo(StatusItemGroup.Entry other)
		{
			return this.id.CompareTo(other.id);
		}

		// Token: 0x06008820 RID: 34848 RVA: 0x0032D7D5 File Offset: 0x0032B9D5
		public bool Equals(StatusItemGroup.Entry other)
		{
			return this.id == other.id;
		}

		// Token: 0x06008821 RID: 34849 RVA: 0x0032D7E8 File Offset: 0x0032B9E8
		public void OnClick()
		{
			this.item.OnClick(this.data);
		}

		// Token: 0x040067AA RID: 26538
		public static StatusItemGroup.Entry EmptyEntry = new StatusItemGroup.Entry
		{
			id = Guid.Empty
		};

		// Token: 0x040067AB RID: 26539
		public Guid id;

		// Token: 0x040067AC RID: 26540
		public StatusItem item;

		// Token: 0x040067AD RID: 26541
		public object data;

		// Token: 0x040067AE RID: 26542
		public Notification notification;

		// Token: 0x040067AF RID: 26543
		public StatusItemCategory category;
	}
}
