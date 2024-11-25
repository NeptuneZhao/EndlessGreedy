using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;

// Token: 0x020005FD RID: 1533
[DebuggerDisplay("{IdHash}")]
public class ChoreGroup : Resource
{
	// Token: 0x170001BF RID: 447
	// (get) Token: 0x060025A5 RID: 9637 RVA: 0x000D1E58 File Offset: 0x000D0058
	public int DefaultPersonalPriority
	{
		get
		{
			return this.defaultPersonalPriority;
		}
	}

	// Token: 0x060025A6 RID: 9638 RVA: 0x000D1E60 File Offset: 0x000D0060
	public ChoreGroup(string id, string name, Klei.AI.Attribute attribute, string sprite, int default_personal_priority, bool user_prioritizable = true) : base(id, name)
	{
		this.attribute = attribute;
		this.description = Strings.Get("STRINGS.DUPLICANTS.CHOREGROUPS." + id.ToUpper() + ".DESC").String;
		this.sprite = sprite;
		this.defaultPersonalPriority = default_personal_priority;
		this.userPrioritizable = user_prioritizable;
	}

	// Token: 0x04001570 RID: 5488
	public List<ChoreType> choreTypes = new List<ChoreType>();

	// Token: 0x04001571 RID: 5489
	public Klei.AI.Attribute attribute;

	// Token: 0x04001572 RID: 5490
	public string description;

	// Token: 0x04001573 RID: 5491
	public string sprite;

	// Token: 0x04001574 RID: 5492
	private int defaultPersonalPriority;

	// Token: 0x04001575 RID: 5493
	public bool userPrioritizable;
}
