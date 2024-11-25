using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x020005FE RID: 1534
[DebuggerDisplay("{IdHash}")]
public class ChoreType : Resource
{
	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x060025A7 RID: 9639 RVA: 0x000D1EC4 File Offset: 0x000D00C4
	// (set) Token: 0x060025A8 RID: 9640 RVA: 0x000D1ECC File Offset: 0x000D00CC
	public Urge urge { get; private set; }

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x060025A9 RID: 9641 RVA: 0x000D1ED5 File Offset: 0x000D00D5
	// (set) Token: 0x060025AA RID: 9642 RVA: 0x000D1EDD File Offset: 0x000D00DD
	public ChoreGroup[] groups { get; private set; }

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x060025AB RID: 9643 RVA: 0x000D1EE6 File Offset: 0x000D00E6
	// (set) Token: 0x060025AC RID: 9644 RVA: 0x000D1EEE File Offset: 0x000D00EE
	public int priority { get; private set; }

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x060025AD RID: 9645 RVA: 0x000D1EF7 File Offset: 0x000D00F7
	// (set) Token: 0x060025AE RID: 9646 RVA: 0x000D1EFF File Offset: 0x000D00FF
	public int interruptPriority { get; set; }

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x060025AF RID: 9647 RVA: 0x000D1F08 File Offset: 0x000D0108
	// (set) Token: 0x060025B0 RID: 9648 RVA: 0x000D1F10 File Offset: 0x000D0110
	public int explicitPriority { get; private set; }

	// Token: 0x060025B1 RID: 9649 RVA: 0x000D1F19 File Offset: 0x000D0119
	private string ResolveStringCallback(string str, object data)
	{
		return ((Chore)data).ResolveString(str);
	}

	// Token: 0x060025B2 RID: 9650 RVA: 0x000D1F28 File Offset: 0x000D0128
	public ChoreType(string id, ResourceSet parent, string[] chore_groups, string urge, string name, string status_message, string tooltip, IEnumerable<Tag> interrupt_exclusion, int implicit_priority, int explicit_priority) : base(id, parent, name)
	{
		this.statusItem = new StatusItem(id, status_message, tooltip, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
		this.statusItem.resolveStringCallback = new Func<string, object, string>(this.ResolveStringCallback);
		this.tags.Add(TagManager.Create(id));
		this.interruptExclusion = new HashSet<Tag>(interrupt_exclusion);
		Db.Get().DuplicantStatusItems.Add(this.statusItem);
		List<ChoreGroup> list = new List<ChoreGroup>();
		for (int i = 0; i < chore_groups.Length; i++)
		{
			ChoreGroup choreGroup = Db.Get().ChoreGroups.TryGet(chore_groups[i]);
			if (choreGroup != null)
			{
				if (!choreGroup.choreTypes.Contains(this))
				{
					choreGroup.choreTypes.Add(this);
				}
				list.Add(choreGroup);
			}
		}
		this.groups = list.ToArray();
		if (!string.IsNullOrEmpty(urge))
		{
			this.urge = Db.Get().Urges.Get(urge);
		}
		this.priority = implicit_priority;
		this.explicitPriority = explicit_priority;
	}

	// Token: 0x04001576 RID: 5494
	public StatusItem statusItem;

	// Token: 0x0400157B RID: 5499
	public HashSet<Tag> tags = new HashSet<Tag>();

	// Token: 0x0400157C RID: 5500
	public HashSet<Tag> interruptExclusion;

	// Token: 0x0400157E RID: 5502
	public string reportName;
}
