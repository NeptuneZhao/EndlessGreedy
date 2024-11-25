using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000FE RID: 254
[EntityConfigOrder(2)]
public class BabyDivergentBeetleConfig : IEntityConfig
{
	// Token: 0x060004B1 RID: 1201 RVA: 0x00024F0D File Offset: 0x0002310D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x00024F14 File Offset: 0x00023114
	public GameObject CreatePrefab()
	{
		GameObject gameObject = DivergentBeetleConfig.CreateDivergentBeetle("DivergentBeetleBaby", CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.BABY.NAME, CREATURES.SPECIES.DIVERGENT.VARIANT_BEETLE.BABY.DESC, "baby_critter_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "DivergentBeetle", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x00024F52 File Offset: 0x00023152
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x00024F54 File Offset: 0x00023154
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400032D RID: 813
	public const string ID = "DivergentBeetleBaby";
}
