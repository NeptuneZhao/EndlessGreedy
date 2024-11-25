using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200011D RID: 285
[EntityConfigOrder(2)]
public class LightBugPinkBabyConfig : IEntityConfig
{
	// Token: 0x0600056B RID: 1387 RVA: 0x00027D51 File Offset: 0x00025F51
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x00027D58 File Offset: 0x00025F58
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugPinkConfig.CreateLightBug("LightBugPinkBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugPink", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x00027D96 File Offset: 0x00025F96
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x00027D98 File Offset: 0x00025F98
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003C2 RID: 962
	public const string ID = "LightBugPinkBaby";
}
