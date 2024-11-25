using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000185 RID: 389
public class SuperWormPlantConfig : IEntityConfig
{
	// Token: 0x060007EC RID: 2028 RVA: 0x00034EB1 File Offset: 0x000330B1
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x00034EB8 File Offset: 0x000330B8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = WormPlantConfig.BaseWormPlant("SuperWormPlant", STRINGS.CREATURES.SPECIES.SUPERWORMPLANT.NAME, STRINGS.CREATURES.SPECIES.SUPERWORMPLANT.DESC, "wormwood_kanim", SuperWormPlantConfig.SUPER_DECOR, "WormSuperFruit");
		gameObject.AddOrGet<SeedProducer>().Configure("WormPlantSeed", SeedProducer.ProductionType.Harvest, 1);
		return gameObject;
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x00034F04 File Offset: 0x00033104
	public void OnPrefabInit(GameObject prefab)
	{
		TransformingPlant transformingPlant = prefab.AddOrGet<TransformingPlant>();
		transformingPlant.SubscribeToTransformEvent(GameHashes.HarvestComplete);
		transformingPlant.transformPlantId = "WormPlant";
		prefab.GetComponent<KAnimControllerBase>().SetSymbolVisiblity("flower", false);
		prefab.AddOrGet<StandardCropPlant>().anims = SuperWormPlantConfig.animSet;
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x00034F52 File Offset: 0x00033152
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400059A RID: 1434
	public const string ID = "SuperWormPlant";

	// Token: 0x0400059B RID: 1435
	public static readonly EffectorValues SUPER_DECOR = DECOR.BONUS.TIER1;

	// Token: 0x0400059C RID: 1436
	public const string SUPER_CROP_ID = "WormSuperFruit";

	// Token: 0x0400059D RID: 1437
	public const int CROP_YIELD = 8;

	// Token: 0x0400059E RID: 1438
	private static StandardCropPlant.AnimSet animSet = new StandardCropPlant.AnimSet
	{
		grow = "super_grow",
		grow_pst = "super_grow_pst",
		idle_full = "super_idle_full",
		wilt_base = "super_wilt",
		harvest = "super_harvest"
	};
}
