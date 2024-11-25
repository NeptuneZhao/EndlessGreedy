using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000E97 RID: 3735
	public class ReachedSpace : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007550 RID: 30032 RVA: 0x002DFD9A File Offset: 0x002DDF9A
		public ReachedSpace(SpaceDestinationType destinationType = null)
		{
			this.destinationType = destinationType;
		}

		// Token: 0x06007551 RID: 30033 RVA: 0x002DFDA9 File Offset: 0x002DDFA9
		public override string Name()
		{
			if (this.destinationType != null)
			{
				return string.Format(COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.REACHED_SPACE_DESTINATION, this.destinationType.Name);
			}
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.REACH_SPACE_ANY_DESTINATION;
		}

		// Token: 0x06007552 RID: 30034 RVA: 0x002DFDD8 File Offset: 0x002DDFD8
		public override string Description()
		{
			if (this.destinationType != null)
			{
				return string.Format(COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.REACHED_SPACE_DESTINATION_DESCRIPTION, this.destinationType.Name);
			}
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.REACH_SPACE_ANY_DESTINATION_DESCRIPTION;
		}

		// Token: 0x06007553 RID: 30035 RVA: 0x002DFE08 File Offset: 0x002DE008
		public override bool Success()
		{
			foreach (Spacecraft spacecraft in SpacecraftManager.instance.GetSpacecraft())
			{
				if (spacecraft.state != Spacecraft.MissionState.Grounded && spacecraft.state != Spacecraft.MissionState.Destroyed)
				{
					SpaceDestination destination = SpacecraftManager.instance.GetDestination(SpacecraftManager.instance.savedSpacecraftDestinations[spacecraft.id]);
					if (this.destinationType == null || destination.GetDestinationType() == this.destinationType)
					{
						if (this.destinationType == Db.Get().SpaceDestinationTypes.Wormhole)
						{
							Game.Instance.unlocks.Unlock("temporaltear", true);
						}
						return true;
					}
				}
			}
			return SpacecraftManager.instance.hasVisitedWormHole;
		}

		// Token: 0x06007554 RID: 30036 RVA: 0x002DFEE4 File Offset: 0x002DE0E4
		public void Deserialize(IReader reader)
		{
			if (reader.ReadByte() <= 0)
			{
				string id = reader.ReadKleiString();
				this.destinationType = Db.Get().SpaceDestinationTypes.Get(id);
			}
		}

		// Token: 0x06007555 RID: 30037 RVA: 0x002DFF19 File Offset: 0x002DE119
		public override string GetProgress(bool completed)
		{
			if (this.destinationType == null)
			{
				return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET;
			}
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET_TO_WORMHOLE;
		}

		// Token: 0x04005555 RID: 21845
		private SpaceDestinationType destinationType;
	}
}
