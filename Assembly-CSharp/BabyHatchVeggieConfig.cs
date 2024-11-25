using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200010F RID: 271
[EntityConfigOrder(2)]
public class BabyHatchVeggieConfig : IEntityConfig
{
	// Token: 0x06000517 RID: 1303 RVA: 0x00026A0B File Offset: 0x00024C0B
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x00026A12 File Offset: 0x00024C12
	public GameObject CreatePrefab()
	{
		GameObject gameObject = HatchVeggieConfig.CreateHatch("HatchVeggieBaby", CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.BABY.NAME, CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.BABY.DESC, "baby_hatch_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "HatchVeggie", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x00026A50 File Offset: 0x00024C50
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x00026A52 File Offset: 0x00024C52
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400038F RID: 911
	public const string ID = "HatchVeggieBaby";
}
