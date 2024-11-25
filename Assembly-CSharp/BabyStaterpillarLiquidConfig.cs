using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000149 RID: 329
[EntityConfigOrder(2)]
public class BabyStaterpillarLiquidConfig : IEntityConfig
{
	// Token: 0x06000671 RID: 1649 RVA: 0x0002BAE5 File Offset: 0x00029CE5
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x0002BAEC File Offset: 0x00029CEC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = StaterpillarLiquidConfig.CreateStaterpillarLiquid("StaterpillarLiquidBaby", CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.BABY.NAME, CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.BABY.DESC, "baby_caterpillar_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "StaterpillarLiquid", null, false, 5f);
		return gameObject;
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x0002BB2A File Offset: 0x00029D2A
	public void OnPrefabInit(GameObject prefab)
	{
		prefab.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("electric_bolt_c_bloom", false);
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x0002BB42 File Offset: 0x00029D42
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000494 RID: 1172
	public const string ID = "StaterpillarLiquidBaby";
}
