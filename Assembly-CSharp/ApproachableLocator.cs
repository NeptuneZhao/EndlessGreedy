using System;
using UnityEngine;

// Token: 0x020002D7 RID: 727
public class ApproachableLocator : IEntityConfig
{
	// Token: 0x06000F30 RID: 3888 RVA: 0x000583EA File Offset: 0x000565EA
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F31 RID: 3889 RVA: 0x000583F1 File Offset: 0x000565F1
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(ApproachableLocator.ID, ApproachableLocator.ID, false);
		gameObject.AddTag(GameTags.NotConversationTopic);
		gameObject.AddOrGet<Approachable>();
		return gameObject;
	}

	// Token: 0x06000F32 RID: 3890 RVA: 0x00058415 File Offset: 0x00056615
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000F33 RID: 3891 RVA: 0x00058417 File Offset: 0x00056617
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000956 RID: 2390
	public static readonly string ID = "ApproachableLocator";
}
