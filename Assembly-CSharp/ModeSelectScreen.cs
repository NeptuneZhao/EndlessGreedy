using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CE7 RID: 3303
public class ModeSelectScreen : NewGameFlowScreen
{
	// Token: 0x0600665B RID: 26203 RVA: 0x00263C7F File Offset: 0x00261E7F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.LoadWorldAndClusterData();
	}

	// Token: 0x0600665C RID: 26204 RVA: 0x00263C90 File Offset: 0x00261E90
	protected override void OnSpawn()
	{
		base.OnSpawn();
		HierarchyReferences component = this.survivalButton.GetComponent<HierarchyReferences>();
		this.survivalButtonHeader = component.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
		this.survivalButtonSelectionFrame = component.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
		MultiToggle multiToggle = this.survivalButton;
		multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(this.OnHoverEnterSurvival));
		MultiToggle multiToggle2 = this.survivalButton;
		multiToggle2.onExit = (System.Action)Delegate.Combine(multiToggle2.onExit, new System.Action(this.OnHoverExitSurvival));
		MultiToggle multiToggle3 = this.survivalButton;
		multiToggle3.onClick = (System.Action)Delegate.Combine(multiToggle3.onClick, new System.Action(this.OnClickSurvival));
		HierarchyReferences component2 = this.nosweatButton.GetComponent<HierarchyReferences>();
		this.nosweatButtonHeader = component2.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
		this.nosweatButtonSelectionFrame = component2.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
		MultiToggle multiToggle4 = this.nosweatButton;
		multiToggle4.onEnter = (System.Action)Delegate.Combine(multiToggle4.onEnter, new System.Action(this.OnHoverEnterNosweat));
		MultiToggle multiToggle5 = this.nosweatButton;
		multiToggle5.onExit = (System.Action)Delegate.Combine(multiToggle5.onExit, new System.Action(this.OnHoverExitNosweat));
		MultiToggle multiToggle6 = this.nosweatButton;
		multiToggle6.onClick = (System.Action)Delegate.Combine(multiToggle6.onClick, new System.Action(this.OnClickNosweat));
		this.closeButton.onClick += base.NavigateBackward;
	}

	// Token: 0x0600665D RID: 26205 RVA: 0x00263E14 File Offset: 0x00262014
	private void OnHoverEnterSurvival()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
		this.survivalButtonSelectionFrame.SetAlpha(1f);
		this.survivalButtonHeader.color = new Color(0.7019608f, 0.3647059f, 0.53333336f, 1f);
		this.descriptionArea.text = UI.FRONTEND.MODESELECTSCREEN.SURVIVAL_DESC;
	}

	// Token: 0x0600665E RID: 26206 RVA: 0x00263E7C File Offset: 0x0026207C
	private void OnHoverExitSurvival()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
		this.survivalButtonSelectionFrame.SetAlpha(0f);
		this.survivalButtonHeader.color = new Color(0.30980393f, 0.34117648f, 0.38431373f, 1f);
		this.descriptionArea.text = UI.FRONTEND.MODESELECTSCREEN.BLANK_DESC;
	}

	// Token: 0x0600665F RID: 26207 RVA: 0x00263EE2 File Offset: 0x002620E2
	private void OnClickSurvival()
	{
		this.Deactivate();
		CustomGameSettings.Instance.SetSurvivalDefaults();
		base.NavigateForward();
	}

	// Token: 0x06006660 RID: 26208 RVA: 0x00263EFA File Offset: 0x002620FA
	private void LoadWorldAndClusterData()
	{
		if (ModeSelectScreen.dataLoaded)
		{
			return;
		}
		CustomGameSettings.Instance.LoadClusters();
		Global.Instance.modManager.Report(base.gameObject);
		ModeSelectScreen.dataLoaded = true;
	}

	// Token: 0x06006661 RID: 26209 RVA: 0x00263F2C File Offset: 0x0026212C
	private void OnHoverEnterNosweat()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
		this.nosweatButtonSelectionFrame.SetAlpha(1f);
		this.nosweatButtonHeader.color = new Color(0.7019608f, 0.3647059f, 0.53333336f, 1f);
		this.descriptionArea.text = UI.FRONTEND.MODESELECTSCREEN.NOSWEAT_DESC;
	}

	// Token: 0x06006662 RID: 26210 RVA: 0x00263F94 File Offset: 0x00262194
	private void OnHoverExitNosweat()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
		this.nosweatButtonSelectionFrame.SetAlpha(0f);
		this.nosweatButtonHeader.color = new Color(0.30980393f, 0.34117648f, 0.38431373f, 1f);
		this.descriptionArea.text = UI.FRONTEND.MODESELECTSCREEN.BLANK_DESC;
	}

	// Token: 0x06006663 RID: 26211 RVA: 0x00263FFA File Offset: 0x002621FA
	private void OnClickNosweat()
	{
		this.Deactivate();
		CustomGameSettings.Instance.SetNosweatDefaults();
		base.NavigateForward();
	}

	// Token: 0x04004504 RID: 17668
	[SerializeField]
	private MultiToggle nosweatButton;

	// Token: 0x04004505 RID: 17669
	private Image nosweatButtonHeader;

	// Token: 0x04004506 RID: 17670
	private Image nosweatButtonSelectionFrame;

	// Token: 0x04004507 RID: 17671
	[SerializeField]
	private MultiToggle survivalButton;

	// Token: 0x04004508 RID: 17672
	private Image survivalButtonHeader;

	// Token: 0x04004509 RID: 17673
	private Image survivalButtonSelectionFrame;

	// Token: 0x0400450A RID: 17674
	[SerializeField]
	private LocText descriptionArea;

	// Token: 0x0400450B RID: 17675
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400450C RID: 17676
	[SerializeField]
	private KBatchedAnimController nosweatAnim;

	// Token: 0x0400450D RID: 17677
	[SerializeField]
	private KBatchedAnimController survivalAnim;

	// Token: 0x0400450E RID: 17678
	private static bool dataLoaded;
}
