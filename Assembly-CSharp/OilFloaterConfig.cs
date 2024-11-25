﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000126 RID: 294
public class OilFloaterConfig : IEntityConfig
{
	// Token: 0x060005A3 RID: 1443 RVA: 0x00028B08 File Offset: 0x00026D08
	public static GameObject CreateOilFloater(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BaseOilFloaterConfig.BaseOilFloater(id, name, desc, anim_file, "OilfloaterBaseTrait", 323.15f, 413.15f, 273.15f, 473.15f, is_baby, null);
		EntityTemplates.ExtendEntityToWildCreature(prefab, OilFloaterTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("OilfloaterBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, OilFloaterTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		GameObject gameObject = BaseOilFloaterConfig.SetupDiet(prefab, SimHashes.CarbonDioxide.CreateTag(), SimHashes.CrudeOil.CreateTag(), OilFloaterConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f, OilFloaterConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x060005A4 RID: 1444 RVA: 0x00028C58 File Offset: 0x00026E58
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x00028C60 File Offset: 0x00026E60
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterConfig.CreateOilFloater("Oilfloater", STRINGS.CREATURES.SPECIES.OILFLOATER.NAME, STRINGS.CREATURES.SPECIES.OILFLOATER.DESC, "oilfloater_kanim", false);
		string eggId = "OilfloaterEgg";
		string eggName = STRINGS.CREATURES.SPECIES.OILFLOATER.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.OILFLOATER.DESC;
		string egg_anim = "egg_oilfloater_kanim";
		float egg_MASS = OilFloaterTuning.EGG_MASS;
		string baby_id = "OilfloaterBaby";
		float fertility_cycles = 60.000004f;
		float incubation_cycles = 20f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_BASE = OilFloaterTuning.EGG_CHANCES_BASE;
		int egg_SORT_ORDER = OilFloaterConfig.EGG_SORT_ORDER;
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_BASE, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
		return gameObject;
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x00028CE5 File Offset: 0x00026EE5
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x00028CE7 File Offset: 0x00026EE7
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003ED RID: 1005
	public const string ID = "Oilfloater";

	// Token: 0x040003EE RID: 1006
	public const string BASE_TRAIT_ID = "OilfloaterBaseTrait";

	// Token: 0x040003EF RID: 1007
	public const string EGG_ID = "OilfloaterEgg";

	// Token: 0x040003F0 RID: 1008
	public const SimHashes CONSUME_ELEMENT = SimHashes.CarbonDioxide;

	// Token: 0x040003F1 RID: 1009
	public const SimHashes EMIT_ELEMENT = SimHashes.CrudeOil;

	// Token: 0x040003F2 RID: 1010
	private static float KG_ORE_EATEN_PER_CYCLE = 20f;

	// Token: 0x040003F3 RID: 1011
	private static float CALORIES_PER_KG_OF_ORE = OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / OilFloaterConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040003F4 RID: 1012
	private static float MIN_POOP_SIZE_IN_KG = 0.5f;

	// Token: 0x040003F5 RID: 1013
	public static int EGG_SORT_ORDER = 400;
}
