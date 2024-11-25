using System;
using UnityEngine;

// Token: 0x0200068A RID: 1674
public interface IKComponentManager
{
	// Token: 0x060029B9 RID: 10681
	HandleVector<int>.Handle Add(GameObject go);

	// Token: 0x060029BA RID: 10682
	void Remove(GameObject go);
}
