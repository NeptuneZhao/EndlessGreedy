using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000BF3 RID: 3059
public class ClusterMapScreen : KScreen
{
	// Token: 0x06005D26 RID: 23846 RVA: 0x00223B9A File Offset: 0x00221D9A
	public static void DestroyInstance()
	{
		ClusterMapScreen.Instance = null;
	}

	// Token: 0x06005D27 RID: 23847 RVA: 0x00223BA2 File Offset: 0x00221DA2
	public ClusterMapVisualizer GetEntityVisAnim(ClusterGridEntity entity)
	{
		if (this.m_gridEntityAnims.ContainsKey(entity))
		{
			return this.m_gridEntityAnims[entity];
		}
		return null;
	}

	// Token: 0x06005D28 RID: 23848 RVA: 0x00223BC0 File Offset: 0x00221DC0
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return 20f;
	}

	// Token: 0x06005D29 RID: 23849 RVA: 0x00223BD5 File Offset: 0x00221DD5
	public float CurrentZoomPercentage()
	{
		return (this.m_currentZoomScale - 50f) / 100f;
	}

	// Token: 0x06005D2A RID: 23850 RVA: 0x00223BE9 File Offset: 0x00221DE9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.m_selectMarker = global::Util.KInstantiateUI<SelectMarker>(this.selectMarkerPrefab, base.gameObject, false);
		this.m_selectMarker.gameObject.SetActive(false);
		ClusterMapScreen.Instance = this;
	}

	// Token: 0x06005D2B RID: 23851 RVA: 0x00223C20 File Offset: 0x00221E20
	protected override void OnSpawn()
	{
		base.OnSpawn();
		global::Debug.Assert(this.cellVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), "The radius of the cellVisPrefab hex must be 1");
		global::Debug.Assert(this.terrainVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), "The radius of the terrainVisPrefab hex must be 1");
		global::Debug.Assert(this.mobileVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), "The radius of the mobileVisPrefab hex must be 1");
		global::Debug.Assert(this.staticVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), "The radius of the staticVisPrefab hex must be 1");
		int num;
		int num2;
		int num3;
		int num4;
		this.GenerateGridVis(out num, out num2, out num3, out num4);
		this.Show(false);
		this.mapScrollRect.content.sizeDelta = new Vector2((float)(num2 * 4), (float)(num4 * 4));
		this.mapScrollRect.content.localScale = new Vector3(this.m_currentZoomScale, this.m_currentZoomScale, 1f);
		this.m_onDestinationChangedDelegate = new Action<object>(this.OnDestinationChanged);
		this.m_onSelectObjectDelegate = new Action<object>(this.OnSelectObject);
		base.Subscribe(1980521255, new Action<object>(this.UpdateVis));
	}

	// Token: 0x06005D2C RID: 23852 RVA: 0x00223D80 File Offset: 0x00221F80
	protected void MoveToNISPosition()
	{
		if (!this.movingToTargetNISPosition)
		{
			return;
		}
		Vector3 b = new Vector3(-this.targetNISPosition.x * this.mapScrollRect.content.localScale.x, -this.targetNISPosition.y * this.mapScrollRect.content.localScale.y, this.targetNISPosition.z);
		this.m_targetZoomScale = Mathf.Lerp(this.m_targetZoomScale, this.targetNISZoom, Time.unscaledDeltaTime * 2f);
		this.mapScrollRect.content.SetLocalPosition(Vector3.Lerp(this.mapScrollRect.content.GetLocalPosition(), b, Time.unscaledDeltaTime * 2.5f));
		float num = Vector3.Distance(this.mapScrollRect.content.GetLocalPosition(), b);
		if (num < 100f)
		{
			ClusterMapHex component = this.m_cellVisByLocation[this.selectOnMoveNISComplete].GetComponent<ClusterMapHex>();
			if (this.m_selectedHex != component)
			{
				this.SelectHex(component);
			}
			if (num < 10f)
			{
				this.movingToTargetNISPosition = false;
			}
		}
	}

	// Token: 0x06005D2D RID: 23853 RVA: 0x00223E9A File Offset: 0x0022209A
	public void SetTargetFocusPosition(AxialI targetPosition, float delayBeforeMove = 0.5f)
	{
		if (this.activeMoveToTargetRoutine != null)
		{
			base.StopCoroutine(this.activeMoveToTargetRoutine);
		}
		this.activeMoveToTargetRoutine = base.StartCoroutine(this.MoveToTargetRoutine(targetPosition, delayBeforeMove));
	}

	// Token: 0x06005D2E RID: 23854 RVA: 0x00223EC4 File Offset: 0x002220C4
	private IEnumerator MoveToTargetRoutine(AxialI targetPosition, float delayBeforeMove)
	{
		delayBeforeMove = Mathf.Max(delayBeforeMove, 0f);
		yield return SequenceUtil.WaitForSecondsRealtime(delayBeforeMove);
		this.targetNISPosition = AxialUtil.AxialToWorld((float)targetPosition.r, (float)targetPosition.q);
		this.targetNISZoom = 150f;
		this.movingToTargetNISPosition = true;
		this.selectOnMoveNISComplete = targetPosition;
		yield break;
	}

	// Token: 0x06005D2F RID: 23855 RVA: 0x00223EE4 File Offset: 0x002220E4
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!e.Consumed && (e.IsAction(global::Action.ZoomIn) || e.IsAction(global::Action.ZoomOut)) && CameraController.IsMouseOverGameWindow)
		{
			List<RaycastResult> list = new List<RaycastResult>();
			PointerEventData pointerEventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
			pointerEventData.position = KInputManager.GetMousePos();
			UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
			if (current != null)
			{
				current.RaycastAll(pointerEventData, list);
				bool flag = false;
				foreach (RaycastResult raycastResult in list)
				{
					if (!raycastResult.gameObject.transform.IsChildOf(base.transform))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					float num;
					if (KInputManager.currentControllerIsGamepad)
					{
						num = 25f;
						num *= (float)(e.IsAction(global::Action.ZoomIn) ? 1 : -1);
					}
					else
					{
						num = Input.mouseScrollDelta.y * 25f;
					}
					this.m_targetZoomScale = Mathf.Clamp(this.m_targetZoomScale + num, 50f, 150f);
					e.TryConsume(global::Action.ZoomIn);
					if (!e.Consumed)
					{
						e.TryConsume(global::Action.ZoomOut);
					}
				}
			}
		}
		CameraController.Instance.ChangeWorldInput(e);
		base.OnKeyDown(e);
	}

	// Token: 0x06005D30 RID: 23856 RVA: 0x0022403C File Offset: 0x0022223C
	public bool TryHandleCancel()
	{
		if (this.m_mode == ClusterMapScreen.Mode.SelectDestination && !this.m_closeOnSelect)
		{
			this.SetMode(ClusterMapScreen.Mode.Default);
			return true;
		}
		return false;
	}

	// Token: 0x06005D31 RID: 23857 RVA: 0x0022405C File Offset: 0x0022225C
	public void ShowInSelectDestinationMode(ClusterDestinationSelector destination_selector)
	{
		this.m_destinationSelector = destination_selector;
		if (!base.gameObject.activeSelf)
		{
			ManagementMenu.Instance.ToggleClusterMap();
			this.m_closeOnSelect = true;
		}
		ClusterGridEntity component = destination_selector.GetComponent<ClusterGridEntity>();
		this.SetSelectedEntity(component, false);
		if (this.m_selectedEntity != null)
		{
			this.m_selectedHex = this.m_cellVisByLocation[this.m_selectedEntity.Location].GetComponent<ClusterMapHex>();
		}
		else
		{
			AxialI myWorldLocation = destination_selector.GetMyWorldLocation();
			ClusterMapHex component2 = this.m_cellVisByLocation[myWorldLocation].GetComponent<ClusterMapHex>();
			this.m_selectedHex = component2;
		}
		this.SetMode(ClusterMapScreen.Mode.SelectDestination);
	}

	// Token: 0x06005D32 RID: 23858 RVA: 0x002240F5 File Offset: 0x002222F5
	private void SetMode(ClusterMapScreen.Mode mode)
	{
		this.m_mode = mode;
		if (this.m_mode == ClusterMapScreen.Mode.Default)
		{
			this.m_destinationSelector = null;
		}
		this.UpdateVis(null);
	}

	// Token: 0x06005D33 RID: 23859 RVA: 0x00224114 File Offset: 0x00222314
	public ClusterMapScreen.Mode GetMode()
	{
		return this.m_mode;
	}

	// Token: 0x06005D34 RID: 23860 RVA: 0x0022411C File Offset: 0x0022231C
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.MoveToNISPosition();
			this.UpdateVis(null);
			if (this.m_mode == ClusterMapScreen.Mode.Default)
			{
				this.TrySelectDefault();
			}
			Game.Instance.Subscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
			Game.Instance.Subscribe(-1554423969, new Action<object>(this.OnNewTelescopeTarget));
			Game.Instance.Subscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
			ClusterMapSelectTool.Instance.Activate();
			this.SetShowingNonClusterMapHud(false);
			CameraController.Instance.DisableUserCameraControl = true;
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MENUStarmapNotPausedSnapshot);
			MusicManager.instance.PlaySong("Music_Starmap", false);
			this.UpdateTearStatus();
			return;
		}
		Game.Instance.Unsubscribe(-1554423969, new Action<object>(this.OnNewTelescopeTarget));
		Game.Instance.Unsubscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
		Game.Instance.Unsubscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
		this.m_mode = ClusterMapScreen.Mode.Default;
		this.m_closeOnSelect = false;
		this.m_destinationSelector = null;
		SelectTool.Instance.Activate();
		this.SetShowingNonClusterMapHud(true);
		CameraController.Instance.DisableUserCameraControl = false;
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUStarmapNotPausedSnapshot, STOP_MODE.ALLOWFADEOUT);
		if (MusicManager.instance.SongIsPlaying("Music_Starmap"))
		{
			MusicManager.instance.StopSong("Music_Starmap", true, STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x06005D35 RID: 23861 RVA: 0x002242A7 File Offset: 0x002224A7
	private void SetShowingNonClusterMapHud(bool show)
	{
		PlanScreen.Instance.gameObject.SetActive(show);
		ToolMenu.Instance.gameObject.SetActive(show);
		OverlayScreen.Instance.gameObject.SetActive(show);
	}

	// Token: 0x06005D36 RID: 23862 RVA: 0x002242DC File Offset: 0x002224DC
	private void SetSelectedEntity(ClusterGridEntity entity, bool frameDelay = false)
	{
		if (this.m_selectedEntity != null)
		{
			this.m_selectedEntity.Unsubscribe(543433792, this.m_onDestinationChangedDelegate);
			this.m_selectedEntity.Unsubscribe(-1503271301, this.m_onSelectObjectDelegate);
		}
		this.m_selectedEntity = entity;
		if (this.m_selectedEntity != null)
		{
			this.m_selectedEntity.Subscribe(543433792, this.m_onDestinationChangedDelegate);
			this.m_selectedEntity.Subscribe(-1503271301, this.m_onSelectObjectDelegate);
		}
		KSelectable new_selected = (this.m_selectedEntity != null) ? this.m_selectedEntity.GetComponent<KSelectable>() : null;
		if (frameDelay)
		{
			ClusterMapSelectTool.Instance.SelectNextFrame(new_selected, false);
			return;
		}
		ClusterMapSelectTool.Instance.Select(new_selected, false);
	}

	// Token: 0x06005D37 RID: 23863 RVA: 0x0022439F File Offset: 0x0022259F
	private void OnDestinationChanged(object data)
	{
		this.UpdateVis(null);
	}

	// Token: 0x06005D38 RID: 23864 RVA: 0x002243A8 File Offset: 0x002225A8
	private void OnSelectObject(object data)
	{
		if (this.m_selectedEntity == null)
		{
			return;
		}
		KSelectable component = this.m_selectedEntity.GetComponent<KSelectable>();
		if (component == null || component.IsSelected)
		{
			return;
		}
		this.SetSelectedEntity(null, false);
		if (this.m_mode == ClusterMapScreen.Mode.SelectDestination)
		{
			if (this.m_closeOnSelect)
			{
				ManagementMenu.Instance.CloseAll();
			}
			else
			{
				this.SetMode(ClusterMapScreen.Mode.Default);
			}
		}
		this.UpdateVis(null);
	}

	// Token: 0x06005D39 RID: 23865 RVA: 0x00224415 File Offset: 0x00222615
	private void OnFogOfWarRevealed(object data = null)
	{
		this.UpdateVis(null);
	}

	// Token: 0x06005D3A RID: 23866 RVA: 0x0022441E File Offset: 0x0022261E
	private void OnNewTelescopeTarget(object data = null)
	{
		this.UpdateVis(null);
	}

	// Token: 0x06005D3B RID: 23867 RVA: 0x00224427 File Offset: 0x00222627
	private void Update()
	{
		if (KInputManager.currentControllerIsGamepad)
		{
			this.mapScrollRect.AnalogUpdate(KInputManager.steamInputInterpreter.GetSteamCameraMovement() * this.scrollSpeed);
		}
	}

	// Token: 0x06005D3C RID: 23868 RVA: 0x00224450 File Offset: 0x00222650
	private void TrySelectDefault()
	{
		if (this.m_selectedHex != null && this.m_selectedEntity != null)
		{
			this.UpdateVis(null);
			return;
		}
		WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
		if (activeWorld == null)
		{
			return;
		}
		ClusterGridEntity component = activeWorld.GetComponent<ClusterGridEntity>();
		if (component == null)
		{
			return;
		}
		this.SelectEntity(component, false);
	}

	// Token: 0x06005D3D RID: 23869 RVA: 0x002244B0 File Offset: 0x002226B0
	private void GenerateGridVis(out int minR, out int maxR, out int minQ, out int maxQ)
	{
		minR = int.MaxValue;
		maxR = int.MinValue;
		minQ = int.MaxValue;
		maxQ = int.MinValue;
		foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> keyValuePair in ClusterGrid.Instance.cellContents)
		{
			ClusterMapVisualizer clusterMapVisualizer = UnityEngine.Object.Instantiate<ClusterMapVisualizer>(this.cellVisPrefab, Vector3.zero, Quaternion.identity, this.cellVisContainer.transform);
			clusterMapVisualizer.rectTransform().SetLocalPosition(keyValuePair.Key.ToWorld());
			clusterMapVisualizer.gameObject.SetActive(true);
			ClusterMapHex component = clusterMapVisualizer.GetComponent<ClusterMapHex>();
			component.SetLocation(keyValuePair.Key);
			this.m_cellVisByLocation.Add(keyValuePair.Key, clusterMapVisualizer);
			minR = Mathf.Min(minR, component.location.R);
			maxR = Mathf.Max(maxR, component.location.R);
			minQ = Mathf.Min(minQ, component.location.Q);
			maxQ = Mathf.Max(maxQ, component.location.Q);
		}
		this.SetupVisGameObjects();
		this.UpdateVis(null);
	}

	// Token: 0x06005D3E RID: 23870 RVA: 0x00224604 File Offset: 0x00222804
	public Transform GetGridEntityNameTarget(ClusterGridEntity entity)
	{
		ClusterMapVisualizer clusterMapVisualizer;
		if (this.m_currentZoomScale >= 115f && this.m_gridEntityVis.TryGetValue(entity, out clusterMapVisualizer))
		{
			return clusterMapVisualizer.nameTarget;
		}
		return null;
	}

	// Token: 0x06005D3F RID: 23871 RVA: 0x00224638 File Offset: 0x00222838
	public override void ScreenUpdate(bool topLevel)
	{
		float t = Mathf.Min(4f * Time.unscaledDeltaTime, 0.9f);
		this.m_currentZoomScale = Mathf.Lerp(this.m_currentZoomScale, this.m_targetZoomScale, t);
		Vector2 v = KInputManager.GetMousePos();
		Vector3 b = this.mapScrollRect.content.InverseTransformPoint(v);
		this.mapScrollRect.content.localScale = new Vector3(this.m_currentZoomScale, this.m_currentZoomScale, 1f);
		Vector3 a = this.mapScrollRect.content.InverseTransformPoint(v);
		this.mapScrollRect.content.localPosition += (a - b) * this.m_currentZoomScale;
		this.MoveToNISPosition();
		this.FloatyAsteroidAnimation();
	}

	// Token: 0x06005D40 RID: 23872 RVA: 0x0022470C File Offset: 0x0022290C
	private void FloatyAsteroidAnimation()
	{
		float num = 0f;
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			AsteroidGridEntity component = worldContainer.GetComponent<AsteroidGridEntity>();
			if (component != null && this.m_gridEntityVis.ContainsKey(component) && ClusterMapScreen.GetRevealLevel(component) == ClusterRevealLevel.Visible)
			{
				KAnimControllerBase firstAnimController = this.m_gridEntityVis[component].GetFirstAnimController();
				float y = this.floatCycleOffset + this.floatCycleScale * Mathf.Sin(this.floatCycleSpeed * (num + GameClock.Instance.GetTime()));
				firstAnimController.Offset = new Vector2(0f, y);
			}
			num += 1f;
		}
	}

	// Token: 0x06005D41 RID: 23873 RVA: 0x002247DC File Offset: 0x002229DC
	private void SetupVisGameObjects()
	{
		foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> keyValuePair in ClusterGrid.Instance.cellContents)
		{
			foreach (ClusterGridEntity clusterGridEntity in keyValuePair.Value)
			{
				ClusterGrid.Instance.GetCellRevealLevel(keyValuePair.Key);
				ClusterRevealLevel isVisibleInFOW = clusterGridEntity.IsVisibleInFOW;
				ClusterRevealLevel revealLevel = ClusterMapScreen.GetRevealLevel(clusterGridEntity);
				if (clusterGridEntity.IsVisible && revealLevel != ClusterRevealLevel.Hidden && !this.m_gridEntityVis.ContainsKey(clusterGridEntity))
				{
					ClusterMapVisualizer original = null;
					GameObject gameObject = null;
					switch (clusterGridEntity.Layer)
					{
					case EntityLayer.Asteroid:
						original = this.terrainVisPrefab;
						gameObject = this.terrainVisContainer;
						break;
					case EntityLayer.Craft:
						original = this.mobileVisPrefab;
						gameObject = this.mobileVisContainer;
						break;
					case EntityLayer.POI:
						original = this.staticVisPrefab;
						gameObject = this.POIVisContainer;
						break;
					case EntityLayer.Telescope:
						original = this.staticVisPrefab;
						gameObject = this.telescopeVisContainer;
						break;
					case EntityLayer.Payload:
						original = this.mobileVisPrefab;
						gameObject = this.mobileVisContainer;
						break;
					case EntityLayer.FX:
						original = this.staticVisPrefab;
						gameObject = this.FXVisContainer;
						break;
					}
					ClusterNameDisplayScreen.Instance.AddNewEntry(clusterGridEntity);
					ClusterMapVisualizer clusterMapVisualizer = UnityEngine.Object.Instantiate<ClusterMapVisualizer>(original, gameObject.transform);
					clusterMapVisualizer.Init(clusterGridEntity, this.pathDrawer);
					clusterMapVisualizer.gameObject.SetActive(true);
					this.m_gridEntityAnims.Add(clusterGridEntity, clusterMapVisualizer);
					this.m_gridEntityVis.Add(clusterGridEntity, clusterMapVisualizer);
					clusterGridEntity.positionDirty = false;
					clusterGridEntity.Subscribe(1502190696, new Action<object>(this.RemoveDeletedEntities));
				}
			}
		}
		this.RemoveDeletedEntities(null);
		foreach (KeyValuePair<ClusterGridEntity, ClusterMapVisualizer> keyValuePair2 in this.m_gridEntityVis)
		{
			ClusterGridEntity key = keyValuePair2.Key;
			if (key.Layer == EntityLayer.Asteroid)
			{
				int id = key.GetComponent<WorldContainer>().id;
				keyValuePair2.Value.alertVignette.worldID = id;
			}
		}
	}

	// Token: 0x06005D42 RID: 23874 RVA: 0x00224A60 File Offset: 0x00222C60
	private void RemoveDeletedEntities(object obj = null)
	{
		foreach (ClusterGridEntity key in (from x in this.m_gridEntityVis.Keys
		where x == null || x.gameObject == (GameObject)obj
		select x).ToList<ClusterGridEntity>())
		{
			global::Util.KDestroyGameObject(this.m_gridEntityVis[key]);
			this.m_gridEntityVis.Remove(key);
			this.m_gridEntityAnims.Remove(key);
		}
	}

	// Token: 0x06005D43 RID: 23875 RVA: 0x00224B00 File Offset: 0x00222D00
	private void OnClusterLocationChanged(object data)
	{
		this.UpdateVis(null);
	}

	// Token: 0x06005D44 RID: 23876 RVA: 0x00224B0C File Offset: 0x00222D0C
	public static ClusterRevealLevel GetRevealLevel(ClusterGridEntity entity)
	{
		ClusterRevealLevel cellRevealLevel = ClusterGrid.Instance.GetCellRevealLevel(entity.Location);
		ClusterRevealLevel isVisibleInFOW = entity.IsVisibleInFOW;
		if (cellRevealLevel == ClusterRevealLevel.Visible || isVisibleInFOW == ClusterRevealLevel.Visible)
		{
			return ClusterRevealLevel.Visible;
		}
		if (cellRevealLevel == ClusterRevealLevel.Peeked && isVisibleInFOW == ClusterRevealLevel.Peeked)
		{
			return ClusterRevealLevel.Peeked;
		}
		return ClusterRevealLevel.Hidden;
	}

	// Token: 0x06005D45 RID: 23877 RVA: 0x00224B48 File Offset: 0x00222D48
	private void UpdateVis(object data = null)
	{
		this.SetupVisGameObjects();
		this.UpdatePaths();
		foreach (KeyValuePair<ClusterGridEntity, ClusterMapVisualizer> keyValuePair in this.m_gridEntityAnims)
		{
			ClusterRevealLevel revealLevel = ClusterMapScreen.GetRevealLevel(keyValuePair.Key);
			keyValuePair.Value.Show(revealLevel);
			bool selected = this.m_selectedEntity == keyValuePair.Key;
			keyValuePair.Value.Select(selected);
			if (keyValuePair.Key.positionDirty)
			{
				Vector3 position = ClusterGrid.Instance.GetPosition(keyValuePair.Key);
				keyValuePair.Value.rectTransform().SetLocalPosition(position);
				keyValuePair.Key.positionDirty = false;
			}
		}
		if (this.m_selectedEntity != null && this.m_gridEntityVis.ContainsKey(this.m_selectedEntity))
		{
			ClusterMapVisualizer clusterMapVisualizer = this.m_gridEntityVis[this.m_selectedEntity];
			this.m_selectMarker.SetTargetTransform(clusterMapVisualizer.transform);
			this.m_selectMarker.gameObject.SetActive(true);
			clusterMapVisualizer.transform.SetAsLastSibling();
		}
		else
		{
			this.m_selectMarker.gameObject.SetActive(false);
		}
		foreach (KeyValuePair<AxialI, ClusterMapVisualizer> keyValuePair2 in this.m_cellVisByLocation)
		{
			ClusterMapHex component = keyValuePair2.Value.GetComponent<ClusterMapHex>();
			AxialI key = keyValuePair2.Key;
			component.SetRevealed(ClusterGrid.Instance.GetCellRevealLevel(key));
		}
		this.UpdateHexToggleStates();
		this.FloatyAsteroidAnimation();
	}

	// Token: 0x06005D46 RID: 23878 RVA: 0x00224D08 File Offset: 0x00222F08
	private void OnEntityDestroyed(object obj)
	{
		this.RemoveDeletedEntities(null);
	}

	// Token: 0x06005D47 RID: 23879 RVA: 0x00224D14 File Offset: 0x00222F14
	private void UpdateHexToggleStates()
	{
		bool flag = this.m_hoveredHex != null && ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.m_hoveredHex.location, EntityLayer.Asteroid);
		foreach (KeyValuePair<AxialI, ClusterMapVisualizer> keyValuePair in this.m_cellVisByLocation)
		{
			ClusterMapHex component = keyValuePair.Value.GetComponent<ClusterMapHex>();
			AxialI key = keyValuePair.Key;
			ClusterMapHex.ToggleState state;
			if (this.m_selectedHex != null && this.m_selectedHex.location == key)
			{
				state = ClusterMapHex.ToggleState.Selected;
			}
			else if (flag && this.m_hoveredHex.location.IsAdjacent(key))
			{
				state = ClusterMapHex.ToggleState.OrbitHighlight;
			}
			else
			{
				state = ClusterMapHex.ToggleState.Unselected;
			}
			component.UpdateToggleState(state);
		}
	}

	// Token: 0x06005D48 RID: 23880 RVA: 0x00224DEC File Offset: 0x00222FEC
	public void SelectEntity(ClusterGridEntity entity, bool frameDelay = false)
	{
		if (entity != null)
		{
			this.SetSelectedEntity(entity, frameDelay);
			ClusterMapHex component = this.m_cellVisByLocation[entity.Location].GetComponent<ClusterMapHex>();
			this.m_selectedHex = component;
		}
		this.UpdateVis(null);
	}

	// Token: 0x06005D49 RID: 23881 RVA: 0x00224E30 File Offset: 0x00223030
	public void SelectHex(ClusterMapHex newSelectionHex)
	{
		if (this.m_mode == ClusterMapScreen.Mode.Default)
		{
			List<ClusterGridEntity> visibleEntitiesAtCell = ClusterGrid.Instance.GetVisibleEntitiesAtCell(newSelectionHex.location);
			for (int i = visibleEntitiesAtCell.Count - 1; i >= 0; i--)
			{
				KSelectable component = visibleEntitiesAtCell[i].GetComponent<KSelectable>();
				if (component == null || !component.IsSelectable)
				{
					visibleEntitiesAtCell.RemoveAt(i);
				}
			}
			if (visibleEntitiesAtCell.Count == 0)
			{
				this.SetSelectedEntity(null, false);
			}
			else
			{
				int num = visibleEntitiesAtCell.IndexOf(this.m_selectedEntity);
				int index = 0;
				if (num >= 0)
				{
					index = (num + 1) % visibleEntitiesAtCell.Count;
				}
				this.SetSelectedEntity(visibleEntitiesAtCell[index], false);
			}
			this.m_selectedHex = newSelectionHex;
		}
		else if (this.m_mode == ClusterMapScreen.Mode.SelectDestination)
		{
			global::Debug.Assert(this.m_destinationSelector != null, "Selected a hex in SelectDestination mode with no ClusterDestinationSelector");
			if (ClusterGrid.Instance.GetPath(this.m_selectedHex.location, newSelectionHex.location, this.m_destinationSelector) != null)
			{
				this.m_destinationSelector.SetDestination(newSelectionHex.location);
				if (this.m_closeOnSelect)
				{
					ManagementMenu.Instance.CloseAll();
				}
				else
				{
					this.SetMode(ClusterMapScreen.Mode.Default);
				}
			}
		}
		this.UpdateVis(null);
	}

	// Token: 0x06005D4A RID: 23882 RVA: 0x00224F50 File Offset: 0x00223150
	public bool HasCurrentHover()
	{
		return this.m_hoveredHex != null;
	}

	// Token: 0x06005D4B RID: 23883 RVA: 0x00224F5E File Offset: 0x0022315E
	public AxialI GetCurrentHoverLocation()
	{
		return this.m_hoveredHex.location;
	}

	// Token: 0x06005D4C RID: 23884 RVA: 0x00224F6B File Offset: 0x0022316B
	public void OnHoverHex(ClusterMapHex newHoverHex)
	{
		this.m_hoveredHex = newHoverHex;
		if (this.m_mode == ClusterMapScreen.Mode.SelectDestination)
		{
			this.UpdateVis(null);
		}
		this.UpdateHexToggleStates();
	}

	// Token: 0x06005D4D RID: 23885 RVA: 0x00224F8A File Offset: 0x0022318A
	public void OnUnhoverHex(ClusterMapHex unhoveredHex)
	{
		if (this.m_hoveredHex == unhoveredHex)
		{
			this.m_hoveredHex = null;
			this.UpdateHexToggleStates();
		}
	}

	// Token: 0x06005D4E RID: 23886 RVA: 0x00224FA7 File Offset: 0x002231A7
	public void SetLocationHighlight(AxialI location, bool highlight)
	{
		this.m_cellVisByLocation[location].GetComponent<ClusterMapHex>().ChangeState(highlight ? 1 : 0);
	}

	// Token: 0x06005D4F RID: 23887 RVA: 0x00224FC8 File Offset: 0x002231C8
	private void UpdatePaths()
	{
		ClusterDestinationSelector clusterDestinationSelector = (this.m_selectedEntity != null) ? this.m_selectedEntity.GetComponent<ClusterDestinationSelector>() : null;
		if (this.m_mode != ClusterMapScreen.Mode.SelectDestination || !(this.m_hoveredHex != null))
		{
			if (this.m_previewMapPath != null)
			{
				global::Util.KDestroyGameObject(this.m_previewMapPath);
				this.m_previewMapPath = null;
			}
			return;
		}
		global::Debug.Assert(this.m_destinationSelector != null, "In SelectDestination mode without a destination selector");
		AxialI myWorldLocation = this.m_destinationSelector.GetMyWorldLocation();
		string text;
		List<AxialI> path = ClusterGrid.Instance.GetPath(myWorldLocation, this.m_hoveredHex.location, this.m_destinationSelector, out text, false);
		if (path != null)
		{
			if (this.m_previewMapPath == null)
			{
				this.m_previewMapPath = this.pathDrawer.AddPath();
			}
			ClusterMapVisualizer clusterMapVisualizer = this.m_gridEntityVis[this.GetSelectorGridEntity(this.m_destinationSelector)];
			this.m_previewMapPath.SetPoints(ClusterMapPathDrawer.GetDrawPathList(clusterMapVisualizer.transform.localPosition, path));
			this.m_previewMapPath.SetColor(this.rocketPreviewPathColor);
		}
		else if (this.m_previewMapPath != null)
		{
			global::Util.KDestroyGameObject(this.m_previewMapPath);
			this.m_previewMapPath = null;
		}
		int num = (path != null) ? path.Count : -1;
		if (this.m_selectedEntity != null)
		{
			int rangeInTiles = this.m_selectedEntity.GetComponent<IClusterRange>().GetRangeInTiles();
			if (num > rangeInTiles && string.IsNullOrEmpty(text))
			{
				text = string.Format(UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_OUT_OF_RANGE, rangeInTiles);
			}
			bool repeat = clusterDestinationSelector.GetComponent<RocketClusterDestinationSelector>().Repeat;
			this.m_hoveredHex.SetDestinationStatus(text, num, rangeInTiles, repeat);
			return;
		}
		this.m_hoveredHex.SetDestinationStatus(text);
	}

	// Token: 0x06005D50 RID: 23888 RVA: 0x00225184 File Offset: 0x00223384
	private ClusterGridEntity GetSelectorGridEntity(ClusterDestinationSelector selector)
	{
		ClusterGridEntity component = selector.GetComponent<ClusterGridEntity>();
		if (component != null && ClusterGrid.Instance.IsVisible(component))
		{
			return component;
		}
		ClusterGridEntity visibleEntityOfLayerAtCell = ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(selector.GetMyWorldLocation(), EntityLayer.Asteroid);
		global::Debug.Assert(component != null || visibleEntityOfLayerAtCell != null, string.Format("{0} has no grid entity and isn't located at a visible asteroid at {1}", selector, selector.GetMyWorldLocation()));
		if (visibleEntityOfLayerAtCell)
		{
			return visibleEntityOfLayerAtCell;
		}
		return component;
	}

	// Token: 0x06005D51 RID: 23889 RVA: 0x002251FC File Offset: 0x002233FC
	private void UpdateTearStatus()
	{
		ClusterPOIManager clusterPOIManager = null;
		if (ClusterManager.Instance != null)
		{
			clusterPOIManager = ClusterManager.Instance.GetComponent<ClusterPOIManager>();
		}
		if (clusterPOIManager != null)
		{
			TemporalTear temporalTear = clusterPOIManager.GetTemporalTear();
			if (temporalTear != null)
			{
				temporalTear.UpdateStatus();
			}
		}
	}

	// Token: 0x04003E63 RID: 15971
	public static ClusterMapScreen Instance;

	// Token: 0x04003E64 RID: 15972
	public GameObject cellVisContainer;

	// Token: 0x04003E65 RID: 15973
	public GameObject terrainVisContainer;

	// Token: 0x04003E66 RID: 15974
	public GameObject mobileVisContainer;

	// Token: 0x04003E67 RID: 15975
	public GameObject telescopeVisContainer;

	// Token: 0x04003E68 RID: 15976
	public GameObject POIVisContainer;

	// Token: 0x04003E69 RID: 15977
	public GameObject FXVisContainer;

	// Token: 0x04003E6A RID: 15978
	public ClusterMapVisualizer cellVisPrefab;

	// Token: 0x04003E6B RID: 15979
	public ClusterMapVisualizer terrainVisPrefab;

	// Token: 0x04003E6C RID: 15980
	public ClusterMapVisualizer mobileVisPrefab;

	// Token: 0x04003E6D RID: 15981
	public ClusterMapVisualizer staticVisPrefab;

	// Token: 0x04003E6E RID: 15982
	public Color rocketPathColor;

	// Token: 0x04003E6F RID: 15983
	public Color rocketSelectedPathColor;

	// Token: 0x04003E70 RID: 15984
	public Color rocketPreviewPathColor;

	// Token: 0x04003E71 RID: 15985
	private ClusterMapHex m_selectedHex;

	// Token: 0x04003E72 RID: 15986
	private ClusterMapHex m_hoveredHex;

	// Token: 0x04003E73 RID: 15987
	private ClusterGridEntity m_selectedEntity;

	// Token: 0x04003E74 RID: 15988
	public KButton closeButton;

	// Token: 0x04003E75 RID: 15989
	private const float ZOOM_SCALE_MIN = 50f;

	// Token: 0x04003E76 RID: 15990
	private const float ZOOM_SCALE_MAX = 150f;

	// Token: 0x04003E77 RID: 15991
	private const float ZOOM_SCALE_INCREMENT = 25f;

	// Token: 0x04003E78 RID: 15992
	private const float ZOOM_SCALE_SPEED = 4f;

	// Token: 0x04003E79 RID: 15993
	private const float ZOOM_NAME_THRESHOLD = 115f;

	// Token: 0x04003E7A RID: 15994
	private float m_currentZoomScale = 75f;

	// Token: 0x04003E7B RID: 15995
	private float m_targetZoomScale = 75f;

	// Token: 0x04003E7C RID: 15996
	private ClusterMapPath m_previewMapPath;

	// Token: 0x04003E7D RID: 15997
	private Dictionary<ClusterGridEntity, ClusterMapVisualizer> m_gridEntityVis = new Dictionary<ClusterGridEntity, ClusterMapVisualizer>();

	// Token: 0x04003E7E RID: 15998
	private Dictionary<ClusterGridEntity, ClusterMapVisualizer> m_gridEntityAnims = new Dictionary<ClusterGridEntity, ClusterMapVisualizer>();

	// Token: 0x04003E7F RID: 15999
	private Dictionary<AxialI, ClusterMapVisualizer> m_cellVisByLocation = new Dictionary<AxialI, ClusterMapVisualizer>();

	// Token: 0x04003E80 RID: 16000
	private Action<object> m_onDestinationChangedDelegate;

	// Token: 0x04003E81 RID: 16001
	private Action<object> m_onSelectObjectDelegate;

	// Token: 0x04003E82 RID: 16002
	[SerializeField]
	private KScrollRect mapScrollRect;

	// Token: 0x04003E83 RID: 16003
	[SerializeField]
	private float scrollSpeed = 15f;

	// Token: 0x04003E84 RID: 16004
	public GameObject selectMarkerPrefab;

	// Token: 0x04003E85 RID: 16005
	public ClusterMapPathDrawer pathDrawer;

	// Token: 0x04003E86 RID: 16006
	private SelectMarker m_selectMarker;

	// Token: 0x04003E87 RID: 16007
	private bool movingToTargetNISPosition;

	// Token: 0x04003E88 RID: 16008
	private Vector3 targetNISPosition;

	// Token: 0x04003E89 RID: 16009
	private float targetNISZoom;

	// Token: 0x04003E8A RID: 16010
	private AxialI selectOnMoveNISComplete;

	// Token: 0x04003E8B RID: 16011
	private ClusterMapScreen.Mode m_mode;

	// Token: 0x04003E8C RID: 16012
	private ClusterDestinationSelector m_destinationSelector;

	// Token: 0x04003E8D RID: 16013
	private bool m_closeOnSelect;

	// Token: 0x04003E8E RID: 16014
	private Coroutine activeMoveToTargetRoutine;

	// Token: 0x04003E8F RID: 16015
	public float floatCycleScale = 4f;

	// Token: 0x04003E90 RID: 16016
	public float floatCycleOffset = 0.75f;

	// Token: 0x04003E91 RID: 16017
	public float floatCycleSpeed = 0.75f;

	// Token: 0x02001CD7 RID: 7383
	public enum Mode
	{
		// Token: 0x04008545 RID: 34117
		Default,
		// Token: 0x04008546 RID: 34118
		SelectDestination
	}
}
