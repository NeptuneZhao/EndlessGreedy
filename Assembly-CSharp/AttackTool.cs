using System;
using UnityEngine;

// Token: 0x020008FE RID: 2302
public class AttackTool : DragTool
{
	// Token: 0x06004223 RID: 16931 RVA: 0x001780B8 File Offset: 0x001762B8
	protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
	{
		Vector2 regularizedPos = base.GetRegularizedPos(Vector2.Min(downPos, upPos), true);
		Vector2 regularizedPos2 = base.GetRegularizedPos(Vector2.Max(downPos, upPos), false);
		AttackTool.MarkForAttack(regularizedPos, regularizedPos2, true);
	}

	// Token: 0x06004224 RID: 16932 RVA: 0x00178100 File Offset: 0x00176300
	public static void MarkForAttack(Vector2 min, Vector2 max, bool mark)
	{
		foreach (FactionAlignment factionAlignment in Components.FactionAlignments.Items)
		{
			if (!factionAlignment.IsNullOrDestroyed())
			{
				Vector2 vector = Grid.PosToXY(factionAlignment.transform.GetPosition());
				if (vector.x >= min.x && vector.x < max.x && vector.y >= min.y && vector.y < max.y)
				{
					if (mark)
					{
						if (FactionManager.Instance.GetDisposition(FactionManager.FactionID.Duplicant, factionAlignment.Alignment) != FactionManager.Disposition.Assist)
						{
							factionAlignment.SetPlayerTargeted(true);
							Prioritizable component = factionAlignment.GetComponent<Prioritizable>();
							if (component != null)
							{
								component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
							}
						}
					}
					else
					{
						factionAlignment.gameObject.Trigger(2127324410, null);
					}
				}
			}
		}
	}

	// Token: 0x06004225 RID: 16933 RVA: 0x00178204 File Offset: 0x00176404
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06004226 RID: 16934 RVA: 0x0017821C File Offset: 0x0017641C
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}
}
