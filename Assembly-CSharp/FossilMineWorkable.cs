using System;

// Token: 0x020001DE RID: 478
public class FossilMineWorkable : ComplexFabricatorWorkable
{
	// Token: 0x060009CC RID: 2508 RVA: 0x0003A1DB File Offset: 0x000383DB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.shouldShowSkillPerkStatusItem = false;
	}
}
