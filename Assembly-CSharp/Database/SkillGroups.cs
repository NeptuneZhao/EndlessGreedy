using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;

namespace Database
{
	// Token: 0x02000ED1 RID: 3793
	public class SkillGroups : ResourceSet<SkillGroup>
	{
		// Token: 0x0600763C RID: 30268 RVA: 0x002E57D0 File Offset: 0x002E39D0
		public SkillGroups(ResourceSet parent) : base("SkillGroups", parent)
		{
			this.Mining = base.Add(new SkillGroup("Mining", Db.Get().ChoreGroups.Dig.Id, DUPLICANTS.CHOREGROUPS.DIG.NAME, "icon_errand_dig", "icon_archetype_dig"));
			this.Mining.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.Dig.attribute
			};
			this.Mining.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.Dig.Id
			};
			this.Building = base.Add(new SkillGroup("Building", Db.Get().ChoreGroups.Build.Id, DUPLICANTS.CHOREGROUPS.BUILD.NAME, "status_item_pending_repair", "icon_archetype_build"));
			this.Building.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.Build.attribute
			};
			this.Building.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.Build.Id
			};
			this.Farming = base.Add(new SkillGroup("Farming", Db.Get().ChoreGroups.Farming.Id, DUPLICANTS.CHOREGROUPS.FARMING.NAME, "icon_errand_farm", "icon_archetype_farm"));
			this.Farming.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.Farming.attribute
			};
			this.Farming.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.Farming.Id
			};
			this.Ranching = base.Add(new SkillGroup("Ranching", Db.Get().ChoreGroups.Ranching.Id, DUPLICANTS.CHOREGROUPS.RANCHING.NAME, "icon_errand_ranch", "icon_archetype_ranch"));
			this.Ranching.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.Ranching.attribute
			};
			this.Ranching.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.Ranching.Id
			};
			this.Cooking = base.Add(new SkillGroup("Cooking", Db.Get().ChoreGroups.Cook.Id, DUPLICANTS.CHOREGROUPS.COOK.NAME, "icon_errand_cook", "icon_archetype_cook"));
			this.Cooking.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.Cook.attribute
			};
			this.Cooking.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.Cook.Id
			};
			this.Art = base.Add(new SkillGroup("Art", Db.Get().ChoreGroups.Art.Id, DUPLICANTS.CHOREGROUPS.ART.NAME, "icon_errand_art", "icon_archetype_art"));
			this.Art.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.Art.attribute
			};
			this.Art.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.Art.Id
			};
			this.Research = base.Add(new SkillGroup("Research", Db.Get().ChoreGroups.Research.Id, DUPLICANTS.CHOREGROUPS.RESEARCH.NAME, "icon_errand_research", "icon_archetype_research"));
			this.Research.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.Research.attribute
			};
			this.Research.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.Research.Id
			};
			if (DlcManager.FeatureClusterSpaceEnabled())
			{
				this.Rocketry = base.Add(new SkillGroup("Rocketry", Db.Get().ChoreGroups.Rocketry.Id, DUPLICANTS.CHOREGROUPS.ROCKETRY.NAME, "icon_errand_rocketry", "icon_archetype_rocketry"));
				this.Rocketry.relevantAttributes = new List<Klei.AI.Attribute>
				{
					Db.Get().ChoreGroups.Rocketry.attribute
				};
				this.Rocketry.requiredChoreGroups = new List<string>
				{
					Db.Get().ChoreGroups.Rocketry.Id
				};
			}
			this.Suits = base.Add(new SkillGroup("Suits", "", DUPLICANTS.ROLES.GROUPS.SUITS, "suit_overlay_icon", "icon_archetype_astronaut"));
			this.Suits.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().Attributes.Athletics
			};
			this.Suits.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.Hauling.Id
			};
			this.Hauling = base.Add(new SkillGroup("Hauling", Db.Get().ChoreGroups.Hauling.Id, DUPLICANTS.CHOREGROUPS.HAULING.NAME, "icon_errand_supply", "icon_archetype_storage"));
			this.Hauling.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.Hauling.attribute
			};
			this.Hauling.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.Hauling.Id
			};
			this.Technicals = base.Add(new SkillGroup("Technicals", Db.Get().ChoreGroups.MachineOperating.Id, DUPLICANTS.CHOREGROUPS.MACHINEOPERATING.NAME, "icon_errand_operate", "icon_archetype_operate"));
			this.Technicals.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.MachineOperating.attribute
			};
			this.Technicals.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.MachineOperating.Id
			};
			this.MedicalAid = base.Add(new SkillGroup("MedicalAid", Db.Get().ChoreGroups.MedicalAid.Id, DUPLICANTS.CHOREGROUPS.MEDICALAID.NAME, "icon_errand_care", "icon_archetype_care"));
			this.MedicalAid.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.MedicalAid.attribute
			};
			this.Basekeeping = base.Add(new SkillGroup("Basekeeping", Db.Get().ChoreGroups.Basekeeping.Id, DUPLICANTS.CHOREGROUPS.BASEKEEPING.NAME, "icon_errand_tidy", "icon_archetype_tidy"));
			this.Basekeeping.relevantAttributes = new List<Klei.AI.Attribute>
			{
				Db.Get().ChoreGroups.Basekeeping.attribute
			};
			this.Basekeeping.requiredChoreGroups = new List<string>
			{
				Db.Get().ChoreGroups.Basekeeping.Id
			};
		}

		// Token: 0x0400561F RID: 22047
		public SkillGroup Mining;

		// Token: 0x04005620 RID: 22048
		public SkillGroup Building;

		// Token: 0x04005621 RID: 22049
		public SkillGroup Farming;

		// Token: 0x04005622 RID: 22050
		public SkillGroup Ranching;

		// Token: 0x04005623 RID: 22051
		public SkillGroup Cooking;

		// Token: 0x04005624 RID: 22052
		public SkillGroup Art;

		// Token: 0x04005625 RID: 22053
		public SkillGroup Research;

		// Token: 0x04005626 RID: 22054
		public SkillGroup Rocketry;

		// Token: 0x04005627 RID: 22055
		public SkillGroup Suits;

		// Token: 0x04005628 RID: 22056
		public SkillGroup Hauling;

		// Token: 0x04005629 RID: 22057
		public SkillGroup Technicals;

		// Token: 0x0400562A RID: 22058
		public SkillGroup MedicalAid;

		// Token: 0x0400562B RID: 22059
		public SkillGroup Basekeeping;
	}
}
