using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BB5 RID: 2997
public class StoryMessageScreen : KScreen
{
	// Token: 0x170006C1 RID: 1729
	// (set) Token: 0x06005AE9 RID: 23273 RVA: 0x00211223 File Offset: 0x0020F423
	public string title
	{
		set
		{
			this.titleLabel.SetText(value);
		}
	}

	// Token: 0x170006C2 RID: 1730
	// (set) Token: 0x06005AEA RID: 23274 RVA: 0x00211231 File Offset: 0x0020F431
	public string body
	{
		set
		{
			this.bodyLabel.SetText(value);
		}
	}

	// Token: 0x06005AEB RID: 23275 RVA: 0x0021123F File Offset: 0x0020F43F
	public override float GetSortKey()
	{
		return 8f;
	}

	// Token: 0x06005AEC RID: 23276 RVA: 0x00211246 File Offset: 0x0020F446
	protected override void OnSpawn()
	{
		base.OnSpawn();
		StoryMessageScreen.HideInterface(true);
		CameraController.Instance.FadeOut(0.5f, 1f, null);
	}

	// Token: 0x06005AED RID: 23277 RVA: 0x00211269 File Offset: 0x0020F469
	private IEnumerator ExpandPanel()
	{
		this.content.gameObject.SetActive(true);
		yield return SequenceUtil.WaitForSecondsRealtime(0.25f);
		float height = 0f;
		while (height < 299f)
		{
			height = Mathf.Lerp(this.dialog.rectTransform().sizeDelta.y, 300f, Time.unscaledDeltaTime * 15f);
			this.dialog.rectTransform().sizeDelta = new Vector2(this.dialog.rectTransform().sizeDelta.x, height);
			yield return 0;
		}
		CameraController.Instance.FadeOut(0.5f, 1f, null);
		yield return null;
		yield break;
	}

	// Token: 0x06005AEE RID: 23278 RVA: 0x00211278 File Offset: 0x0020F478
	private IEnumerator CollapsePanel()
	{
		float height = 300f;
		while (height > 0f)
		{
			height = Mathf.Lerp(this.dialog.rectTransform().sizeDelta.y, -1f, Time.unscaledDeltaTime * 15f);
			this.dialog.rectTransform().sizeDelta = new Vector2(this.dialog.rectTransform().sizeDelta.x, height);
			yield return 0;
		}
		this.content.gameObject.SetActive(false);
		if (this.OnClose != null)
		{
			this.OnClose();
			this.OnClose = null;
		}
		this.Deactivate();
		yield return null;
		yield break;
	}

	// Token: 0x06005AEF RID: 23279 RVA: 0x00211288 File Offset: 0x0020F488
	public static void HideInterface(bool hide)
	{
		SelectTool.Instance.Select(null, true);
		NotificationScreen.Instance.Show(!hide);
		OverlayMenu.Instance.Show(!hide);
		if (PlanScreen.Instance != null)
		{
			PlanScreen.Instance.Show(!hide);
		}
		if (BuildMenu.Instance != null)
		{
			BuildMenu.Instance.Show(!hide);
		}
		ManagementMenu.Instance.Show(!hide);
		ToolMenu.Instance.Show(!hide);
		ToolMenu.Instance.PriorityScreen.Show(!hide);
		ColonyDiagnosticScreen.Instance.Show(!hide);
		PinnedResourcesPanel.Instance.Show(!hide);
		TopLeftControlScreen.Instance.Show(!hide);
		if (WorldSelector.Instance != null)
		{
			WorldSelector.Instance.Show(!hide);
		}
		global::DateTime.Instance.Show(!hide);
		if (BuildWatermark.Instance != null)
		{
			BuildWatermark.Instance.Show(!hide);
		}
		PopFXManager.Instance.Show(!hide);
	}

	// Token: 0x06005AF0 RID: 23280 RVA: 0x002113A0 File Offset: 0x0020F5A0
	public void Update()
	{
		if (!this.startFade)
		{
			return;
		}
		Color color = this.bg.color;
		color.a -= 0.01f;
		if (color.a <= 0f)
		{
			color.a = 0f;
		}
		this.bg.color = color;
	}

	// Token: 0x06005AF1 RID: 23281 RVA: 0x002113F8 File Offset: 0x0020F5F8
	protected override void OnActivate()
	{
		base.OnActivate();
		SelectTool.Instance.Select(null, false);
		this.button.onClick += delegate()
		{
			base.StartCoroutine(this.CollapsePanel());
		};
		this.dialog.GetComponent<KScreen>().Show(false);
		this.startFade = false;
		CameraController.Instance.DisableUserCameraControl = true;
		KFMOD.PlayUISound(this.dialogSound);
		this.dialog.GetComponent<KScreen>().Activate();
		this.dialog.GetComponent<KScreen>().SetShouldFadeIn(true);
		this.dialog.GetComponent<KScreen>().Show(true);
		MusicManager.instance.PlaySong("Music_Victory_01_Message", false);
		base.StartCoroutine(this.ExpandPanel());
	}

	// Token: 0x06005AF2 RID: 23282 RVA: 0x002114AC File Offset: 0x0020F6AC
	protected override void OnDeactivate()
	{
		base.IsActive();
		base.OnDeactivate();
		MusicManager.instance.StopSong("Music_Victory_01_Message", true, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		if (this.restoreInterfaceOnClose)
		{
			CameraController.Instance.DisableUserCameraControl = false;
			CameraController.Instance.FadeIn(0f, 1f, null);
			StoryMessageScreen.HideInterface(false);
		}
	}

	// Token: 0x06005AF3 RID: 23283 RVA: 0x00211505 File Offset: 0x0020F705
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			base.StartCoroutine(this.CollapsePanel());
		}
		e.Consumed = true;
	}

	// Token: 0x06005AF4 RID: 23284 RVA: 0x00211524 File Offset: 0x0020F724
	public override void OnKeyUp(KButtonEvent e)
	{
		e.Consumed = true;
	}

	// Token: 0x04003BDC RID: 15324
	private const float ALPHA_SPEED = 0.01f;

	// Token: 0x04003BDD RID: 15325
	[SerializeField]
	private Image bg;

	// Token: 0x04003BDE RID: 15326
	[SerializeField]
	private GameObject dialog;

	// Token: 0x04003BDF RID: 15327
	[SerializeField]
	private KButton button;

	// Token: 0x04003BE0 RID: 15328
	[SerializeField]
	private EventReference dialogSound;

	// Token: 0x04003BE1 RID: 15329
	[SerializeField]
	private LocText titleLabel;

	// Token: 0x04003BE2 RID: 15330
	[SerializeField]
	private LocText bodyLabel;

	// Token: 0x04003BE3 RID: 15331
	private const float expandedHeight = 300f;

	// Token: 0x04003BE4 RID: 15332
	[SerializeField]
	private GameObject content;

	// Token: 0x04003BE5 RID: 15333
	public bool restoreInterfaceOnClose = true;

	// Token: 0x04003BE6 RID: 15334
	public System.Action OnClose;

	// Token: 0x04003BE7 RID: 15335
	private bool startFade;
}
