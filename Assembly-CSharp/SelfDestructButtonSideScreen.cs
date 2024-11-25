using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000DA1 RID: 3489
public class SelfDestructButtonSideScreen : SideScreenContent
{
	// Token: 0x06006E2D RID: 28205 RVA: 0x002976E5 File Offset: 0x002958E5
	protected override void OnSpawn()
	{
		this.Refresh();
		this.button.onClick += this.TriggerDestruct;
	}

	// Token: 0x06006E2E RID: 28206 RVA: 0x00297704 File Offset: 0x00295904
	public override int GetSideScreenSortOrder()
	{
		return -150;
	}

	// Token: 0x06006E2F RID: 28207 RVA: 0x0029770B File Offset: 0x0029590B
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<CraftModuleInterface>() != null && target.HasTag(GameTags.RocketInSpace);
	}

	// Token: 0x06006E30 RID: 28208 RVA: 0x00297728 File Offset: 0x00295928
	public override void SetTarget(GameObject target)
	{
		this.craftInterface = target.GetComponent<CraftModuleInterface>();
		this.acknowledgeWarnings = false;
		this.craftInterface.Subscribe<SelfDestructButtonSideScreen>(-1582839653, SelfDestructButtonSideScreen.TagsChangedDelegate);
		this.Refresh();
	}

	// Token: 0x06006E31 RID: 28209 RVA: 0x00297759 File Offset: 0x00295959
	public override void ClearTarget()
	{
		if (this.craftInterface != null)
		{
			this.craftInterface.Unsubscribe<SelfDestructButtonSideScreen>(-1582839653, SelfDestructButtonSideScreen.TagsChangedDelegate, false);
			this.craftInterface = null;
		}
	}

	// Token: 0x06006E32 RID: 28210 RVA: 0x00297786 File Offset: 0x00295986
	private void OnTagsChanged(object data)
	{
		if (((TagChangedEventData)data).tag == GameTags.RocketStranded)
		{
			this.Refresh();
		}
	}

	// Token: 0x06006E33 RID: 28211 RVA: 0x002977A5 File Offset: 0x002959A5
	private void TriggerDestruct()
	{
		if (this.acknowledgeWarnings)
		{
			this.craftInterface.gameObject.Trigger(-1061799784, null);
			this.acknowledgeWarnings = false;
		}
		else
		{
			this.acknowledgeWarnings = true;
		}
		this.Refresh();
	}

	// Token: 0x06006E34 RID: 28212 RVA: 0x002977DC File Offset: 0x002959DC
	private void Refresh()
	{
		if (this.craftInterface == null)
		{
			return;
		}
		this.statusText.text = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.MESSAGE_TEXT;
		if (this.acknowledgeWarnings)
		{
			this.button.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.BUTTON_TEXT_CONFIRM;
			this.button.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.BUTTON_TOOLTIP_CONFIRM;
			return;
		}
		this.button.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.BUTTON_TEXT;
		this.button.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.BUTTON_TOOLTIP;
	}

	// Token: 0x04004B39 RID: 19257
	public KButton button;

	// Token: 0x04004B3A RID: 19258
	public LocText statusText;

	// Token: 0x04004B3B RID: 19259
	private CraftModuleInterface craftInterface;

	// Token: 0x04004B3C RID: 19260
	private bool acknowledgeWarnings;

	// Token: 0x04004B3D RID: 19261
	private static readonly EventSystem.IntraObjectHandler<SelfDestructButtonSideScreen> TagsChangedDelegate = new EventSystem.IntraObjectHandler<SelfDestructButtonSideScreen>(delegate(SelfDestructButtonSideScreen cmp, object data)
	{
		cmp.OnTagsChanged(data);
	});
}
