using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200012A RID: 298
public class OilFloaterHighTempConfig : IEntityConfig
{
	// Token: 0x060005BB RID: 1467 RVA: 0x00028FDC File Offset: 0x000271DC
	public static GameObject CreateOilFloater(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BaseOilFloaterConfig.BaseOilFloater(id, name, desc, anim_file, "OilfloaterHighTempBaseTrait", 373.15f, 473.15f, 323.15f, 573.15f, is_baby, "hot_");
		EntityTemplates.ExtendEntityToWildCreature(prefab, OilFloaterTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("OilfloaterHighTempBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, OilFloaterTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		return BaseOilFloaterConfig.SetupDiet(prefab, SimHashes.CarbonDioxide.CreateTag(), SimHashes.Petroleum.CreateTag(), OilFloaterHighTempConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f, OilFloaterHighTempConfig.MIN_POOP_SIZE_IN_KG);
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x00029125 File Offset: 0x00027325
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x0002912C File Offset: 0x0002732C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterHighTempConfig.CreateOilFloater("OilfloaterHighTemp", STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.NAME, STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.DESC, "oilfloater_kanim", false);
		string eggId = "OilfloaterHighTempEgg";
		string eggName = STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.DESC;
		string egg_anim = "egg_oilfloater_kanim";
		float egg_MASS = OilFloaterTuning.EGG_MASS;
		string baby_id = "OilfloaterHighTempBaby";
		float fertility_cycles = 60.000004f;
		float incubation_cycles = 20f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_HIGHTEMP = OilFloaterTuning.EGG_CHANCES_HIGHTEMP;
		int egg_SORT_ORDER = OilFloaterHighTempConfig.EGG_SORT_ORDER;
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_HIGHTEMP, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
		return gameObject;
	}

	// Token: 0x060005BE RID: 1470 RVA: 0x000291B1 File Offset: 0x000273B1
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x000291B3 File Offset: 0x000273B3
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040003FF RID: 1023
	public const string ID = "OilfloaterHighTemp";

	// Token: 0x04000400 RID: 1024
	public const string BASE_TRAIT_ID = "OilfloaterHighTempBaseTrait";

	// Token: 0x04000401 RID: 1025
	public const string EGG_ID = "OilfloaterHighTempEgg";

	// Token: 0x04000402 RID: 1026
	public const SimHashes CONSUME_ELEMENT = SimHashes.CarbonDioxide;

	// Token: 0x04000403 RID: 1027
	public const SimHashes EMIT_ELEMENT = SimHashes.Petroleum;

	// Token: 0x04000404 RID: 1028
	private static float KG_ORE_EATEN_PER_CYCLE = 20f;

	// Token: 0x04000405 RID: 1029
	private static float CALORIES_PER_KG_OF_ORE = OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / OilFloaterHighTempConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000406 RID: 1030
	private static float MIN_POOP_SIZE_IN_KG = 0.5f;

	// Token: 0x04000407 RID: 1031
	public static int EGG_SORT_ORDER = OilFloaterConfig.EGG_SORT_ORDER + 1;
}
