using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000EB2 RID: 3762
	public class UpgradeAllBasicBuildings : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x060075CE RID: 30158 RVA: 0x002E1970 File Offset: 0x002DFB70
		public UpgradeAllBasicBuildings(Tag basicBuilding, Tag upgradeBuilding)
		{
			this.basicBuilding = basicBuilding;
			this.upgradeBuilding = upgradeBuilding;
		}

		// Token: 0x060075CF RID: 30159 RVA: 0x002E1988 File Offset: 0x002DFB88
		public override bool Success()
		{
			bool result = false;
			foreach (IBasicBuilding basicBuilding in Components.BasicBuildings.Items)
			{
				KPrefabID component = basicBuilding.transform.GetComponent<KPrefabID>();
				if (component.HasTag(this.basicBuilding))
				{
					return false;
				}
				if (component.HasTag(this.upgradeBuilding))
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060075D0 RID: 30160 RVA: 0x002E1A0C File Offset: 0x002DFC0C
		public void Deserialize(IReader reader)
		{
			string name = reader.ReadKleiString();
			this.basicBuilding = new Tag(name);
			string name2 = reader.ReadKleiString();
			this.upgradeBuilding = new Tag(name2);
		}

		// Token: 0x060075D1 RID: 30161 RVA: 0x002E1A40 File Offset: 0x002DFC40
		public override string GetProgress(bool complete)
		{
			BuildingDef buildingDef = Assets.GetBuildingDef(this.basicBuilding.Name);
			BuildingDef buildingDef2 = Assets.GetBuildingDef(this.upgradeBuilding.Name);
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.UPGRADE_ALL_BUILDINGS, buildingDef.Name, buildingDef2.Name);
		}

		// Token: 0x04005573 RID: 21875
		private Tag basicBuilding;

		// Token: 0x04005574 RID: 21876
		private Tag upgradeBuilding;
	}
}
