using System;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200090C RID: 2316
public class DragTool : InterfaceTool
{
	// Token: 0x170004DF RID: 1247
	// (get) Token: 0x060042D6 RID: 17110 RVA: 0x0017BFA5 File Offset: 0x0017A1A5
	public bool Dragging
	{
		get
		{
			return this.dragging;
		}
	}

	// Token: 0x060042D7 RID: 17111 RVA: 0x0017BFAD File Offset: 0x0017A1AD
	protected virtual DragTool.Mode GetMode()
	{
		return this.mode;
	}

	// Token: 0x060042D8 RID: 17112 RVA: 0x0017BFB5 File Offset: 0x0017A1B5
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		this.dragging = false;
		this.SetMode(this.mode);
	}

	// Token: 0x060042D9 RID: 17113 RVA: 0x0017BFD0 File Offset: 0x0017A1D0
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		if (KScreenManager.Instance != null)
		{
			KScreenManager.Instance.SetEventSystemEnabled(true);
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		this.RemoveCurrentAreaText();
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x060042DA RID: 17114 RVA: 0x0017C008 File Offset: 0x0017A208
	protected override void OnPrefabInit()
	{
		Game.Instance.Subscribe(1634669191, new Action<object>(this.OnTutorialOpened));
		base.OnPrefabInit();
		if (this.visualizer != null)
		{
			this.visualizer = global::Util.KInstantiate(this.visualizer, null, null);
		}
		if (this.areaVisualizer != null)
		{
			this.areaVisualizer = global::Util.KInstantiate(this.areaVisualizer, null, null);
			this.areaVisualizer.SetActive(false);
			this.areaVisualizerSpriteRenderer = this.areaVisualizer.GetComponent<SpriteRenderer>();
			this.areaVisualizer.transform.SetParent(base.transform);
			this.areaVisualizer.GetComponent<Renderer>().material.color = this.areaColour;
		}
	}

	// Token: 0x060042DB RID: 17115 RVA: 0x0017C0CC File Offset: 0x0017A2CC
	protected override void OnCmpEnable()
	{
		this.dragging = false;
	}

	// Token: 0x060042DC RID: 17116 RVA: 0x0017C0D5 File Offset: 0x0017A2D5
	protected override void OnCmpDisable()
	{
		if (this.visualizer != null)
		{
			this.visualizer.SetActive(false);
		}
		if (this.areaVisualizer != null)
		{
			this.areaVisualizer.SetActive(false);
		}
	}

	// Token: 0x060042DD RID: 17117 RVA: 0x0017C10C File Offset: 0x0017A30C
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		cursor_pos = this.ClampPositionToWorld(cursor_pos, ClusterManager.Instance.activeWorld);
		this.dragging = true;
		this.downPos = cursor_pos;
		this.cellChangedSinceDown = false;
		this.previousCursorPos = cursor_pos;
		if (this.currentVirtualInputInUse != null)
		{
			this.currentVirtualInputInUse.mouseMovementOnly = false;
			this.currentVirtualInputInUse = null;
		}
		if (!KInputManager.currentControllerIsGamepad)
		{
			KScreenManager.Instance.SetEventSystemEnabled(false);
		}
		else
		{
			UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
			base.SetCurrentVirtualInputModuleMousMovementMode(true, delegate(VirtualInputModule module)
			{
				this.currentVirtualInputInUse = module;
			});
		}
		this.hasFocus = true;
		this.RemoveCurrentAreaText();
		if (this.areaVisualizerTextPrefab != null)
		{
			this.areaVisualizerText = NameDisplayScreen.Instance.AddAreaText("", this.areaVisualizerTextPrefab);
			NameDisplayScreen.Instance.GetWorldText(this.areaVisualizerText).GetComponent<LocText>().color = this.areaColour;
		}
		DragTool.Mode mode = this.GetMode();
		if (mode == DragTool.Mode.Brush)
		{
			if (this.visualizer != null)
			{
				this.AddDragPoint(cursor_pos);
				return;
			}
		}
		else if (mode == DragTool.Mode.Box || mode == DragTool.Mode.Line)
		{
			if (this.visualizer != null)
			{
				this.visualizer.SetActive(false);
			}
			if (this.areaVisualizer != null)
			{
				this.areaVisualizer.SetActive(true);
				this.areaVisualizer.transform.SetPosition(cursor_pos);
				this.areaVisualizerSpriteRenderer.size = new Vector2(0.01f, 0.01f);
			}
		}
	}

	// Token: 0x060042DE RID: 17118 RVA: 0x0017C279 File Offset: 0x0017A479
	public void RemoveCurrentAreaText()
	{
		if (this.areaVisualizerText != Guid.Empty)
		{
			NameDisplayScreen.Instance.RemoveWorldText(this.areaVisualizerText);
			this.areaVisualizerText = Guid.Empty;
		}
	}

	// Token: 0x060042DF RID: 17119 RVA: 0x0017C2A8 File Offset: 0x0017A4A8
	public void CancelDragging()
	{
		KScreenManager.Instance.SetEventSystemEnabled(true);
		if (this.currentVirtualInputInUse != null)
		{
			this.currentVirtualInputInUse.mouseMovementOnly = false;
			this.currentVirtualInputInUse = null;
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		this.dragAxis = DragTool.DragAxis.Invalid;
		if (!this.dragging)
		{
			return;
		}
		this.dragging = false;
		this.RemoveCurrentAreaText();
		DragTool.Mode mode = this.GetMode();
		if ((mode == DragTool.Mode.Box || mode == DragTool.Mode.Line) && this.areaVisualizer != null)
		{
			this.areaVisualizer.SetActive(false);
		}
	}

	// Token: 0x060042E0 RID: 17120 RVA: 0x0017C338 File Offset: 0x0017A538
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		KScreenManager.Instance.SetEventSystemEnabled(true);
		if (this.currentVirtualInputInUse != null)
		{
			this.currentVirtualInputInUse.mouseMovementOnly = false;
			this.currentVirtualInputInUse = null;
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		this.dragAxis = DragTool.DragAxis.Invalid;
		if (!this.dragging)
		{
			return;
		}
		this.dragging = false;
		cursor_pos = this.ClampPositionToWorld(cursor_pos, ClusterManager.Instance.activeWorld);
		this.RemoveCurrentAreaText();
		DragTool.Mode mode = this.GetMode();
		if (mode == DragTool.Mode.Line || Input.GetKey((KeyCode)Global.GetInputManager().GetDefaultController().GetInputForAction(global::Action.DragStraight)))
		{
			cursor_pos = this.SnapToLine(cursor_pos);
		}
		if ((mode == DragTool.Mode.Box || mode == DragTool.Mode.Line) && this.areaVisualizer != null)
		{
			this.areaVisualizer.SetActive(false);
			int num;
			int num2;
			Grid.PosToXY(this.downPos, out num, out num2);
			int num3 = num;
			int num4 = num2;
			int num5;
			int num6;
			Grid.PosToXY(cursor_pos, out num5, out num6);
			if (num5 < num)
			{
				global::Util.Swap<int>(ref num, ref num5);
			}
			if (num6 < num2)
			{
				global::Util.Swap<int>(ref num2, ref num6);
			}
			for (int i = num2; i <= num6; i++)
			{
				for (int j = num; j <= num5; j++)
				{
					int cell = Grid.XYToCell(j, i);
					if (Grid.IsValidCell(cell) && Grid.IsVisible(cell))
					{
						int num7 = i - num4;
						int num8 = j - num3;
						num7 = Mathf.Abs(num7);
						num8 = Mathf.Abs(num8);
						this.OnDragTool(cell, num7 + num8);
					}
				}
			}
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound(this.GetConfirmSound(), false));
			this.OnDragComplete(this.downPos, cursor_pos);
		}
	}

	// Token: 0x060042E1 RID: 17121 RVA: 0x0017C4C7 File Offset: 0x0017A6C7
	protected virtual string GetConfirmSound()
	{
		return "Tile_Confirm";
	}

	// Token: 0x060042E2 RID: 17122 RVA: 0x0017C4CE File Offset: 0x0017A6CE
	protected virtual string GetDragSound()
	{
		return "Tile_Drag";
	}

	// Token: 0x060042E3 RID: 17123 RVA: 0x0017C4D5 File Offset: 0x0017A6D5
	public override string GetDeactivateSound()
	{
		return "Tile_Cancel";
	}

	// Token: 0x060042E4 RID: 17124 RVA: 0x0017C4DC File Offset: 0x0017A6DC
	protected Vector3 ClampPositionToWorld(Vector3 position, WorldContainer world)
	{
		position.x = Mathf.Clamp(position.x, world.minimumBounds.x, world.maximumBounds.x);
		position.y = Mathf.Clamp(position.y, world.minimumBounds.y, world.maximumBounds.y);
		return position;
	}

	// Token: 0x060042E5 RID: 17125 RVA: 0x0017C53C File Offset: 0x0017A73C
	protected Vector3 SnapToLine(Vector3 cursorPos)
	{
		Vector3 vector = cursorPos - this.downPos;
		if (this.canChangeDragAxis || (!this.canChangeDragAxis && !this.cellChangedSinceDown) || this.dragAxis == DragTool.DragAxis.Invalid)
		{
			this.dragAxis = DragTool.DragAxis.Invalid;
			if (Mathf.Abs(vector.x) < Mathf.Abs(vector.y))
			{
				this.dragAxis = DragTool.DragAxis.Vertical;
			}
			else
			{
				this.dragAxis = DragTool.DragAxis.Horizontal;
			}
		}
		DragTool.DragAxis dragAxis = this.dragAxis;
		if (dragAxis != DragTool.DragAxis.Horizontal)
		{
			if (dragAxis == DragTool.DragAxis.Vertical)
			{
				cursorPos.x = this.downPos.x;
				if (this.lineModeMaxLength != -1 && Mathf.Abs(vector.y) > (float)(this.lineModeMaxLength - 1))
				{
					cursorPos.y = this.downPos.y + Mathf.Sign(vector.y) * (float)(this.lineModeMaxLength - 1);
				}
			}
		}
		else
		{
			cursorPos.y = this.downPos.y;
			if (this.lineModeMaxLength != -1 && Mathf.Abs(vector.x) > (float)(this.lineModeMaxLength - 1))
			{
				cursorPos.x = this.downPos.x + Mathf.Sign(vector.x) * (float)(this.lineModeMaxLength - 1);
			}
		}
		return cursorPos;
	}

	// Token: 0x060042E6 RID: 17126 RVA: 0x0017C678 File Offset: 0x0017A878
	public override void OnMouseMove(Vector3 cursorPos)
	{
		cursorPos = this.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		if (this.dragging && (Input.GetKey((KeyCode)Global.GetInputManager().GetDefaultController().GetInputForAction(global::Action.DragStraight)) || this.GetMode() == DragTool.Mode.Line))
		{
			cursorPos = this.SnapToLine(cursorPos);
		}
		else
		{
			this.dragAxis = DragTool.DragAxis.Invalid;
		}
		base.OnMouseMove(cursorPos);
		if (!this.dragging)
		{
			return;
		}
		if (Grid.PosToCell(cursorPos) != Grid.PosToCell(this.downPos))
		{
			this.cellChangedSinceDown = true;
		}
		DragTool.Mode mode = this.GetMode();
		if (mode != DragTool.Mode.Brush)
		{
			if (mode - DragTool.Mode.Box <= 1)
			{
				Vector2 vector = Vector3.Max(this.downPos, cursorPos);
				Vector2 vector2 = Vector3.Min(this.downPos, cursorPos);
				vector = base.GetWorldRestrictedPosition(vector);
				vector2 = base.GetWorldRestrictedPosition(vector2);
				vector = base.GetRegularizedPos(vector, false);
				vector2 = base.GetRegularizedPos(vector2, true);
				Vector2 vector3 = vector - vector2;
				Vector2 vector4 = (vector + vector2) * 0.5f;
				this.areaVisualizer.transform.SetPosition(new Vector2(vector4.x, vector4.y));
				int num = (int)(vector.x - vector2.x + (vector.y - vector2.y) - 1f);
				if (this.areaVisualizerSpriteRenderer.size != vector3)
				{
					string sound = GlobalAssets.GetSound(this.GetDragSound(), false);
					if (sound != null)
					{
						Vector3 position = this.areaVisualizer.transform.GetPosition();
						position.z = 0f;
						EventInstance instance = SoundEvent.BeginOneShot(sound, position, 1f, false);
						instance.setParameterByName("tileCount", (float)num, false);
						SoundEvent.EndOneShot(instance);
					}
				}
				this.areaVisualizerSpriteRenderer.size = vector3;
				if (this.areaVisualizerText != Guid.Empty)
				{
					Vector2I vector2I = new Vector2I(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y));
					LocText component = NameDisplayScreen.Instance.GetWorldText(this.areaVisualizerText).GetComponent<LocText>();
					component.text = string.Format(UI.TOOLS.TOOL_AREA_FMT, vector2I.x, vector2I.y, vector2I.x * vector2I.y);
					Vector2 v = vector4;
					component.transform.SetPosition(v);
				}
			}
		}
		else
		{
			this.AddDragPoints(cursorPos, this.previousCursorPos);
			if (this.areaVisualizerText != Guid.Empty)
			{
				int dragLength = this.GetDragLength();
				LocText component2 = NameDisplayScreen.Instance.GetWorldText(this.areaVisualizerText).GetComponent<LocText>();
				component2.text = string.Format(UI.TOOLS.TOOL_LENGTH_FMT, dragLength);
				Vector3 vector5 = Grid.CellToPos(Grid.PosToCell(cursorPos));
				vector5 += new Vector3(0f, 1f, 0f);
				component2.transform.SetPosition(vector5);
			}
		}
		this.previousCursorPos = cursorPos;
	}

	// Token: 0x060042E7 RID: 17127 RVA: 0x0017C978 File Offset: 0x0017AB78
	protected virtual void OnDragTool(int cell, int distFromOrigin)
	{
	}

	// Token: 0x060042E8 RID: 17128 RVA: 0x0017C97A File Offset: 0x0017AB7A
	protected virtual void OnDragComplete(Vector3 cursorDown, Vector3 cursorUp)
	{
	}

	// Token: 0x060042E9 RID: 17129 RVA: 0x0017C97C File Offset: 0x0017AB7C
	protected virtual int GetDragLength()
	{
		return 0;
	}

	// Token: 0x060042EA RID: 17130 RVA: 0x0017C980 File Offset: 0x0017AB80
	private void AddDragPoint(Vector3 cursorPos)
	{
		cursorPos = this.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		int cell = Grid.PosToCell(cursorPos);
		if (Grid.IsValidCell(cell) && Grid.IsVisible(cell))
		{
			this.OnDragTool(cell, 0);
		}
	}

	// Token: 0x060042EB RID: 17131 RVA: 0x0017C9C0 File Offset: 0x0017ABC0
	private void AddDragPoints(Vector3 cursorPos, Vector3 previousCursorPos)
	{
		cursorPos = this.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		Vector3 a = cursorPos - previousCursorPos;
		float magnitude = a.magnitude;
		float num = Grid.CellSizeInMeters * 0.25f;
		int num2 = 1 + (int)(magnitude / num);
		a.Normalize();
		for (int i = 0; i < num2; i++)
		{
			Vector3 cursorPos2 = previousCursorPos + a * ((float)i * num);
			this.AddDragPoint(cursorPos2);
		}
	}

	// Token: 0x060042EC RID: 17132 RVA: 0x0017CA35 File Offset: 0x0017AC35
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.interceptNumberKeysForPriority)
		{
			this.HandlePriortyKeysDown(e);
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x060042ED RID: 17133 RVA: 0x0017CA55 File Offset: 0x0017AC55
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.interceptNumberKeysForPriority)
		{
			this.HandlePriorityKeysUp(e);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x060042EE RID: 17134 RVA: 0x0017CA78 File Offset: 0x0017AC78
	private void HandlePriortyKeysDown(KButtonEvent e)
	{
		global::Action action = e.GetAction();
		if (global::Action.Plan1 > action || action > global::Action.Plan10 || !e.TryConsume(action))
		{
			return;
		}
		int num = action - global::Action.Plan1 + 1;
		if (num <= 9)
		{
			ToolMenu.Instance.PriorityScreen.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, num), true);
			return;
		}
		ToolMenu.Instance.PriorityScreen.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.topPriority, 1), true);
	}

	// Token: 0x060042EF RID: 17135 RVA: 0x0017CADC File Offset: 0x0017ACDC
	private void HandlePriorityKeysUp(KButtonEvent e)
	{
		global::Action action = e.GetAction();
		if (global::Action.Plan1 <= action && action <= global::Action.Plan10)
		{
			e.TryConsume(action);
		}
	}

	// Token: 0x060042F0 RID: 17136 RVA: 0x0017CB04 File Offset: 0x0017AD04
	protected void SetMode(DragTool.Mode newMode)
	{
		this.mode = newMode;
		switch (this.mode)
		{
		case DragTool.Mode.Brush:
			if (this.areaVisualizer != null)
			{
				this.areaVisualizer.SetActive(false);
			}
			if (this.visualizer != null)
			{
				this.visualizer.SetActive(true);
			}
			base.SetCursor(this.cursor, this.cursorOffset, CursorMode.Auto);
			return;
		case DragTool.Mode.Box:
			if (this.visualizer != null)
			{
				this.visualizer.SetActive(true);
			}
			this.mode = DragTool.Mode.Box;
			base.SetCursor(this.boxCursor, this.cursorOffset, CursorMode.Auto);
			return;
		case DragTool.Mode.Line:
			if (this.visualizer != null)
			{
				this.visualizer.SetActive(true);
			}
			this.mode = DragTool.Mode.Line;
			base.SetCursor(this.boxCursor, this.cursorOffset, CursorMode.Auto);
			return;
		default:
			return;
		}
	}

	// Token: 0x060042F1 RID: 17137 RVA: 0x0017CBE4 File Offset: 0x0017ADE4
	public override void OnFocus(bool focus)
	{
		DragTool.Mode mode = this.GetMode();
		if (mode == DragTool.Mode.Brush)
		{
			if (this.visualizer != null)
			{
				this.visualizer.SetActive(focus);
			}
			this.hasFocus = focus;
			return;
		}
		if (mode - DragTool.Mode.Box > 1)
		{
			return;
		}
		if (this.visualizer != null && !this.dragging)
		{
			this.visualizer.SetActive(focus);
		}
		this.hasFocus = (focus || this.dragging);
	}

	// Token: 0x060042F2 RID: 17138 RVA: 0x0017CC58 File Offset: 0x0017AE58
	private void OnTutorialOpened(object data)
	{
		this.dragging = false;
	}

	// Token: 0x060042F3 RID: 17139 RVA: 0x0017CC61 File Offset: 0x0017AE61
	public override bool ShowHoverUI()
	{
		return this.dragging || base.ShowHoverUI();
	}

	// Token: 0x04002C11 RID: 11281
	[SerializeField]
	private Texture2D boxCursor;

	// Token: 0x04002C12 RID: 11282
	[SerializeField]
	private GameObject areaVisualizer;

	// Token: 0x04002C13 RID: 11283
	[SerializeField]
	private GameObject areaVisualizerTextPrefab;

	// Token: 0x04002C14 RID: 11284
	[SerializeField]
	private Color32 areaColour = new Color(1f, 1f, 1f, 0.5f);

	// Token: 0x04002C15 RID: 11285
	protected SpriteRenderer areaVisualizerSpriteRenderer;

	// Token: 0x04002C16 RID: 11286
	protected Guid areaVisualizerText;

	// Token: 0x04002C17 RID: 11287
	protected Vector3 placementPivot;

	// Token: 0x04002C18 RID: 11288
	protected bool interceptNumberKeysForPriority;

	// Token: 0x04002C19 RID: 11289
	private bool dragging;

	// Token: 0x04002C1A RID: 11290
	private Vector3 previousCursorPos;

	// Token: 0x04002C1B RID: 11291
	private DragTool.Mode mode = DragTool.Mode.Box;

	// Token: 0x04002C1C RID: 11292
	private DragTool.DragAxis dragAxis = DragTool.DragAxis.Invalid;

	// Token: 0x04002C1D RID: 11293
	protected bool canChangeDragAxis = true;

	// Token: 0x04002C1E RID: 11294
	protected int lineModeMaxLength = -1;

	// Token: 0x04002C1F RID: 11295
	protected Vector3 downPos;

	// Token: 0x04002C20 RID: 11296
	private bool cellChangedSinceDown;

	// Token: 0x04002C21 RID: 11297
	private VirtualInputModule currentVirtualInputInUse;

	// Token: 0x0200186F RID: 6255
	private enum DragAxis
	{
		// Token: 0x04007641 RID: 30273
		Invalid = -1,
		// Token: 0x04007642 RID: 30274
		None,
		// Token: 0x04007643 RID: 30275
		Horizontal,
		// Token: 0x04007644 RID: 30276
		Vertical
	}

	// Token: 0x02001870 RID: 6256
	public enum Mode
	{
		// Token: 0x04007646 RID: 30278
		Brush,
		// Token: 0x04007647 RID: 30279
		Box,
		// Token: 0x04007648 RID: 30280
		Line
	}
}
