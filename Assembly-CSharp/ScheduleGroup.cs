using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

// Token: 0x02000603 RID: 1539
[DebuggerDisplay("{Id}")]
public class ScheduleGroup : Resource
{
	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x060025DD RID: 9693 RVA: 0x000D24BB File Offset: 0x000D06BB
	// (set) Token: 0x060025DE RID: 9694 RVA: 0x000D24C3 File Offset: 0x000D06C3
	public int defaultSegments { get; private set; }

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x060025DF RID: 9695 RVA: 0x000D24CC File Offset: 0x000D06CC
	// (set) Token: 0x060025E0 RID: 9696 RVA: 0x000D24D4 File Offset: 0x000D06D4
	public string description { get; private set; }

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x060025E1 RID: 9697 RVA: 0x000D24DD File Offset: 0x000D06DD
	// (set) Token: 0x060025E2 RID: 9698 RVA: 0x000D24E5 File Offset: 0x000D06E5
	public string notificationTooltip { get; private set; }

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x060025E3 RID: 9699 RVA: 0x000D24EE File Offset: 0x000D06EE
	// (set) Token: 0x060025E4 RID: 9700 RVA: 0x000D24F6 File Offset: 0x000D06F6
	public List<ScheduleBlockType> allowedTypes { get; private set; }

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x060025E5 RID: 9701 RVA: 0x000D24FF File Offset: 0x000D06FF
	// (set) Token: 0x060025E6 RID: 9702 RVA: 0x000D2507 File Offset: 0x000D0707
	public bool alarm { get; private set; }

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x060025E7 RID: 9703 RVA: 0x000D2510 File Offset: 0x000D0710
	// (set) Token: 0x060025E8 RID: 9704 RVA: 0x000D2518 File Offset: 0x000D0718
	public Color uiColor { get; private set; }

	// Token: 0x060025E9 RID: 9705 RVA: 0x000D2521 File Offset: 0x000D0721
	public ScheduleGroup(string id, ResourceSet parent, int defaultSegments, string name, string description, Color uiColor, string notificationTooltip, List<ScheduleBlockType> allowedTypes, bool alarm = false) : base(id, parent, name)
	{
		this.defaultSegments = defaultSegments;
		this.description = description;
		this.notificationTooltip = notificationTooltip;
		this.allowedTypes = allowedTypes;
		this.alarm = alarm;
		this.uiColor = uiColor;
	}

	// Token: 0x060025EA RID: 9706 RVA: 0x000D255C File Offset: 0x000D075C
	public bool Allowed(ScheduleBlockType type)
	{
		return this.allowedTypes.Contains(type);
	}

	// Token: 0x060025EB RID: 9707 RVA: 0x000D256A File Offset: 0x000D076A
	public string GetTooltip()
	{
		return string.Format(UI.SCHEDULEGROUPS.TOOLTIP_FORMAT, this.Name, this.description);
	}
}
