using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000131 RID: 305
[EntityConfigOrder(1)]
public class PacuTropicalConfig : IEntityConfig
{
	// Token: 0x060005E0 RID: 1504 RVA: 0x00029624 File Offset: 0x00027824
	public static GameObject CreatePacu(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToWildCreature(BasePacuConfig.CreatePrefab(id, "PacuTropicalBaseTrait", name, desc, anim_file, is_baby, "trp_", 303.15f, 353.15f, 283.15f, 373.15f), PacuTuning.PEN_SIZE_PER_CREATURE, false);
		gameObject.AddOrGet<DecorProvider>().SetValues(PacuTropicalConfig.DECOR);
		return gameObject;
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x00029675 File Offset: 0x00027875
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x0002967C File Offset: 0x0002787C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(EntityTemplates.ExtendEntityToWildCreature(PacuTropicalConfig.CreatePacu("PacuTropical", STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.NAME, STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.DESC, "pacu_kanim", false), PacuTuning.PEN_SIZE_PER_CREATURE, false), "PacuTropicalEgg", STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.EGG_NAME, STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.DESC, "egg_pacu_kanim", PacuTuning.EGG_MASS, "PacuTropicalBaby", 15.000001f, 5f, PacuTuning.EGG_CHANCES_TROPICAL, this.GetDlcIds(), 502, false, true, false, 0.75f, false);
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x00029708 File Offset: 0x00027908
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x0002970A File Offset: 0x0002790A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000417 RID: 1047
	public const string ID = "PacuTropical";

	// Token: 0x04000418 RID: 1048
	public const string BASE_TRAIT_ID = "PacuTropicalBaseTrait";

	// Token: 0x04000419 RID: 1049
	public const string EGG_ID = "PacuTropicalEgg";

	// Token: 0x0400041A RID: 1050
	public static readonly EffectorValues DECOR = TUNING.BUILDINGS.DECOR.BONUS.TIER4;

	// Token: 0x0400041B RID: 1051
	public const int EGG_SORT_ORDER = 502;
}
