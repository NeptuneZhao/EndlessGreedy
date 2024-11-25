using System;
using System.Collections.Generic;
using Klei.Input;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020009FE RID: 2558
[AddComponentMenu("KMonoBehaviour/scripts/PlayerController")]
public class PlayerController : KMonoBehaviour, IInputHandler
{
	// Token: 0x1700052A RID: 1322
	// (get) Token: 0x060049FE RID: 18942 RVA: 0x001A7143 File Offset: 0x001A5343
	public string handlerName
	{
		get
		{
			return "PlayerController";
		}
	}

	// Token: 0x1700052B RID: 1323
	// (get) Token: 0x060049FF RID: 18943 RVA: 0x001A714A File Offset: 0x001A534A
	// (set) Token: 0x06004A00 RID: 18944 RVA: 0x001A7152 File Offset: 0x001A5352
	public KInputHandler inputHandler { get; set; }

	// Token: 0x1700052C RID: 1324
	// (get) Token: 0x06004A01 RID: 18945 RVA: 0x001A715B File Offset: 0x001A535B
	public InterfaceTool ActiveTool
	{
		get
		{
			return this.activeTool;
		}
	}

	// Token: 0x1700052D RID: 1325
	// (get) Token: 0x06004A02 RID: 18946 RVA: 0x001A7163 File Offset: 0x001A5363
	// (set) Token: 0x06004A03 RID: 18947 RVA: 0x001A716A File Offset: 0x001A536A
	public static PlayerController Instance { get; private set; }

	// Token: 0x06004A04 RID: 18948 RVA: 0x001A7172 File Offset: 0x001A5372
	public static void DestroyInstance()
	{
		PlayerController.Instance = null;
	}

	// Token: 0x06004A05 RID: 18949 RVA: 0x001A717C File Offset: 0x001A537C
	protected override void OnPrefabInit()
	{
		PlayerController.Instance = this;
		InterfaceTool.InitializeConfigs(this.defaultConfigKey, this.interfaceConfigs);
		this.vim = UnityEngine.Object.FindObjectOfType<VirtualInputModule>(true);
		for (int i = 0; i < this.tools.Length; i++)
		{
			if (DlcManager.IsDlcListValidForCurrentContent(this.tools[i].DlcIDs))
			{
				GameObject gameObject = Util.KInstantiate(this.tools[i].gameObject, base.gameObject, null);
				this.tools[i] = gameObject.GetComponent<InterfaceTool>();
				this.tools[i].gameObject.SetActive(true);
				this.tools[i].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06004A06 RID: 18950 RVA: 0x001A7221 File Offset: 0x001A5421
	protected override void OnSpawn()
	{
		if (this.tools.Length == 0)
		{
			return;
		}
		this.ActivateTool(this.tools[0]);
	}

	// Token: 0x06004A07 RID: 18951 RVA: 0x001A723B File Offset: 0x001A543B
	private void InitializeConfigs()
	{
	}

	// Token: 0x06004A08 RID: 18952 RVA: 0x001A723D File Offset: 0x001A543D
	private Vector3 GetCursorPos()
	{
		return PlayerController.GetCursorPos(KInputManager.GetMousePos());
	}

	// Token: 0x06004A09 RID: 18953 RVA: 0x001A724C File Offset: 0x001A544C
	public static Vector3 GetCursorPos(Vector3 mouse_pos)
	{
		RaycastHit raycastHit;
		Vector3 vector;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(mouse_pos), out raycastHit, float.PositiveInfinity, Game.BlockSelectionLayerMask))
		{
			vector = raycastHit.point;
		}
		else
		{
			mouse_pos.z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters;
			vector = Camera.main.ScreenToWorldPoint(mouse_pos);
		}
		float num = vector.x;
		float num2 = vector.y;
		num = Mathf.Max(num, 0f);
		num = Mathf.Min(num, Grid.WidthInMeters);
		num2 = Mathf.Max(num2, 0f);
		num2 = Mathf.Min(num2, Grid.HeightInMeters);
		vector.x = num;
		vector.y = num2;
		return vector;
	}

	// Token: 0x06004A0A RID: 18954 RVA: 0x001A7300 File Offset: 0x001A5500
	private void UpdateHover()
	{
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current != null)
		{
			this.activeTool.OnFocus(!current.IsPointerOverGameObject());
		}
	}

	// Token: 0x06004A0B RID: 18955 RVA: 0x001A7330 File Offset: 0x001A5530
	private void Update()
	{
		this.UpdateDrag();
		if (this.activeTool && this.activeTool.enabled)
		{
			this.UpdateHover();
			Vector3 cursorPos = this.GetCursorPos();
			if (cursorPos != this.prevMousePos)
			{
				this.prevMousePos = cursorPos;
				this.activeTool.OnMouseMove(cursorPos);
			}
		}
		if (Input.GetKeyDown(KeyCode.F12) && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
		{
			this.DebugHidingCursor = !this.DebugHidingCursor;
			Cursor.visible = !this.DebugHidingCursor;
			HoverTextScreen.Instance.Show(!this.DebugHidingCursor);
		}
	}

	// Token: 0x06004A0C RID: 18956 RVA: 0x001A73DF File Offset: 0x001A55DF
	private void OnCleanup()
	{
		Global.GetInputManager().usedMenus.Remove(this);
	}

	// Token: 0x06004A0D RID: 18957 RVA: 0x001A73F2 File Offset: 0x001A55F2
	private void LateUpdate()
	{
		if (this.queueStopDrag)
		{
			this.queueStopDrag = false;
			this.dragging = false;
			this.dragAction = global::Action.Invalid;
			this.dragDelta = Vector3.zero;
			this.worldDragDelta = Vector3.zero;
		}
	}

	// Token: 0x06004A0E RID: 18958 RVA: 0x001A7428 File Offset: 0x001A5628
	public void ActivateTool(InterfaceTool tool)
	{
		if (this.activeTool == tool)
		{
			return;
		}
		this.DeactivateTool(tool);
		this.activeTool = tool;
		this.activeTool.enabled = true;
		this.activeTool.gameObject.SetActive(true);
		this.activeTool.ActivateTool();
		this.UpdateHover();
	}

	// Token: 0x06004A0F RID: 18959 RVA: 0x001A7480 File Offset: 0x001A5680
	public void ToolDeactivated(InterfaceTool tool)
	{
		if (this.activeTool == tool && this.activeTool != null)
		{
			this.DeactivateTool(null);
		}
		if (this.activeTool == null)
		{
			this.ActivateTool(SelectTool.Instance);
		}
	}

	// Token: 0x06004A10 RID: 18960 RVA: 0x001A74BE File Offset: 0x001A56BE
	private void DeactivateTool(InterfaceTool new_tool = null)
	{
		if (this.activeTool != null)
		{
			this.activeTool.enabled = false;
			this.activeTool.gameObject.SetActive(false);
			InterfaceTool interfaceTool = this.activeTool;
			this.activeTool = null;
			interfaceTool.DeactivateTool(new_tool);
		}
	}

	// Token: 0x06004A11 RID: 18961 RVA: 0x001A74FE File Offset: 0x001A56FE
	public bool IsUsingDefaultTool()
	{
		return this.tools.Length != 0 && this.activeTool == this.tools[0];
	}

	// Token: 0x06004A12 RID: 18962 RVA: 0x001A751E File Offset: 0x001A571E
	private void StartDrag(global::Action action)
	{
		if (this.dragAction == global::Action.Invalid)
		{
			this.dragAction = action;
			this.startDragPos = KInputManager.GetMousePos();
			this.startDragTime = Time.unscaledTime;
		}
	}

	// Token: 0x06004A13 RID: 18963 RVA: 0x001A7548 File Offset: 0x001A5748
	private void UpdateDrag()
	{
		this.dragDelta = Vector2.zero;
		Vector3 mousePos = KInputManager.GetMousePos();
		if (!this.dragging && this.CanDrag() && ((mousePos - this.startDragPos).sqrMagnitude > 36f || Time.unscaledTime - this.startDragTime > 0.3f))
		{
			this.dragging = true;
		}
		if (DistributionPlatform.Initialized && KInputManager.currentControllerIsGamepad && this.dragging)
		{
			return;
		}
		if (this.dragging)
		{
			this.dragDelta = mousePos - this.startDragPos;
			this.worldDragDelta = Camera.main.ScreenToWorldPoint(mousePos) - Camera.main.ScreenToWorldPoint(this.startDragPos);
			this.startDragPos = mousePos;
		}
	}

	// Token: 0x06004A14 RID: 18964 RVA: 0x001A760E File Offset: 0x001A580E
	private void StopDrag(global::Action action)
	{
		if (this.dragAction == action)
		{
			this.queueStopDrag = true;
			if (KInputManager.currentControllerIsGamepad)
			{
				this.dragging = false;
			}
		}
	}

	// Token: 0x06004A15 RID: 18965 RVA: 0x001A7630 File Offset: 0x001A5830
	public void CancelDragging()
	{
		this.queueStopDrag = true;
		if (this.activeTool != null)
		{
			DragTool dragTool = this.activeTool as DragTool;
			if (dragTool != null)
			{
				dragTool.CancelDragging();
			}
		}
	}

	// Token: 0x06004A16 RID: 18966 RVA: 0x001A766D File Offset: 0x001A586D
	public void OnCancelInput()
	{
		this.CancelDragging();
	}

	// Token: 0x06004A17 RID: 18967 RVA: 0x001A7678 File Offset: 0x001A5878
	public void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.ToggleScreenshotMode))
		{
			DebugHandler.ToggleScreenshotMode();
			return;
		}
		if (DebugHandler.HideUI && e.TryConsume(global::Action.Escape))
		{
			DebugHandler.ToggleScreenshotMode();
			return;
		}
		bool flag = true;
		if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
		{
			this.StartDrag(global::Action.MouseLeft);
		}
		else if (e.IsAction(global::Action.MouseRight))
		{
			this.StartDrag(global::Action.MouseRight);
		}
		else if (e.IsAction(global::Action.MouseMiddle))
		{
			this.StartDrag(global::Action.MouseMiddle);
		}
		else
		{
			flag = false;
		}
		if (this.activeTool == null || !this.activeTool.enabled)
		{
			return;
		}
		List<RaycastResult> list = new List<RaycastResult>();
		PointerEventData pointerEventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
		pointerEventData.position = KInputManager.GetMousePos();
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current != null)
		{
			current.RaycastAll(pointerEventData, list);
			if (list.Count > 0)
			{
				return;
			}
		}
		if (flag && !this.draggingAllowed)
		{
			e.TryConsume(e.GetAction());
			return;
		}
		if (e.TryConsume(global::Action.MouseLeft) || e.TryConsume(global::Action.ShiftMouseLeft))
		{
			this.activeTool.OnLeftClickDown(this.GetCursorPos());
			return;
		}
		if (e.IsAction(global::Action.MouseRight))
		{
			this.activeTool.OnRightClickDown(this.GetCursorPos(), e);
			return;
		}
		this.activeTool.OnKeyDown(e);
	}

	// Token: 0x06004A18 RID: 18968 RVA: 0x001A77B4 File Offset: 0x001A59B4
	public void OnKeyUp(KButtonEvent e)
	{
		bool flag = true;
		if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
		{
			this.StopDrag(global::Action.MouseLeft);
		}
		else if (e.IsAction(global::Action.MouseRight))
		{
			this.StopDrag(global::Action.MouseRight);
		}
		else if (e.IsAction(global::Action.MouseMiddle))
		{
			this.StopDrag(global::Action.MouseMiddle);
		}
		else
		{
			flag = false;
		}
		if (this.activeTool == null || !this.activeTool.enabled)
		{
			return;
		}
		if (!this.activeTool.hasFocus)
		{
			return;
		}
		if (flag && !this.draggingAllowed)
		{
			e.TryConsume(e.GetAction());
			return;
		}
		if (!KInputManager.currentControllerIsGamepad)
		{
			if (e.TryConsume(global::Action.MouseLeft) || e.TryConsume(global::Action.ShiftMouseLeft))
			{
				this.activeTool.OnLeftClickUp(this.GetCursorPos());
				return;
			}
			if (e.IsAction(global::Action.MouseRight))
			{
				this.activeTool.OnRightClickUp(this.GetCursorPos());
				return;
			}
			this.activeTool.OnKeyUp(e);
			return;
		}
		else
		{
			if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
			{
				this.activeTool.OnLeftClickUp(this.GetCursorPos());
				return;
			}
			if (e.IsAction(global::Action.MouseRight))
			{
				this.activeTool.OnRightClickUp(this.GetCursorPos());
				return;
			}
			this.activeTool.OnKeyUp(e);
			return;
		}
	}

	// Token: 0x06004A19 RID: 18969 RVA: 0x001A78E5 File Offset: 0x001A5AE5
	public bool ConsumeIfNotDragging(KButtonEvent e, global::Action action)
	{
		return (this.dragAction != action || !this.dragging) && e.TryConsume(action);
	}

	// Token: 0x06004A1A RID: 18970 RVA: 0x001A7901 File Offset: 0x001A5B01
	public bool IsDragging()
	{
		return this.dragging && this.CanDrag();
	}

	// Token: 0x06004A1B RID: 18971 RVA: 0x001A7913 File Offset: 0x001A5B13
	public bool CanDrag()
	{
		return this.draggingAllowed && this.dragAction > global::Action.Invalid;
	}

	// Token: 0x06004A1C RID: 18972 RVA: 0x001A7928 File Offset: 0x001A5B28
	public void AllowDragging(bool allow)
	{
		this.draggingAllowed = allow;
	}

	// Token: 0x06004A1D RID: 18973 RVA: 0x001A7931 File Offset: 0x001A5B31
	public Vector3 GetDragDelta()
	{
		return this.dragDelta;
	}

	// Token: 0x06004A1E RID: 18974 RVA: 0x001A7939 File Offset: 0x001A5B39
	public Vector3 GetWorldDragDelta()
	{
		if (!this.draggingAllowed)
		{
			return Vector3.zero;
		}
		return this.worldDragDelta;
	}

	// Token: 0x0400308E RID: 12430
	[SerializeField]
	private global::Action defaultConfigKey;

	// Token: 0x0400308F RID: 12431
	[SerializeField]
	private List<InterfaceToolConfig> interfaceConfigs;

	// Token: 0x04003091 RID: 12433
	public InterfaceTool[] tools;

	// Token: 0x04003092 RID: 12434
	private InterfaceTool activeTool;

	// Token: 0x04003093 RID: 12435
	public VirtualInputModule vim;

	// Token: 0x04003095 RID: 12437
	private bool DebugHidingCursor;

	// Token: 0x04003096 RID: 12438
	private Vector3 prevMousePos = new Vector3(float.PositiveInfinity, 0f, 0f);

	// Token: 0x04003097 RID: 12439
	private const float MIN_DRAG_DIST_SQR = 36f;

	// Token: 0x04003098 RID: 12440
	private const float MIN_DRAG_TIME = 0.3f;

	// Token: 0x04003099 RID: 12441
	private global::Action dragAction;

	// Token: 0x0400309A RID: 12442
	private bool draggingAllowed = true;

	// Token: 0x0400309B RID: 12443
	private bool dragging;

	// Token: 0x0400309C RID: 12444
	private bool queueStopDrag;

	// Token: 0x0400309D RID: 12445
	private Vector3 startDragPos;

	// Token: 0x0400309E RID: 12446
	private float startDragTime;

	// Token: 0x0400309F RID: 12447
	private Vector3 dragDelta;

	// Token: 0x040030A0 RID: 12448
	private Vector3 worldDragDelta;
}
