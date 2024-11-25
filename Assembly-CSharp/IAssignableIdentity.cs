using System;
using System.Collections.Generic;

// Token: 0x0200056D RID: 1389
public interface IAssignableIdentity
{
	// Token: 0x06002026 RID: 8230
	string GetProperName();

	// Token: 0x06002027 RID: 8231
	List<Ownables> GetOwners();

	// Token: 0x06002028 RID: 8232
	Ownables GetSoleOwner();

	// Token: 0x06002029 RID: 8233
	bool IsNull();

	// Token: 0x0600202A RID: 8234
	bool HasOwner(Assignables owner);

	// Token: 0x0600202B RID: 8235
	int NumOwners();
}
