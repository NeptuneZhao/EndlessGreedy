using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B9C RID: 2972
public class InspectSaveScreen : KModalScreen
{
	// Token: 0x060059AE RID: 22958 RVA: 0x00206DA5 File Offset: 0x00204FA5
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.closeButton.onClick += this.CloseScreen;
		this.deleteSaveBtn.onClick += this.DeleteSave;
	}

	// Token: 0x060059AF RID: 22959 RVA: 0x00206DDB File Offset: 0x00204FDB
	private void CloseScreen()
	{
		LoadScreen.Instance.Show(true);
		this.Show(false);
	}

	// Token: 0x060059B0 RID: 22960 RVA: 0x00206DEF File Offset: 0x00204FEF
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			this.buttonPool.ClearAll();
			this.buttonFileMap.Clear();
		}
	}

	// Token: 0x060059B1 RID: 22961 RVA: 0x00206E14 File Offset: 0x00205014
	public void SetTarget(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			global::Debug.LogError("The directory path provided is empty.");
			this.Show(false);
			return;
		}
		if (!Directory.Exists(path))
		{
			global::Debug.LogError("The directory provided does not exist.");
			this.Show(false);
			return;
		}
		if (this.buttonPool == null)
		{
			this.buttonPool = new UIPool<KButton>(this.backupBtnPrefab);
		}
		this.currentPath = path;
		List<string> list = (from filename in Directory.GetFiles(path)
		where Path.GetExtension(filename).ToLower() == ".sav"
		orderby File.GetLastWriteTime(filename) descending
		select filename).ToList<string>();
		string text = list[0];
		if (File.Exists(text))
		{
			this.mainSaveBtn.gameObject.SetActive(true);
			this.AddNewSave(this.mainSaveBtn, text);
		}
		else
		{
			this.mainSaveBtn.gameObject.SetActive(false);
		}
		if (list.Count > 1)
		{
			for (int i = 1; i < list.Count; i++)
			{
				this.AddNewSave(this.buttonPool.GetFreeElement(this.buttonGroup, true), list[i]);
			}
		}
		this.Show(true);
	}

	// Token: 0x060059B2 RID: 22962 RVA: 0x00206F4C File Offset: 0x0020514C
	private void ConfirmDoAction(string message, System.Action action)
	{
		if (this.confirmScreen == null)
		{
			this.confirmScreen = Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, false);
			this.confirmScreen.PopupConfirmDialog(message, action, delegate
			{
			}, null, null, null, null, null, null);
			this.confirmScreen.GetComponent<LayoutElement>().ignoreLayout = true;
			this.confirmScreen.gameObject.SetActive(true);
		}
	}

	// Token: 0x060059B3 RID: 22963 RVA: 0x00206FDC File Offset: 0x002051DC
	private void DeleteSave()
	{
		if (string.IsNullOrEmpty(this.currentPath))
		{
			global::Debug.LogError("The path provided is not valid and cannot be deleted.");
			return;
		}
		this.ConfirmDoAction(UI.FRONTEND.LOADSCREEN.CONFIRMDELETE, delegate
		{
			string[] files = Directory.GetFiles(this.currentPath);
			for (int i = 0; i < files.Length; i++)
			{
				File.Delete(files[i]);
			}
			Directory.Delete(this.currentPath);
			this.CloseScreen();
		});
	}

	// Token: 0x060059B4 RID: 22964 RVA: 0x00207012 File Offset: 0x00205212
	private void AddNewSave(KButton btn, string file)
	{
	}

	// Token: 0x060059B5 RID: 22965 RVA: 0x00207014 File Offset: 0x00205214
	private void ButtonClicked(KButton btn)
	{
		LoadingOverlay.Load(delegate
		{
			this.Load(this.buttonFileMap[btn]);
		});
	}

	// Token: 0x060059B6 RID: 22966 RVA: 0x00207039 File Offset: 0x00205239
	private void Load(string filename)
	{
		if (Game.Instance != null)
		{
			LoadScreen.ForceStopGame();
		}
		SaveLoader.SetActiveSaveFilePath(filename);
		App.LoadScene("backend");
		this.Deactivate();
	}

	// Token: 0x060059B7 RID: 22967 RVA: 0x00207063 File Offset: 0x00205263
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.CloseScreen();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x04003AF8 RID: 15096
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003AF9 RID: 15097
	[SerializeField]
	private KButton mainSaveBtn;

	// Token: 0x04003AFA RID: 15098
	[SerializeField]
	private KButton backupBtnPrefab;

	// Token: 0x04003AFB RID: 15099
	[SerializeField]
	private KButton deleteSaveBtn;

	// Token: 0x04003AFC RID: 15100
	[SerializeField]
	private GameObject buttonGroup;

	// Token: 0x04003AFD RID: 15101
	private UIPool<KButton> buttonPool;

	// Token: 0x04003AFE RID: 15102
	private Dictionary<KButton, string> buttonFileMap = new Dictionary<KButton, string>();

	// Token: 0x04003AFF RID: 15103
	private ConfirmDialogScreen confirmScreen;

	// Token: 0x04003B00 RID: 15104
	private string currentPath = "";
}
