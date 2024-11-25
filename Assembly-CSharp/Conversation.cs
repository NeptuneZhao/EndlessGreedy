using System;
using System.Collections.Generic;

// Token: 0x020007E4 RID: 2020
public class Conversation
{
	// Token: 0x040021A4 RID: 8612
	public List<MinionIdentity> minions = new List<MinionIdentity>();

	// Token: 0x040021A5 RID: 8613
	public MinionIdentity lastTalked;

	// Token: 0x040021A6 RID: 8614
	public ConversationType conversationType;

	// Token: 0x040021A7 RID: 8615
	public float lastTalkedTime;

	// Token: 0x040021A8 RID: 8616
	public Conversation.Topic lastTopic;

	// Token: 0x040021A9 RID: 8617
	public int numUtterances;

	// Token: 0x020016B4 RID: 5812
	public enum ModeType
	{
		// Token: 0x0400709E RID: 28830
		Query,
		// Token: 0x0400709F RID: 28831
		Statement,
		// Token: 0x040070A0 RID: 28832
		Agreement,
		// Token: 0x040070A1 RID: 28833
		Disagreement,
		// Token: 0x040070A2 RID: 28834
		Musing,
		// Token: 0x040070A3 RID: 28835
		Satisfaction,
		// Token: 0x040070A4 RID: 28836
		Nominal,
		// Token: 0x040070A5 RID: 28837
		Dissatisfaction,
		// Token: 0x040070A6 RID: 28838
		Stressing,
		// Token: 0x040070A7 RID: 28839
		Segue,
		// Token: 0x040070A8 RID: 28840
		End
	}

	// Token: 0x020016B5 RID: 5813
	public class Mode
	{
		// Token: 0x06009358 RID: 37720 RVA: 0x00358F8F File Offset: 0x0035718F
		public Mode(Conversation.ModeType type, string voice, string icon, string mouth, string anim, bool newTopic = false)
		{
			this.type = type;
			this.voice = voice;
			this.mouth = mouth;
			this.anim = anim;
			this.icon = icon;
			this.newTopic = newTopic;
		}

		// Token: 0x040070A9 RID: 28841
		public Conversation.ModeType type;

		// Token: 0x040070AA RID: 28842
		public string voice;

		// Token: 0x040070AB RID: 28843
		public string mouth;

		// Token: 0x040070AC RID: 28844
		public string anim;

		// Token: 0x040070AD RID: 28845
		public string icon;

		// Token: 0x040070AE RID: 28846
		public bool newTopic;
	}

	// Token: 0x020016B6 RID: 5814
	public class Topic
	{
		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x06009359 RID: 37721 RVA: 0x00358FC4 File Offset: 0x003571C4
		public static Dictionary<int, Conversation.Mode> Modes
		{
			get
			{
				if (Conversation.Topic._modes == null)
				{
					Conversation.Topic._modes = new Dictionary<int, Conversation.Mode>();
					foreach (Conversation.Mode mode in Conversation.Topic.modeList)
					{
						Conversation.Topic._modes[(int)mode.type] = mode;
					}
				}
				return Conversation.Topic._modes;
			}
		}

		// Token: 0x0600935A RID: 37722 RVA: 0x00359038 File Offset: 0x00357238
		public Topic(string topic, Conversation.ModeType mode)
		{
			this.topic = topic;
			this.mode = mode;
		}

		// Token: 0x040070AF RID: 28847
		public static List<Conversation.Mode> modeList = new List<Conversation.Mode>
		{
			new Conversation.Mode(Conversation.ModeType.Query, "conversation_question", "mode_query", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Statement, "conversation_answer", "mode_statement", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Agreement, "conversation_answer", "mode_agreement", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Disagreement, "conversation_answer", "mode_disagreement", SpeechMonitor.PREFIX_SAD, "unhappy", false),
			new Conversation.Mode(Conversation.ModeType.Musing, "conversation_short", "mode_musing", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Satisfaction, "conversation_short", "mode_satisfaction", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Nominal, "conversation_short", "mode_nominal", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Dissatisfaction, "conversation_short", "mode_dissatisfaction", SpeechMonitor.PREFIX_SAD, "unhappy", false),
			new Conversation.Mode(Conversation.ModeType.Stressing, "conversation_short", "mode_stressing", SpeechMonitor.PREFIX_SAD, "unhappy", false),
			new Conversation.Mode(Conversation.ModeType.Segue, "conversation_question", "mode_segue", SpeechMonitor.PREFIX_HAPPY, "happy", true)
		};

		// Token: 0x040070B0 RID: 28848
		private static Dictionary<int, Conversation.Mode> _modes;

		// Token: 0x040070B1 RID: 28849
		public string topic;

		// Token: 0x040070B2 RID: 28850
		public Conversation.ModeType mode;
	}
}
