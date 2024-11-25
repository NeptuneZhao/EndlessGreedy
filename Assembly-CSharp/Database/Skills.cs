using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000ED3 RID: 3795
	public class Skills : ResourceSet<Skill>
	{
		// Token: 0x06007641 RID: 30273 RVA: 0x002E6018 File Offset: 0x002E4218
		public Skills(ResourceSet parent) : base("Skills", parent)
		{
			this.Mining1 = this.AddSkill(new Skill("Mining1", DUPLICANTS.ROLES.JUNIOR_MINER.NAME, DUPLICANTS.ROLES.JUNIOR_MINER.DESCRIPTION, "", 0, "hat_role_mining1", "skillbadge_role_mining1", Db.Get().SkillGroups.Mining.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseDigSpeedSmall,
				Db.Get().SkillPerks.CanDigVeryFirm
			}, null));
			this.Mining2 = this.AddSkill(new Skill("Mining2", DUPLICANTS.ROLES.MINER.NAME, DUPLICANTS.ROLES.MINER.DESCRIPTION, "", 1, "hat_role_mining2", "skillbadge_role_mining2", Db.Get().SkillGroups.Mining.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseDigSpeedMedium,
				Db.Get().SkillPerks.CanDigNearlyImpenetrable
			}, new List<string>
			{
				this.Mining1.Id
			}));
			this.Mining3 = this.AddSkill(new Skill("Mining3", DUPLICANTS.ROLES.SENIOR_MINER.NAME, DUPLICANTS.ROLES.SENIOR_MINER.DESCRIPTION, "", 2, "hat_role_mining3", "skillbadge_role_mining3", Db.Get().SkillGroups.Mining.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseDigSpeedLarge,
				Db.Get().SkillPerks.CanDigSuperDuperHard
			}, new List<string>
			{
				this.Mining2.Id
			}));
			this.Mining4 = this.AddSkill(new Skill("Mining4", DUPLICANTS.ROLES.MASTER_MINER.NAME, DUPLICANTS.ROLES.MASTER_MINER.DESCRIPTION, "EXPANSION1_ID", 3, "hat_role_mining4", "skillbadge_role_mining4", Db.Get().SkillGroups.Mining.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanDigRadioactiveMaterials
			}, new List<string>
			{
				this.Mining3.Id
			}));
			this.Building1 = this.AddSkill(new Skill("Building1", DUPLICANTS.ROLES.JUNIOR_BUILDER.NAME, DUPLICANTS.ROLES.JUNIOR_BUILDER.DESCRIPTION, "", 0, "hat_role_building1", "skillbadge_role_building1", Db.Get().SkillGroups.Building.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseConstructionSmall
			}, null));
			this.Building2 = this.AddSkill(new Skill("Building2", DUPLICANTS.ROLES.BUILDER.NAME, DUPLICANTS.ROLES.BUILDER.DESCRIPTION, "", 1, "hat_role_building2", "skillbadge_role_building2", Db.Get().SkillGroups.Building.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseConstructionMedium
			}, new List<string>
			{
				this.Building1.Id
			}));
			this.Building3 = this.AddSkill(new Skill("Building3", DUPLICANTS.ROLES.SENIOR_BUILDER.NAME, DUPLICANTS.ROLES.SENIOR_BUILDER.DESCRIPTION, "", 2, "hat_role_building3", "skillbadge_role_building3", Db.Get().SkillGroups.Building.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseConstructionLarge,
				Db.Get().SkillPerks.CanDemolish
			}, new List<string>
			{
				this.Building2.Id
			}));
			this.Farming1 = this.AddSkill(new Skill("Farming1", DUPLICANTS.ROLES.JUNIOR_FARMER.NAME, DUPLICANTS.ROLES.JUNIOR_FARMER.DESCRIPTION, "", 0, "hat_role_farming1", "skillbadge_role_farming1", Db.Get().SkillGroups.Farming.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseBotanySmall
			}, null));
			this.Farming2 = this.AddSkill(new Skill("Farming2", DUPLICANTS.ROLES.FARMER.NAME, DUPLICANTS.ROLES.FARMER.DESCRIPTION, "", 1, "hat_role_farming2", "skillbadge_role_farming2", Db.Get().SkillGroups.Farming.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseBotanyMedium,
				Db.Get().SkillPerks.CanFarmTinker
			}, new List<string>
			{
				this.Farming1.Id
			}));
			this.Farming3 = this.AddSkill(new Skill("Farming3", DUPLICANTS.ROLES.SENIOR_FARMER.NAME, DUPLICANTS.ROLES.SENIOR_FARMER.DESCRIPTION, "", 2, "hat_role_farming3", "skillbadge_role_farming3", Db.Get().SkillGroups.Farming.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseBotanyLarge
			}, new List<string>
			{
				this.Farming2.Id
			}));
			if (DlcManager.FeaturePlantMutationsEnabled())
			{
				this.Farming3.perks.Add(Db.Get().SkillPerks.CanIdentifyMutantSeeds);
			}
			this.Ranching1 = this.AddSkill(new Skill("Ranching1", DUPLICANTS.ROLES.RANCHER.NAME, DUPLICANTS.ROLES.RANCHER.DESCRIPTION, "", 1, "hat_role_rancher1", "skillbadge_role_rancher1", Db.Get().SkillGroups.Ranching.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanWrangleCreatures,
				Db.Get().SkillPerks.CanUseRanchStation,
				Db.Get().SkillPerks.IncreaseRanchingSmall
			}, new List<string>
			{
				this.Farming1.Id
			}));
			this.Ranching2 = this.AddSkill(new Skill("Ranching2", DUPLICANTS.ROLES.SENIOR_RANCHER.NAME, DUPLICANTS.ROLES.SENIOR_RANCHER.DESCRIPTION, "", 2, "hat_role_rancher2", "skillbadge_role_rancher2", Db.Get().SkillGroups.Ranching.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanUseMilkingStation,
				Db.Get().SkillPerks.IncreaseRanchingMedium
			}, new List<string>
			{
				this.Ranching1.Id
			}));
			this.Researching1 = this.AddSkill(new Skill("Researching1", DUPLICANTS.ROLES.JUNIOR_RESEARCHER.NAME, DUPLICANTS.ROLES.JUNIOR_RESEARCHER.DESCRIPTION, "", 0, "hat_role_research1", "skillbadge_role_research1", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningSmall,
				Db.Get().SkillPerks.AllowAdvancedResearch
			}, null));
			this.Researching2 = this.AddSkill(new Skill("Researching2", DUPLICANTS.ROLES.RESEARCHER.NAME, DUPLICANTS.ROLES.RESEARCHER.DESCRIPTION, "", 1, "hat_role_research2", "skillbadge_role_research2", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningMedium,
				Db.Get().SkillPerks.CanStudyWorldObjects,
				Db.Get().SkillPerks.AllowGeyserTuning
			}, new List<string>
			{
				this.Researching1.Id
			}));
			this.AtomicResearch = this.AddSkill(new Skill("AtomicResearch", DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.NAME, DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.DESCRIPTION, "EXPANSION1_ID", 2, "hat_role_research5", "skillbadge_role_research3", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningLarge,
				Db.Get().SkillPerks.AllowNuclearResearch
			}, new List<string>
			{
				this.Researching2.Id
			}));
			this.Researching4 = this.AddSkill(new Skill("Researching4", DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.NAME, DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.DESCRIPTION, "EXPANSION1_ID", 2, "hat_role_research4", "skillbadge_role_research3", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningLarge,
				Db.Get().SkillPerks.AllowNuclearResearch
			}, new List<string>
			{
				this.Researching2.Id
			}));
			this.Researching4.deprecated = true;
			this.Researching3 = this.AddSkill(new Skill("Researching3", DUPLICANTS.ROLES.SENIOR_RESEARCHER.NAME, DUPLICANTS.ROLES.SENIOR_RESEARCHER.DESCRIPTION, "", 2, "hat_role_research3", "skillbadge_role_research3", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningLarge,
				Db.Get().SkillPerks.AllowInterstellarResearch,
				Db.Get().SkillPerks.CanMissionControl
			}, new List<string>
			{
				this.Researching2.Id
			}));
			this.Researching3.deprecated = DlcManager.IsExpansion1Active();
			this.Astronomy = this.AddSkill(new Skill("Astronomy", DUPLICANTS.ROLES.SENIOR_RESEARCHER.NAME, DUPLICANTS.ROLES.SENIOR_RESEARCHER.DESCRIPTION, "EXPANSION1_ID", 1, "hat_role_research3", "skillbadge_role_research2", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanUseClusterTelescope,
				Db.Get().SkillPerks.CanMissionControl
			}, new List<string>
			{
				this.Researching1.Id
			}));
			this.SpaceResearch = this.AddSkill(new Skill("SpaceResearch", DUPLICANTS.ROLES.SPACE_RESEARCHER.NAME, DUPLICANTS.ROLES.SPACE_RESEARCHER.DESCRIPTION, "EXPANSION1_ID", 2, "hat_role_research4", "skillbadge_role_research3", Db.Get().SkillGroups.Research.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseLearningLargeSpace,
				Db.Get().SkillPerks.AllowOrbitalResearch
			}, new List<string>
			{
				this.Astronomy.Id
			}));
			if (DlcManager.IsExpansion1Active())
			{
				this.RocketPiloting1 = this.AddSkill(new Skill("RocketPiloting1", DUPLICANTS.ROLES.ROCKETPILOT.NAME, DUPLICANTS.ROLES.ROCKETPILOT.DESCRIPTION, "EXPANSION1_ID", 0, "hat_role_astronaut1", "skillbadge_role_rocketry1", Db.Get().SkillGroups.Rocketry.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.CanUseRocketControlStation
				}, new List<string>()));
				this.RocketPiloting2 = this.AddSkill(new Skill("RocketPiloting2", DUPLICANTS.ROLES.SENIOR_ROCKETPILOT.NAME, DUPLICANTS.ROLES.SENIOR_ROCKETPILOT.DESCRIPTION, "EXPANSION1_ID", 2, "hat_role_astronaut2", "skillbadge_role_rocketry3", Db.Get().SkillGroups.Rocketry.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.IncreaseRocketSpeedSmall
				}, new List<string>
				{
					this.RocketPiloting1.Id,
					this.Astronomy.Id
				}));
			}
			this.Cooking1 = this.AddSkill(new Skill("Cooking1", DUPLICANTS.ROLES.JUNIOR_COOK.NAME, DUPLICANTS.ROLES.JUNIOR_COOK.DESCRIPTION, "", 0, "hat_role_cooking1", "skillbadge_role_cooking1", Db.Get().SkillGroups.Cooking.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseCookingSmall,
				Db.Get().SkillPerks.CanElectricGrill
			}, null));
			this.Cooking2 = this.AddSkill(new Skill("Cooking2", DUPLICANTS.ROLES.COOK.NAME, DUPLICANTS.ROLES.COOK.DESCRIPTION, "", 1, "hat_role_cooking2", "skillbadge_role_cooking2", Db.Get().SkillGroups.Cooking.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseCookingMedium,
				Db.Get().SkillPerks.CanSpiceGrinder
			}, new List<string>
			{
				this.Cooking1.Id
			}));
			this.Arting1 = this.AddSkill(new Skill("Arting1", DUPLICANTS.ROLES.JUNIOR_ARTIST.NAME, DUPLICANTS.ROLES.JUNIOR_ARTIST.DESCRIPTION, "", 0, "hat_role_art1", "skillbadge_role_art1", Db.Get().SkillGroups.Art.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanArt,
				Db.Get().SkillPerks.CanArtUgly,
				Db.Get().SkillPerks.IncreaseArtSmall
			}, null));
			this.Arting2 = this.AddSkill(new Skill("Arting2", DUPLICANTS.ROLES.ARTIST.NAME, DUPLICANTS.ROLES.ARTIST.DESCRIPTION, "", 1, "hat_role_art2", "skillbadge_role_art2", Db.Get().SkillGroups.Art.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanArtOkay,
				Db.Get().SkillPerks.IncreaseArtMedium,
				Db.Get().SkillPerks.CanClothingAlteration
			}, new List<string>
			{
				this.Arting1.Id
			}));
			if (DlcManager.FeatureClusterSpaceEnabled())
			{
				this.Arting2.perks.Add(Db.Get().SkillPerks.CanStudyArtifact);
			}
			this.Arting3 = this.AddSkill(new Skill("Arting3", DUPLICANTS.ROLES.MASTER_ARTIST.NAME, DUPLICANTS.ROLES.MASTER_ARTIST.DESCRIPTION, "", 2, "hat_role_art3", "skillbadge_role_art3", Db.Get().SkillGroups.Art.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanArtGreat,
				Db.Get().SkillPerks.IncreaseArtLarge
			}, new List<string>
			{
				this.Arting2.Id
			}));
			this.Hauling1 = this.AddSkill(new Skill("Hauling1", DUPLICANTS.ROLES.HAULER.NAME, DUPLICANTS.ROLES.HAULER.DESCRIPTION, "", 0, "hat_role_hauling1", "skillbadge_role_hauling1", Db.Get().SkillGroups.Hauling.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseStrengthGofer,
				Db.Get().SkillPerks.IncreaseCarryAmountSmall
			}, null));
			this.Hauling2 = this.AddSkill(new Skill("Hauling2", DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME, DUPLICANTS.ROLES.MATERIALS_MANAGER.DESCRIPTION, "", 1, "hat_role_hauling2", "skillbadge_role_hauling2", Db.Get().SkillGroups.Hauling.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseStrengthCourier,
				Db.Get().SkillPerks.IncreaseCarryAmountMedium
			}, new List<string>
			{
				this.Hauling1.Id
			}));
			if (DlcManager.IsExpansion1Active())
			{
				this.ThermalSuits = this.AddSkill(new Skill("ThermalSuits", DUPLICANTS.ROLES.SUIT_DURABILITY.NAME, DUPLICANTS.ROLES.SUIT_DURABILITY.DESCRIPTION, "EXPANSION1_ID", 1, "hat_role_suits1", "skillbadge_role_suits2", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.IncreaseAthleticsLarge,
					Db.Get().SkillPerks.ExosuitDurability
				}, new List<string>
				{
					this.Hauling1.Id,
					this.RocketPiloting1.Id
				}));
			}
			else
			{
				this.ThermalSuits = this.AddSkill(new Skill("ThermalSuits", DUPLICANTS.ROLES.SUIT_DURABILITY.NAME, DUPLICANTS.ROLES.SUIT_DURABILITY.DESCRIPTION, "", 1, "hat_role_suits1", "skillbadge_role_suits2", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.IncreaseAthleticsLarge,
					Db.Get().SkillPerks.ExosuitDurability
				}, new List<string>
				{
					this.Hauling1.Id
				}));
			}
			this.Suits1 = this.AddSkill(new Skill("Suits1", DUPLICANTS.ROLES.SUIT_EXPERT.NAME, DUPLICANTS.ROLES.SUIT_EXPERT.DESCRIPTION, "", 2, "hat_role_suits2", "skillbadge_role_suits3", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.ExosuitExpertise,
				Db.Get().SkillPerks.IncreaseAthleticsMedium
			}, new List<string>
			{
				this.ThermalSuits.Id
			}));
			this.Technicals1 = this.AddSkill(new Skill("Technicals1", DUPLICANTS.ROLES.MACHINE_TECHNICIAN.NAME, DUPLICANTS.ROLES.MACHINE_TECHNICIAN.DESCRIPTION, "", 0, "hat_role_technicals1", "skillbadge_role_technicals1", Db.Get().SkillGroups.Technicals.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseMachinerySmall
			}, null));
			this.Technicals2 = this.AddSkill(new Skill("Technicals2", DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME, DUPLICANTS.ROLES.POWER_TECHNICIAN.DESCRIPTION, "", 1, "hat_role_technicals2", "skillbadge_role_technicals2", Db.Get().SkillGroups.Technicals.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseMachineryMedium,
				Db.Get().SkillPerks.CanPowerTinker,
				Db.Get().SkillPerks.CanCraftElectronics
			}, new List<string>
			{
				this.Technicals1.Id
			}));
			this.Engineering1 = this.AddSkill(new Skill("Engineering1", DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME, DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.DESCRIPTION, "", 2, "hat_role_engineering1", "skillbadge_role_engineering1", Db.Get().SkillGroups.Technicals.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseMachineryLarge,
				Db.Get().SkillPerks.IncreaseConstructionMechatronics,
				Db.Get().SkillPerks.ConveyorBuild
			}, new List<string>
			{
				this.Hauling2.Id,
				this.Technicals2.Id
			}));
			this.Basekeeping1 = this.AddSkill(new Skill("Basekeeping1", DUPLICANTS.ROLES.HANDYMAN.NAME, DUPLICANTS.ROLES.HANDYMAN.DESCRIPTION, "", 0, "hat_role_basekeeping1", "skillbadge_role_basekeeping1", Db.Get().SkillGroups.Basekeeping.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseStrengthGroundskeeper
			}, null));
			this.Basekeeping2 = this.AddSkill(new Skill("Basekeeping2", DUPLICANTS.ROLES.PLUMBER.NAME, DUPLICANTS.ROLES.PLUMBER.DESCRIPTION, "", 1, "hat_role_basekeeping2", "skillbadge_role_basekeeping2", Db.Get().SkillGroups.Basekeeping.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.IncreaseStrengthPlumber,
				Db.Get().SkillPerks.CanDoPlumbing
			}, new List<string>
			{
				this.Basekeeping1.Id
			}));
			this.Pyrotechnics = this.AddSkill(new Skill("Pyrotechnics", DUPLICANTS.ROLES.PYROTECHNIC.NAME, DUPLICANTS.ROLES.PYROTECHNIC.DESCRIPTION, "", 2, "hat_role_pyrotechnics", "skillbadge_role_technicals2", Db.Get().SkillGroups.Basekeeping.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanMakeMissiles
			}, new List<string>
			{
				this.Basekeeping2.Id
			}));
			if (DlcManager.IsExpansion1Active())
			{
				this.Astronauting1 = this.AddSkill(new Skill("Astronauting1", DUPLICANTS.ROLES.USELESSSKILL.NAME, DUPLICANTS.ROLES.USELESSSKILL.DESCRIPTION, "EXPANSION1_ID", 3, "hat_role_astronaut1", "skillbadge_role_astronaut1", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.IncreaseAthleticsMedium
				}, new List<string>
				{
					this.Researching3.Id,
					this.Suits1.Id
				}));
				this.Astronauting1.deprecated = true;
				this.Astronauting2 = this.AddSkill(new Skill("Astronauting2", DUPLICANTS.ROLES.USELESSSKILL.NAME, DUPLICANTS.ROLES.USELESSSKILL.DESCRIPTION, "EXPANSION1_ID", 4, "hat_role_astronaut2", "skillbadge_role_astronaut2", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.IncreaseAthleticsMedium
				}, new List<string>
				{
					this.Astronauting1.Id
				}));
				this.Astronauting2.deprecated = true;
			}
			else
			{
				this.Astronauting1 = this.AddSkill(new Skill("Astronauting1", DUPLICANTS.ROLES.ASTRONAUTTRAINEE.NAME, DUPLICANTS.ROLES.ASTRONAUTTRAINEE.DESCRIPTION, "", 3, "hat_role_astronaut1", "skillbadge_role_astronaut1", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.CanUseRockets
				}, new List<string>
				{
					this.Researching3.Id,
					this.Suits1.Id
				}));
				this.Astronauting2 = this.AddSkill(new Skill("Astronauting2", DUPLICANTS.ROLES.ASTRONAUT.NAME, DUPLICANTS.ROLES.ASTRONAUT.DESCRIPTION, "", 4, "hat_role_astronaut2", "skillbadge_role_astronaut2", Db.Get().SkillGroups.Suits.Id, new List<SkillPerk>
				{
					Db.Get().SkillPerks.FasterSpaceFlight
				}, new List<string>
				{
					this.Astronauting1.Id
				}));
			}
			this.Medicine1 = this.AddSkill(new Skill("Medicine1", DUPLICANTS.ROLES.JUNIOR_MEDIC.NAME, DUPLICANTS.ROLES.JUNIOR_MEDIC.DESCRIPTION, "", 0, "hat_role_medicalaid1", "skillbadge_role_medicalaid1", Db.Get().SkillGroups.MedicalAid.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanCompound,
				Db.Get().SkillPerks.IncreaseCaringSmall
			}, null));
			this.Medicine2 = this.AddSkill(new Skill("Medicine2", DUPLICANTS.ROLES.MEDIC.NAME, DUPLICANTS.ROLES.MEDIC.DESCRIPTION, "", 1, "hat_role_medicalaid2", "skillbadge_role_medicalaid2", Db.Get().SkillGroups.MedicalAid.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanDoctor,
				Db.Get().SkillPerks.IncreaseCaringMedium
			}, new List<string>
			{
				this.Medicine1.Id
			}));
			this.Medicine3 = this.AddSkill(new Skill("Medicine3", DUPLICANTS.ROLES.SENIOR_MEDIC.NAME, DUPLICANTS.ROLES.SENIOR_MEDIC.DESCRIPTION, "", 2, "hat_role_medicalaid3", "skillbadge_role_medicalaid3", Db.Get().SkillGroups.MedicalAid.Id, new List<SkillPerk>
			{
				Db.Get().SkillPerks.CanAdvancedMedicine,
				Db.Get().SkillPerks.IncreaseCaringLarge
			}, new List<string>
			{
				this.Medicine2.Id
			}));
		}

		// Token: 0x06007642 RID: 30274 RVA: 0x002E78FC File Offset: 0x002E5AFC
		private Skill AddSkill(Skill skill)
		{
			if (DlcManager.IsContentSubscribed(skill.dlcId))
			{
				return base.Add(skill);
			}
			return skill;
		}

		// Token: 0x06007643 RID: 30275 RVA: 0x002E7914 File Offset: 0x002E5B14
		public List<Skill> GetSkillsWithPerk(string perk)
		{
			List<Skill> list = new List<Skill>();
			foreach (Skill skill in this.resources)
			{
				if (skill.GivesPerk(perk))
				{
					list.Add(skill);
				}
			}
			return list;
		}

		// Token: 0x06007644 RID: 30276 RVA: 0x002E797C File Offset: 0x002E5B7C
		public List<Skill> GetSkillsWithPerk(SkillPerk perk)
		{
			List<Skill> list = new List<Skill>();
			foreach (Skill skill in this.resources)
			{
				if (skill.GivesPerk(perk))
				{
					list.Add(skill);
				}
			}
			return list;
		}

		// Token: 0x06007645 RID: 30277 RVA: 0x002E79E0 File Offset: 0x002E5BE0
		public List<Skill> GetAllPriorSkills(Skill skill)
		{
			List<Skill> list = new List<Skill>();
			foreach (string id in skill.priorSkills)
			{
				Skill skill2 = base.Get(id);
				list.Add(skill2);
				list.AddRange(this.GetAllPriorSkills(skill2));
			}
			return list;
		}

		// Token: 0x06007646 RID: 30278 RVA: 0x002E7A50 File Offset: 0x002E5C50
		public List<Skill> GetTerminalSkills()
		{
			List<Skill> list = new List<Skill>();
			foreach (Skill skill in this.resources)
			{
				bool flag = true;
				using (List<Skill>.Enumerator enumerator2 = this.resources.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.priorSkills.Contains(skill.Id))
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					list.Add(skill);
				}
			}
			return list;
		}

		// Token: 0x04005635 RID: 22069
		public Skill Mining1;

		// Token: 0x04005636 RID: 22070
		public Skill Mining2;

		// Token: 0x04005637 RID: 22071
		public Skill Mining3;

		// Token: 0x04005638 RID: 22072
		public Skill Mining4;

		// Token: 0x04005639 RID: 22073
		public Skill Building1;

		// Token: 0x0400563A RID: 22074
		public Skill Building2;

		// Token: 0x0400563B RID: 22075
		public Skill Building3;

		// Token: 0x0400563C RID: 22076
		public Skill Farming1;

		// Token: 0x0400563D RID: 22077
		public Skill Farming2;

		// Token: 0x0400563E RID: 22078
		public Skill Farming3;

		// Token: 0x0400563F RID: 22079
		public Skill Ranching1;

		// Token: 0x04005640 RID: 22080
		public Skill Ranching2;

		// Token: 0x04005641 RID: 22081
		public Skill Researching1;

		// Token: 0x04005642 RID: 22082
		public Skill Researching2;

		// Token: 0x04005643 RID: 22083
		public Skill Researching3;

		// Token: 0x04005644 RID: 22084
		public Skill Researching4;

		// Token: 0x04005645 RID: 22085
		public Skill AtomicResearch;

		// Token: 0x04005646 RID: 22086
		public Skill SpaceResearch;

		// Token: 0x04005647 RID: 22087
		public Skill Astronomy;

		// Token: 0x04005648 RID: 22088
		public Skill RocketPiloting1;

		// Token: 0x04005649 RID: 22089
		public Skill RocketPiloting2;

		// Token: 0x0400564A RID: 22090
		public Skill Cooking1;

		// Token: 0x0400564B RID: 22091
		public Skill Cooking2;

		// Token: 0x0400564C RID: 22092
		public Skill Arting1;

		// Token: 0x0400564D RID: 22093
		public Skill Arting2;

		// Token: 0x0400564E RID: 22094
		public Skill Arting3;

		// Token: 0x0400564F RID: 22095
		public Skill Hauling1;

		// Token: 0x04005650 RID: 22096
		public Skill Hauling2;

		// Token: 0x04005651 RID: 22097
		public Skill ThermalSuits;

		// Token: 0x04005652 RID: 22098
		public Skill Suits1;

		// Token: 0x04005653 RID: 22099
		public Skill Technicals1;

		// Token: 0x04005654 RID: 22100
		public Skill Technicals2;

		// Token: 0x04005655 RID: 22101
		public Skill Engineering1;

		// Token: 0x04005656 RID: 22102
		public Skill Basekeeping1;

		// Token: 0x04005657 RID: 22103
		public Skill Basekeeping2;

		// Token: 0x04005658 RID: 22104
		public Skill Pyrotechnics;

		// Token: 0x04005659 RID: 22105
		public Skill Astronauting1;

		// Token: 0x0400565A RID: 22106
		public Skill Astronauting2;

		// Token: 0x0400565B RID: 22107
		public Skill Medicine1;

		// Token: 0x0400565C RID: 22108
		public Skill Medicine2;

		// Token: 0x0400565D RID: 22109
		public Skill Medicine3;
	}
}
