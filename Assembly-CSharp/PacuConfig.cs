using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200012F RID: 303
[EntityConfigOrder(1)]
public class PacuConfig : IEntityConfig
{
	// Token: 0x060005D5 RID: 1493 RVA: 0x000294F0 File Offset: 0x000276F0
	public static GameObject CreatePacu(string id, string name, string desc, string anim_file, bool is_baby)
	{
		return EntityTemplates.ExtendEntityToWildCreature(BasePacuConfig.CreatePrefab(id, "PacuBaseTrait", name, desc, anim_file, is_baby, null, 273.15f, 333.15f, 253.15f, 373.15f), PacuTuning.PEN_SIZE_PER_CREATURE, false);
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x0002952D File Offset: 0x0002772D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x00029534 File Offset: 0x00027734
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFertileCreature(PacuConfig.CreatePacu("Pacu", CREATURES.SPECIES.PACU.NAME, CREATURES.SPECIES.PACU.DESC, "pacu_kanim", false), "PacuEgg", CREATURES.SPECIES.PACU.EGG_NAME, CREATURES.SPECIES.PACU.DESC, "egg_pacu_kanim", PacuTuning.EGG_MASS, "PacuBaby", 15.000001f, 5f, PacuTuning.EGG_CHANCES_BASE, this.GetDlcIds(), 500, false, true, false, 0.75f, false);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x000295C0 File Offset: 0x000277C0
	public void OnPrefabInit(GameObject prefab)
	{
		prefab.AddOrGet<LoopingSounds>();
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x000295C9 File Offset: 0x000277C9
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000412 RID: 1042
	public const string ID = "Pacu";

	// Token: 0x04000413 RID: 1043
	public const string BASE_TRAIT_ID = "PacuBaseTrait";

	// Token: 0x04000414 RID: 1044
	public const string EGG_ID = "PacuEgg";

	// Token: 0x04000415 RID: 1045
	public const int EGG_SORT_ORDER = 500;
}
