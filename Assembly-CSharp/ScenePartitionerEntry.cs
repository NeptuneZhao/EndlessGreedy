using System;

// Token: 0x02000A80 RID: 2688
public class ScenePartitionerEntry
{
	// Token: 0x06004EE4 RID: 20196 RVA: 0x001C6794 File Offset: 0x001C4994
	public ScenePartitionerEntry(string name, object obj, int x, int y, int width, int height, ScenePartitionerLayer layer, ScenePartitioner partitioner, Action<object> event_callback)
	{
		if (x < 0 || y < 0 || width >= 0)
		{
		}
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
		this.layer = layer.layer;
		this.partitioner = partitioner;
		this.eventCallback = event_callback;
		this.obj = obj;
	}

	// Token: 0x06004EE5 RID: 20197 RVA: 0x001C67FD File Offset: 0x001C49FD
	public void UpdatePosition(int x, int y)
	{
		this.partitioner.UpdatePosition(x, y, this);
	}

	// Token: 0x06004EE6 RID: 20198 RVA: 0x001C680D File Offset: 0x001C4A0D
	public void UpdatePosition(Extents e)
	{
		this.partitioner.UpdatePosition(e, this);
	}

	// Token: 0x06004EE7 RID: 20199 RVA: 0x001C681C File Offset: 0x001C4A1C
	public void Release()
	{
		if (this.partitioner != null)
		{
			this.partitioner.Remove(this);
		}
	}

	// Token: 0x0400346C RID: 13420
	public int x;

	// Token: 0x0400346D RID: 13421
	public int y;

	// Token: 0x0400346E RID: 13422
	public int width;

	// Token: 0x0400346F RID: 13423
	public int height;

	// Token: 0x04003470 RID: 13424
	public int layer;

	// Token: 0x04003471 RID: 13425
	public int queryId;

	// Token: 0x04003472 RID: 13426
	public ScenePartitioner partitioner;

	// Token: 0x04003473 RID: 13427
	public Action<object> eventCallback;

	// Token: 0x04003474 RID: 13428
	public object obj;
}
