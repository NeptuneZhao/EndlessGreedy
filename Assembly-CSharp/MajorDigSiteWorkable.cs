using System;

// Token: 0x02000282 RID: 642
public class MajorDigSiteWorkable : FossilExcavationWorkable
{
	// Token: 0x06000D4D RID: 3405 RVA: 0x0004C276 File Offset: 0x0004A476
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(90f);
	}

	// Token: 0x06000D4E RID: 3406 RVA: 0x0004C289 File Offset: 0x0004A489
	protected override void OnSpawn()
	{
		this.digsite = base.gameObject.GetSMI<MajorFossilDigSite.Instance>();
		base.OnSpawn();
	}

	// Token: 0x06000D4F RID: 3407 RVA: 0x0004C2A4 File Offset: 0x0004A4A4
	protected override bool IsMarkedForExcavation()
	{
		return this.digsite != null && !this.digsite.sm.IsRevealed.Get(this.digsite) && this.digsite.sm.MarkedForDig.Get(this.digsite);
	}

	// Token: 0x04000846 RID: 2118
	private MajorFossilDigSite.Instance digsite;
}
