using System;
using KSerialization;

// Token: 0x0200056A RID: 1386
[SerializationConfig(MemberSerialization.OptIn)]
public class GasSource : SubstanceSource
{
	// Token: 0x0600201D RID: 8221 RVA: 0x000B4B6A File Offset: 0x000B2D6A
	protected override CellOffset[] GetOffsetGroup()
	{
		return OffsetGroups.LiquidSource;
	}

	// Token: 0x0600201E RID: 8222 RVA: 0x000B4B71 File Offset: 0x000B2D71
	protected override IChunkManager GetChunkManager()
	{
		return GasSourceManager.Instance;
	}

	// Token: 0x0600201F RID: 8223 RVA: 0x000B4B78 File Offset: 0x000B2D78
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}
}
