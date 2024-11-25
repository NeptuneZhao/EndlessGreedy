using System;
using STRINGS;
using UnityEngine;

// Token: 0x020002AE RID: 686
public class CarePackageConfig : IEntityConfig
{
	// Token: 0x06000E4D RID: 3661 RVA: 0x00054A8A File Offset: 0x00052C8A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000E4E RID: 3662 RVA: 0x00054A94 File Offset: 0x00052C94
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateLooseEntity(CarePackageConfig.ID, ITEMS.CARGO_CAPSULE.NAME, ITEMS.CARGO_CAPSULE.DESC, 1f, true, Assets.GetAnim("portal_carepackage_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f, false, 0, SimHashes.Creature, null);
	}

	// Token: 0x06000E4F RID: 3663 RVA: 0x00054AEE File Offset: 0x00052CEE
	public void OnPrefabInit(GameObject go)
	{
		go.AddOrGet<CarePackage>();
	}

	// Token: 0x06000E50 RID: 3664 RVA: 0x00054AF7 File Offset: 0x00052CF7
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x040008FD RID: 2301
	public static readonly string ID = "CarePackage";
}
