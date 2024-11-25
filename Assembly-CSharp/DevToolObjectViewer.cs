using System;

// Token: 0x0200061F RID: 1567
public class DevToolObjectViewer<T> : DevTool
{
	// Token: 0x0600269C RID: 9884 RVA: 0x000D8F53 File Offset: 0x000D7153
	public DevToolObjectViewer(Func<T> getValue)
	{
		this.getValue = getValue;
		this.Name = typeof(T).Name;
	}

	// Token: 0x0600269D RID: 9885 RVA: 0x000D8F78 File Offset: 0x000D7178
	protected override void RenderTo(DevPanel panel)
	{
		T t = this.getValue();
		this.Name = t.GetType().Name;
		ImGuiEx.DrawObject(t, null);
	}

	// Token: 0x0400161B RID: 5659
	private Func<T> getValue;
}
