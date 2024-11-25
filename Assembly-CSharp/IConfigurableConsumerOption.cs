using System;
using UnityEngine;

// Token: 0x02000D5F RID: 3423
public interface IConfigurableConsumerOption
{
	// Token: 0x06006BD3 RID: 27603
	Tag GetID();

	// Token: 0x06006BD4 RID: 27604
	string GetName();

	// Token: 0x06006BD5 RID: 27605
	string GetDetailedDescription();

	// Token: 0x06006BD6 RID: 27606
	string GetDescription();

	// Token: 0x06006BD7 RID: 27607
	Sprite GetIcon();

	// Token: 0x06006BD8 RID: 27608
	IConfigurableConsumerIngredient[] GetIngredients();
}
