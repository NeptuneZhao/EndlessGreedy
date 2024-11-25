using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000CFE RID: 3326
public class OptionsMenuScreen : KModalButtonMenu
{
	// Token: 0x0600672C RID: 26412 RVA: 0x002682AC File Offset: 0x002664AC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.keepMenuOpen = true;
		this.buttons = new List<KButtonMenu.ButtonInfo>
		{
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.GRAPHICS, global::Action.NumActions, new UnityAction(this.OnGraphicsOptions), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.AUDIO, global::Action.NumActions, new UnityAction(this.OnAudioOptions), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.GAME, global::Action.NumActions, new UnityAction(this.OnGameOptions), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.METRICS, global::Action.NumActions, new UnityAction(this.OnMetrics), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.FEEDBACK, global::Action.NumActions, new UnityAction(this.OnFeedback), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.CREDITS, global::Action.NumActions, new UnityAction(this.OnCredits), null, null)
		};
		this.closeButton.onClick += this.Deactivate;
		this.backButton.onClick += this.Deactivate;
	}

	// Token: 0x0600672D RID: 26413 RVA: 0x002683F1 File Offset: 0x002665F1
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.title.SetText(UI.FRONTEND.OPTIONS_SCREEN.TITLE);
		this.backButton.transform.SetAsLastSibling();
	}

	// Token: 0x0600672E RID: 26414 RVA: 0x00268420 File Offset: 0x00266620
	protected override void OnActivate()
	{
		base.OnActivate();
		foreach (GameObject gameObject in this.buttonObjects)
		{
		}
	}

	// Token: 0x0600672F RID: 26415 RVA: 0x0026844C File Offset: 0x0026664C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006730 RID: 26416 RVA: 0x0026846E File Offset: 0x0026666E
	private void OnGraphicsOptions()
	{
		base.ActivateChildScreen(this.graphicsOptionsScreenPrefab.gameObject);
	}

	// Token: 0x06006731 RID: 26417 RVA: 0x00268482 File Offset: 0x00266682
	private void OnAudioOptions()
	{
		base.ActivateChildScreen(this.audioOptionsScreenPrefab.gameObject);
	}

	// Token: 0x06006732 RID: 26418 RVA: 0x00268496 File Offset: 0x00266696
	private void OnGameOptions()
	{
		base.ActivateChildScreen(this.gameOptionsScreenPrefab.gameObject);
	}

	// Token: 0x06006733 RID: 26419 RVA: 0x002684AA File Offset: 0x002666AA
	private void OnMetrics()
	{
		base.ActivateChildScreen(this.metricsScreenPrefab.gameObject);
	}

	// Token: 0x06006734 RID: 26420 RVA: 0x002684BE File Offset: 0x002666BE
	public void ShowMetricsScreen()
	{
		base.ActivateChildScreen(this.metricsScreenPrefab.gameObject);
	}

	// Token: 0x06006735 RID: 26421 RVA: 0x002684D2 File Offset: 0x002666D2
	private void OnFeedback()
	{
		base.ActivateChildScreen(this.feedbackScreenPrefab.gameObject);
	}

	// Token: 0x06006736 RID: 26422 RVA: 0x002684E6 File Offset: 0x002666E6
	private void OnCredits()
	{
		base.ActivateChildScreen(this.creditsScreenPrefab.gameObject);
	}

	// Token: 0x06006737 RID: 26423 RVA: 0x002684FA File Offset: 0x002666FA
	private void Update()
	{
		global::Debug.developerConsoleVisible = false;
	}

	// Token: 0x0400459B RID: 17819
	[SerializeField]
	private GameOptionsScreen gameOptionsScreenPrefab;

	// Token: 0x0400459C RID: 17820
	[SerializeField]
	private AudioOptionsScreen audioOptionsScreenPrefab;

	// Token: 0x0400459D RID: 17821
	[SerializeField]
	private GraphicsOptionsScreen graphicsOptionsScreenPrefab;

	// Token: 0x0400459E RID: 17822
	[SerializeField]
	private CreditsScreen creditsScreenPrefab;

	// Token: 0x0400459F RID: 17823
	[SerializeField]
	private KButton closeButton;

	// Token: 0x040045A0 RID: 17824
	[SerializeField]
	private MetricsOptionsScreen metricsScreenPrefab;

	// Token: 0x040045A1 RID: 17825
	[SerializeField]
	private FeedbackScreen feedbackScreenPrefab;

	// Token: 0x040045A2 RID: 17826
	[SerializeField]
	private LocText title;

	// Token: 0x040045A3 RID: 17827
	[SerializeField]
	private KButton backButton;
}
