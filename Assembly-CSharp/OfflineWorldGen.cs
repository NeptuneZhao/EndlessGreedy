using System;
using System.Collections.Generic;
using System.Threading;
using Klei.CustomSettings;
using ProcGenGame;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x02000E02 RID: 3586
[AddComponentMenu("KMonoBehaviour/scripts/OfflineWorldGen")]
public class OfflineWorldGen : KMonoBehaviour
{
	// Token: 0x060071C3 RID: 29123 RVA: 0x002B13B4 File Offset: 0x002AF5B4
	private void TrackProgress(string text)
	{
		if (this.trackProgress)
		{
			global::Debug.Log(text);
		}
	}

	// Token: 0x060071C4 RID: 29124 RVA: 0x002B13C4 File Offset: 0x002AF5C4
	public static bool CanLoadSave()
	{
		bool flag = WorldGen.CanLoad(SaveLoader.GetActiveSaveFilePath());
		if (!flag)
		{
			SaveLoader.SetActiveSaveFilePath(null);
			flag = WorldGen.CanLoad(WorldGen.WORLDGEN_SAVE_FILENAME);
		}
		return flag;
	}

	// Token: 0x060071C5 RID: 29125 RVA: 0x002B13F4 File Offset: 0x002AF5F4
	public void Generate()
	{
		this.doWorldGen = !OfflineWorldGen.CanLoadSave();
		this.updateText.gameObject.SetActive(false);
		this.percentText.gameObject.SetActive(false);
		this.doWorldGen |= this.debug;
		if (this.doWorldGen)
		{
			this.seedText.text = string.Format(UI.WORLDGEN.USING_PLAYER_SEED, this.seed);
			this.titleText.text = UI.FRONTEND.WORLDGENSCREEN.TITLE.ToString();
			this.mainText.text = UI.WORLDGEN.CHOOSEWORLDSIZE.ToString();
			for (int i = 0; i < this.validDimensions.Length; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab);
				gameObject.SetActive(true);
				RectTransform component = gameObject.GetComponent<RectTransform>();
				component.SetParent(this.buttonRoot);
				component.localScale = Vector3.one;
				TMP_Text componentInChildren = gameObject.GetComponentInChildren<LocText>();
				OfflineWorldGen.ValidDimensions validDimensions = this.validDimensions[i];
				componentInChildren.text = validDimensions.name.ToString();
				int idx = i;
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.DoWorldGen(idx);
					this.ToggleGenerationUI();
				};
			}
			if (this.validDimensions.Length == 1)
			{
				this.DoWorldGen(0);
				this.ToggleGenerationUI();
			}
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
			this.OnResize();
		}
		else
		{
			this.titleText.text = UI.FRONTEND.WORLDGENSCREEN.LOADINGGAME.ToString();
			this.mainText.gameObject.SetActive(false);
			this.currentConvertedCurrentStage = UI.WORLDGEN.COMPLETE.key;
			this.currentPercent = 1f;
			this.updateText.gameObject.SetActive(false);
			this.percentText.gameObject.SetActive(false);
			this.RemoveButtons();
		}
		this.buttonPrefab.SetActive(false);
	}

	// Token: 0x060071C6 RID: 29126 RVA: 0x002B15F4 File Offset: 0x002AF7F4
	private void OnResize()
	{
		float canvasScale = base.GetComponentInParent<KCanvasScaler>().GetCanvasScale();
		if (this.asteriodAnim != null)
		{
			this.asteriodAnim.animScale = 0.005f * (1f / canvasScale);
		}
	}

	// Token: 0x060071C7 RID: 29127 RVA: 0x002B163C File Offset: 0x002AF83C
	private void ToggleGenerationUI()
	{
		this.percentText.gameObject.SetActive(false);
		this.updateText.gameObject.SetActive(true);
		this.titleText.text = UI.FRONTEND.WORLDGENSCREEN.GENERATINGWORLD.ToString();
		if (this.titleText != null && this.titleText.gameObject != null)
		{
			this.titleText.gameObject.SetActive(false);
		}
		if (this.buttonRoot != null && this.buttonRoot.gameObject != null)
		{
			this.buttonRoot.gameObject.SetActive(false);
		}
	}

	// Token: 0x060071C8 RID: 29128 RVA: 0x002B16E4 File Offset: 0x002AF8E4
	private bool UpdateProgress(StringKey stringKeyRoot, float completePercent, WorldGenProgressStages.Stages stage)
	{
		if (this.currentStage != stage)
		{
			this.currentStage = stage;
		}
		if (this.currentStringKeyRoot.Hash != stringKeyRoot.Hash)
		{
			this.currentConvertedCurrentStage = stringKeyRoot;
			this.currentStringKeyRoot = stringKeyRoot;
		}
		else
		{
			int num = (int)completePercent * 10;
			LocString locString = this.convertList.Find((LocString s) => s.key.Hash == stringKeyRoot.Hash);
			if (num != 0 && locString != null)
			{
				this.currentConvertedCurrentStage = new StringKey(locString.key.String + num.ToString());
			}
		}
		float num2 = 0f;
		float num3 = 0f;
		float num4 = WorldGenProgressStages.StageWeights[(int)stage].Value * completePercent;
		for (int i = 0; i < WorldGenProgressStages.StageWeights.Length; i++)
		{
			num3 += WorldGenProgressStages.StageWeights[i].Value;
			if (i < (int)this.currentStage)
			{
				num2 += WorldGenProgressStages.StageWeights[i].Value;
			}
		}
		float num5 = (num2 + num4) / num3;
		this.currentPercent = num5;
		return !this.shouldStop;
	}

	// Token: 0x060071C9 RID: 29129 RVA: 0x002B180C File Offset: 0x002AFA0C
	private void Update()
	{
		if (this.loadTriggered)
		{
			return;
		}
		if (this.currentConvertedCurrentStage.String == null)
		{
			return;
		}
		this.errorMutex.WaitOne();
		int count = this.errors.Count;
		this.errorMutex.ReleaseMutex();
		if (count > 0)
		{
			this.DoExitFlow();
			return;
		}
		this.updateText.text = Strings.Get(this.currentConvertedCurrentStage.String);
		if (!this.debug && this.currentConvertedCurrentStage.Hash == UI.WORLDGEN.COMPLETE.key.Hash && this.currentPercent >= 1f && this.cluster.IsGenerationComplete)
		{
			if (KCrashReporter.terminateOnError && KCrashReporter.hasCrash)
			{
				return;
			}
			this.percentText.text = "";
			this.loadTriggered = true;
			App.LoadScene(this.mainGameLevel);
			return;
		}
		else
		{
			if (this.currentPercent < 0f)
			{
				this.DoExitFlow();
				return;
			}
			if (this.currentPercent > 0f && !this.percentText.gameObject.activeSelf)
			{
				this.percentText.gameObject.SetActive(false);
			}
			this.percentText.text = GameUtil.GetFormattedPercent(this.currentPercent * 100f, GameUtil.TimeSlice.None);
			this.meterAnim.SetPositionPercent(this.currentPercent);
			return;
		}
	}

	// Token: 0x060071CA RID: 29130 RVA: 0x002B1960 File Offset: 0x002AFB60
	private void DisplayErrors()
	{
		this.errorMutex.WaitOne();
		if (this.errors.Count > 0)
		{
			foreach (OfflineWorldGen.ErrorInfo errorInfo in this.errors)
			{
				Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, FrontEndManager.Instance.gameObject, true).PopupConfirmDialog(errorInfo.errorDesc, new System.Action(this.OnConfirmExit), null, null, null, null, null, null, null);
			}
		}
		this.errorMutex.ReleaseMutex();
	}

	// Token: 0x060071CB RID: 29131 RVA: 0x002B1A10 File Offset: 0x002AFC10
	private void DoExitFlow()
	{
		if (this.startedExitFlow)
		{
			return;
		}
		this.startedExitFlow = true;
		this.percentText.text = UI.WORLDGEN.RESTARTING.ToString();
		this.loadTriggered = true;
		Sim.Shutdown();
		this.DisplayErrors();
	}

	// Token: 0x060071CC RID: 29132 RVA: 0x002B1A49 File Offset: 0x002AFC49
	private void OnConfirmExit()
	{
		App.LoadScene(this.frontendGameLevel);
	}

	// Token: 0x060071CD RID: 29133 RVA: 0x002B1A58 File Offset: 0x002AFC58
	private void RemoveButtons()
	{
		for (int i = this.buttonRoot.childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(this.buttonRoot.GetChild(i).gameObject);
		}
	}

	// Token: 0x060071CE RID: 29134 RVA: 0x002B1A93 File Offset: 0x002AFC93
	private void DoWorldGen(int selectedDimension)
	{
		this.RemoveButtons();
		this.DoWorldGenInitialize();
	}

	// Token: 0x060071CF RID: 29135 RVA: 0x002B1AA4 File Offset: 0x002AFCA4
	private void DoWorldGenInitialize()
	{
		string clusterName = "";
		Func<int, WorldGen, bool> shouldSkipWorldCallback = null;
		this.seed = CustomGameSettings.Instance.GetCurrentWorldgenSeed();
		clusterName = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout).id;
		List<string> list = new List<string>();
		foreach (string id in CustomGameSettings.Instance.GetCurrentStories())
		{
			list.Add(Db.Get().Stories.Get(id).worldgenStoryTraitKey);
		}
		this.cluster = new Cluster(clusterName, this.seed, list, true, false, false);
		this.cluster.ShouldSkipWorldCallback = shouldSkipWorldCallback;
		this.cluster.Generate(new WorldGen.OfflineCallbackFunction(this.UpdateProgress), new Action<OfflineWorldGen.ErrorInfo>(this.OnError), this.seed, this.seed, this.seed, this.seed, true, false, false);
	}

	// Token: 0x060071D0 RID: 29136 RVA: 0x002B1BA4 File Offset: 0x002AFDA4
	private void OnError(OfflineWorldGen.ErrorInfo error)
	{
		this.errorMutex.WaitOne();
		this.errors.Add(error);
		this.errorMutex.ReleaseMutex();
	}

	// Token: 0x04004E7B RID: 20091
	[SerializeField]
	private RectTransform buttonRoot;

	// Token: 0x04004E7C RID: 20092
	[SerializeField]
	private GameObject buttonPrefab;

	// Token: 0x04004E7D RID: 20093
	[SerializeField]
	private RectTransform chooseLocationPanel;

	// Token: 0x04004E7E RID: 20094
	[SerializeField]
	private GameObject locationButtonPrefab;

	// Token: 0x04004E7F RID: 20095
	private const float baseScale = 0.005f;

	// Token: 0x04004E80 RID: 20096
	private Mutex errorMutex = new Mutex();

	// Token: 0x04004E81 RID: 20097
	private List<OfflineWorldGen.ErrorInfo> errors = new List<OfflineWorldGen.ErrorInfo>();

	// Token: 0x04004E82 RID: 20098
	private OfflineWorldGen.ValidDimensions[] validDimensions = new OfflineWorldGen.ValidDimensions[]
	{
		new OfflineWorldGen.ValidDimensions
		{
			width = 256,
			height = 384,
			name = UI.FRONTEND.WORLDGENSCREEN.SIZES.STANDARD.key
		}
	};

	// Token: 0x04004E83 RID: 20099
	public string frontendGameLevel = "frontend";

	// Token: 0x04004E84 RID: 20100
	public string mainGameLevel = "backend";

	// Token: 0x04004E85 RID: 20101
	private bool shouldStop;

	// Token: 0x04004E86 RID: 20102
	private StringKey currentConvertedCurrentStage;

	// Token: 0x04004E87 RID: 20103
	private float currentPercent;

	// Token: 0x04004E88 RID: 20104
	public bool debug;

	// Token: 0x04004E89 RID: 20105
	private bool trackProgress = true;

	// Token: 0x04004E8A RID: 20106
	private bool doWorldGen;

	// Token: 0x04004E8B RID: 20107
	[SerializeField]
	private LocText titleText;

	// Token: 0x04004E8C RID: 20108
	[SerializeField]
	private LocText mainText;

	// Token: 0x04004E8D RID: 20109
	[SerializeField]
	private LocText updateText;

	// Token: 0x04004E8E RID: 20110
	[SerializeField]
	private LocText percentText;

	// Token: 0x04004E8F RID: 20111
	[SerializeField]
	private LocText seedText;

	// Token: 0x04004E90 RID: 20112
	[SerializeField]
	private KBatchedAnimController meterAnim;

	// Token: 0x04004E91 RID: 20113
	[SerializeField]
	private KBatchedAnimController asteriodAnim;

	// Token: 0x04004E92 RID: 20114
	private Cluster cluster;

	// Token: 0x04004E93 RID: 20115
	private StringKey currentStringKeyRoot;

	// Token: 0x04004E94 RID: 20116
	private List<LocString> convertList = new List<LocString>
	{
		UI.WORLDGEN.SETTLESIM,
		UI.WORLDGEN.BORDERS,
		UI.WORLDGEN.PROCESSING,
		UI.WORLDGEN.COMPLETELAYOUT,
		UI.WORLDGEN.WORLDLAYOUT,
		UI.WORLDGEN.GENERATENOISE,
		UI.WORLDGEN.BUILDNOISESOURCE,
		UI.WORLDGEN.GENERATESOLARSYSTEM
	};

	// Token: 0x04004E95 RID: 20117
	private WorldGenProgressStages.Stages currentStage;

	// Token: 0x04004E96 RID: 20118
	private bool loadTriggered;

	// Token: 0x04004E97 RID: 20119
	private bool startedExitFlow;

	// Token: 0x04004E98 RID: 20120
	private int seed;

	// Token: 0x02001F06 RID: 7942
	public struct ErrorInfo
	{
		// Token: 0x04008C66 RID: 35942
		public string errorDesc;

		// Token: 0x04008C67 RID: 35943
		public Exception exception;
	}

	// Token: 0x02001F07 RID: 7943
	[Serializable]
	private struct ValidDimensions
	{
		// Token: 0x04008C68 RID: 35944
		public int width;

		// Token: 0x04008C69 RID: 35945
		public int height;

		// Token: 0x04008C6A RID: 35946
		public StringKey name;
	}
}
