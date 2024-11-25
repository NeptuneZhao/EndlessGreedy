using System;

// Token: 0x02000D5E RID: 3422
public interface IConfigurableConsumer
{
	// Token: 0x06006BD0 RID: 27600
	IConfigurableConsumerOption[] GetSettingOptions();

	// Token: 0x06006BD1 RID: 27601
	IConfigurableConsumerOption GetSelectedOption();

	// Token: 0x06006BD2 RID: 27602
	void SetSelectedOption(IConfigurableConsumerOption option);
}
