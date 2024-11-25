using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000100 RID: 256
[EntityConfigOrder(2)]
public class BabyWormConfig : IEntityConfig
{
	// Token: 0x060004BD RID: 1213 RVA: 0x00025295 File Offset: 0x00023495
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x0002529C File Offset: 0x0002349C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = DivergentWormConfig.CreateWorm("DivergentWormBaby", CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.BABY.NAME, CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.BABY.DESC, "baby_worm_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "DivergentWorm", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x000252DA File Offset: 0x000234DA
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x000252DC File Offset: 0x000234DC
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400033C RID: 828
	public const string ID = "DivergentWormBaby";
}
