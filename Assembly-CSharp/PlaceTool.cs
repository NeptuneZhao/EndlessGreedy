using System;
using UnityEngine;

// Token: 0x02000914 RID: 2324
public class PlaceTool : DragTool
{
	// Token: 0x06004356 RID: 17238 RVA: 0x0017E939 File Offset: 0x0017CB39
	public static void DestroyInstance()
	{
		PlaceTool.Instance = null;
	}

	// Token: 0x06004357 RID: 17239 RVA: 0x0017E941 File Offset: 0x0017CB41
	protected override void OnPrefabInit()
	{
		PlaceTool.Instance = this;
		this.tooltip = base.GetComponent<ToolTip>();
	}

	// Token: 0x06004358 RID: 17240 RVA: 0x0017E958 File Offset: 0x0017CB58
	protected override void OnActivateTool()
	{
		this.active = true;
		base.OnActivateTool();
		this.visualizer = new GameObject("PlaceToolVisualizer");
		this.visualizer.SetActive(false);
		this.visualizer.SetLayerRecursively(LayerMask.NameToLayer("Place"));
		KBatchedAnimController kbatchedAnimController = this.visualizer.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.Always;
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.SetLayer(LayerMask.NameToLayer("Place"));
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim(this.source.kAnimName)
		};
		kbatchedAnimController.initialAnim = this.source.animName;
		this.visualizer.SetActive(true);
		this.ShowToolTip();
		base.GetComponent<PlaceToolHoverTextCard>().currentPlaceable = this.source;
		ResourceRemainingDisplayScreen.instance.ActivateDisplay(this.visualizer);
		GridCompositor.Instance.ToggleMajor(true);
	}

	// Token: 0x06004359 RID: 17241 RVA: 0x0017EA40 File Offset: 0x0017CC40
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		this.active = false;
		GridCompositor.Instance.ToggleMajor(false);
		this.HideToolTip();
		ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
		UnityEngine.Object.Destroy(this.visualizer);
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound(this.GetDeactivateSound(), false));
		this.source = null;
		this.onPlacedCallback = null;
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x0600435A RID: 17242 RVA: 0x0017EAA0 File Offset: 0x0017CCA0
	public void Activate(Placeable source, Action<Placeable, int> onPlacedCallback)
	{
		this.source = source;
		this.onPlacedCallback = onPlacedCallback;
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x0600435B RID: 17243 RVA: 0x0017EABC File Offset: 0x0017CCBC
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (this.visualizer == null)
		{
			return;
		}
		bool flag = false;
		string text;
		if (this.source.IsValidPlaceLocation(cell, out text))
		{
			this.onPlacedCallback(this.source, cell);
			flag = true;
		}
		if (flag)
		{
			base.DeactivateTool(null);
		}
	}

	// Token: 0x0600435C RID: 17244 RVA: 0x0017EB08 File Offset: 0x0017CD08
	protected override DragTool.Mode GetMode()
	{
		return DragTool.Mode.Brush;
	}

	// Token: 0x0600435D RID: 17245 RVA: 0x0017EB0B File Offset: 0x0017CD0B
	private void ShowToolTip()
	{
		ToolTipScreen.Instance.SetToolTip(this.tooltip);
	}

	// Token: 0x0600435E RID: 17246 RVA: 0x0017EB1D File Offset: 0x0017CD1D
	private void HideToolTip()
	{
		ToolTipScreen.Instance.ClearToolTip(this.tooltip);
	}

	// Token: 0x0600435F RID: 17247 RVA: 0x0017EB30 File Offset: 0x0017CD30
	public override void OnMouseMove(Vector3 cursorPos)
	{
		cursorPos = base.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		int cell = Grid.PosToCell(cursorPos);
		KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
		string text;
		if (this.source.IsValidPlaceLocation(cell, out text))
		{
			component.TintColour = Color.white;
		}
		else
		{
			component.TintColour = Color.red;
		}
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06004360 RID: 17248 RVA: 0x0017EB9C File Offset: 0x0017CD9C
	public void Update()
	{
		if (this.active)
		{
			KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				component.SetLayer(LayerMask.NameToLayer("Place"));
			}
		}
	}

	// Token: 0x06004361 RID: 17249 RVA: 0x0017EBD6 File Offset: 0x0017CDD6
	public override string GetDeactivateSound()
	{
		return "HUD_Click_Deselect";
	}

	// Token: 0x04002C52 RID: 11346
	[SerializeField]
	private TextStyleSetting tooltipStyle;

	// Token: 0x04002C53 RID: 11347
	private Action<Placeable, int> onPlacedCallback;

	// Token: 0x04002C54 RID: 11348
	private Placeable source;

	// Token: 0x04002C55 RID: 11349
	private ToolTip tooltip;

	// Token: 0x04002C56 RID: 11350
	public static PlaceTool Instance;

	// Token: 0x04002C57 RID: 11351
	private bool active;
}
