using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000109 RID: 265
[EntityConfigOrder(2)]
public class BabyHatchConfig : IEntityConfig
{
	// Token: 0x060004F2 RID: 1266 RVA: 0x00026251 File Offset: 0x00024451
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x00026258 File Offset: 0x00024458
	public GameObject CreatePrefab()
	{
		GameObject gameObject = HatchConfig.CreateHatch("HatchBaby", CREATURES.SPECIES.HATCH.BABY.NAME, CREATURES.SPECIES.HATCH.BABY.DESC, "baby_hatch_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Hatch", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x00026296 File Offset: 0x00024496
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x00026298 File Offset: 0x00024498
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000375 RID: 885
	public const string ID = "HatchBaby";
}
