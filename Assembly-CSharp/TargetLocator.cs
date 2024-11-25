using System;
using UnityEngine;

// Token: 0x020002D6 RID: 726
public class TargetLocator : IEntityConfig
{
	// Token: 0x06000F2A RID: 3882 RVA: 0x000583AE File Offset: 0x000565AE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F2B RID: 3883 RVA: 0x000583B5 File Offset: 0x000565B5
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(TargetLocator.ID, TargetLocator.ID, false);
		gameObject.AddTag(GameTags.NotConversationTopic);
		return gameObject;
	}

	// Token: 0x06000F2C RID: 3884 RVA: 0x000583D2 File Offset: 0x000565D2
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000F2D RID: 3885 RVA: 0x000583D4 File Offset: 0x000565D4
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000955 RID: 2389
	public static readonly string ID = "TargetLocator";
}
