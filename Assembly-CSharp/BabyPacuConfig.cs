using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000130 RID: 304
[EntityConfigOrder(2)]
public class BabyPacuConfig : IEntityConfig
{
	// Token: 0x060005DB RID: 1499 RVA: 0x000295D3 File Offset: 0x000277D3
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x000295DA File Offset: 0x000277DA
	public GameObject CreatePrefab()
	{
		GameObject gameObject = PacuConfig.CreatePacu("PacuBaby", CREATURES.SPECIES.PACU.BABY.NAME, CREATURES.SPECIES.PACU.BABY.DESC, "baby_pacu_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Pacu", null, false, 5f);
		return gameObject;
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x00029618 File Offset: 0x00027818
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x0002961A File Offset: 0x0002781A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000416 RID: 1046
	public const string ID = "PacuBaby";
}
