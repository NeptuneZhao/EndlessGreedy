using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020002E2 RID: 738
public class MissileBasicConfig : IEntityConfig
{
	// Token: 0x06000F78 RID: 3960 RVA: 0x000593A1 File Offset: 0x000575A1
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F79 RID: 3961 RVA: 0x000593A8 File Offset: 0x000575A8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("MissileBasic", ITEMS.MISSILE_BASIC.NAME, ITEMS.MISSILE_BASIC.DESC, 10f, true, Assets.GetAnim("missile_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Iron, new List<Tag>());
		gameObject.AddTag(GameTags.IndustrialProduct);
		gameObject.AddOrGetDef<MissileProjectile.Def>();
		gameObject.AddOrGet<EntitySplitter>().maxStackSize = 50f;
		return gameObject;
	}

	// Token: 0x06000F7A RID: 3962 RVA: 0x00059428 File Offset: 0x00057628
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000F7B RID: 3963 RVA: 0x0005942A File Offset: 0x0005762A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000980 RID: 2432
	public const string ID = "MissileBasic";

	// Token: 0x04000981 RID: 2433
	public static ComplexRecipe recipe;

	// Token: 0x04000982 RID: 2434
	public const float MASS_PER_MISSILE = 10f;
}
