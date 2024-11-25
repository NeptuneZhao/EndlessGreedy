using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000B9F RID: 2975
public class LoadingOverlay : KModalScreen
{
	// Token: 0x060059FD RID: 23037 RVA: 0x00209AAE File Offset: 0x00207CAE
	protected override void OnPrefabInit()
	{
		this.pause = false;
		this.fadeIn = false;
		base.OnPrefabInit();
	}

	// Token: 0x060059FE RID: 23038 RVA: 0x00209AC4 File Offset: 0x00207CC4
	private void Update()
	{
		if (!this.loadNextFrame && this.showLoad)
		{
			this.loadNextFrame = true;
			this.showLoad = false;
			return;
		}
		if (this.loadNextFrame)
		{
			this.loadNextFrame = false;
			this.loadCb();
		}
	}

	// Token: 0x060059FF RID: 23039 RVA: 0x00209AFF File Offset: 0x00207CFF
	public static void DestroyInstance()
	{
		LoadingOverlay.instance = null;
	}

	// Token: 0x06005A00 RID: 23040 RVA: 0x00209B08 File Offset: 0x00207D08
	public static void Load(System.Action cb)
	{
		GameObject gameObject = GameObject.Find("/SceneInitializerFE/FrontEndManager");
		if (LoadingOverlay.instance == null)
		{
			LoadingOverlay.instance = Util.KInstantiateUI<LoadingOverlay>(ScreenPrefabs.Instance.loadingOverlay.gameObject, (GameScreenManager.Instance == null) ? gameObject : GameScreenManager.Instance.ssOverlayCanvas, false);
			LoadingOverlay.instance.GetComponentInChildren<LocText>().SetText(UI.FRONTEND.LOADING);
		}
		if (GameScreenManager.Instance != null)
		{
			LoadingOverlay.instance.transform.SetParent(GameScreenManager.Instance.ssOverlayCanvas.transform);
			LoadingOverlay.instance.transform.SetSiblingIndex(GameScreenManager.Instance.ssOverlayCanvas.transform.childCount - 1);
		}
		else
		{
			LoadingOverlay.instance.transform.SetParent(gameObject.transform);
			LoadingOverlay.instance.transform.SetSiblingIndex(gameObject.transform.childCount - 1);
			if (MainMenu.Instance != null)
			{
				MainMenu.Instance.StopAmbience();
			}
		}
		LoadingOverlay.instance.loadCb = cb;
		LoadingOverlay.instance.showLoad = true;
		LoadingOverlay.instance.Activate();
	}

	// Token: 0x06005A01 RID: 23041 RVA: 0x00209C34 File Offset: 0x00207E34
	public static void Clear()
	{
		if (LoadingOverlay.instance != null)
		{
			LoadingOverlay.instance.Deactivate();
		}
	}

	// Token: 0x04003B1E RID: 15134
	private bool loadNextFrame;

	// Token: 0x04003B1F RID: 15135
	private bool showLoad;

	// Token: 0x04003B20 RID: 15136
	private System.Action loadCb;

	// Token: 0x04003B21 RID: 15137
	private static LoadingOverlay instance;
}
