using System;
using System.IO;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x02000BB2 RID: 2994
public class SaveScreen : KModalScreen
{
	// Token: 0x06005AB8 RID: 23224 RVA: 0x0020E098 File Offset: 0x0020C298
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.oldSaveButtonPrefab.gameObject.SetActive(false);
		this.newSaveButton.onClick += this.OnClickNewSave;
		this.closeButton.onClick += this.Deactivate;
	}

	// Token: 0x06005AB9 RID: 23225 RVA: 0x0020E0EC File Offset: 0x0020C2EC
	protected override void OnCmpEnable()
	{
		foreach (SaveLoader.SaveFileEntry saveFileEntry in SaveLoader.GetAllColonyFiles(true, SearchOption.TopDirectoryOnly))
		{
			this.AddExistingSaveFile(saveFileEntry.path);
		}
		SpeedControlScreen.Instance.Pause(true, false);
	}

	// Token: 0x06005ABA RID: 23226 RVA: 0x0020E154 File Offset: 0x0020C354
	protected override void OnDeactivate()
	{
		SpeedControlScreen.Instance.Unpause(true);
		base.OnDeactivate();
	}

	// Token: 0x06005ABB RID: 23227 RVA: 0x0020E168 File Offset: 0x0020C368
	private void AddExistingSaveFile(string filename)
	{
		KButton kbutton = Util.KInstantiateUI<KButton>(this.oldSaveButtonPrefab.gameObject, this.oldSavesRoot.gameObject, true);
		HierarchyReferences component = kbutton.GetComponent<HierarchyReferences>();
		LocText component2 = component.GetReference<RectTransform>("Title").GetComponent<LocText>();
		TMP_Text component3 = component.GetReference<RectTransform>("Date").GetComponent<LocText>();
		System.DateTime lastWriteTime = File.GetLastWriteTime(filename);
		component2.text = string.Format("{0}", Path.GetFileNameWithoutExtension(filename));
		component3.text = string.Format("{0:H:mm:ss}" + Localization.GetFileDateFormat(0), lastWriteTime);
		kbutton.onClick += delegate()
		{
			this.Save(filename);
		};
	}

	// Token: 0x06005ABC RID: 23228 RVA: 0x0020E224 File Offset: 0x0020C424
	public static string GetValidSaveFilename(string filename)
	{
		string text = ".sav";
		if (Path.GetExtension(filename).ToLower() != text)
		{
			filename += text;
		}
		return filename;
	}

	// Token: 0x06005ABD RID: 23229 RVA: 0x0020E254 File Offset: 0x0020C454
	public void Save(string filename)
	{
		filename = SaveScreen.GetValidSaveFilename(filename);
		if (File.Exists(filename))
		{
			ScreenPrefabs.Instance.ConfirmDoAction(string.Format(UI.FRONTEND.SAVESCREEN.OVERWRITEMESSAGE, Path.GetFileNameWithoutExtension(filename)), delegate
			{
				this.DoSave(filename);
			}, base.transform.parent);
			return;
		}
		this.DoSave(filename);
	}

	// Token: 0x06005ABE RID: 23230 RVA: 0x0020E2DC File Offset: 0x0020C4DC
	private void DoSave(string filename)
	{
		try
		{
			SaveLoader.Instance.Save(filename, false, true);
			PauseScreen.Instance.OnSaveComplete();
			this.Deactivate();
		}
		catch (IOException ex)
		{
			IOException e2 = ex;
			IOException e = e2;
			Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(string.Format(UI.FRONTEND.SAVESCREEN.IO_ERROR, e.ToString()), delegate
			{
				this.Deactivate();
			}, null, UI.FRONTEND.SAVESCREEN.REPORT_BUG, delegate
			{
				KCrashReporter.ReportError(e.Message, e.StackTrace.ToString(), null, null, null, true, new string[]
				{
					KCrashReporter.CRASH_CATEGORY.FILEIO
				}, null);
			}, null, null, null, null);
		}
	}

	// Token: 0x06005ABF RID: 23231 RVA: 0x0020E39C File Offset: 0x0020C59C
	public void OnClickNewSave()
	{
		FileNameDialog fileNameDialog = (FileNameDialog)KScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.FileNameDialog.gameObject, base.transform.parent.gameObject);
		string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
		if (activeSaveFilePath != null)
		{
			string text = SaveLoader.GetOriginalSaveFileName(activeSaveFilePath);
			text = Path.GetFileNameWithoutExtension(text);
			fileNameDialog.SetTextAndSelect(text);
		}
		fileNameDialog.onConfirm = delegate(string filename)
		{
			filename = Path.Combine(SaveLoader.GetActiveSaveColonyFolder(), filename);
			this.Save(filename);
		};
	}

	// Token: 0x06005AC0 RID: 23232 RVA: 0x0020E408 File Offset: 0x0020C608
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
		}
		e.Consumed = true;
	}

	// Token: 0x06005AC1 RID: 23233 RVA: 0x0020E420 File Offset: 0x0020C620
	public override void OnKeyDown(KButtonEvent e)
	{
		e.Consumed = true;
	}

	// Token: 0x04003BB7 RID: 15287
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003BB8 RID: 15288
	[SerializeField]
	private KButton newSaveButton;

	// Token: 0x04003BB9 RID: 15289
	[SerializeField]
	private KButton oldSaveButtonPrefab;

	// Token: 0x04003BBA RID: 15290
	[SerializeField]
	private Transform oldSavesRoot;
}
