using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000135 RID: 309
public class PuftBleachstoneConfig : IEntityConfig
{
	// Token: 0x060005F8 RID: 1528 RVA: 0x00029ADC File Offset: 0x00027CDC
	public static GameObject CreatePuftBleachstone(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BasePuftConfig.BasePuft(id, name, desc, "PuftBleachstoneBaseTrait", anim_file, is_baby, "anti_", 273.15f, 333.15f, 223.15f, 373.15f);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, PuftTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("PuftBleachstoneBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BasePuftConfig.SetupDiet(gameObject, SimHashes.ChlorineGas.CreateTag(), SimHashes.BleachStone.CreateTag(), PuftBleachstoneConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_2, null, 0f, PuftBleachstoneConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[]
		{
			SimHashes.BleachStone.CreateTag(),
			GameTags.Creatures.FlyersLure
		};
		return gameObject;
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x00029C57 File Offset: 0x00027E57
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x00029C60 File Offset: 0x00027E60
	public GameObject CreatePrefab()
	{
		GameObject prefab = PuftBleachstoneConfig.CreatePuftBleachstone("PuftBleachstone", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.DESC, "puft_kanim", false);
		string eggId = "PuftBleachstoneEgg";
		string eggName = STRINGS.CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.DESC;
		string egg_anim = "egg_puft_kanim";
		float egg_MASS = PuftTuning.EGG_MASS;
		string baby_id = "PuftBleachstoneBaby";
		float fertility_cycles = 45f;
		float incubation_cycles = 15f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_BLEACHSTONE = PuftTuning.EGG_CHANCES_BLEACHSTONE;
		int egg_SORT_ORDER = PuftBleachstoneConfig.EGG_SORT_ORDER;
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_BLEACHSTONE, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x00029CE3 File Offset: 0x00027EE3
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x00029CE5 File Offset: 0x00027EE5
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x04000429 RID: 1065
	public const string ID = "PuftBleachstone";

	// Token: 0x0400042A RID: 1066
	public const string BASE_TRAIT_ID = "PuftBleachstoneBaseTrait";

	// Token: 0x0400042B RID: 1067
	public const string EGG_ID = "PuftBleachstoneEgg";

	// Token: 0x0400042C RID: 1068
	public const SimHashes CONSUME_ELEMENT = SimHashes.ChlorineGas;

	// Token: 0x0400042D RID: 1069
	public const SimHashes EMIT_ELEMENT = SimHashes.BleachStone;

	// Token: 0x0400042E RID: 1070
	private static float KG_ORE_EATEN_PER_CYCLE = 30f;

	// Token: 0x0400042F RID: 1071
	private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / PuftBleachstoneConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000430 RID: 1072
	private static float MIN_POOP_SIZE_IN_KG = 15f;

	// Token: 0x04000431 RID: 1073
	public static int EGG_SORT_ORDER = PuftConfig.EGG_SORT_ORDER + 3;
}
