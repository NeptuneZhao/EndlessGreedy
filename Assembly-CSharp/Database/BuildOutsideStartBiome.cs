using System;
using Klei;
using ProcGen;
using STRINGS;

namespace Database
{
	// Token: 0x02000EAD RID: 3757
	public class BuildOutsideStartBiome : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x060075BA RID: 30138 RVA: 0x002E12C8 File Offset: 0x002DF4C8
		public override bool Success()
		{
			WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
			foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
			{
				if (!buildingComplete.GetComponent<KPrefabID>().HasTag(GameTags.TemplateBuilding))
				{
					for (int i = 0; i < clusterDetailSave.overworldCells.Count; i++)
					{
						WorldDetailSave.OverworldCell overworldCell = clusterDetailSave.overworldCells[i];
						if (overworldCell.tags != null && !overworldCell.tags.Contains(WorldGenTags.StartWorld) && overworldCell.poly.PointInPolygon(buildingComplete.transform.GetPosition()))
						{
							Game.Instance.unlocks.Unlock("buildoutsidestartingbiome", true);
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060075BB RID: 30139 RVA: 0x002E13B8 File Offset: 0x002DF5B8
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x060075BC RID: 30140 RVA: 0x002E13BA File Offset: 0x002DF5BA
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_OUTSIDE_START;
		}
	}
}
