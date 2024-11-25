using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Database;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020005FA RID: 1530
public class Db : EntityModifierSet
{
	// Token: 0x06002580 RID: 9600 RVA: 0x000D15E8 File Offset: 0x000CF7E8
	public static string GetPath(string dlcId, string folder)
	{
		string result;
		if (dlcId == "")
		{
			result = FileSystem.Normalize(Path.Combine(Application.streamingAssetsPath, folder));
		}
		else
		{
			string contentDirectoryName = DlcManager.GetContentDirectoryName(dlcId);
			result = FileSystem.Normalize(Path.Combine(Application.streamingAssetsPath, "dlc", contentDirectoryName, folder));
		}
		return result;
	}

	// Token: 0x06002581 RID: 9601 RVA: 0x000D1634 File Offset: 0x000CF834
	public static Db Get()
	{
		if (Db._Instance == null)
		{
			Db._Instance = Resources.Load<Db>("Db");
			Db._Instance.Initialize();
		}
		return Db._Instance;
	}

	// Token: 0x06002582 RID: 9602 RVA: 0x000D1661 File Offset: 0x000CF861
	public static BuildingFacades GetBuildingFacades()
	{
		return Db.Get().Permits.BuildingFacades;
	}

	// Token: 0x06002583 RID: 9603 RVA: 0x000D1672 File Offset: 0x000CF872
	public static ArtableStages GetArtableStages()
	{
		return Db.Get().Permits.ArtableStages;
	}

	// Token: 0x06002584 RID: 9604 RVA: 0x000D1683 File Offset: 0x000CF883
	public static EquippableFacades GetEquippableFacades()
	{
		return Db.Get().Permits.EquippableFacades;
	}

	// Token: 0x06002585 RID: 9605 RVA: 0x000D1694 File Offset: 0x000CF894
	public static StickerBombs GetStickerBombs()
	{
		return Db.Get().Permits.StickerBombs;
	}

	// Token: 0x06002586 RID: 9606 RVA: 0x000D16A5 File Offset: 0x000CF8A5
	public static MonumentParts GetMonumentParts()
	{
		return Db.Get().Permits.MonumentParts;
	}

	// Token: 0x06002587 RID: 9607 RVA: 0x000D16B8 File Offset: 0x000CF8B8
	public override void Initialize()
	{
		base.Initialize();
		this.Urges = new Urges();
		this.AssignableSlots = new AssignableSlots();
		this.StateMachineCategories = new StateMachineCategories();
		this.Personalities = new Personalities();
		this.Faces = new Faces();
		this.Shirts = new Shirts();
		this.Expressions = new Expressions(this.Root);
		this.Emotes = new Emotes(this.Root);
		this.Thoughts = new Thoughts(this.Root);
		this.Dreams = new Dreams(this.Root);
		this.Deaths = new Deaths(this.Root);
		this.StatusItemCategories = new StatusItemCategories(this.Root);
		this.TechTreeTitles = new TechTreeTitles(this.Root);
		this.TechTreeTitles.Load(DlcManager.IsExpansion1Active() ? this.researchTreeFileExpansion1 : this.researchTreeFileVanilla);
		this.Techs = new Techs(this.Root);
		this.TechItems = new TechItems(this.Root);
		this.Techs.Init();
		this.Techs.Load(DlcManager.IsExpansion1Active() ? this.researchTreeFileExpansion1 : this.researchTreeFileVanilla);
		this.TechItems.Init();
		this.Accessories = new Accessories(this.Root);
		this.AccessorySlots = new AccessorySlots(this.Root);
		this.ScheduleBlockTypes = new ScheduleBlockTypes(this.Root);
		this.ScheduleGroups = new ScheduleGroups(this.Root);
		this.RoomTypeCategories = new RoomTypeCategories(this.Root);
		this.RoomTypes = new RoomTypes(this.Root);
		this.ArtifactDropRates = new ArtifactDropRates(this.Root);
		this.SpaceDestinationTypes = new SpaceDestinationTypes(this.Root);
		this.Diseases = new Diseases(this.Root, false);
		this.Sicknesses = new Database.Sicknesses(this.Root);
		this.SkillPerks = new SkillPerks(this.Root);
		this.SkillGroups = new SkillGroups(this.Root);
		this.Skills = new Skills(this.Root);
		this.ColonyAchievements = new ColonyAchievements(this.Root);
		this.MiscStatusItems = new MiscStatusItems(this.Root);
		this.CreatureStatusItems = new CreatureStatusItems(this.Root);
		this.BuildingStatusItems = new BuildingStatusItems(this.Root);
		this.RobotStatusItems = new RobotStatusItems(this.Root);
		this.ChoreTypes = new ChoreTypes(this.Root);
		this.Quests = new Quests(this.Root);
		this.GameplayEvents = new GameplayEvents(this.Root);
		this.GameplaySeasons = new GameplaySeasons(this.Root);
		this.Stories = new Stories(this.Root);
		if (DlcManager.FeaturePlantMutationsEnabled())
		{
			this.PlantMutations = new PlantMutations(this.Root);
		}
		this.OrbitalTypeCategories = new OrbitalTypeCategories(this.Root);
		this.ArtableStatuses = new ArtableStatuses(this.Root);
		this.Permits = new PermitResources(this.Root);
		Effect effect = new Effect("CenterOfAttention", DUPLICANTS.MODIFIERS.CENTEROFATTENTION.NAME, DUPLICANTS.MODIFIERS.CENTEROFATTENTION.TOOLTIP, 0f, true, true, false, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier("StressDelta", -0.008333334f, DUPLICANTS.MODIFIERS.CENTEROFATTENTION.NAME, false, false, true));
		this.effects.Add(effect);
		this.Spices = new Spices(this.Root);
		this.CollectResources(this.Root, this.ResourceTable);
	}

	// Token: 0x06002588 RID: 9608 RVA: 0x000D1A59 File Offset: 0x000CFC59
	public void PostProcess()
	{
		this.Techs.PostProcess();
		this.Permits.PostProcess();
	}

	// Token: 0x06002589 RID: 9609 RVA: 0x000D1A74 File Offset: 0x000CFC74
	private void CollectResources(Resource resource, List<Resource> resource_table)
	{
		if (resource.Guid != null)
		{
			resource_table.Add(resource);
		}
		ResourceSet resourceSet = resource as ResourceSet;
		if (resourceSet != null)
		{
			for (int i = 0; i < resourceSet.Count; i++)
			{
				this.CollectResources(resourceSet.GetResource(i), resource_table);
			}
		}
	}

	// Token: 0x0600258A RID: 9610 RVA: 0x000D1AC0 File Offset: 0x000CFCC0
	public ResourceType GetResource<ResourceType>(ResourceGuid guid) where ResourceType : Resource
	{
		Resource resource = this.ResourceTable.FirstOrDefault((Resource s) => s.Guid == guid);
		if (resource == null)
		{
			string str = "Could not find resource: ";
			ResourceGuid guid2 = guid;
			global::Debug.LogWarning(str + ((guid2 != null) ? guid2.ToString() : null));
			return default(ResourceType);
		}
		ResourceType resourceType = (ResourceType)((object)resource);
		if (resourceType == null)
		{
			global::Debug.LogError(string.Concat(new string[]
			{
				"Resource type mismatch for resource: ",
				resource.Id,
				"\nExpecting Type: ",
				typeof(ResourceType).Name,
				"\nGot Type: ",
				resource.GetType().Name
			}));
			return default(ResourceType);
		}
		return resourceType;
	}

	// Token: 0x0600258B RID: 9611 RVA: 0x000D1B8B File Offset: 0x000CFD8B
	public void ResetProblematicDbs()
	{
		this.Emotes.ResetProblematicReferences();
	}

	// Token: 0x04001539 RID: 5433
	private static Db _Instance;

	// Token: 0x0400153A RID: 5434
	public TextAsset researchTreeFileVanilla;

	// Token: 0x0400153B RID: 5435
	public TextAsset researchTreeFileExpansion1;

	// Token: 0x0400153C RID: 5436
	public Diseases Diseases;

	// Token: 0x0400153D RID: 5437
	public Database.Sicknesses Sicknesses;

	// Token: 0x0400153E RID: 5438
	public Urges Urges;

	// Token: 0x0400153F RID: 5439
	public AssignableSlots AssignableSlots;

	// Token: 0x04001540 RID: 5440
	public StateMachineCategories StateMachineCategories;

	// Token: 0x04001541 RID: 5441
	public Personalities Personalities;

	// Token: 0x04001542 RID: 5442
	public Faces Faces;

	// Token: 0x04001543 RID: 5443
	public Shirts Shirts;

	// Token: 0x04001544 RID: 5444
	public Expressions Expressions;

	// Token: 0x04001545 RID: 5445
	public Emotes Emotes;

	// Token: 0x04001546 RID: 5446
	public Thoughts Thoughts;

	// Token: 0x04001547 RID: 5447
	public Dreams Dreams;

	// Token: 0x04001548 RID: 5448
	public BuildingStatusItems BuildingStatusItems;

	// Token: 0x04001549 RID: 5449
	public MiscStatusItems MiscStatusItems;

	// Token: 0x0400154A RID: 5450
	public CreatureStatusItems CreatureStatusItems;

	// Token: 0x0400154B RID: 5451
	public RobotStatusItems RobotStatusItems;

	// Token: 0x0400154C RID: 5452
	public StatusItemCategories StatusItemCategories;

	// Token: 0x0400154D RID: 5453
	public Deaths Deaths;

	// Token: 0x0400154E RID: 5454
	public ChoreTypes ChoreTypes;

	// Token: 0x0400154F RID: 5455
	public TechItems TechItems;

	// Token: 0x04001550 RID: 5456
	public AccessorySlots AccessorySlots;

	// Token: 0x04001551 RID: 5457
	public Accessories Accessories;

	// Token: 0x04001552 RID: 5458
	public ScheduleBlockTypes ScheduleBlockTypes;

	// Token: 0x04001553 RID: 5459
	public ScheduleGroups ScheduleGroups;

	// Token: 0x04001554 RID: 5460
	public RoomTypeCategories RoomTypeCategories;

	// Token: 0x04001555 RID: 5461
	public RoomTypes RoomTypes;

	// Token: 0x04001556 RID: 5462
	public ArtifactDropRates ArtifactDropRates;

	// Token: 0x04001557 RID: 5463
	public SpaceDestinationTypes SpaceDestinationTypes;

	// Token: 0x04001558 RID: 5464
	public SkillPerks SkillPerks;

	// Token: 0x04001559 RID: 5465
	public SkillGroups SkillGroups;

	// Token: 0x0400155A RID: 5466
	public Skills Skills;

	// Token: 0x0400155B RID: 5467
	public ColonyAchievements ColonyAchievements;

	// Token: 0x0400155C RID: 5468
	public Quests Quests;

	// Token: 0x0400155D RID: 5469
	public GameplayEvents GameplayEvents;

	// Token: 0x0400155E RID: 5470
	public GameplaySeasons GameplaySeasons;

	// Token: 0x0400155F RID: 5471
	public PlantMutations PlantMutations;

	// Token: 0x04001560 RID: 5472
	public Spices Spices;

	// Token: 0x04001561 RID: 5473
	public Techs Techs;

	// Token: 0x04001562 RID: 5474
	public TechTreeTitles TechTreeTitles;

	// Token: 0x04001563 RID: 5475
	public OrbitalTypeCategories OrbitalTypeCategories;

	// Token: 0x04001564 RID: 5476
	public PermitResources Permits;

	// Token: 0x04001565 RID: 5477
	public ArtableStatuses ArtableStatuses;

	// Token: 0x04001566 RID: 5478
	public Stories Stories;

	// Token: 0x020013ED RID: 5101
	[Serializable]
	public class SlotInfo : Resource
	{
	}
}
