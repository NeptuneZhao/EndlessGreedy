using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CE0 RID: 3296
public class MetricsOptionsScreen : KModalScreen
{
	// Token: 0x060065DF RID: 26079 RVA: 0x0025F93B File Offset: 0x0025DB3B
	private bool IsSettingsDirty()
	{
		return this.disableDataCollection != KPrivacyPrefs.instance.disableDataCollection;
	}

	// Token: 0x060065E0 RID: 26080 RVA: 0x0025F952 File Offset: 0x0025DB52
	public override void OnKeyDown(KButtonEvent e)
	{
		if ((e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight)) && !this.IsSettingsDirty())
		{
			this.Show(false);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060065E1 RID: 26081 RVA: 0x0025F97C File Offset: 0x0025DB7C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.disableDataCollection = KPrivacyPrefs.instance.disableDataCollection;
		this.title.SetText(UI.FRONTEND.METRICS_OPTIONS_SCREEN.TITLE);
		GameObject gameObject = this.enableButton.GetComponent<HierarchyReferences>().GetReference("Button").gameObject;
		gameObject.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.METRICS_OPTIONS_SCREEN.TOOLTIP);
		gameObject.GetComponent<KButton>().onClick += delegate()
		{
			this.OnClickToggle();
		};
		this.enableButton.GetComponent<HierarchyReferences>().GetReference<LocText>("Text").SetText(UI.FRONTEND.METRICS_OPTIONS_SCREEN.ENABLE_BUTTON);
		this.dismissButton.onClick += delegate()
		{
			if (this.IsSettingsDirty())
			{
				this.ApplySettingsAndDoRestart();
				return;
			}
			this.Deactivate();
		};
		this.closeButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.descriptionButton.onClick.AddListener(delegate()
		{
			App.OpenWebURL("https://www.kleientertainment.com/privacy-policy");
		});
		this.Refresh();
	}

	// Token: 0x060065E2 RID: 26082 RVA: 0x0025FA80 File Offset: 0x0025DC80
	private void OnClickToggle()
	{
		this.disableDataCollection = !this.disableDataCollection;
		this.enableButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(this.disableDataCollection);
		this.Refresh();
	}

	// Token: 0x060065E3 RID: 26083 RVA: 0x0025FABC File Offset: 0x0025DCBC
	private void ApplySettingsAndDoRestart()
	{
		KPrivacyPrefs.instance.disableDataCollection = this.disableDataCollection;
		KPrivacyPrefs.Save();
		KPlayerPrefs.SetString("DisableDataCollection", KPrivacyPrefs.instance.disableDataCollection ? "yes" : "no");
		KPlayerPrefs.Save();
		ThreadedHttps<KleiMetrics>.Instance.SetEnabled(!KPrivacyPrefs.instance.disableDataCollection);
		this.enableButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(ThreadedHttps<KleiMetrics>.Instance.enabled);
		App.instance.Restart();
	}

	// Token: 0x060065E4 RID: 26084 RVA: 0x0025FB50 File Offset: 0x0025DD50
	private void Refresh()
	{
		this.enableButton.GetComponent<HierarchyReferences>().GetReference("Button").transform.GetChild(0).gameObject.SetActive(!this.disableDataCollection);
		this.closeButton.isInteractable = !this.IsSettingsDirty();
		this.restartWarningText.gameObject.SetActive(this.IsSettingsDirty());
		if (this.IsSettingsDirty())
		{
			this.dismissButton.GetComponentInChildren<LocText>().text = UI.FRONTEND.METRICS_OPTIONS_SCREEN.RESTART_BUTTON;
			return;
		}
		this.dismissButton.GetComponentInChildren<LocText>().text = UI.FRONTEND.METRICS_OPTIONS_SCREEN.DONE_BUTTON;
	}

	// Token: 0x040044BF RID: 17599
	public LocText title;

	// Token: 0x040044C0 RID: 17600
	public KButton dismissButton;

	// Token: 0x040044C1 RID: 17601
	public KButton closeButton;

	// Token: 0x040044C2 RID: 17602
	public GameObject enableButton;

	// Token: 0x040044C3 RID: 17603
	public Button descriptionButton;

	// Token: 0x040044C4 RID: 17604
	public LocText restartWarningText;

	// Token: 0x040044C5 RID: 17605
	private bool disableDataCollection;
}
