using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000EB3 RID: 3763
	public class AutomateABuilding : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x060075D3 RID: 30163 RVA: 0x002E1A94 File Offset: 0x002DFC94
		public override bool Success()
		{
			foreach (UtilityNetwork utilityNetwork in Game.Instance.logicCircuitSystem.GetNetworks())
			{
				LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)utilityNetwork;
				if (logicCircuitNetwork.Receivers.Count > 0 && logicCircuitNetwork.Senders.Count > 0)
				{
					bool flag = false;
					foreach (ILogicEventReceiver logicEventReceiver in logicCircuitNetwork.Receivers)
					{
						if (!logicEventReceiver.IsNullOrDestroyed())
						{
							GameObject gameObject = Grid.Objects[logicEventReceiver.GetLogicCell(), 1];
							if (gameObject != null && !gameObject.GetComponent<KPrefabID>().HasTag(GameTags.TemplateBuilding))
							{
								flag = true;
								break;
							}
						}
					}
					bool flag2 = false;
					foreach (ILogicEventSender logicEventSender in logicCircuitNetwork.Senders)
					{
						if (!logicEventSender.IsNullOrDestroyed())
						{
							GameObject gameObject2 = Grid.Objects[logicEventSender.GetLogicCell(), 1];
							if (gameObject2 != null && !gameObject2.GetComponent<KPrefabID>().HasTag(GameTags.TemplateBuilding))
							{
								flag2 = true;
								break;
							}
						}
					}
					if (flag && flag2)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060075D4 RID: 30164 RVA: 0x002E1C38 File Offset: 0x002DFE38
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x060075D5 RID: 30165 RVA: 0x002E1C3A File Offset: 0x002DFE3A
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.AUTOMATE_A_BUILDING;
		}
	}
}
