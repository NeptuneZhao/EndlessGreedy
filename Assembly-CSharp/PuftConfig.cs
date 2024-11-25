using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000137 RID: 311
public class PuftConfig : IEntityConfig
{
	// Token: 0x06000604 RID: 1540 RVA: 0x00029D80 File Offset: 0x00027F80
	public static GameObject CreatePuft(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BasePuftConfig.BasePuft(id, name, STRINGS.CREATURES.SPECIES.PUFT.DESC, "PuftBaseTrait", anim_file, is_baby, null, 288.15f, 328.15f, 223.15f, 373.15f);
		EntityTemplates.ExtendEntityToWildCreature(prefab, PuftTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("PuftBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		GameObject gameObject = BasePuftConfig.SetupDiet(prefab, SimHashes.ContaminatedOxygen.CreateTag(), SimHashes.SlimeMold.CreateTag(), PuftConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_2, "SlimeLung", 0f, PuftConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = "SlimeLung";
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x00029EED File Offset: 0x000280ED
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x00029EF4 File Offset: 0x000280F4
	public GameObject CreatePrefab()
	{
		GameObject prefab = PuftConfig.CreatePuft("Puft", STRINGS.CREATURES.SPECIES.PUFT.NAME, STRINGS.CREATURES.SPECIES.PUFT.DESC, "puft_kanim", false);
		string eggId = "PuftEgg";
		string eggName = STRINGS.CREATURES.SPECIES.PUFT.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.PUFT.DESC;
		string egg_anim = "egg_puft_kanim";
		float egg_MASS = PuftTuning.EGG_MASS;
		string baby_id = "PuftBaby";
		float fertility_cycles = 45f;
		float incubation_cycles = 15f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_BASE = PuftTuning.EGG_CHANCES_BASE;
		int egg_SORT_ORDER = PuftConfig.EGG_SORT_ORDER;
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_BASE, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x00029F77 File Offset: 0x00028177
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x00029F79 File Offset: 0x00028179
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x04000433 RID: 1075
	public const string ID = "Puft";

	// Token: 0x04000434 RID: 1076
	public const string BASE_TRAIT_ID = "PuftBaseTrait";

	// Token: 0x04000435 RID: 1077
	public const string EGG_ID = "PuftEgg";

	// Token: 0x04000436 RID: 1078
	public const SimHashes CONSUME_ELEMENT = SimHashes.ContaminatedOxygen;

	// Token: 0x04000437 RID: 1079
	public const SimHashes EMIT_ELEMENT = SimHashes.SlimeMold;

	// Token: 0x04000438 RID: 1080
	public const string EMIT_DISEASE = "SlimeLung";

	// Token: 0x04000439 RID: 1081
	public const float EMIT_DISEASE_PER_KG = 0f;

	// Token: 0x0400043A RID: 1082
	private static float KG_ORE_EATEN_PER_CYCLE = 50f;

	// Token: 0x0400043B RID: 1083
	private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / PuftConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x0400043C RID: 1084
	private static float MIN_POOP_SIZE_IN_KG = 15f;

	// Token: 0x0400043D RID: 1085
	public static int EGG_SORT_ORDER = 300;
}
