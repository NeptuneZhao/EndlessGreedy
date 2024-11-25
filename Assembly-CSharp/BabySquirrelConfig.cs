using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000141 RID: 321
[EntityConfigOrder(2)]
public class BabySquirrelConfig : IEntityConfig
{
	// Token: 0x06000641 RID: 1601 RVA: 0x0002AE4D File Offset: 0x0002904D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x0002AE54 File Offset: 0x00029054
	public GameObject CreatePrefab()
	{
		GameObject gameObject = SquirrelConfig.CreateSquirrel("SquirrelBaby", CREATURES.SPECIES.SQUIRREL.BABY.NAME, CREATURES.SPECIES.SQUIRREL.BABY.DESC, "baby_squirrel_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Squirrel", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x0002AE92 File Offset: 0x00029092
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000644 RID: 1604 RVA: 0x0002AE94 File Offset: 0x00029094
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000469 RID: 1129
	public const string ID = "SquirrelBaby";
}
