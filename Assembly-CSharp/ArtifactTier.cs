using System;

// Token: 0x020002A3 RID: 675
public class ArtifactTier
{
	// Token: 0x06000DF9 RID: 3577 RVA: 0x0005069D File Offset: 0x0004E89D
	public ArtifactTier(StringKey str_key, EffectorValues values, float payload_drop_chance)
	{
		this.decorValues = values;
		this.name_key = str_key;
		this.payloadDropChance = payload_drop_chance;
	}

	// Token: 0x040008C5 RID: 2245
	public EffectorValues decorValues;

	// Token: 0x040008C6 RID: 2246
	public StringKey name_key;

	// Token: 0x040008C7 RID: 2247
	public float payloadDropChance;
}
