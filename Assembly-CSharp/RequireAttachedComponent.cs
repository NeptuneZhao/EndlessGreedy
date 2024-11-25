using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B0E RID: 2830
[SerializationConfig(MemberSerialization.OptIn)]
public class RequireAttachedComponent : ProcessCondition
{
	// Token: 0x17000656 RID: 1622
	// (get) Token: 0x0600544E RID: 21582 RVA: 0x001E282C File Offset: 0x001E0A2C
	// (set) Token: 0x0600544F RID: 21583 RVA: 0x001E2834 File Offset: 0x001E0A34
	public Type RequiredType
	{
		get
		{
			return this.requiredType;
		}
		set
		{
			this.requiredType = value;
			this.typeNameString = this.requiredType.Name;
		}
	}

	// Token: 0x06005450 RID: 21584 RVA: 0x001E284E File Offset: 0x001E0A4E
	public RequireAttachedComponent(AttachableBuilding myAttachable, Type required_type, string type_name_string)
	{
		this.myAttachable = myAttachable;
		this.requiredType = required_type;
		this.typeNameString = type_name_string;
	}

	// Token: 0x06005451 RID: 21585 RVA: 0x001E286C File Offset: 0x001E0A6C
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.myAttachable != null)
		{
			using (List<GameObject>.Enumerator enumerator = AttachableBuilding.GetAttachedNetwork(this.myAttachable).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetComponent(this.requiredType))
					{
						return ProcessCondition.Status.Ready;
					}
				}
			}
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06005452 RID: 21586 RVA: 0x001E28E4 File Offset: 0x001E0AE4
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		return this.typeNameString;
	}

	// Token: 0x06005453 RID: 21587 RVA: 0x001E28F0 File Offset: 0x001E0AF0
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return string.Format(UI.STARMAP.LAUNCHCHECKLIST.INSTALLED_TOOLTIP, this.typeNameString.ToLower());
		}
		return string.Format(UI.STARMAP.LAUNCHCHECKLIST.MISSING_TOOLTIP, this.typeNameString.ToLower());
	}

	// Token: 0x06005454 RID: 21588 RVA: 0x001E292B File Offset: 0x001E0B2B
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400374C RID: 14156
	private string typeNameString;

	// Token: 0x0400374D RID: 14157
	private Type requiredType;

	// Token: 0x0400374E RID: 14158
	private AttachableBuilding myAttachable;
}
