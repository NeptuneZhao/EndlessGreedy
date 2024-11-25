using System;

// Token: 0x02000B56 RID: 2902
public class VisibleAreaUpdater
{
	// Token: 0x060056B9 RID: 22201 RVA: 0x001EFEF6 File Offset: 0x001EE0F6
	public VisibleAreaUpdater(Action<int> outside_view_first_time_cb, Action<int> inside_view_first_time_cb, string name)
	{
		this.OutsideViewFirstTimeCallback = outside_view_first_time_cb;
		this.InsideViewFirstTimeCallback = inside_view_first_time_cb;
		this.UpdateCallback = new Action<int>(this.InternalUpdateCell);
		this.Name = name;
	}

	// Token: 0x060056BA RID: 22202 RVA: 0x001EFF25 File Offset: 0x001EE125
	public void Update()
	{
		if (CameraController.Instance != null && this.VisibleArea == null)
		{
			this.VisibleArea = CameraController.Instance.VisibleArea;
			this.VisibleArea.Run(this.InsideViewFirstTimeCallback);
		}
	}

	// Token: 0x060056BB RID: 22203 RVA: 0x001EFF5D File Offset: 0x001EE15D
	private void InternalUpdateCell(int cell)
	{
		this.OutsideViewFirstTimeCallback(cell);
		this.InsideViewFirstTimeCallback(cell);
	}

	// Token: 0x060056BC RID: 22204 RVA: 0x001EFF77 File Offset: 0x001EE177
	public void UpdateCell(int cell)
	{
		if (this.VisibleArea != null)
		{
			this.VisibleArea.RunIfVisible(cell, this.UpdateCallback);
		}
	}

	// Token: 0x040038C6 RID: 14534
	private GridVisibleArea VisibleArea;

	// Token: 0x040038C7 RID: 14535
	private Action<int> OutsideViewFirstTimeCallback;

	// Token: 0x040038C8 RID: 14536
	private Action<int> InsideViewFirstTimeCallback;

	// Token: 0x040038C9 RID: 14537
	private Action<int> UpdateCallback;

	// Token: 0x040038CA RID: 14538
	private string Name;
}
