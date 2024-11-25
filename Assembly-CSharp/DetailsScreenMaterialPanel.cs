using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C3C RID: 3132
public class DetailsScreenMaterialPanel : TargetScreen
{
	// Token: 0x06006039 RID: 24633 RVA: 0x0023C491 File Offset: 0x0023A691
	public override bool IsValidForTarget(GameObject target)
	{
		return true;
	}

	// Token: 0x0600603A RID: 24634 RVA: 0x0023C494 File Offset: 0x0023A694
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.openChangeMaterialPanelButton.onClick += delegate()
		{
			this.OpenMaterialSelectionPanel();
			this.RefreshMaterialSelectionPanel();
			this.RefreshOrderChangeMaterialButton();
		};
	}

	// Token: 0x0600603B RID: 24635 RVA: 0x0023C4B4 File Offset: 0x0023A6B4
	public override void SetTarget(GameObject target)
	{
		if (this.selectedTarget != null)
		{
			this.selectedTarget.Unsubscribe(this.subHandle);
		}
		this.building = null;
		base.SetTarget(target);
		if (target == null)
		{
			return;
		}
		this.materialSelectionPanel.gameObject.SetActive(false);
		this.orderChangeMaterialButton.ClearOnClick();
		this.orderChangeMaterialButton.isInteractable = false;
		this.CloseMaterialSelectionPanel();
		this.building = this.selectedTarget.GetComponent<Building>();
		bool flag = Db.Get().TechItems.IsTechItemComplete(this.building.Def.PrefabID) || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
		this.openChangeMaterialPanelButton.isInteractable = (target.GetComponent<Reconstructable>() != null && target.GetComponent<Reconstructable>().AllowReconstruct && flag);
		this.openChangeMaterialPanelButton.GetComponent<ToolTip>().SetSimpleTooltip(flag ? "" : string.Format(UI.PRODUCTINFO_REQUIRESRESEARCHDESC, Db.Get().TechItems.GetTechFromItemID(this.building.Def.PrefabID).Name));
		this.Refresh(null);
		this.subHandle = target.Subscribe(954267658, new Action<object>(this.RefreshOrderChangeMaterialButton));
		Game.Instance.Subscribe(1980521255, new Action<object>(this.Refresh));
	}

	// Token: 0x0600603C RID: 24636 RVA: 0x0023C624 File Offset: 0x0023A824
	private void OpenMaterialSelectionPanel()
	{
		this.openChangeMaterialPanelButton.gameObject.SetActive(false);
		this.materialSelectionPanel.gameObject.SetActive(true);
		this.RefreshMaterialSelectionPanel();
		if (this.selectedTarget != null && this.building != null)
		{
			this.materialSelectionPanel.SelectSourcesMaterials(this.building);
		}
	}

	// Token: 0x0600603D RID: 24637 RVA: 0x0023C686 File Offset: 0x0023A886
	private void CloseMaterialSelectionPanel()
	{
		this.currentMaterialDescriptionRow.gameObject.SetActive(true);
		this.openChangeMaterialPanelButton.gameObject.SetActive(true);
		this.materialSelectionPanel.gameObject.SetActive(false);
	}

	// Token: 0x0600603E RID: 24638 RVA: 0x0023C6BB File Offset: 0x0023A8BB
	public override void OnDeselectTarget(GameObject target)
	{
		base.OnDeselectTarget(target);
		this.Refresh(null);
	}

	// Token: 0x0600603F RID: 24639 RVA: 0x0023C6CB File Offset: 0x0023A8CB
	private void Refresh(object data = null)
	{
		this.RefreshCurrentMaterial();
		this.RefreshMaterialSelectionPanel();
	}

	// Token: 0x06006040 RID: 24640 RVA: 0x0023C6DC File Offset: 0x0023A8DC
	private void RefreshCurrentMaterial()
	{
		if (this.selectedTarget == null)
		{
			return;
		}
		CellSelectionObject component = this.selectedTarget.GetComponent<CellSelectionObject>();
		Element element = (component == null) ? this.selectedTarget.GetComponent<PrimaryElement>().Element : component.element;
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(element, "ui", false);
		this.currentMaterialIcon.sprite = uisprite.first;
		this.currentMaterialIcon.color = uisprite.second;
		if (component == null)
		{
			this.currentMaterialLabel.SetText(element.name + " x " + GameUtil.GetFormattedMass(this.selectedTarget.GetComponent<PrimaryElement>().Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		}
		else
		{
			this.currentMaterialLabel.SetText(element.name);
		}
		this.currentMaterialDescription.SetText(element.description);
		List<Descriptor> materialDescriptors = GameUtil.GetMaterialDescriptors(element);
		if (materialDescriptors.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(ELEMENTS.MATERIAL_MODIFIERS.EFFECTS_HEADER, ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.EFFECTS_HEADER, Descriptor.DescriptorType.Effect);
			materialDescriptors.Insert(0, item);
			this.descriptorPanel.gameObject.SetActive(true);
			this.descriptorPanel.SetDescriptors(materialDescriptors);
			return;
		}
		this.descriptorPanel.gameObject.SetActive(false);
	}

	// Token: 0x06006041 RID: 24641 RVA: 0x0023C828 File Offset: 0x0023AA28
	private void RefreshMaterialSelectionPanel()
	{
		if (this.selectedTarget == null)
		{
			return;
		}
		this.materialSelectionPanel.ClearSelectActions();
		if (!(this.building == null) && !(this.building is BuildingUnderConstruction))
		{
			this.materialSelectionPanel.ConfigureScreen(this.building.Def.CraftRecipe, (BuildingDef data) => true, (BuildingDef data) => "");
			this.materialSelectionPanel.AddSelectAction(new MaterialSelector.SelectMaterialActions(this.RefreshOrderChangeMaterialButton));
			Reconstructable component = this.selectedTarget.GetComponent<Reconstructable>();
			if (component != null && component.ReconstructRequested)
			{
				if (!this.materialSelectionPanel.gameObject.activeSelf)
				{
					this.OpenMaterialSelectionPanel();
					this.materialSelectionPanel.RefreshSelectors();
				}
				this.materialSelectionPanel.ForceSelectPrimaryTag(component.PrimarySelectedElementTag);
			}
		}
		this.confirmChangeRow.transform.SetAsLastSibling();
	}

	// Token: 0x06006042 RID: 24642 RVA: 0x0023C941 File Offset: 0x0023AB41
	private void RefreshOrderChangeMaterialButton()
	{
		this.RefreshOrderChangeMaterialButton(null);
	}

	// Token: 0x06006043 RID: 24643 RVA: 0x0023C94C File Offset: 0x0023AB4C
	private void RefreshOrderChangeMaterialButton(object data = null)
	{
		if (this.selectedTarget == null)
		{
			return;
		}
		Reconstructable reconstructable = this.selectedTarget.GetComponent<Reconstructable>();
		bool flag = this.materialSelectionPanel.CurrentSelectedElement != null;
		this.orderChangeMaterialButton.isInteractable = (flag && this.building.GetComponent<PrimaryElement>().Element.tag != this.materialSelectionPanel.CurrentSelectedElement);
		this.orderChangeMaterialButton.ClearOnClick();
		this.orderChangeMaterialButton.onClick += delegate()
		{
			reconstructable.RequestReconstruct(this.materialSelectionPanel.CurrentSelectedElement);
			this.RefreshOrderChangeMaterialButton();
		};
		this.orderChangeMaterialButton.GetComponentInChildren<LocText>().SetText(reconstructable.ReconstructRequested ? UI.USERMENUACTIONS.RECONSTRUCT.CANCEL_RECONSTRUCT : UI.USERMENUACTIONS.RECONSTRUCT.REQUEST_RECONSTRUCT);
		this.orderChangeMaterialButton.GetComponent<ToolTip>().SetSimpleTooltip(reconstructable.ReconstructRequested ? UI.USERMENUACTIONS.RECONSTRUCT.CANCEL_RECONSTRUCT_TOOLTIP : UI.USERMENUACTIONS.RECONSTRUCT.REQUEST_RECONSTRUCT_TOOLTIP);
	}

	// Token: 0x040040EE RID: 16622
	[Header("Current Material")]
	[SerializeField]
	private Image currentMaterialIcon;

	// Token: 0x040040EF RID: 16623
	[SerializeField]
	private RectTransform currentMaterialDescriptionRow;

	// Token: 0x040040F0 RID: 16624
	[SerializeField]
	private LocText currentMaterialLabel;

	// Token: 0x040040F1 RID: 16625
	[SerializeField]
	private LocText currentMaterialDescription;

	// Token: 0x040040F2 RID: 16626
	[SerializeField]
	private DescriptorPanel descriptorPanel;

	// Token: 0x040040F3 RID: 16627
	[Header("Change Material")]
	[SerializeField]
	private MaterialSelectionPanel materialSelectionPanel;

	// Token: 0x040040F4 RID: 16628
	[SerializeField]
	private RectTransform confirmChangeRow;

	// Token: 0x040040F5 RID: 16629
	[SerializeField]
	private KButton orderChangeMaterialButton;

	// Token: 0x040040F6 RID: 16630
	[SerializeField]
	private KButton openChangeMaterialPanelButton;

	// Token: 0x040040F7 RID: 16631
	private int subHandle = -1;

	// Token: 0x040040F8 RID: 16632
	private Building building;
}
