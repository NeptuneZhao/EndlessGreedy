using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C4A RID: 3146
[AddComponentMenu("KMonoBehaviour/scripts/FrontEndManager")]
public class FrontEndManager : KMonoBehaviour
{
	// Token: 0x060060A6 RID: 24742 RVA: 0x0023F5A4 File Offset: 0x0023D7A4
	protected override void OnPrefabInit()
	{
		FrontEndManager.<>c__DisplayClass2_0 CS$<>8__locals1 = new FrontEndManager.<>c__DisplayClass2_0();
		CS$<>8__locals1.<>4__this = this;
		base.OnPrefabInit();
		FrontEndManager.Instance = this;
		GameObject gameObject = base.gameObject;
		string highestActiveDlcId = DlcManager.GetHighestActiveDlcId();
		if ((highestActiveDlcId != null && highestActiveDlcId.Length == 0) || !(highestActiveDlcId == "EXPANSION1_ID"))
		{
			Util.KInstantiateUI(ScreenPrefabs.Instance.MainMenuForVanilla, gameObject, true);
		}
		else
		{
			Util.KInstantiateUI(ScreenPrefabs.Instance.MainMenuForSpacedOut, gameObject, true);
		}
		if (!FrontEndManager.firstInit)
		{
			return;
		}
		FrontEndManager.firstInit = false;
		GameObject[] array = new GameObject[]
		{
			ScreenPrefabs.Instance.MainMenuIntroShort,
			ScreenPrefabs.Instance.MainMenuHealthyGameMessage,
			ScreenPrefabs.Instance.DLCBetaWarningScreen
		};
		for (int i = 0; i < array.Length; i++)
		{
			Util.KInstantiateUI(array[i], gameObject, true);
		}
		CS$<>8__locals1.screensPrefabsToSpawn = new GameObject[]
		{
			ScreenPrefabs.Instance.KleiItemDropScreen,
			ScreenPrefabs.Instance.LockerMenuScreen,
			ScreenPrefabs.Instance.LockerNavigator
		};
		CS$<>8__locals1.gameObjectsToDestroyOnNextCreate = new List<GameObject>();
		FrontEndManager.<>c__DisplayClass2_1 CS$<>8__locals2 = new FrontEndManager.<>c__DisplayClass2_1();
		CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
		CS$<>8__locals2.coroutineRunner = CoroutineRunner.Create();
		UnityEngine.Object.DontDestroyOnLoad(CS$<>8__locals2.coroutineRunner);
		CS$<>8__locals2.CS$<>8__locals1.<OnPrefabInit>g__CreateCanvases|0();
		Singleton<KBatchedAnimUpdater>.Instance.OnClear += CS$<>8__locals2.<OnPrefabInit>g__RecreateCanvases|1;
	}

	// Token: 0x060060A7 RID: 24743 RVA: 0x0023F6F7 File Offset: 0x0023D8F7
	protected override void OnForcedCleanUp()
	{
		FrontEndManager.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x060060A8 RID: 24744 RVA: 0x0023F708 File Offset: 0x0023D908
	private void LateUpdate()
	{
		if (global::Debug.developerConsoleVisible)
		{
			global::Debug.developerConsoleVisible = false;
		}
		KAnimBatchManager.Instance().UpdateActiveArea(new Vector2I(0, 0), new Vector2I(9999, 9999));
		KAnimBatchManager.Instance().UpdateDirty(Time.frameCount);
		KAnimBatchManager.Instance().Render();
	}

	// Token: 0x060060A9 RID: 24745 RVA: 0x0023F75C File Offset: 0x0023D95C
	public GameObject MakeKleiCanvas(string gameObjectName = "Canvas")
	{
		GameObject gameObject = new GameObject(gameObjectName, new Type[]
		{
			typeof(RectTransform)
		});
		this.ConfigureAsKleiCanvas(gameObject);
		return gameObject;
	}

	// Token: 0x060060AA RID: 24746 RVA: 0x0023F78C File Offset: 0x0023D98C
	public void ConfigureAsKleiCanvas(GameObject gameObject)
	{
		Canvas canvas = gameObject.AddOrGet<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.sortingOrder = 10;
		canvas.pixelPerfect = false;
		canvas.additionalShaderChannels = (AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent);
		GraphicRaycaster graphicRaycaster = gameObject.AddOrGet<GraphicRaycaster>();
		graphicRaycaster.ignoreReversedGraphics = true;
		graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
		graphicRaycaster.blockingMask = -1;
		CanvasScaler canvasScaler = gameObject.AddOrGet<CanvasScaler>();
		canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
		canvasScaler.referencePixelsPerUnit = 100f;
		gameObject.AddOrGet<KCanvasScaler>();
	}

	// Token: 0x04004159 RID: 16729
	public static FrontEndManager Instance;

	// Token: 0x0400415A RID: 16730
	public static bool firstInit = true;
}
