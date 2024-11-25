using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D88 RID: 3464
public class OwnablesSidescreen : SideScreenContent
{
	// Token: 0x06006D05 RID: 27909 RVA: 0x0028FDF8 File Offset: 0x0028DFF8
	private void DefineCategories()
	{
		if (this.categories == null)
		{
			OwnablesSidescreen.Category[] array = new OwnablesSidescreen.Category[2];
			array[0] = new OwnablesSidescreen.Category((IAssignableIdentity assignableIdentity) => (assignableIdentity as MinionIdentity).GetEquipment(), new OwnablesSidescreenCategoryRow.Data(UI.UISIDESCREENS.OWNABLESSIDESCREEN.CATEGORIES.SUITS, new OwnablesSidescreenCategoryRow.AssignableSlotData[]
			{
				new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Suit, new Func<IAssignableIdentity, bool>(this.Always)),
				new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Outfit, new Func<IAssignableIdentity, bool>(this.Always))
			}));
			array[1] = new OwnablesSidescreen.Category((IAssignableIdentity assignableIdentity) => assignableIdentity.GetSoleOwner(), new OwnablesSidescreenCategoryRow.Data(UI.UISIDESCREENS.OWNABLESSIDESCREEN.CATEGORIES.AMENITIES, new OwnablesSidescreenCategoryRow.AssignableSlotData[]
			{
				new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Bed, this.HasAmount("Stamina")),
				new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Toilet, new Func<IAssignableIdentity, bool>(this.Always)),
				new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.MessStation, this.HasAmount("Calories"))
			}));
			this.categories = array;
		}
	}

	// Token: 0x06006D06 RID: 27910 RVA: 0x0028FF5D File Offset: 0x0028E15D
	private bool Always(IAssignableIdentity identity)
	{
		return true;
	}

	// Token: 0x06006D07 RID: 27911 RVA: 0x0028FF60 File Offset: 0x0028E160
	private Func<IAssignableIdentity, bool> HasAmount(string amountID)
	{
		return delegate(IAssignableIdentity identity)
		{
			if (identity == null)
			{
				return false;
			}
			GameObject targetGameObject = identity.GetOwners()[0].GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
			return Db.Get().Amounts.Get(amountID).Lookup(targetGameObject) != null;
		};
	}

	// Token: 0x06006D08 RID: 27912 RVA: 0x0028FF79 File Offset: 0x0028E179
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06006D09 RID: 27913 RVA: 0x0028FF81 File Offset: 0x0028E181
	private void ActivateSecondSidescreen(AssignableSlotInstance slot)
	{
		((OwnablesSecondSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.selectedSlotScreenPrefab, slot.slot.Name)).SetSlot(slot);
		if (slot != null && this.OnSlotInstanceSelected != null)
		{
			this.OnSlotInstanceSelected(slot);
		}
	}

	// Token: 0x06006D0A RID: 27914 RVA: 0x0028FFC0 File Offset: 0x0028E1C0
	private void DeactivateSecondScreen()
	{
		DetailsScreen.Instance.ClearSecondarySideScreen();
	}

	// Token: 0x06006D0B RID: 27915 RVA: 0x0028FFCC File Offset: 0x0028E1CC
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.UnsubscribeFromLastTarget();
		this.lastSelectedSlot = null;
		this.DefineCategories();
		this.CreateCategoryRows();
		this.DeactivateSecondScreen();
		this.RefreshSelectedStatusOnRows();
		IAssignableIdentity component = target.GetComponent<IAssignableIdentity>();
		for (int i = 0; i < this.categoryRows.Length; i++)
		{
			Assignables owner = this.categories[i].getAssignablesFn(component);
			this.categoryRows[i].SetOwner(owner);
		}
		this.titleSection.SetActive(target.GetComponent<MinionIdentity>().model == BionicMinionConfig.MODEL);
		MinionIdentity minionIdentity = component as MinionIdentity;
		if (minionIdentity != null)
		{
			this.lastTarget = minionIdentity;
			this.minionDestroyedCallbackIDX = minionIdentity.gameObject.Subscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
		}
	}

	// Token: 0x06006D0C RID: 27916 RVA: 0x0029009E File Offset: 0x0028E29E
	private void OnTargetDestroyed(object o)
	{
		this.ClearTarget();
	}

	// Token: 0x06006D0D RID: 27917 RVA: 0x002900A8 File Offset: 0x0028E2A8
	public override void ClearTarget()
	{
		base.ClearTarget();
		this.lastSelectedSlot = null;
		this.RefreshSelectedStatusOnRows();
		for (int i = 0; i < this.categoryRows.Length; i++)
		{
			this.categoryRows[i].SetOwner(null);
		}
		this.DeactivateSecondScreen();
		this.UnsubscribeFromLastTarget();
	}

	// Token: 0x06006D0E RID: 27918 RVA: 0x002900F8 File Offset: 0x0028E2F8
	private void CreateCategoryRows()
	{
		if (this.categoryRows == null)
		{
			this.originalCategoryRow.gameObject.SetActive(false);
			this.categoryRows = new OwnablesSidescreenCategoryRow[this.categories.Length];
			for (int i = 0; i < this.categories.Length; i++)
			{
				OwnablesSidescreenCategoryRow.Data data = this.categories[i].data;
				OwnablesSidescreenCategoryRow component = Util.KInstantiateUI(this.originalCategoryRow.gameObject, this.originalCategoryRow.transform.parent.gameObject, false).GetComponent<OwnablesSidescreenCategoryRow>();
				OwnablesSidescreenCategoryRow ownablesSidescreenCategoryRow = component;
				ownablesSidescreenCategoryRow.OnSlotRowClicked = (Action<OwnablesSidescreenItemRow>)Delegate.Combine(ownablesSidescreenCategoryRow.OnSlotRowClicked, new Action<OwnablesSidescreenItemRow>(this.OnSlotRowClicked));
				component.gameObject.SetActive(true);
				component.SetCategoryData(data);
				this.categoryRows[i] = component;
			}
			this.RefreshSelectedStatusOnRows();
		}
	}

	// Token: 0x06006D0F RID: 27919 RVA: 0x002901CF File Offset: 0x0028E3CF
	private void OnSlotRowClicked(OwnablesSidescreenItemRow slotRow)
	{
		if (slotRow.IsLocked || slotRow.SlotInstance == this.lastSelectedSlot)
		{
			this.SetSelectedSlot(null);
			return;
		}
		this.SetSelectedSlot(slotRow.SlotInstance);
	}

	// Token: 0x06006D10 RID: 27920 RVA: 0x002901FC File Offset: 0x0028E3FC
	public void RefreshSelectedStatusOnRows()
	{
		if (this.categoryRows == null)
		{
			return;
		}
		for (int i = 0; i < this.categoryRows.Length; i++)
		{
			this.categoryRows[i].SetSelectedRow_VisualsOnly(this.lastSelectedSlot);
		}
	}

	// Token: 0x06006D11 RID: 27921 RVA: 0x00290238 File Offset: 0x0028E438
	public void SetSelectedSlot(AssignableSlotInstance slot)
	{
		this.lastSelectedSlot = slot;
		if (slot != null)
		{
			this.ActivateSecondSidescreen(slot);
		}
		else
		{
			this.DeactivateSecondScreen();
		}
		this.RefreshSelectedStatusOnRows();
	}

	// Token: 0x06006D12 RID: 27922 RVA: 0x0029025C File Offset: 0x0028E45C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.categoryRows != null)
		{
			for (int i = 0; i < this.categoryRows.Length; i++)
			{
				if (this.categoryRows[i] != null)
				{
					this.categoryRows[i].SetOwner(null);
				}
			}
		}
		this.UnsubscribeFromLastTarget();
	}

	// Token: 0x06006D13 RID: 27923 RVA: 0x002902AE File Offset: 0x0028E4AE
	private void UnsubscribeFromLastTarget()
	{
		if (this.lastTarget != null && this.minionDestroyedCallbackIDX != -1)
		{
			this.lastTarget.Unsubscribe(this.minionDestroyedCallbackIDX);
		}
		this.minionDestroyedCallbackIDX = -1;
		this.lastTarget = null;
	}

	// Token: 0x06006D14 RID: 27924 RVA: 0x002902E6 File Offset: 0x0028E4E6
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IAssignableIdentity>() != null;
	}

	// Token: 0x06006D15 RID: 27925 RVA: 0x002902F1 File Offset: 0x0028E4F1
	public void OnValidate()
	{
	}

	// Token: 0x06006D16 RID: 27926 RVA: 0x002902F3 File Offset: 0x0028E4F3
	private void SetScrollBarVisibility(bool isVisible)
	{
		this.scrollbarSection.gameObject.SetActive(isVisible);
		this.mainLayoutGroup.padding.right = (isVisible ? 20 : 0);
		this.scrollRect.enabled = isVisible;
	}

	// Token: 0x04004A56 RID: 19030
	public OwnablesSecondSideScreen selectedSlotScreenPrefab;

	// Token: 0x04004A57 RID: 19031
	public OwnablesSidescreenCategoryRow originalCategoryRow;

	// Token: 0x04004A58 RID: 19032
	[Header("Editor Settings")]
	public bool usingSlider = true;

	// Token: 0x04004A59 RID: 19033
	public GameObject titleSection;

	// Token: 0x04004A5A RID: 19034
	public GameObject scrollbarSection;

	// Token: 0x04004A5B RID: 19035
	public VerticalLayoutGroup mainLayoutGroup;

	// Token: 0x04004A5C RID: 19036
	public KScrollRect scrollRect;

	// Token: 0x04004A5D RID: 19037
	private OwnablesSidescreenCategoryRow[] categoryRows;

	// Token: 0x04004A5E RID: 19038
	private AssignableSlotInstance lastSelectedSlot;

	// Token: 0x04004A5F RID: 19039
	private OwnablesSidescreen.Category[] categories;

	// Token: 0x04004A60 RID: 19040
	public Action<AssignableSlotInstance> OnSlotInstanceSelected;

	// Token: 0x04004A61 RID: 19041
	private MinionIdentity lastTarget;

	// Token: 0x04004A62 RID: 19042
	private int minionDestroyedCallbackIDX = -1;

	// Token: 0x02001EA9 RID: 7849
	public struct Category
	{
		// Token: 0x0600AC0F RID: 44047 RVA: 0x003A6748 File Offset: 0x003A4948
		public Category(Func<IAssignableIdentity, Assignables> getAssignablesFn, OwnablesSidescreenCategoryRow.Data categoryData)
		{
			this.getAssignablesFn = getAssignablesFn;
			this.data = categoryData;
		}

		// Token: 0x04008B2B RID: 35627
		public Func<IAssignableIdentity, Assignables> getAssignablesFn;

		// Token: 0x04008B2C RID: 35628
		public OwnablesSidescreenCategoryRow.Data data;
	}
}
