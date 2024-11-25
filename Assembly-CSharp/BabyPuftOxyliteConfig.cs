using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200013A RID: 314
[EntityConfigOrder(2)]
public class BabyPuftOxyliteConfig : IEntityConfig
{
	// Token: 0x06000617 RID: 1559 RVA: 0x0002A257 File Offset: 0x00028457
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x0002A25E File Offset: 0x0002845E
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PuftOxyliteConfig.CreatePuftOxylite("PuftOxyliteBaby", CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.BABY.NAME, CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.BABY.DESC, "baby_puft_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PuftOxylite", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x0002A29C File Offset: 0x0002849C
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x0002A29E File Offset: 0x0002849E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000448 RID: 1096
	public const string ID = "PuftOxyliteBaby";
}
