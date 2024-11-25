using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000134 RID: 308
[EntityConfigOrder(2)]
public class BabyPuftAlphaConfig : IEntityConfig
{
	// Token: 0x060005F3 RID: 1523 RVA: 0x00029A82 File Offset: 0x00027C82
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x00029A89 File Offset: 0x00027C89
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PuftAlphaConfig.CreatePuftAlpha("PuftAlphaBaby", CREATURES.SPECIES.PUFT.VARIANT_ALPHA.BABY.NAME, CREATURES.SPECIES.PUFT.VARIANT_ALPHA.BABY.DESC, "baby_puft_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PuftAlpha", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x00029AC7 File Offset: 0x00027CC7
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x00029AC9 File Offset: 0x00027CC9
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x04000428 RID: 1064
	public const string ID = "PuftAlphaBaby";
}
