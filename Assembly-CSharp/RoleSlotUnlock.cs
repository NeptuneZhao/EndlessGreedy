using System;
using System.Collections.Generic;

// Token: 0x02000A6C RID: 2668
public class RoleSlotUnlock
{
	// Token: 0x17000596 RID: 1430
	// (get) Token: 0x06004DA0 RID: 19872 RVA: 0x001BD339 File Offset: 0x001BB539
	// (set) Token: 0x06004DA1 RID: 19873 RVA: 0x001BD341 File Offset: 0x001BB541
	public string id { get; protected set; }

	// Token: 0x17000597 RID: 1431
	// (get) Token: 0x06004DA2 RID: 19874 RVA: 0x001BD34A File Offset: 0x001BB54A
	// (set) Token: 0x06004DA3 RID: 19875 RVA: 0x001BD352 File Offset: 0x001BB552
	public string name { get; protected set; }

	// Token: 0x17000598 RID: 1432
	// (get) Token: 0x06004DA4 RID: 19876 RVA: 0x001BD35B File Offset: 0x001BB55B
	// (set) Token: 0x06004DA5 RID: 19877 RVA: 0x001BD363 File Offset: 0x001BB563
	public string description { get; protected set; }

	// Token: 0x17000599 RID: 1433
	// (get) Token: 0x06004DA6 RID: 19878 RVA: 0x001BD36C File Offset: 0x001BB56C
	// (set) Token: 0x06004DA7 RID: 19879 RVA: 0x001BD374 File Offset: 0x001BB574
	public List<global::Tuple<string, int>> slots { get; protected set; }

	// Token: 0x1700059A RID: 1434
	// (get) Token: 0x06004DA8 RID: 19880 RVA: 0x001BD37D File Offset: 0x001BB57D
	// (set) Token: 0x06004DA9 RID: 19881 RVA: 0x001BD385 File Offset: 0x001BB585
	public Func<bool> isSatisfied { get; protected set; }

	// Token: 0x06004DAA RID: 19882 RVA: 0x001BD38E File Offset: 0x001BB58E
	public RoleSlotUnlock(string id, string name, string description, List<global::Tuple<string, int>> slots, Func<bool> isSatisfied)
	{
		this.id = id;
		this.name = name;
		this.description = description;
		this.slots = slots;
		this.isSatisfied = isSatisfied;
	}
}
