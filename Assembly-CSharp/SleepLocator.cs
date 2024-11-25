using System;
using UnityEngine;

// Token: 0x020002D8 RID: 728
public class SleepLocator : IEntityConfig
{
	// Token: 0x06000F36 RID: 3894 RVA: 0x0005842D File Offset: 0x0005662D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F37 RID: 3895 RVA: 0x00058434 File Offset: 0x00056634
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(SleepLocator.ID, SleepLocator.ID, false);
		gameObject.AddTag(GameTags.NotConversationTopic);
		gameObject.AddOrGet<Approachable>();
		gameObject.AddOrGet<Sleepable>().isNormalBed = false;
		return gameObject;
	}

	// Token: 0x06000F38 RID: 3896 RVA: 0x00058464 File Offset: 0x00056664
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000F39 RID: 3897 RVA: 0x00058466 File Offset: 0x00056666
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000957 RID: 2391
	public static readonly string ID = "SleepLocator";
}
