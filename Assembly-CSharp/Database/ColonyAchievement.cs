using System;
using System.Collections.Generic;
using FMODUnity;
using ProcGen;

namespace Database
{
	// Token: 0x02000ECB RID: 3787
	public class ColonyAchievement : Resource
	{
		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06007624 RID: 30244 RVA: 0x002E4621 File Offset: 0x002E2821
		// (set) Token: 0x06007625 RID: 30245 RVA: 0x002E4629 File Offset: 0x002E2829
		public EventReference victoryNISSnapshot { get; private set; }

		// Token: 0x06007626 RID: 30246 RVA: 0x002E4634 File Offset: 0x002E2834
		public ColonyAchievement()
		{
			this.Id = "Disabled";
			this.platformAchievementId = "Disabled";
			this.Name = "Disabled";
			this.description = "Disabled";
			this.isVictoryCondition = false;
			this.requirementChecklist = new List<ColonyAchievementRequirement>();
			this.messageTitle = string.Empty;
			this.messageBody = string.Empty;
			this.shortVideoName = string.Empty;
			this.loopVideoName = string.Empty;
			this.platformAchievementId = string.Empty;
			this.icon = string.Empty;
			this.clusterTag = string.Empty;
			this.Disabled = true;
		}

		// Token: 0x06007627 RID: 30247 RVA: 0x002E46E4 File Offset: 0x002E28E4
		public ColonyAchievement(string Id, string platformAchievementId, string Name, string description, bool isVictoryCondition, List<ColonyAchievementRequirement> requirementChecklist, string messageTitle = "", string messageBody = "", string videoDataName = "", string victoryLoopVideo = "", Action<KMonoBehaviour> VictorySequence = null, EventReference victorySnapshot = default(EventReference), string icon = "", string[] dlcIds = null, string dlcIdFrom = null, string clusterTag = null) : base(Id, Name)
		{
			this.Id = Id;
			this.platformAchievementId = platformAchievementId;
			this.Name = Name;
			this.description = description;
			this.isVictoryCondition = isVictoryCondition;
			this.requirementChecklist = requirementChecklist;
			this.messageTitle = messageTitle;
			this.messageBody = messageBody;
			this.shortVideoName = videoDataName;
			this.loopVideoName = victoryLoopVideo;
			this.victorySequence = VictorySequence;
			this.victoryNISSnapshot = (victorySnapshot.IsNull ? AudioMixerSnapshots.Get().VictoryNISGenericSnapshot : victorySnapshot);
			this.icon = icon;
			this.clusterTag = clusterTag;
			this.dlcIds = dlcIds;
			if (this.dlcIds == null)
			{
				this.dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
			}
			this.dlcIdFrom = dlcIdFrom;
		}

		// Token: 0x06007628 RID: 30248 RVA: 0x002E47AC File Offset: 0x002E29AC
		public bool IsValidForSave()
		{
			if (this.clusterTag.IsNullOrWhiteSpace())
			{
				return true;
			}
			DebugUtil.Assert(CustomGameSettings.Instance != null, "IsValidForSave called when CustomGamesSettings is not initialized.");
			ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
			return currentClusterLayout != null && currentClusterLayout.clusterTags.Contains(this.clusterTag);
		}

		// Token: 0x040055B6 RID: 21942
		public string description;

		// Token: 0x040055B7 RID: 21943
		public bool isVictoryCondition;

		// Token: 0x040055B8 RID: 21944
		public string messageTitle;

		// Token: 0x040055B9 RID: 21945
		public string messageBody;

		// Token: 0x040055BA RID: 21946
		public string shortVideoName;

		// Token: 0x040055BB RID: 21947
		public string loopVideoName;

		// Token: 0x040055BC RID: 21948
		public string platformAchievementId;

		// Token: 0x040055BD RID: 21949
		public string icon;

		// Token: 0x040055BE RID: 21950
		public string clusterTag;

		// Token: 0x040055BF RID: 21951
		public List<ColonyAchievementRequirement> requirementChecklist = new List<ColonyAchievementRequirement>();

		// Token: 0x040055C0 RID: 21952
		public Action<KMonoBehaviour> victorySequence;

		// Token: 0x040055C2 RID: 21954
		public string[] dlcIds;

		// Token: 0x040055C3 RID: 21955
		public string dlcIdFrom;
	}
}
