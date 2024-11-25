using System;
using UnityEngine;

// Token: 0x02000909 RID: 2313
public class DigTool : DragTool
{
	// Token: 0x060042B7 RID: 17079 RVA: 0x0017B7A3 File Offset: 0x001799A3
	public static void DestroyInstance()
	{
		DigTool.Instance = null;
	}

	// Token: 0x060042B8 RID: 17080 RVA: 0x0017B7AB File Offset: 0x001799AB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DigTool.Instance = this;
	}

	// Token: 0x060042B9 RID: 17081 RVA: 0x0017B7B9 File Offset: 0x001799B9
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		InterfaceTool.ActiveConfig.DigAction.Uproot(cell);
		InterfaceTool.ActiveConfig.DigAction.Dig(cell, distFromOrigin);
	}

	// Token: 0x060042BA RID: 17082 RVA: 0x0017B7DC File Offset: 0x001799DC
	public static GameObject PlaceDig(int cell, int animationDelay = 0)
	{
		if (Grid.Solid[cell] && !Grid.Foundation[cell] && Grid.Objects[cell, 7] == null)
		{
			for (int i = 0; i < 45; i++)
			{
				if (Grid.Objects[cell, i] != null && Grid.Objects[cell, i].GetComponent<Constructable>() != null)
				{
					return null;
				}
			}
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(new Tag("DigPlacer")), null, null);
			gameObject.SetActive(true);
			Grid.Objects[cell, 7] = gameObject;
			Vector3 vector = Grid.CellToPosCBC(cell, DigTool.Instance.visualizerLayer);
			float num = -0.15f;
			vector.z += num;
			gameObject.transform.SetPosition(vector);
			gameObject.GetComponentInChildren<EasingAnimations>().PlayAnimation("ScaleUp", Mathf.Max(0f, (float)animationDelay * 0.02f));
			return gameObject;
		}
		if (Grid.Objects[cell, 7] != null)
		{
			return Grid.Objects[cell, 7];
		}
		return null;
	}

	// Token: 0x060042BB RID: 17083 RVA: 0x0017B900 File Offset: 0x00179B00
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x060042BC RID: 17084 RVA: 0x0017B918 File Offset: 0x00179B18
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x04002C08 RID: 11272
	public static DigTool Instance;
}
