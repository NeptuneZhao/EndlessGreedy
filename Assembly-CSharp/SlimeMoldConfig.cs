using System;
using UnityEngine;

// Token: 0x02000319 RID: 793
public class SlimeMoldConfig : IOreConfig
{
	// Token: 0x17000042 RID: 66
	// (get) Token: 0x0600109D RID: 4253 RVA: 0x0005DA08 File Offset: 0x0005BC08
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.SlimeMold;
		}
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x0600109E RID: 4254 RVA: 0x0005DA0F File Offset: 0x0005BC0F
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.ContaminatedOxygen;
		}
	}

	// Token: 0x0600109F RID: 4255 RVA: 0x0005DA16 File Offset: 0x0005BC16
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060010A0 RID: 4256 RVA: 0x0005DA20 File Offset: 0x0005BC20
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.ContaminatedOxygenBubble;
		sublimates.info = new Sublimates.Info(0.025f, 0.125f, 1.8f, 0f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
