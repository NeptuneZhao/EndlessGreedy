using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000CDF RID: 3295
public abstract class MeterScreen_ValueTrackerDisplayer : KMonoBehaviour
{
	// Token: 0x060065D7 RID: 26071 RVA: 0x0025F89A File Offset: 0x0025DA9A
	protected override void OnSpawn()
	{
		this.Tooltip.OnToolTip = new Func<string>(this.OnTooltip);
		base.OnSpawn();
	}

	// Token: 0x060065D8 RID: 26072 RVA: 0x0025F8BA File Offset: 0x0025DABA
	public void Refresh()
	{
		this.RefreshWorldMinionIdentities();
		this.InternalRefresh();
	}

	// Token: 0x060065D9 RID: 26073
	protected abstract void InternalRefresh();

	// Token: 0x060065DA RID: 26074
	protected abstract string OnTooltip();

	// Token: 0x060065DB RID: 26075 RVA: 0x0025F8C8 File Offset: 0x0025DAC8
	public virtual void OnClick(BaseEventData base_ev_data)
	{
	}

	// Token: 0x060065DC RID: 26076 RVA: 0x0025F8CC File Offset: 0x0025DACC
	private void RefreshWorldMinionIdentities()
	{
		this.worldLiveMinionIdentities = new List<MinionIdentity>(from x in Components.LiveMinionIdentities.GetWorldItems(ClusterManager.Instance.activeWorldId, false)
		where !x.IsNullOrDestroyed()
		select x);
	}

	// Token: 0x060065DD RID: 26077 RVA: 0x0025F91D File Offset: 0x0025DB1D
	protected virtual List<MinionIdentity> GetWorldMinionIdentities()
	{
		if (this.worldLiveMinionIdentities == null)
		{
			this.RefreshWorldMinionIdentities();
		}
		return this.worldLiveMinionIdentities;
	}

	// Token: 0x040044B9 RID: 17593
	public LocText Label;

	// Token: 0x040044BA RID: 17594
	public ToolTip Tooltip;

	// Token: 0x040044BB RID: 17595
	public GameObject diagnosticGraph;

	// Token: 0x040044BC RID: 17596
	public TextStyleSetting ToolTipStyle_Header;

	// Token: 0x040044BD RID: 17597
	public TextStyleSetting ToolTipStyle_Property;

	// Token: 0x040044BE RID: 17598
	private List<MinionIdentity> worldLiveMinionIdentities;
}
