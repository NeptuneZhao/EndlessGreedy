using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002AD RID: 685
public class BionicUpgradeComponentConfig : IMultiEntityConfig
{
	// Token: 0x06000E45 RID: 3653 RVA: 0x00054122 File Offset: 0x00052322
	public static string CraftBionicPrefabID(string sufix)
	{
		return "bionic_upgrade_" + sufix.ToLower();
	}

	// Token: 0x06000E46 RID: 3654 RVA: 0x00054134 File Offset: 0x00052334
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_CONSTRUCTION, STRINGS.ITEMS.BIONIC_BOOSTERS.CONSTRUCTION_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.CONSTRUCTION_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_2, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), new BionicUpgrade_SkilledWorker.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_CONSTRUCTION), Db.Get().Attributes.Construction.Id, "BionicConstructionBoost", new string[]
		{
			"Building1",
			"Building2"
		})), DlcManager.DLC3, "upgrade_disc_kanim", "construction", SimHashes.Creature, "PrettyGoodConductors", "DefaultBionicBoostBuilding", BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_EXCAVATION, STRINGS.ITEMS.BIONIC_BOOSTERS.EXCAVATION_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.EXCAVATION_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_2, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), new BionicUpgrade_SkilledWorker.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_EXCAVATION), Db.Get().Attributes.Digging.Id, "BionicExcavationBoost", new string[]
		{
			"Mining1",
			"Mining2"
		})), DlcManager.DLC3, "upgrade_disc_kanim", "excavation", SimHashes.Creature, "RoboticTools", "DefaultBionicBoostDigging", BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_MACHINERY, STRINGS.ITEMS.BIONIC_BOOSTERS.MACHINERY_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.MACHINERY_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_2, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), new BionicUpgrade_SkilledWorker.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_MACHINERY), Db.Get().Attributes.Machinery.Id, "BionicMachineryBoost", new string[]
		{
			"Technicals1",
			"Technicals2"
		})), DlcManager.DLC3, "upgrade_disc_kanim", "machinery", SimHashes.Creature, "Smelting", null, BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_COOKING, STRINGS.ITEMS.BIONIC_BOOSTERS.COOKING_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.COOKING_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_2, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), new BionicUpgrade_SkilledWorker.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_COOKING), Db.Get().Attributes.Cooking.Id, "BionicCookingBoost", new string[]
		{
			"Cooking1",
			"Cooking2"
		})), DlcManager.DLC3, "upgrade_disc_kanim", "cooking", SimHashes.Creature, "FoodRepurposing", "DefaultBionicBoostCooking", BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_MEDICINE, STRINGS.ITEMS.BIONIC_BOOSTERS.MEDICINE_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.MEDICINE_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_2, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), new BionicUpgrade_SkilledWorker.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_MEDICINE), Db.Get().Attributes.Caring.Id, "BionicMedicineBoost", new string[]
		{
			"Medicine1",
			"Medicine2"
		})), DlcManager.DLC3, "upgrade_disc_kanim", "medicine", SimHashes.Creature, "MedicineIV", "DefaultBionicBoostMedicine", BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_CREATIVITY, STRINGS.ITEMS.BIONIC_BOOSTERS.CREATIVITY_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.CREATIVITY_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_2, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), new BionicUpgrade_SkilledWorker.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_CREATIVITY), Db.Get().Attributes.Art.Id, "BionicCreativityBoost", new string[]
		{
			"Arting1",
			"Arting2"
		})), DlcManager.DLC3, "upgrade_disc_kanim", "creativity", SimHashes.Creature, "RefractiveDecor", "DefaultBionicBoostArt", BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_AGRICULTURE, STRINGS.ITEMS.BIONIC_BOOSTERS.AGRICULTURE_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.AGRICULTURE_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_2, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), new BionicUpgrade_SkilledWorker.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_AGRICULTURE), Db.Get().Attributes.Botanist.Id, "BionicAgricultureBoost", new string[]
		{
			"Farming1",
			"Farming2"
		})), DlcManager.DLC3, "upgrade_disc_kanim", "agriculture", SimHashes.Creature, "AnimalControl", "DefaultBionicBoostFarming", BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_HUSBANDRY, STRINGS.ITEMS.BIONIC_BOOSTERS.HUSBANDRY_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.HUSBANDRY_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_1, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), new BionicUpgrade_SkilledWorker.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_HUSBANDRY), Db.Get().Attributes.Ranching.Id, "BionicHusbandryBoost", new string[]
		{
			"Ranching1"
		})), DlcManager.DLC3, "upgrade_disc_kanim", "ranching", SimHashes.Creature, "AnimalControl", "DefaultBionicBoostRanching", BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_SCIENCE, STRINGS.ITEMS.BIONIC_BOOSTERS.SCIENCE_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.SCIENCE_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_1, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), new BionicUpgrade_SkilledWorker.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_SCIENCE), Db.Get().Attributes.Learning.Id, "BionicScienceBoost", new string[]
		{
			"Researching1"
		})), DlcManager.DLC3, "upgrade_disc_kanim", "science", SimHashes.Creature, "ParallelAutomation", null, BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_PILOTING, STRINGS.ITEMS.BIONIC_BOOSTERS.PILOTING_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.PILOTING_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_1, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), new BionicUpgrade_SkilledWorker.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_PILOTING), Db.Get().Attributes.SpaceNavigation.Id, "BionicPilotingBoost", new string[]
		{
			"RocketPiloting1"
		})), new string[]
		{
			"EXPANSION1_ID",
			"DLC3_ID"
		}, "upgrade_disc_kanim", "piloting", SimHashes.Creature, "CrashPlan", null, BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_STRENGTH, STRINGS.ITEMS.BIONIC_BOOSTERS.STRENGTH_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.STRENGTH_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_2, (StateMachine.Instance smi) => new BionicUpgrade_OnGoingEffect.Instance(smi.GetMaster(), new BionicUpgrade_OnGoingEffect.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_STRENGTH), "BionicStrengthBoost", new string[]
		{
			"Hauling1",
			"Hauling2"
		})), DlcManager.DLC3, "upgrade_disc_kanim", "strength", SimHashes.Creature, "Suits", null, BionicUpgradeComponentConfig.RarityType.Basic));
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_ATHLETICS, STRINGS.ITEMS.BIONIC_BOOSTERS.ATHLETICS_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.ATHLETICS_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_2, (StateMachine.Instance smi) => new BionicUpgrade_OnGoingEffect.Instance(smi.GetMaster(), new BionicUpgrade_OnGoingEffect.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_ATHLETICS), "BionicAthleticsBoost", null)), DlcManager.DLC3, "upgrade_disc_kanim", "athletics", SimHashes.Creature, "SolidTransport", null, BionicUpgradeComponentConfig.RarityType.Basic));
		List<GameObject> list2 = list;
		GameObject gameObject = BionicUpgradeComponentConfig.CreateNewUpgradeComponent(BionicUpgradeComponentConfig.SUFFIX_EXPLORER, STRINGS.ITEMS.BIONIC_BOOSTERS.EXPLORER_BOOSTER.NAME, STRINGS.ITEMS.BIONIC_BOOSTERS.EXPLORER_BOOSTER.DESC, TUNING.ITEMS.BIONIC_UPGRADES.POWER_COST.TIER_2, (StateMachine.Instance smi) => new BionicUpgrade_ExplorerBoosterMonitor.Instance(smi.GetMaster(), new BionicUpgrade_ExplorerBoosterMonitor.Def(BionicUpgradeComponentConfig.CraftBionicPrefabID(BionicUpgradeComponentConfig.SUFFIX_EXPLORER))), DlcManager.DLC3, "upgrade_disc_kanim", "explore", SimHashes.Creature, "HighTempForging", "DefaultBionicBoostExplorer", BionicUpgradeComponentConfig.RarityType.Special);
		if (gameObject != null)
		{
			gameObject.AddOrGetDef<BionicUpgrade_ExplorerBooster.Def>();
			list2.Add(gameObject);
		}
		list2.RemoveAll((GameObject t) => t == null);
		return list2;
	}

	// Token: 0x06000E47 RID: 3655 RVA: 0x000546BA File Offset: 0x000528BA
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000E48 RID: 3656 RVA: 0x000546BC File Offset: 0x000528BC
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000E49 RID: 3657 RVA: 0x000546C0 File Offset: 0x000528C0
	public static Tag GetBionicUpgradePrefabIDWithTraitID(string traitID)
	{
		foreach (Tag tag in BionicUpgradeComponentConfig.UpgradesData.Keys)
		{
			BionicUpgradeComponentConfig.BionicUpgradeData bionicUpgradeData = BionicUpgradeComponentConfig.UpgradesData[tag];
			if (bionicUpgradeData.relatedTrait != null && bionicUpgradeData.relatedTrait == traitID)
			{
				return tag;
			}
		}
		return Tag.Invalid;
	}

	// Token: 0x06000E4A RID: 3658 RVA: 0x00054740 File Offset: 0x00052940
	public static GameObject CreateNewUpgradeComponent(string id, string name, string desc, float wattageCost, Func<StateMachine.Instance, StateMachine.Instance> stateMachine = null, string[] dlcIDs = null, string animFile = "upgrade_disc_kanim", string animStateName = "object", SimHashes element = SimHashes.Creature, string craftTechUnlockID = null, string relatedTrait = null, BionicUpgradeComponentConfig.RarityType rarity = BionicUpgradeComponentConfig.RarityType.Basic)
	{
		if (!DlcManager.IsAllContentSubscribed(dlcIDs))
		{
			return null;
		}
		string ID = BionicUpgradeComponentConfig.CraftBionicPrefabID(id);
		TechItem techItem = new TechItem(ID, Db.Get().TechItems, Strings.Get("STRINGS.RESEARCH.OTHER_TECH_ITEMS." + id.ToUpper() + ".NAME"), Strings.Get("STRINGS.RESEARCH.OTHER_TECH_ITEMS." + id.ToUpper() + ".DESC"), (string a, bool b) => Def.GetUISprite(Assets.GetPrefab(ID), "ui", false).first, craftTechUnlockID, DlcManager.DLC3, null, false);
		Db.Get().Techs.Get(craftTechUnlockID).AddUnlockedItemIDs(new string[]
		{
			techItem.Id
		});
		GameObject gameObject = EntityTemplates.CreateLooseEntity(ID, name, desc, 25f, true, Assets.GetAnim(animFile), animStateName, Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.45f, true, SORTORDER.ARTIFACTS, element, new List<Tag>
		{
			GameTags.MiscPickupable,
			GameTags.BionicUpgrade,
			GameTags.NotRoomAssignable
		});
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		DecorProvider decorProvider = gameObject.AddOrGet<DecorProvider>();
		decorProvider.SetValues(DECOR.NONE);
		decorProvider.overrideName = gameObject.GetProperName();
		gameObject.AddOrGet<BionicUpgradeComponent>().slotID = Db.Get().AssignableSlots.BionicUpgrade.Id;
		gameObject.AddOrGet<KSelectable>();
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.AddTag(GameTags.PedestalDisplayable, false);
		component.requiredDlcIds = dlcIDs;
		BionicUpgradeComponentConfig.UpgradesData.Add(component.PrefabTag, new BionicUpgradeComponentConfig.BionicUpgradeData(wattageCost, animStateName, relatedTrait, rarity, stateMachine));
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.Polypropylene.CreateTag(), 100f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(ID.ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		ElectrobankConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, array, array2), array, array2)
		{
			time = INDUSTRIAL.RECIPES.STANDARD_FABRICATION_TIME,
			description = string.Format(STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.BIONIC_COMPONENT_RECIPE_DESC, DlcManager.IsExpansion1Active() ? STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.NAME : STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.RESEARCH_DATABANK.NAME, name),
			nameDisplay = ComplexRecipe.RecipeNameDisplay.ResultWithIngredient,
			fabricators = new List<Tag>
			{
				"AdvancedCraftingTable"
			},
			requiredTech = craftTechUnlockID
		};
		return gameObject;
	}

	// Token: 0x040008ED RID: 2285
	public const string DEFAULT_ANIM_FILE_NAME = "upgrade_disc_kanim";

	// Token: 0x040008EE RID: 2286
	public const string ID_PREFIX = "bionic_upgrade_";

	// Token: 0x040008EF RID: 2287
	public static string SUFFIX_CONSTRUCTION = "ConstructionBooster".ToLower();

	// Token: 0x040008F0 RID: 2288
	public static string SUFFIX_EXCAVATION = "ExcavationBooster".ToLower();

	// Token: 0x040008F1 RID: 2289
	public static string SUFFIX_MACHINERY = "MachineryBooster".ToLower();

	// Token: 0x040008F2 RID: 2290
	public static string SUFFIX_ATHLETICS = "AthleticsBooster".ToLower();

	// Token: 0x040008F3 RID: 2291
	public static string SUFFIX_COOKING = "CookingBooster".ToLower();

	// Token: 0x040008F4 RID: 2292
	public static string SUFFIX_MEDICINE = "MedicineBooster".ToLower();

	// Token: 0x040008F5 RID: 2293
	public static string SUFFIX_STRENGTH = "StrengthBooster".ToLower();

	// Token: 0x040008F6 RID: 2294
	public static string SUFFIX_CREATIVITY = "CreativityBooster".ToLower();

	// Token: 0x040008F7 RID: 2295
	public static string SUFFIX_AGRICULTURE = "AgricultureBooster".ToLower();

	// Token: 0x040008F8 RID: 2296
	public static string SUFFIX_HUSBANDRY = "HusbandryBooster".ToLower();

	// Token: 0x040008F9 RID: 2297
	public static string SUFFIX_SCIENCE = "ScienceBooster".ToLower();

	// Token: 0x040008FA RID: 2298
	public static string SUFFIX_PILOTING = "PilotingBooster".ToLower();

	// Token: 0x040008FB RID: 2299
	public static string SUFFIX_EXPLORER = "ExplorerBooster".ToLower();

	// Token: 0x040008FC RID: 2300
	public static Dictionary<Tag, BionicUpgradeComponentConfig.BionicUpgradeData> UpgradesData = new Dictionary<Tag, BionicUpgradeComponentConfig.BionicUpgradeData>();

	// Token: 0x0200110D RID: 4365
	public enum RarityType
	{
		// Token: 0x04005EEC RID: 24300
		Basic,
		// Token: 0x04005EED RID: 24301
		Special
	}

	// Token: 0x0200110E RID: 4366
	public class BionicUpgradeData
	{
		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06007E3D RID: 32317 RVA: 0x00309E49 File Offset: 0x00308049
		// (set) Token: 0x06007E3C RID: 32316 RVA: 0x00309E40 File Offset: 0x00308040
		public float WattageCost { get; private set; }

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06007E3F RID: 32319 RVA: 0x00309E5A File Offset: 0x0030805A
		// (set) Token: 0x06007E3E RID: 32318 RVA: 0x00309E51 File Offset: 0x00308051
		public Func<StateMachine.Instance, StateMachine.Instance> stateMachine { get; private set; }

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06007E40 RID: 32320 RVA: 0x00309E62 File Offset: 0x00308062
		public string uiAnimName
		{
			get
			{
				if (!(this.animStateName == "object"))
				{
					return "ui_" + this.animStateName;
				}
				return "ui";
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06007E42 RID: 32322 RVA: 0x00309E95 File Offset: 0x00308095
		// (set) Token: 0x06007E41 RID: 32321 RVA: 0x00309E8C File Offset: 0x0030808C
		public string relatedTrait { get; private set; }

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06007E44 RID: 32324 RVA: 0x00309EA6 File Offset: 0x003080A6
		// (set) Token: 0x06007E43 RID: 32323 RVA: 0x00309E9D File Offset: 0x0030809D
		public BionicUpgradeComponentConfig.RarityType rarity { get; private set; }

		// Token: 0x06007E45 RID: 32325 RVA: 0x00309EAE File Offset: 0x003080AE
		public BionicUpgradeData(float cost, string animStateName, string relatedTrait, BionicUpgradeComponentConfig.RarityType rarity, Func<StateMachine.Instance, StateMachine.Instance> smi)
		{
			this.WattageCost = cost;
			this.stateMachine = smi;
			this.animStateName = animStateName;
			this.relatedTrait = relatedTrait;
			this.rarity = rarity;
		}

		// Token: 0x04005EEE RID: 24302
		private const string DEFAULT_ANIM_STATE_NAME = "object";

		// Token: 0x04005EF1 RID: 24305
		public string animStateName = "object";
	}
}
