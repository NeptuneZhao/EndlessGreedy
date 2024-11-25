using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200010B RID: 267
[EntityConfigOrder(2)]
public class BabyHatchHardConfig : IEntityConfig
{
	// Token: 0x060004FE RID: 1278 RVA: 0x000264C7 File Offset: 0x000246C7
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x000264CE File Offset: 0x000246CE
	public GameObject CreatePrefab()
	{
		GameObject gameObject = HatchHardConfig.CreateHatch("HatchHardBaby", CREATURES.SPECIES.HATCH.VARIANT_HARD.BABY.NAME, CREATURES.SPECIES.HATCH.VARIANT_HARD.BABY.DESC, "baby_hatch_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "HatchHard", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x0002650C File Offset: 0x0002470C
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x0002650E File Offset: 0x0002470E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400037E RID: 894
	public const string ID = "HatchHardBaby";
}
