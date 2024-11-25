using System;
using System.IO;
using ProcGenGame;
using STRINGS;
using UnityEngine;

// Token: 0x020008FB RID: 2299
public class InitializeCheck : MonoBehaviour
{
	// Token: 0x170004DB RID: 1243
	// (get) Token: 0x06004214 RID: 16916 RVA: 0x00177BF8 File Offset: 0x00175DF8
	// (set) Token: 0x06004215 RID: 16917 RVA: 0x00177BFF File Offset: 0x00175DFF
	public static InitializeCheck.SavePathIssue savePathState { get; private set; }

	// Token: 0x06004216 RID: 16918 RVA: 0x00177C08 File Offset: 0x00175E08
	private void Awake()
	{
		this.CheckForSavePathIssue();
		if (InitializeCheck.savePathState == InitializeCheck.SavePathIssue.Ok && !KCrashReporter.hasCrash)
		{
			AudioMixer.Create();
			App.LoadScene("frontend");
			return;
		}
		Canvas cmp = base.gameObject.AddComponent<Canvas>();
		cmp.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500f);
		cmp.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 500f);
		Camera camera = base.gameObject.AddComponent<Camera>();
		camera.orthographic = true;
		camera.orthographicSize = 200f;
		camera.backgroundColor = Color.black;
		camera.clearFlags = CameraClearFlags.Color;
		camera.nearClipPlane = 0f;
		global::Debug.Log("Cannot initialize filesystem. [" + InitializeCheck.savePathState.ToString() + "]");
		Localization.Initialize();
		GameObject.Find("BootCanvas").SetActive(false);
		this.ShowFileErrorDialogs();
	}

	// Token: 0x06004217 RID: 16919 RVA: 0x00177CE1 File Offset: 0x00175EE1
	private GameObject CreateUIRoot()
	{
		return Util.KInstantiate(this.rootCanvasPrefab, null, "CanvasRoot");
	}

	// Token: 0x06004218 RID: 16920 RVA: 0x00177CF4 File Offset: 0x00175EF4
	private void ShowErrorDialog(string msg)
	{
		GameObject parent = this.CreateUIRoot();
		Util.KInstantiateUI<ConfirmDialogScreen>(this.confirmDialogScreen.gameObject, parent, true).PopupConfirmDialog(msg, new System.Action(this.Quit), null, null, null, null, null, null, this.sadDupe);
	}

	// Token: 0x06004219 RID: 16921 RVA: 0x00177D38 File Offset: 0x00175F38
	private void ShowFileErrorDialogs()
	{
		string text = null;
		switch (InitializeCheck.savePathState)
		{
		case InitializeCheck.SavePathIssue.WriteTestFail:
			text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_READ_ONLY, SaveLoader.GetSavePrefix());
			break;
		case InitializeCheck.SavePathIssue.SpaceTestFail:
			text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_INSUFFICIENT_SPACE, SaveLoader.GetSavePrefix());
			break;
		case InitializeCheck.SavePathIssue.WorldGenFilesFail:
			text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FILES, WorldGen.WORLDGEN_SAVE_FILENAME);
			break;
		}
		if (text != null)
		{
			this.ShowErrorDialog(text);
		}
	}

	// Token: 0x0600421A RID: 16922 RVA: 0x00177DB0 File Offset: 0x00175FB0
	private void CheckForSavePathIssue()
	{
		if (this.test_issue != InitializeCheck.SavePathIssue.Ok)
		{
			InitializeCheck.savePathState = this.test_issue;
			return;
		}
		string savePrefix = SaveLoader.GetSavePrefix();
		InitializeCheck.savePathState = InitializeCheck.SavePathIssue.Ok;
		try
		{
			SaveLoader.GetSavePrefixAndCreateFolder();
			using (FileStream fileStream = File.Open(savePrefix + InitializeCheck.testFile, FileMode.Create, FileAccess.Write))
			{
				new BinaryWriter(fileStream);
				fileStream.Close();
			}
		}
		catch
		{
			InitializeCheck.savePathState = InitializeCheck.SavePathIssue.WriteTestFail;
			goto IL_C8;
		}
		using (FileStream fileStream2 = File.Open(savePrefix + InitializeCheck.testSave, FileMode.Create, FileAccess.Write))
		{
			try
			{
				fileStream2.SetLength(15000000L);
				new BinaryWriter(fileStream2);
				fileStream2.Close();
			}
			catch
			{
				fileStream2.Close();
				InitializeCheck.savePathState = InitializeCheck.SavePathIssue.SpaceTestFail;
				goto IL_C8;
			}
		}
		try
		{
			using (File.Open(WorldGen.WORLDGEN_SAVE_FILENAME, FileMode.Append))
			{
			}
		}
		catch
		{
			InitializeCheck.savePathState = InitializeCheck.SavePathIssue.WorldGenFilesFail;
		}
		IL_C8:
		try
		{
			if (File.Exists(savePrefix + InitializeCheck.testFile))
			{
				File.Delete(savePrefix + InitializeCheck.testFile);
			}
			if (File.Exists(savePrefix + InitializeCheck.testSave))
			{
				File.Delete(savePrefix + InitializeCheck.testSave);
			}
		}
		catch
		{
		}
	}

	// Token: 0x0600421B RID: 16923 RVA: 0x00177F28 File Offset: 0x00176128
	private void Quit()
	{
		global::Debug.Log("Quitting...");
		App.Quit();
	}

	// Token: 0x04002BCD RID: 11213
	private static readonly string testFile = "testfile";

	// Token: 0x04002BCE RID: 11214
	private static readonly string testSave = "testsavefile";

	// Token: 0x04002BCF RID: 11215
	public Canvas rootCanvasPrefab;

	// Token: 0x04002BD0 RID: 11216
	public ConfirmDialogScreen confirmDialogScreen;

	// Token: 0x04002BD1 RID: 11217
	public Sprite sadDupe;

	// Token: 0x04002BD2 RID: 11218
	private InitializeCheck.SavePathIssue test_issue;

	// Token: 0x02001865 RID: 6245
	public enum SavePathIssue
	{
		// Token: 0x04007614 RID: 30228
		Ok,
		// Token: 0x04007615 RID: 30229
		WriteTestFail,
		// Token: 0x04007616 RID: 30230
		SpaceTestFail,
		// Token: 0x04007617 RID: 30231
		WorldGenFilesFail
	}
}
