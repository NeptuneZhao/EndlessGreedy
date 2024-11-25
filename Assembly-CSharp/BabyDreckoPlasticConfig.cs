using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000104 RID: 260
[EntityConfigOrder(2)]
public class BabyDreckoPlasticConfig : IEntityConfig
{
	// Token: 0x060004D5 RID: 1237 RVA: 0x0002595D File Offset: 0x00023B5D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x00025964 File Offset: 0x00023B64
	public GameObject CreatePrefab()
	{
		GameObject gameObject = DreckoPlasticConfig.CreateDrecko("DreckoPlasticBaby", CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.BABY.NAME, CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.BABY.DESC, "baby_drecko_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "DreckoPlastic", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x000259A2 File Offset: 0x00023BA2
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x000259A4 File Offset: 0x00023BA4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000358 RID: 856
	public const string ID = "DreckoPlasticBaby";
}
