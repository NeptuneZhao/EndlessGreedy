using System;
using System.Collections.Generic;

// Token: 0x020008B2 RID: 2226
public interface IFetchList
{
	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x06003E32 RID: 15922
	Storage Destination { get; }

	// Token: 0x06003E33 RID: 15923
	float GetMinimumAmount(Tag tag);

	// Token: 0x06003E34 RID: 15924
	Dictionary<Tag, float> GetRemaining();

	// Token: 0x06003E35 RID: 15925
	Dictionary<Tag, float> GetRemainingMinimum();
}
