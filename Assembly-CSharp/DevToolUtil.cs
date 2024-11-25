using System;
using UnityEngine;

// Token: 0x02000630 RID: 1584
public static class DevToolUtil
{
	// Token: 0x060026F6 RID: 9974 RVA: 0x000DDDCC File Offset: 0x000DBFCC
	public static DevPanel Open(DevTool devTool)
	{
		return DevToolManager.Instance.panels.AddPanelFor(devTool);
	}

	// Token: 0x060026F7 RID: 9975 RVA: 0x000DDDDE File Offset: 0x000DBFDE
	public static DevPanel Open<T>() where T : DevTool, new()
	{
		return DevToolManager.Instance.panels.AddPanelFor<T>();
	}

	// Token: 0x060026F8 RID: 9976 RVA: 0x000DDDEF File Offset: 0x000DBFEF
	public static DevPanel DebugObject<T>(T obj)
	{
		return DevToolUtil.Open(new DevToolObjectViewer<T>(() => obj));
	}

	// Token: 0x060026F9 RID: 9977 RVA: 0x000DDE12 File Offset: 0x000DC012
	public static DevPanel DebugObject<T>(Func<T> get_obj_fn)
	{
		return DevToolUtil.Open(new DevToolObjectViewer<T>(get_obj_fn));
	}

	// Token: 0x060026FA RID: 9978 RVA: 0x000DDE1F File Offset: 0x000DC01F
	public static void Close(DevTool devTool)
	{
		devTool.ClosePanel();
	}

	// Token: 0x060026FB RID: 9979 RVA: 0x000DDE27 File Offset: 0x000DC027
	public static void Close(DevPanel devPanel)
	{
		devPanel.Close();
	}

	// Token: 0x060026FC RID: 9980 RVA: 0x000DDE2F File Offset: 0x000DC02F
	public static string GenerateDevToolName(DevTool devTool)
	{
		return DevToolUtil.GenerateDevToolName(devTool.GetType());
	}

	// Token: 0x060026FD RID: 9981 RVA: 0x000DDE3C File Offset: 0x000DC03C
	public static string GenerateDevToolName(Type devToolType)
	{
		string result;
		if (DevToolManager.Instance != null && DevToolManager.Instance.devToolNameDict.TryGetValue(devToolType, out result))
		{
			return result;
		}
		string text = devToolType.Name;
		if (text.StartsWith("DevTool_"))
		{
			text = text.Substring("DevTool_".Length);
		}
		else if (text.StartsWith("DevTool"))
		{
			text = text.Substring("DevTool".Length);
		}
		return text;
	}

	// Token: 0x060026FE RID: 9982 RVA: 0x000DDEAC File Offset: 0x000DC0AC
	public static bool CanRevealAndFocus(GameObject gameObject)
	{
		int num;
		return DevToolUtil.TryGetCellIndexFor(gameObject, out num);
	}

	// Token: 0x060026FF RID: 9983 RVA: 0x000DDEC4 File Offset: 0x000DC0C4
	public static void RevealAndFocus(GameObject gameObject)
	{
		int cellIndex;
		if (DevToolUtil.TryGetCellIndexFor(gameObject, out cellIndex))
		{
			return;
		}
		DevToolUtil.RevealAndFocusAt(cellIndex);
		if (!gameObject.GetComponent<KSelectable>().IsNullOrDestroyed())
		{
			SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
			return;
		}
		SelectTool.Instance.Select(null, false);
	}

	// Token: 0x06002700 RID: 9984 RVA: 0x000DDF10 File Offset: 0x000DC110
	public static void FocusCameraOnCell(int cellIndex)
	{
		Vector3 position = Grid.CellToPos2D(cellIndex);
		CameraController.Instance.SetPosition(position);
	}

	// Token: 0x06002701 RID: 9985 RVA: 0x000DDF2F File Offset: 0x000DC12F
	public static bool TryGetCellIndexFor(GameObject gameObject, out int cellIndex)
	{
		cellIndex = -1;
		if (gameObject.IsNullOrDestroyed())
		{
			return false;
		}
		if (!gameObject.GetComponent<RectTransform>().IsNullOrDestroyed())
		{
			return false;
		}
		cellIndex = Grid.PosToCell(gameObject);
		return true;
	}

	// Token: 0x06002702 RID: 9986 RVA: 0x000DDF58 File Offset: 0x000DC158
	public static bool TryGetCellIndexForUniqueBuilding(string prefabId, out int index)
	{
		index = -1;
		BuildingComplete[] array = UnityEngine.Object.FindObjectsOfType<BuildingComplete>(true);
		if (array == null)
		{
			return false;
		}
		foreach (BuildingComplete buildingComplete in array)
		{
			if (prefabId == buildingComplete.Def.PrefabID)
			{
				index = buildingComplete.GetCell();
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002703 RID: 9987 RVA: 0x000DDFA8 File Offset: 0x000DC1A8
	public static void RevealAndFocusAt(int cellIndex)
	{
		int num;
		int num2;
		Grid.CellToXY(cellIndex, out num, out num2);
		GridVisibility.Reveal(num + 2, num2 + 2, 10, 10f);
		DevToolUtil.FocusCameraOnCell(cellIndex);
		int cell;
		if (DevToolUtil.TryGetCellIndexForUniqueBuilding("Headquarters", out cell))
		{
			Vector3 a = Grid.CellToPos2D(cellIndex);
			Vector3 b = Grid.CellToPos2D(cell);
			float num3 = 2f / Vector3.Distance(a, b);
			for (float num4 = 0f; num4 < 1f; num4 += num3)
			{
				int num5;
				int num6;
				Grid.PosToXY(Vector3.Lerp(a, b, num4), out num5, out num6);
				GridVisibility.Reveal(num5 + 2, num6 + 2, 4, 4f);
			}
		}
	}

	// Token: 0x0200140E RID: 5134
	public enum TextAlignment
	{
		// Token: 0x040068BC RID: 26812
		Center,
		// Token: 0x040068BD RID: 26813
		Left,
		// Token: 0x040068BE RID: 26814
		Right
	}
}
