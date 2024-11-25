using System;
using UnityEngine;

// Token: 0x02000786 RID: 1926
public interface IUsable
{
	// Token: 0x06003477 RID: 13431
	bool IsUsable();

	// Token: 0x17000397 RID: 919
	// (get) Token: 0x06003478 RID: 13432
	Transform transform { get; }
}
