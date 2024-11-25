using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000654 RID: 1620
[DebuggerDisplay("{Id}")]
[Serializable]
public class AssignableSlot : Resource
{
	// Token: 0x060027D5 RID: 10197 RVA: 0x000E2879 File Offset: 0x000E0A79
	public AssignableSlot(string id, string name, bool showInUI = true) : base(id, name)
	{
		this.showInUI = showInUI;
	}

	// Token: 0x060027D6 RID: 10198 RVA: 0x000E2894 File Offset: 0x000E0A94
	public AssignableSlotInstance Lookup(GameObject go)
	{
		Assignables component = go.GetComponent<Assignables>();
		if (component != null)
		{
			return component.GetSlot(this);
		}
		return null;
	}

	// Token: 0x0400170B RID: 5899
	public bool showInUI = true;
}
