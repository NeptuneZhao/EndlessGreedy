using System;
using System.Collections.Generic;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BEE RID: 3054
public class ClusterCategorySelectionScreen : NewGameFlowScreen
{
	// Token: 0x06005D05 RID: 23813 RVA: 0x0022313C File Offset: 0x0022133C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.closeButton.onClick += base.NavigateBackward;
		int num = 0;
		using (Dictionary<string, ClusterLayout>.ValueCollection.Enumerator enumerator = SettingsCache.clusterLayouts.clusterCache.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.clusterCategory == ClusterLayout.ClusterCategory.Special)
				{
					num++;
				}
			}
		}
		if (num > 0)
		{
			this.eventStyle.button.gameObject.SetActive(true);
			this.eventStyle.Init(this.descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.EVENT_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.EVENT_TITLE);
			MultiToggle button = this.eventStyle.button;
			button.onClick = (System.Action)Delegate.Combine(button.onClick, new System.Action(delegate()
			{
				this.OnClickOption(ClusterLayout.ClusterCategory.Special);
			}));
		}
		if (DlcManager.IsExpansion1Active())
		{
			this.classicStyle.button.gameObject.SetActive(true);
			this.classicStyle.Init(this.descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.CLASSIC_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.CLASSIC_TITLE);
			MultiToggle button2 = this.classicStyle.button;
			button2.onClick = (System.Action)Delegate.Combine(button2.onClick, new System.Action(delegate()
			{
				this.OnClickOption(ClusterLayout.ClusterCategory.SpacedOutVanillaStyle);
			}));
			this.spacedOutStyle.button.gameObject.SetActive(true);
			this.spacedOutStyle.Init(this.descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.SPACEDOUT_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.SPACEDOUT_TITLE);
			MultiToggle button3 = this.spacedOutStyle.button;
			button3.onClick = (System.Action)Delegate.Combine(button3.onClick, new System.Action(delegate()
			{
				this.OnClickOption(ClusterLayout.ClusterCategory.SpacedOutStyle);
			}));
			this.panel.sizeDelta = ((num > 0) ? new Vector2(622f, this.panel.sizeDelta.y) : new Vector2(480f, this.panel.sizeDelta.y));
			return;
		}
		this.vanillaStyle.button.gameObject.SetActive(true);
		this.vanillaStyle.Init(this.descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.VANILLA_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.VANILLA_TITLE);
		MultiToggle button4 = this.vanillaStyle.button;
		button4.onClick = (System.Action)Delegate.Combine(button4.onClick, new System.Action(delegate()
		{
			this.OnClickOption(ClusterLayout.ClusterCategory.Vanilla);
		}));
		this.panel.sizeDelta = new Vector2(480f, this.panel.sizeDelta.y);
		this.eventStyle.kanim.Play("lab_asteroid_standard", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06005D06 RID: 23814 RVA: 0x002233F8 File Offset: 0x002215F8
	private void OnClickOption(ClusterLayout.ClusterCategory clusterCategory)
	{
		this.Deactivate();
		DestinationSelectPanel.ChosenClusterCategorySetting = (int)clusterCategory;
		base.NavigateForward();
	}

	// Token: 0x04003E47 RID: 15943
	public ClusterCategorySelectionScreen.ButtonConfig vanillaStyle;

	// Token: 0x04003E48 RID: 15944
	public ClusterCategorySelectionScreen.ButtonConfig classicStyle;

	// Token: 0x04003E49 RID: 15945
	public ClusterCategorySelectionScreen.ButtonConfig spacedOutStyle;

	// Token: 0x04003E4A RID: 15946
	public ClusterCategorySelectionScreen.ButtonConfig eventStyle;

	// Token: 0x04003E4B RID: 15947
	[SerializeField]
	private LocText descriptionArea;

	// Token: 0x04003E4C RID: 15948
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003E4D RID: 15949
	[SerializeField]
	private RectTransform panel;

	// Token: 0x02001CD3 RID: 7379
	[Serializable]
	public class ButtonConfig
	{
		// Token: 0x0600A709 RID: 42761 RVA: 0x00399734 File Offset: 0x00397934
		public void Init(LocText descriptionArea, string hoverDescriptionText, string headerText)
		{
			this.descriptionArea = descriptionArea;
			this.hoverDescriptionText = hoverDescriptionText;
			this.headerLabel.SetText(headerText);
			MultiToggle multiToggle = this.button;
			multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(this.OnHoverEnter));
			MultiToggle multiToggle2 = this.button;
			multiToggle2.onExit = (System.Action)Delegate.Combine(multiToggle2.onExit, new System.Action(this.OnHoverExit));
			HierarchyReferences component = this.button.GetComponent<HierarchyReferences>();
			this.headerImage = component.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
			this.selectionFrame = component.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
		}

		// Token: 0x0600A70A RID: 42762 RVA: 0x003997E4 File Offset: 0x003979E4
		private void OnHoverEnter()
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
			this.selectionFrame.SetAlpha(1f);
			this.headerImage.color = new Color(0.7019608f, 0.3647059f, 0.53333336f, 1f);
			this.descriptionArea.text = this.hoverDescriptionText;
		}

		// Token: 0x0600A70B RID: 42763 RVA: 0x00399848 File Offset: 0x00397A48
		private void OnHoverExit()
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
			this.selectionFrame.SetAlpha(0f);
			this.headerImage.color = new Color(0.30980393f, 0.34117648f, 0.38431373f, 1f);
			this.descriptionArea.text = UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.BLANK_DESC;
		}

		// Token: 0x04008534 RID: 34100
		public MultiToggle button;

		// Token: 0x04008535 RID: 34101
		public Image headerImage;

		// Token: 0x04008536 RID: 34102
		public LocText headerLabel;

		// Token: 0x04008537 RID: 34103
		public Image selectionFrame;

		// Token: 0x04008538 RID: 34104
		public KAnimControllerBase kanim;

		// Token: 0x04008539 RID: 34105
		private string hoverDescriptionText;

		// Token: 0x0400853A RID: 34106
		private LocText descriptionArea;
	}
}
