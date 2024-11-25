using System;
using UnityEngine;

// Token: 0x020002E5 RID: 741
public class OrbitalBGConfig : IEntityConfig
{
	// Token: 0x06000F8B RID: 3979 RVA: 0x000597F2 File Offset: 0x000579F2
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F8C RID: 3980 RVA: 0x000597F9 File Offset: 0x000579F9
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(OrbitalBGConfig.ID, OrbitalBGConfig.ID, false);
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<OrbitalObject>();
		gameObject.AddOrGet<SaveLoadRoot>();
		return gameObject;
	}

	// Token: 0x06000F8D RID: 3981 RVA: 0x00059820 File Offset: 0x00057A20
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000F8E RID: 3982 RVA: 0x00059822 File Offset: 0x00057A22
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000985 RID: 2437
	public static string ID = "OrbitalBG";
}
