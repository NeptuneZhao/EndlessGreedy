using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000119 RID: 281
[EntityConfigOrder(2)]
public class LightBugCrystalBabyConfig : IEntityConfig
{
	// Token: 0x06000553 RID: 1363 RVA: 0x000277ED File Offset: 0x000259ED
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x000277F4 File Offset: 0x000259F4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugCrystalConfig.CreateLightBug("LightBugCrystalBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugCrystal", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x00027832 File Offset: 0x00025A32
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x00027834 File Offset: 0x00025A34
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003B4 RID: 948
	public const string ID = "LightBugCrystalBaby";
}
