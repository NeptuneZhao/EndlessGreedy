using System;
using UnityEngine;

// Token: 0x020002DE RID: 734
public class MinionAssignablesProxyConfig : IEntityConfig
{
	// Token: 0x06000F5D RID: 3933 RVA: 0x00058AF2 File Offset: 0x00056CF2
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F5E RID: 3934 RVA: 0x00058AF9 File Offset: 0x00056CF9
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MinionAssignablesProxyConfig.ID, MinionAssignablesProxyConfig.ID, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<Ownables>();
		gameObject.AddOrGet<Equipment>();
		gameObject.AddOrGet<MinionAssignablesProxy>();
		return gameObject;
	}

	// Token: 0x06000F5F RID: 3935 RVA: 0x00058B27 File Offset: 0x00056D27
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F60 RID: 3936 RVA: 0x00058B29 File Offset: 0x00056D29
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000979 RID: 2425
	public static string ID = "MinionAssignablesProxy";
}
