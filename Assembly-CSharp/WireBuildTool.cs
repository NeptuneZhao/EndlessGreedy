﻿using System;

// Token: 0x02000930 RID: 2352
public class WireBuildTool : BaseUtilityBuildTool
{
	// Token: 0x06004453 RID: 17491 RVA: 0x00185302 File Offset: 0x00183502
	public static void DestroyInstance()
	{
		WireBuildTool.Instance = null;
	}

	// Token: 0x06004454 RID: 17492 RVA: 0x0018530A File Offset: 0x0018350A
	protected override void OnPrefabInit()
	{
		WireBuildTool.Instance = this;
		base.OnPrefabInit();
		this.viewMode = OverlayModes.Power.ID;
	}

	// Token: 0x06004455 RID: 17493 RVA: 0x00185324 File Offset: 0x00183524
	protected override void ApplyPathToConduitSystem()
	{
		if (this.path.Count < 2)
		{
			return;
		}
		for (int i = 1; i < this.path.Count; i++)
		{
			if (this.path[i - 1].valid && this.path[i].valid)
			{
				int cell = this.path[i - 1].cell;
				int cell2 = this.path[i].cell;
				UtilityConnections utilityConnections = UtilityConnectionsExtensions.DirectionFromToCell(cell, this.path[i].cell);
				if (utilityConnections != (UtilityConnections)0)
				{
					UtilityConnections new_connection = utilityConnections.InverseDirection();
					this.conduitMgr.AddConnection(utilityConnections, cell, false);
					this.conduitMgr.AddConnection(new_connection, cell2, false);
				}
			}
		}
	}

	// Token: 0x04002CB6 RID: 11446
	public static WireBuildTool Instance;
}
