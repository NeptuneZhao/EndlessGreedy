using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000143 RID: 323
[EntityConfigOrder(2)]
public class BabySquirrelHugConfig : IEntityConfig
{
	// Token: 0x0600064D RID: 1613 RVA: 0x0002B0C1 File Offset: 0x000292C1
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x0002B0C8 File Offset: 0x000292C8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = SquirrelHugConfig.CreateSquirrelHug("SquirrelHugBaby", CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.BABY.NAME, CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.BABY.DESC, "baby_squirrel_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "SquirrelHug", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x0002B106 File Offset: 0x00029306
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x0002B108 File Offset: 0x00029308
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000475 RID: 1141
	public const string ID = "SquirrelHugBaby";
}
