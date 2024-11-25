using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000128 RID: 296
public class OilFloaterDecorConfig : IEntityConfig
{
	// Token: 0x060005AF RID: 1455 RVA: 0x00028D74 File Offset: 0x00026F74
	public static GameObject CreateOilFloater(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseOilFloaterConfig.BaseOilFloater(id, name, desc, anim_file, "OilfloaterDecorBaseTrait", 273.15f, 323.15f, 223.15f, 373.15f, is_baby, "oxy_");
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.BONUS.TIER6);
		EntityTemplates.ExtendEntityToWildCreature(gameObject, OilFloaterTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("OilfloaterDecorBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, OilFloaterTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 150f, name, false, false, true));
		return BaseOilFloaterConfig.SetupDiet(gameObject, SimHashes.Oxygen.CreateTag(), Tag.Invalid, OilFloaterDecorConfig.CALORIES_PER_KG_OF_ORE, 0f, null, 0f, 0f);
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x00028EC8 File Offset: 0x000270C8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x00028ED0 File Offset: 0x000270D0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterDecorConfig.CreateOilFloater("OilfloaterDecor", STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.NAME, STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.DESC, "oilfloater_kanim", false);
		string eggId = "OilfloaterDecorEgg";
		string eggName = STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.DESC;
		string egg_anim = "egg_oilfloater_kanim";
		float egg_MASS = OilFloaterTuning.EGG_MASS;
		string baby_id = "OilfloaterDecorBaby";
		float fertility_cycles = 90f;
		float incubation_cycles = 30f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_DECOR = OilFloaterTuning.EGG_CHANCES_DECOR;
		int egg_SORT_ORDER = OilFloaterDecorConfig.EGG_SORT_ORDER;
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_DECOR, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
		return gameObject;
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x00028F55 File Offset: 0x00027155
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x00028F57 File Offset: 0x00027157
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003F7 RID: 1015
	public const string ID = "OilfloaterDecor";

	// Token: 0x040003F8 RID: 1016
	public const string BASE_TRAIT_ID = "OilfloaterDecorBaseTrait";

	// Token: 0x040003F9 RID: 1017
	public const string EGG_ID = "OilfloaterDecorEgg";

	// Token: 0x040003FA RID: 1018
	public const SimHashes CONSUME_ELEMENT = SimHashes.Oxygen;

	// Token: 0x040003FB RID: 1019
	private static float KG_ORE_EATEN_PER_CYCLE = 30f;

	// Token: 0x040003FC RID: 1020
	private static float CALORIES_PER_KG_OF_ORE = OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / OilFloaterDecorConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040003FD RID: 1021
	public static int EGG_SORT_ORDER = OilFloaterConfig.EGG_SORT_ORDER + 2;
}
