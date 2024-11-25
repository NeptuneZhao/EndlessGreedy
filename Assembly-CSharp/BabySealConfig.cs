using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200013E RID: 318
[EntityConfigOrder(2)]
public class BabySealConfig : IEntityConfig
{
	// Token: 0x06000630 RID: 1584 RVA: 0x0002AB01 File Offset: 0x00028D01
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x0002AB08 File Offset: 0x00028D08
	public GameObject CreatePrefab()
	{
		GameObject gameObject = SealConfig.CreateSeal("SealBaby", CREATURES.SPECIES.SEAL.BABY.NAME, CREATURES.SPECIES.SEAL.BABY.DESC, "baby_seal_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Seal", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x0002AB46 File Offset: 0x00028D46
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x0002AB48 File Offset: 0x00028D48
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400045C RID: 1116
	public const string ID = "SealBaby";
}
