using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000602 RID: 1538
[DebuggerDisplay("{Id}")]
public class ScheduleBlockType : Resource
{
	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x060025D8 RID: 9688 RVA: 0x000D247E File Offset: 0x000D067E
	// (set) Token: 0x060025D9 RID: 9689 RVA: 0x000D2486 File Offset: 0x000D0686
	public Color color { get; private set; }

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x060025DA RID: 9690 RVA: 0x000D248F File Offset: 0x000D068F
	// (set) Token: 0x060025DB RID: 9691 RVA: 0x000D2497 File Offset: 0x000D0697
	public string description { get; private set; }

	// Token: 0x060025DC RID: 9692 RVA: 0x000D24A0 File Offset: 0x000D06A0
	public ScheduleBlockType(string id, ResourceSet parent, string name, string description, Color color) : base(id, parent, name)
	{
		this.color = color;
		this.description = description;
	}
}
