using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000900 RID: 2304
public class BrushTool : InterfaceTool
{
	// Token: 0x170004DC RID: 1244
	// (get) Token: 0x0600423E RID: 16958 RVA: 0x00179279 File Offset: 0x00177479
	public bool Dragging
	{
		get
		{
			return this.dragging;
		}
	}

	// Token: 0x0600423F RID: 16959 RVA: 0x00179281 File Offset: 0x00177481
	protected virtual void PlaySound()
	{
	}

	// Token: 0x06004240 RID: 16960 RVA: 0x00179283 File Offset: 0x00177483
	protected virtual void clearVisitedCells()
	{
		this.visitedCells.Clear();
	}

	// Token: 0x06004241 RID: 16961 RVA: 0x00179290 File Offset: 0x00177490
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		this.dragging = false;
	}

	// Token: 0x06004242 RID: 16962 RVA: 0x001792A0 File Offset: 0x001774A0
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06004243 RID: 16963 RVA: 0x00179308 File Offset: 0x00177508
	public virtual void SetBrushSize(int radius)
	{
		if (radius == this.brushRadius)
		{
			return;
		}
		this.brushRadius = radius;
		this.brushOffsets.Clear();
		for (int i = 0; i < this.brushRadius * 2; i++)
		{
			for (int j = 0; j < this.brushRadius * 2; j++)
			{
				if (Vector2.Distance(new Vector2((float)i, (float)j), new Vector2((float)this.brushRadius, (float)this.brushRadius)) < (float)this.brushRadius - 0.8f)
				{
					this.brushOffsets.Add(new Vector2((float)(i - this.brushRadius), (float)(j - this.brushRadius)));
				}
			}
		}
	}

	// Token: 0x06004244 RID: 16964 RVA: 0x001793A9 File Offset: 0x001775A9
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		KScreenManager.Instance.SetEventSystemEnabled(true);
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x06004245 RID: 16965 RVA: 0x001793CC File Offset: 0x001775CC
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
			this.areaVisualizer.GetComponent<RectTransform>().SetParent(base.transform);
			this.areaVisualizer.GetComponent<Renderer>().material.color = this.areaColour;
		}
	}

	// Token: 0x06004246 RID: 16966 RVA: 0x0017947F File Offset: 0x0017767F
	protected override void OnCmpEnable()
	{
		this.dragging = false;
	}

	// Token: 0x06004247 RID: 16967 RVA: 0x00179488 File Offset: 0x00177688
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

	// Token: 0x06004248 RID: 16968 RVA: 0x001794BE File Offset: 0x001776BE
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		cursor_pos -= this.placementPivot;
		this.dragging = true;
		this.downPos = cursor_pos;
		if (!KInputManager.currentControllerIsGamepad)
		{
			KScreenManager.Instance.SetEventSystemEnabled(false);
		}
		else
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(true, null);
		}
		this.Paint();
	}

	// Token: 0x06004249 RID: 16969 RVA: 0x00179500 File Offset: 0x00177700
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		cursor_pos -= this.placementPivot;
		KScreenManager.Instance.SetEventSystemEnabled(true);
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		if (!this.dragging)
		{
			return;
		}
		this.dragging = false;
		BrushTool.DragAxis dragAxis = this.dragAxis;
		if (dragAxis == BrushTool.DragAxis.Horizontal)
		{
			cursor_pos.y = this.downPos.y;
			this.dragAxis = BrushTool.DragAxis.None;
			return;
		}
		if (dragAxis != BrushTool.DragAxis.Vertical)
		{
			return;
		}
		cursor_pos.x = this.downPos.x;
		this.dragAxis = BrushTool.DragAxis.None;
	}

	// Token: 0x0600424A RID: 16970 RVA: 0x00179588 File Offset: 0x00177788
	protected virtual string GetConfirmSound()
	{
		return "Tile_Confirm";
	}

	// Token: 0x0600424B RID: 16971 RVA: 0x0017958F File Offset: 0x0017778F
	protected virtual string GetDragSound()
	{
		return "Tile_Drag";
	}

	// Token: 0x0600424C RID: 16972 RVA: 0x00179596 File Offset: 0x00177796
	public override string GetDeactivateSound()
	{
		return "Tile_Cancel";
	}

	// Token: 0x0600424D RID: 16973 RVA: 0x001795A0 File Offset: 0x001777A0
	private static int GetGridDistance(int cell, int center_cell)
	{
		Vector2I u = Grid.CellToXY(cell);
		Vector2I v = Grid.CellToXY(center_cell);
		Vector2I vector2I = u - v;
		return Math.Abs(vector2I.x) + Math.Abs(vector2I.y);
	}

	// Token: 0x0600424E RID: 16974 RVA: 0x001795D8 File Offset: 0x001777D8
	private void Paint()
	{
		int count = this.visitedCells.Count;
		foreach (int num in this.cellsInRadius)
		{
			if (Grid.IsValidCell(num) && (int)Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId && (!Grid.Foundation[num] || this.affectFoundation))
			{
				this.OnPaintCell(num, Grid.GetCellDistance(this.currentCell, num));
			}
		}
		if (this.lastCell != this.currentCell)
		{
			this.PlayDragSound();
		}
		if (count < this.visitedCells.Count)
		{
			this.PlaySound();
		}
	}

	// Token: 0x0600424F RID: 16975 RVA: 0x0017969C File Offset: 0x0017789C
	protected virtual void PlayDragSound()
	{
		string dragSound = this.GetDragSound();
		if (!string.IsNullOrEmpty(dragSound))
		{
			string sound = GlobalAssets.GetSound(dragSound, false);
			if (sound != null)
			{
				Vector3 pos = Grid.CellToPos(this.currentCell);
				pos.z = 0f;
				int cellDistance = Grid.GetCellDistance(Grid.PosToCell(this.downPos), this.currentCell);
				EventInstance instance = SoundEvent.BeginOneShot(sound, pos, 1f, false);
				instance.setParameterByName("tileCount", (float)cellDistance, false);
				SoundEvent.EndOneShot(instance);
			}
		}
	}

	// Token: 0x06004250 RID: 16976 RVA: 0x0017971C File Offset: 0x0017791C
	public override void OnMouseMove(Vector3 cursorPos)
	{
		int num = Grid.PosToCell(cursorPos);
		this.currentCell = num;
		base.OnMouseMove(cursorPos);
		this.cellsInRadius.Clear();
		foreach (Vector2 vector in this.brushOffsets)
		{
			int num2 = Grid.OffsetCell(Grid.PosToCell(cursorPos), new CellOffset((int)vector.x, (int)vector.y));
			if (Grid.IsValidCell(num2) && (int)Grid.WorldIdx[num2] == ClusterManager.Instance.activeWorldId)
			{
				this.cellsInRadius.Add(Grid.OffsetCell(Grid.PosToCell(cursorPos), new CellOffset((int)vector.x, (int)vector.y)));
			}
		}
		if (!this.dragging)
		{
			return;
		}
		this.Paint();
		this.lastCell = this.currentCell;
	}

	// Token: 0x06004251 RID: 16977 RVA: 0x0017980C File Offset: 0x00177A0C
	protected virtual void OnPaintCell(int cell, int distFromOrigin)
	{
		if (!this.visitedCells.Contains(cell))
		{
			this.visitedCells.Add(cell);
		}
	}

	// Token: 0x06004252 RID: 16978 RVA: 0x00179828 File Offset: 0x00177A28
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.DragStraight))
		{
			this.dragAxis = BrushTool.DragAxis.None;
		}
		else if (this.interceptNumberKeysForPriority)
		{
			this.HandlePriortyKeysDown(e);
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06004253 RID: 16979 RVA: 0x0017985B File Offset: 0x00177A5B
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.DragStraight))
		{
			this.dragAxis = BrushTool.DragAxis.Invalid;
		}
		else if (this.interceptNumberKeysForPriority)
		{
			this.HandlePriorityKeysUp(e);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06004254 RID: 16980 RVA: 0x00179890 File Offset: 0x00177A90
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

	// Token: 0x06004255 RID: 16981 RVA: 0x001798F4 File Offset: 0x00177AF4
	private void HandlePriorityKeysUp(KButtonEvent e)
	{
		global::Action action = e.GetAction();
		if (global::Action.Plan1 <= action && action <= global::Action.Plan10)
		{
			e.TryConsume(action);
		}
	}

	// Token: 0x06004256 RID: 16982 RVA: 0x0017991A File Offset: 0x00177B1A
	public override void OnFocus(bool focus)
	{
		if (this.visualizer != null)
		{
			this.visualizer.SetActive(focus);
		}
		this.hasFocus = focus;
		base.OnFocus(focus);
	}

	// Token: 0x06004257 RID: 16983 RVA: 0x00179944 File Offset: 0x00177B44
	private void OnTutorialOpened(object data)
	{
		this.dragging = false;
	}

	// Token: 0x06004258 RID: 16984 RVA: 0x0017994D File Offset: 0x00177B4D
	public override bool ShowHoverUI()
	{
		return this.dragging || base.ShowHoverUI();
	}

	// Token: 0x06004259 RID: 16985 RVA: 0x0017995F File Offset: 0x00177B5F
	public override void LateUpdate()
	{
		base.LateUpdate();
	}

	// Token: 0x04002BDF RID: 11231
	[SerializeField]
	private Texture2D brushCursor;

	// Token: 0x04002BE0 RID: 11232
	[SerializeField]
	private GameObject areaVisualizer;

	// Token: 0x04002BE1 RID: 11233
	[SerializeField]
	private Color32 areaColour = new Color(1f, 1f, 1f, 0.5f);

	// Token: 0x04002BE2 RID: 11234
	protected Color radiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);

	// Token: 0x04002BE3 RID: 11235
	protected Vector3 placementPivot;

	// Token: 0x04002BE4 RID: 11236
	protected bool interceptNumberKeysForPriority;

	// Token: 0x04002BE5 RID: 11237
	protected List<Vector2> brushOffsets = new List<Vector2>();

	// Token: 0x04002BE6 RID: 11238
	protected bool affectFoundation;

	// Token: 0x04002BE7 RID: 11239
	private bool dragging;

	// Token: 0x04002BE8 RID: 11240
	protected int brushRadius = -1;

	// Token: 0x04002BE9 RID: 11241
	private BrushTool.DragAxis dragAxis = BrushTool.DragAxis.Invalid;

	// Token: 0x04002BEA RID: 11242
	protected Vector3 downPos;

	// Token: 0x04002BEB RID: 11243
	protected int currentCell;

	// Token: 0x04002BEC RID: 11244
	protected int lastCell;

	// Token: 0x04002BED RID: 11245
	protected List<int> visitedCells = new List<int>();

	// Token: 0x04002BEE RID: 11246
	protected HashSet<int> cellsInRadius = new HashSet<int>();

	// Token: 0x02001869 RID: 6249
	private enum DragAxis
	{
		// Token: 0x04007620 RID: 30240
		Invalid = -1,
		// Token: 0x04007621 RID: 30241
		None,
		// Token: 0x04007622 RID: 30242
		Horizontal,
		// Token: 0x04007623 RID: 30243
		Vertical
	}
}
