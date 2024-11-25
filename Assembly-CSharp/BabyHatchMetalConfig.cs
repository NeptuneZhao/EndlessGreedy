using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200010D RID: 269
[EntityConfigOrder(2)]
public class BabyHatchMetalConfig : IEntityConfig
{
	// Token: 0x0600050B RID: 1291 RVA: 0x00026795 File Offset: 0x00024995
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x0002679C File Offset: 0x0002499C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = HatchMetalConfig.CreateHatch("HatchMetalBaby", CREATURES.SPECIES.HATCH.VARIANT_METAL.BABY.NAME, CREATURES.SPECIES.HATCH.VARIANT_METAL.BABY.DESC, "baby_hatch_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "HatchMetal", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x000267DA File Offset: 0x000249DA
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x000267DC File Offset: 0x000249DC
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000386 RID: 902
	public const string ID = "HatchMetalBaby";
}
