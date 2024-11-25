using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200012B RID: 299
[EntityConfigOrder(2)]
public class OilFloaterHighTempBabyConfig : IEntityConfig
{
	// Token: 0x060005C2 RID: 1474 RVA: 0x000291EF File Offset: 0x000273EF
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x000291F6 File Offset: 0x000273F6
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterHighTempConfig.CreateOilFloater("OilfloaterHighTempBaby", CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.BABY.NAME, CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.BABY.DESC, "baby_oilfloater_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "OilfloaterHighTemp", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x00029234 File Offset: 0x00027434
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x00029236 File Offset: 0x00027436
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000408 RID: 1032
	public const string ID = "OilfloaterHighTempBaby";
}
