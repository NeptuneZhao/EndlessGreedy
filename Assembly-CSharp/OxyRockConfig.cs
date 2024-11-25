using System;
using UnityEngine;

// Token: 0x02000318 RID: 792
public class OxyRockConfig : IOreConfig
{
	// Token: 0x17000040 RID: 64
	// (get) Token: 0x06001098 RID: 4248 RVA: 0x0005D997 File Offset: 0x0005BB97
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.OxyRock;
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x06001099 RID: 4249 RVA: 0x0005D99E File Offset: 0x0005BB9E
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.Oxygen;
		}
	}

	// Token: 0x0600109A RID: 4250 RVA: 0x0005D9A5 File Offset: 0x0005BBA5
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600109B RID: 4251 RVA: 0x0005D9AC File Offset: 0x0005BBAC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.OxygenEmissionBubbles;
		sublimates.info = new Sublimates.Info(0.010000001f, 0.0050000004f, 1.8f, 0.7f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
