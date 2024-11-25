using System;
using KSerialization;
using STRINGS;

// Token: 0x02000807 RID: 2055
public class EntombVulnerable : KMonoBehaviour, IWiltCause
{
	// Token: 0x170003F9 RID: 1017
	// (get) Token: 0x060038C7 RID: 14535 RVA: 0x00135D55 File Offset: 0x00133F55
	private OccupyArea occupyArea
	{
		get
		{
			if (this._occupyArea == null)
			{
				this._occupyArea = base.GetComponent<OccupyArea>();
			}
			return this._occupyArea;
		}
	}

	// Token: 0x170003FA RID: 1018
	// (get) Token: 0x060038C8 RID: 14536 RVA: 0x00135D77 File Offset: 0x00133F77
	public bool GetEntombed
	{
		get
		{
			return this.isEntombed;
		}
	}

	// Token: 0x060038C9 RID: 14537 RVA: 0x00135D80 File Offset: 0x00133F80
	public void SetStatusItem(StatusItem si)
	{
		bool flag = this.showStatusItemOnEntombed;
		this.SetShowStatusItemOnEntombed(false);
		this.EntombedStatusItem = si;
		this.SetShowStatusItemOnEntombed(flag);
	}

	// Token: 0x060038CA RID: 14538 RVA: 0x00135DAC File Offset: 0x00133FAC
	public void SetShowStatusItemOnEntombed(bool val)
	{
		this.showStatusItemOnEntombed = val;
		if (this.isEntombed && this.EntombedStatusItem != null)
		{
			if (this.showStatusItemOnEntombed)
			{
				this.selectable.AddStatusItem(this.EntombedStatusItem, null);
				return;
			}
			this.selectable.RemoveStatusItem(this.EntombedStatusItem, false);
		}
	}

	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x060038CB RID: 14539 RVA: 0x00135DFF File Offset: 0x00133FFF
	public string WiltStateString
	{
		get
		{
			return Db.Get().CreatureStatusItems.Entombed.resolveStringCallback(CREATURES.STATUSITEMS.ENTOMBED.LINE_ITEM, base.gameObject);
		}
	}

	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x060038CC RID: 14540 RVA: 0x00135E2A File Offset: 0x0013402A
	public WiltCondition.Condition[] Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Entombed
			};
		}
	}

	// Token: 0x060038CD RID: 14541 RVA: 0x00135E38 File Offset: 0x00134038
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.EntombedStatusItem == null)
		{
			this.EntombedStatusItem = this.DefaultEntombedStatusItem;
		}
		this.partitionerEntry = GameScenePartitioner.Instance.Add("EntombVulnerable", base.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		this.CheckEntombed();
		if (this.isEntombed)
		{
			base.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
			base.Trigger(-1089732772, true);
		}
	}

	// Token: 0x060038CE RID: 14542 RVA: 0x00135ECB File Offset: 0x001340CB
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x060038CF RID: 14543 RVA: 0x00135EE3 File Offset: 0x001340E3
	private void OnSolidChanged(object data)
	{
		this.CheckEntombed();
	}

	// Token: 0x060038D0 RID: 14544 RVA: 0x00135EEC File Offset: 0x001340EC
	private void CheckEntombed()
	{
		int cell = Grid.PosToCell(base.gameObject.transform.GetPosition());
		if (!Grid.IsValidCell(cell))
		{
			return;
		}
		if (!this.IsCellSafe(cell))
		{
			if (!this.isEntombed)
			{
				this.isEntombed = true;
				if (this.showStatusItemOnEntombed)
				{
					this.selectable.AddStatusItem(this.EntombedStatusItem, base.gameObject);
				}
				base.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
				base.Trigger(-1089732772, true);
			}
		}
		else if (this.isEntombed)
		{
			this.isEntombed = false;
			this.selectable.RemoveStatusItem(this.EntombedStatusItem, false);
			base.GetComponent<KPrefabID>().RemoveTag(GameTags.Entombed);
			base.Trigger(-1089732772, false);
		}
		if (this.operational != null)
		{
			this.operational.SetFlag(EntombVulnerable.notEntombedFlag, !this.isEntombed);
		}
	}

	// Token: 0x060038D1 RID: 14545 RVA: 0x00135FE1 File Offset: 0x001341E1
	public bool IsCellSafe(int cell)
	{
		return this.occupyArea.TestArea(cell, null, EntombVulnerable.IsCellSafeCBDelegate);
	}

	// Token: 0x060038D2 RID: 14546 RVA: 0x00135FF5 File Offset: 0x001341F5
	private static bool IsCellSafeCB(int cell, object data)
	{
		return Grid.IsValidCell(cell) && !Grid.Solid[cell];
	}

	// Token: 0x04002224 RID: 8740
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04002225 RID: 8741
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002226 RID: 8742
	private OccupyArea _occupyArea;

	// Token: 0x04002227 RID: 8743
	[Serialize]
	private bool isEntombed;

	// Token: 0x04002228 RID: 8744
	private StatusItem DefaultEntombedStatusItem = Db.Get().CreatureStatusItems.Entombed;

	// Token: 0x04002229 RID: 8745
	[NonSerialized]
	private StatusItem EntombedStatusItem;

	// Token: 0x0400222A RID: 8746
	private bool showStatusItemOnEntombed = true;

	// Token: 0x0400222B RID: 8747
	public static readonly Operational.Flag notEntombedFlag = new Operational.Flag("not_entombed", Operational.Flag.Type.Functional);

	// Token: 0x0400222C RID: 8748
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400222D RID: 8749
	private static readonly Func<int, object, bool> IsCellSafeCBDelegate = (int cell, object data) => EntombVulnerable.IsCellSafeCB(cell, data);
}
