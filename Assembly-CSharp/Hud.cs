using System;

// Token: 0x02000C61 RID: 3169
public class Hud : KScreen
{
	// Token: 0x06006146 RID: 24902 RVA: 0x00244025 File Offset: 0x00242225
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Help))
		{
			GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ControlsScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
		}
	}
}
