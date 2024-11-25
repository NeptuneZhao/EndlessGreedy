using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

// Token: 0x020002F3 RID: 755
[Serializable]
public struct ModInfo
{
	// Token: 0x040009A1 RID: 2465
	[JsonConverter(typeof(StringEnumConverter))]
	public ModInfo.Source source;

	// Token: 0x040009A2 RID: 2466
	[JsonConverter(typeof(StringEnumConverter))]
	public ModInfo.ModType type;

	// Token: 0x040009A3 RID: 2467
	public string assetID;

	// Token: 0x040009A4 RID: 2468
	public string assetPath;

	// Token: 0x040009A5 RID: 2469
	public bool enabled;

	// Token: 0x040009A6 RID: 2470
	public bool markedForDelete;

	// Token: 0x040009A7 RID: 2471
	public bool markedForUpdate;

	// Token: 0x040009A8 RID: 2472
	public string description;

	// Token: 0x040009A9 RID: 2473
	public ulong lastModifiedTime;

	// Token: 0x0200111D RID: 4381
	public enum Source
	{
		// Token: 0x04005F25 RID: 24357
		Local,
		// Token: 0x04005F26 RID: 24358
		Steam,
		// Token: 0x04005F27 RID: 24359
		Rail
	}

	// Token: 0x0200111E RID: 4382
	public enum ModType
	{
		// Token: 0x04005F29 RID: 24361
		WorldGen,
		// Token: 0x04005F2A RID: 24362
		Scenario,
		// Token: 0x04005F2B RID: 24363
		Mod
	}
}
