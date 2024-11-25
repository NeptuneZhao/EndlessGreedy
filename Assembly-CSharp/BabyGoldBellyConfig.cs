using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000107 RID: 263
[EntityConfigOrder(2)]
public class BabyGoldBellyConfig : IEntityConfig
{
	// Token: 0x060004E6 RID: 1254 RVA: 0x00025FA8 File Offset: 0x000241A8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x00025FB0 File Offset: 0x000241B0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = GoldBellyConfig.CreateGoldBelly("GoldBellyBaby", CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.BABY.NAME, CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.BABY.DESC, "baby_icebelly_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "GoldBelly", null, false, 5f).AddOrGetDef<BabyMonitor.Def>().configureAdultOnMaturation = delegate(GameObject go)
		{
			AmountInstance amountInstance = Db.Get().Amounts.ScaleGrowth.Lookup(go);
			amountInstance.value = amountInstance.GetMax() * GoldBellyConfig.SCALE_INITIAL_GROWTH_PCT;
		};
		return gameObject;
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x00026021 File Offset: 0x00024221
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x00026023 File Offset: 0x00024223
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400036C RID: 876
	public const string ID = "GoldBellyBaby";
}
