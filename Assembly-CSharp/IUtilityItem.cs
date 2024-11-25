using System;

// Token: 0x02000B44 RID: 2884
public interface IUtilityItem
{
	// Token: 0x1700066B RID: 1643
	// (get) Token: 0x0600561E RID: 22046
	// (set) Token: 0x0600561F RID: 22047
	UtilityConnections Connections { get; set; }

	// Token: 0x06005620 RID: 22048
	void UpdateConnections(UtilityConnections Connections);

	// Token: 0x06005621 RID: 22049
	int GetNetworkID();

	// Token: 0x06005622 RID: 22050
	UtilityNetwork GetNetworkForDirection(Direction d);
}
