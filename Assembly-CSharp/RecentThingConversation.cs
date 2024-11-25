using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007E7 RID: 2023
public class RecentThingConversation : ConversationType
{
	// Token: 0x060037EE RID: 14318 RVA: 0x0013165F File Offset: 0x0012F85F
	public RecentThingConversation()
	{
		this.id = "RecentThingConversation";
	}

	// Token: 0x060037EF RID: 14319 RVA: 0x00131674 File Offset: 0x0012F874
	public override void NewTarget(MinionIdentity speaker)
	{
		ConversationMonitor.Instance smi = speaker.GetSMI<ConversationMonitor.Instance>();
		this.target = smi.GetATopic();
	}

	// Token: 0x060037F0 RID: 14320 RVA: 0x00131694 File Offset: 0x0012F894
	public override Conversation.Topic GetNextTopic(MinionIdentity speaker, Conversation.Topic lastTopic)
	{
		if (string.IsNullOrEmpty(this.target))
		{
			return null;
		}
		List<Conversation.ModeType> list;
		if (lastTopic == null)
		{
			list = new List<Conversation.ModeType>
			{
				Conversation.ModeType.Query,
				Conversation.ModeType.Statement,
				Conversation.ModeType.Musing
			};
		}
		else
		{
			list = RecentThingConversation.transitions[lastTopic.mode];
		}
		Conversation.ModeType mode = list[UnityEngine.Random.Range(0, list.Count)];
		return new Conversation.Topic(this.target, mode);
	}

	// Token: 0x060037F1 RID: 14321 RVA: 0x00131700 File Offset: 0x0012F900
	public override Sprite GetSprite(string topic)
	{
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(topic, "ui", true);
		if (uisprite != null)
		{
			return uisprite.first;
		}
		return null;
	}

	// Token: 0x040021AD RID: 8621
	public static Dictionary<Conversation.ModeType, List<Conversation.ModeType>> transitions = new Dictionary<Conversation.ModeType, List<Conversation.ModeType>>
	{
		{
			Conversation.ModeType.Query,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Agreement,
				Conversation.ModeType.Disagreement,
				Conversation.ModeType.Musing
			}
		},
		{
			Conversation.ModeType.Statement,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Agreement,
				Conversation.ModeType.Disagreement,
				Conversation.ModeType.Query,
				Conversation.ModeType.Segue
			}
		},
		{
			Conversation.ModeType.Agreement,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Satisfaction
			}
		},
		{
			Conversation.ModeType.Disagreement,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Dissatisfaction
			}
		},
		{
			Conversation.ModeType.Musing,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Query,
				Conversation.ModeType.Statement,
				Conversation.ModeType.Segue
			}
		},
		{
			Conversation.ModeType.Satisfaction,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Segue,
				Conversation.ModeType.End
			}
		},
		{
			Conversation.ModeType.Nominal,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Segue,
				Conversation.ModeType.End
			}
		},
		{
			Conversation.ModeType.Dissatisfaction,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Segue,
				Conversation.ModeType.End
			}
		}
	};
}
