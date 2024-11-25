using System;
using UnityEngine;

// Token: 0x020002ED RID: 749
public class TelescopeTargetConfig : IEntityConfig
{
	// Token: 0x06000FB7 RID: 4023 RVA: 0x00059BF9 File Offset: 0x00057DF9
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000FB8 RID: 4024 RVA: 0x00059C00 File Offset: 0x00057E00
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("TelescopeTarget", "TelescopeTarget", true);
		gameObject.AddOrGet<TelescopeTarget>();
		return gameObject;
	}

	// Token: 0x06000FB9 RID: 4025 RVA: 0x00059C19 File Offset: 0x00057E19
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000FBA RID: 4026 RVA: 0x00059C1B File Offset: 0x00057E1B
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400098E RID: 2446
	public const string ID = "TelescopeTarget";
}
