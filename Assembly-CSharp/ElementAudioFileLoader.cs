using System;

// Token: 0x02000652 RID: 1618
internal class ElementAudioFileLoader : AsyncCsvLoader<ElementAudioFileLoader, ElementsAudio.ElementAudioConfig>
{
	// Token: 0x060027B7 RID: 10167 RVA: 0x000E2031 File Offset: 0x000E0231
	public ElementAudioFileLoader() : base(Assets.instance.elementAudio)
	{
	}

	// Token: 0x060027B8 RID: 10168 RVA: 0x000E2043 File Offset: 0x000E0243
	public override void Run()
	{
		base.Run();
	}
}
