using System;

// Token: 0x02000A81 RID: 2689
public class ScenePartitionerLayer
{
	// Token: 0x06004EE8 RID: 20200 RVA: 0x001C6832 File Offset: 0x001C4A32
	public ScenePartitionerLayer(HashedString name, int layer)
	{
		this.name = name;
		this.layer = layer;
	}

	// Token: 0x04003475 RID: 13429
	public HashedString name;

	// Token: 0x04003476 RID: 13430
	public int layer;

	// Token: 0x04003477 RID: 13431
	public Action<int, object> OnEvent;
}
