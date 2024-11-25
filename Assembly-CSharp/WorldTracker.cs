using System;

// Token: 0x020005E4 RID: 1508
public abstract class WorldTracker : Tracker
{
	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06002490 RID: 9360 RVA: 0x000CBA61 File Offset: 0x000C9C61
	// (set) Token: 0x06002491 RID: 9361 RVA: 0x000CBA69 File Offset: 0x000C9C69
	public int WorldID { get; private set; }

	// Token: 0x06002492 RID: 9362 RVA: 0x000CBA72 File Offset: 0x000C9C72
	public WorldTracker(int worldID)
	{
		this.WorldID = worldID;
	}
}
