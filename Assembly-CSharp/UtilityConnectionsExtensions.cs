using System;

// Token: 0x02000B4B RID: 2891
public static class UtilityConnectionsExtensions
{
	// Token: 0x06005644 RID: 22084 RVA: 0x001ED70C File Offset: 0x001EB90C
	public static UtilityConnections InverseDirection(this UtilityConnections direction)
	{
		switch (direction)
		{
		case UtilityConnections.Left:
			return UtilityConnections.Right;
		case UtilityConnections.Right:
			return UtilityConnections.Left;
		case UtilityConnections.Left | UtilityConnections.Right:
			break;
		case UtilityConnections.Up:
			return UtilityConnections.Down;
		default:
			if (direction == UtilityConnections.Down)
			{
				return UtilityConnections.Up;
			}
			break;
		}
		throw new ArgumentException("Unexpected enum value: " + direction.ToString(), "direction");
	}

	// Token: 0x06005645 RID: 22085 RVA: 0x001ED760 File Offset: 0x001EB960
	public static UtilityConnections LeftDirection(this UtilityConnections direction)
	{
		switch (direction)
		{
		case UtilityConnections.Left:
			return UtilityConnections.Down;
		case UtilityConnections.Right:
			return UtilityConnections.Up;
		case UtilityConnections.Left | UtilityConnections.Right:
			break;
		case UtilityConnections.Up:
			return UtilityConnections.Left;
		default:
			if (direction == UtilityConnections.Down)
			{
				return UtilityConnections.Right;
			}
			break;
		}
		throw new ArgumentException("Unexpected enum value: " + direction.ToString(), "direction");
	}

	// Token: 0x06005646 RID: 22086 RVA: 0x001ED7B4 File Offset: 0x001EB9B4
	public static UtilityConnections RightDirection(this UtilityConnections direction)
	{
		switch (direction)
		{
		case UtilityConnections.Left:
			return UtilityConnections.Up;
		case UtilityConnections.Right:
			return UtilityConnections.Down;
		case UtilityConnections.Left | UtilityConnections.Right:
			break;
		case UtilityConnections.Up:
			return UtilityConnections.Right;
		default:
			if (direction == UtilityConnections.Down)
			{
				return UtilityConnections.Left;
			}
			break;
		}
		throw new ArgumentException("Unexpected enum value: " + direction.ToString(), "direction");
	}

	// Token: 0x06005647 RID: 22087 RVA: 0x001ED808 File Offset: 0x001EBA08
	public static int CellInDirection(this UtilityConnections direction, int from_cell)
	{
		switch (direction)
		{
		case UtilityConnections.Left:
			return from_cell - 1;
		case UtilityConnections.Right:
			return from_cell + 1;
		case UtilityConnections.Left | UtilityConnections.Right:
			break;
		case UtilityConnections.Up:
			return from_cell + Grid.WidthInCells;
		default:
			if (direction == UtilityConnections.Down)
			{
				return from_cell - Grid.WidthInCells;
			}
			break;
		}
		throw new ArgumentException("Unexpected enum value: " + direction.ToString(), "direction");
	}

	// Token: 0x06005648 RID: 22088 RVA: 0x001ED86C File Offset: 0x001EBA6C
	public static UtilityConnections DirectionFromToCell(int from_cell, int to_cell)
	{
		if (to_cell == from_cell - 1)
		{
			return UtilityConnections.Left;
		}
		if (to_cell == from_cell + 1)
		{
			return UtilityConnections.Right;
		}
		if (to_cell == from_cell + Grid.WidthInCells)
		{
			return UtilityConnections.Up;
		}
		if (to_cell == from_cell - Grid.WidthInCells)
		{
			return UtilityConnections.Down;
		}
		return (UtilityConnections)0;
	}
}
