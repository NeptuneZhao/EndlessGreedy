using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020007E3 RID: 2019
[AddComponentMenu("KMonoBehaviour/scripts/ConversationManager")]
public class ConversationManager : KMonoBehaviour, ISim200ms
{
	// Token: 0x060037D7 RID: 14295 RVA: 0x00130B3A File Offset: 0x0012ED3A
	protected override void OnPrefabInit()
	{
		this.activeSetups = new List<Conversation>();
		this.lastConvoTimeByMinion = new Dictionary<MinionIdentity, float>();
		this.simRenderLoadBalance = true;
	}

	// Token: 0x060037D8 RID: 14296 RVA: 0x00130B5C File Offset: 0x0012ED5C
	public void Sim200ms(float dt)
	{
		for (int i = this.activeSetups.Count - 1; i >= 0; i--)
		{
			Conversation conversation = this.activeSetups[i];
			for (int j = conversation.minions.Count - 1; j >= 0; j--)
			{
				if (!this.ValidMinionTags(conversation.minions[j]) || !this.MinionCloseEnoughToConvo(conversation.minions[j], conversation))
				{
					conversation.minions.RemoveAt(j);
				}
				else
				{
					this.setupsByMinion[conversation.minions[j]] = conversation;
				}
			}
			if (conversation.minions.Count <= 1)
			{
				this.activeSetups.RemoveAt(i);
			}
			else
			{
				bool flag = true;
				bool flag2 = conversation.minions.Find((MinionIdentity match) => !match.HasTag(GameTags.Partying)) == null;
				if ((conversation.numUtterances == 0 && flag2 && GameClock.Instance.GetTime() > conversation.lastTalkedTime) || GameClock.Instance.GetTime() > conversation.lastTalkedTime + TuningData<ConversationManager.Tuning>.Get().delayBeforeStart)
				{
					MinionIdentity minionIdentity = conversation.minions[UnityEngine.Random.Range(0, conversation.minions.Count)];
					conversation.conversationType.NewTarget(minionIdentity);
					flag = this.DoTalking(conversation, minionIdentity);
				}
				else if (conversation.numUtterances > 0 && conversation.numUtterances < TuningData<ConversationManager.Tuning>.Get().maxUtterances && ((flag2 && GameClock.Instance.GetTime() > conversation.lastTalkedTime + TuningData<ConversationManager.Tuning>.Get().speakTime / 4f) || GameClock.Instance.GetTime() > conversation.lastTalkedTime + TuningData<ConversationManager.Tuning>.Get().speakTime + TuningData<ConversationManager.Tuning>.Get().delayBetweenUtterances))
				{
					int index = (conversation.minions.IndexOf(conversation.lastTalked) + UnityEngine.Random.Range(1, conversation.minions.Count)) % conversation.minions.Count;
					MinionIdentity new_speaker = conversation.minions[index];
					flag = this.DoTalking(conversation, new_speaker);
				}
				else if (conversation.numUtterances >= TuningData<ConversationManager.Tuning>.Get().maxUtterances)
				{
					flag = false;
				}
				if (!flag)
				{
					this.activeSetups.RemoveAt(i);
				}
			}
		}
		foreach (MinionIdentity minionIdentity2 in Components.LiveMinionIdentities.Items)
		{
			if (this.ValidMinionTags(minionIdentity2) && !this.setupsByMinion.ContainsKey(minionIdentity2) && !this.MinionOnCooldown(minionIdentity2))
			{
				foreach (MinionIdentity minionIdentity3 in Components.LiveMinionIdentities.Items)
				{
					if (!(minionIdentity3 == minionIdentity2) && this.ValidMinionTags(minionIdentity3))
					{
						if (this.setupsByMinion.ContainsKey(minionIdentity3))
						{
							Conversation conversation2 = this.setupsByMinion[minionIdentity3];
							if (conversation2.minions.Count < TuningData<ConversationManager.Tuning>.Get().maxDupesPerConvo && (this.GetCentroid(conversation2) - minionIdentity2.transform.GetPosition()).magnitude < TuningData<ConversationManager.Tuning>.Get().maxDistance * 0.5f)
							{
								conversation2.minions.Add(minionIdentity2);
								this.setupsByMinion[minionIdentity2] = conversation2;
								break;
							}
						}
						else if (!this.MinionOnCooldown(minionIdentity3) && (minionIdentity3.transform.GetPosition() - minionIdentity2.transform.GetPosition()).magnitude < TuningData<ConversationManager.Tuning>.Get().maxDistance)
						{
							Conversation conversation3 = new Conversation();
							conversation3.minions.Add(minionIdentity2);
							conversation3.minions.Add(minionIdentity3);
							Type type = this.convoTypes[UnityEngine.Random.Range(0, this.convoTypes.Count)];
							conversation3.conversationType = (ConversationType)Activator.CreateInstance(type);
							conversation3.lastTalkedTime = GameClock.Instance.GetTime();
							this.activeSetups.Add(conversation3);
							this.setupsByMinion[minionIdentity2] = conversation3;
							this.setupsByMinion[minionIdentity3] = conversation3;
							break;
						}
					}
				}
			}
		}
		this.setupsByMinion.Clear();
	}

	// Token: 0x060037D9 RID: 14297 RVA: 0x0013100C File Offset: 0x0012F20C
	private bool DoTalking(Conversation setup, MinionIdentity new_speaker)
	{
		DebugUtil.Assert(setup != null, "setup was null");
		DebugUtil.Assert(new_speaker != null, "new_speaker was null");
		if (setup.lastTalked != null)
		{
			setup.lastTalked.Trigger(25860745, setup.lastTalked.gameObject);
		}
		DebugUtil.Assert(setup.conversationType != null, "setup.conversationType was null");
		Conversation.Topic nextTopic = setup.conversationType.GetNextTopic(new_speaker, setup.lastTopic);
		if (nextTopic == null || nextTopic.mode == Conversation.ModeType.End || nextTopic.mode == Conversation.ModeType.Segue)
		{
			return false;
		}
		Thought thoughtForTopic = this.GetThoughtForTopic(setup, nextTopic);
		if (thoughtForTopic == null)
		{
			return false;
		}
		ThoughtGraph.Instance smi = new_speaker.GetSMI<ThoughtGraph.Instance>();
		if (smi == null)
		{
			return false;
		}
		smi.AddThought(thoughtForTopic);
		setup.lastTopic = nextTopic;
		setup.lastTalked = new_speaker;
		setup.lastTalkedTime = GameClock.Instance.GetTime();
		DebugUtil.Assert(this.lastConvoTimeByMinion != null, "lastConvoTimeByMinion was null");
		this.lastConvoTimeByMinion[setup.lastTalked] = GameClock.Instance.GetTime();
		Effects component = setup.lastTalked.GetComponent<Effects>();
		DebugUtil.Assert(component != null, "effects was null");
		component.Add("GoodConversation", true);
		Conversation.Mode mode = Conversation.Topic.Modes[(int)nextTopic.mode];
		DebugUtil.Assert(mode != null, "mode was null");
		ConversationManager.StartedTalkingEvent data = new ConversationManager.StartedTalkingEvent
		{
			talker = new_speaker.gameObject,
			anim = mode.anim
		};
		foreach (MinionIdentity minionIdentity in setup.minions)
		{
			if (!minionIdentity)
			{
				DebugUtil.DevAssert(false, "minion in setup.minions was null", null);
			}
			else
			{
				minionIdentity.Trigger(-594200555, data);
			}
		}
		setup.numUtterances++;
		return true;
	}

	// Token: 0x060037DA RID: 14298 RVA: 0x001311E8 File Offset: 0x0012F3E8
	public bool TryGetConversation(MinionIdentity minion, out Conversation conversation)
	{
		return this.setupsByMinion.TryGetValue(minion, out conversation);
	}

	// Token: 0x060037DB RID: 14299 RVA: 0x001311F8 File Offset: 0x0012F3F8
	private Vector3 GetCentroid(Conversation setup)
	{
		Vector3 a = Vector3.zero;
		foreach (MinionIdentity minionIdentity in setup.minions)
		{
			if (!(minionIdentity == null))
			{
				a += minionIdentity.transform.GetPosition();
			}
		}
		return a / (float)setup.minions.Count;
	}

	// Token: 0x060037DC RID: 14300 RVA: 0x00131278 File Offset: 0x0012F478
	private Thought GetThoughtForTopic(Conversation setup, Conversation.Topic topic)
	{
		if (string.IsNullOrEmpty(topic.topic))
		{
			DebugUtil.DevAssert(false, "topic.topic was null", null);
			return null;
		}
		Sprite sprite = setup.conversationType.GetSprite(topic.topic);
		if (sprite != null)
		{
			Conversation.Mode mode = Conversation.Topic.Modes[(int)topic.mode];
			return new Thought("Topic_" + topic.topic, null, sprite, mode.icon, mode.voice, "bubble_chatter", mode.mouth, DUPLICANTS.THOUGHTS.CONVERSATION.TOOLTIP, true, TuningData<ConversationManager.Tuning>.Get().speakTime);
		}
		return null;
	}

	// Token: 0x060037DD RID: 14301 RVA: 0x0013130C File Offset: 0x0012F50C
	private bool ValidMinionTags(MinionIdentity minion)
	{
		return !(minion == null) && !minion.GetComponent<KPrefabID>().HasAnyTags(ConversationManager.invalidConvoTags);
	}

	// Token: 0x060037DE RID: 14302 RVA: 0x0013132C File Offset: 0x0012F52C
	private bool MinionCloseEnoughToConvo(MinionIdentity minion, Conversation setup)
	{
		return (this.GetCentroid(setup) - minion.transform.GetPosition()).magnitude < TuningData<ConversationManager.Tuning>.Get().maxDistance * 0.5f;
	}

	// Token: 0x060037DF RID: 14303 RVA: 0x0013136C File Offset: 0x0012F56C
	private bool MinionOnCooldown(MinionIdentity minion)
	{
		return !minion.GetComponent<KPrefabID>().HasTag(GameTags.AlwaysConverse) && ((this.lastConvoTimeByMinion.ContainsKey(minion) && GameClock.Instance.GetTime() < this.lastConvoTimeByMinion[minion] + TuningData<ConversationManager.Tuning>.Get().minionCooldownTime) || GameClock.Instance.GetTime() / 600f < TuningData<ConversationManager.Tuning>.Get().cyclesBeforeFirstConversation);
	}

	// Token: 0x0400219F RID: 8607
	private List<Conversation> activeSetups;

	// Token: 0x040021A0 RID: 8608
	private Dictionary<MinionIdentity, float> lastConvoTimeByMinion;

	// Token: 0x040021A1 RID: 8609
	private Dictionary<MinionIdentity, Conversation> setupsByMinion = new Dictionary<MinionIdentity, Conversation>();

	// Token: 0x040021A2 RID: 8610
	private List<Type> convoTypes = new List<Type>
	{
		typeof(RecentThingConversation),
		typeof(AmountStateConversation),
		typeof(CurrentJobConversation)
	};

	// Token: 0x040021A3 RID: 8611
	private static readonly Tag[] invalidConvoTags = new Tag[]
	{
		GameTags.Asleep,
		GameTags.HoldingBreath,
		GameTags.Dead
	};

	// Token: 0x020016B1 RID: 5809
	public class Tuning : TuningData<ConversationManager.Tuning>
	{
		// Token: 0x04007091 RID: 28817
		public float cyclesBeforeFirstConversation;

		// Token: 0x04007092 RID: 28818
		public float maxDistance;

		// Token: 0x04007093 RID: 28819
		public int maxDupesPerConvo;

		// Token: 0x04007094 RID: 28820
		public float minionCooldownTime;

		// Token: 0x04007095 RID: 28821
		public float speakTime;

		// Token: 0x04007096 RID: 28822
		public float delayBetweenUtterances;

		// Token: 0x04007097 RID: 28823
		public float delayBeforeStart;

		// Token: 0x04007098 RID: 28824
		public int maxUtterances;
	}

	// Token: 0x020016B2 RID: 5810
	public class StartedTalkingEvent
	{
		// Token: 0x04007099 RID: 28825
		public GameObject talker;

		// Token: 0x0400709A RID: 28826
		public string anim;
	}
}
