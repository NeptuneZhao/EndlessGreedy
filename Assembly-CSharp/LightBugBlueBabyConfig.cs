using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000115 RID: 277
[EntityConfigOrder(2)]
public class LightBugBlueBabyConfig : IEntityConfig
{
	// Token: 0x0600053B RID: 1339 RVA: 0x00027289 File Offset: 0x00025489
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x00027290 File Offset: 0x00025490
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugBlueConfig.CreateLightBug("LightBugBlueBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugBlue", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x000272CE File Offset: 0x000254CE
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x000272D0 File Offset: 0x000254D0
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003A6 RID: 934
	public const string ID = "LightBugBlueBaby";
}
