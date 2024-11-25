using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000B50 RID: 2896
public class UtilityNetworkTubesManager : UtilityNetworkManager<TravelTubeNetwork, TravelTube>
{
	// Token: 0x06005688 RID: 22152 RVA: 0x001EEE2B File Offset: 0x001ED02B
	public UtilityNetworkTubesManager(int game_width, int game_height, int tile_layer) : base(game_width, game_height, tile_layer)
	{
	}

	// Token: 0x06005689 RID: 22153 RVA: 0x001EEE36 File Offset: 0x001ED036
	public override bool CanAddConnection(UtilityConnections new_connection, int cell, bool is_physical_building, out string fail_reason)
	{
		return this.TestForUTurnLeft(cell, new_connection, is_physical_building, out fail_reason) && this.TestForUTurnRight(cell, new_connection, is_physical_building, out fail_reason) && this.TestForNoAdjacentBridge(cell, new_connection, out fail_reason);
	}

	// Token: 0x0600568A RID: 22154 RVA: 0x001EEE5E File Offset: 0x001ED05E
	public override void SetConnections(UtilityConnections connections, int cell, bool is_physical_building)
	{
		base.SetConnections(connections, cell, is_physical_building);
		Pathfinding.Instance.AddDirtyNavGridCell(cell);
	}

	// Token: 0x0600568B RID: 22155 RVA: 0x001EEE74 File Offset: 0x001ED074
	private bool TestForUTurnLeft(int first_cell, UtilityConnections first_connection, bool is_physical_building, out string fail_reason)
	{
		int from_cell = first_cell;
		UtilityConnections direction = first_connection;
		int num = 1;
		for (int i = 0; i < 3; i++)
		{
			int num2 = direction.CellInDirection(from_cell);
			UtilityConnections utilityConnections = direction.LeftDirection();
			if (this.HasConnection(num2, utilityConnections, is_physical_building))
			{
				num++;
			}
			from_cell = num2;
			direction = utilityConnections;
		}
		fail_reason = UI.TOOLTIPS.HELP_TUBELOCATION_NO_UTURNS;
		return num <= 2;
	}

	// Token: 0x0600568C RID: 22156 RVA: 0x001EEED0 File Offset: 0x001ED0D0
	private bool TestForUTurnRight(int first_cell, UtilityConnections first_connection, bool is_physical_building, out string fail_reason)
	{
		int from_cell = first_cell;
		UtilityConnections direction = first_connection;
		int num = 1;
		for (int i = 0; i < 3; i++)
		{
			int num2 = direction.CellInDirection(from_cell);
			UtilityConnections utilityConnections = direction.RightDirection();
			if (this.HasConnection(num2, utilityConnections, is_physical_building))
			{
				num++;
			}
			from_cell = num2;
			direction = utilityConnections;
		}
		fail_reason = UI.TOOLTIPS.HELP_TUBELOCATION_NO_UTURNS;
		return num <= 2;
	}

	// Token: 0x0600568D RID: 22157 RVA: 0x001EEF2C File Offset: 0x001ED12C
	private bool TestForNoAdjacentBridge(int cell, UtilityConnections connection, out string fail_reason)
	{
		UtilityConnections direction = connection.LeftDirection();
		UtilityConnections direction2 = connection.RightDirection();
		int cell2 = direction.CellInDirection(cell);
		int cell3 = direction2.CellInDirection(cell);
		GameObject gameObject = Grid.Objects[cell2, 9];
		GameObject gameObject2 = Grid.Objects[cell3, 9];
		fail_reason = UI.TOOLTIPS.HELP_TUBELOCATION_STRAIGHT_BRIDGES;
		return (gameObject == null || gameObject.GetComponent<TravelTubeBridge>() == null) && (gameObject2 == null || gameObject2.GetComponent<TravelTubeBridge>() == null);
	}

	// Token: 0x0600568E RID: 22158 RVA: 0x001EEFB0 File Offset: 0x001ED1B0
	private bool HasConnection(int cell, UtilityConnections connection, bool is_physical_building)
	{
		return (base.GetConnections(cell, is_physical_building) & connection) > (UtilityConnections)0;
	}
}
