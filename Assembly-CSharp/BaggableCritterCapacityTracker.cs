using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200052F RID: 1327
public class BaggableCritterCapacityTracker : KMonoBehaviour, ISim1000ms, IUserControlledCapacity
{
	// Token: 0x1700011B RID: 283
	// (get) Token: 0x06001DDD RID: 7645 RVA: 0x000A5817 File Offset: 0x000A3A17
	// (set) Token: 0x06001DDE RID: 7646 RVA: 0x000A581F File Offset: 0x000A3A1F
	[Serialize]
	public int creatureLimit { get; set; } = 20;

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x06001DDF RID: 7647 RVA: 0x000A5828 File Offset: 0x000A3A28
	// (set) Token: 0x06001DE0 RID: 7648 RVA: 0x000A5830 File Offset: 0x000A3A30
	public int storedCreatureCount { get; private set; }

	// Token: 0x06001DE1 RID: 7649 RVA: 0x000A583C File Offset: 0x000A3A3C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int cell = Grid.PosToCell(this);
		this.cavityCell = Grid.OffsetCell(cell, this.cavityOffset);
		this.filter = base.GetComponent<TreeFilterable>();
		TreeFilterable treeFilterable = this.filter;
		treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(this.RefreshCreatureCount));
		base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
		base.Subscribe(144050788, new Action<object>(this.RefreshCreatureCount));
	}

	// Token: 0x06001DE2 RID: 7650 RVA: 0x000A58CC File Offset: 0x000A3ACC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (BaggableCritterCapacityTracker.capacityStatusItem == null)
		{
			BaggableCritterCapacityTracker.capacityStatusItem = new StatusItem("CritterCapacity", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			BaggableCritterCapacityTracker.capacityStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				IUserControlledCapacity userControlledCapacity = (IUserControlledCapacity)data;
				string newValue = Util.FormatWholeNumber(Mathf.Floor(userControlledCapacity.AmountStored));
				string newValue2 = Util.FormatWholeNumber(userControlledCapacity.UserMaxCapacity);
				str = str.Replace("{Stored}", newValue).Replace("{StoredUnits}", ((int)userControlledCapacity.AmountStored == 1) ? BUILDING.STATUSITEMS.CRITTERCAPACITY.UNIT : BUILDING.STATUSITEMS.CRITTERCAPACITY.UNITS).Replace("{Capacity}", newValue2).Replace("{CapacityUnits}", ((int)userControlledCapacity.UserMaxCapacity == 1) ? BUILDING.STATUSITEMS.CRITTERCAPACITY.UNIT : BUILDING.STATUSITEMS.CRITTERCAPACITY.UNITS);
				return str;
			};
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, BaggableCritterCapacityTracker.capacityStatusItem, this);
	}

	// Token: 0x06001DE3 RID: 7651 RVA: 0x000A5958 File Offset: 0x000A3B58
	protected override void OnCleanUp()
	{
		TreeFilterable treeFilterable = this.filter;
		treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Remove(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(this.RefreshCreatureCount));
		base.Unsubscribe(144050788);
		base.OnCleanUp();
	}

	// Token: 0x06001DE4 RID: 7652 RVA: 0x000A5994 File Offset: 0x000A3B94
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		BaggableCritterCapacityTracker component = gameObject.GetComponent<BaggableCritterCapacityTracker>();
		if (component == null)
		{
			return;
		}
		this.creatureLimit = component.creatureLimit;
	}

	// Token: 0x06001DE5 RID: 7653 RVA: 0x000A59D0 File Offset: 0x000A3BD0
	public void RefreshCreatureCount(object data = null)
	{
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(this.cavityCell);
		int storedCreatureCount = this.storedCreatureCount;
		this.storedCreatureCount = 0;
		if (cavityForCell != null)
		{
			foreach (KPrefabID kprefabID in cavityForCell.creatures)
			{
				if (!kprefabID.HasTag(GameTags.Creatures.Bagged) && !kprefabID.HasTag(GameTags.Trapped) && (!this.filteredCount || this.filter.AcceptedTags.Contains(kprefabID.PrefabTag)))
				{
					int storedCreatureCount2 = this.storedCreatureCount;
					this.storedCreatureCount = storedCreatureCount2 + 1;
				}
			}
		}
		if (this.onCountChanged != null && this.storedCreatureCount != storedCreatureCount)
		{
			this.onCountChanged();
		}
	}

	// Token: 0x06001DE6 RID: 7654 RVA: 0x000A5AAC File Offset: 0x000A3CAC
	public void Sim1000ms(float dt)
	{
		this.RefreshCreatureCount(null);
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x06001DE7 RID: 7655 RVA: 0x000A5AB5 File Offset: 0x000A3CB5
	// (set) Token: 0x06001DE8 RID: 7656 RVA: 0x000A5ABE File Offset: 0x000A3CBE
	float IUserControlledCapacity.UserMaxCapacity
	{
		get
		{
			return (float)this.creatureLimit;
		}
		set
		{
			this.creatureLimit = Mathf.RoundToInt(value);
			if (this.onCountChanged != null)
			{
				this.onCountChanged();
			}
		}
	}

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x06001DE9 RID: 7657 RVA: 0x000A5ADF File Offset: 0x000A3CDF
	float IUserControlledCapacity.AmountStored
	{
		get
		{
			return (float)this.storedCreatureCount;
		}
	}

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x06001DEA RID: 7658 RVA: 0x000A5AE8 File Offset: 0x000A3CE8
	float IUserControlledCapacity.MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x06001DEB RID: 7659 RVA: 0x000A5AEF File Offset: 0x000A3CEF
	float IUserControlledCapacity.MaxCapacity
	{
		get
		{
			return (float)this.maximumCreatures;
		}
	}

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x06001DEC RID: 7660 RVA: 0x000A5AF8 File Offset: 0x000A3CF8
	bool IUserControlledCapacity.WholeValues
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06001DED RID: 7661 RVA: 0x000A5AFB File Offset: 0x000A3CFB
	LocString IUserControlledCapacity.CapacityUnits
	{
		get
		{
			return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.UNITS_SUFFIX;
		}
	}

	// Token: 0x040010C9 RID: 4297
	public int maximumCreatures = 40;

	// Token: 0x040010CA RID: 4298
	public CellOffset cavityOffset;

	// Token: 0x040010CB RID: 4299
	public bool filteredCount;

	// Token: 0x040010CC RID: 4300
	public System.Action onCountChanged;

	// Token: 0x040010CD RID: 4301
	private int cavityCell;

	// Token: 0x040010CE RID: 4302
	[MyCmpReq]
	private TreeFilterable filter;

	// Token: 0x040010CF RID: 4303
	private static StatusItem capacityStatusItem;
}
