using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D56 RID: 3414
public class ClusterGridWorldSideScreen : SideScreenContent
{
	// Token: 0x06006B8A RID: 27530 RVA: 0x0028721C File Offset: 0x0028541C
	protected override void OnSpawn()
	{
		this.viewButton.onClick += this.OnClickView;
	}

	// Token: 0x06006B8B RID: 27531 RVA: 0x00287235 File Offset: 0x00285435
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<AsteroidGridEntity>() != null;
	}

	// Token: 0x06006B8C RID: 27532 RVA: 0x00287244 File Offset: 0x00285444
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetEntity = target.GetComponent<AsteroidGridEntity>();
		this.icon.sprite = Def.GetUISprite(this.targetEntity, "ui", false).first;
		WorldContainer component = this.targetEntity.GetComponent<WorldContainer>();
		bool flag = component != null && component.IsDiscovered;
		this.viewButton.isInteractable = flag;
		if (!flag)
		{
			this.viewButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.CLUSTERWORLDSIDESCREEN.VIEW_WORLD_DISABLE_TOOLTIP);
			return;
		}
		this.viewButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.CLUSTERWORLDSIDESCREEN.VIEW_WORLD_TOOLTIP);
	}

	// Token: 0x06006B8D RID: 27533 RVA: 0x002872E8 File Offset: 0x002854E8
	private void OnClickView()
	{
		WorldContainer component = this.targetEntity.GetComponent<WorldContainer>();
		if (!component.IsDupeVisited)
		{
			component.LookAtSurface();
		}
		ClusterManager.Instance.SetActiveWorld(component.id);
		ManagementMenu.Instance.CloseAll();
	}

	// Token: 0x0400494A RID: 18762
	public Image icon;

	// Token: 0x0400494B RID: 18763
	public KButton viewButton;

	// Token: 0x0400494C RID: 18764
	private AsteroidGridEntity targetEntity;
}
