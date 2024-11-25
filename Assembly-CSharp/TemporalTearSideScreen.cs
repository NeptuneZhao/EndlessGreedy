using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000DB3 RID: 3507
public class TemporalTearSideScreen : SideScreenContent
{
	// Token: 0x170007C6 RID: 1990
	// (get) Token: 0x06006ECA RID: 28362 RVA: 0x00299A7C File Offset: 0x00297C7C
	private CraftModuleInterface craftModuleInterface
	{
		get
		{
			return this.targetCraft.GetComponent<CraftModuleInterface>();
		}
	}

	// Token: 0x06006ECB RID: 28363 RVA: 0x00299A89 File Offset: 0x00297C89
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06006ECC RID: 28364 RVA: 0x00299A99 File Offset: 0x00297C99
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x06006ECD RID: 28365 RVA: 0x00299AA0 File Offset: 0x00297CA0
	public override bool IsValidForTarget(GameObject target)
	{
		Clustercraft component = target.GetComponent<Clustercraft>();
		TemporalTear temporalTear = ClusterManager.Instance.GetComponent<ClusterPOIManager>().GetTemporalTear();
		return component != null && temporalTear != null && temporalTear.Location == component.Location;
	}

	// Token: 0x06006ECE RID: 28366 RVA: 0x00299AEC File Offset: 0x00297CEC
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetCraft = target.GetComponent<Clustercraft>();
		KButton reference = base.GetComponent<HierarchyReferences>().GetReference<KButton>("button");
		reference.ClearOnClick();
		reference.onClick += delegate()
		{
			target.GetComponent<Clustercraft>();
			ClusterManager.Instance.GetComponent<ClusterPOIManager>().GetTemporalTear().ConsumeCraft(this.targetCraft);
		};
		this.RefreshPanel(null);
	}

	// Token: 0x06006ECF RID: 28367 RVA: 0x00299B58 File Offset: 0x00297D58
	private void RefreshPanel(object data = null)
	{
		TemporalTear temporalTear = ClusterManager.Instance.GetComponent<ClusterPOIManager>().GetTemporalTear();
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		bool flag = temporalTear.IsOpen();
		component.GetReference<LocText>("label").SetText(flag ? UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.BUTTON_OPEN : UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.BUTTON_CLOSED);
		component.GetReference<KButton>("button").isInteractable = flag;
	}

	// Token: 0x04004B8F RID: 19343
	private Clustercraft targetCraft;
}
