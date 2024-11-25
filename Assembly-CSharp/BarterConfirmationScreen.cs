using System;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BE6 RID: 3046
public class BarterConfirmationScreen : KModalScreen
{
	// Token: 0x06005CB2 RID: 23730 RVA: 0x0021F0AF File Offset: 0x0021D2AF
	protected override void OnActivate()
	{
		base.OnActivate();
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.cancelButton.onClick += delegate()
		{
			this.Show(false);
		};
	}

	// Token: 0x06005CB3 RID: 23731 RVA: 0x0021F0E8 File Offset: 0x0021D2E8
	public void Present(PermitResource permit, bool isPurchase)
	{
		this.Show(true);
		this.ShowContentContainer(true);
		this.ShowLoadingPanel(false);
		this.HideResultPanel();
		if (isPurchase)
		{
			this.itemIcon.transform.SetAsLastSibling();
			this.filamentIcon.transform.SetAsFirstSibling();
		}
		else
		{
			this.itemIcon.transform.SetAsFirstSibling();
			this.filamentIcon.transform.SetAsLastSibling();
		}
		KleiItems.ResponseCallback <>9__1;
		KleiItems.ResponseCallback <>9__2;
		this.confirmButton.onClick += delegate()
		{
			string serverTypeFromPermit = PermitItems.GetServerTypeFromPermit(permit);
			if (serverTypeFromPermit == null)
			{
				return;
			}
			this.ShowContentContainer(false);
			this.HideResultPanel();
			this.ShowLoadingPanel(true);
			if (isPurchase)
			{
				string itemType = serverTypeFromPermit;
				KleiItems.ResponseCallback cb;
				if ((cb = <>9__1) == null)
				{
					cb = (<>9__1 = delegate(KleiItems.Result result)
					{
						if (this.IsNullOrDestroyed())
						{
							return;
						}
						this.ShowContentContainer(false);
						this.ShowLoadingPanel(false);
						if (!result.Success)
						{
							this.ShowResultPanel(permit, true, false);
							return;
						}
						this.ShowResultPanel(permit, true, true);
					});
				}
				KleiItems.AddRequestBarterGainItem(itemType, cb);
				return;
			}
			ulong itemInstanceID = KleiItems.GetItemInstanceID(serverTypeFromPermit);
			KleiItems.ResponseCallback cb2;
			if ((cb2 = <>9__2) == null)
			{
				cb2 = (<>9__2 = delegate(KleiItems.Result result)
				{
					if (this.IsNullOrDestroyed())
					{
						return;
					}
					this.ShowContentContainer(false);
					this.ShowLoadingPanel(false);
					if (!result.Success)
					{
						this.ShowResultPanel(permit, false, false);
						return;
					}
					this.ShowResultPanel(permit, false, true);
				});
			}
			KleiItems.AddRequestBarterLoseItem(itemInstanceID, cb2);
		};
		ulong num;
		ulong num2;
		PermitItems.TryGetBarterPrice(permit.Id, out num, out num2);
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		this.itemIcon.GetComponent<Image>().sprite = permitPresentationInfo.sprite;
		this.itemLabel.SetText(permit.Name);
		this.transactionDescriptionLabel.SetText(isPurchase ? UI.KLEI_INVENTORY_SCREEN.BARTERING.ACTION_DESCRIPTION_PRINT : UI.KLEI_INVENTORY_SCREEN.BARTERING.ACTION_DESCRIPTION_RECYCLE);
		this.panelHeaderLabel.SetText(isPurchase ? UI.KLEI_INVENTORY_SCREEN.BARTERING.CONFIRM_PRINT_HEADER : UI.KLEI_INVENTORY_SCREEN.BARTERING.CONFIRM_RECYCLE_HEADER);
		this.confirmButtonActionLabel.SetText(isPurchase ? UI.KLEI_INVENTORY_SCREEN.BARTERING.BUY : UI.KLEI_INVENTORY_SCREEN.BARTERING.SELL);
		this.confirmButtonFilamentLabel.SetText(isPurchase ? num.ToString() : (UIConstants.ColorPrefixGreen + "+" + num2.ToString() + UIConstants.ColorSuffix));
		this.largeCostLabel.SetText(isPurchase ? ("x" + num.ToString()) : ("x" + num2.ToString()));
	}

	// Token: 0x06005CB4 RID: 23732 RVA: 0x0021F2B3 File Offset: 0x0021D4B3
	private void Update()
	{
		if (this.shouldCloseScreen)
		{
			this.ShowContentContainer(false);
			this.ShowLoadingPanel(false);
			this.HideResultPanel();
			this.Show(false);
		}
	}

	// Token: 0x06005CB5 RID: 23733 RVA: 0x0021F2D8 File Offset: 0x0021D4D8
	private void ShowContentContainer(bool show)
	{
		this.contentContainer.SetActive(show);
	}

	// Token: 0x06005CB6 RID: 23734 RVA: 0x0021F2E8 File Offset: 0x0021D4E8
	private void ShowLoadingPanel(bool show)
	{
		this.loadingContainer.SetActive(show);
		this.resultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.LOADING);
		if (show)
		{
			this.loadingAnimation.Play("loading_rocket", KAnim.PlayMode.Loop, 1f, 0f);
		}
		else
		{
			this.loadingAnimation.Stop();
		}
		if (!show)
		{
			this.shouldCloseScreen = false;
		}
	}

	// Token: 0x06005CB7 RID: 23735 RVA: 0x0021F350 File Offset: 0x0021D550
	private void HideResultPanel()
	{
		this.resultContainer.SetActive(false);
	}

	// Token: 0x06005CB8 RID: 23736 RVA: 0x0021F360 File Offset: 0x0021D560
	private void ShowResultPanel(PermitResource permit, bool isPurchase, bool transationResult)
	{
		this.resultContainer.SetActive(true);
		if (!transationResult)
		{
			this.resultIcon.sprite = Assets.GetSprite("error_message");
			this.mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_ERROR);
			this.panelHeaderLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_INCOMPLETE_HEADER);
			this.resultFilamentLabel.SetText("");
			KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Bartering_Failed", false));
			return;
		}
		this.panelHeaderLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_COMPLETE_HEADER);
		if (isPurchase)
		{
			PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
			this.resultIcon.sprite = permitPresentationInfo.sprite;
			this.resultFilamentLabel.SetText("");
			this.mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.PURCHASE_SUCCESS);
			KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Print_Succeed", false));
			return;
		}
		ulong num;
		ulong num2;
		PermitItems.TryGetBarterPrice(permit.Id, out num, out num2);
		this.resultIcon.sprite = Assets.GetSprite("filament");
		this.resultFilamentLabel.GetComponent<LocText>().SetText("x" + num2.ToString());
		this.mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.SELL_SUCCESS);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Bartering_Succeed", false));
	}

	// Token: 0x04003DF1 RID: 15857
	[SerializeField]
	private GameObject itemIcon;

	// Token: 0x04003DF2 RID: 15858
	[SerializeField]
	private GameObject filamentIcon;

	// Token: 0x04003DF3 RID: 15859
	[SerializeField]
	private LocText largeCostLabel;

	// Token: 0x04003DF4 RID: 15860
	[SerializeField]
	private LocText largeQuantityLabel;

	// Token: 0x04003DF5 RID: 15861
	[SerializeField]
	private LocText itemLabel;

	// Token: 0x04003DF6 RID: 15862
	[SerializeField]
	private LocText transactionDescriptionLabel;

	// Token: 0x04003DF7 RID: 15863
	[SerializeField]
	private KButton confirmButton;

	// Token: 0x04003DF8 RID: 15864
	[SerializeField]
	private KButton cancelButton;

	// Token: 0x04003DF9 RID: 15865
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003DFA RID: 15866
	[SerializeField]
	private LocText panelHeaderLabel;

	// Token: 0x04003DFB RID: 15867
	[SerializeField]
	private LocText confirmButtonActionLabel;

	// Token: 0x04003DFC RID: 15868
	[SerializeField]
	private LocText confirmButtonFilamentLabel;

	// Token: 0x04003DFD RID: 15869
	[SerializeField]
	private LocText resultLabel;

	// Token: 0x04003DFE RID: 15870
	[SerializeField]
	private KBatchedAnimController loadingAnimation;

	// Token: 0x04003DFF RID: 15871
	[SerializeField]
	private GameObject contentContainer;

	// Token: 0x04003E00 RID: 15872
	[SerializeField]
	private GameObject loadingContainer;

	// Token: 0x04003E01 RID: 15873
	[SerializeField]
	private GameObject resultContainer;

	// Token: 0x04003E02 RID: 15874
	[SerializeField]
	private Image resultIcon;

	// Token: 0x04003E03 RID: 15875
	[SerializeField]
	private LocText mainResultLabel;

	// Token: 0x04003E04 RID: 15876
	[SerializeField]
	private LocText resultFilamentLabel;

	// Token: 0x04003E05 RID: 15877
	private bool shouldCloseScreen;
}
