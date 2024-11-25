using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C6B RID: 3179
public class InputBindingsScreen : KModalScreen
{
	// Token: 0x06006178 RID: 24952 RVA: 0x0024470F File Offset: 0x0024290F
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06006179 RID: 24953 RVA: 0x00244712 File Offset: 0x00242912
	private bool IsKeyDown(KeyCode key_code)
	{
		return Input.GetKey(key_code) || Input.GetKeyDown(key_code);
	}

	// Token: 0x0600617A RID: 24954 RVA: 0x00244724 File Offset: 0x00242924
	private string GetModifierString(Modifier modifiers)
	{
		string text = "";
		foreach (object obj in Enum.GetValues(typeof(Modifier)))
		{
			Modifier modifier = (Modifier)obj;
			if ((modifiers & modifier) != Modifier.None)
			{
				text = text + " + " + modifier.ToString();
			}
		}
		return text;
	}

	// Token: 0x0600617B RID: 24955 RVA: 0x002447A4 File Offset: 0x002429A4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.entryPrefab.SetActive(false);
		this.prevScreenButton.onClick += this.OnPrevScreen;
		this.nextScreenButton.onClick += this.OnNextScreen;
	}

	// Token: 0x0600617C RID: 24956 RVA: 0x002447F4 File Offset: 0x002429F4
	protected override void OnActivate()
	{
		this.CollectScreens();
		string text = this.screens[this.activeScreen];
		string key = "STRINGS.INPUT_BINDINGS." + text.ToUpper() + ".NAME";
		this.screenTitle.text = Strings.Get(key);
		this.closeButton.onClick += this.OnBack;
		this.backButton.onClick += this.OnBack;
		this.resetButton.onClick += this.OnReset;
		this.BuildDisplay();
	}

	// Token: 0x0600617D RID: 24957 RVA: 0x00244890 File Offset: 0x00242A90
	private void CollectScreens()
	{
		this.screens.Clear();
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry = GameInputMapping.KeyBindings[i];
			if (bindingEntry.mGroup != null && bindingEntry.mRebindable && !this.screens.Contains(bindingEntry.mGroup) && DlcManager.IsAllContentSubscribed(bindingEntry.dlcIds))
			{
				if (bindingEntry.mGroup == "Root")
				{
					this.activeScreen = this.screens.Count;
				}
				this.screens.Add(bindingEntry.mGroup);
			}
		}
	}

	// Token: 0x0600617E RID: 24958 RVA: 0x0024492A File Offset: 0x00242B2A
	protected override void OnDeactivate()
	{
		GameInputMapping.SaveBindings();
		this.DestroyDisplay();
	}

	// Token: 0x0600617F RID: 24959 RVA: 0x00244937 File Offset: 0x00242B37
	private LocString GetActionString(global::Action action)
	{
		return null;
	}

	// Token: 0x06006180 RID: 24960 RVA: 0x0024493C File Offset: 0x00242B3C
	private string GetBindingText(BindingEntry binding)
	{
		string text = GameUtil.GetKeycodeLocalized(binding.mKeyCode);
		if (binding.mKeyCode != KKeyCode.LeftAlt && binding.mKeyCode != KKeyCode.RightAlt && binding.mKeyCode != KKeyCode.LeftControl && binding.mKeyCode != KKeyCode.RightControl && binding.mKeyCode != KKeyCode.LeftShift && binding.mKeyCode != KKeyCode.RightShift)
		{
			text += this.GetModifierString(binding.mModifier);
		}
		return text;
	}

	// Token: 0x06006181 RID: 24961 RVA: 0x002449B8 File Offset: 0x00242BB8
	private void BuildDisplay()
	{
		string text = this.screens[this.activeScreen];
		string key = "STRINGS.INPUT_BINDINGS." + text.ToUpper() + ".NAME";
		this.screenTitle.text = Strings.Get(key);
		if (this.entryPool == null)
		{
			this.entryPool = new UIPool<HorizontalLayoutGroup>(this.entryPrefab.GetComponent<HorizontalLayoutGroup>());
		}
		this.DestroyDisplay();
		int num = 0;
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry binding = GameInputMapping.KeyBindings[i];
			if (binding.mGroup == this.screens[this.activeScreen] && binding.mRebindable && DlcManager.IsAllContentSubscribed(binding.dlcIds))
			{
				GameObject gameObject = this.entryPool.GetFreeElement(this.parent, true).gameObject;
				TMP_Text componentInChildren = gameObject.transform.GetChild(0).GetComponentInChildren<LocText>();
				string key2 = "STRINGS.INPUT_BINDINGS." + binding.mGroup.ToUpper() + "." + binding.mAction.ToString().ToUpper();
				componentInChildren.text = Strings.Get(key2);
				LocText key_label = gameObject.transform.GetChild(1).GetComponentInChildren<LocText>();
				key_label.text = this.GetBindingText(binding);
				KButton button = gameObject.GetComponentInChildren<KButton>();
				button.onClick += delegate()
				{
					this.waitingForKeyPress = true;
					this.actionToRebind = binding.mAction;
					this.ignoreRootConflicts = binding.mIgnoreRootConflics;
					this.activeButton = button;
					key_label.text = UI.FRONTEND.INPUT_BINDINGS_SCREEN.WAITING_FOR_INPUT;
				};
				gameObject.transform.SetSiblingIndex(num);
				num++;
			}
		}
	}

	// Token: 0x06006182 RID: 24962 RVA: 0x00244B94 File Offset: 0x00242D94
	private void DestroyDisplay()
	{
		this.entryPool.ClearAll();
	}

	// Token: 0x06006183 RID: 24963 RVA: 0x00244BA4 File Offset: 0x00242DA4
	private void Update()
	{
		if (this.waitingForKeyPress)
		{
			Modifier modifier = Modifier.None;
			modifier |= ((this.IsKeyDown(KeyCode.LeftAlt) || this.IsKeyDown(KeyCode.RightAlt)) ? Modifier.Alt : Modifier.None);
			modifier |= ((this.IsKeyDown(KeyCode.LeftControl) || this.IsKeyDown(KeyCode.RightControl)) ? Modifier.Ctrl : Modifier.None);
			modifier |= ((this.IsKeyDown(KeyCode.LeftShift) || this.IsKeyDown(KeyCode.RightShift)) ? Modifier.Shift : Modifier.None);
			modifier |= (this.IsKeyDown(KeyCode.CapsLock) ? Modifier.CapsLock : Modifier.None);
			modifier |= (this.IsKeyDown(KeyCode.BackQuote) ? Modifier.Backtick : Modifier.None);
			bool flag = false;
			for (int i = 0; i < InputBindingsScreen.validKeys.Length; i++)
			{
				KeyCode keyCode = InputBindingsScreen.validKeys[i];
				if (Input.GetKeyDown(keyCode))
				{
					KKeyCode kkey_code = (KKeyCode)keyCode;
					this.Bind(kkey_code, modifier);
					flag = true;
				}
			}
			if (!flag)
			{
				float axis = Input.GetAxis("Mouse ScrollWheel");
				KKeyCode kkeyCode = KKeyCode.None;
				if (axis < 0f)
				{
					kkeyCode = KKeyCode.MouseScrollDown;
				}
				else if (axis > 0f)
				{
					kkeyCode = KKeyCode.MouseScrollUp;
				}
				if (kkeyCode != KKeyCode.None)
				{
					this.Bind(kkeyCode, modifier);
				}
			}
		}
	}

	// Token: 0x06006184 RID: 24964 RVA: 0x00244CBC File Offset: 0x00242EBC
	private BindingEntry GetDuplicatedBinding(string activeScreen, BindingEntry new_binding)
	{
		BindingEntry result = default(BindingEntry);
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry = GameInputMapping.KeyBindings[i];
			if (new_binding.IsBindingEqual(bindingEntry) && (bindingEntry.mGroup == null || bindingEntry.mGroup == activeScreen || bindingEntry.mGroup == "Root" || activeScreen == "Root") && (!(activeScreen == "Root") || !bindingEntry.mIgnoreRootConflics) && (!(bindingEntry.mGroup == "Root") || !new_binding.mIgnoreRootConflics))
			{
				result = bindingEntry;
				break;
			}
		}
		return result;
	}

	// Token: 0x06006185 RID: 24965 RVA: 0x00244D68 File Offset: 0x00242F68
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.waitingForKeyPress)
		{
			e.Consumed = true;
			return;
		}
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006186 RID: 24966 RVA: 0x00244D9A File Offset: 0x00242F9A
	public override void OnKeyUp(KButtonEvent e)
	{
		e.Consumed = true;
	}

	// Token: 0x06006187 RID: 24967 RVA: 0x00244DA4 File Offset: 0x00242FA4
	private void OnBack()
	{
		int num = this.NumUnboundActions();
		if (num == 0)
		{
			this.Deactivate();
			return;
		}
		string text;
		if (num == 1)
		{
			BindingEntry firstUnbound = this.GetFirstUnbound();
			text = string.Format(UI.FRONTEND.INPUT_BINDINGS_SCREEN.UNBOUND_ACTION, firstUnbound.mAction.ToString());
		}
		else
		{
			text = UI.FRONTEND.INPUT_BINDINGS_SCREEN.MULTIPLE_UNBOUND_ACTIONS;
		}
		this.confirmDialog = Util.KInstantiateUI(this.confirmPrefab.gameObject, base.transform.gameObject, false).GetComponent<ConfirmDialogScreen>();
		this.confirmDialog.PopupConfirmDialog(text, delegate
		{
			this.Deactivate();
		}, delegate
		{
			this.confirmDialog.Deactivate();
		}, null, null, null, null, null, null);
		this.confirmDialog.gameObject.SetActive(true);
	}

	// Token: 0x06006188 RID: 24968 RVA: 0x00244E60 File Offset: 0x00243060
	private int NumUnboundActions()
	{
		int num = 0;
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry = GameInputMapping.KeyBindings[i];
			if (bindingEntry.mKeyCode == KKeyCode.None && bindingEntry.mRebindable && (BuildMenu.UseHotkeyBuildMenu() || !bindingEntry.mIgnoreRootConflics))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06006189 RID: 24969 RVA: 0x00244EB4 File Offset: 0x002430B4
	private BindingEntry GetFirstUnbound()
	{
		BindingEntry result = default(BindingEntry);
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry = GameInputMapping.KeyBindings[i];
			if (bindingEntry.mKeyCode == KKeyCode.None)
			{
				result = bindingEntry;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600618A RID: 24970 RVA: 0x00244EF4 File Offset: 0x002430F4
	private void OnReset()
	{
		GameInputMapping.KeyBindings = (BindingEntry[])GameInputMapping.DefaultBindings.Clone();
		Global.GetInputManager().RebindControls();
		this.BuildDisplay();
	}

	// Token: 0x0600618B RID: 24971 RVA: 0x00244F1A File Offset: 0x0024311A
	public void OnPrevScreen()
	{
		if (this.activeScreen > 0)
		{
			this.activeScreen--;
		}
		else
		{
			this.activeScreen = this.screens.Count - 1;
		}
		this.BuildDisplay();
	}

	// Token: 0x0600618C RID: 24972 RVA: 0x00244F4E File Offset: 0x0024314E
	public void OnNextScreen()
	{
		if (this.activeScreen < this.screens.Count - 1)
		{
			this.activeScreen++;
		}
		else
		{
			this.activeScreen = 0;
		}
		this.BuildDisplay();
	}

	// Token: 0x0600618D RID: 24973 RVA: 0x00244F84 File Offset: 0x00243184
	private void Bind(KKeyCode kkey_code, Modifier modifier)
	{
		BindingEntry bindingEntry = new BindingEntry(this.screens[this.activeScreen], GamepadButton.NumButtons, kkey_code, modifier, this.actionToRebind, true, this.ignoreRootConflicts);
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry2 = GameInputMapping.KeyBindings[i];
			if (bindingEntry2.mRebindable && bindingEntry2.mAction == this.actionToRebind)
			{
				BindingEntry duplicatedBinding = this.GetDuplicatedBinding(this.screens[this.activeScreen], bindingEntry);
				bindingEntry.mButton = GameInputMapping.KeyBindings[i].mButton;
				GameInputMapping.KeyBindings[i] = bindingEntry;
				this.activeButton.GetComponentInChildren<LocText>().text = this.GetBindingText(bindingEntry);
				if (duplicatedBinding.mAction != global::Action.Invalid && duplicatedBinding.mAction != this.actionToRebind)
				{
					this.confirmDialog = Util.KInstantiateUI(this.confirmPrefab.gameObject, base.transform.gameObject, false).GetComponent<ConfirmDialogScreen>();
					string arg = Strings.Get("STRINGS.INPUT_BINDINGS." + duplicatedBinding.mGroup.ToUpper() + "." + duplicatedBinding.mAction.ToString().ToUpper());
					string bindingText = this.GetBindingText(duplicatedBinding);
					string text = string.Format(UI.FRONTEND.INPUT_BINDINGS_SCREEN.DUPLICATE, arg, bindingText);
					this.Unbind(duplicatedBinding.mAction);
					this.confirmDialog.PopupConfirmDialog(text, null, null, null, null, null, null, null, null);
					this.confirmDialog.gameObject.SetActive(true);
				}
				Global.GetInputManager().RebindControls();
				this.waitingForKeyPress = false;
				this.actionToRebind = global::Action.NumActions;
				this.activeButton = null;
				this.BuildDisplay();
				return;
			}
		}
	}

	// Token: 0x0600618E RID: 24974 RVA: 0x00245148 File Offset: 0x00243348
	private void Unbind(global::Action action)
	{
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry = GameInputMapping.KeyBindings[i];
			if (bindingEntry.mAction == action)
			{
				bindingEntry.mKeyCode = KKeyCode.None;
				bindingEntry.mModifier = Modifier.None;
				GameInputMapping.KeyBindings[i] = bindingEntry;
			}
		}
	}

	// Token: 0x06006190 RID: 24976 RVA: 0x002451BD File Offset: 0x002433BD
	// Note: this type is marked as 'beforefieldinit'.
	static InputBindingsScreen()
	{
		KeyCode[] array = new KeyCode[111];
		RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.4522A529DBF1D30936B6BCC06D2E607CD76E3B0FB1C18D9DA2635843A2840CD7).FieldHandle);
		InputBindingsScreen.validKeys = array;
	}

	// Token: 0x04004213 RID: 16915
	private const string ROOT_KEY = "STRINGS.INPUT_BINDINGS.";

	// Token: 0x04004214 RID: 16916
	[SerializeField]
	private OptionsMenuScreen optionsScreen;

	// Token: 0x04004215 RID: 16917
	[SerializeField]
	private ConfirmDialogScreen confirmPrefab;

	// Token: 0x04004216 RID: 16918
	public KButton backButton;

	// Token: 0x04004217 RID: 16919
	public KButton resetButton;

	// Token: 0x04004218 RID: 16920
	public KButton closeButton;

	// Token: 0x04004219 RID: 16921
	public KButton prevScreenButton;

	// Token: 0x0400421A RID: 16922
	public KButton nextScreenButton;

	// Token: 0x0400421B RID: 16923
	private bool waitingForKeyPress;

	// Token: 0x0400421C RID: 16924
	private global::Action actionToRebind = global::Action.NumActions;

	// Token: 0x0400421D RID: 16925
	private bool ignoreRootConflicts;

	// Token: 0x0400421E RID: 16926
	private KButton activeButton;

	// Token: 0x0400421F RID: 16927
	[SerializeField]
	private LocText screenTitle;

	// Token: 0x04004220 RID: 16928
	[SerializeField]
	private GameObject parent;

	// Token: 0x04004221 RID: 16929
	[SerializeField]
	private GameObject entryPrefab;

	// Token: 0x04004222 RID: 16930
	private ConfirmDialogScreen confirmDialog;

	// Token: 0x04004223 RID: 16931
	private int activeScreen = -1;

	// Token: 0x04004224 RID: 16932
	private List<string> screens = new List<string>();

	// Token: 0x04004225 RID: 16933
	private UIPool<HorizontalLayoutGroup> entryPool;

	// Token: 0x04004226 RID: 16934
	private static readonly KeyCode[] validKeys;
}
