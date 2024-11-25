using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000127 RID: 295
[EntityConfigOrder(2)]
public class OilFloaterBabyConfig : IEntityConfig
{
	// Token: 0x060005AA RID: 1450 RVA: 0x00028D21 File Offset: 0x00026F21
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x00028D28 File Offset: 0x00026F28
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterConfig.CreateOilFloater("OilfloaterBaby", CREATURES.SPECIES.OILFLOATER.BABY.NAME, CREATURES.SPECIES.OILFLOATER.BABY.DESC, "baby_oilfloater_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Oilfloater", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x00028D66 File Offset: 0x00026F66
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x00028D68 File Offset: 0x00026F68
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003F6 RID: 1014
	public const string ID = "OilfloaterBaby";
}
