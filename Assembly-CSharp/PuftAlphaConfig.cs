using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000133 RID: 307
public class PuftAlphaConfig : IEntityConfig
{
	// Token: 0x060005EC RID: 1516 RVA: 0x00029774 File Offset: 0x00027974
	public static GameObject CreatePuftAlpha(string id, string name, string desc, string anim_file, bool is_baby)
	{
		string symbol_override_prefix = "alp_";
		GameObject gameObject = BasePuftConfig.BasePuft(id, name, desc, "PuftAlphaBaseTrait", anim_file, is_baby, symbol_override_prefix, 293.15f, 313.15f, 223.15f, 373.15f);
		EntityTemplates.ExtendEntityToWildCreature(gameObject, PuftTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("PuftAlphaBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BasePuftConfig.SetupDiet(gameObject, new List<Diet.Info>
		{
			new Diet.Info(new HashSet<Tag>(new Tag[]
			{
				SimHashes.ContaminatedOxygen.CreateTag()
			}), SimHashes.SlimeMold.CreateTag(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, "SlimeLung", 0f, false, Diet.Info.FoodType.EatSolid, false, null),
			new Diet.Info(new HashSet<Tag>(new Tag[]
			{
				SimHashes.ChlorineGas.CreateTag()
			}), SimHashes.BleachStone.CreateTag(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, "SlimeLung", 0f, false, Diet.Info.FoodType.EatSolid, false, null),
			new Diet.Info(new HashSet<Tag>(new Tag[]
			{
				SimHashes.Oxygen.CreateTag()
			}), SimHashes.OxyRock.CreateTag(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, "SlimeLung", 0f, false, Diet.Info.FoodType.EatSolid, false, null)
		}.ToArray(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, PuftAlphaConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = "SlimeLung";
		return gameObject;
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x0002999B File Offset: 0x00027B9B
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x000299A4 File Offset: 0x00027BA4
	public GameObject CreatePrefab()
	{
		GameObject prefab = PuftAlphaConfig.CreatePuftAlpha("PuftAlpha", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.DESC, "puft_kanim", false);
		string eggId = "PuftAlphaEgg";
		string eggName = STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.DESC;
		string egg_anim = "egg_puft_kanim";
		float egg_MASS = PuftTuning.EGG_MASS;
		string baby_id = "PuftAlphaBaby";
		float fertility_cycles = 45f;
		float incubation_cycles = 15f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_ALPHA = PuftTuning.EGG_CHANCES_ALPHA;
		int egg_SORT_ORDER = PuftAlphaConfig.EGG_SORT_ORDER;
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_ALPHA, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x00029A27 File Offset: 0x00027C27
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<KBatchedAnimController>().animScale *= 1.1f;
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x00029A40 File Offset: 0x00027C40
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x0400041D RID: 1053
	public const string ID = "PuftAlpha";

	// Token: 0x0400041E RID: 1054
	public const string BASE_TRAIT_ID = "PuftAlphaBaseTrait";

	// Token: 0x0400041F RID: 1055
	public const string EGG_ID = "PuftAlphaEgg";

	// Token: 0x04000420 RID: 1056
	public const SimHashes CONSUME_ELEMENT = SimHashes.ContaminatedOxygen;

	// Token: 0x04000421 RID: 1057
	public const SimHashes EMIT_ELEMENT = SimHashes.SlimeMold;

	// Token: 0x04000422 RID: 1058
	public const string EMIT_DISEASE = "SlimeLung";

	// Token: 0x04000423 RID: 1059
	public const float EMIT_DISEASE_PER_KG = 0f;

	// Token: 0x04000424 RID: 1060
	private static float KG_ORE_EATEN_PER_CYCLE = 30f;

	// Token: 0x04000425 RID: 1061
	private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / PuftAlphaConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000426 RID: 1062
	private static float MIN_POOP_SIZE_IN_KG = 5f;

	// Token: 0x04000427 RID: 1063
	public static int EGG_SORT_ORDER = PuftConfig.EGG_SORT_ORDER + 1;
}
