using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DAE RID: 3502
public class SpecialCargoBayClusterSideScreen : ReceptacleSideScreen
{
	// Token: 0x06006E9C RID: 28316 RVA: 0x00298C7B File Offset: 0x00296E7B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06006E9D RID: 28317 RVA: 0x00298C83 File Offset: 0x00296E83
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<SpecialCargoBayClusterReceptacle>() != null;
	}

	// Token: 0x06006E9E RID: 28318 RVA: 0x00298C91 File Offset: 0x00296E91
	protected override bool RequiresAvailableAmountToDeposit()
	{
		return false;
	}

	// Token: 0x06006E9F RID: 28319 RVA: 0x00298C94 File Offset: 0x00296E94
	protected override void UpdateState(object data)
	{
		base.UpdateState(data);
		this.SetDescriptionSidescreenFoldState(this.targetReceptacle != null && this.targetReceptacle.Occupant == null);
	}

	// Token: 0x06006EA0 RID: 28320 RVA: 0x00298CC8 File Offset: 0x00296EC8
	protected override void SetResultDescriptions(GameObject go)
	{
		base.SetResultDescriptions(go);
		if (this.targetReceptacle != null && this.targetReceptacle.Occupant != null)
		{
			this.descriptionLabel.SetText("");
			this.SetDescriptionSidescreenFoldState(false);
			return;
		}
		this.SetDescriptionSidescreenFoldState(true);
	}

	// Token: 0x06006EA1 RID: 28321 RVA: 0x00298D1C File Offset: 0x00296F1C
	public void SetDescriptionSidescreenFoldState(bool visible)
	{
		this.descriptionContent.minHeight = (visible ? this.descriptionLayoutDefaultSize : 0f);
	}

	// Token: 0x04004B6C RID: 19308
	public LayoutElement descriptionContent;

	// Token: 0x04004B6D RID: 19309
	public float descriptionLayoutDefaultSize = -1f;
}
