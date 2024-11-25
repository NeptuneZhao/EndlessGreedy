using System;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000DDC RID: 3548
public class TopLeftControlScreen : KScreen
{
	// Token: 0x060070BC RID: 28860 RVA: 0x002AAE43 File Offset: 0x002A9043
	public static void DestroyInstance()
	{
		TopLeftControlScreen.Instance = null;
	}

	// Token: 0x060070BD RID: 28861 RVA: 0x002AAE4C File Offset: 0x002A904C
	protected override void OnActivate()
	{
		base.OnActivate();
		TopLeftControlScreen.Instance = this;
		this.RefreshName();
		KInputManager.InputChange.AddListener(new UnityAction(this.ResetToolTip));
		this.UpdateSandboxToggleState();
		MultiToggle multiToggle = this.sandboxToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnClickSandboxToggle));
		MultiToggle multiToggle2 = this.kleiItemDropButton;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(this.OnClickKleiItemDropButton));
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(new System.Action(this.RefreshKleiItemDropButton));
		this.RefreshKleiItemDropButton();
		Game.Instance.Subscribe(-1948169901, delegate(object data)
		{
			this.UpdateSandboxToggleState();
		});
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.secondaryRow);
	}

	// Token: 0x060070BE RID: 28862 RVA: 0x002AAF19 File Offset: 0x002A9119
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.ResetToolTip));
		base.OnForcedCleanUp();
	}

	// Token: 0x060070BF RID: 28863 RVA: 0x002AAF37 File Offset: 0x002A9137
	public void RefreshName()
	{
		if (SaveGame.Instance != null)
		{
			this.locText.text = SaveGame.Instance.BaseName;
		}
	}

	// Token: 0x060070C0 RID: 28864 RVA: 0x002AAF5C File Offset: 0x002A915C
	public void ResetToolTip()
	{
		if (this.CheckSandboxModeLocked())
		{
			this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_LOCKED, global::Action.ToggleSandboxTools));
			return;
		}
		this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_UNLOCKED, global::Action.ToggleSandboxTools));
	}

	// Token: 0x060070C1 RID: 28865 RVA: 0x002AAFBC File Offset: 0x002A91BC
	public void UpdateSandboxToggleState()
	{
		if (this.CheckSandboxModeLocked())
		{
			this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_LOCKED, global::Action.ToggleSandboxTools));
			this.sandboxToggle.ChangeState(0);
		}
		else
		{
			this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_UNLOCKED, global::Action.ToggleSandboxTools));
			this.sandboxToggle.ChangeState(Game.Instance.SandboxModeActive ? 2 : 1);
		}
		this.sandboxToggle.gameObject.SetActive(SaveGame.Instance.sandboxEnabled);
	}

	// Token: 0x060070C2 RID: 28866 RVA: 0x002AB05C File Offset: 0x002A925C
	private void OnClickSandboxToggle()
	{
		if (this.CheckSandboxModeLocked())
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
		else
		{
			Game.Instance.SandboxModeActive = !Game.Instance.SandboxModeActive;
			KMonoBehaviour.PlaySound(Game.Instance.SandboxModeActive ? GlobalAssets.GetSound("SandboxTool_Toggle_On", false) : GlobalAssets.GetSound("SandboxTool_Toggle_Off", false));
		}
		this.UpdateSandboxToggleState();
	}

	// Token: 0x060070C3 RID: 28867 RVA: 0x002AB0CC File Offset: 0x002A92CC
	private void RefreshKleiItemDropButton()
	{
		if (!KleiItemDropScreen.HasItemsToShow())
		{
			this.kleiItemDropButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.ITEM_DROP_SCREEN.IN_GAME_BUTTON.TOOLTIP_ERROR_NO_ITEMS);
			this.kleiItemDropButton.ChangeState(1);
			return;
		}
		this.kleiItemDropButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.ITEM_DROP_SCREEN.IN_GAME_BUTTON.TOOLTIP_ITEMS_AVAILABLE);
		this.kleiItemDropButton.ChangeState(2);
	}

	// Token: 0x060070C4 RID: 28868 RVA: 0x002AB12D File Offset: 0x002A932D
	private void OnClickKleiItemDropButton()
	{
		this.RefreshKleiItemDropButton();
		if (!KleiItemDropScreen.HasItemsToShow())
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
			return;
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
		UnityEngine.Object.FindObjectOfType<KleiItemDropScreen>(true).Show(true);
	}

	// Token: 0x060070C5 RID: 28869 RVA: 0x002AB169 File Offset: 0x002A9369
	private bool CheckSandboxModeLocked()
	{
		return !SaveGame.Instance.sandboxEnabled;
	}

	// Token: 0x04004D84 RID: 19844
	public static TopLeftControlScreen Instance;

	// Token: 0x04004D85 RID: 19845
	[SerializeField]
	private MultiToggle sandboxToggle;

	// Token: 0x04004D86 RID: 19846
	[SerializeField]
	private MultiToggle kleiItemDropButton;

	// Token: 0x04004D87 RID: 19847
	[SerializeField]
	private LocText locText;

	// Token: 0x04004D88 RID: 19848
	[SerializeField]
	private RectTransform secondaryRow;

	// Token: 0x02001EEA RID: 7914
	private enum MultiToggleState
	{
		// Token: 0x04008BFD RID: 35837
		Disabled,
		// Token: 0x04008BFE RID: 35838
		Off,
		// Token: 0x04008BFF RID: 35839
		On
	}
}
