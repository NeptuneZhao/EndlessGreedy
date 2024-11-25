using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000138 RID: 312
[EntityConfigOrder(2)]
public class BabyPuftConfig : IEntityConfig
{
	// Token: 0x0600060B RID: 1547 RVA: 0x00029FB9 File Offset: 0x000281B9
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x00029FC0 File Offset: 0x000281C0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PuftConfig.CreatePuft("PuftBaby", CREATURES.SPECIES.PUFT.BABY.NAME, CREATURES.SPECIES.PUFT.BABY.DESC, "baby_puft_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Puft", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x00029FFE File Offset: 0x000281FE
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x0002A000 File Offset: 0x00028200
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400043E RID: 1086
	public const string ID = "PuftBaby";
}
