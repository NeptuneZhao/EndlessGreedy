using System;
using UnityEngine;

// Token: 0x020002E8 RID: 744
public class RepairableStorageProxy : IEntityConfig
{
	// Token: 0x06000F9B RID: 3995 RVA: 0x00059A9F File Offset: 0x00057C9F
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F9C RID: 3996 RVA: 0x00059AA6 File Offset: 0x00057CA6
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(RepairableStorageProxy.ID, RepairableStorageProxy.ID, true);
		gameObject.AddOrGet<Storage>();
		gameObject.AddTag(GameTags.NotConversationTopic);
		return gameObject;
	}

	// Token: 0x06000F9D RID: 3997 RVA: 0x00059ACA File Offset: 0x00057CCA
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000F9E RID: 3998 RVA: 0x00059ACC File Offset: 0x00057CCC
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000988 RID: 2440
	public static string ID = "RepairableStorageProxy";
}
