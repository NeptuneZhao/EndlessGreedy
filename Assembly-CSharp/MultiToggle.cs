using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000DF8 RID: 3576
[AddComponentMenu("KMonoBehaviour/scripts/MultiToggle")]
public class MultiToggle : KMonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	// Token: 0x170007DC RID: 2012
	// (get) Token: 0x06007183 RID: 29059 RVA: 0x002AF205 File Offset: 0x002AD405
	public int CurrentState
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x06007184 RID: 29060 RVA: 0x002AF20D File Offset: 0x002AD40D
	public void NextState()
	{
		this.ChangeState((this.state + 1) % this.states.Length);
	}

	// Token: 0x06007185 RID: 29061 RVA: 0x002AF226 File Offset: 0x002AD426
	protected virtual void Update()
	{
		if (this.clickHeldDown)
		{
			this.totalHeldTime += Time.unscaledDeltaTime;
			if (this.totalHeldTime > this.heldTimeThreshold && this.onHold != null)
			{
				this.onHold();
			}
		}
	}

	// Token: 0x06007186 RID: 29062 RVA: 0x002AF263 File Offset: 0x002AD463
	protected override void OnDisable()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			this.RefreshHoverColor();
			this.pointerOver = false;
			this.StopHolding();
		}
	}

	// Token: 0x06007187 RID: 29063 RVA: 0x002AF285 File Offset: 0x002AD485
	public void ChangeState(int new_state_index, bool forceRefreshState)
	{
		if (forceRefreshState)
		{
			this.stateDirty = true;
		}
		this.ChangeState(new_state_index);
	}

	// Token: 0x06007188 RID: 29064 RVA: 0x002AF298 File Offset: 0x002AD498
	public void ChangeState(int new_state_index)
	{
		if (!this.stateDirty && new_state_index == this.state)
		{
			return;
		}
		this.stateDirty = false;
		this.state = new_state_index;
		try
		{
			this.toggle_image.sprite = this.states[new_state_index].sprite;
			this.toggle_image.color = this.states[new_state_index].color;
			if (this.states[new_state_index].use_rect_margins)
			{
				this.toggle_image.rectTransform().sizeDelta = this.states[new_state_index].rect_margins;
			}
		}
		catch
		{
			string text = base.gameObject.name;
			Transform transform = base.transform;
			while (transform.parent != null)
			{
				text = text.Insert(0, transform.name + ">");
				transform = transform.parent;
			}
			global::Debug.LogError("Multi Toggle state index out of range: " + text + " idx:" + new_state_index.ToString(), base.gameObject);
		}
		foreach (StatePresentationSetting statePresentationSetting in this.states[this.state].additional_display_settings)
		{
			if (!(statePresentationSetting.image_target == null))
			{
				statePresentationSetting.image_target.sprite = statePresentationSetting.sprite;
				statePresentationSetting.image_target.color = statePresentationSetting.color;
			}
		}
		this.RefreshHoverColor();
	}

	// Token: 0x06007189 RID: 29065 RVA: 0x002AF414 File Offset: 0x002AD614
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (!this.allowRightClick && eventData.button == PointerEventData.InputButton.Right)
		{
			return;
		}
		if (this.states.Length - 1 < this.state)
		{
			global::Debug.LogWarning("Multi toggle has too few / no states");
		}
		bool flag = false;
		if (this.onDoubleClick != null && eventData.clickCount == 2)
		{
			flag = this.onDoubleClick();
		}
		if (this.onClick != null && !flag)
		{
			this.onClick();
		}
		this.RefreshHoverColor();
	}

	// Token: 0x0600718A RID: 29066 RVA: 0x002AF48C File Offset: 0x002AD68C
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.pointerOver = true;
		if (!KInputManager.isFocused)
		{
			return;
		}
		KInputManager.SetUserActive();
		if (this.states.Length == 0)
		{
			return;
		}
		if (this.states[this.state].use_color_on_hover && this.states[this.state].color_on_hover != this.states[this.state].color)
		{
			this.toggle_image.color = this.states[this.state].color_on_hover;
		}
		if (this.states[this.state].use_rect_margins)
		{
			this.toggle_image.rectTransform().sizeDelta = this.states[this.state].rect_margins;
		}
		foreach (StatePresentationSetting statePresentationSetting in this.states[this.state].additional_display_settings)
		{
			if (!(statePresentationSetting.image_target == null) && statePresentationSetting.use_color_on_hover)
			{
				statePresentationSetting.image_target.color = statePresentationSetting.color_on_hover;
			}
		}
		if (this.onEnter != null)
		{
			this.onEnter();
		}
	}

	// Token: 0x0600718B RID: 29067 RVA: 0x002AF5C8 File Offset: 0x002AD7C8
	protected void RefreshHoverColor()
	{
		if (base.gameObject.activeInHierarchy)
		{
			if (this.pointerOver)
			{
				if (this.states[this.state].use_color_on_hover && this.states[this.state].color_on_hover != this.states[this.state].color)
				{
					this.toggle_image.color = this.states[this.state].color_on_hover;
				}
				foreach (StatePresentationSetting statePresentationSetting in this.states[this.state].additional_display_settings)
				{
					if (!(statePresentationSetting.image_target == null) && !(statePresentationSetting.image_target == null) && statePresentationSetting.use_color_on_hover)
					{
						statePresentationSetting.image_target.color = statePresentationSetting.color_on_hover;
					}
				}
			}
			return;
		}
		if (this.states.Length == 0)
		{
			return;
		}
		if (this.states[this.state].use_color_on_hover && this.states[this.state].color_on_hover != this.states[this.state].color)
		{
			this.toggle_image.color = this.states[this.state].color;
		}
		foreach (StatePresentationSetting statePresentationSetting2 in this.states[this.state].additional_display_settings)
		{
			if (!(statePresentationSetting2.image_target == null) && statePresentationSetting2.use_color_on_hover)
			{
				statePresentationSetting2.image_target.color = statePresentationSetting2.color;
			}
		}
	}

	// Token: 0x0600718C RID: 29068 RVA: 0x002AF78C File Offset: 0x002AD98C
	public void OnPointerExit(PointerEventData eventData)
	{
		this.pointerOver = false;
		if (!KInputManager.isFocused)
		{
			return;
		}
		KInputManager.SetUserActive();
		if (this.states.Length == 0)
		{
			return;
		}
		if (this.states[this.state].use_color_on_hover && this.states[this.state].color_on_hover != this.states[this.state].color)
		{
			this.toggle_image.color = this.states[this.state].color;
		}
		if (this.states[this.state].use_rect_margins)
		{
			this.toggle_image.rectTransform().sizeDelta = this.states[this.state].rect_margins;
		}
		foreach (StatePresentationSetting statePresentationSetting in this.states[this.state].additional_display_settings)
		{
			if (!(statePresentationSetting.image_target == null) && statePresentationSetting.use_color_on_hover)
			{
				statePresentationSetting.image_target.color = statePresentationSetting.color;
			}
		}
		if (this.onExit != null)
		{
			this.onExit();
		}
	}

	// Token: 0x0600718D RID: 29069 RVA: 0x002AF8C8 File Offset: 0x002ADAC8
	public virtual void OnPointerDown(PointerEventData eventData)
	{
		if (!this.allowRightClick && eventData.button == PointerEventData.InputButton.Right)
		{
			return;
		}
		this.clickHeldDown = true;
		if (this.play_sound_on_click)
		{
			ToggleState toggleState = this.states[this.state];
			string on_click_override_sound_path = toggleState.on_click_override_sound_path;
			bool has_sound_parameter = toggleState.has_sound_parameter;
			if (on_click_override_sound_path == "")
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Click", false));
				return;
			}
			if (on_click_override_sound_path != "" && has_sound_parameter)
			{
				KFMOD.PlayUISoundWithParameter(GlobalAssets.GetSound("General_Item_Click", false), toggleState.sound_parameter_name, toggleState.sound_parameter_value);
				KFMOD.PlayUISoundWithParameter(GlobalAssets.GetSound(on_click_override_sound_path, false), toggleState.sound_parameter_name, toggleState.sound_parameter_value);
				return;
			}
			KFMOD.PlayUISound(GlobalAssets.GetSound(on_click_override_sound_path, false));
		}
	}

	// Token: 0x0600718E RID: 29070 RVA: 0x002AF987 File Offset: 0x002ADB87
	public virtual void OnPointerUp(PointerEventData eventData)
	{
		if (!this.allowRightClick && eventData.button == PointerEventData.InputButton.Right)
		{
			return;
		}
		this.StopHolding();
	}

	// Token: 0x0600718F RID: 29071 RVA: 0x002AF9A4 File Offset: 0x002ADBA4
	private void StopHolding()
	{
		if (this.clickHeldDown)
		{
			if (this.play_sound_on_release && this.states[this.state].on_release_override_sound_path != "")
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound(this.states[this.state].on_release_override_sound_path, false));
			}
			this.clickHeldDown = false;
			if (this.onStopHold != null)
			{
				this.onStopHold();
			}
		}
		this.totalHeldTime = 0f;
	}

	// Token: 0x04004E3E RID: 20030
	[Header("Settings")]
	[SerializeField]
	public ToggleState[] states;

	// Token: 0x04004E3F RID: 20031
	public bool play_sound_on_click = true;

	// Token: 0x04004E40 RID: 20032
	public bool play_sound_on_release;

	// Token: 0x04004E41 RID: 20033
	public Image toggle_image;

	// Token: 0x04004E42 RID: 20034
	protected int state;

	// Token: 0x04004E43 RID: 20035
	public System.Action onClick;

	// Token: 0x04004E44 RID: 20036
	private bool stateDirty = true;

	// Token: 0x04004E45 RID: 20037
	public Func<bool> onDoubleClick;

	// Token: 0x04004E46 RID: 20038
	public System.Action onEnter;

	// Token: 0x04004E47 RID: 20039
	public System.Action onExit;

	// Token: 0x04004E48 RID: 20040
	public System.Action onHold;

	// Token: 0x04004E49 RID: 20041
	public System.Action onStopHold;

	// Token: 0x04004E4A RID: 20042
	public bool allowRightClick = true;

	// Token: 0x04004E4B RID: 20043
	protected bool clickHeldDown;

	// Token: 0x04004E4C RID: 20044
	protected float totalHeldTime;

	// Token: 0x04004E4D RID: 20045
	protected float heldTimeThreshold = 0.4f;

	// Token: 0x04004E4E RID: 20046
	private bool pointerOver;
}
