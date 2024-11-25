using System;
using UnityEngine;

// Token: 0x02000317 RID: 791
public class NuclearWasteConfig : IOreConfig
{
	// Token: 0x1700003F RID: 63
	// (get) Token: 0x06001094 RID: 4244 RVA: 0x0005D925 File Offset: 0x0005BB25
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.NuclearWaste;
		}
	}

	// Token: 0x06001095 RID: 4245 RVA: 0x0005D92C File Offset: 0x0005BB2C
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001096 RID: 4246 RVA: 0x0005D934 File Offset: 0x0005BB34
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLiquidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.decayStorage = true;
		sublimates.spawnFXHash = SpawnFXHashes.NuclearWasteDrip;
		sublimates.info = new Sublimates.Info(0.066f, 6.6f, 1000f, 0f, this.ElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
