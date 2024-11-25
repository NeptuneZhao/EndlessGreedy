using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000F8 RID: 248
[EntityConfigOrder(2)]
public class BabyCrabConfig : IEntityConfig
{
	// Token: 0x0600048D RID: 1165 RVA: 0x000245D3 File Offset: 0x000227D3
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x000245DC File Offset: 0x000227DC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabConfig.CreateCrab("CrabBaby", CREATURES.SPECIES.CRAB.BABY.NAME, CREATURES.SPECIES.CRAB.BABY.DESC, "baby_pincher_kanim", true, "BabyCrabShell");
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Crab", "BabyCrabShell", false, 5f);
		return gameObject;
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x0002462E File Offset: 0x0002282E
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x00024630 File Offset: 0x00022830
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400030F RID: 783
	public const string ID = "CrabBaby";
}
