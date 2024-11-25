using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200011B RID: 283
[EntityConfigOrder(2)]
public class LightBugOrangeBabyConfig : IEntityConfig
{
	// Token: 0x0600055F RID: 1375 RVA: 0x00027A8F File Offset: 0x00025C8F
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x00027A96 File Offset: 0x00025C96
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugOrangeConfig.CreateLightBug("LightBugOrangeBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugOrange", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x00027AD4 File Offset: 0x00025CD4
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x00027AD6 File Offset: 0x00025CD6
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003BB RID: 955
	public const string ID = "LightBugOrangeBaby";
}
