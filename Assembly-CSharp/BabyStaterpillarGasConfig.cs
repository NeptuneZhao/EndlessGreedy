using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000147 RID: 327
[EntityConfigOrder(2)]
public class BabyStaterpillarGasConfig : IEntityConfig
{
	// Token: 0x06000665 RID: 1637 RVA: 0x0002B715 File Offset: 0x00029915
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x0002B71C File Offset: 0x0002991C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = StaterpillarGasConfig.CreateStaterpillarGas("StaterpillarGasBaby", CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.BABY.NAME, CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.BABY.DESC, "baby_caterpillar_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "StaterpillarGas", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x0002B75A File Offset: 0x0002995A
	public void OnPrefabInit(GameObject prefab)
	{
		prefab.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("electric_bolt_c_bloom", false);
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x0002B772 File Offset: 0x00029972
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000488 RID: 1160
	public const string ID = "StaterpillarGasBaby";
}
