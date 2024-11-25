using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000113 RID: 275
[EntityConfigOrder(2)]
public class LightBugBlackBabyConfig : IEntityConfig
{
	// Token: 0x0600052F RID: 1327 RVA: 0x00026FC3 File Offset: 0x000251C3
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x00026FCA File Offset: 0x000251CA
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugBlackConfig.CreateLightBug("LightBugBlackBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugBlack", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x00027008 File Offset: 0x00025208
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x0002700A File Offset: 0x0002520A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400039F RID: 927
	public const string ID = "LightBugBlackBaby";
}
