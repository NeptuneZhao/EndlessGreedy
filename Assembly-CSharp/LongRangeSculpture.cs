using System;

// Token: 0x02000580 RID: 1408
public class LongRangeSculpture : Sculpture
{
	// Token: 0x060020BB RID: 8379 RVA: 0x000B719C File Offset: 0x000B539C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = null;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.multitoolContext = "paint";
		this.multitoolHitEffectTag = "fx_paint_splash";
	}
}
