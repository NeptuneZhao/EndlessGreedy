using System;
using Database;
using UnityEngine;

// Token: 0x02000C84 RID: 3204
public interface IKleiPermitDioramaVisTarget
{
	// Token: 0x06006297 RID: 25239
	GameObject GetGameObject();

	// Token: 0x06006298 RID: 25240
	void ConfigureSetup();

	// Token: 0x06006299 RID: 25241
	void ConfigureWith(PermitResource permit);
}
