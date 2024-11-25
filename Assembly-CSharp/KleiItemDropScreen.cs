using System;
using System.Collections;
using System.Collections.Generic;
using Database;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C7B RID: 3195
public class KleiItemDropScreen : KModalScreen
{
	// Token: 0x06006251 RID: 25169 RVA: 0x0024BA95 File Offset: 0x00249C95
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		KleiItemDropScreen.Instance = this;
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		if (string.IsNullOrEmpty(KleiAccount.KleiToken))
		{
			base.Show(false);
		}
	}

	// Token: 0x06006252 RID: 25170 RVA: 0x0024BACD File Offset: 0x00249CCD
	protected override void OnActivate()
	{
		KleiItemDropScreen.Instance = this;
		this.Show(false);
	}

	// Token: 0x06006253 RID: 25171 RVA: 0x0024BADC File Offset: 0x00249CDC
	public override void Show(bool show = true)
	{
		this.serverRequestState.Reset();
		if (!show)
		{
			this.animatedLoadingIcon.gameObject.SetActive(false);
			if (this.activePresentationRoutine != null)
			{
				base.StopCoroutine(this.activePresentationRoutine);
			}
			if (this.shouldDoCloseRoutine)
			{
				this.closeButton.gameObject.SetActive(false);
				Updater.RunRoutine(this, this.AnimateScreenOutRoutine()).Then(delegate
				{
					base.Show(false);
				});
				this.shouldDoCloseRoutine = false;
			}
			else
			{
				base.Show(false);
			}
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndItemDropScreenSnapshot, STOP_MODE.ALLOWFADEOUT);
			return;
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndItemDropScreenSnapshot);
		base.Show(true);
	}

	// Token: 0x06006254 RID: 25172 RVA: 0x0024BB99 File Offset: 0x00249D99
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006255 RID: 25173 RVA: 0x0024BBBC File Offset: 0x00249DBC
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			return;
		}
		if (PermitItems.HasUnopenedItem())
		{
			this.PresentNextUnopenedItem(true);
			this.shouldDoCloseRoutine = true;
			return;
		}
		this.userMessageLabel.SetText(UI.ITEM_DROP_SCREEN.NOTHING_AVAILABLE);
		this.PresentNoItemAvailablePrompt(true);
		this.shouldDoCloseRoutine = true;
	}

	// Token: 0x06006256 RID: 25174 RVA: 0x0024BC10 File Offset: 0x00249E10
	public void PresentNextUnopenedItem(bool firstItemPresentation = true)
	{
		int num = 0;
		using (IEnumerator<KleiItems.ItemData> enumerator = PermitItems.IterateInventory().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.IsOpened)
				{
					num++;
				}
			}
		}
		this.RefreshUnopenedItemsLabel();
		foreach (KleiItems.ItemData itemData in PermitItems.IterateInventory())
		{
			if (!itemData.IsOpened)
			{
				this.PresentItem(itemData, firstItemPresentation, num == 1);
				return;
			}
		}
		this.PresentNoItemAvailablePrompt(false);
	}

	// Token: 0x06006257 RID: 25175 RVA: 0x0024BCBC File Offset: 0x00249EBC
	private void RefreshUnopenedItemsLabel()
	{
		int num = 0;
		using (IEnumerator<KleiItems.ItemData> enumerator = PermitItems.IterateInventory().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.IsOpened)
				{
					num++;
				}
			}
		}
		if (num > 1)
		{
			this.unopenedItemCountLabel.gameObject.SetActive(true);
			this.unopenedItemCountLabel.SetText(UI.ITEM_DROP_SCREEN.UNOPENED_ITEM_COUNT, (float)num);
			return;
		}
		if (num == 1)
		{
			this.unopenedItemCountLabel.gameObject.SetActive(true);
			this.unopenedItemCountLabel.SetText(UI.ITEM_DROP_SCREEN.UNOPENED_ITEM, (float)num);
			return;
		}
		this.unopenedItemCountLabel.gameObject.SetActive(false);
	}

	// Token: 0x06006258 RID: 25176 RVA: 0x0024BD78 File Offset: 0x00249F78
	public void PresentItem(KleiItems.ItemData item, bool firstItemPresentation, bool lastItemPresentation)
	{
		this.userMessageLabel.SetText(UI.ITEM_DROP_SCREEN.THANKS_FOR_PLAYING);
		this.giftAcknowledged = false;
		this.serverRequestState.revealConfirmedByServer = false;
		this.serverRequestState.revealRejectedByServer = false;
		if (this.activePresentationRoutine != null)
		{
			base.StopCoroutine(this.activePresentationRoutine);
		}
		this.activePresentationRoutine = base.StartCoroutine(this.PresentItemRoutine(item, firstItemPresentation, lastItemPresentation));
		this.acceptButton.ClearOnClick();
		this.acknowledgeButton.ClearOnClick();
		this.acceptButton.GetComponentInChildren<LocText>().SetText(UI.ITEM_DROP_SCREEN.PRINT_ITEM_BUTTON);
		this.acceptButton.onClick += delegate()
		{
			this.RequestReveal(item);
		};
		this.acknowledgeButton.onClick += delegate()
		{
			if (this.serverRequestState.revealConfirmedByServer)
			{
				this.giftAcknowledged = true;
			}
		};
	}

	// Token: 0x06006259 RID: 25177 RVA: 0x0024BE59 File Offset: 0x0024A059
	private void RequestReveal(KleiItems.ItemData item)
	{
		this.serverRequestState.revealRequested = true;
		PermitItems.QueueRequestOpenOrUnboxItem(item, new KleiItems.ResponseCallback(this.OnOpenItemRequestResponse));
	}

	// Token: 0x0600625A RID: 25178 RVA: 0x0024BE7C File Offset: 0x0024A07C
	public void OnOpenItemRequestResponse(KleiItems.Result result)
	{
		if (!this.serverRequestState.revealRequested)
		{
			return;
		}
		this.serverRequestState.revealRequested = false;
		if (result.Success)
		{
			this.serverRequestState.revealRejectedByServer = false;
			this.serverRequestState.revealConfirmedByServer = true;
			return;
		}
		this.serverRequestState.revealRejectedByServer = true;
		this.serverRequestState.revealConfirmedByServer = false;
	}

	// Token: 0x0600625B RID: 25179 RVA: 0x0024BEDC File Offset: 0x0024A0DC
	public void PresentNoItemAvailablePrompt(bool firstItemPresentation)
	{
		this.userMessageLabel.SetText(UI.ITEM_DROP_SCREEN.NOTHING_AVAILABLE);
		this.noItemAvailableAcknowledged = false;
		this.acknowledgeButton.ClearOnClick();
		this.acceptButton.ClearOnClick();
		this.acceptButton.GetComponentInChildren<LocText>().SetText(UI.ITEM_DROP_SCREEN.DISMISS_BUTTON);
		this.acceptButton.onClick += delegate()
		{
			this.noItemAvailableAcknowledged = true;
		};
		if (this.activePresentationRoutine != null)
		{
			base.StopCoroutine(this.activePresentationRoutine);
		}
		this.activePresentationRoutine = base.StartCoroutine(this.PresentNoItemAvailableRoutine(firstItemPresentation));
	}

	// Token: 0x0600625C RID: 25180 RVA: 0x0024BF73 File Offset: 0x0024A173
	private IEnumerator AnimateScreenInRoutine()
	{
		float scaleFactor = base.transform.parent.GetComponent<CanvasScaler>().scaleFactor;
		float OPEN_WIDTH = (float)Screen.width / scaleFactor;
		float y = Mathf.Clamp((float)Screen.height / scaleFactor, 720f, 900f);
		KFMOD.PlayUISound(GlobalAssets.GetSound("GiftItemDrop_Screen_Open", false));
		this.userMessageLabel.gameObject.SetActive(false);
		yield return Updater.Ease(delegate(Vector2 v2)
		{
			this.shieldMaskRect.sizeDelta = v2;
		}, this.shieldMaskRect.sizeDelta, new Vector2(this.shieldMaskRect.sizeDelta.x, y), 0.5f, Easing.CircInOut, -1f);
		yield return Updater.Ease(delegate(Vector2 v2)
		{
			this.shieldMaskRect.sizeDelta = v2;
		}, this.shieldMaskRect.sizeDelta, new Vector2(OPEN_WIDTH, this.shieldMaskRect.sizeDelta.y), 0.25f, Easing.CircInOut, -1f);
		this.userMessageLabel.gameObject.SetActive(true);
		yield break;
	}

	// Token: 0x0600625D RID: 25181 RVA: 0x0024BF82 File Offset: 0x0024A182
	private IEnumerator AnimateScreenOutRoutine()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("GiftItemDrop_Screen_Close", false));
		this.userMessageLabel.gameObject.SetActive(false);
		yield return Updater.Ease(delegate(Vector2 v2)
		{
			this.shieldMaskRect.sizeDelta = v2;
		}, this.shieldMaskRect.sizeDelta, new Vector2(8f, this.shieldMaskRect.sizeDelta.y), 0.25f, Easing.CircInOut, -1f);
		yield return Updater.Ease(delegate(Vector2 v2)
		{
			this.shieldMaskRect.sizeDelta = v2;
		}, this.shieldMaskRect.sizeDelta, new Vector2(this.shieldMaskRect.sizeDelta.x, 0f), 0.25f, Easing.CircInOut, -1f);
		yield break;
	}

	// Token: 0x0600625E RID: 25182 RVA: 0x0024BF91 File Offset: 0x0024A191
	private IEnumerator PresentNoItemAvailableRoutine(bool firstItem)
	{
		yield return null;
		this.itemNameLabel.SetText("");
		this.itemDescriptionLabel.SetText("");
		this.itemRarityLabel.SetText("");
		this.itemCategoryLabel.SetText("");
		if (firstItem)
		{
			this.animatedPod.Play("idle", KAnim.PlayMode.Loop, 1f, 0f);
			this.acceptButtonRect.gameObject.SetActive(false);
			this.shieldMaskRect.sizeDelta = new Vector2(8f, 0f);
			this.shieldMaskRect.gameObject.SetActive(true);
		}
		if (firstItem)
		{
			this.closeButton.gameObject.SetActive(false);
			yield return Updater.WaitForSeconds(0.5f);
			yield return this.AnimateScreenInRoutine();
			yield return Updater.WaitForSeconds(0.125f);
			this.closeButton.gameObject.SetActive(true);
		}
		else
		{
			yield return Updater.WaitForSeconds(0.25f);
		}
		Vector2 animate_offset = new Vector2(0f, -30f);
		this.acceptButtonRect.FindOrAddComponent<CanvasGroup>().alpha = 0f;
		this.acceptButtonRect.gameObject.SetActive(true);
		this.acceptButtonPosition.SetOn(this.acceptButtonRect);
		yield return Updater.WaitForSeconds(0.75f);
		yield return PresUtil.OffsetToAndFade(this.acceptButton.rectTransform(), animate_offset, 1f, 0.125f, Easing.ExpoOut);
		yield return Updater.Until(() => this.noItemAvailableAcknowledged);
		yield return PresUtil.OffsetFromAndFade(this.acceptButton.rectTransform(), animate_offset, 0f, 0.125f, Easing.SmoothStep);
		this.Show(false);
		yield break;
	}

	// Token: 0x0600625F RID: 25183 RVA: 0x0024BFA7 File Offset: 0x0024A1A7
	private IEnumerator PresentItemRoutine(KleiItems.ItemData item, bool firstItem, bool lastItem)
	{
		yield return null;
		if (item.ItemId == 0UL)
		{
			global::Debug.LogError("Could not find dropped item inventory.");
			yield break;
		}
		this.itemNameLabel.SetText("");
		this.itemDescriptionLabel.SetText("");
		this.itemRarityLabel.SetText("");
		this.itemCategoryLabel.SetText("");
		this.permitVisualizer.ResetState();
		if (firstItem)
		{
			this.animatedPod.Play("idle", KAnim.PlayMode.Loop, 1f, 0f);
			this.acceptButtonRect.gameObject.SetActive(false);
			this.shieldMaskRect.sizeDelta = new Vector2(8f, 0f);
			this.shieldMaskRect.gameObject.SetActive(true);
		}
		if (firstItem)
		{
			this.closeButton.gameObject.SetActive(false);
			yield return Updater.WaitForSeconds(0.5f);
			yield return this.AnimateScreenInRoutine();
			yield return Updater.WaitForSeconds(0.125f);
			this.closeButton.gameObject.SetActive(true);
		}
		Vector2 animate_offset = new Vector2(0f, -30f);
		if (firstItem)
		{
			this.acceptButtonRect.FindOrAddComponent<CanvasGroup>().alpha = 0f;
			this.acceptButtonRect.gameObject.SetActive(true);
			this.acceptButtonPosition.SetOn(this.acceptButtonRect);
			this.animatedPod.Play("powerup", KAnim.PlayMode.Once, 1f, 0f);
			this.animatedPod.Queue("working_loop", KAnim.PlayMode.Loop, 1f, 0f);
			yield return Updater.WaitForSeconds(1.25f);
			yield return PresUtil.OffsetToAndFade(this.acceptButton.rectTransform(), animate_offset, 1f, 0.125f, Easing.ExpoOut);
			yield return Updater.Until(() => this.serverRequestState.revealRequested);
			yield return PresUtil.OffsetFromAndFade(this.acceptButton.rectTransform(), animate_offset, 0f, 0.125f, Easing.SmoothStep);
		}
		else
		{
			this.RequestReveal(item);
		}
		this.animatedLoadingIcon.gameObject.rectTransform().anchoredPosition = new Vector2(0f, -352f);
		if (this.animatedLoadingIcon.GetComponent<CanvasGroup>() != null)
		{
			this.animatedLoadingIcon.GetComponent<CanvasGroup>().alpha = 1f;
		}
		yield return new WaitForSecondsRealtime(0.3f);
		if (!this.serverRequestState.revealConfirmedByServer && !this.serverRequestState.revealRejectedByServer)
		{
			this.animatedLoadingIcon.gameObject.SetActive(true);
			this.animatedLoadingIcon.Play("loading_rocket", KAnim.PlayMode.Loop, 1f, 0f);
			yield return Updater.Until(() => this.serverRequestState.revealConfirmedByServer || this.serverRequestState.revealRejectedByServer);
			yield return new WaitForSecondsRealtime(2f);
			yield return PresUtil.OffsetFromAndFade(this.animatedLoadingIcon.gameObject.rectTransform(), new Vector2(0f, -512f), 0f, 0.25f, Easing.SmoothStep);
			this.animatedLoadingIcon.gameObject.SetActive(false);
		}
		if (this.serverRequestState.revealRejectedByServer)
		{
			this.animatedPod.Play("idle", KAnim.PlayMode.Loop, 1f, 0f);
			this.errorMessage.gameObject.SetActive(true);
			yield return Updater.WaitForSeconds(3f);
			this.errorMessage.gameObject.SetActive(false);
		}
		else if (this.serverRequestState.revealConfirmedByServer)
		{
			float num = 1f;
			this.animatedPod.PlaySpeedMultiplier = (firstItem ? 1f : (1f * num));
			this.animatedPod.Play("additional_pre", KAnim.PlayMode.Once, 1f, 0f);
			this.animatedPod.Queue("working_loop", KAnim.PlayMode.Loop, 1f, 0f);
			yield return Updater.WaitForSeconds(firstItem ? 1f : (1f / num));
			this.animatedPod.PlaySpeedMultiplier = 1f;
			this.RefreshUnopenedItemsLabel();
			DropScreenPresentationInfo info;
			info.UseEquipmentVis = false;
			info.BuildOverride = null;
			info.Sprite = null;
			string name = "";
			string desc = "";
			PermitRarity rarity = PermitRarity.Unknown;
			string categoryString = "";
			string s;
			if (PermitItems.TryGetBoxInfo(item, out name, out desc, out s))
			{
				info.UseEquipmentVis = false;
				info.BuildOverride = null;
				info.Sprite = Assets.GetSprite(s);
				rarity = PermitRarity.Loyalty;
			}
			else
			{
				PermitResource permitResource = Db.Get().Permits.Get(item.Id);
				info.Sprite = permitResource.GetPermitPresentationInfo().sprite;
				info.UseEquipmentVis = (permitResource.Category == PermitCategory.Equipment);
				if (permitResource is EquippableFacadeResource)
				{
					info.BuildOverride = (permitResource as EquippableFacadeResource).BuildOverride;
				}
				name = permitResource.Name;
				desc = permitResource.Description;
				rarity = permitResource.Rarity;
				PermitCategory category = permitResource.Category;
				if (category != PermitCategory.Building)
				{
					if (category != PermitCategory.Artwork)
					{
						if (category != PermitCategory.JoyResponse)
						{
							categoryString = PermitCategories.GetDisplayName(permitResource.Category);
						}
						else
						{
							categoryString = PermitCategories.GetDisplayName(permitResource.Category);
							if (permitResource is BalloonArtistFacadeResource)
							{
								categoryString = PermitCategories.GetDisplayName(permitResource.Category) + ": " + UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JOY_RESPONSES.BALLOON_ARTIST;
							}
						}
					}
					else
					{
						categoryString = Assets.GetPrefab((permitResource as ArtableStage).prefabId).GetProperName();
					}
				}
				else
				{
					categoryString = Assets.GetPrefab((permitResource as BuildingFacadeResource).PrefabID).GetProperName();
				}
			}
			this.permitVisualizer.ConfigureWith(info);
			yield return this.permitVisualizer.AnimateIn();
			KFMOD.PlayUISoundWithLabeledParameter(GlobalAssets.GetSound("GiftItemDrop_Rarity", false), "GiftItemRarity", string.Format("{0}", rarity));
			this.itemNameLabel.SetText(name);
			this.itemDescriptionLabel.SetText(desc);
			this.itemRarityLabel.SetText(rarity.GetLocStringName());
			this.itemCategoryLabel.SetText(categoryString);
			this.itemTextContainerPosition.SetOn(this.itemTextContainer);
			yield return Updater.Parallel(new Updater[]
			{
				PresUtil.OffsetToAndFade(this.itemTextContainer.rectTransform(), animate_offset, 1f, 0.125f, Easing.CircInOut)
			});
			yield return Updater.Until(() => this.giftAcknowledged);
			if (lastItem)
			{
				this.animatedPod.Play("working_pst", KAnim.PlayMode.Once, 1f, 0f);
				this.animatedPod.Queue("idle", KAnim.PlayMode.Loop, 1f, 0f);
				yield return Updater.Parallel(new Updater[]
				{
					PresUtil.OffsetFromAndFade(this.itemTextContainer.rectTransform(), animate_offset, 0f, 0.125f, Easing.CircInOut)
				});
				this.itemNameLabel.SetText("");
				this.itemDescriptionLabel.SetText("");
				this.itemRarityLabel.SetText("");
				this.itemCategoryLabel.SetText("");
				yield return this.permitVisualizer.AnimateOut();
			}
			else
			{
				this.itemNameLabel.SetText("");
				this.itemDescriptionLabel.SetText("");
				this.itemRarityLabel.SetText("");
				this.itemCategoryLabel.SetText("");
			}
			name = null;
			desc = null;
			categoryString = null;
		}
		this.PresentNextUnopenedItem(false);
		yield break;
	}

	// Token: 0x06006260 RID: 25184 RVA: 0x0024BFCB File Offset: 0x0024A1CB
	public static bool HasItemsToShow()
	{
		return PermitItems.HasUnopenedItem();
	}

	// Token: 0x040042B5 RID: 17077
	[SerializeField]
	private RectTransform shieldMaskRect;

	// Token: 0x040042B6 RID: 17078
	[SerializeField]
	private KButton closeButton;

	// Token: 0x040042B7 RID: 17079
	[Header("Animated Item")]
	[SerializeField]
	private KleiItemDropScreen_PermitVis permitVisualizer;

	// Token: 0x040042B8 RID: 17080
	[SerializeField]
	private KBatchedAnimController animatedPod;

	// Token: 0x040042B9 RID: 17081
	[SerializeField]
	private LocText userMessageLabel;

	// Token: 0x040042BA RID: 17082
	[SerializeField]
	private LocText unopenedItemCountLabel;

	// Token: 0x040042BB RID: 17083
	[Header("Item Info")]
	[SerializeField]
	private RectTransform itemTextContainer;

	// Token: 0x040042BC RID: 17084
	[SerializeField]
	private LocText itemNameLabel;

	// Token: 0x040042BD RID: 17085
	[SerializeField]
	private LocText itemDescriptionLabel;

	// Token: 0x040042BE RID: 17086
	[SerializeField]
	private LocText itemRarityLabel;

	// Token: 0x040042BF RID: 17087
	[SerializeField]
	private LocText itemCategoryLabel;

	// Token: 0x040042C0 RID: 17088
	[Header("Accept Button")]
	[SerializeField]
	private RectTransform acceptButtonRect;

	// Token: 0x040042C1 RID: 17089
	[SerializeField]
	private KButton acceptButton;

	// Token: 0x040042C2 RID: 17090
	[SerializeField]
	private KBatchedAnimController animatedLoadingIcon;

	// Token: 0x040042C3 RID: 17091
	[SerializeField]
	private KButton acknowledgeButton;

	// Token: 0x040042C4 RID: 17092
	[SerializeField]
	private LocText errorMessage;

	// Token: 0x040042C5 RID: 17093
	private Coroutine activePresentationRoutine;

	// Token: 0x040042C6 RID: 17094
	private KleiItemDropScreen.ServerRequestState serverRequestState;

	// Token: 0x040042C7 RID: 17095
	private bool giftAcknowledged;

	// Token: 0x040042C8 RID: 17096
	private bool noItemAvailableAcknowledged;

	// Token: 0x040042C9 RID: 17097
	public static KleiItemDropScreen Instance;

	// Token: 0x040042CA RID: 17098
	private bool shouldDoCloseRoutine;

	// Token: 0x040042CB RID: 17099
	private const float TEXT_AND_BUTTON_ANIMATE_OFFSET_Y = -30f;

	// Token: 0x040042CC RID: 17100
	private PrefabDefinedUIPosition acceptButtonPosition = new PrefabDefinedUIPosition();

	// Token: 0x040042CD RID: 17101
	private PrefabDefinedUIPosition itemTextContainerPosition = new PrefabDefinedUIPosition();

	// Token: 0x02001D67 RID: 7527
	private struct ServerRequestState
	{
		// Token: 0x0600A87A RID: 43130 RVA: 0x0039CA33 File Offset: 0x0039AC33
		public void Reset()
		{
			this.revealRequested = false;
			this.revealConfirmedByServer = false;
			this.revealRejectedByServer = false;
		}

		// Token: 0x04008736 RID: 34614
		public bool revealRequested;

		// Token: 0x04008737 RID: 34615
		public bool revealConfirmedByServer;

		// Token: 0x04008738 RID: 34616
		public bool revealRejectedByServer;
	}
}
