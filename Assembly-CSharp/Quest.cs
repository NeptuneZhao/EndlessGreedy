using System;

// Token: 0x02000A05 RID: 2565
public class Quest : Resource
{
	// Token: 0x06004A73 RID: 19059 RVA: 0x001AA2E0 File Offset: 0x001A84E0
	public Quest(string id, QuestCriteria[] criteria) : base(id, id)
	{
		Debug.Assert(criteria.Length != 0);
		this.Criteria = criteria;
		string str = "STRINGS.CODEX.QUESTS." + id.ToUpperInvariant();
		StringEntry stringEntry;
		if (Strings.TryGet(str + ".NAME", out stringEntry))
		{
			this.Title = stringEntry.String;
		}
		if (Strings.TryGet(str + ".COMPLETE", out stringEntry))
		{
			this.CompletionText = stringEntry.String;
		}
		for (int i = 0; i < this.Criteria.Length; i++)
		{
			this.Criteria[i].PopulateStrings("STRINGS.CODEX.QUESTS.");
		}
	}

	// Token: 0x040030D9 RID: 12505
	public const string STRINGS_PREFIX = "STRINGS.CODEX.QUESTS.";

	// Token: 0x040030DA RID: 12506
	public readonly QuestCriteria[] Criteria;

	// Token: 0x040030DB RID: 12507
	public readonly string Title;

	// Token: 0x040030DC RID: 12508
	public readonly string CompletionText;

	// Token: 0x02001A26 RID: 6694
	public struct ItemData
	{
		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06009F3E RID: 40766 RVA: 0x0037BBA5 File Offset: 0x00379DA5
		// (set) Token: 0x06009F3F RID: 40767 RVA: 0x0037BBAF File Offset: 0x00379DAF
		public int ValueHandle
		{
			get
			{
				return this.valueHandle - 1;
			}
			set
			{
				this.valueHandle = value + 1;
			}
		}

		// Token: 0x04007B8E RID: 31630
		public int LocalCellId;

		// Token: 0x04007B8F RID: 31631
		public float CurrentValue;

		// Token: 0x04007B90 RID: 31632
		public Tag SatisfyingItem;

		// Token: 0x04007B91 RID: 31633
		public Tag QualifyingTag;

		// Token: 0x04007B92 RID: 31634
		public HashedString CriteriaId;

		// Token: 0x04007B93 RID: 31635
		private int valueHandle;
	}

	// Token: 0x02001A27 RID: 6695
	public enum State
	{
		// Token: 0x04007B95 RID: 31637
		NotStarted,
		// Token: 0x04007B96 RID: 31638
		InProgress,
		// Token: 0x04007B97 RID: 31639
		Completed
	}
}
