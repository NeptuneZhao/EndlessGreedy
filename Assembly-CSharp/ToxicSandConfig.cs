using System;
using UnityEngine;

// Token: 0x0200031A RID: 794
public class ToxicSandConfig : IOreConfig
{
	// Token: 0x17000044 RID: 68
	// (get) Token: 0x060010A2 RID: 4258 RVA: 0x0005DA7C File Offset: 0x0005BC7C
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.ToxicSand;
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x060010A3 RID: 4259 RVA: 0x0005DA83 File Offset: 0x0005BC83
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.ContaminatedOxygen;
		}
	}

	// Token: 0x060010A4 RID: 4260 RVA: 0x0005DA8A File Offset: 0x0005BC8A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060010A5 RID: 4261 RVA: 0x0005DA94 File Offset: 0x0005BC94
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.ContaminatedOxygenBubble;
		sublimates.info = new Sublimates.Info(2.0000001E-05f, 0.05f, 1.8f, 0.5f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
