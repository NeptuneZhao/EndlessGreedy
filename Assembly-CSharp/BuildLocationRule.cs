using System;

// Token: 0x02000838 RID: 2104
public enum BuildLocationRule
{
	// Token: 0x0400233A RID: 9018
	Anywhere,
	// Token: 0x0400233B RID: 9019
	OnFloor,
	// Token: 0x0400233C RID: 9020
	OnFloorOverSpace,
	// Token: 0x0400233D RID: 9021
	OnCeiling,
	// Token: 0x0400233E RID: 9022
	OnWall,
	// Token: 0x0400233F RID: 9023
	InCorner,
	// Token: 0x04002340 RID: 9024
	Tile,
	// Token: 0x04002341 RID: 9025
	NotInTiles,
	// Token: 0x04002342 RID: 9026
	Conduit,
	// Token: 0x04002343 RID: 9027
	LogicBridge,
	// Token: 0x04002344 RID: 9028
	WireBridge,
	// Token: 0x04002345 RID: 9029
	HighWattBridgeTile,
	// Token: 0x04002346 RID: 9030
	BuildingAttachPoint,
	// Token: 0x04002347 RID: 9031
	OnFloorOrBuildingAttachPoint,
	// Token: 0x04002348 RID: 9032
	OnFoundationRotatable,
	// Token: 0x04002349 RID: 9033
	BelowRocketCeiling,
	// Token: 0x0400234A RID: 9034
	OnRocketEnvelope,
	// Token: 0x0400234B RID: 9035
	WallFloor,
	// Token: 0x0400234C RID: 9036
	NoLiquidConduitAtOrigin
}
