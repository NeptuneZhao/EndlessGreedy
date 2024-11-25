using System;
using System.Collections.Generic;

// Token: 0x02000B4C RID: 2892
public interface IUtilityNetworkMgr
{
	// Token: 0x06005649 RID: 22089
	bool CanAddConnection(UtilityConnections new_connection, int cell, bool is_physical_building, out string fail_reason);

	// Token: 0x0600564A RID: 22090
	void AddConnection(UtilityConnections new_connection, int cell, bool is_physical_building);

	// Token: 0x0600564B RID: 22091
	void StashVisualGrids();

	// Token: 0x0600564C RID: 22092
	void UnstashVisualGrids();

	// Token: 0x0600564D RID: 22093
	string GetVisualizerString(int cell);

	// Token: 0x0600564E RID: 22094
	string GetVisualizerString(UtilityConnections connections);

	// Token: 0x0600564F RID: 22095
	UtilityConnections GetConnections(int cell, bool is_physical_building);

	// Token: 0x06005650 RID: 22096
	UtilityConnections GetDisplayConnections(int cell);

	// Token: 0x06005651 RID: 22097
	void SetConnections(UtilityConnections connections, int cell, bool is_physical_building);

	// Token: 0x06005652 RID: 22098
	void ClearCell(int cell, bool is_physical_building);

	// Token: 0x06005653 RID: 22099
	void ForceRebuildNetworks();

	// Token: 0x06005654 RID: 22100
	void AddToNetworks(int cell, object item, bool is_endpoint);

	// Token: 0x06005655 RID: 22101
	void RemoveFromNetworks(int cell, object vent, bool is_endpoint);

	// Token: 0x06005656 RID: 22102
	object GetEndpoint(int cell);

	// Token: 0x06005657 RID: 22103
	UtilityNetwork GetNetworkForDirection(int cell, Direction direction);

	// Token: 0x06005658 RID: 22104
	UtilityNetwork GetNetworkForCell(int cell);

	// Token: 0x06005659 RID: 22105
	void AddNetworksRebuiltListener(Action<IList<UtilityNetwork>, ICollection<int>> listener);

	// Token: 0x0600565A RID: 22106
	void RemoveNetworksRebuiltListener(Action<IList<UtilityNetwork>, ICollection<int>> listener);

	// Token: 0x0600565B RID: 22107
	IList<UtilityNetwork> GetNetworks();
}
