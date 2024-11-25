using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000132 RID: 306
[EntityConfigOrder(2)]
public class BabyPacuTropicalConfig : IEntityConfig
{
	// Token: 0x060005E7 RID: 1511 RVA: 0x00029720 File Offset: 0x00027920
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x00029727 File Offset: 0x00027927
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PacuTropicalConfig.CreatePacu("PacuTropicalBaby", CREATURES.SPECIES.PACU.VARIANT_TROPICAL.BABY.NAME, CREATURES.SPECIES.PACU.VARIANT_TROPICAL.BABY.DESC, "baby_pacu_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "PacuTropical", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x00029765 File Offset: 0x00027965
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x00029767 File Offset: 0x00027967
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400041C RID: 1052
	public const string ID = "PacuTropicalBaby";
}
