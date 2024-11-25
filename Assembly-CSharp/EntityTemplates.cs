using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200007B RID: 123
public class EntityTemplates
{
	// Token: 0x06000242 RID: 578 RVA: 0x0000F904 File Offset: 0x0000DB04
	public static void CreateTemplates()
	{
		EntityTemplates.unselectableEntityTemplate = new GameObject("unselectableEntityTemplate");
		EntityTemplates.unselectableEntityTemplate.SetActive(false);
		EntityTemplates.unselectableEntityTemplate.AddComponent<KPrefabID>();
		UnityEngine.Object.DontDestroyOnLoad(EntityTemplates.unselectableEntityTemplate);
		EntityTemplates.selectableEntityTemplate = UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.unselectableEntityTemplate);
		EntityTemplates.selectableEntityTemplate.name = "selectableEntityTemplate";
		EntityTemplates.selectableEntityTemplate.AddComponent<KSelectable>();
		UnityEngine.Object.DontDestroyOnLoad(EntityTemplates.selectableEntityTemplate);
		EntityTemplates.baseEntityTemplate = UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.selectableEntityTemplate);
		EntityTemplates.baseEntityTemplate.name = "baseEntityTemplate";
		EntityTemplates.baseEntityTemplate.AddComponent<KBatchedAnimController>();
		EntityTemplates.baseEntityTemplate.AddComponent<SaveLoadRoot>();
		EntityTemplates.baseEntityTemplate.AddComponent<StateMachineController>();
		EntityTemplates.baseEntityTemplate.AddComponent<PrimaryElement>();
		EntityTemplates.baseEntityTemplate.AddComponent<SimTemperatureTransfer>();
		EntityTemplates.baseEntityTemplate.AddComponent<InfoDescription>();
		EntityTemplates.baseEntityTemplate.AddComponent<Notifier>();
		UnityEngine.Object.DontDestroyOnLoad(EntityTemplates.baseEntityTemplate);
		EntityTemplates.placedEntityTemplate = UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.baseEntityTemplate);
		EntityTemplates.placedEntityTemplate.name = "placedEntityTemplate";
		EntityTemplates.placedEntityTemplate.AddComponent<KBoxCollider2D>();
		EntityTemplates.placedEntityTemplate.AddComponent<OccupyArea>();
		EntityTemplates.placedEntityTemplate.AddComponent<Modifiers>();
		EntityTemplates.placedEntityTemplate.AddComponent<DecorProvider>();
		UnityEngine.Object.DontDestroyOnLoad(EntityTemplates.placedEntityTemplate);
	}

	// Token: 0x06000243 RID: 579 RVA: 0x0000FA3C File Offset: 0x0000DC3C
	private static void ConfigEntity(GameObject template, string id, string name, bool is_selectable = true)
	{
		template.name = id;
		template.AddOrGet<KPrefabID>().PrefabTag = TagManager.Create(id, name);
		if (is_selectable)
		{
			template.AddOrGet<KSelectable>().SetName(name);
		}
	}

	// Token: 0x06000244 RID: 580 RVA: 0x0000FA68 File Offset: 0x0000DC68
	public static GameObject CreateEntity(string id, string name, bool is_selectable = true)
	{
		GameObject gameObject;
		if (is_selectable)
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.selectableEntityTemplate);
		}
		else
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.unselectableEntityTemplate);
		}
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		EntityTemplates.ConfigEntity(gameObject, id, name, is_selectable);
		return gameObject;
	}

	// Token: 0x06000245 RID: 581 RVA: 0x0000FAA4 File Offset: 0x0000DCA4
	public static GameObject ConfigBasicEntity(GameObject template, string id, string name, string desc, float mass, bool unitMass, KAnimFile anim, string initialAnim, Grid.SceneLayer sceneLayer, SimHashes element = SimHashes.Creature, List<Tag> additionalTags = null, float defaultTemperature = 293f)
	{
		EntityTemplates.ConfigEntity(template, id, name, true);
		KPrefabID kprefabID = template.AddOrGet<KPrefabID>();
		if (additionalTags != null)
		{
			foreach (Tag tag in additionalTags)
			{
				kprefabID.AddTag(tag, false);
			}
		}
		KBatchedAnimController kbatchedAnimController = template.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			anim
		};
		kbatchedAnimController.sceneLayer = sceneLayer;
		kbatchedAnimController.initialAnim = initialAnim;
		template.AddOrGet<StateMachineController>();
		PrimaryElement primaryElement = template.AddOrGet<PrimaryElement>();
		primaryElement.ElementID = element;
		primaryElement.Temperature = defaultTemperature;
		if (unitMass)
		{
			primaryElement.MassPerUnit = mass;
			primaryElement.Units = 1f;
			GameTags.DisplayAsUnits.Add(kprefabID.PrefabTag);
		}
		else
		{
			primaryElement.Mass = mass;
		}
		template.AddOrGet<InfoDescription>().description = desc;
		template.AddOrGet<Notifier>();
		return template;
	}

	// Token: 0x06000246 RID: 582 RVA: 0x0000FB98 File Offset: 0x0000DD98
	public static GameObject CreateBasicEntity(string id, string name, string desc, float mass, bool unitMass, KAnimFile anim, string initialAnim, Grid.SceneLayer sceneLayer, SimHashes element = SimHashes.Creature, List<Tag> additionalTags = null, float defaultTemperature = 293f)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.baseEntityTemplate);
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		EntityTemplates.ConfigBasicEntity(gameObject, id, name, desc, mass, unitMass, anim, initialAnim, sceneLayer, element, additionalTags, defaultTemperature);
		return gameObject;
	}

	// Token: 0x06000247 RID: 583 RVA: 0x0000FBD0 File Offset: 0x0000DDD0
	private static GameObject ConfigPlacedEntity(GameObject template, string id, string name, string desc, float mass, KAnimFile anim, string initialAnim, Grid.SceneLayer sceneLayer, int width, int height, EffectorValues decor, EffectorValues noise = default(EffectorValues), SimHashes element = SimHashes.Creature, List<Tag> additionalTags = null, float defaultTemperature = 293f)
	{
		if (anim == null)
		{
			global::Debug.LogErrorFormat("Cant create [{0}] entity without an anim", new object[]
			{
				name
			});
		}
		EntityTemplates.ConfigBasicEntity(template, id, name, desc, mass, true, anim, initialAnim, sceneLayer, element, additionalTags, defaultTemperature);
		KBoxCollider2D kboxCollider2D = template.AddOrGet<KBoxCollider2D>();
		kboxCollider2D.size = new Vector2f(width, height);
		float num = 0.5f * (float)((width + 1) % 2);
		kboxCollider2D.offset = new Vector2f(num, (float)height / 2f);
		template.GetComponent<KBatchedAnimController>().Offset = new Vector3(num, 0f, 0f);
		template.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(width, height));
		DecorProvider decorProvider = template.AddOrGet<DecorProvider>();
		decorProvider.SetValues(decor);
		decorProvider.overrideName = name;
		return template;
	}

	// Token: 0x06000248 RID: 584 RVA: 0x0000FC9C File Offset: 0x0000DE9C
	public static GameObject CreatePlacedEntity(string id, string name, string desc, float mass, KAnimFile anim, string initialAnim, Grid.SceneLayer sceneLayer, int width, int height, EffectorValues decor, EffectorValues noise = default(EffectorValues), SimHashes element = SimHashes.Creature, List<Tag> additionalTags = null, float defaultTemperature = 293f)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.placedEntityTemplate);
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		EntityTemplates.ConfigPlacedEntity(gameObject, id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, noise, element, additionalTags, defaultTemperature);
		return gameObject;
	}

	// Token: 0x06000249 RID: 585 RVA: 0x0000FCD8 File Offset: 0x0000DED8
	public static GameObject CreatePlacedEntity(string id, string name, string desc, float mass, KAnimFile anim, string initialAnim, Grid.SceneLayer sceneLayer, int width, int height, EffectorValues decor, PermittedRotations permittedRotation, Orientation orientation = Orientation.Neutral, EffectorValues noise = default(EffectorValues), SimHashes element = SimHashes.Creature, List<Tag> additionalTags = null, float defaultTemperature = 293f)
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, noise, element, additionalTags, defaultTemperature);
		if (permittedRotation != PermittedRotations.Unrotatable)
		{
			Rotatable rotatable = gameObject.AddOrGet<Rotatable>();
			rotatable.SetSize(width, height);
			rotatable.permittedRotations = permittedRotation;
			rotatable.SetOrientation(orientation);
		}
		return gameObject;
	}

	// Token: 0x0600024A RID: 586 RVA: 0x0000FD28 File Offset: 0x0000DF28
	public static GameObject MakeHangingOffsets(GameObject template, int width, int height)
	{
		KBoxCollider2D component = template.GetComponent<KBoxCollider2D>();
		if (component)
		{
			component.size = new Vector2f(width, height);
			float a = 0.5f * (float)((width + 1) % 2);
			component.offset = new Vector2f(a, (float)(-(float)height) / 2f + 1f);
		}
		OccupyArea component2 = template.GetComponent<OccupyArea>();
		if (component2)
		{
			component2.SetCellOffsets(EntityTemplates.GenerateHangingOffsets(width, height));
		}
		return template;
	}

	// Token: 0x0600024B RID: 587 RVA: 0x0000FDA0 File Offset: 0x0000DFA0
	public static GameObject ExtendEntityToBasicPlant(GameObject template, float temperature_lethal_low = 218.15f, float temperature_warning_low = 283.15f, float temperature_warning_high = 303.15f, float temperature_lethal_high = 398.15f, SimHashes[] safe_elements = null, bool pressure_sensitive = true, float pressure_lethal_low = 0f, float pressure_warning_low = 0.15f, string crop_id = null, bool can_drown = true, bool can_tinker = true, bool require_solid_tile = true, bool should_grow_old = true, float max_age = 2400f, float min_radiation = 0f, float max_radiation = 2200f, string baseTraitId = null, string baseTraitName = null)
	{
		Modifiers component = template.GetComponent<Modifiers>();
		Trait trait = Db.Get().CreateTrait(baseTraitId, baseTraitName, baseTraitName, null, false, null, true, true);
		template.AddTag(GameTags.Plant);
		template.AddOrGet<EntombVulnerable>();
		PressureVulnerable pressureVulnerable = template.AddOrGet<PressureVulnerable>();
		if (pressure_sensitive)
		{
			pressureVulnerable.Configure(pressure_warning_low, pressure_lethal_low, 10f, 30f, safe_elements);
		}
		else
		{
			pressureVulnerable.Configure(safe_elements);
		}
		template.AddOrGet<WiltCondition>();
		template.AddOrGet<Prioritizable>();
		template.AddOrGet<Uprootable>();
		template.AddOrGet<Effects>();
		if (require_solid_tile)
		{
			template.AddOrGet<UprootedMonitor>();
		}
		template.AddOrGet<ReceptacleMonitor>();
		template.AddOrGet<Notifier>();
		if (can_drown)
		{
			template.AddOrGet<DrowningMonitor>();
		}
		template.AddOrGet<KAnimControllerBase>().randomiseLoopedOffset = true;
		component.initialAttributes.Add(Db.Get().PlantAttributes.WiltTempRangeMod.Id);
		template.AddOrGet<TemperatureVulnerable>().Configure(temperature_warning_low, temperature_lethal_low, temperature_warning_high, temperature_lethal_high);
		if (DlcManager.FeaturePlantMutationsEnabled())
		{
			component.initialAttributes.Add(Db.Get().PlantAttributes.MinRadiationThreshold.Id);
			if (min_radiation != 0f)
			{
				trait.Add(new AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold.Id, min_radiation, baseTraitName, false, false, true));
			}
			component.initialAttributes.Add(Db.Get().PlantAttributes.MaxRadiationThreshold.Id);
			trait.Add(new AttributeModifier(Db.Get().PlantAttributes.MaxRadiationThreshold.Id, max_radiation, baseTraitName, false, false, true));
			template.AddOrGetDef<RadiationVulnerable.Def>();
		}
		template.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		KPrefabID component2 = template.GetComponent<KPrefabID>();
		if (crop_id != null)
		{
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, component2.PrefabID().ToString());
			Crop.CropVal cropVal = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == crop_id);
			global::Debug.Assert(baseTraitId != null && baseTraitName != null, "Extending " + template.name + " to a crop plant failed because the base trait wasn't specified.");
			component.initialAttributes.Add(Db.Get().PlantAttributes.YieldAmount.Id);
			component.initialAmounts.Add(Db.Get().Amounts.Maturity.Id);
			trait.Add(new AttributeModifier(Db.Get().PlantAttributes.YieldAmount.Id, (float)cropVal.numProduced, baseTraitName, false, false, true));
			trait.Add(new AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute.Id, cropVal.cropDuration / 600f, baseTraitName, false, false, true));
			if (DlcManager.FeaturePlantMutationsEnabled())
			{
				template.AddOrGet<MutantPlant>().SpeciesID = component2.PrefabTag;
				SymbolOverrideControllerUtil.AddToPrefab(template);
			}
			template.AddOrGet<Crop>().Configure(cropVal);
			Growing growing = template.AddOrGet<Growing>();
			growing.shouldGrowOld = should_grow_old;
			growing.maxAge = max_age;
			template.AddOrGet<Harvestable>();
			template.AddOrGet<HarvestDesignatable>();
		}
		if (trait.SelfModifiers != null && trait.SelfModifiers.Count > 0)
		{
			template.AddOrGet<Traits>();
			component.initialTraits.Add(baseTraitId);
		}
		component2.prefabInitFn += delegate(GameObject inst)
		{
			PressureVulnerable component3 = inst.GetComponent<PressureVulnerable>();
			if (component3 != null && safe_elements != null)
			{
				foreach (SimHashes hash in safe_elements)
				{
					component3.safe_atmospheres.Add(ElementLoader.FindElementByHash(hash));
				}
			}
		};
		if (can_tinker)
		{
			Tinkerable.MakeFarmTinkerable(template);
		}
		return template;
	}

	// Token: 0x0600024C RID: 588 RVA: 0x00010103 File Offset: 0x0000E303
	public static GameObject ExtendEntityToWildCreature(GameObject prefab, int space_required_per_creature)
	{
		return EntityTemplates.ExtendEntityToWildCreature(prefab, space_required_per_creature, true);
	}

	// Token: 0x0600024D RID: 589 RVA: 0x00010110 File Offset: 0x0000E310
	public static GameObject ExtendEntityToWildCreature(GameObject prefab, int space_required_per_creature, bool add_fixed_capturable_monitor)
	{
		prefab.AddOrGetDef<AgeMonitor.Def>();
		prefab.AddOrGetDef<HappinessMonitor.Def>();
		Tag prefabTag = prefab.GetComponent<KPrefabID>().PrefabTag;
		WildnessMonitor.Def def = prefab.AddOrGetDef<WildnessMonitor.Def>();
		def.wildEffect = new Effect("Wild" + prefabTag.Name, STRINGS.CREATURES.MODIFIERS.WILD.NAME, STRINGS.CREATURES.MODIFIERS.WILD.TOOLTIP, 0f, true, true, false, null, -1f, 0f, null, "");
		def.wildEffect.Add(new AttributeModifier(Db.Get().Amounts.Wildness.deltaAttribute.Id, 0.008333334f, STRINGS.CREATURES.MODIFIERS.WILD.NAME, false, false, true));
		def.wildEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -75f, STRINGS.CREATURES.MODIFIERS.WILD.NAME, false, false, true));
		def.wildEffect.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, -0.75f, STRINGS.CREATURES.MODIFIERS.WILD.NAME, true, false, true));
		def.tameEffect = new Effect("Tame" + prefabTag.Name, STRINGS.CREATURES.MODIFIERS.TAME.NAME, STRINGS.CREATURES.MODIFIERS.TAME.TOOLTIP, 0f, true, true, false, null, -1f, 0f, null, "");
		def.tameEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -1f, STRINGS.CREATURES.MODIFIERS.TAME.NAME, false, false, true));
		prefab.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = space_required_per_creature;
		if (add_fixed_capturable_monitor)
		{
			prefab.AddOrGetDef<FixedCapturableMonitor.Def>();
		}
		return prefab;
	}

	// Token: 0x0600024E RID: 590 RVA: 0x000102C8 File Offset: 0x0000E4C8
	public static GameObject ExtendEntityToFertileCreature(GameObject prefab, string eggId, string eggName, string eggDesc, string egg_anim, float egg_mass, string baby_id, float fertility_cycles, float incubation_cycles, List<FertilityMonitor.BreedingChance> egg_chances, string[] dlcIds, int eggSortOrder = -1, bool is_ranchable = true, bool add_fish_overcrowding_monitor = false, bool add_fixed_capturable_monitor = true, float egg_anim_scale = 1f, bool deprecated = false)
	{
		FertilityMonitor.Def def = prefab.AddOrGetDef<FertilityMonitor.Def>();
		def.baseFertileCycles = fertility_cycles;
		DebugUtil.DevAssert(eggSortOrder > -1, "Added a fertile creature without an egg sort order!", null);
		float base_incubation_rate = 100f / (600f * incubation_cycles);
		GameObject gameObject = EggConfig.CreateEgg(eggId, eggName, eggDesc, baby_id, egg_anim, egg_mass, eggSortOrder, base_incubation_rate, dlcIds);
		def.eggPrefab = new Tag(eggId);
		def.initialBreedingWeights = egg_chances;
		if (egg_anim_scale != 1f)
		{
			KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
			component.animWidth = egg_anim_scale;
			component.animHeight = egg_anim_scale;
		}
		KPrefabID egg_prefab_id = gameObject.GetComponent<KPrefabID>();
		SymbolOverrideController symbol_override_controller = SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		string symbolPrefix = prefab.GetComponent<CreatureBrain>().symbolPrefix;
		if (!string.IsNullOrEmpty(symbolPrefix))
		{
			symbol_override_controller.ApplySymbolOverridesByAffix(Assets.GetAnim(egg_anim), symbolPrefix, null, 0);
		}
		KPrefabID creature_prefab_id = prefab.GetComponent<KPrefabID>();
		creature_prefab_id.prefabSpawnFn += delegate(GameObject inst)
		{
			DiscoveredResources.Instance.Discover(eggId.ToTag(), DiscoveredResources.GetCategoryForTags(egg_prefab_id.Tags));
			DiscoveredResources.Instance.Discover(baby_id.ToTag(), DiscoveredResources.GetCategoryForTags(creature_prefab_id.Tags));
		};
		if (is_ranchable)
		{
			prefab.AddOrGetDef<RanchableMonitor.Def>();
		}
		if (add_fish_overcrowding_monitor)
		{
			gameObject.AddOrGetDef<FishOvercrowdingMonitor.Def>();
		}
		if (deprecated)
		{
			gameObject.AddTag(GameTags.DeprecatedContent);
			prefab.AddTag(GameTags.DeprecatedContent);
		}
		return prefab;
	}

	// Token: 0x0600024F RID: 591 RVA: 0x00010404 File Offset: 0x0000E604
	[Obsolete("Mod compatibility: use ExtendEntityToFertileCreature passing dlcIds")]
	public static GameObject ExtendEntityToFertileCreature(GameObject prefab, string eggId, string eggName, string eggDesc, string egg_anim, float egg_mass, string baby_id, float fertility_cycles, float incubation_cycles, List<FertilityMonitor.BreedingChance> egg_chances, int eggSortOrder = -1, bool is_ranchable = true, bool add_fish_overcrowding_monitor = false, bool add_fixed_capturable_monitor = true, float egg_anim_scale = 1f, bool deprecated = false)
	{
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_mass, baby_id, fertility_cycles, incubation_cycles, egg_chances, DlcManager.AVAILABLE_ALL_VERSIONS, eggSortOrder, is_ranchable, add_fish_overcrowding_monitor, add_fixed_capturable_monitor, egg_anim_scale, deprecated);
	}

	// Token: 0x06000250 RID: 592 RVA: 0x00010438 File Offset: 0x0000E638
	public static GameObject ExtendEntityToBeingABaby(GameObject prefab, Tag adult_prefab_id, string on_grow_item_drop_id = null, bool force_adult_nav_type = false, float adult_threshold = 5f)
	{
		prefab.AddOrGetDef<BabyMonitor.Def>().adultPrefab = adult_prefab_id;
		prefab.AddOrGetDef<BabyMonitor.Def>().onGrowDropID = on_grow_item_drop_id;
		prefab.AddOrGetDef<BabyMonitor.Def>().forceAdultNavType = force_adult_nav_type;
		prefab.AddOrGetDef<BabyMonitor.Def>().adultThreshold = adult_threshold;
		prefab.AddOrGetDef<IncubatorMonitor.Def>();
		prefab.AddOrGetDef<CreatureSleepMonitor.Def>();
		prefab.AddOrGetDef<CallAdultMonitor.Def>();
		prefab.AddOrGetDef<AgeMonitor.Def>().maxAgePercentOnSpawn = 0.01f;
		Pickupable pickupable = prefab.AddOrGet<Pickupable>();
		int sortOrder = Assets.GetPrefab(adult_prefab_id).GetComponent<Pickupable>().sortOrder + 1;
		pickupable.sortOrder = sortOrder;
		return prefab;
	}

	// Token: 0x06000251 RID: 593 RVA: 0x000104BC File Offset: 0x0000E6BC
	public static GameObject ExtendEntityToBasicCreature(GameObject template, FactionManager.FactionID faction = FactionManager.FactionID.Prey, string initialTraitID = null, string NavGridName = "WalkerNavGrid1x1", NavType navType = NavType.Floor, int max_probing_radius = 32, float moveSpeed = 2f, string onDeathDropID = "Meat", int onDeathDropCount = 1, bool drownVulnerable = true, bool entombVulnerable = true, float warningLowTemperature = 283.15f, float warningHighTemperature = 293.15f, float lethalLowTemperature = 243.15f, float lethalHighTemperature = 343.15f)
	{
		return EntityTemplates.ExtendEntityToBasicCreature(false, template, faction, initialTraitID, NavGridName, navType, max_probing_radius, moveSpeed, onDeathDropID, onDeathDropCount, drownVulnerable, entombVulnerable, warningLowTemperature, warningHighTemperature, lethalLowTemperature, lethalHighTemperature);
	}

	// Token: 0x06000252 RID: 594 RVA: 0x000104EC File Offset: 0x0000E6EC
	public static GameObject ExtendEntityToBasicCreature(bool isWarmBlooded, GameObject template, FactionManager.FactionID faction = FactionManager.FactionID.Prey, string initialTraitID = null, string NavGridName = "WalkerNavGrid1x1", NavType navType = NavType.Floor, int max_probing_radius = 32, float moveSpeed = 2f, string onDeathDropID = "Meat", int onDeathDropCount = 1, bool drownVulnerable = true, bool entombVulnerable = true, float warningLowTemperature = 283.15f, float warningHighTemperature = 293.15f, float lethalLowTemperature = 243.15f, float lethalHighTemperature = 343.15f)
	{
		template.GetComponent<KBatchedAnimController>().isMovable = true;
		template.AddOrGet<KPrefabID>().AddTag(GameTags.Creature, false);
		Modifiers modifiers = template.AddOrGet<Modifiers>();
		if (initialTraitID != null)
		{
			modifiers.initialTraits.Add(initialTraitID);
		}
		modifiers.initialAmounts.Add(Db.Get().Amounts.HitPoints.Id);
		template.AddOrGet<KBatchedAnimController>().SetSymbolVisiblity("snapto_pivot", false);
		Pickupable pickupable = template.AddOrGet<Pickupable>();
		int sortOrder = -1;
		string name = template.PrefabID().Name;
		if (TUNING.CREATURES.SORTING.CRITTER_ORDER.ContainsKey(name))
		{
			sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER[name];
		}
		pickupable.sortOrder = sortOrder;
		template.AddOrGet<Clearable>().isClearable = false;
		template.AddOrGet<Traits>();
		template.AddOrGet<Health>().isCritter = true;
		template.AddOrGet<CharacterOverlay>();
		template.AddOrGet<RangedAttackable>();
		template.AddOrGet<FactionAlignment>().Alignment = faction;
		template.AddOrGet<Prioritizable>();
		template.AddOrGet<Effects>();
		template.AddOrGetDef<CreatureDebugGoToMonitor.Def>();
		template.AddOrGetDef<DeathMonitor.Def>();
		template.AddOrGetDef<CreatureThoughtGraph.Def>();
		template.AddOrGetDef<AnimInterruptMonitor.Def>();
		template.AddOrGet<AnimEventHandler>();
		SymbolOverrideControllerUtil.AddToPrefab(template);
		CritterTemperatureMonitor.Def def = template.AddOrGetDef<CritterTemperatureMonitor.Def>();
		def.temperatureHotDeadly = lethalHighTemperature;
		def.temperatureHotUncomfortable = warningHighTemperature;
		def.temperatureColdDeadly = lethalLowTemperature;
		def.temperatureColdUncomfortable = warningLowTemperature;
		template.GetComponent<PrimaryElement>().Temperature = def.GetIdealTemperature();
		modifiers.initialAmounts.Add(Db.Get().Amounts.CritterTemperature.Id);
		if (isWarmBlooded)
		{
			string properName = template.GetProperName();
			template.UpdateComponentRequirement(false);
			CreatureSimTemperatureTransfer creatureSimTemperatureTransfer = template.AddOrGet<CreatureSimTemperatureTransfer>();
			creatureSimTemperatureTransfer.temperatureAttributeName = "CritterTemperature";
			creatureSimTemperatureTransfer.SurfaceArea = 17.5f;
			creatureSimTemperatureTransfer.Thickness = 0.025f;
			creatureSimTemperatureTransfer.GroundTransferScale = 0f;
			creatureSimTemperatureTransfer.skinThickness = 0.025f;
			creatureSimTemperatureTransfer.skinThicknessAttributeModifierName = properName;
			WarmBlooded warmBlooded = template.AddOrGet<WarmBlooded>();
			warmBlooded.TemperatureAmountName = "CritterTemperature";
			warmBlooded.complexity = WarmBlooded.ComplexityType.SimpleHeatProduction;
			warmBlooded.IdealTemperature = def.GetIdealTemperature();
			warmBlooded.BaseGenerationKW = 10f;
			warmBlooded.BaseTemperatureModifierDescription = properName;
		}
		if (drownVulnerable)
		{
			template.AddOrGet<DrowningMonitor>();
		}
		if (entombVulnerable)
		{
			template.AddOrGet<EntombVulnerable>();
		}
		if (onDeathDropCount > 0 && onDeathDropID != "")
		{
			string[] array = new string[onDeathDropCount];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = onDeathDropID;
			}
			template.AddOrGet<Butcherable>().SetDrops(array);
		}
		Navigator navigator = template.AddOrGet<Navigator>();
		navigator.NavGridName = NavGridName;
		navigator.CurrentNavType = navType;
		navigator.defaultSpeed = moveSpeed;
		navigator.updateProber = true;
		navigator.maxProbingRadius = max_probing_radius;
		navigator.sceneLayer = Grid.SceneLayer.Creatures;
		return template;
	}

	// Token: 0x06000253 RID: 595 RVA: 0x00010780 File Offset: 0x0000E980
	public static void AddCreatureBrain(GameObject prefab, ChoreTable.Builder chore_table, Tag species, string symbol_prefix)
	{
		CreatureBrain creatureBrain = prefab.AddOrGet<CreatureBrain>();
		creatureBrain.species = species;
		creatureBrain.symbolPrefix = symbol_prefix;
		if (chore_table.HasChoreType(typeof(CritterCondoStates.Def)))
		{
			prefab.AddOrGetDef<CritterCondoInteractMontior.Def>();
		}
		DrinkMilkStates.Def def;
		if (chore_table.TryGetChoreDef<DrinkMilkStates.Def>(out def))
		{
			prefab.AddOrGetDef<DrinkMilkMonitor.Def>().drinkCellOffsetGetFn = def.drinkCellOffsetGetFn;
		}
		ChoreConsumer chore_consumer = prefab.AddOrGet<ChoreConsumer>();
		chore_consumer.choreTable = chore_table.CreateTable();
		KPrefabID kprefabID = prefab.AddOrGet<KPrefabID>();
		kprefabID.AddTag(GameTags.CreatureBrain, false);
		kprefabID.instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<ChoreConsumer>().choreTable = chore_consumer.choreTable;
		};
		kprefabID.prefabSpawnFn += delegate(GameObject go)
		{
			Game.BrainScheduler.PrioritizeBrain(go.GetComponent<CreatureBrain>());
		};
	}

	// Token: 0x06000254 RID: 596 RVA: 0x0001083F File Offset: 0x0000EA3F
	public static Tag GetBaggedCreatureTag(Tag tag)
	{
		return TagManager.Create("Bagged" + tag.Name);
	}

	// Token: 0x06000255 RID: 597 RVA: 0x00010857 File Offset: 0x0000EA57
	public static Tag GetUnbaggedCreatureTag(Tag bagged_tag)
	{
		return TagManager.Create(bagged_tag.Name.Substring(6));
	}

	// Token: 0x06000256 RID: 598 RVA: 0x0001086B File Offset: 0x0000EA6B
	public static string GetBaggedCreatureID(string name)
	{
		return "Bagged" + name;
	}

	// Token: 0x06000257 RID: 599 RVA: 0x00010878 File Offset: 0x0000EA78
	public static GameObject CreateAndRegisterBaggedCreature(GameObject creature, bool must_stand_on_top_for_pickup, bool allow_mark_for_capture, bool use_gun_for_pickup = false)
	{
		KPrefabID creature_prefab_id = creature.GetComponent<KPrefabID>();
		creature_prefab_id.AddTag(GameTags.BagableCreature, false);
		Baggable baggable = creature.AddOrGet<Baggable>();
		baggable.mustStandOntopOfTrapForPickup = must_stand_on_top_for_pickup;
		baggable.useGunForPickup = use_gun_for_pickup;
		creature.AddOrGet<Capturable>().allowCapture = allow_mark_for_capture;
		if (allow_mark_for_capture)
		{
			creature.AddComponent<Movable>();
		}
		creature_prefab_id.prefabSpawnFn += delegate(GameObject inst)
		{
			DiscoveredResources.Instance.Discover(creature_prefab_id.PrefabTag, DiscoveredResources.GetCategoryForTags(creature_prefab_id.Tags));
		};
		return creature;
	}

	// Token: 0x06000258 RID: 600 RVA: 0x000108EC File Offset: 0x0000EAEC
	public static GameObject CreateLooseEntity(string id, string name, string desc, float mass, bool unitMass, KAnimFile anim, string initialAnim, Grid.SceneLayer sceneLayer, EntityTemplates.CollisionShape collisionShape, float width = 1f, float height = 1f, bool isPickupable = false, int sortOrder = 0, SimHashes element = SimHashes.Creature, List<Tag> additionalTags = null)
	{
		GameObject gameObject = EntityTemplates.CreateBasicEntity(id, name, desc, mass, unitMass, anim, initialAnim, sceneLayer, element, additionalTags, 293f);
		gameObject = EntityTemplates.AddCollision(gameObject, collisionShape, width, height);
		gameObject.GetComponent<KBatchedAnimController>().isMovable = true;
		gameObject.AddOrGet<Modifiers>();
		if (isPickupable)
		{
			Pickupable pickupable = gameObject.AddOrGet<Pickupable>();
			pickupable.SetWorkTime(5f);
			pickupable.sortOrder = sortOrder;
			gameObject.AddOrGet<Movable>();
		}
		return gameObject;
	}

	// Token: 0x06000259 RID: 601 RVA: 0x00010958 File Offset: 0x0000EB58
	public static void CreateBaseOreTemplates()
	{
		EntityTemplates.baseOreTemplate = new GameObject("OreTemplate");
		UnityEngine.Object.DontDestroyOnLoad(EntityTemplates.baseOreTemplate);
		EntityTemplates.baseOreTemplate.SetActive(false);
		EntityTemplates.baseOreTemplate.AddComponent<KPrefabID>();
		EntityTemplates.baseOreTemplate.AddComponent<PrimaryElement>();
		EntityTemplates.baseOreTemplate.AddComponent<Pickupable>();
		EntityTemplates.baseOreTemplate.AddComponent<KSelectable>();
		EntityTemplates.baseOreTemplate.AddComponent<SaveLoadRoot>();
		EntityTemplates.baseOreTemplate.AddComponent<StateMachineController>();
		EntityTemplates.baseOreTemplate.AddComponent<Clearable>();
		EntityTemplates.baseOreTemplate.AddComponent<Prioritizable>();
		EntityTemplates.baseOreTemplate.AddComponent<KBatchedAnimController>();
		EntityTemplates.baseOreTemplate.AddComponent<SimTemperatureTransfer>();
		EntityTemplates.baseOreTemplate.AddComponent<Modifiers>();
		EntityTemplates.baseOreTemplate.AddComponent<Movable>();
		EntityTemplates.baseOreTemplate.AddOrGet<OccupyArea>().SetCellOffsets(new CellOffset[1]);
		DecorProvider decorProvider = EntityTemplates.baseOreTemplate.AddOrGet<DecorProvider>();
		decorProvider.baseDecor = -10f;
		decorProvider.baseRadius = 1f;
		EntityTemplates.baseOreTemplate.AddOrGet<ElementChunk>();
	}

	// Token: 0x0600025A RID: 602 RVA: 0x00010A4C File Offset: 0x0000EC4C
	public static void DestroyBaseOreTemplates()
	{
		UnityEngine.Object.Destroy(EntityTemplates.baseOreTemplate);
		EntityTemplates.baseOreTemplate = null;
	}

	// Token: 0x0600025B RID: 603 RVA: 0x00010A60 File Offset: 0x0000EC60
	public static GameObject CreateOreEntity(SimHashes elementID, EntityTemplates.CollisionShape shape, float width, float height, List<Tag> additionalTags = null, float default_temperature = 293f)
	{
		Element element = ElementLoader.FindElementByHash(elementID);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.baseOreTemplate);
		gameObject.name = element.name;
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		KPrefabID kprefabID = gameObject.AddOrGet<KPrefabID>();
		kprefabID.PrefabTag = element.tag;
		kprefabID.InitializeTags(false);
		if (additionalTags != null)
		{
			foreach (Tag tag in additionalTags)
			{
				kprefabID.AddTag(tag, false);
			}
		}
		if (element.lowTemp < 296.15f && element.highTemp > 296.15f)
		{
			kprefabID.AddTag(GameTags.PedestalDisplayable, false);
		}
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.SetElement(elementID, true);
		primaryElement.Mass = 1f;
		primaryElement.Temperature = default_temperature;
		Pickupable pickupable = gameObject.AddOrGet<Pickupable>();
		pickupable.SetWorkTime(5f);
		pickupable.sortOrder = element.buildMenuSort;
		gameObject.AddOrGet<KSelectable>().SetName(element.name);
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			element.substance.anim
		};
		kbatchedAnimController.sceneLayer = Grid.SceneLayer.Front;
		kbatchedAnimController.initialAnim = "idle1";
		kbatchedAnimController.isMovable = true;
		gameObject = EntityTemplates.AddCollision(gameObject, shape, width, height);
		return gameObject;
	}

	// Token: 0x0600025C RID: 604 RVA: 0x00010BB0 File Offset: 0x0000EDB0
	public static GameObject CreateSolidOreEntity(SimHashes elementId, List<Tag> additionalTags = null)
	{
		return EntityTemplates.CreateOreEntity(elementId, EntityTemplates.CollisionShape.CIRCLE, 0.5f, 0.5f, additionalTags, 293f);
	}

	// Token: 0x0600025D RID: 605 RVA: 0x00010BC9 File Offset: 0x0000EDC9
	public static GameObject CreateLiquidOreEntity(SimHashes elementId, List<Tag> additionalTags = null)
	{
		GameObject gameObject = EntityTemplates.CreateOreEntity(elementId, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.6f, additionalTags, 293f);
		gameObject.AddOrGet<Dumpable>().SetWorkTime(5f);
		gameObject.AddOrGet<SubstanceChunk>();
		return gameObject;
	}

	// Token: 0x0600025E RID: 606 RVA: 0x00010BF9 File Offset: 0x0000EDF9
	public static GameObject CreateGasOreEntity(SimHashes elementId, List<Tag> additionalTags = null)
	{
		GameObject gameObject = EntityTemplates.CreateOreEntity(elementId, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.6f, additionalTags, 293f);
		gameObject.AddOrGet<Dumpable>().SetWorkTime(5f);
		gameObject.AddOrGet<SubstanceChunk>();
		return gameObject;
	}

	// Token: 0x0600025F RID: 607 RVA: 0x00010C29 File Offset: 0x0000EE29
	public static GameObject ExtendEntityToFood(GameObject template, EdiblesManager.FoodInfo foodInfo)
	{
		return EntityTemplates.ExtendEntityToFood(template, foodInfo, true);
	}

	// Token: 0x06000260 RID: 608 RVA: 0x00010C34 File Offset: 0x0000EE34
	public static GameObject ExtendEntityToFood(GameObject template, EdiblesManager.FoodInfo foodInfo, bool splittable)
	{
		if (splittable)
		{
			template.AddOrGet<EntitySplitter>();
		}
		if (foodInfo.CanRot)
		{
			Rottable.Def def = template.AddOrGetDef<Rottable.Def>();
			def.preserveTemperature = foodInfo.PreserveTemperature;
			def.rotTemperature = foodInfo.RotTemperature;
			def.spoilTime = foodInfo.SpoilTime;
			def.staleTime = foodInfo.StaleTime;
			EntityTemplates.CreateAndRegisterCompostableFromPrefab(template);
		}
		KPrefabID component = template.GetComponent<KPrefabID>();
		component.AddTag(GameTags.PedestalDisplayable, false);
		if (foodInfo.CaloriesPerUnit > 0f)
		{
			component.AddTag(GameTags.Edible, false);
			template.AddOrGet<Edible>().FoodInfo = foodInfo;
			component.instantiateFn += delegate(GameObject go)
			{
				go.GetComponent<Edible>().FoodInfo = foodInfo;
			};
			GameTags.DisplayAsCalories.Add(component.PrefabTag);
		}
		else
		{
			component.AddTag(GameTags.CookingIngredient, false);
			template.AddOrGet<HasSortOrder>();
		}
		return template;
	}

	// Token: 0x06000261 RID: 609 RVA: 0x00010D30 File Offset: 0x0000EF30
	public static GameObject ExtendEntityToDehydratedFoodPackage(GameObject template, EdiblesManager.FoodInfo foodInfo)
	{
		KPrefabID component = template.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Dehydrated, false);
		component.AddTag(GameTags.PickupableStorage, false);
		Storage storage = template.AddComponent<Storage>();
		storage.allowItemRemoval = false;
		storage.capacityKg = 1f;
		storage.showInUI = false;
		storage.storageFilters = new List<Tag>
		{
			foodInfo.Id
		};
		DehydratedFoodPackage dehydratedFoodPackage = template.AddOrGet<DehydratedFoodPackage>();
		dehydratedFoodPackage.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_rehydrator_kanim")
		};
		dehydratedFoodPackage.workTime = 5f;
		dehydratedFoodPackage.workLayer = Grid.SceneLayer.Front;
		dehydratedFoodPackage.FoodTag = foodInfo.Id;
		return template;
	}

	// Token: 0x06000262 RID: 610 RVA: 0x00010DE0 File Offset: 0x0000EFE0
	public static GameObject ExtendEntityToMedicine(GameObject template, MedicineInfo medicineInfo)
	{
		template.AddOrGet<EntitySplitter>();
		KPrefabID component = template.GetComponent<KPrefabID>();
		global::Debug.Assert(component.PrefabID() == medicineInfo.id, "Tried assigning a medicine info to a non-matching prefab!");
		MedicinalPill medicinalPill = template.AddOrGet<MedicinalPill>();
		medicinalPill.info = medicineInfo;
		if (medicineInfo.doctorStationId == null)
		{
			template.AddOrGet<MedicinalPillWorkable>().pill = medicinalPill;
			component.AddTag(GameTags.Medicine, false);
		}
		else
		{
			component.AddTag(GameTags.MedicalSupplies, false);
			component.AddTag(medicineInfo.GetSupplyTag(), false);
		}
		return template;
	}

	// Token: 0x06000263 RID: 611 RVA: 0x00010E68 File Offset: 0x0000F068
	public static GameObject ExtendPlantToFertilizable(GameObject template, PlantElementAbsorber.ConsumeInfo[] fertilizers)
	{
		template.GetComponent<Modifiers>().initialAttributes.Add(Db.Get().PlantAttributes.FertilizerUsageMod.Id);
		HashedString idHash = Db.Get().ChoreTypes.FarmFetch.IdHash;
		foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in fertilizers)
		{
			ManualDeliveryKG manualDeliveryKG = template.AddComponent<ManualDeliveryKG>();
			manualDeliveryKG.RequestedItemTag = consumeInfo.tag;
			manualDeliveryKG.capacity = consumeInfo.massConsumptionRate * 600f * 3f;
			manualDeliveryKG.refillMass = consumeInfo.massConsumptionRate * 600f * 0.5f;
			manualDeliveryKG.MinimumMass = consumeInfo.massConsumptionRate * 600f * 0.5f;
			manualDeliveryKG.operationalRequirement = Operational.State.Functional;
			manualDeliveryKG.choreTypeIDHash = idHash;
		}
		KPrefabID component = template.GetComponent<KPrefabID>();
		FertilizationMonitor.Def def = template.AddOrGetDef<FertilizationMonitor.Def>();
		def.wrongFertilizerTestTag = GameTags.Solid;
		def.consumedElements = fertilizers;
		component.prefabInitFn += delegate(GameObject inst)
		{
			ManualDeliveryKG[] components = inst.GetComponents<ManualDeliveryKG>();
			for (int j = 0; j < components.Length; j++)
			{
				components[j].Pause(true, "init");
			}
		};
		return template;
	}

	// Token: 0x06000264 RID: 612 RVA: 0x00010F6F File Offset: 0x0000F16F
	public static GameObject ExtendPlantToIrrigated(GameObject template, PlantElementAbsorber.ConsumeInfo info)
	{
		return EntityTemplates.ExtendPlantToIrrigated(template, new PlantElementAbsorber.ConsumeInfo[]
		{
			info
		});
	}

	// Token: 0x06000265 RID: 613 RVA: 0x00010F88 File Offset: 0x0000F188
	public static GameObject ExtendPlantToIrrigated(GameObject template, PlantElementAbsorber.ConsumeInfo[] consume_info)
	{
		template.GetComponent<Modifiers>().initialAttributes.Add(Db.Get().PlantAttributes.FertilizerUsageMod.Id);
		HashedString idHash = Db.Get().ChoreTypes.FarmFetch.IdHash;
		foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in consume_info)
		{
			ManualDeliveryKG manualDeliveryKG = template.AddComponent<ManualDeliveryKG>();
			manualDeliveryKG.RequestedItemTag = consumeInfo.tag;
			manualDeliveryKG.capacity = consumeInfo.massConsumptionRate * 600f * 3f;
			manualDeliveryKG.refillMass = consumeInfo.massConsumptionRate * 600f * 0.5f;
			manualDeliveryKG.MinimumMass = consumeInfo.massConsumptionRate * 600f * 0.5f;
			manualDeliveryKG.operationalRequirement = Operational.State.Functional;
			manualDeliveryKG.choreTypeIDHash = idHash;
		}
		IrrigationMonitor.Def def = template.AddOrGetDef<IrrigationMonitor.Def>();
		def.wrongIrrigationTestTag = GameTags.Liquid;
		def.consumedElements = consume_info;
		return template;
	}

	// Token: 0x06000266 RID: 614 RVA: 0x00011068 File Offset: 0x0000F268
	public static GameObject CreateAndRegisterCompostableFromPrefab(GameObject original)
	{
		if (original.GetComponent<Compostable>() != null)
		{
			return null;
		}
		original.AddComponent<Compostable>().isMarkedForCompost = false;
		KPrefabID component = original.GetComponent<KPrefabID>();
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		string tag_string = "Compost" + component.PrefabTag.Name;
		string text = MISC.TAGS.COMPOST_FORMAT.Replace("{Item}", component.PrefabTag.ProperName());
		gameObject.GetComponent<KPrefabID>().PrefabTag = TagManager.Create(tag_string, text);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Compostable, false);
		gameObject.name = text;
		gameObject.GetComponent<Compostable>().isMarkedForCompost = true;
		gameObject.GetComponent<KSelectable>().SetName(text);
		gameObject.GetComponent<Compostable>().originalPrefab = original;
		gameObject.GetComponent<Compostable>().compostPrefab = gameObject;
		original.GetComponent<Compostable>().originalPrefab = original;
		original.GetComponent<Compostable>().compostPrefab = gameObject;
		Assets.AddPrefab(gameObject.GetComponent<KPrefabID>());
		return gameObject;
	}

	// Token: 0x06000267 RID: 615 RVA: 0x00011154 File Offset: 0x0000F354
	public static GameObject CreateAndRegisterSeedForPlant(GameObject plant, SeedProducer.ProductionType productionType, string id, string name, string desc, KAnimFile anim, string initialAnim = "object", int numberOfSeeds = 1, List<Tag> additionalTags = null, SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top, Tag replantGroundTag = default(Tag), int sortOrder = 0, string domesticatedDescription = "", EntityTemplates.CollisionShape collisionShape = EntityTemplates.CollisionShape.CIRCLE, float width = 0.25f, float height = 0.25f, Recipe.Ingredient[] recipe_ingredients = null, string recipe_description = "", bool ignoreDefaultSeedTag = false, string[] dlcIds = null)
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(id, name, desc, 1f, true, anim, initialAnim, Grid.SceneLayer.Front, collisionShape, width, height, true, SORTORDER.SEEDS + sortOrder, SimHashes.Creature, null);
		gameObject.AddOrGet<EntitySplitter>();
		GameObject go = EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		PlantableSeed plantableSeed = gameObject.AddOrGet<PlantableSeed>();
		plantableSeed.PlantID = new Tag(plant.name);
		plantableSeed.replantGroundTag = replantGroundTag;
		plantableSeed.domesticatedDescription = domesticatedDescription;
		plantableSeed.direction = planterDirection;
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		foreach (Tag tag in additionalTags)
		{
			component.AddTag(tag, false);
		}
		component.requiredDlcIds = ((dlcIds == null) ? DlcManager.AVAILABLE_ALL_VERSIONS : dlcIds);
		if (!ignoreDefaultSeedTag)
		{
			component.AddTag(GameTags.Seed, false);
		}
		component.AddTag(GameTags.PedestalDisplayable, false);
		MutantPlant component2 = plant.GetComponent<MutantPlant>();
		if (component2 != null)
		{
			MutantPlant mutantPlant = gameObject.AddOrGet<MutantPlant>();
			MutantPlant mutantPlant2 = go.AddOrGet<MutantPlant>();
			mutantPlant.SpeciesID = component2.SpeciesID;
			mutantPlant2.SpeciesID = component2.SpeciesID;
		}
		Assets.AddPrefab(component);
		plant.AddOrGet<SeedProducer>().Configure(id, productionType, numberOfSeeds);
		return gameObject;
	}

	// Token: 0x06000268 RID: 616 RVA: 0x00011290 File Offset: 0x0000F490
	public static GameObject CreateAndRegisterPreview(string id, KAnimFile anim, string initial_anim, ObjectLayer object_layer, int width, int height)
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, id, id, 1f, anim, initial_anim, Grid.SceneLayer.Front, width, height, TUNING.BUILDINGS.DECOR.NONE, default(EffectorValues), SimHashes.Creature, null, 293f);
		gameObject.UpdateComponentRequirement(false);
		gameObject.UpdateComponentRequirement(false);
		gameObject.AddOrGet<EntityPreview>().objectLayer = object_layer;
		OccupyArea occupyArea = gameObject.AddOrGet<OccupyArea>();
		occupyArea.objectLayers = new ObjectLayer[]
		{
			object_layer
		};
		occupyArea.ApplyToCells = false;
		gameObject.AddOrGet<Storage>();
		Assets.AddPrefab(gameObject.GetComponent<KPrefabID>());
		return gameObject;
	}

	// Token: 0x06000269 RID: 617 RVA: 0x00011319 File Offset: 0x0000F519
	public static GameObject CreateAndRegisterPreviewForPlant(GameObject seed, string id, KAnimFile anim, string initialAnim, int width, int height)
	{
		GameObject result = EntityTemplates.CreateAndRegisterPreview(id, anim, initialAnim, ObjectLayer.Building, width, height);
		seed.GetComponent<PlantableSeed>().PreviewID = TagManager.Create(id);
		return result;
	}

	// Token: 0x0600026A RID: 618 RVA: 0x0001133C File Offset: 0x0000F53C
	public static CellOffset[] GenerateOffsets(int width, int height)
	{
		int num = width / 2;
		int startX = num - width + 1;
		int startY = 0;
		int endY = height - 1;
		return EntityTemplates.GenerateOffsets(startX, startY, num, endY);
	}

	// Token: 0x0600026B RID: 619 RVA: 0x00011360 File Offset: 0x0000F560
	private static CellOffset[] GenerateOffsets(int startX, int startY, int endX, int endY)
	{
		List<CellOffset> list = new List<CellOffset>();
		for (int i = startY; i <= endY; i++)
		{
			for (int j = startX; j <= endX; j++)
			{
				list.Add(new CellOffset
				{
					x = j,
					y = i
				});
			}
		}
		return list.ToArray();
	}

	// Token: 0x0600026C RID: 620 RVA: 0x000113B0 File Offset: 0x0000F5B0
	public static CellOffset[] GenerateHangingOffsets(int width, int height)
	{
		int num = width / 2;
		int startX = num - width + 1;
		int startY = -height + 1;
		int endY = 0;
		return EntityTemplates.GenerateOffsets(startX, startY, num, endY);
	}

	// Token: 0x0600026D RID: 621 RVA: 0x000113D8 File Offset: 0x0000F5D8
	public static GameObject AddCollision(GameObject template, EntityTemplates.CollisionShape shape, float width, float height)
	{
		if (shape != EntityTemplates.CollisionShape.RECTANGLE)
		{
			if (shape != EntityTemplates.CollisionShape.POLYGONAL)
			{
				template.AddOrGet<KCircleCollider2D>().radius = Mathf.Max(width, height);
			}
			else
			{
				template.AddOrGet<PolygonCollider2D>();
			}
		}
		else
		{
			template.AddOrGet<KBoxCollider2D>().size = new Vector2f(width, height);
		}
		return template;
	}

	// Token: 0x0400016B RID: 363
	private static GameObject selectableEntityTemplate;

	// Token: 0x0400016C RID: 364
	private static GameObject unselectableEntityTemplate;

	// Token: 0x0400016D RID: 365
	private static GameObject baseEntityTemplate;

	// Token: 0x0400016E RID: 366
	private static GameObject placedEntityTemplate;

	// Token: 0x0400016F RID: 367
	private static GameObject baseOreTemplate;

	// Token: 0x02000FC1 RID: 4033
	public enum CollisionShape
	{
		// Token: 0x04005B74 RID: 23412
		CIRCLE,
		// Token: 0x04005B75 RID: 23413
		RECTANGLE,
		// Token: 0x04005B76 RID: 23414
		POLYGONAL
	}
}
