using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007E6 RID: 2022
public class CurrentJobConversation : ConversationType
{
	// Token: 0x060037E7 RID: 14311 RVA: 0x00131489 File Offset: 0x0012F689
	public CurrentJobConversation()
	{
		this.id = "CurrentJobConversation";
	}

	// Token: 0x060037E8 RID: 14312 RVA: 0x0013149C File Offset: 0x0012F69C
	public override void NewTarget(MinionIdentity speaker)
	{
		this.target = "hows_role";
	}

	// Token: 0x060037E9 RID: 14313 RVA: 0x001314AC File Offset: 0x0012F6AC
	public override Conversation.Topic GetNextTopic(MinionIdentity speaker, Conversation.Topic lastTopic)
	{
		if (lastTopic == null)
		{
			return new Conversation.Topic(this.target, Conversation.ModeType.Query);
		}
		List<Conversation.ModeType> list = CurrentJobConversation.transitions[lastTopic.mode];
		Conversation.ModeType modeType = list[UnityEngine.Random.Range(0, list.Count)];
		if (modeType == Conversation.ModeType.Statement)
		{
			this.target = this.GetRoleForSpeaker(speaker);
			Conversation.ModeType modeForRole = this.GetModeForRole(speaker, this.target);
			return new Conversation.Topic(this.target, modeForRole);
		}
		return new Conversation.Topic(this.target, modeType);
	}

	// Token: 0x060037EA RID: 14314 RVA: 0x00131528 File Offset: 0x0012F728
	public override Sprite GetSprite(string topic)
	{
		if (topic == "hows_role")
		{
			return Assets.GetSprite("crew_state_role");
		}
		if (Db.Get().Skills.TryGet(topic) != null)
		{
			return Assets.GetSprite(Db.Get().Skills.Get(topic).hat);
		}
		return null;
	}

	// Token: 0x060037EB RID: 14315 RVA: 0x00131585 File Offset: 0x0012F785
	private Conversation.ModeType GetModeForRole(MinionIdentity speaker, string roleId)
	{
		return Conversation.ModeType.Nominal;
	}

	// Token: 0x060037EC RID: 14316 RVA: 0x00131588 File Offset: 0x0012F788
	private string GetRoleForSpeaker(MinionIdentity speaker)
	{
		return speaker.GetComponent<MinionResume>().CurrentRole;
	}

	// Token: 0x040021AC RID: 8620
	public static Dictionary<Conversation.ModeType, List<Conversation.ModeType>> transitions = new Dictionary<Conversation.ModeType, List<Conversation.ModeType>>
	{
		{
			Conversation.ModeType.Query,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Statement
			}
		},
		{
			Conversation.ModeType.Satisfaction,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Agreement
			}
		},
		{
			Conversation.ModeType.Nominal,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Musing
			}
		},
		{
			Conversation.ModeType.Dissatisfaction,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Disagreement
			}
		},
		{
			Conversation.ModeType.Stressing,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Disagreement
			}
		},
		{
			Conversation.ModeType.Agreement,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Query,
				Conversation.ModeType.End
			}
		},
		{
			Conversation.ModeType.Disagreement,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Query,
				Conversation.ModeType.End
			}
		},
		{
			Conversation.ModeType.Musing,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Query,
				Conversation.ModeType.End
			}
		}
	};
}
