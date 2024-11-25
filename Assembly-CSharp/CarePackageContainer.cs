using System;
using System.Collections;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B89 RID: 2953
public class CarePackageContainer : KScreen, ITelepadDeliverableContainer
{
	// Token: 0x060058CE RID: 22734 RVA: 0x0020136C File Offset: 0x001FF56C
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x170006B7 RID: 1719
	// (get) Token: 0x060058CF RID: 22735 RVA: 0x00201374 File Offset: 0x001FF574
	public CarePackageInfo Info
	{
		get
		{
			return this.info;
		}
	}

	// Token: 0x060058D0 RID: 22736 RVA: 0x0020137C File Offset: 0x001FF57C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Initialize();
		base.StartCoroutine(this.DelayedGeneration());
	}

	// Token: 0x060058D1 RID: 22737 RVA: 0x00201397 File Offset: 0x001FF597
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x060058D2 RID: 22738 RVA: 0x0020139E File Offset: 0x001FF59E
	private IEnumerator DelayedGeneration()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		if (this.controller != null)
		{
			this.GenerateCharacter(this.controller.IsStarterMinion);
		}
		yield break;
	}

	// Token: 0x060058D3 RID: 22739 RVA: 0x002013AD File Offset: 0x001FF5AD
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.animController != null)
		{
			this.animController.gameObject.DeleteObject();
			this.animController = null;
		}
	}

	// Token: 0x060058D4 RID: 22740 RVA: 0x002013DC File Offset: 0x001FF5DC
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.controller != null)
		{
			CharacterSelectionController characterSelectionController = this.controller;
			characterSelectionController.OnLimitReachedEvent = (System.Action)Delegate.Remove(characterSelectionController.OnLimitReachedEvent, new System.Action(this.OnCharacterSelectionLimitReached));
			CharacterSelectionController characterSelectionController2 = this.controller;
			characterSelectionController2.OnLimitUnreachedEvent = (System.Action)Delegate.Remove(characterSelectionController2.OnLimitUnreachedEvent, new System.Action(this.OnCharacterSelectionLimitUnReached));
			CharacterSelectionController characterSelectionController3 = this.controller;
			characterSelectionController3.OnReshuffleEvent = (Action<bool>)Delegate.Remove(characterSelectionController3.OnReshuffleEvent, new Action<bool>(this.Reshuffle));
		}
	}

	// Token: 0x060058D5 RID: 22741 RVA: 0x00201472 File Offset: 0x001FF672
	private void Initialize()
	{
		this.professionIconMap = new Dictionary<string, Sprite>();
		this.professionIcons.ForEach(delegate(CarePackageContainer.ProfessionIcon ic)
		{
			this.professionIconMap.Add(ic.professionName, ic.iconImg);
		});
		if (CarePackageContainer.containers == null)
		{
			CarePackageContainer.containers = new List<ITelepadDeliverableContainer>();
		}
		CarePackageContainer.containers.Add(this);
	}

	// Token: 0x060058D6 RID: 22742 RVA: 0x002014B4 File Offset: 0x001FF6B4
	private void GenerateCharacter(bool is_starter)
	{
		int num = 0;
		do
		{
			this.info = Immigration.Instance.RandomCarePackage();
			num++;
		}
		while (this.IsCharacterRedundant() && num < 20);
		if (this.animController != null)
		{
			UnityEngine.Object.Destroy(this.animController.gameObject);
			this.animController = null;
		}
		this.carePackageInstanceData = new CarePackageContainer.CarePackageInstanceData();
		this.carePackageInstanceData.info = this.info;
		if (this.info.facadeID == "SELECTRANDOM")
		{
			this.carePackageInstanceData.facadeID = Db.GetEquippableFacades().resources.FindAll((EquippableFacadeResource match) => match.DefID == this.info.id).GetRandom<EquippableFacadeResource>().Id;
		}
		else
		{
			this.carePackageInstanceData.facadeID = this.info.facadeID;
		}
		this.SetAnimator();
		this.SetInfoText();
		this.selectButton.ClearOnClick();
		if (!this.controller.IsStarterMinion)
		{
			this.selectButton.onClick += delegate()
			{
				this.SelectDeliverable();
			};
		}
	}

	// Token: 0x060058D7 RID: 22743 RVA: 0x002015C0 File Offset: 0x001FF7C0
	private void SetAnimator()
	{
		GameObject prefab = Assets.GetPrefab(this.info.id.ToTag());
		EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(this.info.id);
		int num;
		if (ElementLoader.FindElementByName(this.info.id) != null)
		{
			num = 1;
		}
		else if (foodInfo != null)
		{
			num = (int)(this.info.quantity % foodInfo.CaloriesPerUnit);
		}
		else
		{
			num = (int)this.info.quantity;
		}
		if (prefab != null)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Util.KInstantiateUI(this.contentBody, this.contentBody.transform.parent.gameObject, false);
				gameObject.SetActive(true);
				Image component = gameObject.GetComponent<Image>();
				global::Tuple<Sprite, Color> uisprite;
				if (!this.carePackageInstanceData.facadeID.IsNullOrWhiteSpace())
				{
					uisprite = Def.GetUISprite(prefab.PrefabID(), this.carePackageInstanceData.facadeID);
				}
				else
				{
					uisprite = Def.GetUISprite(prefab, "ui", false);
				}
				component.sprite = uisprite.first;
				component.color = uisprite.second;
				this.entryIcons.Add(gameObject);
				if (num > 1)
				{
					int num2;
					int num3;
					int num4;
					if (num % 2 == 1)
					{
						num2 = Mathf.CeilToInt((float)(num / 2));
						num3 = num2 - i;
						num4 = ((num3 > 0) ? 1 : -1);
						num3 = Mathf.Abs(num3);
					}
					else
					{
						num2 = num / 2 - 1;
						if (i <= num2)
						{
							num3 = Mathf.Abs(num2 - i);
							num4 = -1;
						}
						else
						{
							num3 = Mathf.Abs(num2 + 1 - i);
							num4 = 1;
						}
					}
					int num5 = 0;
					if (num % 2 == 0)
					{
						num5 = ((i <= num2) ? -6 : 6);
						gameObject.transform.SetPosition(gameObject.transform.position += new Vector3((float)num5, 0f, 0f));
					}
					gameObject.transform.localScale = new Vector3(1f - (float)num3 * 0.1f, 1f - (float)num3 * 0.1f, 1f);
					gameObject.transform.Rotate(0f, 0f, 3f * (float)num3 * (float)num4);
					gameObject.transform.SetPosition(gameObject.transform.position + new Vector3(25f * (float)num3 * (float)num4, 5f * (float)num3) + new Vector3((float)num5, 0f, 0f));
					gameObject.GetComponent<Canvas>().sortingOrder = num - num3;
				}
			}
			return;
		}
		GameObject gameObject2 = Util.KInstantiateUI(this.contentBody, this.contentBody.transform.parent.gameObject, false);
		gameObject2.SetActive(true);
		Image component2 = gameObject2.GetComponent<Image>();
		component2.sprite = Def.GetUISpriteFromMultiObjectAnim(ElementLoader.GetElement(this.info.id.ToTag()).substance.anim, "ui", false, "");
		component2.color = ElementLoader.GetElement(this.info.id.ToTag()).substance.uiColour;
		this.entryIcons.Add(gameObject2);
	}

	// Token: 0x060058D8 RID: 22744 RVA: 0x002018F0 File Offset: 0x001FFAF0
	private string GetSpawnableName()
	{
		GameObject prefab = Assets.GetPrefab(this.info.id);
		if (prefab == null)
		{
			Element element = ElementLoader.FindElementByName(this.info.id);
			if (element != null)
			{
				return element.substance.name;
			}
			return "";
		}
		else
		{
			if (string.IsNullOrEmpty(this.carePackageInstanceData.facadeID))
			{
				return prefab.GetProperName();
			}
			return EquippableFacade.GetNameOverride(this.carePackageInstanceData.info.id, this.carePackageInstanceData.facadeID);
		}
	}

	// Token: 0x060058D9 RID: 22745 RVA: 0x0020197C File Offset: 0x001FFB7C
	private string GetSpawnableQuantityOnly()
	{
		if (ElementLoader.GetElement(this.info.id.ToTag()) != null)
		{
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT_ONLY, GameUtil.GetFormattedMass(this.info.quantity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		}
		if (EdiblesManager.GetFoodInfo(this.info.id) != null)
		{
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT_ONLY, GameUtil.GetFormattedCaloriesForItem(this.info.id, this.info.quantity, GameUtil.TimeSlice.None, true));
		}
		return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT_ONLY, this.info.quantity.ToString());
	}

	// Token: 0x060058DA RID: 22746 RVA: 0x00201A30 File Offset: 0x001FFC30
	private string GetCurrentQuantity(WorldInventory inventory)
	{
		if (ElementLoader.GetElement(this.info.id.ToTag()) != null)
		{
			float amount = inventory.GetAmount(this.info.id.ToTag(), false);
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_CURRENT_AMOUNT, GameUtil.GetFormattedMass(amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		}
		if (EdiblesManager.GetFoodInfo(this.info.id) != null)
		{
			float calories = WorldResourceAmountTracker<RationTracker>.Get().CountAmountForItemWithID(this.info.id, inventory, true);
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_CURRENT_AMOUNT, GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true));
		}
		float amount2 = inventory.GetAmount(this.info.id.ToTag(), false);
		return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_CURRENT_AMOUNT, amount2.ToString());
	}

	// Token: 0x060058DB RID: 22747 RVA: 0x00201AFC File Offset: 0x001FFCFC
	private string GetSpawnableQuantity()
	{
		if (ElementLoader.GetElement(this.info.id.ToTag()) != null)
		{
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_QUANTITY, GameUtil.GetFormattedMass(this.info.quantity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), Assets.GetPrefab(this.info.id).GetProperName());
		}
		if (EdiblesManager.GetFoodInfo(this.info.id) != null)
		{
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_QUANTITY, GameUtil.GetFormattedCaloriesForItem(this.info.id, this.info.quantity, GameUtil.TimeSlice.None, true), Assets.GetPrefab(this.info.id).GetProperName());
		}
		return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT, Assets.GetPrefab(this.info.id).GetProperName(), this.info.quantity.ToString());
	}

	// Token: 0x060058DC RID: 22748 RVA: 0x00201BFC File Offset: 0x001FFDFC
	private string GetSpawnableDescription()
	{
		Element element = ElementLoader.GetElement(this.info.id.ToTag());
		if (element != null)
		{
			return element.Description();
		}
		GameObject prefab = Assets.GetPrefab(this.info.id);
		if (prefab == null)
		{
			return "";
		}
		InfoDescription component = prefab.GetComponent<InfoDescription>();
		if (component != null)
		{
			return component.description;
		}
		return prefab.GetProperName();
	}

	// Token: 0x060058DD RID: 22749 RVA: 0x00201C6C File Offset: 0x001FFE6C
	private void SetInfoText()
	{
		this.characterName.SetText(this.GetSpawnableName());
		this.description.SetText(this.GetSpawnableDescription());
		this.itemName.SetText(this.GetSpawnableName());
		this.quantity.SetText(this.GetSpawnableQuantityOnly());
		this.currentQuantity.SetText(this.GetCurrentQuantity(ClusterManager.Instance.activeWorld.worldInventory));
	}

	// Token: 0x060058DE RID: 22750 RVA: 0x00201CE0 File Offset: 0x001FFEE0
	public void SelectDeliverable()
	{
		if (this.controller != null)
		{
			this.controller.AddDeliverable(this.carePackageInstanceData);
		}
		if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
		{
			MusicManager.instance.SetSongParameter("Music_SelectDuplicant", "songSection", 1f, true);
		}
		this.selectButton.GetComponent<ImageToggleState>().SetActive();
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.DeselectDeliverable();
			if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
			{
				MusicManager.instance.SetSongParameter("Music_SelectDuplicant", "songSection", 0f, true);
			}
		};
		this.selectedBorder.SetActive(true);
		this.titleBar.color = this.selectedTitleColor;
	}

	// Token: 0x060058DF RID: 22751 RVA: 0x00201D88 File Offset: 0x001FFF88
	public void DeselectDeliverable()
	{
		if (this.controller != null)
		{
			this.controller.RemoveDeliverable(this.carePackageInstanceData);
		}
		this.selectButton.GetComponent<ImageToggleState>().SetInactive();
		this.selectButton.Deselect();
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.SelectDeliverable();
		};
		this.selectedBorder.SetActive(false);
		this.titleBar.color = this.deselectedTitleColor;
	}

	// Token: 0x060058E0 RID: 22752 RVA: 0x00201E0E File Offset: 0x0020000E
	private void OnReplacedEvent(ITelepadDeliverable stats)
	{
		if (stats == this.carePackageInstanceData)
		{
			this.DeselectDeliverable();
		}
	}

	// Token: 0x060058E1 RID: 22753 RVA: 0x00201E20 File Offset: 0x00200020
	private void OnCharacterSelectionLimitReached()
	{
		if (this.controller != null && this.controller.IsSelected(this.info))
		{
			return;
		}
		this.selectButton.ClearOnClick();
		if (this.controller.AllowsReplacing)
		{
			this.selectButton.onClick += this.ReplaceCharacterSelection;
			return;
		}
		this.selectButton.onClick += this.CantSelectCharacter;
	}

	// Token: 0x060058E2 RID: 22754 RVA: 0x00201E96 File Offset: 0x00200096
	private void CantSelectCharacter()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
	}

	// Token: 0x060058E3 RID: 22755 RVA: 0x00201EA8 File Offset: 0x002000A8
	private void ReplaceCharacterSelection()
	{
		if (this.controller == null)
		{
			return;
		}
		this.controller.RemoveLast();
		this.SelectDeliverable();
	}

	// Token: 0x060058E4 RID: 22756 RVA: 0x00201ECC File Offset: 0x002000CC
	private void OnCharacterSelectionLimitUnReached()
	{
		if (this.controller != null && this.controller.IsSelected(this.info))
		{
			return;
		}
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.SelectDeliverable();
		};
	}

	// Token: 0x060058E5 RID: 22757 RVA: 0x00201F1D File Offset: 0x0020011D
	public void SetReshufflingState(bool enable)
	{
		this.reshuffleButton.gameObject.SetActive(enable);
	}

	// Token: 0x060058E6 RID: 22758 RVA: 0x00201F30 File Offset: 0x00200130
	private void Reshuffle(bool is_starter)
	{
		if (this.controller != null && this.controller.IsSelected(this.info))
		{
			this.DeselectDeliverable();
		}
		this.ClearEntryIcons();
		this.GenerateCharacter(is_starter);
	}

	// Token: 0x060058E7 RID: 22759 RVA: 0x00201F68 File Offset: 0x00200168
	public void SetController(CharacterSelectionController csc)
	{
		if (csc == this.controller)
		{
			return;
		}
		this.controller = csc;
		CharacterSelectionController characterSelectionController = this.controller;
		characterSelectionController.OnLimitReachedEvent = (System.Action)Delegate.Combine(characterSelectionController.OnLimitReachedEvent, new System.Action(this.OnCharacterSelectionLimitReached));
		CharacterSelectionController characterSelectionController2 = this.controller;
		characterSelectionController2.OnLimitUnreachedEvent = (System.Action)Delegate.Combine(characterSelectionController2.OnLimitUnreachedEvent, new System.Action(this.OnCharacterSelectionLimitUnReached));
		CharacterSelectionController characterSelectionController3 = this.controller;
		characterSelectionController3.OnReshuffleEvent = (Action<bool>)Delegate.Combine(characterSelectionController3.OnReshuffleEvent, new Action<bool>(this.Reshuffle));
		CharacterSelectionController characterSelectionController4 = this.controller;
		characterSelectionController4.OnReplacedEvent = (Action<ITelepadDeliverable>)Delegate.Combine(characterSelectionController4.OnReplacedEvent, new Action<ITelepadDeliverable>(this.OnReplacedEvent));
	}

	// Token: 0x060058E8 RID: 22760 RVA: 0x00202028 File Offset: 0x00200228
	public void DisableSelectButton()
	{
		this.selectButton.soundPlayer.AcceptClickCondition = (() => false);
		this.selectButton.GetComponent<ImageToggleState>().SetDisabled();
		this.selectButton.soundPlayer.Enabled = false;
	}

	// Token: 0x060058E9 RID: 22761 RVA: 0x00202088 File Offset: 0x00200288
	private bool IsCharacterRedundant()
	{
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in CarePackageContainer.containers)
		{
			if (telepadDeliverableContainer != this)
			{
				CarePackageContainer carePackageContainer = telepadDeliverableContainer as CarePackageContainer;
				if (carePackageContainer != null && carePackageContainer.info == this.info)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060058EA RID: 22762 RVA: 0x002020FC File Offset: 0x002002FC
	public string GetValueColor(bool isPositive)
	{
		if (!isPositive)
		{
			return "<color=#ff2222ff>";
		}
		return "<color=green>";
	}

	// Token: 0x060058EB RID: 22763 RVA: 0x0020210C File Offset: 0x0020030C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.IsAction(global::Action.Escape))
		{
			this.controller.OnPressBack();
		}
		if (!KInputManager.currentControllerIsGamepad)
		{
			e.Consumed = true;
		}
	}

	// Token: 0x060058EC RID: 22764 RVA: 0x00202130 File Offset: 0x00200330
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!KInputManager.currentControllerIsGamepad)
		{
			e.Consumed = true;
		}
	}

	// Token: 0x060058ED RID: 22765 RVA: 0x00202140 File Offset: 0x00200340
	protected override void OnCmpEnable()
	{
		base.OnActivate();
		if (this.info == null)
		{
			return;
		}
		this.ClearEntryIcons();
		this.SetAnimator();
		this.SetInfoText();
	}

	// Token: 0x060058EE RID: 22766 RVA: 0x00202164 File Offset: 0x00200364
	private void ClearEntryIcons()
	{
		for (int i = 0; i < this.entryIcons.Count; i++)
		{
			UnityEngine.Object.Destroy(this.entryIcons[i]);
		}
	}

	// Token: 0x04003A4D RID: 14925
	[Header("UI References")]
	[SerializeField]
	private GameObject contentBody;

	// Token: 0x04003A4E RID: 14926
	[SerializeField]
	private LocText characterName;

	// Token: 0x04003A4F RID: 14927
	public GameObject selectedBorder;

	// Token: 0x04003A50 RID: 14928
	[SerializeField]
	private Image titleBar;

	// Token: 0x04003A51 RID: 14929
	[SerializeField]
	private Color selectedTitleColor;

	// Token: 0x04003A52 RID: 14930
	[SerializeField]
	private Color deselectedTitleColor;

	// Token: 0x04003A53 RID: 14931
	[SerializeField]
	private KButton reshuffleButton;

	// Token: 0x04003A54 RID: 14932
	private KBatchedAnimController animController;

	// Token: 0x04003A55 RID: 14933
	[SerializeField]
	private LocText itemName;

	// Token: 0x04003A56 RID: 14934
	[SerializeField]
	private LocText quantity;

	// Token: 0x04003A57 RID: 14935
	[SerializeField]
	private LocText currentQuantity;

	// Token: 0x04003A58 RID: 14936
	[SerializeField]
	private LocText description;

	// Token: 0x04003A59 RID: 14937
	[SerializeField]
	private KToggle selectButton;

	// Token: 0x04003A5A RID: 14938
	private CarePackageInfo info;

	// Token: 0x04003A5B RID: 14939
	public CarePackageContainer.CarePackageInstanceData carePackageInstanceData;

	// Token: 0x04003A5C RID: 14940
	private CharacterSelectionController controller;

	// Token: 0x04003A5D RID: 14941
	private static List<ITelepadDeliverableContainer> containers;

	// Token: 0x04003A5E RID: 14942
	[SerializeField]
	private Sprite enabledSpr;

	// Token: 0x04003A5F RID: 14943
	[SerializeField]
	private List<CarePackageContainer.ProfessionIcon> professionIcons;

	// Token: 0x04003A60 RID: 14944
	private Dictionary<string, Sprite> professionIconMap;

	// Token: 0x04003A61 RID: 14945
	public float baseCharacterScale = 0.38f;

	// Token: 0x04003A62 RID: 14946
	private List<GameObject> entryIcons = new List<GameObject>();

	// Token: 0x02001BEA RID: 7146
	[Serializable]
	public struct ProfessionIcon
	{
		// Token: 0x0400811A RID: 33050
		public string professionName;

		// Token: 0x0400811B RID: 33051
		public Sprite iconImg;
	}

	// Token: 0x02001BEB RID: 7147
	public class CarePackageInstanceData : ITelepadDeliverable
	{
		// Token: 0x0600A4D1 RID: 42193 RVA: 0x0038DA5A File Offset: 0x0038BC5A
		public GameObject Deliver(Vector3 position)
		{
			GameObject gameObject = this.info.Deliver(position);
			gameObject.GetComponent<CarePackage>().SetFacade(this.facadeID);
			return gameObject;
		}

		// Token: 0x0400811C RID: 33052
		public CarePackageInfo info;

		// Token: 0x0400811D RID: 33053
		public string facadeID;
	}
}
