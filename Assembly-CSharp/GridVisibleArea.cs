using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008E2 RID: 2274
public class GridVisibleArea
{
	// Token: 0x170004BD RID: 1213
	// (get) Token: 0x06004127 RID: 16679 RVA: 0x001726A8 File Offset: 0x001708A8
	public GridArea CurrentArea
	{
		get
		{
			return this.VisibleAreas[0];
		}
	}

	// Token: 0x170004BE RID: 1214
	// (get) Token: 0x06004128 RID: 16680 RVA: 0x001726B6 File Offset: 0x001708B6
	public GridArea PreviousArea
	{
		get
		{
			return this.VisibleAreas[1];
		}
	}

	// Token: 0x170004BF RID: 1215
	// (get) Token: 0x06004129 RID: 16681 RVA: 0x001726C4 File Offset: 0x001708C4
	public GridArea PreviousPreviousArea
	{
		get
		{
			return this.VisibleAreas[2];
		}
	}

	// Token: 0x170004C0 RID: 1216
	// (get) Token: 0x0600412A RID: 16682 RVA: 0x001726D2 File Offset: 0x001708D2
	public GridArea CurrentAreaExtended
	{
		get
		{
			return this.VisibleAreasExtended[0];
		}
	}

	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x0600412B RID: 16683 RVA: 0x001726E0 File Offset: 0x001708E0
	public GridArea PreviousAreaExtended
	{
		get
		{
			return this.VisibleAreasExtended[1];
		}
	}

	// Token: 0x170004C2 RID: 1218
	// (get) Token: 0x0600412C RID: 16684 RVA: 0x001726EE File Offset: 0x001708EE
	public GridArea PreviousPreviousAreaExtended
	{
		get
		{
			return this.VisibleAreasExtended[2];
		}
	}

	// Token: 0x0600412D RID: 16685 RVA: 0x001726FC File Offset: 0x001708FC
	public GridVisibleArea()
	{
	}

	// Token: 0x0600412E RID: 16686 RVA: 0x00172727 File Offset: 0x00170927
	public GridVisibleArea(int padding)
	{
		this._padding = padding;
	}

	// Token: 0x0600412F RID: 16687 RVA: 0x0017275C File Offset: 0x0017095C
	public void Update()
	{
		if (!this.debugFreezeVisibleArea)
		{
			this.VisibleAreas[2] = this.VisibleAreas[1];
			this.VisibleAreas[1] = this.VisibleAreas[0];
			this.VisibleAreas[0] = GridVisibleArea.GetVisibleArea();
		}
		if (!this.debugFreezeVisibleAreasExtended)
		{
			this.VisibleAreasExtended[2] = this.VisibleAreasExtended[1];
			this.VisibleAreasExtended[1] = this.VisibleAreasExtended[0];
			this.VisibleAreasExtended[0] = GridVisibleArea.GetVisibleAreaExtended(this._padding);
		}
		foreach (GridVisibleArea.Callback callback in this.Callbacks)
		{
			callback.OnUpdate();
		}
	}

	// Token: 0x06004130 RID: 16688 RVA: 0x0017284C File Offset: 0x00170A4C
	public void AddCallback(string name, System.Action on_update)
	{
		GridVisibleArea.Callback item = new GridVisibleArea.Callback
		{
			Name = name,
			OnUpdate = on_update
		};
		this.Callbacks.Add(item);
	}

	// Token: 0x06004131 RID: 16689 RVA: 0x00172880 File Offset: 0x00170A80
	public void Run(Action<int> in_view)
	{
		if (in_view != null)
		{
			this.CurrentArea.Run(in_view);
		}
	}

	// Token: 0x06004132 RID: 16690 RVA: 0x001728A0 File Offset: 0x00170AA0
	public void RunExtended(Action<int> in_view)
	{
		if (in_view != null)
		{
			this.CurrentAreaExtended.Run(in_view);
		}
	}

	// Token: 0x06004133 RID: 16691 RVA: 0x001728C0 File Offset: 0x00170AC0
	public void Run(Action<int> outside_view, Action<int> inside_view, Action<int> inside_view_second_time)
	{
		if (outside_view != null)
		{
			this.PreviousArea.RunOnDifference(this.CurrentArea, outside_view);
		}
		if (inside_view != null)
		{
			this.CurrentArea.RunOnDifference(this.PreviousArea, inside_view);
		}
		if (inside_view_second_time != null)
		{
			this.PreviousArea.RunOnDifference(this.PreviousPreviousArea, inside_view_second_time);
		}
	}

	// Token: 0x06004134 RID: 16692 RVA: 0x00172918 File Offset: 0x00170B18
	public void RunExtended(Action<int> outside_view, Action<int> inside_view, Action<int> inside_view_second_time)
	{
		if (outside_view != null)
		{
			this.PreviousAreaExtended.RunOnDifference(this.CurrentAreaExtended, outside_view);
		}
		if (inside_view != null)
		{
			this.CurrentAreaExtended.RunOnDifference(this.PreviousAreaExtended, inside_view);
		}
		if (inside_view_second_time != null)
		{
			this.PreviousAreaExtended.RunOnDifference(this.PreviousPreviousAreaExtended, inside_view_second_time);
		}
	}

	// Token: 0x06004135 RID: 16693 RVA: 0x00172970 File Offset: 0x00170B70
	public void RunIfVisible(int cell, Action<int> action)
	{
		this.CurrentArea.RunIfInside(cell, action);
	}

	// Token: 0x06004136 RID: 16694 RVA: 0x00172990 File Offset: 0x00170B90
	public void RunIfVisibleExtended(int cell, Action<int> action)
	{
		this.CurrentAreaExtended.RunIfInside(cell, action);
	}

	// Token: 0x06004137 RID: 16695 RVA: 0x001729AD File Offset: 0x00170BAD
	public static GridArea GetVisibleArea()
	{
		return GridVisibleArea.GetVisibleAreaExtended(0);
	}

	// Token: 0x06004138 RID: 16696 RVA: 0x001729B8 File Offset: 0x00170BB8
	public static GridArea GetVisibleAreaExtended(int padding)
	{
		GridArea result = default(GridArea);
		Camera mainCamera = Game.MainCamera;
		if (mainCamera != null)
		{
			Vector3 vector = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, mainCamera.transform.GetPosition().z));
			Vector3 vector2 = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, mainCamera.transform.GetPosition().z));
			vector.x += (float)padding;
			vector.y += (float)padding;
			vector2.x -= (float)padding;
			vector2.y -= (float)padding;
			if (CameraController.Instance != null)
			{
				Vector2I vector2I;
				Vector2I vector2I2;
				CameraController.Instance.GetWorldCamera(out vector2I, out vector2I2);
				result.SetExtents(Math.Max((int)(vector2.x - 0.5f), vector2I.x), Math.Max((int)(vector2.y - 0.5f), vector2I.y), Math.Min((int)(vector.x + 1.5f), vector2I2.x + vector2I.x), Math.Min((int)(vector.y + 1.5f), vector2I2.y + vector2I.y));
			}
			else
			{
				result.SetExtents(Math.Max((int)(vector2.x - 0.5f), 0), Math.Max((int)(vector2.y - 0.5f), 0), Math.Min((int)(vector.x + 1.5f), Grid.WidthInCells), Math.Min((int)(vector.y + 1.5f), Grid.HeightInCells));
			}
		}
		return result;
	}

	// Token: 0x04002B3A RID: 11066
	private GridArea[] VisibleAreas = new GridArea[3];

	// Token: 0x04002B3B RID: 11067
	private GridArea[] VisibleAreasExtended = new GridArea[3];

	// Token: 0x04002B3C RID: 11068
	private List<GridVisibleArea.Callback> Callbacks = new List<GridVisibleArea.Callback>();

	// Token: 0x04002B3D RID: 11069
	public bool debugFreezeVisibleArea;

	// Token: 0x04002B3E RID: 11070
	public bool debugFreezeVisibleAreasExtended;

	// Token: 0x04002B3F RID: 11071
	private readonly int _padding;

	// Token: 0x0200184D RID: 6221
	public struct Callback
	{
		// Token: 0x04007578 RID: 30072
		public System.Action OnUpdate;

		// Token: 0x04007579 RID: 30073
		public string Name;
	}
}
