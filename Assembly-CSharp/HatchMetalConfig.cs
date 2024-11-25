using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200010C RID: 268
[EntityConfigOrder(1)]
public class HatchMetalConfig : IEntityConfig
{
	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000503 RID: 1283 RVA: 0x00026518 File Offset: 0x00024718
	public static HashSet<Tag> METAL_ORE_TAGS
	{
		get
		{
			HashSet<Tag> hashSet = new HashSet<Tag>
			{
				SimHashes.Cuprite.CreateTag(),
				SimHashes.GoldAmalgam.CreateTag(),
				SimHashes.IronOre.CreateTag(),
				SimHashes.Wolframite.CreateTag(),
				SimHashes.AluminumOre.CreateTag()
			};
			if (DlcManager.IsExpansion1Active())
			{
				hashSet.Add(SimHashes.Cobaltite.CreateTag());
			}
			return hashSet;
		}
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x0002659C File Offset: 0x0002479C
	public static GameObject CreateHatch(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseHatchConfig.BaseHatch(id, name, desc, anim_file, "HatchMetalBaseTrait", is_baby, "mtl_"), HatchTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("HatchMetalBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, HatchTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -HatchTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 400f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> diet_infos = BaseHatchConfig.MetalDiet(GameTags.Metal, HatchMetalConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_1, null, 0f);
		return BaseHatchConfig.SetupDiet(prefab, diet_infos, HatchMetalConfig.CALORIES_PER_KG_OF_ORE, HatchMetalConfig.MIN_POOP_SIZE_IN_KG);
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x000266CC File Offset: 0x000248CC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x000266D4 File Offset: 0x000248D4
	public GameObject CreatePrefab()
	{
		GameObject prefab = HatchMetalConfig.CreateHatch("HatchMetal", STRINGS.CREATURES.SPECIES.HATCH.VARIANT_METAL.NAME, STRINGS.CREATURES.SPECIES.HATCH.VARIANT_METAL.DESC, "hatch_kanim", false);
		string eggId = "HatchMetalEgg";
		string eggName = STRINGS.CREATURES.SPECIES.HATCH.VARIANT_METAL.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.HATCH.VARIANT_METAL.DESC;
		string egg_anim = "egg_hatch_kanim";
		float egg_MASS = HatchTuning.EGG_MASS;
		string baby_id = "HatchMetalBaby";
		float fertility_cycles = 60.000004f;
		float incubation_cycles = 20f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_METAL = HatchTuning.EGG_CHANCES_METAL;
		int egg_SORT_ORDER = HatchMetalConfig.EGG_SORT_ORDER;
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_METAL, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x00026757 File Offset: 0x00024957
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x00026759 File Offset: 0x00024959
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400037F RID: 895
	public const string ID = "HatchMetal";

	// Token: 0x04000380 RID: 896
	public const string BASE_TRAIT_ID = "HatchMetalBaseTrait";

	// Token: 0x04000381 RID: 897
	public const string EGG_ID = "HatchMetalEgg";

	// Token: 0x04000382 RID: 898
	private static float KG_ORE_EATEN_PER_CYCLE = 100f;

	// Token: 0x04000383 RID: 899
	private static float CALORIES_PER_KG_OF_ORE = HatchTuning.STANDARD_CALORIES_PER_CYCLE / HatchMetalConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000384 RID: 900
	private static float MIN_POOP_SIZE_IN_KG = 10f;

	// Token: 0x04000385 RID: 901
	public static int EGG_SORT_ORDER = HatchConfig.EGG_SORT_ORDER + 3;
}
