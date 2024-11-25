using System;

// Token: 0x0200061D RID: 1565
public class DevToolMenuNodeAction : IMenuNode
{
	// Token: 0x0600268F RID: 9871 RVA: 0x000D86E8 File Offset: 0x000D68E8
	public DevToolMenuNodeAction(string name, System.Action onClickFn)
	{
		this.name = name;
		this.onClickFn = onClickFn;
	}

	// Token: 0x06002690 RID: 9872 RVA: 0x000D86FE File Offset: 0x000D68FE
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x06002691 RID: 9873 RVA: 0x000D8706 File Offset: 0x000D6906
	public void Draw()
	{
		if (ImGuiEx.MenuItem(this.name, this.isEnabledFn == null || this.isEnabledFn()))
		{
			this.onClickFn();
		}
	}

	// Token: 0x0400160D RID: 5645
	public string name;

	// Token: 0x0400160E RID: 5646
	public System.Action onClickFn;

	// Token: 0x0400160F RID: 5647
	public Func<bool> isEnabledFn;
}
