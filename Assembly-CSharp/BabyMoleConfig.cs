using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000121 RID: 289
[EntityConfigOrder(2)]
public class BabyMoleConfig : IEntityConfig
{
	// Token: 0x06000584 RID: 1412 RVA: 0x00028385 File Offset: 0x00026585
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x0002838C File Offset: 0x0002658C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = MoleConfig.CreateMole("MoleBaby", CREATURES.SPECIES.MOLE.BABY.NAME, CREATURES.SPECIES.MOLE.BABY.DESC, "baby_driller_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Mole", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x000283CA File Offset: 0x000265CA
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x000283CC File Offset: 0x000265CC
	public void OnSpawn(GameObject inst)
	{
		MoleConfig.SetSpawnNavType(inst);
	}

	// Token: 0x040003D0 RID: 976
	public const string ID = "MoleBaby";
}
