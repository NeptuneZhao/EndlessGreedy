using System;

// Token: 0x02000609 RID: 1545
public class DevToolAnimFile : DevTool
{
	// Token: 0x06002620 RID: 9760 RVA: 0x000D3268 File Offset: 0x000D1468
	public DevToolAnimFile(KAnimFile animFile)
	{
		this.animFile = animFile;
		this.Name = "Anim File: \"" + animFile.name + "\"";
	}

	// Token: 0x06002621 RID: 9761 RVA: 0x000D3294 File Offset: 0x000D1494
	protected override void RenderTo(DevPanel panel)
	{
		ImGuiEx.DrawObject(this.animFile, null);
		ImGuiEx.DrawObject(this.animFile.GetData(), null);
	}

	// Token: 0x040015B9 RID: 5561
	private KAnimFile animFile;
}
