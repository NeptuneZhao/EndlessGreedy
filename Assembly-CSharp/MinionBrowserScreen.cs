using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CE2 RID: 3298
public class MinionBrowserScreen : KMonoBehaviour
{
	// Token: 0x060065ED RID: 26093 RVA: 0x0025FD98 File Offset: 0x0025DF98
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.gridLayouter = new GridLayouter
		{
			minCellSize = 112f,
			maxCellSize = 144f,
			targetGridLayouts = this.galleryGridContent.GetComponents<GridLayoutGroup>().ToList<GridLayoutGroup>()
		};
		this.galleryGridItemPool = new UIPrefabLocalPool(this.gridItemPrefab, this.galleryGridContent.gameObject);
	}

	// Token: 0x060065EE RID: 26094 RVA: 0x0025FE00 File Offset: 0x0025E000
	protected override void OnCmpEnable()
	{
		if (this.isFirstDisplay)
		{
			this.isFirstDisplay = false;
			this.PopulateGallery();
			this.RefreshPreview();
			this.cycler.Initialize(this.CreateCycleOptions());
			this.editButton.onClick += delegate()
			{
				if (this.OnEditClickedFn != null)
				{
					this.OnEditClickedFn();
				}
			};
			this.changeOutfitButton.onClick += this.OnClickChangeOutfit;
		}
		else
		{
			this.RefreshGallery();
			this.RefreshPreview();
		}
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.RefreshGallery();
			this.RefreshPreview();
		});
	}

	// Token: 0x060065EF RID: 26095 RVA: 0x0025FE8C File Offset: 0x0025E08C
	private void Update()
	{
		this.gridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x060065F0 RID: 26096 RVA: 0x0025FE9C File Offset: 0x0025E09C
	protected override void OnSpawn()
	{
		this.postponeConfiguration = false;
		if (this.Config.isValid)
		{
			this.Configure(this.Config);
			return;
		}
		this.Configure(MinionBrowserScreenConfig.Personalities(default(Option<Personality>)));
	}

	// Token: 0x1700075B RID: 1883
	// (get) Token: 0x060065F1 RID: 26097 RVA: 0x0025FEDE File Offset: 0x0025E0DE
	// (set) Token: 0x060065F2 RID: 26098 RVA: 0x0025FEE6 File Offset: 0x0025E0E6
	public MinionBrowserScreenConfig Config { get; private set; }

	// Token: 0x060065F3 RID: 26099 RVA: 0x0025FEEF File Offset: 0x0025E0EF
	public void Configure(MinionBrowserScreenConfig config)
	{
		this.Config = config;
		if (this.postponeConfiguration)
		{
			return;
		}
		this.PopulateGallery();
		this.RefreshPreview();
	}

	// Token: 0x060065F4 RID: 26100 RVA: 0x0025FF0D File Offset: 0x0025E10D
	private void RefreshGallery()
	{
		if (this.RefreshGalleryFn != null)
		{
			this.RefreshGalleryFn();
		}
	}

	// Token: 0x060065F5 RID: 26101 RVA: 0x0025FF24 File Offset: 0x0025E124
	public void PopulateGallery()
	{
		this.RefreshGalleryFn = null;
		this.galleryGridItemPool.ReturnAll();
		foreach (MinionBrowserScreen.GridItem item in this.Config.items)
		{
			this.<PopulateGallery>g__AddGridIcon|32_0(item);
		}
		this.RefreshGallery();
		this.SelectMinion(this.Config.defaultSelectedItem.Unwrap());
	}

	// Token: 0x060065F6 RID: 26102 RVA: 0x0025FF88 File Offset: 0x0025E188
	private void SelectMinion(MinionBrowserScreen.GridItem item)
	{
		this.selectedGridItem = item;
		this.RefreshGallery();
		this.RefreshPreview();
		this.UIMinion.GetMinionVoice().PlaySoundUI("voice_land");
	}

	// Token: 0x060065F7 RID: 26103 RVA: 0x0025FFC0 File Offset: 0x0025E1C0
	public void RefreshPreview()
	{
		this.UIMinion.SetMinion(this.selectedGridItem.GetPersonality());
		this.UIMinion.ReactToPersonalityChange();
		this.detailsHeaderText.SetText(this.selectedGridItem.GetName());
		this.detailHeaderIcon.sprite = this.selectedGridItem.GetIcon();
		this.RefreshOutfitDescription();
		this.RefreshPreviewButtonsInteractable();
		this.SetDioramaBG();
	}

	// Token: 0x060065F8 RID: 26104 RVA: 0x0026002C File Offset: 0x0025E22C
	private void RefreshOutfitDescription()
	{
		if (this.RefreshOutfitDescriptionFn != null)
		{
			this.RefreshOutfitDescriptionFn();
		}
	}

	// Token: 0x060065F9 RID: 26105 RVA: 0x00260044 File Offset: 0x0025E244
	private void OnClickChangeOutfit()
	{
		if (this.selectedOutfitType.IsNone())
		{
			return;
		}
		OutfitBrowserScreenConfig.Minion(this.selectedOutfitType.Unwrap(), this.selectedGridItem).WithOutfit(this.selectedOutfit).ApplyAndOpenScreen();
	}

	// Token: 0x060065FA RID: 26106 RVA: 0x0026008C File Offset: 0x0025E28C
	private void RefreshPreviewButtonsInteractable()
	{
		this.editButton.isInteractable = true;
		if (this.currentOutfitType == ClothingOutfitUtility.OutfitType.JoyResponse)
		{
			Option<string> joyResponseEditError = this.GetJoyResponseEditError();
			if (joyResponseEditError.IsSome())
			{
				this.editButton.isInteractable = false;
				this.editButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(joyResponseEditError.Unwrap());
				return;
			}
			this.editButton.isInteractable = true;
			this.editButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
		}
	}

	// Token: 0x060065FB RID: 26107 RVA: 0x00260108 File Offset: 0x0025E308
	private void SetDioramaBG()
	{
		this.dioramaBGImage.sprite = KleiPermitDioramaVis.GetDioramaBackground(this.currentOutfitType);
	}

	// Token: 0x060065FC RID: 26108 RVA: 0x00260120 File Offset: 0x0025E320
	private Option<string> GetJoyResponseEditError()
	{
		string joyTrait = this.selectedGridItem.GetPersonality().joyTrait;
		if (!(joyTrait == "BalloonArtist"))
		{
			return Option.Some<string>(UI.JOY_RESPONSE_DESIGNER_SCREEN.TOOLTIP_NO_FACADES_FOR_JOY_TRAIT.Replace("{JoyResponseType}", Db.Get().traits.Get(joyTrait).Name));
		}
		return Option.None;
	}

	// Token: 0x060065FD RID: 26109 RVA: 0x00260180 File Offset: 0x0025E380
	public void SetEditingOutfitType(ClothingOutfitUtility.OutfitType outfitType)
	{
		this.currentOutfitType = outfitType;
		switch (outfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			this.editButtonText.text = UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_OUTFIT_ITEMS;
			this.changeOutfitButton.gameObject.SetActive(true);
			break;
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			this.editButtonText.text = UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_JOY_RESPONSE;
			this.changeOutfitButton.gameObject.SetActive(false);
			break;
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			this.editButtonText.text = UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_ATMO_SUIT_OUTFIT_ITEMS;
			this.changeOutfitButton.gameObject.SetActive(true);
			break;
		default:
			throw new NotImplementedException();
		}
		this.RefreshPreviewButtonsInteractable();
		this.OnEditClickedFn = delegate()
		{
			switch (outfitType)
			{
			case ClothingOutfitUtility.OutfitType.Clothing:
			case ClothingOutfitUtility.OutfitType.AtmoSuit:
				OutfitDesignerScreenConfig.Minion(this.selectedOutfit.IsSome() ? this.selectedOutfit.Unwrap() : ClothingOutfitTarget.ForNewTemplateOutfit(outfitType), this.selectedGridItem).ApplyAndOpenScreen();
				return;
			case ClothingOutfitUtility.OutfitType.JoyResponse:
			{
				JoyResponseScreenConfig joyResponseScreenConfig = JoyResponseScreenConfig.From(this.selectedGridItem);
				joyResponseScreenConfig = joyResponseScreenConfig.WithInitialSelection(this.selectedGridItem.GetJoyResponseOutfitTarget().ReadFacadeId().AndThen<BalloonArtistFacadeResource>((string id) => Db.Get().Permits.BalloonArtistFacades.Get(id)));
				joyResponseScreenConfig.ApplyAndOpenScreen();
				return;
			}
			default:
				throw new NotImplementedException();
			}
		};
		this.RefreshOutfitDescriptionFn = delegate()
		{
			switch (outfitType)
			{
			case ClothingOutfitUtility.OutfitType.Clothing:
			case ClothingOutfitUtility.OutfitType.AtmoSuit:
				this.selectedOutfit = this.selectedGridItem.GetClothingOutfitTarget(outfitType);
				this.UIMinion.SetOutfit(outfitType, this.selectedOutfit);
				this.outfitDescriptionPanel.Refresh(this.selectedOutfit, outfitType, this.selectedGridItem.GetPersonality());
				return;
			case ClothingOutfitUtility.OutfitType.JoyResponse:
			{
				this.selectedOutfit = this.selectedGridItem.GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType.Clothing);
				this.UIMinion.SetOutfit(ClothingOutfitUtility.OutfitType.Clothing, this.selectedOutfit);
				string text = this.selectedGridItem.GetJoyResponseOutfitTarget().ReadFacadeId().UnwrapOr(null, null);
				this.outfitDescriptionPanel.Refresh((text != null) ? Db.Get().Permits.Get(text) : null, outfitType, this.selectedGridItem.GetPersonality());
				return;
			}
			default:
				throw new NotImplementedException();
			}
		};
		this.RefreshOutfitDescription();
	}

	// Token: 0x060065FE RID: 26110 RVA: 0x00260278 File Offset: 0x0025E478
	private MinionBrowserScreen.CyclerUI.OnSelectedFn[] CreateCycleOptions()
	{
		MinionBrowserScreen.CyclerUI.OnSelectedFn[] array = new MinionBrowserScreen.CyclerUI.OnSelectedFn[3];
		for (int i = 0; i < 3; i++)
		{
			ClothingOutfitUtility.OutfitType outfitType = (ClothingOutfitUtility.OutfitType)i;
			array[i] = delegate()
			{
				this.selectedOutfitType = Option.Some<ClothingOutfitUtility.OutfitType>(outfitType);
				this.cycler.SetLabel(outfitType.GetName());
				this.SetEditingOutfitType(outfitType);
				this.RefreshPreview();
			};
		}
		return array;
	}

	// Token: 0x060065FF RID: 26111 RVA: 0x002602BC File Offset: 0x0025E4BC
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x06006603 RID: 26115 RVA: 0x00260308 File Offset: 0x0025E508
	[CompilerGenerated]
	private void <PopulateGallery>g__AddGridIcon|32_0(MinionBrowserScreen.GridItem item)
	{
		GameObject gameObject = this.galleryGridItemPool.Borrow();
		gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = item.GetIcon();
		gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(item.GetName());
		string requiredDlcId = item.GetPersonality().requiredDlcId;
		ToolTip component = gameObject.GetComponent<ToolTip>();
		Image component2 = gameObject.transform.Find("DlcBanner").GetComponent<Image>();
		if (DlcManager.IsDlcId(requiredDlcId))
		{
			component2.gameObject.SetActive(true);
			component2.color = DlcManager.GetDlcBannerColor(requiredDlcId);
			component.SetSimpleTooltip(string.Format(UI.MINION_BROWSER_SCREEN.TOOLTIP_FROM_DLC, DlcManager.GetDlcTitle(requiredDlcId)));
		}
		else
		{
			component2.gameObject.SetActive(false);
			component.ClearMultiStringTooltip();
		}
		MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
		MultiToggle toggle3 = toggle;
		toggle3.onEnter = (System.Action)Delegate.Combine(toggle3.onEnter, new System.Action(this.OnMouseOverToggle));
		MultiToggle toggle2 = toggle;
		toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
		{
			this.SelectMinion(item);
		}));
		this.RefreshGalleryFn = (System.Action)Delegate.Combine(this.RefreshGalleryFn, new System.Action(delegate()
		{
			toggle.ChangeState((item == this.selectedGridItem) ? 1 : 0);
		}));
	}

	// Token: 0x040044C9 RID: 17609
	[Header("ItemGalleryColumn")]
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x040044CA RID: 17610
	[SerializeField]
	private GameObject gridItemPrefab;

	// Token: 0x040044CB RID: 17611
	private GridLayouter gridLayouter;

	// Token: 0x040044CC RID: 17612
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private KleiPermitDioramaVis permitVis;

	// Token: 0x040044CD RID: 17613
	[SerializeField]
	private UIMinion UIMinion;

	// Token: 0x040044CE RID: 17614
	[SerializeField]
	private LocText detailsHeaderText;

	// Token: 0x040044CF RID: 17615
	[SerializeField]
	private Image detailHeaderIcon;

	// Token: 0x040044D0 RID: 17616
	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	// Token: 0x040044D1 RID: 17617
	[SerializeField]
	private MinionBrowserScreen.CyclerUI cycler;

	// Token: 0x040044D2 RID: 17618
	[SerializeField]
	private KButton editButton;

	// Token: 0x040044D3 RID: 17619
	[SerializeField]
	private LocText editButtonText;

	// Token: 0x040044D4 RID: 17620
	[SerializeField]
	private KButton changeOutfitButton;

	// Token: 0x040044D5 RID: 17621
	private Option<ClothingOutfitUtility.OutfitType> selectedOutfitType;

	// Token: 0x040044D6 RID: 17622
	private Option<ClothingOutfitTarget> selectedOutfit;

	// Token: 0x040044D7 RID: 17623
	[Header("Diorama Backgrounds")]
	[SerializeField]
	private Image dioramaBGImage;

	// Token: 0x040044D8 RID: 17624
	private MinionBrowserScreen.GridItem selectedGridItem;

	// Token: 0x040044D9 RID: 17625
	private System.Action OnEditClickedFn;

	// Token: 0x040044DA RID: 17626
	private bool isFirstDisplay = true;

	// Token: 0x040044DC RID: 17628
	private bool postponeConfiguration = true;

	// Token: 0x040044DD RID: 17629
	private UIPrefabLocalPool galleryGridItemPool;

	// Token: 0x040044DE RID: 17630
	private System.Action RefreshGalleryFn;

	// Token: 0x040044DF RID: 17631
	private System.Action RefreshOutfitDescriptionFn;

	// Token: 0x040044E0 RID: 17632
	private ClothingOutfitUtility.OutfitType currentOutfitType;

	// Token: 0x02001DDD RID: 7645
	private enum MultiToggleState
	{
		// Token: 0x0400886C RID: 34924
		Default,
		// Token: 0x0400886D RID: 34925
		Selected,
		// Token: 0x0400886E RID: 34926
		NonInteractable
	}

	// Token: 0x02001DDE RID: 7646
	[Serializable]
	public class CyclerUI
	{
		// Token: 0x0600A9D9 RID: 43481 RVA: 0x0039FE80 File Offset: 0x0039E080
		public void Initialize(MinionBrowserScreen.CyclerUI.OnSelectedFn[] cycleOptions)
		{
			this.cyclePrevButton.onClick += this.CyclePrev;
			this.cycleNextButton.onClick += this.CycleNext;
			this.SetCycleOptions(cycleOptions);
		}

		// Token: 0x0600A9DA RID: 43482 RVA: 0x0039FEB7 File Offset: 0x0039E0B7
		public void SetCycleOptions(MinionBrowserScreen.CyclerUI.OnSelectedFn[] cycleOptions)
		{
			DebugUtil.Assert(cycleOptions != null);
			DebugUtil.Assert(cycleOptions.Length != 0);
			this.cycleOptions = cycleOptions;
			this.GoTo(0);
		}

		// Token: 0x0600A9DB RID: 43483 RVA: 0x0039FEDC File Offset: 0x0039E0DC
		public void GoTo(int wrappingIndex)
		{
			if (this.cycleOptions == null || this.cycleOptions.Length == 0)
			{
				return;
			}
			while (wrappingIndex < 0)
			{
				wrappingIndex += this.cycleOptions.Length;
			}
			while (wrappingIndex >= this.cycleOptions.Length)
			{
				wrappingIndex -= this.cycleOptions.Length;
			}
			this.selectedIndex = wrappingIndex;
			this.cycleOptions[this.selectedIndex]();
		}

		// Token: 0x0600A9DC RID: 43484 RVA: 0x0039FF3D File Offset: 0x0039E13D
		public void CyclePrev()
		{
			this.GoTo(this.selectedIndex - 1);
		}

		// Token: 0x0600A9DD RID: 43485 RVA: 0x0039FF4D File Offset: 0x0039E14D
		public void CycleNext()
		{
			this.GoTo(this.selectedIndex + 1);
		}

		// Token: 0x0600A9DE RID: 43486 RVA: 0x0039FF5D File Offset: 0x0039E15D
		public void SetLabel(string text)
		{
			this.currentLabel.text = text;
		}

		// Token: 0x0400886F RID: 34927
		[SerializeField]
		public KButton cyclePrevButton;

		// Token: 0x04008870 RID: 34928
		[SerializeField]
		public KButton cycleNextButton;

		// Token: 0x04008871 RID: 34929
		[SerializeField]
		public LocText currentLabel;

		// Token: 0x04008872 RID: 34930
		[NonSerialized]
		private int selectedIndex = -1;

		// Token: 0x04008873 RID: 34931
		[NonSerialized]
		private MinionBrowserScreen.CyclerUI.OnSelectedFn[] cycleOptions;

		// Token: 0x02002657 RID: 9815
		// (Invoke) Token: 0x0600C210 RID: 49680
		public delegate void OnSelectedFn();
	}

	// Token: 0x02001DDF RID: 7647
	public abstract class GridItem : IEquatable<MinionBrowserScreen.GridItem>
	{
		// Token: 0x0600A9E0 RID: 43488
		public abstract string GetName();

		// Token: 0x0600A9E1 RID: 43489
		public abstract Sprite GetIcon();

		// Token: 0x0600A9E2 RID: 43490
		public abstract string GetUniqueId();

		// Token: 0x0600A9E3 RID: 43491
		public abstract Personality GetPersonality();

		// Token: 0x0600A9E4 RID: 43492
		public abstract Option<ClothingOutfitTarget> GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType outfitType);

		// Token: 0x0600A9E5 RID: 43493
		public abstract JoyResponseOutfitTarget GetJoyResponseOutfitTarget();

		// Token: 0x0600A9E6 RID: 43494 RVA: 0x0039FF7C File Offset: 0x0039E17C
		public override bool Equals(object obj)
		{
			MinionBrowserScreen.GridItem gridItem = obj as MinionBrowserScreen.GridItem;
			return gridItem != null && this.Equals(gridItem);
		}

		// Token: 0x0600A9E7 RID: 43495 RVA: 0x0039FF9C File Offset: 0x0039E19C
		public bool Equals(MinionBrowserScreen.GridItem other)
		{
			return this.GetHashCode() == other.GetHashCode();
		}

		// Token: 0x0600A9E8 RID: 43496 RVA: 0x0039FFAC File Offset: 0x0039E1AC
		public override int GetHashCode()
		{
			return Hash.SDBMLower(this.GetUniqueId());
		}

		// Token: 0x0600A9E9 RID: 43497 RVA: 0x0039FFB9 File Offset: 0x0039E1B9
		public override string ToString()
		{
			return this.GetUniqueId();
		}

		// Token: 0x0600A9EA RID: 43498 RVA: 0x0039FFC4 File Offset: 0x0039E1C4
		public static MinionBrowserScreen.GridItem.MinionInstanceTarget Of(GameObject minionInstance)
		{
			MinionIdentity component = minionInstance.GetComponent<MinionIdentity>();
			return new MinionBrowserScreen.GridItem.MinionInstanceTarget
			{
				minionInstance = minionInstance,
				minionIdentity = component,
				personality = Db.Get().Personalities.Get(component.personalityResourceId)
			};
		}

		// Token: 0x0600A9EB RID: 43499 RVA: 0x003A0006 File Offset: 0x0039E206
		public static MinionBrowserScreen.GridItem.PersonalityTarget Of(Personality personality)
		{
			return new MinionBrowserScreen.GridItem.PersonalityTarget
			{
				personality = personality
			};
		}

		// Token: 0x02002658 RID: 9816
		public class MinionInstanceTarget : MinionBrowserScreen.GridItem
		{
			// Token: 0x0600C213 RID: 49683 RVA: 0x003DFE70 File Offset: 0x003DE070
			public override Sprite GetIcon()
			{
				return this.personality.GetMiniIcon();
			}

			// Token: 0x0600C214 RID: 49684 RVA: 0x003DFE7D File Offset: 0x003DE07D
			public override string GetName()
			{
				return this.minionIdentity.GetProperName();
			}

			// Token: 0x0600C215 RID: 49685 RVA: 0x003DFE8C File Offset: 0x003DE08C
			public override string GetUniqueId()
			{
				return "minion_instance_id::" + this.minionInstance.GetInstanceID().ToString();
			}

			// Token: 0x0600C216 RID: 49686 RVA: 0x003DFEB6 File Offset: 0x003DE0B6
			public override Personality GetPersonality()
			{
				return this.personality;
			}

			// Token: 0x0600C217 RID: 49687 RVA: 0x003DFEBE File Offset: 0x003DE0BE
			public override Option<ClothingOutfitTarget> GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType outfitType)
			{
				return ClothingOutfitTarget.FromMinion(outfitType, this.minionInstance);
			}

			// Token: 0x0600C218 RID: 49688 RVA: 0x003DFED1 File Offset: 0x003DE0D1
			public override JoyResponseOutfitTarget GetJoyResponseOutfitTarget()
			{
				return JoyResponseOutfitTarget.FromMinion(this.minionInstance);
			}

			// Token: 0x0400AA66 RID: 43622
			public GameObject minionInstance;

			// Token: 0x0400AA67 RID: 43623
			public MinionIdentity minionIdentity;

			// Token: 0x0400AA68 RID: 43624
			public Personality personality;
		}

		// Token: 0x02002659 RID: 9817
		public class PersonalityTarget : MinionBrowserScreen.GridItem
		{
			// Token: 0x0600C21A RID: 49690 RVA: 0x003DFEE6 File Offset: 0x003DE0E6
			public override Sprite GetIcon()
			{
				return this.personality.GetMiniIcon();
			}

			// Token: 0x0600C21B RID: 49691 RVA: 0x003DFEF3 File Offset: 0x003DE0F3
			public override string GetName()
			{
				return this.personality.Name;
			}

			// Token: 0x0600C21C RID: 49692 RVA: 0x003DFF00 File Offset: 0x003DE100
			public override string GetUniqueId()
			{
				return "personality::" + this.personality.nameStringKey;
			}

			// Token: 0x0600C21D RID: 49693 RVA: 0x003DFF17 File Offset: 0x003DE117
			public override Personality GetPersonality()
			{
				return this.personality;
			}

			// Token: 0x0600C21E RID: 49694 RVA: 0x003DFF1F File Offset: 0x003DE11F
			public override Option<ClothingOutfitTarget> GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType outfitType)
			{
				return ClothingOutfitTarget.TryFromTemplateId(this.personality.GetSelectedTemplateOutfitId(outfitType));
			}

			// Token: 0x0600C21F RID: 49695 RVA: 0x003DFF32 File Offset: 0x003DE132
			public override JoyResponseOutfitTarget GetJoyResponseOutfitTarget()
			{
				return JoyResponseOutfitTarget.FromPersonality(this.personality);
			}

			// Token: 0x0400AA69 RID: 43625
			public Personality personality;
		}
	}
}
