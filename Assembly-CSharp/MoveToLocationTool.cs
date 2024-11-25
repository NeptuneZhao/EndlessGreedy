using System;
using UnityEngine;

// Token: 0x02000913 RID: 2323
public class MoveToLocationTool : InterfaceTool
{
	// Token: 0x06004349 RID: 17225 RVA: 0x0017E6E2 File Offset: 0x0017C8E2
	public static void DestroyInstance()
	{
		MoveToLocationTool.Instance = null;
	}

	// Token: 0x0600434A RID: 17226 RVA: 0x0017E6EA File Offset: 0x0017C8EA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MoveToLocationTool.Instance = this;
		this.visualizer = Util.KInstantiate(this.visualizer, null, null);
	}

	// Token: 0x0600434B RID: 17227 RVA: 0x0017E70B File Offset: 0x0017C90B
	public void Activate(Navigator navigator)
	{
		this.targetNavigator = navigator;
		this.targetMovable = null;
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x0600434C RID: 17228 RVA: 0x0017E726 File Offset: 0x0017C926
	public void Activate(Movable movable)
	{
		this.targetNavigator = null;
		this.targetMovable = movable;
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x0600434D RID: 17229 RVA: 0x0017E744 File Offset: 0x0017C944
	public bool CanMoveTo(int target_cell)
	{
		if (this.targetNavigator != null)
		{
			return this.targetNavigator.GetSMI<MoveToLocationMonitor.Instance>() != null && this.targetNavigator.CanReach(target_cell);
		}
		return this.targetMovable != null && this.targetMovable.CanMoveTo(target_cell);
	}

	// Token: 0x0600434E RID: 17230 RVA: 0x0017E798 File Offset: 0x0017C998
	private void SetMoveToLocation(int target_cell)
	{
		if (this.targetNavigator != null)
		{
			MoveToLocationMonitor.Instance smi = this.targetNavigator.GetSMI<MoveToLocationMonitor.Instance>();
			if (smi != null)
			{
				smi.MoveToLocation(target_cell);
				return;
			}
		}
		else if (this.targetMovable != null)
		{
			this.targetMovable.MoveToLocation(target_cell);
		}
	}

	// Token: 0x0600434F RID: 17231 RVA: 0x0017E7E4 File Offset: 0x0017C9E4
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		this.visualizer.gameObject.SetActive(true);
	}

	// Token: 0x06004350 RID: 17232 RVA: 0x0017E800 File Offset: 0x0017CA00
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		if (this.targetNavigator != null && new_tool == SelectTool.Instance)
		{
			SelectTool.Instance.SelectNextFrame(this.targetNavigator.GetComponent<KSelectable>(), true);
		}
		this.visualizer.gameObject.SetActive(false);
	}

	// Token: 0x06004351 RID: 17233 RVA: 0x0017E858 File Offset: 0x0017CA58
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		if (this.targetNavigator != null || this.targetMovable != null)
		{
			int mouseCell = DebugHandler.GetMouseCell();
			if (this.CanMoveTo(mouseCell))
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
				this.SetMoveToLocation(mouseCell);
				SelectTool.Instance.Activate();
				return;
			}
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
	}

	// Token: 0x06004352 RID: 17234 RVA: 0x0017E8CC File Offset: 0x0017CACC
	private void RefreshColor()
	{
		Color white = new Color(0.91f, 0.21f, 0.2f);
		if (this.CanMoveTo(DebugHandler.GetMouseCell()))
		{
			white = Color.white;
		}
		this.SetColor(this.visualizer, white);
	}

	// Token: 0x06004353 RID: 17235 RVA: 0x0017E90F File Offset: 0x0017CB0F
	public override void OnMouseMove(Vector3 cursor_pos)
	{
		base.OnMouseMove(cursor_pos);
		this.RefreshColor();
	}

	// Token: 0x06004354 RID: 17236 RVA: 0x0017E91E File Offset: 0x0017CB1E
	private void SetColor(GameObject root, Color c)
	{
		root.GetComponentInChildren<MeshRenderer>().material.color = c;
	}

	// Token: 0x04002C4F RID: 11343
	public static MoveToLocationTool Instance;

	// Token: 0x04002C50 RID: 11344
	private Navigator targetNavigator;

	// Token: 0x04002C51 RID: 11345
	private Movable targetMovable;
}
