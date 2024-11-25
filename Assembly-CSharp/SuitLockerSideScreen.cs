using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000DAF RID: 3503
public class SuitLockerSideScreen : SideScreenContent
{
	// Token: 0x06006EA3 RID: 28323 RVA: 0x00298D4C File Offset: 0x00296F4C
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06006EA4 RID: 28324 RVA: 0x00298D54 File Offset: 0x00296F54
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<SuitLocker>() != null;
	}

	// Token: 0x06006EA5 RID: 28325 RVA: 0x00298D64 File Offset: 0x00296F64
	public override void SetTarget(GameObject target)
	{
		this.suitLocker = target.GetComponent<SuitLocker>();
		this.initialConfigRequestSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_REQUEST_SUIT_TOOLTIP);
		this.initialConfigNoSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_NO_SUIT_TOOLTIP);
		this.initialConfigRequestSuitButton.ClearOnClick();
		this.initialConfigRequestSuitButton.onClick += delegate()
		{
			this.suitLocker.ConfigRequestSuit();
		};
		this.initialConfigNoSuitButton.ClearOnClick();
		this.initialConfigNoSuitButton.onClick += delegate()
		{
			this.suitLocker.ConfigNoSuit();
		};
		this.regularConfigRequestSuitButton.ClearOnClick();
		this.regularConfigRequestSuitButton.onClick += delegate()
		{
			if (this.suitLocker.smi.sm.isWaitingForSuit.Get(this.suitLocker.smi))
			{
				this.suitLocker.ConfigNoSuit();
				return;
			}
			this.suitLocker.ConfigRequestSuit();
		};
		this.regularConfigDropSuitButton.ClearOnClick();
		this.regularConfigDropSuitButton.onClick += delegate()
		{
			this.suitLocker.DropSuit();
		};
	}

	// Token: 0x06006EA6 RID: 28326 RVA: 0x00298E3C File Offset: 0x0029703C
	private void Update()
	{
		bool flag = this.suitLocker.smi.sm.isConfigured.Get(this.suitLocker.smi);
		this.initialConfigScreen.gameObject.SetActive(!flag);
		this.regularConfigScreen.gameObject.SetActive(flag);
		bool flag2 = this.suitLocker.GetStoredOutfit() != null;
		bool flag3 = this.suitLocker.smi.sm.isWaitingForSuit.Get(this.suitLocker.smi);
		this.regularConfigRequestSuitButton.isInteractable = !flag2;
		if (!flag3)
		{
			this.regularConfigRequestSuitButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_REQUEST_SUIT;
			this.regularConfigRequestSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_REQUEST_SUIT_TOOLTIP);
		}
		else
		{
			this.regularConfigRequestSuitButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_CANCEL_REQUEST;
			this.regularConfigRequestSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_CANCEL_REQUEST_TOOLTIP);
		}
		if (flag2)
		{
			this.regularConfigDropSuitButton.isInteractable = true;
			this.regularConfigDropSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_DROP_SUIT_TOOLTIP);
		}
		else
		{
			this.regularConfigDropSuitButton.isInteractable = false;
			this.regularConfigDropSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_DROP_SUIT_NO_SUIT_TOOLTIP);
		}
		KSelectable component = this.suitLocker.GetComponent<KSelectable>();
		if (component != null)
		{
			StatusItemGroup.Entry statusItem = component.GetStatusItem(Db.Get().StatusItemCategories.Main);
			if (statusItem.item != null)
			{
				this.regularConfigLabel.text = statusItem.item.GetName(statusItem.data);
				this.regularConfigLabel.GetComponentInChildren<ToolTip>().SetSimpleTooltip(statusItem.item.GetTooltip(statusItem.data));
			}
		}
	}

	// Token: 0x04004B6E RID: 19310
	[SerializeField]
	private GameObject initialConfigScreen;

	// Token: 0x04004B6F RID: 19311
	[SerializeField]
	private GameObject regularConfigScreen;

	// Token: 0x04004B70 RID: 19312
	[SerializeField]
	private LocText initialConfigLabel;

	// Token: 0x04004B71 RID: 19313
	[SerializeField]
	private KButton initialConfigRequestSuitButton;

	// Token: 0x04004B72 RID: 19314
	[SerializeField]
	private KButton initialConfigNoSuitButton;

	// Token: 0x04004B73 RID: 19315
	[SerializeField]
	private LocText regularConfigLabel;

	// Token: 0x04004B74 RID: 19316
	[SerializeField]
	private KButton regularConfigRequestSuitButton;

	// Token: 0x04004B75 RID: 19317
	[SerializeField]
	private KButton regularConfigDropSuitButton;

	// Token: 0x04004B76 RID: 19318
	private SuitLocker suitLocker;
}
