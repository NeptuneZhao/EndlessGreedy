using System;
using UnityEngine;

// Token: 0x02000D9D RID: 3485
public class SealedDoorSideScreen : SideScreenContent
{
	// Token: 0x06006DEC RID: 28140 RVA: 0x002952CC File Offset: 0x002934CC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.button.onClick += delegate()
		{
			this.target.OrderUnseal();
		};
		this.Refresh();
	}

	// Token: 0x06006DED RID: 28141 RVA: 0x002952F1 File Offset: 0x002934F1
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Door>() != null;
	}

	// Token: 0x06006DEE RID: 28142 RVA: 0x00295300 File Offset: 0x00293500
	public override void SetTarget(GameObject target)
	{
		Door component = target.GetComponent<Door>();
		if (component == null)
		{
			global::Debug.LogError("Target doesn't have a Door associated with it.");
			return;
		}
		this.target = component;
		this.Refresh();
	}

	// Token: 0x06006DEF RID: 28143 RVA: 0x00295335 File Offset: 0x00293535
	private void Refresh()
	{
		if (!this.target.isSealed)
		{
			this.ContentContainer.SetActive(false);
			return;
		}
		this.ContentContainer.SetActive(true);
	}

	// Token: 0x04004B03 RID: 19203
	[SerializeField]
	private LocText label;

	// Token: 0x04004B04 RID: 19204
	[SerializeField]
	private KButton button;

	// Token: 0x04004B05 RID: 19205
	[SerializeField]
	private Door target;
}
