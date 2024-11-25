using System;

// Token: 0x02000C2E RID: 3118
public class DemoOverScreen : KModalScreen
{
	// Token: 0x06005FB5 RID: 24501 RVA: 0x00238E04 File Offset: 0x00237004
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Init();
		PlayerController.Instance.ActivateTool(SelectTool.Instance);
		SelectTool.Instance.Select(null, false);
	}

	// Token: 0x06005FB6 RID: 24502 RVA: 0x00238E2D File Offset: 0x0023702D
	private void Init()
	{
		this.QuitButton.onClick += delegate()
		{
			this.Quit();
		};
	}

	// Token: 0x06005FB7 RID: 24503 RVA: 0x00238E46 File Offset: 0x00237046
	private void Quit()
	{
		PauseScreen.TriggerQuitGame();
	}

	// Token: 0x0400407D RID: 16509
	public KButton QuitButton;
}
