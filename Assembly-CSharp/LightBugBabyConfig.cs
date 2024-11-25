using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000117 RID: 279
[EntityConfigOrder(2)]
public class LightBugBabyConfig : IEntityConfig
{
	// Token: 0x06000547 RID: 1351 RVA: 0x0002753D File Offset: 0x0002573D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x00027544 File Offset: 0x00025744
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugConfig.CreateLightBug("LightBugBaby", CREATURES.SPECIES.LIGHTBUG.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBug", null, false, 5f);
		gameObject.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightSource, false);
		return gameObject;
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x0002759E File Offset: 0x0002579E
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x000275A0 File Offset: 0x000257A0
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003AD RID: 941
	public const string ID = "LightBugBaby";
}
