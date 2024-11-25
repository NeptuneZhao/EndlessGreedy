using System;

// Token: 0x020009E4 RID: 2532
public class PassiveElementConsumer : ElementConsumer, IGameObjectEffectDescriptor
{
	// Token: 0x06004977 RID: 18807 RVA: 0x001A4C6A File Offset: 0x001A2E6A
	protected override bool IsActive()
	{
		return true;
	}
}
