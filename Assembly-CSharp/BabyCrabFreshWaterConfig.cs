using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000FA RID: 250
[EntityConfigOrder(2)]
public class BabyCrabFreshWaterConfig : IEntityConfig
{
	// Token: 0x06000499 RID: 1177 RVA: 0x000249E1 File Offset: 0x00022BE1
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x000249E8 File Offset: 0x00022BE8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabFreshWaterConfig.CreateCrabFreshWater("CrabFreshWaterBaby", CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.BABY.NAME, CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.BABY.DESC, "baby_pincher_kanim", true, null);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "CrabFreshWater", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x00024A27 File Offset: 0x00022C27
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x00024A29 File Offset: 0x00022C29
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000319 RID: 793
	public const string ID = "CrabFreshWaterBaby";
}
