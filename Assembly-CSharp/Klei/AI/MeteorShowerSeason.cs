using System;
using System.Diagnostics;
using Klei.CustomSettings;

namespace Klei.AI
{
	// Token: 0x02000F65 RID: 3941
	[DebuggerDisplay("{base.Id}")]
	public class MeteorShowerSeason : GameplaySeason
	{
		// Token: 0x0600790D RID: 30989 RVA: 0x002FE634 File Offset: 0x002FC834
		public MeteorShowerSeason(string id, GameplaySeason.Type type, string dlcId, float period, bool synchronizedToPeriod, float randomizedEventStartTime = -1f, bool startActive = false, int finishAfterNumEvents = -1, float minCycle = 0f, float maxCycle = float.PositiveInfinity, int numEventsToStartEachPeriod = 1, bool affectedByDifficultySettings = true, float clusterTravelDuration = -1f) : base(id, type, dlcId, period, synchronizedToPeriod, randomizedEventStartTime, startActive, finishAfterNumEvents, minCycle, maxCycle, numEventsToStartEachPeriod)
		{
			this.affectedByDifficultySettings = affectedByDifficultySettings;
			this.clusterTravelDuration = clusterTravelDuration;
		}

		// Token: 0x0600790E RID: 30990 RVA: 0x002FE67C File Offset: 0x002FC87C
		public override void AdditionalEventInstanceSetup(StateMachine.Instance generic_smi)
		{
			(generic_smi as MeteorShowerEvent.StatesInstance).clusterTravelDuration = this.clusterTravelDuration;
		}

		// Token: 0x0600790F RID: 30991 RVA: 0x002FE690 File Offset: 0x002FC890
		public override float GetSeasonPeriod()
		{
			SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers);
			float num = base.GetSeasonPeriod();
			if (this.affectedByDifficultySettings && currentQualitySetting != null)
			{
				string id = currentQualitySetting.id;
				if (!(id == "Infrequent"))
				{
					if (!(id == "Intense"))
					{
						if (id == "Doomed")
						{
							num *= 1f;
						}
					}
					else
					{
						num *= 1f;
					}
				}
				else
				{
					num *= 2f;
				}
			}
			return num;
		}

		// Token: 0x04005A79 RID: 23161
		public bool affectedByDifficultySettings = true;

		// Token: 0x04005A7A RID: 23162
		public float clusterTravelDuration = -1f;
	}
}
