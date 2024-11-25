using System;
using System.Collections.Generic;
using KMod;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D1B RID: 3355
public class ReportErrorDialog : MonoBehaviour
{
	// Token: 0x060068C4 RID: 26820 RVA: 0x00273A1C File Offset: 0x00271C1C
	private void Start()
	{
		ThreadedHttps<KleiMetrics>.Instance.EndSession(true);
		if (KScreenManager.Instance)
		{
			KScreenManager.Instance.DisableInput(true);
		}
		this.StackTrace.SetActive(false);
		this.CrashLabel.text = ((this.mode == ReportErrorDialog.Mode.SubmitError) ? UI.CRASHSCREEN.TITLE : UI.CRASHSCREEN.TITLE_MODS);
		this.CrashDescription.SetActive(this.mode == ReportErrorDialog.Mode.SubmitError);
		this.ModsInfo.SetActive(this.mode == ReportErrorDialog.Mode.DisableMods);
		if (this.mode == ReportErrorDialog.Mode.DisableMods)
		{
			this.BuildModsList();
		}
		this.submitButton.gameObject.SetActive(this.submitAction != null);
		this.submitButton.onClick += this.OnSelect_SUBMIT;
		this.moreInfoButton.onClick += this.OnSelect_MOREINFO;
		this.continueGameButton.gameObject.SetActive(this.continueAction != null);
		this.continueGameButton.onClick += this.OnSelect_CONTINUE;
		this.quitButton.onClick += this.OnSelect_QUIT;
		this.messageInputField.text = UI.CRASHSCREEN.BODY;
		KCrashReporter.onCrashReported += this.OpenRefMessage;
		KCrashReporter.onCrashUploadProgress += this.UpdateProgressBar;
	}

	// Token: 0x060068C5 RID: 26821 RVA: 0x00273B78 File Offset: 0x00271D78
	private void BuildModsList()
	{
		DebugUtil.Assert(Global.Instance != null && Global.Instance.modManager != null);
		Manager mod_mgr = Global.Instance.modManager;
		List<Mod> allCrashableMods = mod_mgr.GetAllCrashableMods();
		allCrashableMods.Sort((Mod x, Mod y) => y.foundInStackTrace.CompareTo(x.foundInStackTrace));
		foreach (Mod mod in allCrashableMods)
		{
			if (mod.foundInStackTrace && mod.label.distribution_platform != Label.DistributionPlatform.Dev)
			{
				mod_mgr.EnableMod(mod.label, false, this);
			}
			HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.modEntryPrefab, this.modEntryParent.gameObject, false);
			LocText reference = hierarchyReferences.GetReference<LocText>("Title");
			reference.text = mod.title;
			reference.color = (mod.foundInStackTrace ? Color.red : Color.white);
			MultiToggle toggle = hierarchyReferences.GetReference<MultiToggle>("EnabledToggle");
			toggle.ChangeState(mod.IsEnabledForActiveDlc() ? 1 : 0);
			Label mod_label = mod.label;
			MultiToggle toggle2 = toggle;
			toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
			{
				bool flag = !mod_mgr.IsModEnabled(mod_label);
				toggle.ChangeState(flag ? 1 : 0);
				mod_mgr.EnableMod(mod_label, flag, this);
			}));
			toggle.GetComponent<ToolTip>().OnToolTip = (() => mod_mgr.IsModEnabled(mod_label) ? UI.FRONTEND.MODS.TOOLTIPS.ENABLED : UI.FRONTEND.MODS.TOOLTIPS.DISABLED);
			hierarchyReferences.gameObject.SetActive(true);
		}
	}

	// Token: 0x060068C6 RID: 26822 RVA: 0x00273D4C File Offset: 0x00271F4C
	private void Update()
	{
		global::Debug.developerConsoleVisible = false;
	}

	// Token: 0x060068C7 RID: 26823 RVA: 0x00273D54 File Offset: 0x00271F54
	private void OnDestroy()
	{
		if (KCrashReporter.terminateOnError)
		{
			App.Quit();
		}
		if (KScreenManager.Instance)
		{
			KScreenManager.Instance.DisableInput(false);
		}
		KCrashReporter.onCrashReported -= this.OpenRefMessage;
		KCrashReporter.onCrashUploadProgress -= this.UpdateProgressBar;
	}

	// Token: 0x060068C8 RID: 26824 RVA: 0x00273DA6 File Offset: 0x00271FA6
	public void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.OnSelect_QUIT();
		}
	}

	// Token: 0x060068C9 RID: 26825 RVA: 0x00273DB7 File Offset: 0x00271FB7
	public void PopupSubmitErrorDialog(string stackTrace, System.Action onSubmit, System.Action onQuit, System.Action onContinue)
	{
		this.mode = ReportErrorDialog.Mode.SubmitError;
		this.m_stackTrace = stackTrace;
		this.submitAction = onSubmit;
		this.quitAction = onQuit;
		this.continueAction = onContinue;
	}

	// Token: 0x060068CA RID: 26826 RVA: 0x00273DDD File Offset: 0x00271FDD
	public void PopupDisableModsDialog(string stackTrace, System.Action onQuit, System.Action onContinue)
	{
		this.mode = ReportErrorDialog.Mode.DisableMods;
		this.m_stackTrace = stackTrace;
		this.quitAction = onQuit;
		this.continueAction = onContinue;
	}

	// Token: 0x060068CB RID: 26827 RVA: 0x00273DFC File Offset: 0x00271FFC
	public void OnSelect_MOREINFO()
	{
		this.StackTrace.GetComponentInChildren<LocText>().text = this.m_stackTrace;
		this.StackTrace.SetActive(true);
		this.moreInfoButton.GetComponentInChildren<LocText>().text = UI.CRASHSCREEN.COPYTOCLIPBOARDBUTTON;
		this.moreInfoButton.ClearOnClick();
		this.moreInfoButton.onClick += this.OnSelect_COPYTOCLIPBOARD;
	}

	// Token: 0x060068CC RID: 26828 RVA: 0x00273E67 File Offset: 0x00272067
	public void OnSelect_COPYTOCLIPBOARD()
	{
		TextEditor textEditor = new TextEditor();
		textEditor.text = this.m_stackTrace + "\nBuild: " + BuildWatermark.GetBuildText();
		textEditor.SelectAll();
		textEditor.Copy();
	}

	// Token: 0x060068CD RID: 26829 RVA: 0x00273E94 File Offset: 0x00272094
	public void OnSelect_SUBMIT()
	{
		this.submitButton.GetComponentInChildren<LocText>().text = UI.CRASHSCREEN.REPORTING;
		this.submitButton.GetComponent<KButton>().isInteractable = false;
		this.Submit();
	}

	// Token: 0x060068CE RID: 26830 RVA: 0x00273EC7 File Offset: 0x002720C7
	public void OnSelect_QUIT()
	{
		if (this.quitAction != null)
		{
			this.quitAction();
		}
	}

	// Token: 0x060068CF RID: 26831 RVA: 0x00273EDC File Offset: 0x002720DC
	public void OnSelect_CONTINUE()
	{
		if (this.continueAction != null)
		{
			this.continueAction();
		}
	}

	// Token: 0x060068D0 RID: 26832 RVA: 0x00273EF4 File Offset: 0x002720F4
	public void OpenRefMessage(bool success)
	{
		this.submitButton.gameObject.SetActive(false);
		this.uploadInProgress.SetActive(false);
		this.referenceMessage.SetActive(true);
		this.messageText.text = (success ? UI.CRASHSCREEN.THANKYOU : UI.CRASHSCREEN.UPLOAD_FAILED);
		this.m_crashSubmitted = success;
	}

	// Token: 0x060068D1 RID: 26833 RVA: 0x00273F50 File Offset: 0x00272150
	public void OpenUploadingMessagee()
	{
		this.submitButton.gameObject.SetActive(false);
		this.uploadInProgress.SetActive(true);
		this.referenceMessage.SetActive(false);
		this.progressBar.fillAmount = 0f;
		this.progressText.text = UI.CRASHSCREEN.UPLOADINPROGRESS.Replace("{0}", GameUtil.GetFormattedPercent(0f, GameUtil.TimeSlice.None));
	}

	// Token: 0x060068D2 RID: 26834 RVA: 0x00273FBB File Offset: 0x002721BB
	public void OnSelect_MESSAGE()
	{
		if (!this.m_crashSubmitted)
		{
			Application.OpenURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
		}
	}

	// Token: 0x060068D3 RID: 26835 RVA: 0x00273FCF File Offset: 0x002721CF
	public string UserMessage()
	{
		return this.messageInputField.text;
	}

	// Token: 0x060068D4 RID: 26836 RVA: 0x00273FDC File Offset: 0x002721DC
	private void Submit()
	{
		this.submitAction();
		this.OpenUploadingMessagee();
	}

	// Token: 0x060068D5 RID: 26837 RVA: 0x00273FEF File Offset: 0x002721EF
	public void UpdateProgressBar(float progress)
	{
		this.progressBar.fillAmount = progress;
		this.progressText.text = UI.CRASHSCREEN.UPLOADINPROGRESS.Replace("{0}", GameUtil.GetFormattedPercent(progress * 100f, GameUtil.TimeSlice.None));
	}

	// Token: 0x040046E4 RID: 18148
	private System.Action submitAction;

	// Token: 0x040046E5 RID: 18149
	private System.Action quitAction;

	// Token: 0x040046E6 RID: 18150
	private System.Action continueAction;

	// Token: 0x040046E7 RID: 18151
	public KInputTextField messageInputField;

	// Token: 0x040046E8 RID: 18152
	[Header("Message")]
	public GameObject referenceMessage;

	// Token: 0x040046E9 RID: 18153
	public LocText messageText;

	// Token: 0x040046EA RID: 18154
	[Header("Upload Progress")]
	public GameObject uploadInProgress;

	// Token: 0x040046EB RID: 18155
	public Image progressBar;

	// Token: 0x040046EC RID: 18156
	public LocText progressText;

	// Token: 0x040046ED RID: 18157
	private string m_stackTrace;

	// Token: 0x040046EE RID: 18158
	private bool m_crashSubmitted;

	// Token: 0x040046EF RID: 18159
	[SerializeField]
	private KButton submitButton;

	// Token: 0x040046F0 RID: 18160
	[SerializeField]
	private KButton moreInfoButton;

	// Token: 0x040046F1 RID: 18161
	[SerializeField]
	private KButton quitButton;

	// Token: 0x040046F2 RID: 18162
	[SerializeField]
	private KButton continueGameButton;

	// Token: 0x040046F3 RID: 18163
	[SerializeField]
	private LocText CrashLabel;

	// Token: 0x040046F4 RID: 18164
	[SerializeField]
	private GameObject CrashDescription;

	// Token: 0x040046F5 RID: 18165
	[SerializeField]
	private GameObject ModsInfo;

	// Token: 0x040046F6 RID: 18166
	[SerializeField]
	private GameObject StackTrace;

	// Token: 0x040046F7 RID: 18167
	[SerializeField]
	private GameObject modEntryPrefab;

	// Token: 0x040046F8 RID: 18168
	[SerializeField]
	private Transform modEntryParent;

	// Token: 0x040046F9 RID: 18169
	private ReportErrorDialog.Mode mode;

	// Token: 0x02001E3C RID: 7740
	private enum Mode
	{
		// Token: 0x040089E0 RID: 35296
		SubmitError,
		// Token: 0x040089E1 RID: 35297
		DisableMods
	}
}
