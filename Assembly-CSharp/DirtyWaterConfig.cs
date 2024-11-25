using System;
using UnityEngine;

// Token: 0x02000314 RID: 788
public class DirtyWaterConfig : IOreConfig
{
	// Token: 0x1700003C RID: 60
	// (get) Token: 0x0600108A RID: 4234 RVA: 0x0005D644 File Offset: 0x0005B844
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.DirtyWater;
		}
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x0600108B RID: 4235 RVA: 0x0005D64B File Offset: 0x0005B84B
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.ContaminatedOxygen;
		}
	}

	// Token: 0x0600108C RID: 4236 RVA: 0x0005D652 File Offset: 0x0005B852
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600108D RID: 4237 RVA: 0x0005D65C File Offset: 0x0005B85C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLiquidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.ContaminatedOxygenBubbleWater;
		sublimates.info = new Sublimates.Info(4.0000006E-05f, 0.025f, 1.8f, 1f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
