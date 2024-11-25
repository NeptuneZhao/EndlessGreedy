using System;
using UnityEngine;

// Token: 0x02000313 RID: 787
public class BleachStoneConfig : IOreConfig
{
	// Token: 0x1700003A RID: 58
	// (get) Token: 0x06001085 RID: 4229 RVA: 0x0005D5D2 File Offset: 0x0005B7D2
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.BleachStone;
		}
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x06001086 RID: 4230 RVA: 0x0005D5D9 File Offset: 0x0005B7D9
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.ChlorineGas;
		}
	}

	// Token: 0x06001087 RID: 4231 RVA: 0x0005D5E0 File Offset: 0x0005B7E0
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001088 RID: 4232 RVA: 0x0005D5E8 File Offset: 0x0005B7E8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.BleachStoneEmissionBubbles;
		sublimates.info = new Sublimates.Info(0.00020000001f, 0.0025000002f, 1.8f, 0.5f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
