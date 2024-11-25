using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000129 RID: 297
[EntityConfigOrder(2)]
public class OilFloaterDecorBabyConfig : IEntityConfig
{
	// Token: 0x060005B6 RID: 1462 RVA: 0x00028F89 File Offset: 0x00027189
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x00028F90 File Offset: 0x00027190
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterDecorConfig.CreateOilFloater("OilfloaterDecorBaby", CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.BABY.NAME, CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.BABY.DESC, "baby_oilfloater_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "OilfloaterDecor", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x00028FCE File Offset: 0x000271CE
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x00028FD0 File Offset: 0x000271D0
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003FE RID: 1022
	public const string ID = "OilfloaterDecorBaby";
}
