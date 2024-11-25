using System;
using Klei;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008B7 RID: 2231
[AddComponentMenu("KMonoBehaviour/scripts/FileErrorReporter")]
public class FileErrorReporter : KMonoBehaviour
{
	// Token: 0x06003E89 RID: 16009 RVA: 0x0015A38B File Offset: 0x0015858B
	protected override void OnSpawn()
	{
		this.OnFileError();
		FileUtil.onErrorMessage += this.OnFileError;
	}

	// Token: 0x06003E8A RID: 16010 RVA: 0x0015A3A4 File Offset: 0x001585A4
	private void OnFileError()
	{
		if (FileUtil.errorType == FileUtil.ErrorType.None)
		{
			return;
		}
		string text;
		switch (FileUtil.errorType)
		{
		case FileUtil.ErrorType.UnauthorizedAccess:
			text = string.Format(FileUtil.errorSubject.Contains("OneDrive") ? UI.FRONTEND.SUPPORTWARNINGS.IO_UNAUTHORIZED_ONEDRIVE : UI.FRONTEND.SUPPORTWARNINGS.IO_UNAUTHORIZED, FileUtil.errorSubject);
			goto IL_7D;
		case FileUtil.ErrorType.IOError:
			text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.IO_SUFFICIENT_SPACE, FileUtil.errorSubject);
			goto IL_7D;
		}
		text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.IO_UNKNOWN, FileUtil.errorSubject);
		IL_7D:
		GameObject gameObject;
		if (FrontEndManager.Instance != null)
		{
			gameObject = FrontEndManager.Instance.gameObject;
		}
		else if (GameScreenManager.Instance != null && GameScreenManager.Instance.ssOverlayCanvas != null)
		{
			gameObject = GameScreenManager.Instance.ssOverlayCanvas;
		}
		else
		{
			gameObject = new GameObject();
			gameObject.name = "FileErrorCanvas";
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			Canvas canvas = gameObject.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
			canvas.sortingOrder = 10;
			gameObject.AddComponent<GraphicRaycaster>();
		}
		if ((FileUtil.exceptionMessage != null || FileUtil.exceptionStackTrace != null) && !KCrashReporter.hasReportedError)
		{
			KCrashReporter.ReportError(FileUtil.exceptionMessage, FileUtil.exceptionStackTrace, null, null, null, true, new string[]
			{
				KCrashReporter.CRASH_CATEGORY.FILEIO
			}, null);
		}
		ConfirmDialogScreen component = Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, gameObject, true).GetComponent<ConfirmDialogScreen>();
		component.PopupConfirmDialog(text, null, null, null, null, null, null, null, null);
		UnityEngine.Object.DontDestroyOnLoad(component.gameObject);
	}

	// Token: 0x06003E8B RID: 16011 RVA: 0x0015A518 File Offset: 0x00158718
	private void OpenMoreInfo()
	{
	}
}
