using System;
using UnityEngine;

// Token: 0x020002E9 RID: 745
public class ResearchDestinationConfig : IEntityConfig
{
	// Token: 0x06000FA1 RID: 4001 RVA: 0x00059AE2 File Offset: 0x00057CE2
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000FA2 RID: 4002 RVA: 0x00059AE9 File Offset: 0x00057CE9
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("ResearchDestination", "ResearchDestination", true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<ResearchDestination>();
		return gameObject;
	}

	// Token: 0x06000FA3 RID: 4003 RVA: 0x00059B09 File Offset: 0x00057D09
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000FA4 RID: 4004 RVA: 0x00059B0B File Offset: 0x00057D0B
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000989 RID: 2441
	public const string ID = "ResearchDestination";
}
