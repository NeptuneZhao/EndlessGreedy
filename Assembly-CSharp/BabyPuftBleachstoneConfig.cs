using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000136 RID: 310
[EntityConfigOrder(2)]
public class BabyPuftBleachstoneConfig : IEntityConfig
{
	// Token: 0x060005FF RID: 1535 RVA: 0x00029D27 File Offset: 0x00027F27
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x00029D2E File Offset: 0x00027F2E
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PuftBleachstoneConfig.CreatePuftBleachstone("PuftBleachstoneBaby", CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.BABY.NAME, CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.BABY.DESC, "baby_puft_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PuftBleachstone", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x00029D6C File Offset: 0x00027F6C
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x00029D6E File Offset: 0x00027F6E
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x04000432 RID: 1074
	public const string ID = "PuftBleachstoneBaby";
}
