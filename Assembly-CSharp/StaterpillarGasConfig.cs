using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000146 RID: 326
public class StaterpillarGasConfig : IEntityConfig
{
	// Token: 0x0600065E RID: 1630 RVA: 0x0002B3AC File Offset: 0x000295AC
	public static GameObject CreateStaterpillarGas(string id, string name, string desc, string anim_file, bool is_baby)
	{
		InhaleStates.Def inhaleDef = new InhaleStates.Def
		{
			behaviourTag = GameTags.Creatures.WantsToStore,
			inhaleAnimPre = "gas_consume_pre",
			inhaleAnimLoop = "gas_consume_loop",
			inhaleAnimPst = "gas_consume_pst",
			useStorage = true,
			alwaysPlayPstAnim = true,
			inhaleTime = StaterpillarGasConfig.INHALE_TIME,
			storageStatusItem = Db.Get().CreatureStatusItems.LookingForGas
		};
		GameObject gameObject = BaseStaterpillarConfig.BaseStaterpillar(id, name, desc, anim_file, "StaterpillarGasBaseTrait", is_baby, ObjectLayer.GasConduit, StaterpillarGasConnectorConfig.ID, GameTags.Unbreathable, "gas_", 263.15f, 313.15f, 173.15f, 373.15f, inhaleDef);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, TUNING.CREATURES.SPACE_REQUIREMENTS.TIER3);
		if (!is_baby)
		{
			GasAndLiquidConsumerMonitor.Def def = gameObject.AddOrGetDef<GasAndLiquidConsumerMonitor.Def>();
			def.behaviourTag = GameTags.Creatures.WantsToStore;
			def.consumableElementTag = GameTags.Unbreathable;
			def.transitionTag = new Tag[]
			{
				GameTags.Creature
			};
			def.minCooldown = StaterpillarGasConfig.COOLDOWN_MIN;
			def.maxCooldown = StaterpillarGasConfig.COOLDOWN_MAX;
			def.consumptionRate = StaterpillarGasConfig.CONSUMPTION_RATE;
		}
		Trait trait = Db.Get().CreateTrait("StaterpillarGasBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, StaterpillarTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> list = new List<Diet.Info>();
		list.AddRange(BaseStaterpillarConfig.RawMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarGasConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, null, 0f));
		list.AddRange(BaseStaterpillarConfig.RefinedMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarGasConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, null, 0f));
		gameObject = BaseStaterpillarConfig.SetupDiet(gameObject, list);
		Storage storage = gameObject.AddComponent<Storage>();
		storage.capacityKg = StaterpillarGasConfig.STORAGE_CAPACITY;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		return gameObject;
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x0002B603 File Offset: 0x00029803
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x0002B60C File Offset: 0x0002980C
	public virtual GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(StaterpillarGasConfig.CreateStaterpillarGas("StaterpillarGas", STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.DESC, "caterpillar_kanim", false), "StaterpillarGasEgg", STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.EGG_NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.DESC, "egg_caterpillar_kanim", StaterpillarTuning.EGG_MASS, "StaterpillarGasBaby", 60.000004f, 20f, StaterpillarTuning.EGG_CHANCES_GAS, this.GetDlcIds(), 1, true, false, true, 1f, false);
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x0002B689 File Offset: 0x00029889
	public void OnPrefabInit(GameObject prefab)
	{
		KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
		component.SetSymbolVisiblity("electric_bolt_c_bloom", false);
		component.SetSymbolVisiblity("gulp", false);
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x0002B6B2 File Offset: 0x000298B2
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400047D RID: 1149
	public const string ID = "StaterpillarGas";

	// Token: 0x0400047E RID: 1150
	public const string BASE_TRAIT_ID = "StaterpillarGasBaseTrait";

	// Token: 0x0400047F RID: 1151
	public const string EGG_ID = "StaterpillarGasEgg";

	// Token: 0x04000480 RID: 1152
	public const int EGG_SORT_ORDER = 1;

	// Token: 0x04000481 RID: 1153
	private static float KG_ORE_EATEN_PER_CYCLE = 30f;

	// Token: 0x04000482 RID: 1154
	private static float CALORIES_PER_KG_OF_ORE = StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / StaterpillarGasConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000483 RID: 1155
	private static float STORAGE_CAPACITY = 100f;

	// Token: 0x04000484 RID: 1156
	private static float COOLDOWN_MIN = 20f;

	// Token: 0x04000485 RID: 1157
	private static float COOLDOWN_MAX = 40f;

	// Token: 0x04000486 RID: 1158
	private static float CONSUMPTION_RATE = 0.5f;

	// Token: 0x04000487 RID: 1159
	private static float INHALE_TIME = 6f;
}
