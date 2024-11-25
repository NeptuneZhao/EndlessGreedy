using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000111 RID: 273
[EntityConfigOrder(2)]
public class BabyIceBellyConfig : IEntityConfig
{
	// Token: 0x06000523 RID: 1315 RVA: 0x00026CC5 File Offset: 0x00024EC5
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x00026CCC File Offset: 0x00024ECC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = IceBellyConfig.CreateIceBelly("IceBellyBaby", CREATURES.SPECIES.ICEBELLY.BABY.NAME, CREATURES.SPECIES.ICEBELLY.BABY.DESC, "baby_icebelly_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "IceBelly", null, false, 5f).AddOrGetDef<BabyMonitor.Def>().configureAdultOnMaturation = delegate(GameObject go)
		{
			AmountInstance amountInstance = Db.Get().Amounts.ScaleGrowth.Lookup(go);
			amountInstance.value = amountInstance.GetMax() * IceBellyConfig.SCALE_INITIAL_GROWTH_PCT;
		};
		return gameObject;
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x00026D3D File Offset: 0x00024F3D
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x00026D3F File Offset: 0x00024F3F
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000398 RID: 920
	public const string ID = "IceBellyBaby";
}
