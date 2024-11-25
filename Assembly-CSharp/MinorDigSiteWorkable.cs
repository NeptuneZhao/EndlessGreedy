using System;

// Token: 0x020002A2 RID: 674
public class MinorDigSiteWorkable : FossilExcavationWorkable
{
	// Token: 0x06000DF5 RID: 3573 RVA: 0x00050616 File Offset: 0x0004E816
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(90f);
	}

	// Token: 0x06000DF6 RID: 3574 RVA: 0x00050629 File Offset: 0x0004E829
	protected override void OnSpawn()
	{
		this.digsite = base.gameObject.GetSMI<MinorFossilDigSite.Instance>();
		base.OnSpawn();
	}

	// Token: 0x06000DF7 RID: 3575 RVA: 0x00050644 File Offset: 0x0004E844
	protected override bool IsMarkedForExcavation()
	{
		return this.digsite != null && !this.digsite.sm.IsRevealed.Get(this.digsite) && this.digsite.sm.MarkedForDig.Get(this.digsite);
	}

	// Token: 0x040008C4 RID: 2244
	private MinorFossilDigSite.Instance digsite;
}
