using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200012D RID: 301
[EntityConfigOrder(2)]
public class BabyPacuCleanerConfig : IEntityConfig
{
	// Token: 0x060005CE RID: 1486 RVA: 0x00029488 File Offset: 0x00027688
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x0002948F File Offset: 0x0002768F
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PacuCleanerConfig.CreatePacu("PacuCleanerBaby", CREATURES.SPECIES.PACU.VARIANT_CLEANER.BABY.NAME, CREATURES.SPECIES.PACU.VARIANT_CLEANER.BABY.DESC, "baby_pacu_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PacuCleaner", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x000294CD File Offset: 0x000276CD
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x000294CF File Offset: 0x000276CF
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000411 RID: 1041
	public const string ID = "PacuCleanerBaby";
}
