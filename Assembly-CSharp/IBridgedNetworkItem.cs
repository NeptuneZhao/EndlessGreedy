using System;
using System.Collections.Generic;

// Token: 0x0200069F RID: 1695
public interface IBridgedNetworkItem
{
	// Token: 0x06002A8B RID: 10891
	void AddNetworks(ICollection<UtilityNetwork> networks);

	// Token: 0x06002A8C RID: 10892
	bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks);

	// Token: 0x06002A8D RID: 10893
	int GetNetworkCell();
}
