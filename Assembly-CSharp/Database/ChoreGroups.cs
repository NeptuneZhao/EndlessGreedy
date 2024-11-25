using System;
using Klei.AI;
using STRINGS;

namespace Database
{
	// Token: 0x02000E54 RID: 3668
	public class ChoreGroups : ResourceSet<ChoreGroup>
	{
		// Token: 0x06007453 RID: 29779 RVA: 0x002CD248 File Offset: 0x002CB448
		private ChoreGroup Add(string id, string name, Klei.AI.Attribute attribute, string sprite, int default_personal_priority, bool user_prioritizable = true)
		{
			ChoreGroup choreGroup = new ChoreGroup(id, name, attribute, sprite, default_personal_priority, user_prioritizable);
			base.Add(choreGroup);
			return choreGroup;
		}

		// Token: 0x06007454 RID: 29780 RVA: 0x002CD270 File Offset: 0x002CB470
		public ChoreGroups(ResourceSet parent) : base("ChoreGroups", parent)
		{
			this.Combat = this.Add("Combat", DUPLICANTS.CHOREGROUPS.COMBAT.NAME, Db.Get().Attributes.Digging, "icon_errand_combat", 5, true);
			this.LifeSupport = this.Add("LifeSupport", DUPLICANTS.CHOREGROUPS.LIFESUPPORT.NAME, Db.Get().Attributes.LifeSupport, "icon_errand_life_support", 5, true);
			this.Toggle = this.Add("Toggle", DUPLICANTS.CHOREGROUPS.TOGGLE.NAME, Db.Get().Attributes.Toggle, "icon_errand_toggle", 5, true);
			this.MedicalAid = this.Add("MedicalAid", DUPLICANTS.CHOREGROUPS.MEDICALAID.NAME, Db.Get().Attributes.Caring, "icon_errand_care", 4, true);
			if (DlcManager.FeatureClusterSpaceEnabled())
			{
				this.Rocketry = this.Add("Rocketry", DUPLICANTS.CHOREGROUPS.ROCKETRY.NAME, Db.Get().Attributes.SpaceNavigation, "icon_errand_rocketry", 4, true);
			}
			this.Basekeeping = this.Add("Basekeeping", DUPLICANTS.CHOREGROUPS.BASEKEEPING.NAME, Db.Get().Attributes.Strength, "icon_errand_tidy", 4, true);
			this.Cook = this.Add("Cook", DUPLICANTS.CHOREGROUPS.COOK.NAME, Db.Get().Attributes.Cooking, "icon_errand_cook", 3, true);
			this.Art = this.Add("Art", DUPLICANTS.CHOREGROUPS.ART.NAME, Db.Get().Attributes.Art, "icon_errand_art", 3, true);
			this.Research = this.Add("Research", DUPLICANTS.CHOREGROUPS.RESEARCH.NAME, Db.Get().Attributes.Learning, "icon_errand_research", 3, true);
			this.MachineOperating = this.Add("MachineOperating", DUPLICANTS.CHOREGROUPS.MACHINEOPERATING.NAME, Db.Get().Attributes.Machinery, "icon_errand_operate", 3, true);
			this.Farming = this.Add("Farming", DUPLICANTS.CHOREGROUPS.FARMING.NAME, Db.Get().Attributes.Botanist, "icon_errand_farm", 3, true);
			this.Ranching = this.Add("Ranching", DUPLICANTS.CHOREGROUPS.RANCHING.NAME, Db.Get().Attributes.Ranching, "icon_errand_ranch", 3, true);
			this.Build = this.Add("Build", DUPLICANTS.CHOREGROUPS.BUILD.NAME, Db.Get().Attributes.Construction, "icon_errand_toggle", 2, true);
			this.Dig = this.Add("Dig", DUPLICANTS.CHOREGROUPS.DIG.NAME, Db.Get().Attributes.Digging, "icon_errand_dig", 2, true);
			this.Hauling = this.Add("Hauling", DUPLICANTS.CHOREGROUPS.HAULING.NAME, Db.Get().Attributes.Strength, "icon_errand_supply", 1, true);
			this.Storage = this.Add("Storage", DUPLICANTS.CHOREGROUPS.STORAGE.NAME, Db.Get().Attributes.Strength, "icon_errand_storage", 1, true);
			this.Recreation = this.Add("Recreation", DUPLICANTS.CHOREGROUPS.RECREATION.NAME, Db.Get().Attributes.Strength, "icon_errand_storage", 1, false);
			Debug.Assert(true);
		}

		// Token: 0x06007455 RID: 29781 RVA: 0x002CD5D8 File Offset: 0x002CB7D8
		public ChoreGroup FindByHash(HashedString id)
		{
			ChoreGroup result = null;
			foreach (ChoreGroup choreGroup in Db.Get().ChoreGroups.resources)
			{
				if (choreGroup.IdHash == id)
				{
					result = choreGroup;
					break;
				}
			}
			return result;
		}

		// Token: 0x0400519F RID: 20895
		public ChoreGroup Build;

		// Token: 0x040051A0 RID: 20896
		public ChoreGroup Basekeeping;

		// Token: 0x040051A1 RID: 20897
		public ChoreGroup Cook;

		// Token: 0x040051A2 RID: 20898
		public ChoreGroup Art;

		// Token: 0x040051A3 RID: 20899
		public ChoreGroup Dig;

		// Token: 0x040051A4 RID: 20900
		public ChoreGroup Research;

		// Token: 0x040051A5 RID: 20901
		public ChoreGroup Farming;

		// Token: 0x040051A6 RID: 20902
		public ChoreGroup Ranching;

		// Token: 0x040051A7 RID: 20903
		public ChoreGroup Hauling;

		// Token: 0x040051A8 RID: 20904
		public ChoreGroup Storage;

		// Token: 0x040051A9 RID: 20905
		public ChoreGroup MachineOperating;

		// Token: 0x040051AA RID: 20906
		public ChoreGroup MedicalAid;

		// Token: 0x040051AB RID: 20907
		public ChoreGroup Combat;

		// Token: 0x040051AC RID: 20908
		public ChoreGroup LifeSupport;

		// Token: 0x040051AD RID: 20909
		public ChoreGroup Toggle;

		// Token: 0x040051AE RID: 20910
		public ChoreGroup Recreation;

		// Token: 0x040051AF RID: 20911
		public ChoreGroup Rocketry;
	}
}
