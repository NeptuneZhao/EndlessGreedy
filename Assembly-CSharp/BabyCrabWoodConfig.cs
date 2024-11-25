using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000FC RID: 252
[EntityConfigOrder(2)]
public class BabyCrabWoodConfig : IEntityConfig
{
	// Token: 0x060004A5 RID: 1189 RVA: 0x00024C97 File Offset: 0x00022E97
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x00024CA0 File Offset: 0x00022EA0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabWoodConfig.CreateCrabWood("CrabWoodBaby", CREATURES.SPECIES.CRAB.VARIANT_WOOD.BABY.NAME, CREATURES.SPECIES.CRAB.VARIANT_WOOD.BABY.DESC, "baby_pincher_kanim", true, "BabyCrabWoodShell");
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "CrabWood", "BabyCrabWoodShell", false, 5f);
		return gameObject;
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x00024CF2 File Offset: 0x00022EF2
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x00024CF4 File Offset: 0x00022EF4
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000323 RID: 803
	public const string ID = "CrabWoodBaby";
}
