using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000139 RID: 313
public class PuftOxyliteConfig : IEntityConfig
{
	// Token: 0x06000610 RID: 1552 RVA: 0x0002A00C File Offset: 0x0002820C
	public static GameObject CreatePuftOxylite(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BasePuftConfig.BasePuft(id, name, desc, "PuftOxyliteBaseTrait", anim_file, is_baby, "com_", 273.15f, 333.15f, 223.15f, 373.15f);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, PuftTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("PuftOxyliteBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BasePuftConfig.SetupDiet(gameObject, SimHashes.Oxygen.CreateTag(), SimHashes.OxyRock.CreateTag(), PuftOxyliteConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_2, null, 0f, PuftOxyliteConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[]
		{
			SimHashes.OxyRock.CreateTag(),
			GameTags.Creatures.FlyersLure
		};
		return gameObject;
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x0002A187 File Offset: 0x00028387
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x0002A190 File Offset: 0x00028390
	public GameObject CreatePrefab()
	{
		GameObject prefab = PuftOxyliteConfig.CreatePuftOxylite("PuftOxylite", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.DESC, "puft_kanim", false);
		string eggId = "PuftOxyliteEgg";
		string eggName = STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.DESC;
		string egg_anim = "egg_puft_kanim";
		float egg_MASS = PuftTuning.EGG_MASS;
		string baby_id = "PuftOxyliteBaby";
		float fertility_cycles = 45f;
		float incubation_cycles = 15f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_OXYLITE = PuftTuning.EGG_CHANCES_OXYLITE;
		int egg_SORT_ORDER = PuftOxyliteConfig.EGG_SORT_ORDER;
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_OXYLITE, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x0002A213 File Offset: 0x00028413
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x0002A215 File Offset: 0x00028415
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x0400043F RID: 1087
	public const string ID = "PuftOxylite";

	// Token: 0x04000440 RID: 1088
	public const string BASE_TRAIT_ID = "PuftOxyliteBaseTrait";

	// Token: 0x04000441 RID: 1089
	public const string EGG_ID = "PuftOxyliteEgg";

	// Token: 0x04000442 RID: 1090
	public const SimHashes CONSUME_ELEMENT = SimHashes.Oxygen;

	// Token: 0x04000443 RID: 1091
	public const SimHashes EMIT_ELEMENT = SimHashes.OxyRock;

	// Token: 0x04000444 RID: 1092
	private static float KG_ORE_EATEN_PER_CYCLE = 50f;

	// Token: 0x04000445 RID: 1093
	private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / PuftOxyliteConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000446 RID: 1094
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x04000447 RID: 1095
	public static int EGG_SORT_ORDER = PuftConfig.EGG_SORT_ORDER + 2;
}
