using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C75 RID: 3189
public class KModalScreen : KScreen
{
	// Token: 0x060061FA RID: 25082 RVA: 0x00249534 File Offset: 0x00247734
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.backgroundRectTransform = KModalScreen.MakeScreenModal(this);
	}

	// Token: 0x060061FB RID: 25083 RVA: 0x00249548 File Offset: 0x00247748
	public static RectTransform MakeScreenModal(KScreen screen)
	{
		screen.ConsumeMouseScroll = true;
		screen.activateOnSpawn = true;
		GameObject gameObject = new GameObject("background");
		gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
		gameObject.AddComponent<CanvasRenderer>();
		Image image = gameObject.AddComponent<Image>();
		image.color = new Color32(0, 0, 0, 160);
		image.raycastTarget = true;
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.SetParent(screen.transform);
		KModalScreen.ResizeBackground(component);
		return component;
	}

	// Token: 0x060061FC RID: 25084 RVA: 0x002495BC File Offset: 0x002477BC
	public static void ResizeBackground(RectTransform rectTransform)
	{
		rectTransform.SetAsFirstSibling();
		rectTransform.SetLocalPosition(Vector3.zero);
		rectTransform.localScale = Vector3.one;
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.sizeDelta = new Vector2(0f, 0f);
	}

	// Token: 0x060061FD RID: 25085 RVA: 0x00249628 File Offset: 0x00247828
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (CameraController.Instance != null)
		{
			CameraController.Instance.DisableUserCameraControl = true;
		}
		if (ScreenResize.Instance != null)
		{
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		}
	}

	// Token: 0x060061FE RID: 25086 RVA: 0x00249688 File Offset: 0x00247888
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (CameraController.Instance != null)
		{
			CameraController.Instance.DisableUserCameraControl = false;
		}
		base.Trigger(476357528, null);
		if (ScreenResize.Instance != null)
		{
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Remove(instance.OnResize, new System.Action(this.OnResize));
		}
	}

	// Token: 0x060061FF RID: 25087 RVA: 0x002496F2 File Offset: 0x002478F2
	private void OnResize()
	{
		KModalScreen.ResizeBackground(this.backgroundRectTransform);
	}

	// Token: 0x06006200 RID: 25088 RVA: 0x002496FF File Offset: 0x002478FF
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06006201 RID: 25089 RVA: 0x00249702 File Offset: 0x00247902
	public override float GetSortKey()
	{
		return 100f;
	}

	// Token: 0x06006202 RID: 25090 RVA: 0x00249709 File Offset: 0x00247909
	protected override void OnActivate()
	{
		this.OnShow(true);
	}

	// Token: 0x06006203 RID: 25091 RVA: 0x00249712 File Offset: 0x00247912
	protected override void OnDeactivate()
	{
		this.OnShow(false);
	}

	// Token: 0x06006204 RID: 25092 RVA: 0x0024971C File Offset: 0x0024791C
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (this.pause && SpeedControlScreen.Instance != null)
		{
			if (show && !this.shown)
			{
				SpeedControlScreen.Instance.Pause(false, false);
			}
			else if (!show && this.shown)
			{
				SpeedControlScreen.Instance.Unpause(false);
			}
			this.shown = show;
		}
	}

	// Token: 0x06006205 RID: 25093 RVA: 0x0024977C File Offset: 0x0024797C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (Game.Instance != null && (e.TryConsume(global::Action.TogglePause) || e.TryConsume(global::Action.CycleSpeed)))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
		if (!e.Consumed && (e.TryConsume(global::Action.Escape) || (e.TryConsume(global::Action.MouseRight) && this.canBackoutWithRightClick)))
		{
			this.Deactivate();
		}
		base.OnKeyDown(e);
		e.Consumed = true;
	}

	// Token: 0x06006206 RID: 25094 RVA: 0x002497F9 File Offset: 0x002479F9
	public override void OnKeyUp(KButtonEvent e)
	{
		base.OnKeyUp(e);
		e.Consumed = true;
	}

	// Token: 0x04004275 RID: 17013
	private bool shown;

	// Token: 0x04004276 RID: 17014
	public bool pause = true;

	// Token: 0x04004277 RID: 17015
	[Tooltip("Only used for main menu")]
	public bool canBackoutWithRightClick;

	// Token: 0x04004278 RID: 17016
	private RectTransform backgroundRectTransform;
}
