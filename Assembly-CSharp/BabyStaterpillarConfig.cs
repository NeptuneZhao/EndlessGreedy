using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000145 RID: 325
[EntityConfigOrder(2)]
public class BabyStaterpillarConfig : IEntityConfig
{
	// Token: 0x06000659 RID: 1625 RVA: 0x0002B35B File Offset: 0x0002955B
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x0002B362 File Offset: 0x00029562
	public GameObject CreatePrefab()
	{
		GameObject gameObject = StaterpillarConfig.CreateStaterpillar("StaterpillarBaby", CREATURES.SPECIES.STATERPILLAR.BABY.NAME, CREATURES.SPECIES.STATERPILLAR.BABY.DESC, "baby_caterpillar_kanim", true);
		EntityTemplates.ExtendEntityToBeingABaby(gameObject, "Staterpillar", null, false, 5f);
		return gameObject;
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x0002B3A0 File Offset: 0x000295A0
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x0002B3A2 File Offset: 0x000295A2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400047C RID: 1148
	public const string ID = "StaterpillarBaby";
}
