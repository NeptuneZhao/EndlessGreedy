using System;
using UnityEngine;

// Token: 0x02000D8E RID: 3470
public class PlayerControlledToggleSideScreen : SideScreenContent, IRenderEveryTick
{
	// Token: 0x06006D5B RID: 27995 RVA: 0x0029230C File Offset: 0x0029050C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.toggleButton.onClick += this.ClickToggle;
		this.togglePendingStatusItem = new StatusItem("PlayerControlledToggleSideScreen", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
	}

	// Token: 0x06006D5C RID: 27996 RVA: 0x0029235F File Offset: 0x0029055F
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IPlayerControlledToggle>() != null;
	}

	// Token: 0x06006D5D RID: 27997 RVA: 0x0029236C File Offset: 0x0029056C
	public void RenderEveryTick(float dt)
	{
		if (base.isActiveAndEnabled)
		{
			if (!this.keyDown && (Input.GetKeyDown(KeyCode.Return) & Time.unscaledTime - this.lastKeyboardShortcutTime > 0.1f))
			{
				if (SpeedControlScreen.Instance.IsPaused)
				{
					this.RequestToggle();
				}
				else
				{
					this.Toggle();
				}
				this.lastKeyboardShortcutTime = Time.unscaledTime;
				this.keyDown = true;
			}
			if (this.keyDown && Input.GetKeyUp(KeyCode.Return))
			{
				this.keyDown = false;
			}
		}
	}

	// Token: 0x06006D5E RID: 27998 RVA: 0x002923EA File Offset: 0x002905EA
	private void ClickToggle()
	{
		if (SpeedControlScreen.Instance.IsPaused)
		{
			this.RequestToggle();
			return;
		}
		this.Toggle();
	}

	// Token: 0x06006D5F RID: 27999 RVA: 0x00292408 File Offset: 0x00290608
	private void RequestToggle()
	{
		this.target.ToggleRequested = !this.target.ToggleRequested;
		if (this.target.ToggleRequested && SpeedControlScreen.Instance.IsPaused)
		{
			this.target.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, this.togglePendingStatusItem, this);
		}
		else
		{
			this.target.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
		this.UpdateVisuals(this.target.ToggleRequested ? (!this.target.ToggledOn()) : this.target.ToggledOn(), true);
	}

	// Token: 0x06006D60 RID: 28000 RVA: 0x002924C4 File Offset: 0x002906C4
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IPlayerControlledToggle>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received is not an IPlayerControlledToggle");
			return;
		}
		this.UpdateVisuals(this.target.ToggleRequested ? (!this.target.ToggledOn()) : this.target.ToggledOn(), false);
		this.titleKey = this.target.SideScreenTitleKey;
	}

	// Token: 0x06006D61 RID: 28001 RVA: 0x00292544 File Offset: 0x00290744
	private void Toggle()
	{
		this.target.ToggledByPlayer();
		this.UpdateVisuals(this.target.ToggledOn(), true);
		this.target.ToggleRequested = false;
		this.target.GetSelectable().RemoveStatusItem(this.togglePendingStatusItem, false);
	}

	// Token: 0x06006D62 RID: 28002 RVA: 0x00292594 File Offset: 0x00290794
	private void UpdateVisuals(bool state, bool smooth)
	{
		if (state != this.currentState)
		{
			if (smooth)
			{
				this.kbac.Play(state ? PlayerControlledToggleSideScreen.ON_ANIMS : PlayerControlledToggleSideScreen.OFF_ANIMS, KAnim.PlayMode.Once);
			}
			else
			{
				this.kbac.Play(state ? PlayerControlledToggleSideScreen.ON_ANIMS[1] : PlayerControlledToggleSideScreen.OFF_ANIMS[1], KAnim.PlayMode.Once, 1f, 0f);
			}
		}
		this.currentState = state;
	}

	// Token: 0x04004A96 RID: 19094
	public IPlayerControlledToggle target;

	// Token: 0x04004A97 RID: 19095
	public KButton toggleButton;

	// Token: 0x04004A98 RID: 19096
	protected static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on"
	};

	// Token: 0x04004A99 RID: 19097
	protected static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"off_pre",
		"off"
	};

	// Token: 0x04004A9A RID: 19098
	public float animScaleBase = 0.25f;

	// Token: 0x04004A9B RID: 19099
	private StatusItem togglePendingStatusItem;

	// Token: 0x04004A9C RID: 19100
	[SerializeField]
	private KBatchedAnimController kbac;

	// Token: 0x04004A9D RID: 19101
	private float lastKeyboardShortcutTime;

	// Token: 0x04004A9E RID: 19102
	private const float KEYBOARD_COOLDOWN = 0.1f;

	// Token: 0x04004A9F RID: 19103
	private bool keyDown;

	// Token: 0x04004AA0 RID: 19104
	private bool currentState;
}
