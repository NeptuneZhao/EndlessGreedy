using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02000BA9 RID: 2985
[AddComponentMenu("KMonoBehaviour/scripts/OverlayScreen")]
public class OverlayScreen : KMonoBehaviour
{
	// Token: 0x170006BE RID: 1726
	// (get) Token: 0x06005A67 RID: 23143 RVA: 0x0020CBCA File Offset: 0x0020ADCA
	public HashedString mode
	{
		get
		{
			return this.currentModeInfo.mode.ViewMode();
		}
	}

	// Token: 0x06005A68 RID: 23144 RVA: 0x0020CBDC File Offset: 0x0020ADDC
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(OverlayScreen.Instance == null);
		OverlayScreen.Instance = this;
		this.powerLabelParent = GameObject.Find("WorldSpaceCanvas").GetComponent<Canvas>();
	}

	// Token: 0x06005A69 RID: 23145 RVA: 0x0020CC09 File Offset: 0x0020AE09
	protected override void OnLoadLevel()
	{
		this.harvestableNotificationPrefab = null;
		this.powerLabelParent = null;
		OverlayScreen.Instance = null;
		OverlayModes.Mode.Clear();
		this.modeInfos = null;
		this.currentModeInfo = default(OverlayScreen.ModeInfo);
		base.OnLoadLevel();
	}

	// Token: 0x06005A6A RID: 23146 RVA: 0x0020CC40 File Offset: 0x0020AE40
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.techViewSound = KFMOD.CreateInstance(this.techViewSoundPath);
		this.techViewSoundPlaying = false;
		Shader.SetGlobalVector("_OverlayParams", Vector4.zero);
		this.RegisterModes();
		this.currentModeInfo = this.modeInfos[OverlayModes.None.ID];
	}

	// Token: 0x06005A6B RID: 23147 RVA: 0x0020CC98 File Offset: 0x0020AE98
	private void RegisterModes()
	{
		this.modeInfos.Clear();
		OverlayModes.None mode = new OverlayModes.None();
		this.RegisterMode(mode);
		this.RegisterMode(new OverlayModes.Oxygen());
		this.RegisterMode(new OverlayModes.Power(this.powerLabelParent, this.powerLabelPrefab, this.batUIPrefab, this.powerLabelOffset, this.batteryUIOffset, this.batteryUITransformerOffset, this.batteryUISmallTransformerOffset));
		this.RegisterMode(new OverlayModes.Temperature());
		this.RegisterMode(new OverlayModes.ThermalConductivity());
		this.RegisterMode(new OverlayModes.Light());
		this.RegisterMode(new OverlayModes.LiquidConduits());
		this.RegisterMode(new OverlayModes.GasConduits());
		this.RegisterMode(new OverlayModes.Decor());
		this.RegisterMode(new OverlayModes.Disease(this.powerLabelParent, this.diseaseOverlayPrefab));
		this.RegisterMode(new OverlayModes.Crop(this.powerLabelParent, this.harvestableNotificationPrefab));
		this.RegisterMode(new OverlayModes.Harvest());
		this.RegisterMode(new OverlayModes.Priorities());
		this.RegisterMode(new OverlayModes.HeatFlow());
		this.RegisterMode(new OverlayModes.Rooms());
		this.RegisterMode(new OverlayModes.Suit(this.powerLabelParent, this.suitOverlayPrefab));
		this.RegisterMode(new OverlayModes.Logic(this.logicModeUIPrefab));
		this.RegisterMode(new OverlayModes.SolidConveyor());
		this.RegisterMode(new OverlayModes.TileMode());
		this.RegisterMode(new OverlayModes.Radiation());
	}

	// Token: 0x06005A6C RID: 23148 RVA: 0x0020CDE4 File Offset: 0x0020AFE4
	private void RegisterMode(OverlayModes.Mode mode)
	{
		this.modeInfos[mode.ViewMode()] = new OverlayScreen.ModeInfo
		{
			mode = mode
		};
	}

	// Token: 0x06005A6D RID: 23149 RVA: 0x0020CE13 File Offset: 0x0020B013
	private void LateUpdate()
	{
		this.currentModeInfo.mode.Update();
	}

	// Token: 0x06005A6E RID: 23150 RVA: 0x0020CE25 File Offset: 0x0020B025
	public void RunPostProcessEffects(RenderTexture src, RenderTexture dest)
	{
		this.currentModeInfo.mode.OnRenderImage(src, dest);
	}

	// Token: 0x06005A6F RID: 23151 RVA: 0x0020CE3C File Offset: 0x0020B03C
	public void ToggleOverlay(HashedString newMode, bool allowSound = true)
	{
		bool flag = allowSound && !(this.currentModeInfo.mode.ViewMode() == newMode);
		if (newMode != OverlayModes.None.ID)
		{
			ManagementMenu.Instance.CloseAll();
		}
		this.currentModeInfo.mode.Disable();
		if (newMode != this.currentModeInfo.mode.ViewMode() && newMode == OverlayModes.None.ID)
		{
			ManagementMenu.Instance.CloseAll();
		}
		SimDebugView.Instance.SetMode(newMode);
		if (!this.modeInfos.TryGetValue(newMode, out this.currentModeInfo))
		{
			this.currentModeInfo = this.modeInfos[OverlayModes.None.ID];
		}
		this.currentModeInfo.mode.Enable();
		if (flag)
		{
			this.UpdateOverlaySounds();
		}
		if (OverlayModes.None.ID == this.currentModeInfo.mode.ViewMode())
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().TechFilterOnMigrated, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			MusicManager.instance.SetDynamicMusicOverlayInactive();
			this.techViewSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			this.techViewSoundPlaying = false;
		}
		else if (!this.techViewSoundPlaying)
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().TechFilterOnMigrated);
			MusicManager.instance.SetDynamicMusicOverlayActive();
			this.techViewSound.start();
			this.techViewSoundPlaying = true;
		}
		if (this.OnOverlayChanged != null)
		{
			this.OnOverlayChanged(this.currentModeInfo.mode.ViewMode());
		}
		this.ActivateLegend();
	}

	// Token: 0x06005A70 RID: 23152 RVA: 0x0020CFC3 File Offset: 0x0020B1C3
	private void ActivateLegend()
	{
		if (OverlayLegend.Instance == null)
		{
			return;
		}
		OverlayLegend.Instance.SetLegend(this.currentModeInfo.mode, false);
	}

	// Token: 0x06005A71 RID: 23153 RVA: 0x0020CFE9 File Offset: 0x0020B1E9
	public void Refresh()
	{
		this.LateUpdate();
	}

	// Token: 0x06005A72 RID: 23154 RVA: 0x0020CFF1 File Offset: 0x0020B1F1
	public HashedString GetMode()
	{
		if (this.currentModeInfo.mode == null)
		{
			return OverlayModes.None.ID;
		}
		return this.currentModeInfo.mode.ViewMode();
	}

	// Token: 0x06005A73 RID: 23155 RVA: 0x0020D018 File Offset: 0x0020B218
	private void UpdateOverlaySounds()
	{
		string text = this.currentModeInfo.mode.GetSoundName();
		if (text != "")
		{
			text = GlobalAssets.GetSound(text, false);
			KMonoBehaviour.PlaySound(text);
		}
	}

	// Token: 0x04003B72 RID: 15218
	public static HashSet<Tag> WireIDs = new HashSet<Tag>();

	// Token: 0x04003B73 RID: 15219
	public static HashSet<Tag> GasVentIDs = new HashSet<Tag>();

	// Token: 0x04003B74 RID: 15220
	public static HashSet<Tag> LiquidVentIDs = new HashSet<Tag>();

	// Token: 0x04003B75 RID: 15221
	public static HashSet<Tag> HarvestableIDs = new HashSet<Tag>();

	// Token: 0x04003B76 RID: 15222
	public static HashSet<Tag> DiseaseIDs = new HashSet<Tag>();

	// Token: 0x04003B77 RID: 15223
	public static HashSet<Tag> SuitIDs = new HashSet<Tag>();

	// Token: 0x04003B78 RID: 15224
	public static HashSet<Tag> SolidConveyorIDs = new HashSet<Tag>();

	// Token: 0x04003B79 RID: 15225
	public static HashSet<Tag> RadiationIDs = new HashSet<Tag>();

	// Token: 0x04003B7A RID: 15226
	[SerializeField]
	public EventReference techViewSoundPath;

	// Token: 0x04003B7B RID: 15227
	private EventInstance techViewSound;

	// Token: 0x04003B7C RID: 15228
	private bool techViewSoundPlaying;

	// Token: 0x04003B7D RID: 15229
	public static OverlayScreen Instance;

	// Token: 0x04003B7E RID: 15230
	[Header("Power")]
	[SerializeField]
	private Canvas powerLabelParent;

	// Token: 0x04003B7F RID: 15231
	[SerializeField]
	private LocText powerLabelPrefab;

	// Token: 0x04003B80 RID: 15232
	[SerializeField]
	private BatteryUI batUIPrefab;

	// Token: 0x04003B81 RID: 15233
	[SerializeField]
	private Vector3 powerLabelOffset;

	// Token: 0x04003B82 RID: 15234
	[SerializeField]
	private Vector3 batteryUIOffset;

	// Token: 0x04003B83 RID: 15235
	[SerializeField]
	private Vector3 batteryUITransformerOffset;

	// Token: 0x04003B84 RID: 15236
	[SerializeField]
	private Vector3 batteryUISmallTransformerOffset;

	// Token: 0x04003B85 RID: 15237
	[SerializeField]
	private Color consumerColour;

	// Token: 0x04003B86 RID: 15238
	[SerializeField]
	private Color generatorColour;

	// Token: 0x04003B87 RID: 15239
	[SerializeField]
	private Color buildingDisabledColour = Color.gray;

	// Token: 0x04003B88 RID: 15240
	[Header("Circuits")]
	[SerializeField]
	private Color32 circuitUnpoweredColour;

	// Token: 0x04003B89 RID: 15241
	[SerializeField]
	private Color32 circuitSafeColour;

	// Token: 0x04003B8A RID: 15242
	[SerializeField]
	private Color32 circuitStrainingColour;

	// Token: 0x04003B8B RID: 15243
	[SerializeField]
	private Color32 circuitOverloadingColour;

	// Token: 0x04003B8C RID: 15244
	[Header("Crops")]
	[SerializeField]
	private GameObject harvestableNotificationPrefab;

	// Token: 0x04003B8D RID: 15245
	[Header("Disease")]
	[SerializeField]
	private GameObject diseaseOverlayPrefab;

	// Token: 0x04003B8E RID: 15246
	[Header("Suit")]
	[SerializeField]
	private GameObject suitOverlayPrefab;

	// Token: 0x04003B8F RID: 15247
	[Header("ToolTip")]
	[SerializeField]
	private TextStyleSetting TooltipHeader;

	// Token: 0x04003B90 RID: 15248
	[SerializeField]
	private TextStyleSetting TooltipDescription;

	// Token: 0x04003B91 RID: 15249
	[Header("Logic")]
	[SerializeField]
	private LogicModeUI logicModeUIPrefab;

	// Token: 0x04003B92 RID: 15250
	public Action<HashedString> OnOverlayChanged;

	// Token: 0x04003B93 RID: 15251
	private OverlayScreen.ModeInfo currentModeInfo;

	// Token: 0x04003B94 RID: 15252
	private Dictionary<HashedString, OverlayScreen.ModeInfo> modeInfos = new Dictionary<HashedString, OverlayScreen.ModeInfo>();

	// Token: 0x02001C38 RID: 7224
	private struct ModeInfo
	{
		// Token: 0x04008274 RID: 33396
		public OverlayModes.Mode mode;
	}
}
