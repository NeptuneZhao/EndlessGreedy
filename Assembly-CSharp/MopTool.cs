using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000912 RID: 2322
public class MopTool : DragTool
{
	// Token: 0x06004341 RID: 17217 RVA: 0x0017E4E3 File Offset: 0x0017C6E3
	public static void DestroyInstance()
	{
		MopTool.Instance = null;
	}

	// Token: 0x06004342 RID: 17218 RVA: 0x0017E4EB File Offset: 0x0017C6EB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Placer = Assets.GetPrefab(new Tag("MopPlacer"));
		this.interceptNumberKeysForPriority = true;
		MopTool.Instance = this;
	}

	// Token: 0x06004343 RID: 17219 RVA: 0x0017E515 File Offset: 0x0017C715
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06004344 RID: 17220 RVA: 0x0017E524 File Offset: 0x0017C724
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (Grid.IsValidCell(cell))
		{
			if (DebugHandler.InstantBuildMode)
			{
				if (Grid.IsValidCell(cell))
				{
					Moppable.MopCell(cell, 1000000f, null);
					return;
				}
			}
			else
			{
				GameObject gameObject = Grid.Objects[cell, 8];
				if (!Grid.Solid[cell] && gameObject == null && Grid.Element[cell].IsLiquid)
				{
					bool flag = Grid.IsValidCell(Grid.CellBelow(cell)) && Grid.Solid[Grid.CellBelow(cell)];
					bool flag2 = Grid.Mass[cell] <= MopTool.maxMopAmt;
					if (flag && flag2)
					{
						gameObject = Util.KInstantiate(this.Placer, null, null);
						Grid.Objects[cell, 8] = gameObject;
						Vector3 vector = Grid.CellToPosCBC(cell, this.visualizerLayer);
						float num = -0.15f;
						vector.z += num;
						gameObject.transform.SetPosition(vector);
						gameObject.SetActive(true);
					}
					else
					{
						string text = UI.TOOLS.MOP.TOO_MUCH_LIQUID;
						if (!flag)
						{
							text = UI.TOOLS.MOP.NOT_ON_FLOOR;
						}
						PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, text, null, Grid.CellToPosCBC(cell, this.visualizerLayer), 1.5f, false, false);
					}
				}
				if (gameObject != null)
				{
					Prioritizable component = gameObject.GetComponent<Prioritizable>();
					if (component != null)
					{
						component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
					}
				}
			}
		}
	}

	// Token: 0x06004345 RID: 17221 RVA: 0x0017E69D File Offset: 0x0017C89D
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06004346 RID: 17222 RVA: 0x0017E6B5 File Offset: 0x0017C8B5
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x04002C4B RID: 11339
	private GameObject Placer;

	// Token: 0x04002C4C RID: 11340
	public static MopTool Instance;

	// Token: 0x04002C4D RID: 11341
	private SimHashes Element;

	// Token: 0x04002C4E RID: 11342
	public static float maxMopAmt = 150f;
}
