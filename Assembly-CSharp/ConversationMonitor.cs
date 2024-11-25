using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000973 RID: 2419
public class ConversationMonitor : GameStateMachine<ConversationMonitor, ConversationMonitor.Instance, IStateMachineTarget, ConversationMonitor.Def>
{
	// Token: 0x060046E4 RID: 18148 RVA: 0x00195664 File Offset: 0x00193864
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.TopicDiscussed, delegate(ConversationMonitor.Instance smi, object obj)
		{
			smi.OnTopicDiscussed(obj);
		}).EventHandler(GameHashes.TopicDiscovered, delegate(ConversationMonitor.Instance smi, object obj)
		{
			smi.OnTopicDiscovered(obj);
		});
	}

	// Token: 0x04002E2C RID: 11820
	private const int MAX_RECENT_TOPICS = 5;

	// Token: 0x04002E2D RID: 11821
	private const int MAX_FAVOURITE_TOPICS = 5;

	// Token: 0x04002E2E RID: 11822
	private const float FAVOURITE_CHANCE = 0.033333335f;

	// Token: 0x04002E2F RID: 11823
	private const float LEARN_CHANCE = 0.33333334f;

	// Token: 0x0200190B RID: 6411
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200190C RID: 6412
	[SerializationConfig(MemberSerialization.OptIn)]
	public new class Instance : GameStateMachine<ConversationMonitor, ConversationMonitor.Instance, IStateMachineTarget, ConversationMonitor.Def>.GameInstance
	{
		// Token: 0x06009B01 RID: 39681 RVA: 0x0036E884 File Offset: 0x0036CA84
		public Instance(IStateMachineTarget master, ConversationMonitor.Def def) : base(master, def)
		{
			this.recentTopics = new Queue<string>();
			this.favouriteTopics = new List<string>
			{
				ConversationMonitor.Instance.randomTopics[UnityEngine.Random.Range(0, ConversationMonitor.Instance.randomTopics.Count)]
			};
			this.personalTopics = new List<string>();
		}

		// Token: 0x06009B02 RID: 39682 RVA: 0x0036E8DC File Offset: 0x0036CADC
		public string GetATopic()
		{
			int maxExclusive = this.recentTopics.Count + this.favouriteTopics.Count * 2 + this.personalTopics.Count;
			int num = UnityEngine.Random.Range(0, maxExclusive);
			if (num < this.recentTopics.Count)
			{
				return this.recentTopics.Dequeue();
			}
			num -= this.recentTopics.Count;
			if (num < this.favouriteTopics.Count)
			{
				return this.favouriteTopics[num];
			}
			num -= this.favouriteTopics.Count;
			if (num < this.favouriteTopics.Count)
			{
				return this.favouriteTopics[num];
			}
			num -= this.favouriteTopics.Count;
			if (num < this.personalTopics.Count)
			{
				return this.personalTopics[num];
			}
			return "";
		}

		// Token: 0x06009B03 RID: 39683 RVA: 0x0036E9B4 File Offset: 0x0036CBB4
		public void OnTopicDiscovered(object data)
		{
			string item = (string)data;
			if (!this.recentTopics.Contains(item))
			{
				this.recentTopics.Enqueue(item);
				if (this.recentTopics.Count > 5)
				{
					string topic = this.recentTopics.Dequeue();
					this.TryMakeFavouriteTopic(topic);
				}
			}
		}

		// Token: 0x06009B04 RID: 39684 RVA: 0x0036EA04 File Offset: 0x0036CC04
		public void OnTopicDiscussed(object data)
		{
			string data2 = (string)data;
			if (UnityEngine.Random.value < 0.33333334f)
			{
				this.OnTopicDiscovered(data2);
			}
		}

		// Token: 0x06009B05 RID: 39685 RVA: 0x0036EA2C File Offset: 0x0036CC2C
		private void TryMakeFavouriteTopic(string topic)
		{
			if (UnityEngine.Random.value < 0.033333335f)
			{
				if (this.favouriteTopics.Count < 5)
				{
					this.favouriteTopics.Add(topic);
					return;
				}
				this.favouriteTopics[UnityEngine.Random.Range(0, this.favouriteTopics.Count)] = topic;
			}
		}

		// Token: 0x0400783A RID: 30778
		[Serialize]
		private Queue<string> recentTopics;

		// Token: 0x0400783B RID: 30779
		[Serialize]
		private List<string> favouriteTopics;

		// Token: 0x0400783C RID: 30780
		private List<string> personalTopics;

		// Token: 0x0400783D RID: 30781
		private static readonly List<string> randomTopics = new List<string>
		{
			"Headquarters"
		};
	}
}
