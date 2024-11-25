using System;
using System.Collections.Generic;
using System.Linq;
using Klei.Input;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000911 RID: 2321
[AddComponentMenu("KMonoBehaviour/scripts/InterfaceTool")]
public class InterfaceTool : KMonoBehaviour
{
	// Token: 0x170004E0 RID: 1248
	// (get) Token: 0x06004318 RID: 17176 RVA: 0x0017D682 File Offset: 0x0017B882
	public static InterfaceToolConfig ActiveConfig
	{
		get
		{
			if (InterfaceTool.interfaceConfigMap == null)
			{
				InterfaceTool.InitializeConfigs(global::Action.Invalid, null);
			}
			return InterfaceTool.activeConfigs[InterfaceTool.activeConfigs.Count - 1];
		}
	}

	// Token: 0x06004319 RID: 17177 RVA: 0x0017D6A8 File Offset: 0x0017B8A8
	public static void ToggleConfig(global::Action configKey)
	{
		if (InterfaceTool.interfaceConfigMap == null)
		{
			InterfaceTool.InitializeConfigs(global::Action.Invalid, null);
		}
		InterfaceToolConfig item;
		if (!InterfaceTool.interfaceConfigMap.TryGetValue(configKey, out item))
		{
			global::Debug.LogWarning(string.Format("[InterfaceTool] No config is associated with Key: {0}!", configKey) + " Are you sure the configs were initialized properly!");
			return;
		}
		if (InterfaceTool.activeConfigs.BinarySearch(item, InterfaceToolConfig.ConfigComparer) <= 0)
		{
			global::Debug.Log(string.Format("[InterfaceTool] Pushing config with key: {0}", configKey));
			InterfaceTool.activeConfigs.Add(item);
			InterfaceTool.activeConfigs.Sort(InterfaceToolConfig.ConfigComparer);
			return;
		}
		global::Debug.Log(string.Format("[InterfaceTool] Popping config with key: {0}", configKey));
		InterfaceTool.activeConfigs.Remove(item);
	}

	// Token: 0x0600431A RID: 17178 RVA: 0x0017D758 File Offset: 0x0017B958
	public static void InitializeConfigs(global::Action defaultKey, List<InterfaceToolConfig> configs)
	{
		string arg = (configs == null) ? "null" : configs.Count.ToString();
		global::Debug.Log(string.Format("[InterfaceTool] Initializing configs with values of DefaultKey: {0} Configs: {1}", defaultKey, arg));
		if (configs == null || configs.Count == 0)
		{
			InterfaceToolConfig interfaceToolConfig = ScriptableObject.CreateInstance<InterfaceToolConfig>();
			InterfaceTool.interfaceConfigMap = new Dictionary<global::Action, InterfaceToolConfig>();
			InterfaceTool.interfaceConfigMap[interfaceToolConfig.InputAction] = interfaceToolConfig;
			return;
		}
		InterfaceTool.interfaceConfigMap = configs.ToDictionary((InterfaceToolConfig x) => x.InputAction);
		InterfaceTool.ToggleConfig(defaultKey);
	}

	// Token: 0x170004E1 RID: 1249
	// (get) Token: 0x0600431B RID: 17179 RVA: 0x0017D7F1 File Offset: 0x0017B9F1
	public HashedString ViewMode
	{
		get
		{
			return this.viewMode;
		}
	}

	// Token: 0x170004E2 RID: 1250
	// (get) Token: 0x0600431C RID: 17180 RVA: 0x0017D7F9 File Offset: 0x0017B9F9
	public virtual string[] DlcIDs
	{
		get
		{
			return DlcManager.AVAILABLE_ALL_VERSIONS;
		}
	}

	// Token: 0x0600431D RID: 17181 RVA: 0x0017D800 File Offset: 0x0017BA00
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.hoverTextConfiguration = base.GetComponent<HoverTextConfiguration>();
	}

	// Token: 0x0600431E RID: 17182 RVA: 0x0017D814 File Offset: 0x0017BA14
	public void ActivateTool()
	{
		this.OnActivateTool();
		this.OnMouseMove(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
		Game.Instance.Trigger(1174281782, this);
	}

	// Token: 0x0600431F RID: 17183 RVA: 0x0017D83C File Offset: 0x0017BA3C
	public virtual bool ShowHoverUI()
	{
		if (ManagementMenu.Instance == null || ManagementMenu.Instance.IsFullscreenUIActive())
		{
			return false;
		}
		Vector3 vector = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
		if (OverlayScreen.Instance == null || !ClusterManager.Instance.IsPositionInActiveWorld(vector) || vector.x < 0f || vector.x > Grid.WidthInMeters || vector.y < 0f || vector.y > Grid.HeightInMeters)
		{
			return false;
		}
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		return current != null && !current.IsPointerOverGameObject();
	}

	// Token: 0x06004320 RID: 17184 RVA: 0x0017D8E0 File Offset: 0x0017BAE0
	protected virtual void OnActivateTool()
	{
		if (OverlayScreen.Instance != null && this.viewMode != OverlayModes.None.ID && OverlayScreen.Instance.mode != this.viewMode)
		{
			OverlayScreen.Instance.ToggleOverlay(this.viewMode, true);
			InterfaceTool.toolActivatedViewMode = this.viewMode;
		}
		this.SetCursor(this.cursor, this.cursorOffset, CursorMode.Auto);
	}

	// Token: 0x06004321 RID: 17185 RVA: 0x0017D954 File Offset: 0x0017BB54
	public void SetCurrentVirtualInputModuleMousMovementMode(bool mouseMovementOnly, Action<VirtualInputModule> extraActions = null)
	{
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current != null && current.currentInputModule != null)
		{
			VirtualInputModule virtualInputModule = current.currentInputModule as VirtualInputModule;
			if (virtualInputModule != null)
			{
				virtualInputModule.mouseMovementOnly = mouseMovementOnly;
				if (extraActions != null)
				{
					extraActions(virtualInputModule);
				}
			}
		}
	}

	// Token: 0x06004322 RID: 17186 RVA: 0x0017D9A4 File Offset: 0x0017BBA4
	public void DeactivateTool(InterfaceTool new_tool = null)
	{
		this.OnDeactivateTool(new_tool);
		if ((new_tool == null || new_tool == SelectTool.Instance) && InterfaceTool.toolActivatedViewMode != OverlayModes.None.ID && InterfaceTool.toolActivatedViewMode == SimDebugView.Instance.GetMode())
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
			InterfaceTool.toolActivatedViewMode = OverlayModes.None.ID;
		}
	}

	// Token: 0x06004323 RID: 17187 RVA: 0x0017DA0F File Offset: 0x0017BC0F
	public virtual void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = null;
	}

	// Token: 0x06004324 RID: 17188 RVA: 0x0017DA14 File Offset: 0x0017BC14
	protected virtual void OnDeactivateTool(InterfaceTool new_tool)
	{
	}

	// Token: 0x06004325 RID: 17189 RVA: 0x0017DA16 File Offset: 0x0017BC16
	private void OnApplicationFocus(bool focusStatus)
	{
		this.isAppFocused = focusStatus;
	}

	// Token: 0x06004326 RID: 17190 RVA: 0x0017DA1F File Offset: 0x0017BC1F
	public virtual string GetDeactivateSound()
	{
		return "Tile_Cancel";
	}

	// Token: 0x06004327 RID: 17191 RVA: 0x0017DA28 File Offset: 0x0017BC28
	public virtual void OnMouseMove(Vector3 cursor_pos)
	{
		if (this.visualizer == null || !this.isAppFocused)
		{
			return;
		}
		cursor_pos = Grid.CellToPosCBC(Grid.PosToCell(cursor_pos), this.visualizerLayer);
		cursor_pos.z += -0.15f;
		this.visualizer.transform.SetLocalPosition(cursor_pos);
	}

	// Token: 0x06004328 RID: 17192 RVA: 0x0017DA84 File Offset: 0x0017BC84
	public virtual void OnKeyDown(KButtonEvent e)
	{
	}

	// Token: 0x06004329 RID: 17193 RVA: 0x0017DA86 File Offset: 0x0017BC86
	public virtual void OnKeyUp(KButtonEvent e)
	{
	}

	// Token: 0x0600432A RID: 17194 RVA: 0x0017DA88 File Offset: 0x0017BC88
	public virtual void OnLeftClickDown(Vector3 cursor_pos)
	{
	}

	// Token: 0x0600432B RID: 17195 RVA: 0x0017DA8A File Offset: 0x0017BC8A
	public virtual void OnLeftClickUp(Vector3 cursor_pos)
	{
	}

	// Token: 0x0600432C RID: 17196 RVA: 0x0017DA8C File Offset: 0x0017BC8C
	public virtual void OnRightClickDown(Vector3 cursor_pos, KButtonEvent e)
	{
	}

	// Token: 0x0600432D RID: 17197 RVA: 0x0017DA8E File Offset: 0x0017BC8E
	public virtual void OnRightClickUp(Vector3 cursor_pos)
	{
	}

	// Token: 0x0600432E RID: 17198 RVA: 0x0017DA90 File Offset: 0x0017BC90
	public virtual void OnFocus(bool focus)
	{
		if (this.visualizer != null)
		{
			this.visualizer.SetActive(focus);
		}
		this.hasFocus = focus;
	}

	// Token: 0x0600432F RID: 17199 RVA: 0x0017DAB4 File Offset: 0x0017BCB4
	protected Vector2 GetRegularizedPos(Vector2 input, bool minimize)
	{
		Vector3 vector = new Vector3(Grid.HalfCellSizeInMeters, Grid.HalfCellSizeInMeters, 0f);
		return Grid.CellToPosCCC(Grid.PosToCell(input), Grid.SceneLayer.Background) + (minimize ? (-vector) : vector);
	}

	// Token: 0x06004330 RID: 17200 RVA: 0x0017DAFC File Offset: 0x0017BCFC
	protected Vector2 GetWorldRestrictedPosition(Vector2 input)
	{
		input.x = Mathf.Clamp(input.x, ClusterManager.Instance.activeWorld.minimumBounds.x, ClusterManager.Instance.activeWorld.maximumBounds.x);
		input.y = Mathf.Clamp(input.y, ClusterManager.Instance.activeWorld.minimumBounds.y, ClusterManager.Instance.activeWorld.maximumBounds.y);
		return input;
	}

	// Token: 0x06004331 RID: 17201 RVA: 0x0017DB80 File Offset: 0x0017BD80
	protected void SetCursor(Texture2D new_cursor, Vector2 offset, CursorMode mode)
	{
		if (new_cursor != InterfaceTool.activeCursor && new_cursor != null)
		{
			InterfaceTool.activeCursor = new_cursor;
			try
			{
				Cursor.SetCursor(new_cursor, offset, mode);
				if (PlayerController.Instance.vim != null)
				{
					PlayerController.Instance.vim.SetCursor(new_cursor);
				}
			}
			catch (Exception ex)
			{
				string details = string.Format("SetCursor Failed new_cursor={0} offset={1} mode={2}", new_cursor, offset, mode);
				KCrashReporter.ReportDevNotification("SetCursor Failed", ex.StackTrace, details, false, null);
			}
		}
	}

	// Token: 0x06004332 RID: 17202 RVA: 0x0017DC14 File Offset: 0x0017BE14
	protected void UpdateHoverElements(List<KSelectable> hits)
	{
		if (this.hoverTextConfiguration != null)
		{
			this.hoverTextConfiguration.UpdateHoverElements(hits);
		}
	}

	// Token: 0x06004333 RID: 17203 RVA: 0x0017DC30 File Offset: 0x0017BE30
	public virtual void LateUpdate()
	{
		if (!this.populateHitsList)
		{
			this.UpdateHoverElements(null);
			return;
		}
		if (!this.isAppFocused)
		{
			return;
		}
		if (!Grid.IsValidCell(Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()))))
		{
			return;
		}
		this.hits.Clear();
		this.GetSelectablesUnderCursor(this.hits);
		KSelectable objectUnderCursor = this.GetObjectUnderCursor<KSelectable>(false, (KSelectable s) => s.GetComponent<KSelectable>().IsSelectable, null);
		this.UpdateHoverElements(this.hits);
		if (!this.hasFocus && this.hoverOverride == null)
		{
			this.ClearHover();
		}
		else if (objectUnderCursor != this.hover)
		{
			this.ClearHover();
			this.hover = objectUnderCursor;
			if (objectUnderCursor != null)
			{
				Game.Instance.Trigger(2095258329, objectUnderCursor.gameObject);
				objectUnderCursor.Hover(!this.playedSoundThisFrame);
				this.playedSoundThisFrame = true;
			}
		}
		this.playedSoundThisFrame = false;
	}

	// Token: 0x06004334 RID: 17204 RVA: 0x0017DD34 File Offset: 0x0017BF34
	public void GetSelectablesUnderCursor(List<KSelectable> hits)
	{
		if (this.hoverOverride != null)
		{
			hits.Add(this.hoverOverride);
		}
		Camera main = Camera.main;
		Vector3 position = new Vector3(KInputManager.GetMousePos().x, KInputManager.GetMousePos().y, -main.transform.GetPosition().z);
		Vector3 vector = main.ScreenToWorldPoint(position);
		Vector2 vector2 = new Vector2(vector.x, vector.y);
		int cell = Grid.PosToCell(vector);
		if (!Grid.IsValidCell(cell) || !Grid.IsVisible(cell))
		{
			return;
		}
		Game.Instance.statusItemRenderer.GetIntersections(vector2, hits);
		ListPool<ScenePartitionerEntry, SelectTool>.PooledList pooledList = ListPool<ScenePartitionerEntry, SelectTool>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)vector2.x, (int)vector2.y, 1, 1, GameScenePartitioner.Instance.collisionLayer, pooledList);
		pooledList.Sort((ScenePartitionerEntry x, ScenePartitionerEntry y) => this.SortHoverCards(x, y));
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			KCollider2D kcollider2D = scenePartitionerEntry.obj as KCollider2D;
			if (!(kcollider2D == null) && kcollider2D.Intersects(new Vector2(vector2.x, vector2.y)))
			{
				KSelectable kselectable = kcollider2D.GetComponent<KSelectable>();
				if (kselectable == null)
				{
					kselectable = kcollider2D.GetComponentInParent<KSelectable>();
				}
				if (!(kselectable == null) && kselectable.isActiveAndEnabled && !hits.Contains(kselectable) && kselectable.IsSelectable)
				{
					hits.Add(kselectable);
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06004335 RID: 17205 RVA: 0x0017DED8 File Offset: 0x0017C0D8
	public void SetLinkCursor(bool set)
	{
		this.SetCursor(set ? Assets.GetTexture("cursor_hand") : this.cursor, set ? Vector2.zero : this.cursorOffset, CursorMode.Auto);
	}

	// Token: 0x06004336 RID: 17206 RVA: 0x0017DF08 File Offset: 0x0017C108
	protected T GetObjectUnderCursor<T>(bool cycleSelection, Func<T, bool> condition = null, Component previous_selection = null) where T : MonoBehaviour
	{
		this.intersections.Clear();
		this.GetObjectUnderCursor2D<T>(this.intersections, condition, this.layerMask);
		this.intersections.RemoveAll(new Predicate<InterfaceTool.Intersection>(InterfaceTool.is_component_null));
		if (this.intersections.Count <= 0)
		{
			this.prevIntersectionGroup.Clear();
			return default(T);
		}
		this.curIntersectionGroup.Clear();
		foreach (InterfaceTool.Intersection intersection in this.intersections)
		{
			this.curIntersectionGroup.Add(intersection.component);
		}
		if (!this.prevIntersectionGroup.Equals(this.curIntersectionGroup))
		{
			this.hitCycleCount = 0;
			this.prevIntersectionGroup = this.curIntersectionGroup;
		}
		this.intersections.Sort((InterfaceTool.Intersection a, InterfaceTool.Intersection b) => this.SortSelectables(a.component as KMonoBehaviour, b.component as KMonoBehaviour));
		int index = 0;
		if (cycleSelection)
		{
			index = this.hitCycleCount % this.intersections.Count;
			if (this.intersections[index].component != previous_selection || previous_selection == null)
			{
				index = 0;
				this.hitCycleCount = 0;
			}
			else
			{
				int num = this.hitCycleCount + 1;
				this.hitCycleCount = num;
				index = num % this.intersections.Count;
			}
		}
		return this.intersections[index].component as T;
	}

	// Token: 0x06004337 RID: 17207 RVA: 0x0017E088 File Offset: 0x0017C288
	private void GetObjectUnderCursor2D<T>(List<InterfaceTool.Intersection> intersections, Func<T, bool> condition, int layer_mask) where T : MonoBehaviour
	{
		Camera main = Camera.main;
		Vector3 position = new Vector3(KInputManager.GetMousePos().x, KInputManager.GetMousePos().y, -main.transform.GetPosition().z);
		Vector3 vector = main.ScreenToWorldPoint(position);
		Vector2 pos = new Vector2(vector.x, vector.y);
		if (this.hoverOverride != null)
		{
			intersections.Add(new InterfaceTool.Intersection
			{
				component = this.hoverOverride,
				distance = -100f
			});
		}
		int cell = Grid.PosToCell(vector);
		if (Grid.IsValidCell(cell) && Grid.IsVisible(cell))
		{
			Game.Instance.statusItemRenderer.GetIntersections(pos, intersections);
			ListPool<ScenePartitionerEntry, SelectTool>.PooledList pooledList = ListPool<ScenePartitionerEntry, SelectTool>.Allocate();
			int x_bottomLeft = 0;
			int y_bottomLeft = 0;
			Grid.CellToXY(cell, out x_bottomLeft, out y_bottomLeft);
			GameScenePartitioner.Instance.GatherEntries(x_bottomLeft, y_bottomLeft, 1, 1, GameScenePartitioner.Instance.collisionLayer, pooledList);
			foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
			{
				KCollider2D kcollider2D = scenePartitionerEntry.obj as KCollider2D;
				if (!(kcollider2D == null) && kcollider2D.Intersects(new Vector2(vector.x, vector.y)))
				{
					T t = kcollider2D.GetComponent<T>();
					if (t == null)
					{
						t = kcollider2D.GetComponentInParent<T>();
					}
					if (!(t == null) && (1 << t.gameObject.layer & layer_mask) != 0 && !(t == null) && (condition == null || condition(t)))
					{
						float num = t.transform.GetPosition().z - vector.z;
						bool flag = false;
						for (int i = 0; i < intersections.Count; i++)
						{
							InterfaceTool.Intersection intersection = intersections[i];
							if (intersection.component.gameObject == t.gameObject)
							{
								intersection.distance = Mathf.Min(intersection.distance, num);
								intersections[i] = intersection;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							intersections.Add(new InterfaceTool.Intersection
							{
								component = t,
								distance = num
							});
						}
					}
				}
			}
			pooledList.Recycle();
		}
	}

	// Token: 0x06004338 RID: 17208 RVA: 0x0017E32C File Offset: 0x0017C52C
	private int SortSelectables(KMonoBehaviour x, KMonoBehaviour y)
	{
		if (x == null && y == null)
		{
			return 0;
		}
		if (x == null)
		{
			return -1;
		}
		if (y == null)
		{
			return 1;
		}
		int num = x.transform.GetPosition().z.CompareTo(y.transform.GetPosition().z);
		if (num != 0)
		{
			return num;
		}
		return x.GetInstanceID().CompareTo(y.GetInstanceID());
	}

	// Token: 0x06004339 RID: 17209 RVA: 0x0017E3A5 File Offset: 0x0017C5A5
	public void SetHoverOverride(KSelectable hover_override)
	{
		this.hoverOverride = hover_override;
	}

	// Token: 0x0600433A RID: 17210 RVA: 0x0017E3B0 File Offset: 0x0017C5B0
	private int SortHoverCards(ScenePartitionerEntry x, ScenePartitionerEntry y)
	{
		KMonoBehaviour x2 = x.obj as KMonoBehaviour;
		KMonoBehaviour y2 = y.obj as KMonoBehaviour;
		return this.SortSelectables(x2, y2);
	}

	// Token: 0x0600433B RID: 17211 RVA: 0x0017E3DD File Offset: 0x0017C5DD
	private static bool is_component_null(InterfaceTool.Intersection intersection)
	{
		return !intersection.component;
	}

	// Token: 0x0600433C RID: 17212 RVA: 0x0017E3ED File Offset: 0x0017C5ED
	protected void ClearHover()
	{
		if (this.hover != null)
		{
			KSelectable kselectable = this.hover;
			this.hover = null;
			kselectable.Unhover();
			Game.Instance.Trigger(-1201923725, null);
		}
	}

	// Token: 0x04002C2F RID: 11311
	private static Dictionary<global::Action, InterfaceToolConfig> interfaceConfigMap = null;

	// Token: 0x04002C30 RID: 11312
	private static List<InterfaceToolConfig> activeConfigs = new List<InterfaceToolConfig>();

	// Token: 0x04002C31 RID: 11313
	public const float MaxClickDistance = 0.02f;

	// Token: 0x04002C32 RID: 11314
	public const float DepthBias = -0.15f;

	// Token: 0x04002C33 RID: 11315
	public GameObject visualizer;

	// Token: 0x04002C34 RID: 11316
	public Grid.SceneLayer visualizerLayer = Grid.SceneLayer.Move;

	// Token: 0x04002C35 RID: 11317
	public string placeSound;

	// Token: 0x04002C36 RID: 11318
	protected bool populateHitsList;

	// Token: 0x04002C37 RID: 11319
	[NonSerialized]
	public bool hasFocus;

	// Token: 0x04002C38 RID: 11320
	[SerializeField]
	protected Texture2D cursor;

	// Token: 0x04002C39 RID: 11321
	public Vector2 cursorOffset = new Vector2(2f, 2f);

	// Token: 0x04002C3A RID: 11322
	public System.Action OnDeactivate;

	// Token: 0x04002C3B RID: 11323
	private static Texture2D activeCursor = null;

	// Token: 0x04002C3C RID: 11324
	private static HashedString toolActivatedViewMode = OverlayModes.None.ID;

	// Token: 0x04002C3D RID: 11325
	protected HashedString viewMode = OverlayModes.None.ID;

	// Token: 0x04002C3E RID: 11326
	private HoverTextConfiguration hoverTextConfiguration;

	// Token: 0x04002C3F RID: 11327
	private KSelectable hoverOverride;

	// Token: 0x04002C40 RID: 11328
	public KSelectable hover;

	// Token: 0x04002C41 RID: 11329
	protected int layerMask;

	// Token: 0x04002C42 RID: 11330
	protected SelectMarker selectMarker;

	// Token: 0x04002C43 RID: 11331
	private List<RaycastResult> castResults = new List<RaycastResult>();

	// Token: 0x04002C44 RID: 11332
	private bool isAppFocused = true;

	// Token: 0x04002C45 RID: 11333
	private List<KSelectable> hits = new List<KSelectable>();

	// Token: 0x04002C46 RID: 11334
	protected bool playedSoundThisFrame;

	// Token: 0x04002C47 RID: 11335
	private List<InterfaceTool.Intersection> intersections = new List<InterfaceTool.Intersection>();

	// Token: 0x04002C48 RID: 11336
	private HashSet<Component> prevIntersectionGroup = new HashSet<Component>();

	// Token: 0x04002C49 RID: 11337
	private HashSet<Component> curIntersectionGroup = new HashSet<Component>();

	// Token: 0x04002C4A RID: 11338
	private int hitCycleCount;

	// Token: 0x02001871 RID: 6257
	public struct Intersection
	{
		// Token: 0x04007649 RID: 30281
		public MonoBehaviour component;

		// Token: 0x0400764A RID: 30282
		public float distance;
	}
}
