using System;
using UnityEngine;

// Token: 0x020002EA RID: 746
public class SimpleFXConfig : IEntityConfig
{
	// Token: 0x06000FA6 RID: 4006 RVA: 0x00059B15 File Offset: 0x00057D15
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000FA7 RID: 4007 RVA: 0x00059B1C File Offset: 0x00057D1C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(SimpleFXConfig.ID, SimpleFXConfig.ID, false);
		gameObject.AddOrGet<KBatchedAnimController>();
		return gameObject;
	}

	// Token: 0x06000FA8 RID: 4008 RVA: 0x00059B35 File Offset: 0x00057D35
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000FA9 RID: 4009 RVA: 0x00059B37 File Offset: 0x00057D37
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x0400098A RID: 2442
	public static readonly string ID = "SimpleFX";
}
