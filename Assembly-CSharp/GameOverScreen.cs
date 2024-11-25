using System;

// Token: 0x02000C4D RID: 3149
public class GameOverScreen : KModalScreen
{
	// Token: 0x060060CB RID: 24779 RVA: 0x0024032A File Offset: 0x0023E52A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Init();
	}

	// Token: 0x060060CC RID: 24780 RVA: 0x00240338 File Offset: 0x0023E538
	private void Init()
	{
		if (this.QuitButton)
		{
			this.QuitButton.onClick += delegate()
			{
				this.Quit();
			};
		}
		if (this.DismissButton)
		{
			this.DismissButton.onClick += delegate()
			{
				this.Dismiss();
			};
		}
	}

	// Token: 0x060060CD RID: 24781 RVA: 0x0024038D File Offset: 0x0023E58D
	private void Quit()
	{
		PauseScreen.TriggerQuitGame();
	}

	// Token: 0x060060CE RID: 24782 RVA: 0x00240394 File Offset: 0x0023E594
	private void Dismiss()
	{
		this.Show(false);
	}

	// Token: 0x0400416E RID: 16750
	public KButton DismissButton;

	// Token: 0x0400416F RID: 16751
	public KButton QuitButton;
}
