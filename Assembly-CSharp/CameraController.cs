using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

// Token: 0x02000533 RID: 1331
[AddComponentMenu("KMonoBehaviour/scripts/CameraController")]
public class CameraController : KMonoBehaviour, IInputHandler
{
	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06001E0B RID: 7691 RVA: 0x000A68F8 File Offset: 0x000A4AF8
	public string handlerName
	{
		get
		{
			return base.gameObject.name;
		}
	}

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06001E0C RID: 7692 RVA: 0x000A6905 File Offset: 0x000A4B05
	// (set) Token: 0x06001E0D RID: 7693 RVA: 0x000A6928 File Offset: 0x000A4B28
	public float OrthographicSize
	{
		get
		{
			if (!(this.baseCamera == null))
			{
				return this.baseCamera.orthographicSize;
			}
			return 0f;
		}
		set
		{
			for (int i = 0; i < this.cameras.Count; i++)
			{
				this.cameras[i].orthographicSize = value;
			}
		}
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06001E0E RID: 7694 RVA: 0x000A695D File Offset: 0x000A4B5D
	// (set) Token: 0x06001E0F RID: 7695 RVA: 0x000A6965 File Offset: 0x000A4B65
	public KInputHandler inputHandler { get; set; }

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x06001E10 RID: 7696 RVA: 0x000A696E File Offset: 0x000A4B6E
	// (set) Token: 0x06001E11 RID: 7697 RVA: 0x000A6976 File Offset: 0x000A4B76
	public float targetOrthographicSize { get; private set; }

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x06001E12 RID: 7698 RVA: 0x000A697F File Offset: 0x000A4B7F
	// (set) Token: 0x06001E13 RID: 7699 RVA: 0x000A6987 File Offset: 0x000A4B87
	public bool isTargetPosSet { get; set; }

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06001E14 RID: 7700 RVA: 0x000A6990 File Offset: 0x000A4B90
	// (set) Token: 0x06001E15 RID: 7701 RVA: 0x000A6998 File Offset: 0x000A4B98
	public Vector3 targetPos { get; private set; }

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x06001E16 RID: 7702 RVA: 0x000A69A1 File Offset: 0x000A4BA1
	// (set) Token: 0x06001E17 RID: 7703 RVA: 0x000A69A9 File Offset: 0x000A4BA9
	public bool ignoreClusterFX { get; private set; }

	// Token: 0x06001E18 RID: 7704 RVA: 0x000A69B2 File Offset: 0x000A4BB2
	public void ToggleClusterFX()
	{
		this.ignoreClusterFX = !this.ignoreClusterFX;
	}

	// Token: 0x06001E19 RID: 7705 RVA: 0x000A69C4 File Offset: 0x000A4BC4
	protected override void OnForcedCleanUp()
	{
		GameInputManager inputManager = Global.GetInputManager();
		if (inputManager == null)
		{
			return;
		}
		inputManager.usedMenus.Remove(this);
	}

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x06001E1A RID: 7706 RVA: 0x000A69E8 File Offset: 0x000A4BE8
	public int cameraActiveCluster
	{
		get
		{
			if (ClusterManager.Instance == null)
			{
				return 255;
			}
			return ClusterManager.Instance.activeWorldId;
		}
	}

	// Token: 0x06001E1B RID: 7707 RVA: 0x000A6A08 File Offset: 0x000A4C08
	public void GetWorldCamera(out Vector2I worldOffset, out Vector2I worldSize)
	{
		WorldContainer worldContainer = null;
		if (ClusterManager.Instance != null)
		{
			worldContainer = ClusterManager.Instance.activeWorld;
		}
		if (!this.ignoreClusterFX && worldContainer != null)
		{
			worldOffset = worldContainer.WorldOffset;
			worldSize = worldContainer.WorldSize;
			return;
		}
		worldOffset = new Vector2I(0, 0);
		worldSize = new Vector2I(Grid.WidthInCells, Grid.HeightInCells);
	}

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x06001E1C RID: 7708 RVA: 0x000A6A7B File Offset: 0x000A4C7B
	// (set) Token: 0x06001E1D RID: 7709 RVA: 0x000A6A83 File Offset: 0x000A4C83
	public bool DisableUserCameraControl
	{
		get
		{
			return this.userCameraControlDisabled;
		}
		set
		{
			this.userCameraControlDisabled = value;
			if (this.userCameraControlDisabled)
			{
				this.panning = false;
				this.panLeft = false;
				this.panRight = false;
				this.panUp = false;
				this.panDown = false;
			}
		}
	}

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06001E1E RID: 7710 RVA: 0x000A6AB7 File Offset: 0x000A4CB7
	// (set) Token: 0x06001E1F RID: 7711 RVA: 0x000A6ABE File Offset: 0x000A4CBE
	public static CameraController Instance { get; private set; }

	// Token: 0x06001E20 RID: 7712 RVA: 0x000A6AC6 File Offset: 0x000A4CC6
	public static void DestroyInstance()
	{
		CameraController.Instance = null;
	}

	// Token: 0x06001E21 RID: 7713 RVA: 0x000A6ACE File Offset: 0x000A4CCE
	public void ToggleColouredOverlayView(bool enabled)
	{
		this.mrt.ToggleColouredOverlayView(enabled);
	}

	// Token: 0x06001E22 RID: 7714 RVA: 0x000A6ADC File Offset: 0x000A4CDC
	protected override void OnPrefabInit()
	{
		global::Util.Reset(base.transform);
		base.transform.SetLocalPosition(new Vector3(Grid.WidthInMeters / 2f, Grid.HeightInMeters / 2f, -100f));
		this.targetOrthographicSize = this.maxOrthographicSize;
		CameraController.Instance = this;
		this.DisableUserCameraControl = false;
		this.baseCamera = this.CopyCamera(Camera.main, "baseCamera");
		this.mrt = this.baseCamera.gameObject.AddComponent<MultipleRenderTarget>();
		this.mrt.onSetupComplete += this.OnMRTSetupComplete;
		this.baseCamera.gameObject.AddComponent<LightBufferCompositor>();
		this.baseCamera.transparencySortMode = TransparencySortMode.Orthographic;
		this.baseCamera.transform.parent = base.transform;
		global::Util.Reset(this.baseCamera.transform);
		int mask = LayerMask.GetMask(new string[]
		{
			"PlaceWithDepth",
			"Overlay"
		});
		int mask2 = LayerMask.GetMask(new string[]
		{
			"Construction"
		});
		this.baseCamera.cullingMask &= ~mask;
		this.baseCamera.cullingMask |= mask2;
		this.baseCamera.tag = "Untagged";
		this.baseCamera.gameObject.AddComponent<CameraRenderTexture>().TextureName = "_LitTex";
		this.infraredCamera = this.CopyCamera(this.baseCamera, "Infrared");
		this.infraredCamera.cullingMask = 0;
		this.infraredCamera.clearFlags = CameraClearFlags.Color;
		this.infraredCamera.depth = this.baseCamera.depth - 1f;
		this.infraredCamera.transform.parent = base.transform;
		this.infraredCamera.gameObject.AddComponent<Infrared>();
		if (SimDebugView.Instance != null)
		{
			this.simOverlayCamera = this.CopyCamera(this.baseCamera, "SimOverlayCamera");
			this.simOverlayCamera.cullingMask = LayerMask.GetMask(new string[]
			{
				"SimDebugView"
			});
			this.simOverlayCamera.clearFlags = CameraClearFlags.Color;
			this.simOverlayCamera.depth = this.baseCamera.depth + 1f;
			this.simOverlayCamera.transform.parent = base.transform;
			this.simOverlayCamera.gameObject.AddComponent<CameraRenderTexture>().TextureName = "_SimDebugViewTex";
		}
		this.overlayCamera = Camera.main;
		this.overlayCamera.name = "Overlay";
		this.overlayCamera.cullingMask = (mask | mask2);
		this.overlayCamera.clearFlags = CameraClearFlags.Nothing;
		this.overlayCamera.transform.parent = base.transform;
		this.overlayCamera.depth = this.baseCamera.depth + 3f;
		this.overlayCamera.transform.SetLocalPosition(Vector3.zero);
		this.overlayCamera.transform.localRotation = Quaternion.identity;
		this.overlayCamera.renderingPath = RenderingPath.Forward;
		this.overlayCamera.allowHDR = false;
		this.overlayCamera.tag = "Untagged";
		this.overlayCamera.gameObject.AddComponent<CameraReferenceTexture>().referenceCamera = this.baseCamera;
		ColorCorrectionLookup component = this.overlayCamera.GetComponent<ColorCorrectionLookup>();
		component.Convert(this.dayColourCube, "");
		component.Convert2(this.nightColourCube, "");
		this.cameras.Add(this.overlayCamera);
		this.lightBufferCamera = this.CopyCamera(this.overlayCamera, "Light Buffer");
		this.lightBufferCamera.clearFlags = CameraClearFlags.Color;
		this.lightBufferCamera.cullingMask = LayerMask.GetMask(new string[]
		{
			"Lights"
		});
		this.lightBufferCamera.depth = this.baseCamera.depth - 1f;
		this.lightBufferCamera.transform.parent = base.transform;
		this.lightBufferCamera.transform.SetLocalPosition(Vector3.zero);
		this.lightBufferCamera.rect = new Rect(0f, 0f, 1f, 1f);
		LightBuffer lightBuffer = this.lightBufferCamera.gameObject.AddComponent<LightBuffer>();
		lightBuffer.Material = this.LightBufferMaterial;
		lightBuffer.CircleMaterial = this.LightCircleOverlay;
		lightBuffer.ConeMaterial = this.LightConeOverlay;
		this.overlayNoDepthCamera = this.CopyCamera(this.overlayCamera, "overlayNoDepth");
		int mask3 = LayerMask.GetMask(new string[]
		{
			"Overlay",
			"Place"
		});
		this.baseCamera.cullingMask &= ~mask3;
		this.overlayNoDepthCamera.clearFlags = CameraClearFlags.Depth;
		this.overlayNoDepthCamera.cullingMask = mask3;
		this.overlayNoDepthCamera.transform.parent = base.transform;
		this.overlayNoDepthCamera.transform.SetLocalPosition(Vector3.zero);
		this.overlayNoDepthCamera.depth = this.baseCamera.depth + 4f;
		this.overlayNoDepthCamera.tag = "MainCamera";
		this.overlayNoDepthCamera.gameObject.AddComponent<NavPathDrawer>();
		this.overlayNoDepthCamera.gameObject.AddComponent<RangeVisualizerEffect>();
		this.overlayNoDepthCamera.gameObject.AddComponent<SkyVisibilityVisualizerEffect>();
		this.overlayNoDepthCamera.gameObject.AddComponent<ScannerNetworkVisualizerEffect>();
		this.overlayNoDepthCamera.gameObject.AddComponent<RocketLaunchConditionVisualizerEffect>();
		this.uiCamera = this.CopyCamera(this.overlayCamera, "uiCamera");
		this.uiCamera.clearFlags = CameraClearFlags.Depth;
		this.uiCamera.cullingMask = LayerMask.GetMask(new string[]
		{
			"UI"
		});
		this.uiCamera.transform.parent = base.transform;
		this.uiCamera.transform.SetLocalPosition(Vector3.zero);
		this.uiCamera.depth = this.baseCamera.depth + 5f;
		if (Game.Instance != null)
		{
			this.timelapseFreezeCamera = this.CopyCamera(this.uiCamera, "timelapseFreezeCamera");
			this.timelapseFreezeCamera.depth = this.uiCamera.depth + 3f;
			this.timelapseFreezeCamera.gameObject.AddComponent<FillRenderTargetEffect>();
			this.timelapseFreezeCamera.enabled = false;
			Camera camera = CameraController.CloneCamera(this.overlayCamera, "timelapseCamera");
			Timelapser timelapser = camera.gameObject.AddComponent<Timelapser>();
			camera.transparencySortMode = TransparencySortMode.Orthographic;
			camera.depth = this.baseCamera.depth + 2f;
			Game.Instance.timelapser = timelapser;
		}
		if (GameScreenManager.Instance != null)
		{
			for (int i = 0; i < this.uiCameraTargets.Count; i++)
			{
				GameScreenManager.Instance.SetCamera(this.uiCameraTargets[i], this.uiCamera);
			}
			this.infoText = GameScreenManager.Instance.screenshotModeCanvas.GetComponentInChildren<LocText>();
		}
		if (!KPlayerPrefs.HasKey("CameraSpeed"))
		{
			CameraController.SetDefaultCameraSpeed();
		}
		this.SetSpeedFromPrefs(null);
		Game.Instance.Subscribe(75424175, new Action<object>(this.SetSpeedFromPrefs));
		this.VisibleArea.Update();
	}

	// Token: 0x06001E23 RID: 7715 RVA: 0x000A7212 File Offset: 0x000A5412
	private void SetSpeedFromPrefs(object data = null)
	{
		this.keyPanningSpeed = Mathf.Clamp(0.1f, KPlayerPrefs.GetFloat("CameraSpeed"), 2f);
	}

	// Token: 0x06001E24 RID: 7716 RVA: 0x000A7234 File Offset: 0x000A5434
	public int GetCursorCell()
	{
		Vector3 rhs = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
		Vector3 vector = Vector3.Max(ClusterManager.Instance.activeWorld.minimumBounds, rhs);
		vector = Vector3.Min(ClusterManager.Instance.activeWorld.maximumBounds, vector);
		return Grid.PosToCell(vector);
	}

	// Token: 0x06001E25 RID: 7717 RVA: 0x000A728D File Offset: 0x000A548D
	public static Camera CloneCamera(Camera camera, string name)
	{
		Camera camera2 = new GameObject
		{
			name = name
		}.AddComponent<Camera>();
		camera2.CopyFrom(camera);
		return camera2;
	}

	// Token: 0x06001E26 RID: 7718 RVA: 0x000A72A8 File Offset: 0x000A54A8
	private Camera CopyCamera(Camera camera, string name)
	{
		Camera camera2 = CameraController.CloneCamera(camera, name);
		this.cameras.Add(camera2);
		return camera2;
	}

	// Token: 0x06001E27 RID: 7719 RVA: 0x000A72CA File Offset: 0x000A54CA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Restore();
	}

	// Token: 0x06001E28 RID: 7720 RVA: 0x000A72D8 File Offset: 0x000A54D8
	public static void SetDefaultCameraSpeed()
	{
		KPlayerPrefs.SetFloat("CameraSpeed", 1f);
	}

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x06001E29 RID: 7721 RVA: 0x000A72E9 File Offset: 0x000A54E9
	// (set) Token: 0x06001E2A RID: 7722 RVA: 0x000A72F1 File Offset: 0x000A54F1
	public Coroutine activeFadeRoutine { get; private set; }

	// Token: 0x06001E2B RID: 7723 RVA: 0x000A72FA File Offset: 0x000A54FA
	public void FadeOut(float targetPercentage = 1f, float speed = 1f, System.Action callback = null)
	{
		if (this.activeFadeRoutine != null)
		{
			base.StopCoroutine(this.activeFadeRoutine);
		}
		this.activeFadeRoutine = base.StartCoroutine(this.FadeWithBlack(true, 0f, targetPercentage, speed, null));
	}

	// Token: 0x06001E2C RID: 7724 RVA: 0x000A732B File Offset: 0x000A552B
	public void FadeIn(float targetPercentage = 0f, float speed = 1f, System.Action callback = null)
	{
		if (this.activeFadeRoutine != null)
		{
			base.StopCoroutine(this.activeFadeRoutine);
		}
		this.activeFadeRoutine = base.StartCoroutine(this.FadeWithBlack(true, 1f, targetPercentage, speed, callback));
	}

	// Token: 0x06001E2D RID: 7725 RVA: 0x000A735C File Offset: 0x000A555C
	public void ActiveWorldStarWipe(int id, System.Action callback = null)
	{
		this.ActiveWorldStarWipe(id, false, default(Vector3), 10f, callback);
	}

	// Token: 0x06001E2E RID: 7726 RVA: 0x000A7380 File Offset: 0x000A5580
	public void ActiveWorldStarWipe(int id, Vector3 position, float forceOrthgraphicSize = 10f, System.Action callback = null)
	{
		this.ActiveWorldStarWipe(id, true, position, forceOrthgraphicSize, callback);
	}

	// Token: 0x06001E2F RID: 7727 RVA: 0x000A7390 File Offset: 0x000A5590
	private void ActiveWorldStarWipe(int id, bool useForcePosition, Vector3 forcePosition, float forceOrthgraphicSize, System.Action callback)
	{
		if (this.activeFadeRoutine != null)
		{
			base.StopCoroutine(this.activeFadeRoutine);
		}
		if (ClusterManager.Instance.activeWorldId != id)
		{
			if (DetailsScreen.Instance != null)
			{
				DetailsScreen.Instance.DeselectAndClose();
			}
			this.activeFadeRoutine = base.StartCoroutine(this.SwapToWorldFade(id, useForcePosition, forcePosition, forceOrthgraphicSize, callback));
			return;
		}
		ManagementMenu.Instance.CloseAll();
		if (useForcePosition)
		{
			CameraController.Instance.SetTargetPos(forcePosition, 8f, true);
			if (callback != null)
			{
				callback();
			}
		}
	}

	// Token: 0x06001E30 RID: 7728 RVA: 0x000A7418 File Offset: 0x000A5618
	private IEnumerator SwapToWorldFade(int worldId, bool useForcePosition, Vector3 forcePosition, float forceOrthgraphicSize, System.Action newWorldCallback)
	{
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().ActiveBaseChangeSnapshot);
		ClusterManager.Instance.UpdateWorldReverbSnapshot(worldId);
		yield return base.StartCoroutine(this.FadeWithBlack(false, 0f, 1f, 3f, null));
		ClusterManager.Instance.SetActiveWorld(worldId);
		if (useForcePosition)
		{
			CameraController.Instance.SetTargetPos(forcePosition, forceOrthgraphicSize, false);
			CameraController.Instance.SetPosition(forcePosition);
		}
		if (newWorldCallback != null)
		{
			newWorldCallback();
		}
		ManagementMenu.Instance.CloseAll();
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().ActiveBaseChangeSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		yield return base.StartCoroutine(this.FadeWithBlack(false, 1f, 0f, 3f, null));
		yield break;
	}

	// Token: 0x06001E31 RID: 7729 RVA: 0x000A744C File Offset: 0x000A564C
	public void SetWorldInteractive(bool state)
	{
		GameScreenManager.Instance.fadePlaneFront.raycastTarget = !state;
	}

	// Token: 0x06001E32 RID: 7730 RVA: 0x000A7461 File Offset: 0x000A5661
	private IEnumerator FadeWithBlack(bool fadeUI, float startBlackPercent, float targetBlackPercent, float speed = 1f, System.Action callback = null)
	{
		Image fadePlane = fadeUI ? GameScreenManager.Instance.fadePlaneFront : GameScreenManager.Instance.fadePlaneBack;
		float percent = 0f;
		while (percent < 1f)
		{
			percent += Time.unscaledDeltaTime * speed;
			float a = MathUtil.ReRange(percent, 0f, 1f, startBlackPercent, targetBlackPercent);
			fadePlane.color = new Color(0f, 0f, 0f, a);
			yield return SequenceUtil.WaitForNextFrame;
		}
		fadePlane.color = new Color(0f, 0f, 0f, targetBlackPercent);
		if (callback != null)
		{
			callback();
		}
		this.activeFadeRoutine = null;
		yield return SequenceUtil.WaitForNextFrame;
		yield break;
	}

	// Token: 0x06001E33 RID: 7731 RVA: 0x000A7495 File Offset: 0x000A5695
	public void EnableFreeCamera(bool enable)
	{
		this.FreeCameraEnabled = enable;
		this.SetInfoText("Screenshot Mode (ESC to exit)");
	}

	// Token: 0x06001E34 RID: 7732 RVA: 0x000A74AC File Offset: 0x000A56AC
	private static bool WithinInputField()
	{
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current == null)
		{
			return false;
		}
		bool result = false;
		if (current.currentSelectedGameObject != null && (current.currentSelectedGameObject.GetComponent<KInputTextField>() != null || current.currentSelectedGameObject.GetComponent<InputField>() != null))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x06001E35 RID: 7733 RVA: 0x000A7504 File Offset: 0x000A5704
	public static bool IsMouseOverGameWindow
	{
		get
		{
			return 0f <= Input.mousePosition.x && 0f <= Input.mousePosition.y && (float)Screen.width >= Input.mousePosition.x && (float)Screen.height >= Input.mousePosition.y;
		}
	}

	// Token: 0x06001E36 RID: 7734 RVA: 0x000A755C File Offset: 0x000A575C
	private void SetInfoText(string text)
	{
		this.infoText.text = text;
		Color color = this.infoText.color;
		color.a = 0.5f;
		this.infoText.color = color;
	}

	// Token: 0x06001E37 RID: 7735 RVA: 0x000A759C File Offset: 0x000A579C
	public void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (this.DisableUserCameraControl)
		{
			return;
		}
		if (CameraController.WithinInputField())
		{
			return;
		}
		if (SaveGame.Instance != null && SaveGame.Instance.GetComponent<UserNavigation>().Handle(e))
		{
			return;
		}
		if (!this.ChangeWorldInput(e))
		{
			if (e.TryConsume(global::Action.TogglePause))
			{
				SpeedControlScreen.Instance.TogglePause(false);
			}
			else if (e.TryConsume(global::Action.ZoomIn) && CameraController.IsMouseOverGameWindow)
			{
				float a = this.targetOrthographicSize * (1f / this.zoomFactor);
				this.targetOrthographicSize = Mathf.Max(a, this.minOrthographicSize);
				this.overrideZoomSpeed = 0f;
				this.isTargetPosSet = false;
			}
			else if (e.TryConsume(global::Action.ZoomOut) && CameraController.IsMouseOverGameWindow)
			{
				float a2 = this.targetOrthographicSize * this.zoomFactor;
				this.targetOrthographicSize = Mathf.Min(a2, this.FreeCameraEnabled ? TuningData<CameraController.Tuning>.Get().maxOrthographicSizeDebug : this.maxOrthographicSize);
				this.overrideZoomSpeed = 0f;
				this.isTargetPosSet = false;
			}
			else if (e.TryConsume(global::Action.MouseMiddle) || e.IsAction(global::Action.MouseRight))
			{
				this.panning = true;
				this.overrideZoomSpeed = 0f;
				this.isTargetPosSet = false;
			}
			else if (this.FreeCameraEnabled && e.TryConsume(global::Action.CinemaCamEnable))
			{
				this.cinemaCamEnabled = !this.cinemaCamEnabled;
				DebugUtil.LogArgs(new object[]
				{
					"Cinema Cam Enabled ",
					this.cinemaCamEnabled
				});
				this.SetInfoText(this.cinemaCamEnabled ? "Cinema Cam Enabled" : "Cinema Cam Disabled");
			}
			else if (this.FreeCameraEnabled && this.cinemaCamEnabled)
			{
				if (e.TryConsume(global::Action.CinemaToggleLock))
				{
					this.cinemaToggleLock = !this.cinemaToggleLock;
					DebugUtil.LogArgs(new object[]
					{
						"Cinema Toggle Lock ",
						this.cinemaToggleLock
					});
					this.SetInfoText(this.cinemaToggleLock ? "Cinema Input Lock ON" : "Cinema Input Lock OFF");
				}
				else if (e.TryConsume(global::Action.CinemaToggleEasing))
				{
					this.cinemaToggleEasing = !this.cinemaToggleEasing;
					DebugUtil.LogArgs(new object[]
					{
						"Cinema Toggle Easing ",
						this.cinemaToggleEasing
					});
					this.SetInfoText(this.cinemaToggleEasing ? "Cinema Easing ON" : "Cinema Easing OFF");
				}
				else if (e.TryConsume(global::Action.CinemaUnpauseOnMove))
				{
					this.cinemaUnpauseNextMove = !this.cinemaUnpauseNextMove;
					DebugUtil.LogArgs(new object[]
					{
						"Cinema Unpause Next Move ",
						this.cinemaUnpauseNextMove
					});
					this.SetInfoText(this.cinemaUnpauseNextMove ? "Cinema Unpause Next Move ON" : "Cinema Unpause Next Move OFF");
				}
				else if (e.TryConsume(global::Action.CinemaPanLeft))
				{
					this.cinemaPanLeft = (!this.cinemaToggleLock || !this.cinemaPanLeft);
					this.cinemaPanRight = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaPanRight))
				{
					this.cinemaPanRight = (!this.cinemaToggleLock || !this.cinemaPanRight);
					this.cinemaPanLeft = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaPanUp))
				{
					this.cinemaPanUp = (!this.cinemaToggleLock || !this.cinemaPanUp);
					this.cinemaPanDown = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaPanDown))
				{
					this.cinemaPanDown = (!this.cinemaToggleLock || !this.cinemaPanDown);
					this.cinemaPanUp = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaZoomIn))
				{
					this.cinemaZoomIn = (!this.cinemaToggleLock || !this.cinemaZoomIn);
					this.cinemaZoomOut = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaZoomOut))
				{
					this.cinemaZoomOut = (!this.cinemaToggleLock || !this.cinemaZoomOut);
					this.cinemaZoomIn = false;
					this.CheckMoveUnpause();
				}
				else if (e.TryConsume(global::Action.CinemaZoomSpeedPlus))
				{
					this.cinemaZoomSpeed++;
					DebugUtil.LogArgs(new object[]
					{
						"Cinema Zoom Speed ",
						this.cinemaZoomSpeed
					});
					this.SetInfoText("Cinema Zoom Speed: " + this.cinemaZoomSpeed.ToString());
				}
				else if (e.TryConsume(global::Action.CinemaZoomSpeedMinus))
				{
					this.cinemaZoomSpeed--;
					DebugUtil.LogArgs(new object[]
					{
						"Cinema Zoom Speed ",
						this.cinemaZoomSpeed
					});
					this.SetInfoText("Cinema Zoom Speed: " + this.cinemaZoomSpeed.ToString());
				}
			}
			else if (e.TryConsume(global::Action.PanLeft))
			{
				this.panLeft = true;
			}
			else if (e.TryConsume(global::Action.PanRight))
			{
				this.panRight = true;
			}
			else if (e.TryConsume(global::Action.PanUp))
			{
				this.panUp = true;
			}
			else if (e.TryConsume(global::Action.PanDown))
			{
				this.panDown = true;
			}
		}
		if (!e.Consumed && OverlayMenu.Instance != null)
		{
			OverlayMenu.Instance.OnKeyDown(e);
		}
	}

	// Token: 0x06001E38 RID: 7736 RVA: 0x000A7AF4 File Offset: 0x000A5CF4
	public bool ChangeWorldInput(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return true;
		}
		int num = -1;
		if (e.TryConsume(global::Action.SwitchActiveWorld1))
		{
			num = 0;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld2))
		{
			num = 1;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld3))
		{
			num = 2;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld4))
		{
			num = 3;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld5))
		{
			num = 4;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld6))
		{
			num = 5;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld7))
		{
			num = 6;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld8))
		{
			num = 7;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld9))
		{
			num = 8;
		}
		else if (e.TryConsume(global::Action.SwitchActiveWorld10))
		{
			num = 9;
		}
		if (num != -1)
		{
			List<int> discoveredAsteroidIDsSorted = ClusterManager.Instance.GetDiscoveredAsteroidIDsSorted();
			if (num < discoveredAsteroidIDsSorted.Count && num >= 0)
			{
				num = discoveredAsteroidIDsSorted[num];
				WorldContainer world = ClusterManager.Instance.GetWorld(num);
				if (world != null && world.IsDiscovered && ClusterManager.Instance.activeWorldId != world.id)
				{
					ManagementMenu.Instance.CloseClusterMap();
					this.ActiveWorldStarWipe(world.id, null);
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06001E39 RID: 7737 RVA: 0x000A7C2C File Offset: 0x000A5E2C
	public void OnKeyUp(KButtonEvent e)
	{
		if (this.DisableUserCameraControl)
		{
			return;
		}
		if (CameraController.WithinInputField())
		{
			return;
		}
		if (e.TryConsume(global::Action.MouseMiddle) || e.IsAction(global::Action.MouseRight))
		{
			this.panning = false;
			return;
		}
		if (this.FreeCameraEnabled && this.cinemaCamEnabled)
		{
			if (e.TryConsume(global::Action.CinemaPanLeft))
			{
				this.cinemaPanLeft = (this.cinemaToggleLock && this.cinemaPanLeft);
				return;
			}
			if (e.TryConsume(global::Action.CinemaPanRight))
			{
				this.cinemaPanRight = (this.cinemaToggleLock && this.cinemaPanRight);
				return;
			}
			if (e.TryConsume(global::Action.CinemaPanUp))
			{
				this.cinemaPanUp = (this.cinemaToggleLock && this.cinemaPanUp);
				return;
			}
			if (e.TryConsume(global::Action.CinemaPanDown))
			{
				this.cinemaPanDown = (this.cinemaToggleLock && this.cinemaPanDown);
				return;
			}
			if (e.TryConsume(global::Action.CinemaZoomIn))
			{
				this.cinemaZoomIn = (this.cinemaToggleLock && this.cinemaZoomIn);
				return;
			}
			if (e.TryConsume(global::Action.CinemaZoomOut))
			{
				this.cinemaZoomOut = (this.cinemaToggleLock && this.cinemaZoomOut);
				return;
			}
		}
		else
		{
			if (e.TryConsume(global::Action.CameraHome))
			{
				this.CameraGoHome(2f);
				return;
			}
			if (e.TryConsume(global::Action.PanLeft))
			{
				this.panLeft = false;
				return;
			}
			if (e.TryConsume(global::Action.PanRight))
			{
				this.panRight = false;
				return;
			}
			if (e.TryConsume(global::Action.PanUp))
			{
				this.panUp = false;
				return;
			}
			if (e.TryConsume(global::Action.PanDown))
			{
				this.panDown = false;
			}
		}
	}

	// Token: 0x06001E3A RID: 7738 RVA: 0x000A7DC7 File Offset: 0x000A5FC7
	public void ForcePanningState(bool state)
	{
		this.panning = false;
	}

	// Token: 0x06001E3B RID: 7739 RVA: 0x000A7DD0 File Offset: 0x000A5FD0
	public void CameraGoHome(float speed = 2f)
	{
		GameObject activeTelepad = GameUtil.GetActiveTelepad();
		if (activeTelepad != null && ClusterUtil.ActiveWorldHasPrinter())
		{
			Vector3 pos = new Vector3(activeTelepad.transform.GetPosition().x, activeTelepad.transform.GetPosition().y + 1f, base.transform.GetPosition().z);
			this.SetTargetPos(pos, 10f, true);
			this.SetOverrideZoomSpeed(speed);
		}
	}

	// Token: 0x06001E3C RID: 7740 RVA: 0x000A7E44 File Offset: 0x000A6044
	public void CameraGoTo(Vector3 pos, float speed = 2f, bool playSound = true)
	{
		pos.z = base.transform.GetPosition().z;
		this.SetTargetPos(pos, 10f, playSound);
		this.SetOverrideZoomSpeed(speed);
	}

	// Token: 0x06001E3D RID: 7741 RVA: 0x000A7E74 File Offset: 0x000A6074
	public void SnapTo(Vector3 pos)
	{
		this.ClearFollowTarget();
		pos.z = -100f;
		this.targetPos = Vector3.zero;
		this.isTargetPosSet = false;
		base.transform.SetPosition(pos);
		this.keyPanDelta = Vector3.zero;
		this.OrthographicSize = this.targetOrthographicSize;
	}

	// Token: 0x06001E3E RID: 7742 RVA: 0x000A7EC9 File Offset: 0x000A60C9
	public void SnapTo(Vector3 pos, float orthographicSize)
	{
		this.targetOrthographicSize = orthographicSize;
		this.SnapTo(pos);
	}

	// Token: 0x06001E3F RID: 7743 RVA: 0x000A7ED9 File Offset: 0x000A60D9
	public void SetOverrideZoomSpeed(float tempZoomSpeed)
	{
		this.overrideZoomSpeed = tempZoomSpeed;
	}

	// Token: 0x06001E40 RID: 7744 RVA: 0x000A7EE4 File Offset: 0x000A60E4
	public void SetTargetPos(Vector3 pos, float orthographic_size, bool playSound)
	{
		int num = Grid.PosToCell(pos);
		if (!Grid.IsValidCell(num) || Grid.WorldIdx[num] == 255 || ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]) == null)
		{
			return;
		}
		this.ClearFollowTarget();
		if (playSound && !this.isTargetPosSet)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Click_Notification", false));
		}
		pos.z = -100f;
		if ((int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
		{
			this.targetOrthographicSize = 20f;
			this.ActiveWorldStarWipe((int)Grid.WorldIdx[num], pos, 10f, delegate()
			{
				this.targetPos = pos;
				this.isTargetPosSet = true;
				this.OrthographicSize = orthographic_size + 5f;
				this.targetOrthographicSize = orthographic_size;
			});
		}
		else
		{
			this.targetPos = pos;
			this.isTargetPosSet = true;
			this.targetOrthographicSize = orthographic_size;
		}
		PlayerController.Instance.CancelDragging();
		this.CheckMoveUnpause();
	}

	// Token: 0x06001E41 RID: 7745 RVA: 0x000A7FEC File Offset: 0x000A61EC
	public void SetTargetPosForWorldChange(Vector3 pos, float orthographic_size, bool playSound)
	{
		int num = Grid.PosToCell(pos);
		if (!Grid.IsValidCell(num) || Grid.WorldIdx[num] == 255 || ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]) == null)
		{
			return;
		}
		this.ClearFollowTarget();
		if (playSound && !this.isTargetPosSet)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Click_Notification", false));
		}
		pos.z = -100f;
		this.targetPos = pos;
		this.isTargetPosSet = true;
		this.targetOrthographicSize = orthographic_size;
		PlayerController.Instance.CancelDragging();
		this.CheckMoveUnpause();
		this.SetPosition(pos);
		this.OrthographicSize = orthographic_size;
	}

	// Token: 0x06001E42 RID: 7746 RVA: 0x000A8090 File Offset: 0x000A6290
	public void SetMaxOrthographicSize(float size)
	{
		this.maxOrthographicSize = size;
	}

	// Token: 0x06001E43 RID: 7747 RVA: 0x000A8099 File Offset: 0x000A6299
	public void SetPosition(Vector3 pos)
	{
		base.transform.SetPosition(pos);
	}

	// Token: 0x06001E44 RID: 7748 RVA: 0x000A80A8 File Offset: 0x000A62A8
	public IEnumerator DoCinematicZoom(float targetOrthographicSize)
	{
		this.cinemaCamEnabled = true;
		this.FreeCameraEnabled = true;
		this.targetOrthographicSize = targetOrthographicSize;
		while (targetOrthographicSize - this.OrthographicSize >= 0.001f)
		{
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.OrthographicSize = targetOrthographicSize;
		this.FreeCameraEnabled = false;
		this.cinemaCamEnabled = false;
		yield break;
	}

	// Token: 0x06001E45 RID: 7749 RVA: 0x000A80C0 File Offset: 0x000A62C0
	private Vector3 PointUnderCursor(Vector3 mousePos, Camera cam)
	{
		Ray ray = cam.ScreenPointToRay(mousePos);
		Vector3 direction = ray.direction;
		Vector3 b = direction * Mathf.Abs(cam.transform.GetPosition().z / direction.z);
		return ray.origin + b;
	}

	// Token: 0x06001E46 RID: 7750 RVA: 0x000A8110 File Offset: 0x000A6310
	private void CinemaCamUpdate()
	{
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		Camera main = Camera.main;
		Vector3 localPosition = base.transform.GetLocalPosition();
		float num = Mathf.Pow((float)this.cinemaZoomSpeed, 3f);
		if (this.cinemaZoomIn)
		{
			this.overrideZoomSpeed = -num / TuningData<CameraController.Tuning>.Get().cinemaZoomFactor;
			this.isTargetPosSet = false;
		}
		else if (this.cinemaZoomOut)
		{
			this.overrideZoomSpeed = num / TuningData<CameraController.Tuning>.Get().cinemaZoomFactor;
			this.isTargetPosSet = false;
		}
		else
		{
			this.overrideZoomSpeed = 0f;
		}
		if (this.cinemaToggleEasing)
		{
			this.cinemaZoomVelocity += (this.overrideZoomSpeed - this.cinemaZoomVelocity) * this.cinemaEasing;
		}
		else
		{
			this.cinemaZoomVelocity = this.overrideZoomSpeed;
		}
		if (this.cinemaZoomVelocity != 0f)
		{
			this.OrthographicSize = main.orthographicSize + this.cinemaZoomVelocity * unscaledDeltaTime * (main.orthographicSize / 20f);
			this.targetOrthographicSize = main.orthographicSize;
		}
		float num2 = num / TuningData<CameraController.Tuning>.Get().cinemaZoomToFactor;
		float num3 = this.keyPanningSpeed / 20f * main.orthographicSize;
		float num4 = num3 * (num / TuningData<CameraController.Tuning>.Get().cinemaPanToFactor);
		if (!this.isTargetPosSet && this.targetOrthographicSize != main.orthographicSize)
		{
			float t = Mathf.Min(num2 * unscaledDeltaTime, 0.1f);
			this.OrthographicSize = Mathf.Lerp(main.orthographicSize, this.targetOrthographicSize, t);
		}
		Vector3 b = Vector3.zero;
		if (this.isTargetPosSet)
		{
			float num5 = this.cinemaEasing * TuningData<CameraController.Tuning>.Get().targetZoomEasingFactor;
			float num6 = this.cinemaEasing * TuningData<CameraController.Tuning>.Get().targetPanEasingFactor;
			float num7 = this.targetOrthographicSize - main.orthographicSize;
			Vector3 vector = this.targetPos - localPosition;
			float num8;
			float num9;
			if (!this.cinemaToggleEasing)
			{
				num8 = num2 * unscaledDeltaTime;
				num9 = num4 * unscaledDeltaTime;
			}
			else
			{
				DebugUtil.LogArgs(new object[]
				{
					"Min zoom of:",
					num2 * unscaledDeltaTime,
					Mathf.Abs(num7) * num5 * unscaledDeltaTime
				});
				num8 = Mathf.Min(num2 * unscaledDeltaTime, Mathf.Abs(num7) * num5 * unscaledDeltaTime);
				DebugUtil.LogArgs(new object[]
				{
					"Min pan of:",
					num4 * unscaledDeltaTime,
					vector.magnitude * num6 * unscaledDeltaTime
				});
				num9 = Mathf.Min(num4 * unscaledDeltaTime, vector.magnitude * num6 * unscaledDeltaTime);
			}
			float num10;
			if (Mathf.Abs(num7) < num8)
			{
				num10 = num7;
			}
			else
			{
				num10 = Mathf.Sign(num7) * num8;
			}
			if (vector.magnitude < num9)
			{
				b = vector;
			}
			else
			{
				b = vector.normalized * num9;
			}
			if (Mathf.Abs(num10) < 0.001f && b.magnitude < 0.001f)
			{
				this.isTargetPosSet = false;
				num10 = num7;
				b = vector;
			}
			this.OrthographicSize = main.orthographicSize + num10 * (main.orthographicSize / 20f);
		}
		if (!PlayerController.Instance.CanDrag())
		{
			this.panning = false;
		}
		Vector3 b2 = Vector3.zero;
		if (this.panning)
		{
			b2 = -PlayerController.Instance.GetWorldDragDelta();
			this.isTargetPosSet = false;
			if (b2.magnitude > 0f)
			{
				this.ClearFollowTarget();
			}
			this.keyPanDelta = Vector3.zero;
		}
		else
		{
			float num11 = num / TuningData<CameraController.Tuning>.Get().cinemaPanFactor;
			Vector3 zero = Vector3.zero;
			if (this.cinemaPanLeft)
			{
				this.ClearFollowTarget();
				zero.x = -num3 * num11;
				this.isTargetPosSet = false;
			}
			if (this.cinemaPanRight)
			{
				this.ClearFollowTarget();
				zero.x = num3 * num11;
				this.isTargetPosSet = false;
			}
			if (this.cinemaPanUp)
			{
				this.ClearFollowTarget();
				zero.y = num3 * num11;
				this.isTargetPosSet = false;
			}
			if (this.cinemaPanDown)
			{
				this.ClearFollowTarget();
				zero.y = -num3 * num11;
				this.isTargetPosSet = false;
			}
			if (this.cinemaToggleEasing)
			{
				this.keyPanDelta += (zero - this.keyPanDelta) * this.cinemaEasing;
			}
			else
			{
				this.keyPanDelta = zero;
			}
		}
		Vector3 vector2 = localPosition + b + b2 + this.keyPanDelta * unscaledDeltaTime;
		if (this.followTarget != null)
		{
			vector2.x = this.followTargetPos.x;
			vector2.y = this.followTargetPos.y;
		}
		vector2.z = -100f;
		if ((double)(vector2 - base.transform.GetLocalPosition()).magnitude > 0.001)
		{
			base.transform.SetLocalPosition(vector2);
		}
	}

	// Token: 0x06001E47 RID: 7751 RVA: 0x000A85D8 File Offset: 0x000A67D8
	private void NormalCamUpdate()
	{
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		Camera main = Camera.main;
		this.smoothDt = this.smoothDt * 2f / 3f + unscaledDeltaTime / 3f;
		float num = (this.overrideZoomSpeed != 0f) ? this.overrideZoomSpeed : this.zoomSpeed;
		Vector3 localPosition = base.transform.GetLocalPosition();
		Vector3 vector = (this.overrideZoomSpeed != 0f) ? new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f, 0f) : KInputManager.GetMousePos();
		Vector3 position = this.PointUnderCursor(vector, main);
		Vector3 position2 = main.ScreenToViewportPoint(vector);
		float num2 = this.keyPanningSpeed / 20f * main.orthographicSize;
		num2 *= Mathf.Min(unscaledDeltaTime / 0.016666666f, 10f);
		float t = num * Mathf.Min(this.smoothDt, 0.3f);
		this.OrthographicSize = Mathf.Lerp(main.orthographicSize, this.targetOrthographicSize, t);
		base.transform.SetLocalPosition(localPosition);
		Vector3 vector2 = main.WorldToViewportPoint(position);
		position2.z = vector2.z;
		Vector3 b = main.ViewportToWorldPoint(vector2) - main.ViewportToWorldPoint(position2);
		if (this.isTargetPosSet)
		{
			b = Vector3.Lerp(localPosition, this.targetPos, num * this.smoothDt) - localPosition;
			if (b.magnitude < 0.001f)
			{
				this.isTargetPosSet = false;
				b = this.targetPos - localPosition;
			}
		}
		if (!PlayerController.Instance.CanDrag())
		{
			this.panning = false;
		}
		Vector3 b2 = Vector3.zero;
		if (this.panning)
		{
			b2 = -PlayerController.Instance.GetWorldDragDelta();
			this.isTargetPosSet = false;
		}
		Vector3 vector3 = localPosition + b + b2;
		if (this.panning)
		{
			if (b2.magnitude > 0f)
			{
				this.ClearFollowTarget();
			}
			this.keyPanDelta = Vector3.zero;
		}
		else if (!this.DisableUserCameraControl)
		{
			if (this.panLeft)
			{
				this.ClearFollowTarget();
				this.keyPanDelta.x = this.keyPanDelta.x - num2;
				this.isTargetPosSet = false;
				this.overrideZoomSpeed = 0f;
			}
			if (this.panRight)
			{
				this.ClearFollowTarget();
				this.keyPanDelta.x = this.keyPanDelta.x + num2;
				this.isTargetPosSet = false;
				this.overrideZoomSpeed = 0f;
			}
			if (this.panUp)
			{
				this.ClearFollowTarget();
				this.keyPanDelta.y = this.keyPanDelta.y + num2;
				this.isTargetPosSet = false;
				this.overrideZoomSpeed = 0f;
			}
			if (this.panDown)
			{
				this.ClearFollowTarget();
				this.keyPanDelta.y = this.keyPanDelta.y - num2;
				this.isTargetPosSet = false;
				this.overrideZoomSpeed = 0f;
			}
			if (KInputManager.currentControllerIsGamepad)
			{
				Vector2 vector4 = num2 * KInputManager.steamInputInterpreter.GetSteamCameraMovement();
				if (Mathf.Abs(vector4.x) > Mathf.Epsilon || Mathf.Abs(vector4.y) > Mathf.Epsilon)
				{
					this.ClearFollowTarget();
					this.isTargetPosSet = false;
					this.overrideZoomSpeed = 0f;
				}
				this.keyPanDelta += new Vector3(vector4.x, vector4.y, 0f);
			}
			Vector3 vector5 = new Vector3(Mathf.Lerp(0f, this.keyPanDelta.x, this.smoothDt * this.keyPanningEasing), Mathf.Lerp(0f, this.keyPanDelta.y, this.smoothDt * this.keyPanningEasing), 0f);
			this.keyPanDelta -= vector5;
			vector3.x += vector5.x;
			vector3.y += vector5.y;
		}
		if (this.followTarget != null)
		{
			vector3.x = this.followTargetPos.x;
			vector3.y = this.followTargetPos.y;
		}
		vector3.z = -100f;
		if ((double)(vector3 - base.transform.GetLocalPosition()).magnitude > 0.001)
		{
			base.transform.SetLocalPosition(vector3);
		}
	}

	// Token: 0x06001E48 RID: 7752 RVA: 0x000A8A24 File Offset: 0x000A6C24
	private void Update()
	{
		if (Game.Instance == null || !Game.Instance.timelapser.CapturingTimelapseScreenshot)
		{
			if (this.FreeCameraEnabled && this.cinemaCamEnabled)
			{
				this.CinemaCamUpdate();
			}
			else
			{
				this.NormalCamUpdate();
			}
		}
		if (this.infoText != null && this.infoText.color.a > 0f)
		{
			Color color = this.infoText.color;
			color.a = Mathf.Max(0f, this.infoText.color.a - Time.unscaledDeltaTime * 0.5f);
			this.infoText.color = color;
		}
		this.ConstrainToWorld();
		Vector3 vector = this.PointUnderCursor(KInputManager.GetMousePos(), Camera.main);
		Shader.SetGlobalVector("_WorldCameraPos", new Vector4(base.transform.GetPosition().x, base.transform.GetPosition().y, base.transform.GetPosition().z, Camera.main.orthographicSize));
		Shader.SetGlobalVector("_WorldCursorPos", new Vector4(vector.x, vector.y, 0f, 0f));
		this.VisibleArea.Update();
		this.soundCuller = SoundCuller.CreateCuller();
	}

	// Token: 0x06001E49 RID: 7753 RVA: 0x000A8B74 File Offset: 0x000A6D74
	private Vector3 GetFollowPos()
	{
		if (this.followTarget != null)
		{
			Vector3 result = this.followTarget.transform.GetPosition();
			KAnimControllerBase component = this.followTarget.GetComponent<KAnimControllerBase>();
			if (component != null)
			{
				result = component.GetWorldPivot();
			}
			return result;
		}
		return Vector3.zero;
	}

	// Token: 0x06001E4A RID: 7754 RVA: 0x000A8BC4 File Offset: 0x000A6DC4
	public static float GetHighestVisibleCell_Height(byte worldID = 255)
	{
		Vector2 zero = Vector2.zero;
		Vector2 vector = new Vector2(Grid.WidthInMeters, Grid.HeightInMeters);
		Camera main = Camera.main;
		float orthographicSize = main.orthographicSize;
		main.orthographicSize = 20f;
		Ray ray = main.ViewportPointToRay(Vector3.one - Vector3.one * 0.33f);
		Vector3 vector2 = CameraController.Instance.transform.GetPosition() - ray.origin;
		main.orthographicSize = orthographicSize;
		if (ClusterManager.Instance != null)
		{
			WorldContainer worldContainer = (worldID == byte.MaxValue) ? ClusterManager.Instance.activeWorld : ClusterManager.Instance.GetWorld((int)worldID);
			worldContainer.minimumBounds * Grid.CellSizeInMeters;
			vector = worldContainer.maximumBounds * Grid.CellSizeInMeters;
			new Vector2((float)worldContainer.Width, (float)worldContainer.Height) * Grid.CellSizeInMeters;
		}
		return vector.y * 1.1f + 20f + vector2.y;
	}

	// Token: 0x06001E4B RID: 7755 RVA: 0x000A8CCC File Offset: 0x000A6ECC
	private void ConstrainToWorld()
	{
		if (Game.Instance != null && Game.Instance.IsLoading())
		{
			return;
		}
		if (this.FreeCameraEnabled)
		{
			return;
		}
		Camera main = Camera.main;
		Ray ray = main.ViewportPointToRay(Vector3.zero + Vector3.one * 0.33f);
		Ray ray2 = main.ViewportPointToRay(Vector3.one - Vector3.one * 0.33f);
		float distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		float distance2 = Mathf.Abs(ray2.origin.z / ray2.direction.z);
		Vector3 point = ray.GetPoint(distance);
		Vector3 point2 = ray2.GetPoint(distance2);
		Vector2 vector = Vector2.zero;
		Vector2 vector2 = new Vector2(Grid.WidthInMeters, Grid.HeightInMeters);
		Vector2 vector3 = vector2;
		if (ClusterManager.Instance != null)
		{
			WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
			vector = activeWorld.minimumBounds * Grid.CellSizeInMeters;
			vector2 = activeWorld.maximumBounds * Grid.CellSizeInMeters;
			vector3 = new Vector2((float)activeWorld.Width, (float)activeWorld.Height) * Grid.CellSizeInMeters;
		}
		if (point2.x - point.x > vector3.x || point2.y - point.y > vector3.y)
		{
			return;
		}
		Vector3 b = base.transform.GetPosition() - ray.origin;
		Vector3 vector4 = point;
		vector4.x = Mathf.Max(vector.x, vector4.x);
		vector4.y = Mathf.Max(vector.y * Grid.CellSizeInMeters, vector4.y);
		ray.origin = vector4;
		ray.direction = -ray.direction;
		vector4 = ray.GetPoint(distance);
		base.transform.SetPosition(vector4 + b);
		b = base.transform.GetPosition() - ray2.origin;
		vector4 = point2;
		vector4.x = Mathf.Min(vector2.x, vector4.x);
		vector4.y = Mathf.Min(vector2.y * 1.1f, vector4.y);
		ray2.origin = vector4;
		ray2.direction = -ray2.direction;
		vector4 = ray2.GetPoint(distance2);
		Vector3 position = vector4 + b;
		position.z = -100f;
		base.transform.SetPosition(position);
	}

	// Token: 0x06001E4C RID: 7756 RVA: 0x000A8F6C File Offset: 0x000A716C
	public void Save(BinaryWriter writer)
	{
		writer.Write(base.transform.GetPosition());
		writer.Write(base.transform.localScale);
		writer.Write(base.transform.rotation);
		writer.Write(this.targetOrthographicSize);
		CameraSaveData.position = base.transform.GetPosition();
		CameraSaveData.localScale = base.transform.localScale;
		CameraSaveData.rotation = base.transform.rotation;
	}

	// Token: 0x06001E4D RID: 7757 RVA: 0x000A8FE8 File Offset: 0x000A71E8
	private void Restore()
	{
		if (CameraSaveData.valid)
		{
			int cell = Grid.PosToCell(CameraSaveData.position);
			if (Grid.IsValidCell(cell) && !Grid.IsVisible(cell))
			{
				global::Debug.LogWarning("Resetting Camera Position... camera was saved in an undiscovered area of the map.");
				this.CameraGoHome(2f);
				return;
			}
			base.transform.SetPosition(CameraSaveData.position);
			base.transform.localScale = CameraSaveData.localScale;
			base.transform.rotation = CameraSaveData.rotation;
			this.targetOrthographicSize = Mathf.Clamp(CameraSaveData.orthographicsSize, this.minOrthographicSize, this.FreeCameraEnabled ? TuningData<CameraController.Tuning>.Get().maxOrthographicSizeDebug : this.maxOrthographicSize);
			this.SnapTo(base.transform.GetPosition());
		}
	}

	// Token: 0x06001E4E RID: 7758 RVA: 0x000A90A2 File Offset: 0x000A72A2
	private void OnMRTSetupComplete(Camera cam)
	{
		this.cameras.Add(cam);
	}

	// Token: 0x06001E4F RID: 7759 RVA: 0x000A90B0 File Offset: 0x000A72B0
	public bool IsAudibleSound(Vector2 pos)
	{
		return this.soundCuller.IsAudible(pos);
	}

	// Token: 0x06001E50 RID: 7760 RVA: 0x000A90C0 File Offset: 0x000A72C0
	public bool IsAudibleSound(Vector3 pos, EventReference event_ref)
	{
		string eventReferencePath = KFMOD.GetEventReferencePath(event_ref);
		return this.soundCuller.IsAudible(pos, eventReferencePath);
	}

	// Token: 0x06001E51 RID: 7761 RVA: 0x000A90EB File Offset: 0x000A72EB
	public bool IsAudibleSound(Vector3 pos, HashedString sound_path)
	{
		return this.soundCuller.IsAudible(pos, sound_path);
	}

	// Token: 0x06001E52 RID: 7762 RVA: 0x000A90FF File Offset: 0x000A72FF
	public Vector3 GetVerticallyScaledPosition(Vector3 pos, bool objectIsSelectedAndVisible = false)
	{
		return this.soundCuller.GetVerticallyScaledPosition(pos, objectIsSelectedAndVisible);
	}

	// Token: 0x06001E53 RID: 7763 RVA: 0x000A9110 File Offset: 0x000A7310
	public bool IsVisiblePos(Vector3 pos)
	{
		return this.VisibleArea.CurrentArea.Contains(pos);
	}

	// Token: 0x06001E54 RID: 7764 RVA: 0x000A9134 File Offset: 0x000A7334
	public bool IsVisiblePosExtended(Vector3 pos)
	{
		return this.VisibleArea.CurrentAreaExtended.Contains(pos);
	}

	// Token: 0x06001E55 RID: 7765 RVA: 0x000A9155 File Offset: 0x000A7355
	protected override void OnCleanUp()
	{
		CameraController.Instance = null;
	}

	// Token: 0x06001E56 RID: 7766 RVA: 0x000A9160 File Offset: 0x000A7360
	public void SetFollowTarget(Transform follow_target)
	{
		this.ClearFollowTarget();
		if (follow_target == null)
		{
			return;
		}
		this.followTarget = follow_target;
		this.OrthographicSize = 6f;
		this.targetOrthographicSize = 6f;
		Vector3 followPos = this.GetFollowPos();
		this.followTargetPos = new Vector3(followPos.x, followPos.y, base.transform.GetPosition().z);
		base.transform.SetPosition(this.followTargetPos);
		this.followTarget.GetComponent<KMonoBehaviour>().Trigger(-1506069671, null);
	}

	// Token: 0x06001E57 RID: 7767 RVA: 0x000A91F0 File Offset: 0x000A73F0
	public void ClearFollowTarget()
	{
		if (this.followTarget == null)
		{
			return;
		}
		this.followTarget.GetComponent<KMonoBehaviour>().Trigger(-485480405, null);
		this.followTarget = null;
	}

	// Token: 0x06001E58 RID: 7768 RVA: 0x000A9220 File Offset: 0x000A7420
	public void UpdateFollowTarget()
	{
		if (this.followTarget != null)
		{
			Vector3 followPos = this.GetFollowPos();
			Vector2 a = new Vector2(base.transform.GetLocalPosition().x, base.transform.GetLocalPosition().y);
			byte b = Grid.WorldIdx[Grid.PosToCell(followPos)];
			if (ClusterManager.Instance.activeWorldId != (int)b)
			{
				Transform transform = this.followTarget;
				this.SetFollowTarget(null);
				ClusterManager.Instance.SetActiveWorld((int)b);
				this.SetFollowTarget(transform);
				return;
			}
			Vector2 vector = Vector2.Lerp(a, followPos, Time.unscaledDeltaTime * 25f);
			this.followTargetPos = new Vector3(vector.x, vector.y, base.transform.GetLocalPosition().z);
		}
	}

	// Token: 0x06001E59 RID: 7769 RVA: 0x000A92EC File Offset: 0x000A74EC
	public void RenderForTimelapser(ref RenderTexture tex)
	{
		this.RenderCameraForTimelapse(this.baseCamera, ref tex, this.timelapseCameraCullingMask, -1f);
		CameraClearFlags clearFlags = this.overlayCamera.clearFlags;
		this.overlayCamera.clearFlags = CameraClearFlags.Nothing;
		this.RenderCameraForTimelapse(this.overlayCamera, ref tex, this.timelapseOverlayCameraCullingMask, -1f);
		this.overlayCamera.clearFlags = clearFlags;
	}

	// Token: 0x06001E5A RID: 7770 RVA: 0x000A9350 File Offset: 0x000A7550
	private void RenderCameraForTimelapse(Camera cam, ref RenderTexture tex, LayerMask mask, float overrideAspect = -1f)
	{
		int cullingMask = cam.cullingMask;
		RenderTexture targetTexture = cam.targetTexture;
		cam.targetTexture = tex;
		cam.aspect = (float)tex.width / (float)tex.height;
		if (overrideAspect != -1f)
		{
			cam.aspect = overrideAspect;
		}
		if (mask != -1)
		{
			cam.cullingMask = mask;
		}
		cam.Render();
		cam.ResetAspect();
		cam.cullingMask = cullingMask;
		cam.targetTexture = targetTexture;
	}

	// Token: 0x06001E5B RID: 7771 RVA: 0x000A93CA File Offset: 0x000A75CA
	private void CheckMoveUnpause()
	{
		if (this.cinemaCamEnabled && this.cinemaUnpauseNextMove)
		{
			this.cinemaUnpauseNextMove = !this.cinemaUnpauseNextMove;
			if (SpeedControlScreen.Instance.IsPaused)
			{
				SpeedControlScreen.Instance.Unpause(false);
			}
		}
	}

	// Token: 0x040010E3 RID: 4323
	public const float DEFAULT_MAX_ORTHO_SIZE = 20f;

	// Token: 0x040010E4 RID: 4324
	public const float MAX_Y_SCALE = 1.1f;

	// Token: 0x040010E5 RID: 4325
	public LocText infoText;

	// Token: 0x040010E6 RID: 4326
	private const float FIXED_Z = -100f;

	// Token: 0x040010E8 RID: 4328
	public bool FreeCameraEnabled;

	// Token: 0x040010E9 RID: 4329
	public float zoomSpeed;

	// Token: 0x040010EA RID: 4330
	public float minOrthographicSize;

	// Token: 0x040010EB RID: 4331
	public float zoomFactor;

	// Token: 0x040010EC RID: 4332
	public float keyPanningSpeed;

	// Token: 0x040010ED RID: 4333
	public float keyPanningEasing;

	// Token: 0x040010EE RID: 4334
	public Texture2D dayColourCube;

	// Token: 0x040010EF RID: 4335
	public Texture2D nightColourCube;

	// Token: 0x040010F0 RID: 4336
	public Material LightBufferMaterial;

	// Token: 0x040010F1 RID: 4337
	public Material LightCircleOverlay;

	// Token: 0x040010F2 RID: 4338
	public Material LightConeOverlay;

	// Token: 0x040010F3 RID: 4339
	public Transform followTarget;

	// Token: 0x040010F4 RID: 4340
	public Vector3 followTargetPos;

	// Token: 0x040010F5 RID: 4341
	public GridVisibleArea VisibleArea = new GridVisibleArea(8);

	// Token: 0x040010F7 RID: 4343
	private float maxOrthographicSize = 20f;

	// Token: 0x040010F8 RID: 4344
	private float overrideZoomSpeed;

	// Token: 0x040010F9 RID: 4345
	private bool panning;

	// Token: 0x040010FA RID: 4346
	private const float MaxEdgePaddingPercent = 0.33f;

	// Token: 0x040010FB RID: 4347
	private Vector3 keyPanDelta;

	// Token: 0x040010FE RID: 4350
	[SerializeField]
	private LayerMask timelapseCameraCullingMask;

	// Token: 0x040010FF RID: 4351
	[SerializeField]
	private LayerMask timelapseOverlayCameraCullingMask;

	// Token: 0x04001101 RID: 4353
	private bool userCameraControlDisabled;

	// Token: 0x04001102 RID: 4354
	private bool panLeft;

	// Token: 0x04001103 RID: 4355
	private bool panRight;

	// Token: 0x04001104 RID: 4356
	private bool panUp;

	// Token: 0x04001105 RID: 4357
	private bool panDown;

	// Token: 0x04001107 RID: 4359
	[NonSerialized]
	public Camera baseCamera;

	// Token: 0x04001108 RID: 4360
	[NonSerialized]
	public Camera overlayCamera;

	// Token: 0x04001109 RID: 4361
	[NonSerialized]
	public Camera overlayNoDepthCamera;

	// Token: 0x0400110A RID: 4362
	[NonSerialized]
	public Camera uiCamera;

	// Token: 0x0400110B RID: 4363
	[NonSerialized]
	public Camera lightBufferCamera;

	// Token: 0x0400110C RID: 4364
	[NonSerialized]
	public Camera simOverlayCamera;

	// Token: 0x0400110D RID: 4365
	[NonSerialized]
	public Camera infraredCamera;

	// Token: 0x0400110E RID: 4366
	[NonSerialized]
	public Camera timelapseFreezeCamera;

	// Token: 0x0400110F RID: 4367
	[SerializeField]
	private List<GameScreenManager.UIRenderTarget> uiCameraTargets;

	// Token: 0x04001110 RID: 4368
	public List<Camera> cameras = new List<Camera>();

	// Token: 0x04001111 RID: 4369
	private MultipleRenderTarget mrt;

	// Token: 0x04001112 RID: 4370
	public SoundCuller soundCuller;

	// Token: 0x04001113 RID: 4371
	private bool cinemaCamEnabled;

	// Token: 0x04001114 RID: 4372
	private bool cinemaToggleLock;

	// Token: 0x04001115 RID: 4373
	private bool cinemaToggleEasing;

	// Token: 0x04001116 RID: 4374
	private bool cinemaUnpauseNextMove;

	// Token: 0x04001117 RID: 4375
	private bool cinemaPanLeft;

	// Token: 0x04001118 RID: 4376
	private bool cinemaPanRight;

	// Token: 0x04001119 RID: 4377
	private bool cinemaPanUp;

	// Token: 0x0400111A RID: 4378
	private bool cinemaPanDown;

	// Token: 0x0400111B RID: 4379
	private bool cinemaZoomIn;

	// Token: 0x0400111C RID: 4380
	private bool cinemaZoomOut;

	// Token: 0x0400111D RID: 4381
	private int cinemaZoomSpeed = 10;

	// Token: 0x0400111E RID: 4382
	private float cinemaEasing = 0.05f;

	// Token: 0x0400111F RID: 4383
	private float cinemaZoomVelocity;

	// Token: 0x04001121 RID: 4385
	private float smoothDt;

	// Token: 0x020012ED RID: 4845
	public class Tuning : TuningData<CameraController.Tuning>
	{
		// Token: 0x04006506 RID: 25862
		public float maxOrthographicSizeDebug;

		// Token: 0x04006507 RID: 25863
		public float cinemaZoomFactor = 100f;

		// Token: 0x04006508 RID: 25864
		public float cinemaPanFactor = 50f;

		// Token: 0x04006509 RID: 25865
		public float cinemaZoomToFactor = 100f;

		// Token: 0x0400650A RID: 25866
		public float cinemaPanToFactor = 50f;

		// Token: 0x0400650B RID: 25867
		public float targetZoomEasingFactor = 400f;

		// Token: 0x0400650C RID: 25868
		public float targetPanEasingFactor = 100f;
	}
}
