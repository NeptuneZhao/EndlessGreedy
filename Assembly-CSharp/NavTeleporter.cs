using System;
using TUNING;

// Token: 0x0200073B RID: 1851
public class NavTeleporter : KMonoBehaviour
{
	// Token: 0x0600312E RID: 12590 RVA: 0x0010F8A8 File Offset: 0x0010DAA8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<KPrefabID>().AddTag(GameTags.NavTeleporters, false);
		this.Register();
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged), "NavTeleporterCellChanged");
	}

	// Token: 0x0600312F RID: 12591 RVA: 0x0010F8F4 File Offset: 0x0010DAF4
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		int cell = this.GetCell();
		if (cell != Grid.InvalidCell)
		{
			Grid.HasNavTeleporter[cell] = false;
		}
		this.Deregister();
		Components.NavTeleporters.Remove(this);
	}

	// Token: 0x06003130 RID: 12592 RVA: 0x0010F933 File Offset: 0x0010DB33
	public void SetOverrideCell(int cell)
	{
		this.overrideCell = cell;
	}

	// Token: 0x06003131 RID: 12593 RVA: 0x0010F93C File Offset: 0x0010DB3C
	public int GetCell()
	{
		if (this.overrideCell >= 0)
		{
			return this.overrideCell;
		}
		return Grid.OffsetCell(Grid.PosToCell(this), this.offset);
	}

	// Token: 0x06003132 RID: 12594 RVA: 0x0010F960 File Offset: 0x0010DB60
	public void TwoWayTarget(NavTeleporter nt)
	{
		if (this.target != null)
		{
			if (nt != null)
			{
				nt.SetTarget(null);
			}
			this.BreakLink();
		}
		this.target = nt;
		if (this.target != null)
		{
			this.SetLink();
			if (nt != null)
			{
				nt.SetTarget(this);
			}
		}
	}

	// Token: 0x06003133 RID: 12595 RVA: 0x0010F9BC File Offset: 0x0010DBBC
	public void EnableTwoWayTarget(bool enable)
	{
		if (enable)
		{
			this.target.SetLink();
			this.SetLink();
			return;
		}
		this.target.BreakLink();
		this.BreakLink();
	}

	// Token: 0x06003134 RID: 12596 RVA: 0x0010F9E4 File Offset: 0x0010DBE4
	public void SetTarget(NavTeleporter nt)
	{
		if (this.target != null)
		{
			this.BreakLink();
		}
		this.target = nt;
		if (this.target != null)
		{
			this.SetLink();
		}
	}

	// Token: 0x06003135 RID: 12597 RVA: 0x0010FA18 File Offset: 0x0010DC18
	private void Register()
	{
		int cell = this.GetCell();
		if (!Grid.IsValidCell(cell))
		{
			this.lastRegisteredCell = Grid.InvalidCell;
			return;
		}
		Grid.HasNavTeleporter[cell] = true;
		Pathfinding.Instance.AddDirtyNavGridCell(cell);
		this.lastRegisteredCell = cell;
		if (this.target != null)
		{
			this.SetLink();
		}
	}

	// Token: 0x06003136 RID: 12598 RVA: 0x0010FA74 File Offset: 0x0010DC74
	private void SetLink()
	{
		int cell = this.target.GetCell();
		Pathfinding.Instance.GetNavGrid(DUPLICANTSTATS.STANDARD.BaseStats.NAV_GRID_NAME).teleportTransitions[this.lastRegisteredCell] = cell;
		Pathfinding.Instance.AddDirtyNavGridCell(this.lastRegisteredCell);
	}

	// Token: 0x06003137 RID: 12599 RVA: 0x0010FAC8 File Offset: 0x0010DCC8
	public void Deregister()
	{
		if (this.lastRegisteredCell != Grid.InvalidCell)
		{
			this.BreakLink();
			Grid.HasNavTeleporter[this.lastRegisteredCell] = false;
			Pathfinding.Instance.AddDirtyNavGridCell(this.lastRegisteredCell);
			this.lastRegisteredCell = Grid.InvalidCell;
		}
	}

	// Token: 0x06003138 RID: 12600 RVA: 0x0010FB14 File Offset: 0x0010DD14
	private void BreakLink()
	{
		Pathfinding.Instance.GetNavGrid(DUPLICANTSTATS.STANDARD.BaseStats.NAV_GRID_NAME).teleportTransitions.Remove(this.lastRegisteredCell);
		Pathfinding.Instance.AddDirtyNavGridCell(this.lastRegisteredCell);
	}

	// Token: 0x06003139 RID: 12601 RVA: 0x0010FB50 File Offset: 0x0010DD50
	private void OnCellChanged()
	{
		this.Deregister();
		this.Register();
		if (this.target != null)
		{
			NavTeleporter component = this.target.GetComponent<NavTeleporter>();
			if (component != null)
			{
				component.SetTarget(this);
			}
		}
	}

	// Token: 0x04001CEB RID: 7403
	private NavTeleporter target;

	// Token: 0x04001CEC RID: 7404
	private int lastRegisteredCell = Grid.InvalidCell;

	// Token: 0x04001CED RID: 7405
	public CellOffset offset;

	// Token: 0x04001CEE RID: 7406
	private int overrideCell = -1;
}
