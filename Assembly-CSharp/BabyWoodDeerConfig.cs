using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200014B RID: 331
[EntityConfigOrder(2)]
public class BabyWoodDeerConfig : IEntityConfig
{
	// Token: 0x0600067D RID: 1661 RVA: 0x0002BEF7 File Offset: 0x0002A0F7
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x0002BF00 File Offset: 0x0002A100
	public GameObject CreatePrefab()
	{
		GameObject gameObject = WoodDeerConfig.CreateWoodDeer("WoodDeerBaby", CREATURES.SPECIES.WOODDEER.BABY.NAME, CREATURES.SPECIES.WOODDEER.BABY.DESC, "baby_ice_floof_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "WoodDeer", null, false, 5f).AddOrGetDef<BabyMonitor.Def>().configureAdultOnMaturation = delegate(GameObject go)
		{
			AmountInstance amountInstance = Db.Get().Amounts.ScaleGrowth.Lookup(go);
			amountInstance.value = amountInstance.GetMax() * WoodDeerConfig.ANTLER_STARTING_GROWTH_PCT;
		};
		return gameObject;
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x0002BF71 File Offset: 0x0002A171
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x0002BF73 File Offset: 0x0002A173
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040004A6 RID: 1190
	public const string ID = "WoodDeerBaby";
}
