using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000123 RID: 291
[EntityConfigOrder(2)]
public class BabyMoleDelicacyConfig : IEntityConfig
{
	// Token: 0x06000591 RID: 1425 RVA: 0x00028782 File Offset: 0x00026982
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x00028789 File Offset: 0x00026989
	public GameObject CreatePrefab()
	{
		GameObject gameObject = MoleDelicacyConfig.CreateMole("MoleDelicacyBaby", CREATURES.SPECIES.MOLE.VARIANT_DELICACY.BABY.NAME, CREATURES.SPECIES.MOLE.VARIANT_DELICACY.BABY.DESC, "baby_driller_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "MoleDelicacy", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x000287C7 File Offset: 0x000269C7
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x000287C9 File Offset: 0x000269C9
	public void OnSpawn(GameObject inst)
	{
		MoleConfig.SetSpawnNavType(inst);
	}

	// Token: 0x040003DE RID: 990
	public const string ID = "MoleDelicacyBaby";
}
