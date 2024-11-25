using System;
using STRINGS;
using TUNING;

namespace Database
{
	// Token: 0x02000ECF RID: 3791
	public class SkillPerks : ResourceSet<SkillPerk>
	{
		// Token: 0x06007639 RID: 30265 RVA: 0x002E49D0 File Offset: 0x002E2BD0
		public SkillPerks(ResourceSet parent) : base("SkillPerks", parent)
		{
			this.IncreaseDigSpeedSmall = base.Add(new SkillAttributePerk("IncreaseDigSpeedSmall", Db.Get().Attributes.Digging.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_MINER.NAME));
			this.IncreaseDigSpeedMedium = base.Add(new SkillAttributePerk("IncreaseDigSpeedMedium", Db.Get().Attributes.Digging.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.MINER.NAME));
			this.IncreaseDigSpeedLarge = base.Add(new SkillAttributePerk("IncreaseDigSpeedLarge", Db.Get().Attributes.Digging.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_MINER.NAME));
			this.CanDigVeryFirm = base.Add(new SimpleSkillPerk("CanDigVeryFirm", UI.ROLES_SCREEN.PERKS.CAN_DIG_VERY_FIRM.DESCRIPTION));
			this.CanDigNearlyImpenetrable = base.Add(new SimpleSkillPerk("CanDigAbyssalite", UI.ROLES_SCREEN.PERKS.CAN_DIG_NEARLY_IMPENETRABLE.DESCRIPTION));
			this.CanDigSuperDuperHard = base.Add(new SimpleSkillPerk("CanDigDiamondAndObsidan", UI.ROLES_SCREEN.PERKS.CAN_DIG_SUPER_SUPER_HARD.DESCRIPTION));
			this.CanDigRadioactiveMaterials = base.Add(new SimpleSkillPerk("CanDigCorium", UI.ROLES_SCREEN.PERKS.CAN_DIG_RADIOACTIVE_MATERIALS.DESCRIPTION));
			this.CanDigUnobtanium = base.Add(new SimpleSkillPerk("CanDigUnobtanium", UI.ROLES_SCREEN.PERKS.CAN_DIG_UNOBTANIUM.DESCRIPTION));
			this.IncreaseConstructionSmall = base.Add(new SkillAttributePerk("IncreaseConstructionSmall", Db.Get().Attributes.Construction.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_BUILDER.NAME));
			this.IncreaseConstructionMedium = base.Add(new SkillAttributePerk("IncreaseConstructionMedium", Db.Get().Attributes.Construction.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.BUILDER.NAME));
			this.IncreaseConstructionLarge = base.Add(new SkillAttributePerk("IncreaseConstructionLarge", Db.Get().Attributes.Construction.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_BUILDER.NAME));
			this.IncreaseConstructionMechatronics = base.Add(new SkillAttributePerk("IncreaseConstructionMechatronics", Db.Get().Attributes.Construction.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME));
			this.CanDemolish = base.Add(new SimpleSkillPerk("CanDemonlish", UI.ROLES_SCREEN.PERKS.CAN_DEMOLISH.DESCRIPTION));
			this.IncreaseLearningSmall = base.Add(new SkillAttributePerk("IncreaseLearningSmall", Db.Get().Attributes.Learning.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_RESEARCHER.NAME));
			this.IncreaseLearningMedium = base.Add(new SkillAttributePerk("IncreaseLearningMedium", Db.Get().Attributes.Learning.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.RESEARCHER.NAME));
			this.IncreaseLearningLarge = base.Add(new SkillAttributePerk("IncreaseLearningLarge", Db.Get().Attributes.Learning.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_RESEARCHER.NAME));
			this.IncreaseLearningLargeSpace = base.Add(new SkillAttributePerk("IncreaseLearningLargeSpace", Db.Get().Attributes.Learning.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SPACE_RESEARCHER.NAME));
			this.IncreaseBotanySmall = base.Add(new SkillAttributePerk("IncreaseBotanySmall", Db.Get().Attributes.Botanist.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_FARMER.NAME));
			this.IncreaseBotanyMedium = base.Add(new SkillAttributePerk("IncreaseBotanyMedium", Db.Get().Attributes.Botanist.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.FARMER.NAME));
			this.IncreaseBotanyLarge = base.Add(new SkillAttributePerk("IncreaseBotanyLarge", Db.Get().Attributes.Botanist.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_FARMER.NAME));
			this.CanFarmTinker = base.Add(new SimpleSkillPerk("CanFarmTinker", UI.ROLES_SCREEN.PERKS.CAN_FARM_TINKER.DESCRIPTION));
			this.CanIdentifyMutantSeeds = base.Add(new SimpleSkillPerk("CanIdentifyMutantSeeds", UI.ROLES_SCREEN.PERKS.CAN_IDENTIFY_MUTANT_SEEDS.DESCRIPTION));
			this.IncreaseRanchingSmall = base.Add(new SkillAttributePerk("IncreaseRanchingSmall", Db.Get().Attributes.Ranching.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.RANCHER.NAME));
			this.IncreaseRanchingMedium = base.Add(new SkillAttributePerk("IncreaseRanchingMedium", Db.Get().Attributes.Ranching.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.SENIOR_RANCHER.NAME));
			this.CanWrangleCreatures = base.Add(new SimpleSkillPerk("CanWrangleCreatures", UI.ROLES_SCREEN.PERKS.CAN_WRANGLE_CREATURES.DESCRIPTION));
			this.CanUseRanchStation = base.Add(new SimpleSkillPerk("CanUseRanchStation", UI.ROLES_SCREEN.PERKS.CAN_USE_RANCH_STATION.DESCRIPTION));
			this.CanUseMilkingStation = base.Add(new SimpleSkillPerk("CanUseMilkingStation", UI.ROLES_SCREEN.PERKS.CAN_USE_MILKING_STATION.DESCRIPTION));
			this.IncreaseAthleticsSmall = base.Add(new SkillAttributePerk("IncreaseAthleticsSmall", Db.Get().Attributes.Athletics.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.HAULER.NAME));
			this.IncreaseAthleticsMedium = base.Add(new SkillAttributePerk("IncreaseAthletics", Db.Get().Attributes.Athletics.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.SUIT_EXPERT.NAME));
			this.IncreaseAthleticsLarge = base.Add(new SkillAttributePerk("IncreaseAthleticsLarge", Db.Get().Attributes.Athletics.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.SUIT_DURABILITY.NAME));
			this.IncreaseStrengthGofer = base.Add(new SkillAttributePerk("IncreaseStrengthGofer", Db.Get().Attributes.Strength.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.HAULER.NAME));
			this.IncreaseStrengthCourier = base.Add(new SkillAttributePerk("IncreaseStrengthCourier", Db.Get().Attributes.Strength.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME));
			this.IncreaseStrengthGroundskeeper = base.Add(new SkillAttributePerk("IncreaseStrengthGroundskeeper", Db.Get().Attributes.Strength.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.HANDYMAN.NAME));
			this.IncreaseStrengthPlumber = base.Add(new SkillAttributePerk("IncreaseStrengthPlumber", Db.Get().Attributes.Strength.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.PLUMBER.NAME));
			this.IncreaseCarryAmountSmall = base.Add(new SkillAttributePerk("IncreaseCarryAmountSmall", Db.Get().Attributes.CarryAmount.Id, 400f, DUPLICANTS.ROLES.HAULER.NAME));
			this.IncreaseCarryAmountMedium = base.Add(new SkillAttributePerk("IncreaseCarryAmountMedium", Db.Get().Attributes.CarryAmount.Id, 800f, DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME));
			this.IncreaseArtSmall = base.Add(new SkillAttributePerk("IncreaseArtSmall", Db.Get().Attributes.Art.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_ARTIST.NAME));
			this.IncreaseArtMedium = base.Add(new SkillAttributePerk("IncreaseArt", Db.Get().Attributes.Art.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.ARTIST.NAME));
			this.IncreaseArtLarge = base.Add(new SkillAttributePerk("IncreaseArtLarge", Db.Get().Attributes.Art.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.MASTER_ARTIST.NAME));
			this.CanArt = base.Add(new SimpleSkillPerk("CanArt", UI.ROLES_SCREEN.PERKS.CAN_ART.DESCRIPTION));
			this.CanArtUgly = base.Add(new SimpleSkillPerk("CanArtUgly", UI.ROLES_SCREEN.PERKS.CAN_ART_UGLY.DESCRIPTION));
			this.CanArtOkay = base.Add(new SimpleSkillPerk("CanArtOkay", UI.ROLES_SCREEN.PERKS.CAN_ART_OKAY.DESCRIPTION));
			this.CanArtGreat = base.Add(new SimpleSkillPerk("CanArtGreat", UI.ROLES_SCREEN.PERKS.CAN_ART_GREAT.DESCRIPTION));
			this.CanStudyArtifact = base.Add(new SimpleSkillPerk("CanStudyArtifact", UI.ROLES_SCREEN.PERKS.CAN_STUDY_ARTIFACTS.DESCRIPTION));
			this.CanClothingAlteration = base.Add(new SimpleSkillPerk("CanClothingAlteration", UI.ROLES_SCREEN.PERKS.CAN_CLOTHING_ALTERATION.DESCRIPTION));
			this.IncreaseMachinerySmall = base.Add(new SkillAttributePerk("IncreaseMachinerySmall", Db.Get().Attributes.Machinery.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.MACHINE_TECHNICIAN.NAME));
			this.IncreaseMachineryMedium = base.Add(new SkillAttributePerk("IncreaseMachineryMedium", Db.Get().Attributes.Machinery.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME));
			this.IncreaseMachineryLarge = base.Add(new SkillAttributePerk("IncreaseMachineryLarge", Db.Get().Attributes.Machinery.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME));
			this.ConveyorBuild = base.Add(new SimpleSkillPerk("ConveyorBuild", UI.ROLES_SCREEN.PERKS.CONVEYOR_BUILD.DESCRIPTION));
			this.CanPowerTinker = base.Add(new SimpleSkillPerk("CanPowerTinker", UI.ROLES_SCREEN.PERKS.CAN_POWER_TINKER.DESCRIPTION));
			this.CanMakeMissiles = base.Add(new SimpleSkillPerk("CanMakeMissiles", UI.ROLES_SCREEN.PERKS.CAN_MAKE_MISSILES.DESCRIPTION));
			this.CanCraftElectronics = base.Add(new SimpleSkillPerk("CanCraftElectronics", UI.ROLES_SCREEN.PERKS.CAN_CRAFT_ELECTRONICS.DESCRIPTION, DlcManager.DLC3));
			this.CanElectricGrill = base.Add(new SimpleSkillPerk("CanElectricGrill", UI.ROLES_SCREEN.PERKS.CAN_ELECTRIC_GRILL.DESCRIPTION));
			this.IncreaseCookingSmall = base.Add(new SkillAttributePerk("IncreaseCookingSmall", Db.Get().Attributes.Cooking.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_COOK.NAME));
			this.IncreaseCookingMedium = base.Add(new SkillAttributePerk("IncreaseCookingMedium", Db.Get().Attributes.Cooking.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.COOK.NAME));
			this.CanSpiceGrinder = base.Add(new SimpleSkillPerk("CanSpiceGrinder ", UI.ROLES_SCREEN.PERKS.CAN_SPICE_GRINDER.DESCRIPTION));
			this.IncreaseCaringSmall = base.Add(new SkillAttributePerk("IncreaseCaringSmall", Db.Get().Attributes.Caring.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.JUNIOR_MEDIC.NAME));
			this.IncreaseCaringMedium = base.Add(new SkillAttributePerk("IncreaseCaringMedium", Db.Get().Attributes.Caring.Id, (float)ROLES.ATTRIBUTE_BONUS_SECOND, DUPLICANTS.ROLES.MEDIC.NAME));
			this.IncreaseCaringLarge = base.Add(new SkillAttributePerk("IncreaseCaringLarge", Db.Get().Attributes.Caring.Id, (float)ROLES.ATTRIBUTE_BONUS_THIRD, DUPLICANTS.ROLES.SENIOR_MEDIC.NAME));
			this.CanCompound = base.Add(new SimpleSkillPerk("CanCompound", UI.ROLES_SCREEN.PERKS.CAN_COMPOUND.DESCRIPTION));
			this.CanDoctor = base.Add(new SimpleSkillPerk("CanDoctor", UI.ROLES_SCREEN.PERKS.CAN_DOCTOR.DESCRIPTION));
			this.CanAdvancedMedicine = base.Add(new SimpleSkillPerk("CanAdvancedMedicine", UI.ROLES_SCREEN.PERKS.CAN_ADVANCED_MEDICINE.DESCRIPTION));
			this.ExosuitExpertise = base.Add(new SimpleSkillPerk("ExosuitExpertise", UI.ROLES_SCREEN.PERKS.EXOSUIT_EXPERTISE.DESCRIPTION));
			this.ExosuitDurability = base.Add(new SimpleSkillPerk("ExosuitDurability", UI.ROLES_SCREEN.PERKS.EXOSUIT_DURABILITY.DESCRIPTION));
			this.AllowAdvancedResearch = base.Add(new SimpleSkillPerk("AllowAdvancedResearch", UI.ROLES_SCREEN.PERKS.ADVANCED_RESEARCH.DESCRIPTION));
			this.AllowInterstellarResearch = base.Add(new SimpleSkillPerk("AllowInterStellarResearch", UI.ROLES_SCREEN.PERKS.INTERSTELLAR_RESEARCH.DESCRIPTION));
			this.AllowNuclearResearch = base.Add(new SimpleSkillPerk("AllowNuclearResearch", UI.ROLES_SCREEN.PERKS.NUCLEAR_RESEARCH.DESCRIPTION));
			this.AllowOrbitalResearch = base.Add(new SimpleSkillPerk("AllowOrbitalResearch", UI.ROLES_SCREEN.PERKS.ORBITAL_RESEARCH.DESCRIPTION));
			this.AllowGeyserTuning = base.Add(new SimpleSkillPerk("AllowGeyserTuning", UI.ROLES_SCREEN.PERKS.GEYSER_TUNING.DESCRIPTION));
			this.CanStudyWorldObjects = base.Add(new SimpleSkillPerk("CanStudyWorldObjects", UI.ROLES_SCREEN.PERKS.CAN_STUDY_WORLD_OBJECTS.DESCRIPTION));
			this.CanUseClusterTelescope = base.Add(new SimpleSkillPerk("CanUseClusterTelescope", UI.ROLES_SCREEN.PERKS.CAN_USE_CLUSTER_TELESCOPE.DESCRIPTION));
			this.CanDoPlumbing = base.Add(new SimpleSkillPerk("CanDoPlumbing", UI.ROLES_SCREEN.PERKS.CAN_DO_PLUMBING.DESCRIPTION));
			this.CanUseRockets = base.Add(new SimpleSkillPerk("CanUseRockets", UI.ROLES_SCREEN.PERKS.CAN_USE_ROCKETS.DESCRIPTION));
			this.FasterSpaceFlight = base.Add(new SkillAttributePerk("FasterSpaceFlight", Db.Get().Attributes.SpaceNavigation.Id, 0.1f, DUPLICANTS.ROLES.ASTRONAUT.NAME));
			this.CanTrainToBeAstronaut = base.Add(new SimpleSkillPerk("CanTrainToBeAstronaut", UI.ROLES_SCREEN.PERKS.CAN_DO_ASTRONAUT_TRAINING.DESCRIPTION));
			this.CanMissionControl = base.Add(new SimpleSkillPerk("CanMissionControl", UI.ROLES_SCREEN.PERKS.CAN_MISSION_CONTROL.DESCRIPTION));
			this.CanUseRocketControlStation = base.Add(new SimpleSkillPerk("CanUseRocketControlStation", UI.ROLES_SCREEN.PERKS.CAN_PILOT_ROCKET.DESCRIPTION));
			this.IncreaseRocketSpeedSmall = base.Add(new SkillAttributePerk("IncreaseRocketSpeedSmall", Db.Get().Attributes.SpaceNavigation.Id, (float)ROLES.ATTRIBUTE_BONUS_FIRST, DUPLICANTS.ROLES.ROCKETPILOT.NAME));
		}

		// Token: 0x040055CA RID: 21962
		public SkillPerk IncreaseDigSpeedSmall;

		// Token: 0x040055CB RID: 21963
		public SkillPerk IncreaseDigSpeedMedium;

		// Token: 0x040055CC RID: 21964
		public SkillPerk IncreaseDigSpeedLarge;

		// Token: 0x040055CD RID: 21965
		public SkillPerk CanDigVeryFirm;

		// Token: 0x040055CE RID: 21966
		public SkillPerk CanDigNearlyImpenetrable;

		// Token: 0x040055CF RID: 21967
		public SkillPerk CanDigSuperDuperHard;

		// Token: 0x040055D0 RID: 21968
		public SkillPerk CanDigRadioactiveMaterials;

		// Token: 0x040055D1 RID: 21969
		public SkillPerk CanDigUnobtanium;

		// Token: 0x040055D2 RID: 21970
		public SkillPerk IncreaseConstructionSmall;

		// Token: 0x040055D3 RID: 21971
		public SkillPerk IncreaseConstructionMedium;

		// Token: 0x040055D4 RID: 21972
		public SkillPerk IncreaseConstructionLarge;

		// Token: 0x040055D5 RID: 21973
		public SkillPerk IncreaseConstructionMechatronics;

		// Token: 0x040055D6 RID: 21974
		public SkillPerk CanDemolish;

		// Token: 0x040055D7 RID: 21975
		public SkillPerk IncreaseLearningSmall;

		// Token: 0x040055D8 RID: 21976
		public SkillPerk IncreaseLearningMedium;

		// Token: 0x040055D9 RID: 21977
		public SkillPerk IncreaseLearningLarge;

		// Token: 0x040055DA RID: 21978
		public SkillPerk IncreaseLearningLargeSpace;

		// Token: 0x040055DB RID: 21979
		public SkillPerk IncreaseBotanySmall;

		// Token: 0x040055DC RID: 21980
		public SkillPerk IncreaseBotanyMedium;

		// Token: 0x040055DD RID: 21981
		public SkillPerk IncreaseBotanyLarge;

		// Token: 0x040055DE RID: 21982
		public SkillPerk CanFarmTinker;

		// Token: 0x040055DF RID: 21983
		public SkillPerk CanIdentifyMutantSeeds;

		// Token: 0x040055E0 RID: 21984
		public SkillPerk CanWrangleCreatures;

		// Token: 0x040055E1 RID: 21985
		public SkillPerk CanUseRanchStation;

		// Token: 0x040055E2 RID: 21986
		public SkillPerk CanUseMilkingStation;

		// Token: 0x040055E3 RID: 21987
		public SkillPerk IncreaseRanchingSmall;

		// Token: 0x040055E4 RID: 21988
		public SkillPerk IncreaseRanchingMedium;

		// Token: 0x040055E5 RID: 21989
		public SkillPerk IncreaseAthleticsSmall;

		// Token: 0x040055E6 RID: 21990
		public SkillPerk IncreaseAthleticsMedium;

		// Token: 0x040055E7 RID: 21991
		public SkillPerk IncreaseAthleticsLarge;

		// Token: 0x040055E8 RID: 21992
		public SkillPerk IncreaseStrengthSmall;

		// Token: 0x040055E9 RID: 21993
		public SkillPerk IncreaseStrengthMedium;

		// Token: 0x040055EA RID: 21994
		public SkillPerk IncreaseStrengthGofer;

		// Token: 0x040055EB RID: 21995
		public SkillPerk IncreaseStrengthCourier;

		// Token: 0x040055EC RID: 21996
		public SkillPerk IncreaseStrengthGroundskeeper;

		// Token: 0x040055ED RID: 21997
		public SkillPerk IncreaseStrengthPlumber;

		// Token: 0x040055EE RID: 21998
		public SkillPerk IncreaseCarryAmountSmall;

		// Token: 0x040055EF RID: 21999
		public SkillPerk IncreaseCarryAmountMedium;

		// Token: 0x040055F0 RID: 22000
		public SkillPerk IncreaseArtSmall;

		// Token: 0x040055F1 RID: 22001
		public SkillPerk IncreaseArtMedium;

		// Token: 0x040055F2 RID: 22002
		public SkillPerk IncreaseArtLarge;

		// Token: 0x040055F3 RID: 22003
		public SkillPerk CanArt;

		// Token: 0x040055F4 RID: 22004
		public SkillPerk CanArtUgly;

		// Token: 0x040055F5 RID: 22005
		public SkillPerk CanArtOkay;

		// Token: 0x040055F6 RID: 22006
		public SkillPerk CanArtGreat;

		// Token: 0x040055F7 RID: 22007
		public SkillPerk CanStudyArtifact;

		// Token: 0x040055F8 RID: 22008
		public SkillPerk CanClothingAlteration;

		// Token: 0x040055F9 RID: 22009
		public SkillPerk IncreaseMachinerySmall;

		// Token: 0x040055FA RID: 22010
		public SkillPerk IncreaseMachineryMedium;

		// Token: 0x040055FB RID: 22011
		public SkillPerk IncreaseMachineryLarge;

		// Token: 0x040055FC RID: 22012
		public SkillPerk ConveyorBuild;

		// Token: 0x040055FD RID: 22013
		public SkillPerk CanMakeMissiles;

		// Token: 0x040055FE RID: 22014
		public SkillPerk CanPowerTinker;

		// Token: 0x040055FF RID: 22015
		public SkillPerk CanCraftElectronics;

		// Token: 0x04005600 RID: 22016
		public SkillPerk CanElectricGrill;

		// Token: 0x04005601 RID: 22017
		public SkillPerk IncreaseCookingSmall;

		// Token: 0x04005602 RID: 22018
		public SkillPerk IncreaseCookingMedium;

		// Token: 0x04005603 RID: 22019
		public SkillPerk CanSpiceGrinder;

		// Token: 0x04005604 RID: 22020
		public SkillPerk IncreaseCaringSmall;

		// Token: 0x04005605 RID: 22021
		public SkillPerk IncreaseCaringMedium;

		// Token: 0x04005606 RID: 22022
		public SkillPerk IncreaseCaringLarge;

		// Token: 0x04005607 RID: 22023
		public SkillPerk CanCompound;

		// Token: 0x04005608 RID: 22024
		public SkillPerk CanDoctor;

		// Token: 0x04005609 RID: 22025
		public SkillPerk CanAdvancedMedicine;

		// Token: 0x0400560A RID: 22026
		public SkillPerk ExosuitExpertise;

		// Token: 0x0400560B RID: 22027
		public SkillPerk ExosuitDurability;

		// Token: 0x0400560C RID: 22028
		public SkillPerk AllowAdvancedResearch;

		// Token: 0x0400560D RID: 22029
		public SkillPerk AllowInterstellarResearch;

		// Token: 0x0400560E RID: 22030
		public SkillPerk AllowNuclearResearch;

		// Token: 0x0400560F RID: 22031
		public SkillPerk AllowOrbitalResearch;

		// Token: 0x04005610 RID: 22032
		public SkillPerk AllowGeyserTuning;

		// Token: 0x04005611 RID: 22033
		public SkillPerk CanStudyWorldObjects;

		// Token: 0x04005612 RID: 22034
		public SkillPerk CanUseClusterTelescope;

		// Token: 0x04005613 RID: 22035
		public SkillPerk IncreaseRocketSpeedSmall;

		// Token: 0x04005614 RID: 22036
		public SkillPerk CanMissionControl;

		// Token: 0x04005615 RID: 22037
		public SkillPerk CanDoPlumbing;

		// Token: 0x04005616 RID: 22038
		public SkillPerk CanUseRockets;

		// Token: 0x04005617 RID: 22039
		public SkillPerk FasterSpaceFlight;

		// Token: 0x04005618 RID: 22040
		public SkillPerk CanTrainToBeAstronaut;

		// Token: 0x04005619 RID: 22041
		public SkillPerk CanUseRocketControlStation;
	}
}
