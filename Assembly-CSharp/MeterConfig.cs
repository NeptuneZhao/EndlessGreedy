using System;
using UnityEngine;

// Token: 0x020002DC RID: 732
public class MeterConfig : IEntityConfig
{
	// Token: 0x06000F51 RID: 3921 RVA: 0x000589A3 File Offset: 0x00056BA3
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F52 RID: 3922 RVA: 0x000589AA File Offset: 0x00056BAA
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MeterConfig.ID, MeterConfig.ID, false);
		gameObject.AddOrGet<KBatchedAnimController>();
		gameObject.AddOrGet<KBatchedAnimTracker>();
		return gameObject;
	}

	// Token: 0x06000F53 RID: 3923 RVA: 0x000589CA File Offset: 0x00056BCA
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000F54 RID: 3924 RVA: 0x000589CC File Offset: 0x00056BCC
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000975 RID: 2421
	public static readonly string ID = "Meter";
}
