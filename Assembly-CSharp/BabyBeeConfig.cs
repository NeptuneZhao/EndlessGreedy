using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000B7 RID: 183
[EntityConfigOrder(2)]
public class BabyBeeConfig : IEntityConfig
{
	// Token: 0x0600033B RID: 827 RVA: 0x0001B4B8 File Offset: 0x000196B8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600033C RID: 828 RVA: 0x0001B4C0 File Offset: 0x000196C0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BeeConfig.CreateBee("BeeBaby", CREATURES.SPECIES.BEE.BABY.NAME, CREATURES.SPECIES.BEE.BABY.DESC, "baby_blarva_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Bee", null, true, 2f);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Walker, false);
		return gameObject;
	}

	// Token: 0x0600033D RID: 829 RVA: 0x0001B51A File Offset: 0x0001971A
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600033E RID: 830 RVA: 0x0001B51C File Offset: 0x0001971C
	public void OnSpawn(GameObject inst)
	{
		BaseBeeConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x0400024E RID: 590
	public const string ID = "BeeBaby";
}
