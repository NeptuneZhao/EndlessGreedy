using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200011F RID: 287
[EntityConfigOrder(2)]
public class LightBugPurpleBabyConfig : IEntityConfig
{
	// Token: 0x06000577 RID: 1399 RVA: 0x00028003 File Offset: 0x00026203
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x0002800A File Offset: 0x0002620A
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugPurpleConfig.CreateLightBug("LightBugPurpleBaby", CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.BABY.NAME, CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.BABY.DESC, "baby_lightbug_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "LightBugPurple", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x00028048 File Offset: 0x00026248
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x0002804A File Offset: 0x0002624A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003C9 RID: 969
	public const string ID = "LightBugPurpleBaby";
}
