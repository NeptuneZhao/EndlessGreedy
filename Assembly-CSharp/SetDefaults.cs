using System;

// Token: 0x02000A93 RID: 2707
public class SetDefaults
{
	// Token: 0x06004F56 RID: 20310 RVA: 0x001C86B0 File Offset: 0x001C68B0
	public static void Initialize()
	{
		KSlider.DefaultSounds[0] = GlobalAssets.GetSound("Slider_Start", false);
		KSlider.DefaultSounds[1] = GlobalAssets.GetSound("Slider_Move", false);
		KSlider.DefaultSounds[2] = GlobalAssets.GetSound("Slider_End", false);
		KSlider.DefaultSounds[3] = GlobalAssets.GetSound("Slider_Boundary_Low", false);
		KSlider.DefaultSounds[4] = GlobalAssets.GetSound("Slider_Boundary_High", false);
		KScrollRect.DefaultSounds[KScrollRect.SoundType.OnMouseScroll] = GlobalAssets.GetSound("Mousewheel_Move", false);
		WidgetSoundPlayer.getSoundPath = new Func<string, string>(SetDefaults.GetSoundPath);
	}

	// Token: 0x06004F57 RID: 20311 RVA: 0x001C873E File Offset: 0x001C693E
	private static string GetSoundPath(string sound_name)
	{
		return GlobalAssets.GetSound(sound_name, false);
	}
}
