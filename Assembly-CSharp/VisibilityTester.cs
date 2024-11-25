using System;
using UnityEngine;

// Token: 0x02000B55 RID: 2901
[AddComponentMenu("KMonoBehaviour/scripts/VisibilityTester")]
public class VisibilityTester : KMonoBehaviour
{
	// Token: 0x060056B5 RID: 22197 RVA: 0x001EFE0B File Offset: 0x001EE00B
	public static void DestroyInstance()
	{
		VisibilityTester.Instance = null;
	}

	// Token: 0x060056B6 RID: 22198 RVA: 0x001EFE13 File Offset: 0x001EE013
	protected override void OnPrefabInit()
	{
		VisibilityTester.Instance = this;
	}

	// Token: 0x060056B7 RID: 22199 RVA: 0x001EFE1C File Offset: 0x001EE01C
	private void Update()
	{
		if (SelectTool.Instance == null || SelectTool.Instance.selected == null || !this.enableTesting)
		{
			return;
		}
		int cell = Grid.PosToCell(SelectTool.Instance.selected);
		int mouseCell = DebugHandler.GetMouseCell();
		string text = "";
		text = text + "Source Cell: " + cell.ToString() + "\n";
		text = text + "Target Cell: " + mouseCell.ToString() + "\n";
		text = text + "Visible: " + Grid.VisibilityTest(cell, mouseCell, false).ToString();
		for (int i = 0; i < 10000; i++)
		{
			Grid.VisibilityTest(cell, mouseCell, false);
		}
		DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
	}

	// Token: 0x040038C4 RID: 14532
	public static VisibilityTester Instance;

	// Token: 0x040038C5 RID: 14533
	public bool enableTesting;
}
