using System;
using System.Collections.Generic;
using System.Diagnostics;
using ProcGen;
using UnityEngine;

// Token: 0x020008DE RID: 2270
public class Grid
{
	// Token: 0x060040A3 RID: 16547 RVA: 0x0017033E File Offset: 0x0016E53E
	private static void UpdateBuildMask(int i, Grid.BuildFlags flag, bool state)
	{
		if (state)
		{
			Grid.BuildMasks[i] |= flag;
			return;
		}
		Grid.BuildMasks[i] &= ~flag;
	}

	// Token: 0x060040A4 RID: 16548 RVA: 0x00170366 File Offset: 0x0016E566
	public static void SetSolid(int cell, bool solid, CellSolidEvent ev)
	{
		Grid.UpdateBuildMask(cell, Grid.BuildFlags.Solid, solid);
	}

	// Token: 0x060040A5 RID: 16549 RVA: 0x00170372 File Offset: 0x0016E572
	private static void UpdateVisMask(int i, Grid.VisFlags flag, bool state)
	{
		if (state)
		{
			Grid.VisMasks[i] |= flag;
			return;
		}
		Grid.VisMasks[i] &= ~flag;
	}

	// Token: 0x060040A6 RID: 16550 RVA: 0x0017039A File Offset: 0x0016E59A
	private static void UpdateNavValidatorMask(int i, Grid.NavValidatorFlags flag, bool state)
	{
		if (state)
		{
			Grid.NavValidatorMasks[i] |= flag;
			return;
		}
		Grid.NavValidatorMasks[i] &= ~flag;
	}

	// Token: 0x060040A7 RID: 16551 RVA: 0x001703C2 File Offset: 0x0016E5C2
	private static void UpdateNavMask(int i, Grid.NavFlags flag, bool state)
	{
		if (state)
		{
			Grid.NavMasks[i] |= flag;
			return;
		}
		Grid.NavMasks[i] &= ~flag;
	}

	// Token: 0x060040A8 RID: 16552 RVA: 0x001703EA File Offset: 0x0016E5EA
	public static void ResetNavMasksAndDetails()
	{
		Grid.NavMasks = null;
		Grid.tubeEntrances.Clear();
		Grid.restrictions.Clear();
		Grid.suitMarkers.Clear();
	}

	// Token: 0x060040A9 RID: 16553 RVA: 0x00170410 File Offset: 0x0016E610
	public static bool DEBUG_GetRestrictions(int cell, out Grid.Restriction restriction)
	{
		return Grid.restrictions.TryGetValue(cell, out restriction);
	}

	// Token: 0x060040AA RID: 16554 RVA: 0x00170420 File Offset: 0x0016E620
	public static void RegisterRestriction(int cell, Grid.Restriction.Orientation orientation)
	{
		Grid.HasAccessDoor[cell] = true;
		Grid.restrictions[cell] = new Grid.Restriction
		{
			DirectionMasksForMinionInstanceID = new Dictionary<int, Grid.Restriction.Directions>(),
			orientation = orientation
		};
	}

	// Token: 0x060040AB RID: 16555 RVA: 0x00170461 File Offset: 0x0016E661
	public static void UnregisterRestriction(int cell)
	{
		Grid.restrictions.Remove(cell);
		Grid.HasAccessDoor[cell] = false;
	}

	// Token: 0x060040AC RID: 16556 RVA: 0x0017047B File Offset: 0x0016E67B
	public static void SetRestriction(int cell, int minionInstanceID, Grid.Restriction.Directions directions)
	{
		Grid.restrictions[cell].DirectionMasksForMinionInstanceID[minionInstanceID] = directions;
	}

	// Token: 0x060040AD RID: 16557 RVA: 0x00170494 File Offset: 0x0016E694
	public static void ClearRestriction(int cell, int minionInstanceID)
	{
		Grid.restrictions[cell].DirectionMasksForMinionInstanceID.Remove(minionInstanceID);
	}

	// Token: 0x060040AE RID: 16558 RVA: 0x001704B0 File Offset: 0x0016E6B0
	public static bool HasPermission(int cell, int minionInstanceID, int fromCell, NavType fromNavType)
	{
		if (!Grid.HasAccessDoor[cell])
		{
			return true;
		}
		Grid.Restriction restriction = Grid.restrictions[cell];
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2I vector2I2 = Grid.CellToXY(fromCell);
		Grid.Restriction.Directions directions = (Grid.Restriction.Directions)0;
		int num = vector2I.x - vector2I2.x;
		int num2 = vector2I.y - vector2I2.y;
		switch (restriction.orientation)
		{
		case Grid.Restriction.Orientation.Vertical:
			if (num < 0)
			{
				directions |= Grid.Restriction.Directions.Left;
			}
			if (num > 0)
			{
				directions |= Grid.Restriction.Directions.Right;
			}
			break;
		case Grid.Restriction.Orientation.Horizontal:
			if (num2 > 0)
			{
				directions |= Grid.Restriction.Directions.Left;
			}
			if (num2 < 0)
			{
				directions |= Grid.Restriction.Directions.Right;
			}
			break;
		case Grid.Restriction.Orientation.SingleCell:
			if (Math.Abs(num) != 1 && Math.Abs(num2) != 1 && fromNavType != NavType.Teleport)
			{
				directions |= Grid.Restriction.Directions.Teleport;
			}
			break;
		}
		Grid.Restriction.Directions directions2 = (Grid.Restriction.Directions)0;
		return (!restriction.DirectionMasksForMinionInstanceID.TryGetValue(minionInstanceID, out directions2) && !restriction.DirectionMasksForMinionInstanceID.TryGetValue(-1, out directions2)) || (directions2 & directions) == (Grid.Restriction.Directions)0;
	}

	// Token: 0x060040AF RID: 16559 RVA: 0x00170590 File Offset: 0x0016E790
	public static void RegisterTubeEntrance(int cell, int reservationCapacity)
	{
		DebugUtil.Assert(!Grid.tubeEntrances.ContainsKey(cell));
		Grid.HasTubeEntrance[cell] = true;
		Grid.tubeEntrances[cell] = new Grid.TubeEntrance
		{
			reservationCapacity = reservationCapacity,
			reservedInstanceIDs = new HashSet<int>()
		};
	}

	// Token: 0x060040B0 RID: 16560 RVA: 0x001705E4 File Offset: 0x0016E7E4
	public static void UnregisterTubeEntrance(int cell)
	{
		DebugUtil.Assert(Grid.tubeEntrances.ContainsKey(cell));
		Grid.HasTubeEntrance[cell] = false;
		Grid.tubeEntrances.Remove(cell);
	}

	// Token: 0x060040B1 RID: 16561 RVA: 0x00170610 File Offset: 0x0016E810
	public static bool ReserveTubeEntrance(int cell, int minionInstanceID, bool reserve)
	{
		Grid.TubeEntrance tubeEntrance = Grid.tubeEntrances[cell];
		HashSet<int> reservedInstanceIDs = tubeEntrance.reservedInstanceIDs;
		if (!reserve)
		{
			return reservedInstanceIDs.Remove(minionInstanceID);
		}
		DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
		if (reservedInstanceIDs.Count == tubeEntrance.reservationCapacity)
		{
			return false;
		}
		DebugUtil.Assert(reservedInstanceIDs.Add(minionInstanceID));
		return true;
	}

	// Token: 0x060040B2 RID: 16562 RVA: 0x00170668 File Offset: 0x0016E868
	public static void SetTubeEntranceReservationCapacity(int cell, int newReservationCapacity)
	{
		DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
		Grid.TubeEntrance value = Grid.tubeEntrances[cell];
		value.reservationCapacity = newReservationCapacity;
		Grid.tubeEntrances[cell] = value;
	}

	// Token: 0x060040B3 RID: 16563 RVA: 0x001706A8 File Offset: 0x0016E8A8
	public static bool HasUsableTubeEntrance(int cell, int minionInstanceID)
	{
		if (!Grid.HasTubeEntrance[cell])
		{
			return false;
		}
		Grid.TubeEntrance tubeEntrance = Grid.tubeEntrances[cell];
		if (!tubeEntrance.operational)
		{
			return false;
		}
		HashSet<int> reservedInstanceIDs = tubeEntrance.reservedInstanceIDs;
		return reservedInstanceIDs.Count < tubeEntrance.reservationCapacity || reservedInstanceIDs.Contains(minionInstanceID);
	}

	// Token: 0x060040B4 RID: 16564 RVA: 0x001706F8 File Offset: 0x0016E8F8
	public static bool HasReservedTubeEntrance(int cell, int minionInstanceID)
	{
		DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
		return Grid.tubeEntrances[cell].reservedInstanceIDs.Contains(minionInstanceID);
	}

	// Token: 0x060040B5 RID: 16565 RVA: 0x00170720 File Offset: 0x0016E920
	public static void SetTubeEntranceOperational(int cell, bool operational)
	{
		DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
		Grid.TubeEntrance value = Grid.tubeEntrances[cell];
		value.operational = operational;
		Grid.tubeEntrances[cell] = value;
	}

	// Token: 0x060040B6 RID: 16566 RVA: 0x00170760 File Offset: 0x0016E960
	public static void RegisterSuitMarker(int cell)
	{
		DebugUtil.Assert(!Grid.HasSuitMarker[cell]);
		Grid.HasSuitMarker[cell] = true;
		Grid.suitMarkers[cell] = new Grid.SuitMarker
		{
			suitCount = 0,
			lockerCount = 0,
			flags = Grid.SuitMarker.Flags.Operational,
			minionIDsWithSuitReservations = new HashSet<int>(),
			minionIDsWithEmptyLockerReservations = new HashSet<int>()
		};
	}

	// Token: 0x060040B7 RID: 16567 RVA: 0x001707D0 File Offset: 0x0016E9D0
	public static void UnregisterSuitMarker(int cell)
	{
		DebugUtil.Assert(Grid.HasSuitMarker[cell]);
		Grid.HasSuitMarker[cell] = false;
		Grid.suitMarkers.Remove(cell);
	}

	// Token: 0x060040B8 RID: 16568 RVA: 0x001707FC File Offset: 0x0016E9FC
	public static bool ReserveSuit(int cell, int minionInstanceID, bool reserve)
	{
		DebugUtil.Assert(Grid.HasSuitMarker[cell]);
		Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
		HashSet<int> minionIDsWithSuitReservations = suitMarker.minionIDsWithSuitReservations;
		if (!reserve)
		{
			bool flag = minionIDsWithSuitReservations.Remove(minionInstanceID);
			DebugUtil.Assert(flag);
			return flag;
		}
		if (minionIDsWithSuitReservations.Count >= suitMarker.suitCount)
		{
			return false;
		}
		DebugUtil.Assert(minionIDsWithSuitReservations.Add(minionInstanceID));
		return true;
	}

	// Token: 0x060040B9 RID: 16569 RVA: 0x0017085C File Offset: 0x0016EA5C
	public static bool ReserveEmptyLocker(int cell, int minionInstanceID, bool reserve)
	{
		DebugUtil.Assert(Grid.HasSuitMarker[cell], "No suit marker");
		Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
		HashSet<int> minionIDsWithEmptyLockerReservations = suitMarker.minionIDsWithEmptyLockerReservations;
		if (!reserve)
		{
			bool flag = minionIDsWithEmptyLockerReservations.Remove(minionInstanceID);
			DebugUtil.Assert(flag, "Reservation not removed");
			return flag;
		}
		if (minionIDsWithEmptyLockerReservations.Count >= suitMarker.emptyLockerCount)
		{
			return false;
		}
		DebugUtil.Assert(minionIDsWithEmptyLockerReservations.Add(minionInstanceID), "Reservation not made");
		return true;
	}

	// Token: 0x060040BA RID: 16570 RVA: 0x001708CC File Offset: 0x0016EACC
	public static void UpdateSuitMarker(int cell, int fullLockerCount, int emptyLockerCount, Grid.SuitMarker.Flags flags, PathFinder.PotentialPath.Flags pathFlags)
	{
		DebugUtil.Assert(Grid.HasSuitMarker[cell]);
		Grid.SuitMarker value = Grid.suitMarkers[cell];
		value.suitCount = fullLockerCount;
		value.lockerCount = fullLockerCount + emptyLockerCount;
		value.flags = flags;
		value.pathFlags = pathFlags;
		Grid.suitMarkers[cell] = value;
	}

	// Token: 0x060040BB RID: 16571 RVA: 0x00170924 File Offset: 0x0016EB24
	public static bool TryGetSuitMarkerFlags(int cell, out Grid.SuitMarker.Flags flags, out PathFinder.PotentialPath.Flags pathFlags)
	{
		if (Grid.HasSuitMarker[cell])
		{
			flags = Grid.suitMarkers[cell].flags;
			pathFlags = Grid.suitMarkers[cell].pathFlags;
			return true;
		}
		flags = (Grid.SuitMarker.Flags)0;
		pathFlags = PathFinder.PotentialPath.Flags.None;
		return false;
	}

	// Token: 0x060040BC RID: 16572 RVA: 0x00170960 File Offset: 0x0016EB60
	public static bool HasSuit(int cell, int minionInstanceID)
	{
		if (!Grid.HasSuitMarker[cell])
		{
			return false;
		}
		Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
		HashSet<int> minionIDsWithSuitReservations = suitMarker.minionIDsWithSuitReservations;
		return minionIDsWithSuitReservations.Count < suitMarker.suitCount || minionIDsWithSuitReservations.Contains(minionInstanceID);
	}

	// Token: 0x060040BD RID: 16573 RVA: 0x001709A8 File Offset: 0x0016EBA8
	public static bool HasEmptyLocker(int cell, int minionInstanceID)
	{
		if (!Grid.HasSuitMarker[cell])
		{
			return false;
		}
		Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
		HashSet<int> minionIDsWithEmptyLockerReservations = suitMarker.minionIDsWithEmptyLockerReservations;
		return minionIDsWithEmptyLockerReservations.Count < suitMarker.emptyLockerCount || minionIDsWithEmptyLockerReservations.Contains(minionInstanceID);
	}

	// Token: 0x060040BE RID: 16574 RVA: 0x001709F0 File Offset: 0x0016EBF0
	public unsafe static void InitializeCells()
	{
		for (int num = 0; num != Grid.WidthInCells * Grid.HeightInCells; num++)
		{
			ushort index = Grid.elementIdx[num];
			Element element = ElementLoader.elements[(int)index];
			Grid.Element[num] = element;
			if (element.IsSolid)
			{
				Grid.BuildMasks[num] |= Grid.BuildFlags.Solid;
			}
			else
			{
				Grid.BuildMasks[num] &= ~Grid.BuildFlags.Solid;
			}
			Grid.RenderedByWorld[num] = (element.substance != null && element.substance.renderedByWorld && Grid.Objects[num, 9] == null);
		}
	}

	// Token: 0x060040BF RID: 16575 RVA: 0x00170A9D File Offset: 0x0016EC9D
	public static bool IsInitialized()
	{
		return Grid.mass != null;
	}

	// Token: 0x060040C0 RID: 16576 RVA: 0x00170AAC File Offset: 0x0016ECAC
	public static int GetCellInDirection(int cell, Direction d)
	{
		switch (d)
		{
		case Direction.Up:
			return Grid.CellAbove(cell);
		case Direction.Right:
			return Grid.CellRight(cell);
		case Direction.Down:
			return Grid.CellBelow(cell);
		case Direction.Left:
			return Grid.CellLeft(cell);
		case Direction.None:
			return cell;
		}
		return -1;
	}

	// Token: 0x060040C1 RID: 16577 RVA: 0x00170AF8 File Offset: 0x0016ECF8
	public static bool Raycast(int cell, Vector2I direction, out int hitDistance, int maxDistance = 100, Grid.BuildFlags layerMask = Grid.BuildFlags.Any)
	{
		bool flag = false;
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2I vector2I2 = vector2I + direction * maxDistance;
		int num = cell;
		int num2 = Grid.XYToCell(vector2I2.x, vector2I2.y);
		int num3 = 0;
		int num4 = 0;
		float num5 = (float)maxDistance * 0.5f;
		while ((float)num3 < num5)
		{
			if (!Grid.IsValidCell(num) || (Grid.BuildMasks[num] & layerMask) != ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
			{
				flag = true;
				break;
			}
			if (!Grid.IsValidCell(num2) || (Grid.BuildMasks[num2] & layerMask) != ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
			{
				num4 = maxDistance - num3;
			}
			vector2I += direction;
			vector2I2 -= direction;
			num = Grid.XYToCell(vector2I.x, vector2I.y);
			num2 = Grid.XYToCell(vector2I2.x, vector2I2.y);
			num3++;
		}
		if (!flag && maxDistance % 2 == 0)
		{
			flag = (!Grid.IsValidCell(num2) || (Grid.BuildMasks[num2] & layerMask) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor));
		}
		hitDistance = (flag ? num3 : ((num4 > 0) ? num4 : maxDistance));
		return flag | hitDistance == num4;
	}

	// Token: 0x060040C2 RID: 16578 RVA: 0x00170BF9 File Offset: 0x0016EDF9
	public static int CellAbove(int cell)
	{
		return cell + Grid.WidthInCells;
	}

	// Token: 0x060040C3 RID: 16579 RVA: 0x00170C02 File Offset: 0x0016EE02
	public static int CellBelow(int cell)
	{
		return cell - Grid.WidthInCells;
	}

	// Token: 0x060040C4 RID: 16580 RVA: 0x00170C0B File Offset: 0x0016EE0B
	public static int CellLeft(int cell)
	{
		if (cell % Grid.WidthInCells <= 0)
		{
			return Grid.InvalidCell;
		}
		return cell - 1;
	}

	// Token: 0x060040C5 RID: 16581 RVA: 0x00170C20 File Offset: 0x0016EE20
	public static int CellRight(int cell)
	{
		if (cell % Grid.WidthInCells >= Grid.WidthInCells - 1)
		{
			return Grid.InvalidCell;
		}
		return cell + 1;
	}

	// Token: 0x060040C6 RID: 16582 RVA: 0x00170C3C File Offset: 0x0016EE3C
	public static CellOffset GetOffset(int cell)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		return new CellOffset(x, y);
	}

	// Token: 0x060040C7 RID: 16583 RVA: 0x00170C60 File Offset: 0x0016EE60
	public static int CellUpLeft(int cell)
	{
		int result = Grid.InvalidCell;
		if (cell < (Grid.HeightInCells - 1) * Grid.WidthInCells && cell % Grid.WidthInCells > 0)
		{
			result = cell - 1 + Grid.WidthInCells;
		}
		return result;
	}

	// Token: 0x060040C8 RID: 16584 RVA: 0x00170C98 File Offset: 0x0016EE98
	public static int CellUpRight(int cell)
	{
		int result = Grid.InvalidCell;
		if (cell < (Grid.HeightInCells - 1) * Grid.WidthInCells && cell % Grid.WidthInCells < Grid.WidthInCells - 1)
		{
			result = cell + 1 + Grid.WidthInCells;
		}
		return result;
	}

	// Token: 0x060040C9 RID: 16585 RVA: 0x00170CD8 File Offset: 0x0016EED8
	public static int CellDownLeft(int cell)
	{
		int result = Grid.InvalidCell;
		if (cell > Grid.WidthInCells && cell % Grid.WidthInCells > 0)
		{
			result = cell - 1 - Grid.WidthInCells;
		}
		return result;
	}

	// Token: 0x060040CA RID: 16586 RVA: 0x00170D08 File Offset: 0x0016EF08
	public static int CellDownRight(int cell)
	{
		int result = Grid.InvalidCell;
		if (cell >= Grid.WidthInCells && cell % Grid.WidthInCells < Grid.WidthInCells - 1)
		{
			result = cell + 1 - Grid.WidthInCells;
		}
		return result;
	}

	// Token: 0x060040CB RID: 16587 RVA: 0x00170D3E File Offset: 0x0016EF3E
	public static bool IsCellLeftOf(int cell, int other_cell)
	{
		return Grid.CellColumn(cell) < Grid.CellColumn(other_cell);
	}

	// Token: 0x060040CC RID: 16588 RVA: 0x00170D50 File Offset: 0x0016EF50
	public static bool IsCellOffsetOf(int cell, int target_cell, CellOffset[] target_offsets)
	{
		int num = target_offsets.Length;
		for (int i = 0; i < num; i++)
		{
			if (cell == Grid.OffsetCell(target_cell, target_offsets[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060040CD RID: 16589 RVA: 0x00170D80 File Offset: 0x0016EF80
	public static int GetCellDistance(int cell_a, int cell_b)
	{
		int num;
		int num2;
		Grid.CellToXY(cell_a, out num, out num2);
		int num3;
		int num4;
		Grid.CellToXY(cell_b, out num3, out num4);
		return Math.Abs(num - num3) + Math.Abs(num2 - num4);
	}

	// Token: 0x060040CE RID: 16590 RVA: 0x00170DB4 File Offset: 0x0016EFB4
	public static int GetCellRange(int cell_a, int cell_b)
	{
		int num;
		int num2;
		Grid.CellToXY(cell_a, out num, out num2);
		int num3;
		int num4;
		Grid.CellToXY(cell_b, out num3, out num4);
		return Math.Max(Math.Abs(num - num3), Math.Abs(num2 - num4));
	}

	// Token: 0x060040CF RID: 16591 RVA: 0x00170DEC File Offset: 0x0016EFEC
	public static CellOffset GetOffset(int base_cell, int offset_cell)
	{
		int num;
		int num2;
		Grid.CellToXY(base_cell, out num, out num2);
		int num3;
		int num4;
		Grid.CellToXY(offset_cell, out num3, out num4);
		return new CellOffset(num3 - num, num4 - num2);
	}

	// Token: 0x060040D0 RID: 16592 RVA: 0x00170E18 File Offset: 0x0016F018
	public static CellOffset GetCellOffsetDirection(int base_cell, int offset_cell)
	{
		CellOffset offset = Grid.GetOffset(base_cell, offset_cell);
		offset.x = Mathf.Clamp(offset.x, -1, 1);
		offset.y = Mathf.Clamp(offset.y, -1, 1);
		return offset;
	}

	// Token: 0x060040D1 RID: 16593 RVA: 0x00170E56 File Offset: 0x0016F056
	public static int OffsetCell(int cell, CellOffset offset)
	{
		return cell + offset.x + offset.y * Grid.WidthInCells;
	}

	// Token: 0x060040D2 RID: 16594 RVA: 0x00170E6D File Offset: 0x0016F06D
	public static int OffsetCell(int cell, int x, int y)
	{
		return cell + x + y * Grid.WidthInCells;
	}

	// Token: 0x060040D3 RID: 16595 RVA: 0x00170E7C File Offset: 0x0016F07C
	public static bool IsCellOffsetValid(int cell, int x, int y)
	{
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		return num + x >= 0 && num + x < Grid.WidthInCells && num2 + y >= 0 && num2 + y < Grid.HeightInCells;
	}

	// Token: 0x060040D4 RID: 16596 RVA: 0x00170EB7 File Offset: 0x0016F0B7
	public static bool IsCellOffsetValid(int cell, CellOffset offset)
	{
		return Grid.IsCellOffsetValid(cell, offset.x, offset.y);
	}

	// Token: 0x060040D5 RID: 16597 RVA: 0x00170ECB File Offset: 0x0016F0CB
	public static int PosToCell(StateMachine.Instance smi)
	{
		return Grid.PosToCell(smi.transform.GetPosition());
	}

	// Token: 0x060040D6 RID: 16598 RVA: 0x00170EDD File Offset: 0x0016F0DD
	public static int PosToCell(GameObject go)
	{
		return Grid.PosToCell(go.transform.GetPosition());
	}

	// Token: 0x060040D7 RID: 16599 RVA: 0x00170EEF File Offset: 0x0016F0EF
	public static int PosToCell(KMonoBehaviour cmp)
	{
		return Grid.PosToCell(cmp.transform.GetPosition());
	}

	// Token: 0x060040D8 RID: 16600 RVA: 0x00170F04 File Offset: 0x0016F104
	public static bool IsValidBuildingCell(int cell)
	{
		if (!Grid.IsWorldValidCell(cell))
		{
			return false;
		}
		WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cell]);
		if (world == null)
		{
			return false;
		}
		Vector2I vector2I = Grid.CellToXY(cell);
		return (float)vector2I.x >= world.minimumBounds.x && (float)vector2I.x <= world.maximumBounds.x && (float)vector2I.y >= world.minimumBounds.y && (float)vector2I.y <= world.maximumBounds.y - (float)Grid.TopBorderHeight;
	}

	// Token: 0x060040D9 RID: 16601 RVA: 0x00170F9B File Offset: 0x0016F19B
	public static bool IsWorldValidCell(int cell)
	{
		return Grid.IsValidCell(cell) && Grid.WorldIdx[cell] != byte.MaxValue;
	}

	// Token: 0x060040DA RID: 16602 RVA: 0x00170FB8 File Offset: 0x0016F1B8
	public static bool IsValidCell(int cell)
	{
		return cell >= 0 && cell < Grid.CellCount;
	}

	// Token: 0x060040DB RID: 16603 RVA: 0x00170FC8 File Offset: 0x0016F1C8
	public static bool IsValidCellInWorld(int cell, int world)
	{
		return cell >= 0 && cell < Grid.CellCount && (int)Grid.WorldIdx[cell] == world;
	}

	// Token: 0x060040DC RID: 16604 RVA: 0x00170FE2 File Offset: 0x0016F1E2
	public static bool IsActiveWorld(int cell)
	{
		return ClusterManager.Instance != null && ClusterManager.Instance.activeWorldId == (int)Grid.WorldIdx[cell];
	}

	// Token: 0x060040DD RID: 16605 RVA: 0x00171006 File Offset: 0x0016F206
	public static bool AreCellsInSameWorld(int cell, int world_cell)
	{
		return Grid.IsValidCell(cell) && Grid.IsValidCell(world_cell) && Grid.WorldIdx[cell] == Grid.WorldIdx[world_cell];
	}

	// Token: 0x060040DE RID: 16606 RVA: 0x0017102A File Offset: 0x0016F22A
	public static bool IsCellOpenToSpace(int cell)
	{
		return !Grid.IsSolidCell(cell) && !(Grid.Objects[cell, 2] != null) && global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space;
	}

	// Token: 0x060040DF RID: 16607 RVA: 0x00171060 File Offset: 0x0016F260
	public static int PosToCell(Vector2 pos)
	{
		float x = pos.x;
		int num = (int)(pos.y + 0.05f);
		int num2 = (int)x;
		return num * Grid.WidthInCells + num2;
	}

	// Token: 0x060040E0 RID: 16608 RVA: 0x0017108C File Offset: 0x0016F28C
	public static int PosToCell(Vector3 pos)
	{
		float x = pos.x;
		int num = (int)(pos.y + 0.05f);
		int num2 = (int)x;
		return num * Grid.WidthInCells + num2;
	}

	// Token: 0x060040E1 RID: 16609 RVA: 0x001710B8 File Offset: 0x0016F2B8
	public static void PosToXY(Vector3 pos, out int x, out int y)
	{
		Grid.CellToXY(Grid.PosToCell(pos), out x, out y);
	}

	// Token: 0x060040E2 RID: 16610 RVA: 0x001710C7 File Offset: 0x0016F2C7
	public static void PosToXY(Vector3 pos, out Vector2I xy)
	{
		Grid.CellToXY(Grid.PosToCell(pos), out xy.x, out xy.y);
	}

	// Token: 0x060040E3 RID: 16611 RVA: 0x001710E0 File Offset: 0x0016F2E0
	public static Vector2I PosToXY(Vector3 pos)
	{
		Vector2I result;
		Grid.CellToXY(Grid.PosToCell(pos), out result.x, out result.y);
		return result;
	}

	// Token: 0x060040E4 RID: 16612 RVA: 0x00171107 File Offset: 0x0016F307
	public static int XYToCell(int x, int y)
	{
		return x + y * Grid.WidthInCells;
	}

	// Token: 0x060040E5 RID: 16613 RVA: 0x00171112 File Offset: 0x0016F312
	public static void CellToXY(int cell, out int x, out int y)
	{
		x = Grid.CellColumn(cell);
		y = Grid.CellRow(cell);
	}

	// Token: 0x060040E6 RID: 16614 RVA: 0x00171124 File Offset: 0x0016F324
	public static Vector2I CellToXY(int cell)
	{
		return new Vector2I(Grid.CellColumn(cell), Grid.CellRow(cell));
	}

	// Token: 0x060040E7 RID: 16615 RVA: 0x00171138 File Offset: 0x0016F338
	public static Vector3 CellToPos(int cell, float x_offset, float y_offset, float z_offset)
	{
		int widthInCells = Grid.WidthInCells;
		float num = Grid.CellSizeInMeters * (float)(cell % widthInCells);
		float num2 = Grid.CellSizeInMeters * (float)(cell / widthInCells);
		return new Vector3(num + x_offset, num2 + y_offset, z_offset);
	}

	// Token: 0x060040E8 RID: 16616 RVA: 0x0017116C File Offset: 0x0016F36C
	public static Vector3 CellToPos(int cell)
	{
		int widthInCells = Grid.WidthInCells;
		float x = Grid.CellSizeInMeters * (float)(cell % widthInCells);
		float y = Grid.CellSizeInMeters * (float)(cell / widthInCells);
		return new Vector3(x, y, 0f);
	}

	// Token: 0x060040E9 RID: 16617 RVA: 0x001711A0 File Offset: 0x0016F3A0
	public static Vector3 CellToPos2D(int cell)
	{
		int widthInCells = Grid.WidthInCells;
		float x = Grid.CellSizeInMeters * (float)(cell % widthInCells);
		float y = Grid.CellSizeInMeters * (float)(cell / widthInCells);
		return new Vector2(x, y);
	}

	// Token: 0x060040EA RID: 16618 RVA: 0x001711D3 File Offset: 0x0016F3D3
	public static int CellRow(int cell)
	{
		return cell / Grid.WidthInCells;
	}

	// Token: 0x060040EB RID: 16619 RVA: 0x001711DC File Offset: 0x0016F3DC
	public static int CellColumn(int cell)
	{
		return cell % Grid.WidthInCells;
	}

	// Token: 0x060040EC RID: 16620 RVA: 0x001711E5 File Offset: 0x0016F3E5
	public static int ClampX(int x)
	{
		return Math.Min(Math.Max(x, 0), Grid.WidthInCells - 1);
	}

	// Token: 0x060040ED RID: 16621 RVA: 0x001711FA File Offset: 0x0016F3FA
	public static int ClampY(int y)
	{
		return Math.Min(Math.Max(y, 0), Grid.HeightInCells - 1);
	}

	// Token: 0x060040EE RID: 16622 RVA: 0x00171210 File Offset: 0x0016F410
	public static Vector2I Constrain(Vector2I val)
	{
		val.x = Mathf.Max(0, Mathf.Min(val.x, Grid.WidthInCells - 1));
		val.y = Mathf.Max(0, Mathf.Min(val.y, Grid.HeightInCells - 1));
		return val;
	}

	// Token: 0x060040EF RID: 16623 RVA: 0x0017125C File Offset: 0x0016F45C
	public static void Reveal(int cell, byte visibility = 255, bool forceReveal = false)
	{
		bool flag = Grid.Spawnable[cell] == 0 && visibility > 0;
		Grid.Spawnable[cell] = Math.Max(visibility, Grid.Visible[cell]);
		if (forceReveal || !Grid.PreventFogOfWarReveal[cell])
		{
			Grid.Visible[cell] = Math.Max(visibility, Grid.Visible[cell]);
		}
		if (flag && Grid.OnReveal != null)
		{
			Grid.OnReveal(cell);
		}
	}

	// Token: 0x060040F0 RID: 16624 RVA: 0x001712C5 File Offset: 0x0016F4C5
	public static ObjectLayer GetObjectLayerForConduitType(ConduitType conduit_type)
	{
		switch (conduit_type)
		{
		case ConduitType.Gas:
			return ObjectLayer.GasConduitConnection;
		case ConduitType.Liquid:
			return ObjectLayer.LiquidConduitConnection;
		case ConduitType.Solid:
			return ObjectLayer.SolidConduitConnection;
		default:
			throw new ArgumentException("Invalid value.", "conduit_type");
		}
	}

	// Token: 0x060040F1 RID: 16625 RVA: 0x001712F8 File Offset: 0x0016F4F8
	public static Vector3 CellToPos(int cell, CellAlignment alignment, Grid.SceneLayer layer)
	{
		switch (alignment)
		{
		case CellAlignment.Bottom:
			return Grid.CellToPosCBC(cell, layer);
		case CellAlignment.Top:
			return Grid.CellToPosCTC(cell, layer);
		case CellAlignment.Left:
			return Grid.CellToPosLCC(cell, layer);
		case CellAlignment.Right:
			return Grid.CellToPosRCC(cell, layer);
		case CellAlignment.RandomInternal:
		{
			Vector3 b = new Vector3(UnityEngine.Random.Range(-0.3f, 0.3f), 0f, 0f);
			return Grid.CellToPosCCC(cell, layer) + b;
		}
		}
		return Grid.CellToPosCCC(cell, layer);
	}

	// Token: 0x060040F2 RID: 16626 RVA: 0x0017137A File Offset: 0x0016F57A
	public static float GetLayerZ(Grid.SceneLayer layer)
	{
		return -Grid.HalfCellSizeInMeters - Grid.CellSizeInMeters * (float)layer * Grid.LayerMultiplier;
	}

	// Token: 0x060040F3 RID: 16627 RVA: 0x00171391 File Offset: 0x0016F591
	public static Vector3 CellToPosCCC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, Grid.HalfCellSizeInMeters, Grid.GetLayerZ(layer));
	}

	// Token: 0x060040F4 RID: 16628 RVA: 0x001713A9 File Offset: 0x0016F5A9
	public static Vector3 CellToPosCBC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, 0.01f, Grid.GetLayerZ(layer));
	}

	// Token: 0x060040F5 RID: 16629 RVA: 0x001713C1 File Offset: 0x0016F5C1
	public static Vector3 CellToPosCCF(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, Grid.HalfCellSizeInMeters, -Grid.CellSizeInMeters * (float)layer * Grid.LayerMultiplier);
	}

	// Token: 0x060040F6 RID: 16630 RVA: 0x001713E2 File Offset: 0x0016F5E2
	public static Vector3 CellToPosLCC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, 0.01f, Grid.HalfCellSizeInMeters, Grid.GetLayerZ(layer));
	}

	// Token: 0x060040F7 RID: 16631 RVA: 0x001713FA File Offset: 0x0016F5FA
	public static Vector3 CellToPosRCC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.CellSizeInMeters - 0.01f, Grid.HalfCellSizeInMeters, Grid.GetLayerZ(layer));
	}

	// Token: 0x060040F8 RID: 16632 RVA: 0x00171418 File Offset: 0x0016F618
	public static Vector3 CellToPosCTC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, Grid.CellSizeInMeters - 0.01f, Grid.GetLayerZ(layer));
	}

	// Token: 0x060040F9 RID: 16633 RVA: 0x00171436 File Offset: 0x0016F636
	public static bool IsSolidCell(int cell)
	{
		return Grid.IsValidCell(cell) && Grid.Solid[cell];
	}

	// Token: 0x060040FA RID: 16634 RVA: 0x00171450 File Offset: 0x0016F650
	public unsafe static bool IsSubstantialLiquid(int cell, float threshold = 0.35f)
	{
		if (Grid.IsValidCell(cell))
		{
			ushort num = Grid.elementIdx[cell];
			if ((int)num < ElementLoader.elements.Count)
			{
				Element element = ElementLoader.elements[(int)num];
				if (element.IsLiquid && Grid.mass[cell] >= element.defaultValues.mass * threshold)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060040FB RID: 16635 RVA: 0x001714B0 File Offset: 0x0016F6B0
	public static bool IsVisiblyInLiquid(Vector2 pos)
	{
		int num = Grid.PosToCell(pos);
		if (Grid.IsValidCell(num) && Grid.IsLiquid(num))
		{
			int cell = Grid.CellAbove(num);
			if (Grid.IsValidCell(cell) && Grid.IsLiquid(cell))
			{
				return true;
			}
			float num2 = Grid.Mass[num];
			float num3 = (float)((int)pos.y) - pos.y;
			if (num2 / 1000f <= num3)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060040FC RID: 16636 RVA: 0x00171514 File Offset: 0x0016F714
	public static bool IsNavigatableLiquid(int cell)
	{
		int num = Grid.CellAbove(cell);
		if (!Grid.IsValidCell(cell) || !Grid.IsValidCell(num))
		{
			return false;
		}
		if (Grid.IsSubstantialLiquid(cell, 0.35f))
		{
			return true;
		}
		if (Grid.IsLiquid(cell))
		{
			if (Grid.Element[num].IsLiquid)
			{
				return true;
			}
			if (Grid.Element[num].IsSolid)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060040FD RID: 16637 RVA: 0x00171572 File Offset: 0x0016F772
	public static bool IsLiquid(int cell)
	{
		return ElementLoader.elements[(int)Grid.ElementIdx[cell]].IsLiquid;
	}

	// Token: 0x060040FE RID: 16638 RVA: 0x00171593 File Offset: 0x0016F793
	public static bool IsGas(int cell)
	{
		return ElementLoader.elements[(int)Grid.ElementIdx[cell]].IsGas;
	}

	// Token: 0x060040FF RID: 16639 RVA: 0x001715B4 File Offset: 0x0016F7B4
	public static void GetVisibleExtents(out int min_x, out int min_y, out int max_x, out int max_y)
	{
		Vector3 vector = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 vector2 = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		min_y = (int)vector2.y;
		max_y = (int)(vector.y + 0.5f);
		min_x = (int)vector2.x;
		max_x = (int)(vector.x + 0.5f);
	}

	// Token: 0x06004100 RID: 16640 RVA: 0x0017164D File Offset: 0x0016F84D
	public static void GetVisibleExtents(out Vector2I min, out Vector2I max)
	{
		Grid.GetVisibleExtents(out min.x, out min.y, out max.x, out max.y);
	}

	// Token: 0x06004101 RID: 16641 RVA: 0x0017166C File Offset: 0x0016F86C
	public static void GetVisibleCellRangeInActiveWorld(out Vector2I min, out Vector2I max, int padding = 4, float rangeScale = 1.5f)
	{
		Grid.GetVisibleExtents(out min.x, out min.y, out max.x, out max.y);
		min.x -= padding;
		min.y -= padding;
		if (CameraController.Instance != null && DlcManager.IsExpansion1Active())
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			CameraController.Instance.GetWorldCamera(out vector2I, out vector2I2);
			min.x = Math.Min(vector2I.x + vector2I2.x - 1, Math.Max(vector2I.x, min.x));
			min.y = Math.Min(vector2I.y + vector2I2.y - 1, Math.Max(vector2I.y, min.y));
			max.x += padding;
			max.y += padding;
			max.x = Math.Min(vector2I.x + vector2I2.x - 1, Math.Max(vector2I.x, max.x));
			max.y = Math.Min(vector2I.y + vector2I2.y - 1 + 20, Math.Max(vector2I.y, max.y));
			return;
		}
		min.x = Math.Min((int)((float)Grid.WidthInCells * rangeScale) - 1, Math.Max(0, min.x));
		min.y = Math.Min((int)((float)Grid.HeightInCells * rangeScale) - 1, Math.Max(0, min.y));
		max.x += padding;
		max.y += padding;
		max.x = Math.Min((int)((float)Grid.WidthInCells * rangeScale) - 1, Math.Max(0, max.x));
		max.y = Math.Min((int)((float)Grid.HeightInCells * rangeScale) - 1, Math.Max(0, max.y));
	}

	// Token: 0x06004102 RID: 16642 RVA: 0x00171838 File Offset: 0x0016FA38
	public static Extents GetVisibleExtentsInActiveWorld(int padding = 4, float rangeScale = 1.5f)
	{
		Vector2I vector2I;
		Vector2I vector2I2;
		Grid.GetVisibleCellRangeInActiveWorld(out vector2I, out vector2I2, 4, 1.5f);
		return new Extents(vector2I.x, vector2I.y, vector2I2.x - vector2I.x, vector2I2.y - vector2I.y);
	}

	// Token: 0x06004103 RID: 16643 RVA: 0x0017187F File Offset: 0x0016FA7F
	public static bool IsVisible(int cell)
	{
		return Grid.Visible[cell] > 0 || !PropertyTextures.IsFogOfWarEnabled;
	}

	// Token: 0x06004104 RID: 16644 RVA: 0x00171895 File Offset: 0x0016FA95
	public static bool VisibleBlockingCB(int cell)
	{
		return !Grid.Transparent[cell] && Grid.IsSolidCell(cell);
	}

	// Token: 0x06004105 RID: 16645 RVA: 0x001718AC File Offset: 0x0016FAAC
	public static bool VisibilityTest(int x, int y, int x2, int y2, bool blocking_tile_visible = false)
	{
		return Grid.TestLineOfSight(x, y, x2, y2, Grid.VisibleBlockingDelegate, blocking_tile_visible, false);
	}

	// Token: 0x06004106 RID: 16646 RVA: 0x001718C0 File Offset: 0x0016FAC0
	public static bool VisibilityTest(int cell, int target_cell, bool blocking_tile_visible = false)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		int x2 = 0;
		int y2 = 0;
		Grid.CellToXY(target_cell, out x2, out y2);
		return Grid.VisibilityTest(x, y, x2, y2, blocking_tile_visible);
	}

	// Token: 0x06004107 RID: 16647 RVA: 0x001718F3 File Offset: 0x0016FAF3
	public static bool PhysicalBlockingCB(int cell)
	{
		return Grid.Solid[cell];
	}

	// Token: 0x06004108 RID: 16648 RVA: 0x00171900 File Offset: 0x0016FB00
	public static bool IsPhysicallyAccessible(int x, int y, int x2, int y2, bool blocking_tile_visible = false)
	{
		return Grid.FastTestLineOfSightSolid(x, y, x2, y2);
	}

	// Token: 0x06004109 RID: 16649 RVA: 0x0017190C File Offset: 0x0016FB0C
	public static void CollectCellsInLine(int startCell, int endCell, HashSet<int> outputCells)
	{
		int num = 2;
		int cellDistance = Grid.GetCellDistance(startCell, endCell);
		Vector2 a = (Grid.CellToPos(endCell) - Grid.CellToPos(startCell)).normalized;
		for (float num2 = 0f; num2 < (float)cellDistance; num2 = Mathf.Min(num2 + 1f / (float)num, (float)cellDistance))
		{
			int num3 = Grid.PosToCell(Grid.CellToPos(startCell) + a * num2);
			if (Grid.GetCellDistance(startCell, num3) <= cellDistance)
			{
				outputCells.Add(num3);
			}
		}
	}

	// Token: 0x0600410A RID: 16650 RVA: 0x00171998 File Offset: 0x0016FB98
	public static bool IsRangeExposedToSunlight(int cell, int scanRadius, CellOffset scanShape, out int cellsClear, int clearThreshold = 1)
	{
		cellsClear = 0;
		if (Grid.IsValidCell(cell) && (int)Grid.ExposedToSunlight[cell] >= clearThreshold)
		{
			cellsClear++;
		}
		bool flag = true;
		bool flag2 = true;
		int num = 1;
		while (num <= scanRadius && (flag || flag2))
		{
			int num2 = Grid.OffsetCell(cell, scanShape.x * num, scanShape.y * num);
			int num3 = Grid.OffsetCell(cell, -scanShape.x * num, scanShape.y * num);
			if (Grid.IsValidCell(num2) && (int)Grid.ExposedToSunlight[num2] >= clearThreshold)
			{
				cellsClear++;
			}
			if (Grid.IsValidCell(num3) && (int)Grid.ExposedToSunlight[num3] >= clearThreshold)
			{
				cellsClear++;
			}
			num++;
		}
		return cellsClear > 0;
	}

	// Token: 0x0600410B RID: 16651 RVA: 0x00171A4C File Offset: 0x0016FC4C
	public static bool FastTestLineOfSightSolid(int x, int y, int x2, int y2)
	{
		int value = x2 - x;
		int num = y2 - y;
		int num2 = 0;
		int num4;
		int num3 = num4 = Math.Sign(value);
		int num5 = Math.Sign(num);
		int num6 = Math.Abs(value);
		int num7 = Math.Abs(num);
		if (num6 <= num7)
		{
			num6 = Math.Abs(num);
			num7 = Math.Abs(value);
			if (num < 0)
			{
				num2 = -1;
			}
			else if (num > 0)
			{
				num2 = 1;
			}
			num4 = 0;
		}
		int num8 = num6 >> 1;
		int num9 = num3 + num5 * Grid.WidthInCells;
		int num10 = num4 + num2 * Grid.WidthInCells;
		int num11 = Grid.XYToCell(x, y);
		for (int i = 1; i < num6; i++)
		{
			num8 += num7;
			if (num8 < num6)
			{
				num11 += num10;
			}
			else
			{
				num8 -= num6;
				num11 += num9;
			}
			if (Grid.Solid[num11])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600410C RID: 16652 RVA: 0x00171B1C File Offset: 0x0016FD1C
	public static bool TestLineOfSightFixedBlockingVisible(int x, int y, int x2, int y2, Func<int, bool> blocking_cb, bool blocking_tile_visible, bool allow_invalid_cells = false)
	{
		int num = x;
		int num2 = y;
		int num3 = x2 - x;
		int num4 = y2 - y;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		if (num3 < 0)
		{
			num5 = -1;
		}
		else if (num3 > 0)
		{
			num5 = 1;
		}
		if (num4 < 0)
		{
			num6 = -1;
		}
		else if (num4 > 0)
		{
			num6 = 1;
		}
		if (num3 < 0)
		{
			num7 = -1;
		}
		else if (num3 > 0)
		{
			num7 = 1;
		}
		int num9 = Math.Abs(num3);
		int num10 = Math.Abs(num4);
		if (num9 <= num10)
		{
			num9 = Math.Abs(num4);
			num10 = Math.Abs(num3);
			if (num4 < 0)
			{
				num8 = -1;
			}
			else if (num4 > 0)
			{
				num8 = 1;
			}
			num7 = 0;
		}
		int num11 = num9 >> 1;
		for (int i = 0; i <= num9; i++)
		{
			int num12 = Grid.XYToCell(x, y);
			if (!allow_invalid_cells && !Grid.IsValidCell(num12))
			{
				return false;
			}
			bool flag = blocking_cb(num12);
			if ((x != num || y != num2) && flag)
			{
				return blocking_tile_visible && x == x2 && y == y2;
			}
			num11 += num10;
			if (num11 >= num9)
			{
				num11 -= num9;
				x += num5;
				y += num6;
			}
			else
			{
				x += num7;
				y += num8;
			}
		}
		return true;
	}

	// Token: 0x0600410D RID: 16653 RVA: 0x00171C38 File Offset: 0x0016FE38
	public static bool TestLineOfSight(int x, int y, int x2, int y2, Func<int, bool> blocking_cb, Func<int, bool> blocking_tile_visible_cb, bool allow_invalid_cells = false)
	{
		int num = x;
		int num2 = y;
		int num3 = x2 - x;
		int num4 = y2 - y;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		if (num3 < 0)
		{
			num5 = -1;
		}
		else if (num3 > 0)
		{
			num5 = 1;
		}
		if (num4 < 0)
		{
			num6 = -1;
		}
		else if (num4 > 0)
		{
			num6 = 1;
		}
		if (num3 < 0)
		{
			num7 = -1;
		}
		else if (num3 > 0)
		{
			num7 = 1;
		}
		int num9 = Math.Abs(num3);
		int num10 = Math.Abs(num4);
		if (num9 <= num10)
		{
			num9 = Math.Abs(num4);
			num10 = Math.Abs(num3);
			if (num4 < 0)
			{
				num8 = -1;
			}
			else if (num4 > 0)
			{
				num8 = 1;
			}
			num7 = 0;
		}
		int num11 = num9 >> 1;
		for (int i = 0; i <= num9; i++)
		{
			int num12 = Grid.XYToCell(x, y);
			if (!allow_invalid_cells && !Grid.IsValidCell(num12))
			{
				return false;
			}
			bool flag = blocking_cb(num12);
			if ((x != num || y != num2) && flag)
			{
				return blocking_tile_visible_cb(num12) && x == x2 && y == y2;
			}
			num11 += num10;
			if (num11 >= num9)
			{
				num11 -= num9;
				x += num5;
				y += num6;
			}
			else
			{
				x += num7;
				y += num8;
			}
		}
		return true;
	}

	// Token: 0x0600410E RID: 16654 RVA: 0x00171D5F File Offset: 0x0016FF5F
	public static bool TestLineOfSight(int x, int y, int x2, int y2, Func<int, bool> blocking_cb, bool blocking_tile_visible = false, bool allow_invalid_cells = false)
	{
		return Grid.TestLineOfSightFixedBlockingVisible(x, y, x2, y2, blocking_cb, blocking_tile_visible, allow_invalid_cells);
	}

	// Token: 0x0600410F RID: 16655 RVA: 0x00171D70 File Offset: 0x0016FF70
	public static bool GetFreeGridSpace(Vector2I size, out Vector2I offset)
	{
		Vector2I gridOffset = BestFit.GetGridOffset(ClusterManager.Instance.WorldContainers, size, out offset);
		if (gridOffset.X <= Grid.WidthInCells && gridOffset.Y <= Grid.HeightInCells)
		{
			SimMessages.SimDataResizeGridAndInitializeVacuumCells(gridOffset, size.x, size.y, offset.x, offset.y);
			Game.Instance.roomProber.Refresh();
			return true;
		}
		return false;
	}

	// Token: 0x06004110 RID: 16656 RVA: 0x00171DDC File Offset: 0x0016FFDC
	public static void FreeGridSpace(Vector2I size, Vector2I offset)
	{
		SimMessages.SimDataFreeCells(size.x, size.y, offset.x, offset.y);
		for (int i = offset.y; i < size.y + offset.y + 1; i++)
		{
			for (int j = offset.x - 1; j < size.x + offset.x + 1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (Grid.IsValidCell(num))
				{
					Grid.Element[num] = ElementLoader.FindElementByHash(SimHashes.Vacuum);
				}
			}
		}
		Game.Instance.roomProber.Refresh();
	}

	// Token: 0x06004111 RID: 16657 RVA: 0x00171E76 File Offset: 0x00170076
	[Conditional("UNITY_EDITOR")]
	public static void DrawBoxOnCell(int cell, Color color, float offset = 0f)
	{
		Grid.CellToPos(cell) + new Vector3(0.5f, 0.5f, 0f);
	}

	// Token: 0x04002AE4 RID: 10980
	public static readonly CellOffset[] DefaultOffset = new CellOffset[1];

	// Token: 0x04002AE5 RID: 10981
	public static float WidthInMeters;

	// Token: 0x04002AE6 RID: 10982
	public static float HeightInMeters;

	// Token: 0x04002AE7 RID: 10983
	public static int WidthInCells;

	// Token: 0x04002AE8 RID: 10984
	public static int HeightInCells;

	// Token: 0x04002AE9 RID: 10985
	public static float CellSizeInMeters;

	// Token: 0x04002AEA RID: 10986
	public static float InverseCellSizeInMeters;

	// Token: 0x04002AEB RID: 10987
	public static float HalfCellSizeInMeters;

	// Token: 0x04002AEC RID: 10988
	public static int CellCount;

	// Token: 0x04002AED RID: 10989
	public static int InvalidCell = -1;

	// Token: 0x04002AEE RID: 10990
	public static int TopBorderHeight = 2;

	// Token: 0x04002AEF RID: 10991
	public static Dictionary<int, GameObject>[] ObjectLayers;

	// Token: 0x04002AF0 RID: 10992
	public static Action<int> OnReveal;

	// Token: 0x04002AF1 RID: 10993
	public static Grid.BuildFlags[] BuildMasks;

	// Token: 0x04002AF2 RID: 10994
	public static Grid.BuildFlagsFoundationIndexer Foundation;

	// Token: 0x04002AF3 RID: 10995
	public static Grid.BuildFlagsSolidIndexer Solid;

	// Token: 0x04002AF4 RID: 10996
	public static Grid.BuildFlagsDupeImpassableIndexer DupeImpassable;

	// Token: 0x04002AF5 RID: 10997
	public static Grid.BuildFlagsFakeFloorIndexer FakeFloor;

	// Token: 0x04002AF6 RID: 10998
	public static Grid.BuildFlagsDupePassableIndexer DupePassable;

	// Token: 0x04002AF7 RID: 10999
	public static Grid.BuildFlagsImpassableIndexer CritterImpassable;

	// Token: 0x04002AF8 RID: 11000
	public static Grid.BuildFlagsDoorIndexer HasDoor;

	// Token: 0x04002AF9 RID: 11001
	public static Grid.VisFlags[] VisMasks;

	// Token: 0x04002AFA RID: 11002
	public static Grid.VisFlagsRevealedIndexer Revealed;

	// Token: 0x04002AFB RID: 11003
	public static Grid.VisFlagsPreventFogOfWarRevealIndexer PreventFogOfWarReveal;

	// Token: 0x04002AFC RID: 11004
	public static Grid.VisFlagsRenderedByWorldIndexer RenderedByWorld;

	// Token: 0x04002AFD RID: 11005
	public static Grid.VisFlagsAllowPathfindingIndexer AllowPathfinding;

	// Token: 0x04002AFE RID: 11006
	public static Grid.NavValidatorFlags[] NavValidatorMasks;

	// Token: 0x04002AFF RID: 11007
	public static Grid.NavValidatorFlagsLadderIndexer HasLadder;

	// Token: 0x04002B00 RID: 11008
	public static Grid.NavValidatorFlagsPoleIndexer HasPole;

	// Token: 0x04002B01 RID: 11009
	public static Grid.NavValidatorFlagsTubeIndexer HasTube;

	// Token: 0x04002B02 RID: 11010
	public static Grid.NavValidatorFlagsNavTeleporterIndexer HasNavTeleporter;

	// Token: 0x04002B03 RID: 11011
	public static Grid.NavValidatorFlagsUnderConstructionIndexer IsTileUnderConstruction;

	// Token: 0x04002B04 RID: 11012
	public static Grid.NavFlags[] NavMasks;

	// Token: 0x04002B05 RID: 11013
	private static Grid.NavFlagsAccessDoorIndexer HasAccessDoor;

	// Token: 0x04002B06 RID: 11014
	public static Grid.NavFlagsTubeEntranceIndexer HasTubeEntrance;

	// Token: 0x04002B07 RID: 11015
	public static Grid.NavFlagsPreventIdleTraversalIndexer PreventIdleTraversal;

	// Token: 0x04002B08 RID: 11016
	public static Grid.NavFlagsReservedIndexer Reserved;

	// Token: 0x04002B09 RID: 11017
	public static Grid.NavFlagsSuitMarkerIndexer HasSuitMarker;

	// Token: 0x04002B0A RID: 11018
	private static Dictionary<int, Grid.Restriction> restrictions = new Dictionary<int, Grid.Restriction>();

	// Token: 0x04002B0B RID: 11019
	private static Dictionary<int, Grid.TubeEntrance> tubeEntrances = new Dictionary<int, Grid.TubeEntrance>();

	// Token: 0x04002B0C RID: 11020
	private static Dictionary<int, Grid.SuitMarker> suitMarkers = new Dictionary<int, Grid.SuitMarker>();

	// Token: 0x04002B0D RID: 11021
	public unsafe static ushort* elementIdx;

	// Token: 0x04002B0E RID: 11022
	public unsafe static float* temperature;

	// Token: 0x04002B0F RID: 11023
	public unsafe static float* radiation;

	// Token: 0x04002B10 RID: 11024
	public unsafe static float* mass;

	// Token: 0x04002B11 RID: 11025
	public unsafe static byte* properties;

	// Token: 0x04002B12 RID: 11026
	public unsafe static byte* strengthInfo;

	// Token: 0x04002B13 RID: 11027
	public unsafe static byte* insulation;

	// Token: 0x04002B14 RID: 11028
	public unsafe static byte* diseaseIdx;

	// Token: 0x04002B15 RID: 11029
	public unsafe static int* diseaseCount;

	// Token: 0x04002B16 RID: 11030
	public unsafe static byte* exposedToSunlight;

	// Token: 0x04002B17 RID: 11031
	public unsafe static float* AccumulatedFlowValues = null;

	// Token: 0x04002B18 RID: 11032
	public static byte[] Visible;

	// Token: 0x04002B19 RID: 11033
	public static byte[] Spawnable;

	// Token: 0x04002B1A RID: 11034
	public static float[] Damage;

	// Token: 0x04002B1B RID: 11035
	public static float[] Decor;

	// Token: 0x04002B1C RID: 11036
	public static bool[] GravitasFacility;

	// Token: 0x04002B1D RID: 11037
	public static byte[] WorldIdx;

	// Token: 0x04002B1E RID: 11038
	public static float[] Loudness;

	// Token: 0x04002B1F RID: 11039
	public static Element[] Element;

	// Token: 0x04002B20 RID: 11040
	public static int[] LightCount;

	// Token: 0x04002B21 RID: 11041
	public static Grid.PressureIndexer Pressure;

	// Token: 0x04002B22 RID: 11042
	public static Grid.TransparentIndexer Transparent;

	// Token: 0x04002B23 RID: 11043
	public static Grid.ElementIdxIndexer ElementIdx;

	// Token: 0x04002B24 RID: 11044
	public static Grid.TemperatureIndexer Temperature;

	// Token: 0x04002B25 RID: 11045
	public static Grid.RadiationIndexer Radiation;

	// Token: 0x04002B26 RID: 11046
	public static Grid.MassIndexer Mass;

	// Token: 0x04002B27 RID: 11047
	public static Grid.PropertiesIndexer Properties;

	// Token: 0x04002B28 RID: 11048
	public static Grid.ExposedToSunlightIndexer ExposedToSunlight;

	// Token: 0x04002B29 RID: 11049
	public static Grid.StrengthInfoIndexer StrengthInfo;

	// Token: 0x04002B2A RID: 11050
	public static Grid.Insulationndexer Insulation;

	// Token: 0x04002B2B RID: 11051
	public static Grid.DiseaseIdxIndexer DiseaseIdx;

	// Token: 0x04002B2C RID: 11052
	public static Grid.DiseaseCountIndexer DiseaseCount;

	// Token: 0x04002B2D RID: 11053
	public static Grid.LightIntensityIndexer LightIntensity;

	// Token: 0x04002B2E RID: 11054
	public static Grid.AccumulatedFlowIndexer AccumulatedFlow;

	// Token: 0x04002B2F RID: 11055
	public static Grid.ObjectLayerIndexer Objects;

	// Token: 0x04002B30 RID: 11056
	public static float LayerMultiplier = 1f;

	// Token: 0x04002B31 RID: 11057
	private static readonly Func<int, bool> VisibleBlockingDelegate = (int cell) => Grid.VisibleBlockingCB(cell);

	// Token: 0x04002B32 RID: 11058
	private static readonly Func<int, bool> PhysicalBlockingDelegate = (int cell) => Grid.PhysicalBlockingCB(cell);

	// Token: 0x02001820 RID: 6176
	[Flags]
	public enum BuildFlags : byte
	{
		// Token: 0x0400752C RID: 29996
		Solid = 1,
		// Token: 0x0400752D RID: 29997
		Foundation = 2,
		// Token: 0x0400752E RID: 29998
		Door = 4,
		// Token: 0x0400752F RID: 29999
		DupePassable = 8,
		// Token: 0x04007530 RID: 30000
		DupeImpassable = 16,
		// Token: 0x04007531 RID: 30001
		CritterImpassable = 32,
		// Token: 0x04007532 RID: 30002
		FakeFloor = 192,
		// Token: 0x04007533 RID: 30003
		Any = 255
	}

	// Token: 0x02001821 RID: 6177
	public struct BuildFlagsFoundationIndexer
	{
		// Token: 0x17000A51 RID: 2641
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.Foundation) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.Foundation, value);
			}
		}
	}

	// Token: 0x02001822 RID: 6178
	public struct BuildFlagsSolidIndexer
	{
		// Token: 0x17000A52 RID: 2642
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.Solid) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
		}
	}

	// Token: 0x02001823 RID: 6179
	public struct BuildFlagsDupeImpassableIndexer
	{
		// Token: 0x17000A53 RID: 2643
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.DupeImpassable) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.DupeImpassable, value);
			}
		}
	}

	// Token: 0x02001824 RID: 6180
	public struct BuildFlagsFakeFloorIndexer
	{
		// Token: 0x17000A54 RID: 2644
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.FakeFloor) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
		}

		// Token: 0x06009798 RID: 38808 RVA: 0x00365E80 File Offset: 0x00364080
		public void Add(int i)
		{
			Grid.BuildFlags buildFlags = Grid.BuildMasks[i];
			int num = (int)(((buildFlags & Grid.BuildFlags.FakeFloor) >> 6) + 1);
			num = Math.Min(num, 3);
			Grid.BuildMasks[i] = ((buildFlags & ~Grid.BuildFlags.FakeFloor) | ((Grid.BuildFlags)(num << 6) & Grid.BuildFlags.FakeFloor));
		}

		// Token: 0x06009799 RID: 38809 RVA: 0x00365EC0 File Offset: 0x003640C0
		public void Remove(int i)
		{
			Grid.BuildFlags buildFlags = Grid.BuildMasks[i];
			int num = (int)(((buildFlags & Grid.BuildFlags.FakeFloor) >> 6) - Grid.BuildFlags.Solid);
			num = Math.Max(num, 0);
			Grid.BuildMasks[i] = ((buildFlags & ~Grid.BuildFlags.FakeFloor) | ((Grid.BuildFlags)(num << 6) & Grid.BuildFlags.FakeFloor));
		}
	}

	// Token: 0x02001825 RID: 6181
	public struct BuildFlagsDupePassableIndexer
	{
		// Token: 0x17000A55 RID: 2645
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.DupePassable) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.DupePassable, value);
			}
		}
	}

	// Token: 0x02001826 RID: 6182
	public struct BuildFlagsImpassableIndexer
	{
		// Token: 0x17000A56 RID: 2646
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.CritterImpassable) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.CritterImpassable, value);
			}
		}
	}

	// Token: 0x02001827 RID: 6183
	public struct BuildFlagsDoorIndexer
	{
		// Token: 0x17000A57 RID: 2647
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.Door) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.Door, value);
			}
		}
	}

	// Token: 0x02001828 RID: 6184
	[Flags]
	public enum VisFlags : byte
	{
		// Token: 0x04007535 RID: 30005
		Revealed = 1,
		// Token: 0x04007536 RID: 30006
		PreventFogOfWarReveal = 2,
		// Token: 0x04007537 RID: 30007
		RenderedByWorld = 4,
		// Token: 0x04007538 RID: 30008
		AllowPathfinding = 8
	}

	// Token: 0x02001829 RID: 6185
	public struct VisFlagsRevealedIndexer
	{
		// Token: 0x17000A58 RID: 2648
		public bool this[int i]
		{
			get
			{
				return (Grid.VisMasks[i] & Grid.VisFlags.Revealed) > (Grid.VisFlags)0;
			}
			set
			{
				Grid.UpdateVisMask(i, Grid.VisFlags.Revealed, value);
			}
		}
	}

	// Token: 0x0200182A RID: 6186
	public struct VisFlagsPreventFogOfWarRevealIndexer
	{
		// Token: 0x17000A59 RID: 2649
		public bool this[int i]
		{
			get
			{
				return (Grid.VisMasks[i] & Grid.VisFlags.PreventFogOfWarReveal) > (Grid.VisFlags)0;
			}
			set
			{
				Grid.UpdateVisMask(i, Grid.VisFlags.PreventFogOfWarReveal, value);
			}
		}
	}

	// Token: 0x0200182B RID: 6187
	public struct VisFlagsRenderedByWorldIndexer
	{
		// Token: 0x17000A5A RID: 2650
		public bool this[int i]
		{
			get
			{
				return (Grid.VisMasks[i] & Grid.VisFlags.RenderedByWorld) > (Grid.VisFlags)0;
			}
			set
			{
				Grid.UpdateVisMask(i, Grid.VisFlags.RenderedByWorld, value);
			}
		}
	}

	// Token: 0x0200182C RID: 6188
	public struct VisFlagsAllowPathfindingIndexer
	{
		// Token: 0x17000A5B RID: 2651
		public bool this[int i]
		{
			get
			{
				return (Grid.VisMasks[i] & Grid.VisFlags.AllowPathfinding) > (Grid.VisFlags)0;
			}
			set
			{
				Grid.UpdateVisMask(i, Grid.VisFlags.AllowPathfinding, value);
			}
		}
	}

	// Token: 0x0200182D RID: 6189
	[Flags]
	public enum NavValidatorFlags : byte
	{
		// Token: 0x0400753A RID: 30010
		Ladder = 1,
		// Token: 0x0400753B RID: 30011
		Pole = 2,
		// Token: 0x0400753C RID: 30012
		Tube = 4,
		// Token: 0x0400753D RID: 30013
		NavTeleporter = 8,
		// Token: 0x0400753E RID: 30014
		UnderConstruction = 16
	}

	// Token: 0x0200182E RID: 6190
	public struct NavValidatorFlagsLadderIndexer
	{
		// Token: 0x17000A5C RID: 2652
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.Ladder) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.Ladder, value);
			}
		}
	}

	// Token: 0x0200182F RID: 6191
	public struct NavValidatorFlagsPoleIndexer
	{
		// Token: 0x17000A5D RID: 2653
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.Pole) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.Pole, value);
			}
		}
	}

	// Token: 0x02001830 RID: 6192
	public struct NavValidatorFlagsTubeIndexer
	{
		// Token: 0x17000A5E RID: 2654
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.Tube) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.Tube, value);
			}
		}
	}

	// Token: 0x02001831 RID: 6193
	public struct NavValidatorFlagsNavTeleporterIndexer
	{
		// Token: 0x17000A5F RID: 2655
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.NavTeleporter) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.NavTeleporter, value);
			}
		}
	}

	// Token: 0x02001832 RID: 6194
	public struct NavValidatorFlagsUnderConstructionIndexer
	{
		// Token: 0x17000A60 RID: 2656
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.UnderConstruction) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.UnderConstruction, value);
			}
		}
	}

	// Token: 0x02001833 RID: 6195
	[Flags]
	public enum NavFlags : byte
	{
		// Token: 0x04007540 RID: 30016
		AccessDoor = 1,
		// Token: 0x04007541 RID: 30017
		TubeEntrance = 2,
		// Token: 0x04007542 RID: 30018
		PreventIdleTraversal = 4,
		// Token: 0x04007543 RID: 30019
		Reserved = 8,
		// Token: 0x04007544 RID: 30020
		SuitMarker = 16
	}

	// Token: 0x02001834 RID: 6196
	public struct NavFlagsAccessDoorIndexer
	{
		// Token: 0x17000A61 RID: 2657
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.AccessDoor) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.AccessDoor, value);
			}
		}
	}

	// Token: 0x02001835 RID: 6197
	public struct NavFlagsTubeEntranceIndexer
	{
		// Token: 0x17000A62 RID: 2658
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.TubeEntrance) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.TubeEntrance, value);
			}
		}
	}

	// Token: 0x02001836 RID: 6198
	public struct NavFlagsPreventIdleTraversalIndexer
	{
		// Token: 0x17000A63 RID: 2659
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.PreventIdleTraversal) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.PreventIdleTraversal, value);
			}
		}
	}

	// Token: 0x02001837 RID: 6199
	public struct NavFlagsReservedIndexer
	{
		// Token: 0x17000A64 RID: 2660
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.Reserved) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.Reserved, value);
			}
		}
	}

	// Token: 0x02001838 RID: 6200
	public struct NavFlagsSuitMarkerIndexer
	{
		// Token: 0x17000A65 RID: 2661
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.SuitMarker) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.SuitMarker, value);
			}
		}
	}

	// Token: 0x02001839 RID: 6201
	public struct Restriction
	{
		// Token: 0x04007545 RID: 30021
		public const int DefaultID = -1;

		// Token: 0x04007546 RID: 30022
		public Dictionary<int, Grid.Restriction.Directions> DirectionMasksForMinionInstanceID;

		// Token: 0x04007547 RID: 30023
		public Grid.Restriction.Orientation orientation;

		// Token: 0x020025A0 RID: 9632
		[Flags]
		public enum Directions : byte
		{
			// Token: 0x0400A793 RID: 42899
			Left = 1,
			// Token: 0x0400A794 RID: 42900
			Right = 2,
			// Token: 0x0400A795 RID: 42901
			Teleport = 4
		}

		// Token: 0x020025A1 RID: 9633
		public enum Orientation : byte
		{
			// Token: 0x0400A797 RID: 42903
			Vertical,
			// Token: 0x0400A798 RID: 42904
			Horizontal,
			// Token: 0x0400A799 RID: 42905
			SingleCell
		}
	}

	// Token: 0x0200183A RID: 6202
	private struct TubeEntrance
	{
		// Token: 0x04007548 RID: 30024
		public bool operational;

		// Token: 0x04007549 RID: 30025
		public int reservationCapacity;

		// Token: 0x0400754A RID: 30026
		public HashSet<int> reservedInstanceIDs;
	}

	// Token: 0x0200183B RID: 6203
	public struct SuitMarker
	{
		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x060097BC RID: 38844 RVA: 0x0036609D File Offset: 0x0036429D
		public int emptyLockerCount
		{
			get
			{
				return this.lockerCount - this.suitCount;
			}
		}

		// Token: 0x0400754B RID: 30027
		public int suitCount;

		// Token: 0x0400754C RID: 30028
		public int lockerCount;

		// Token: 0x0400754D RID: 30029
		public Grid.SuitMarker.Flags flags;

		// Token: 0x0400754E RID: 30030
		public PathFinder.PotentialPath.Flags pathFlags;

		// Token: 0x0400754F RID: 30031
		public HashSet<int> minionIDsWithSuitReservations;

		// Token: 0x04007550 RID: 30032
		public HashSet<int> minionIDsWithEmptyLockerReservations;

		// Token: 0x020025A2 RID: 9634
		[Flags]
		public enum Flags : byte
		{
			// Token: 0x0400A79B RID: 42907
			OnlyTraverseIfUnequipAvailable = 1,
			// Token: 0x0400A79C RID: 42908
			Operational = 2,
			// Token: 0x0400A79D RID: 42909
			Rotated = 4
		}
	}

	// Token: 0x0200183C RID: 6204
	public struct ObjectLayerIndexer
	{
		// Token: 0x17000A67 RID: 2663
		public GameObject this[int cell, int layer]
		{
			get
			{
				GameObject result = null;
				Grid.ObjectLayers[layer].TryGetValue(cell, out result);
				return result;
			}
			set
			{
				if (value == null)
				{
					Grid.ObjectLayers[layer].Remove(cell);
				}
				else
				{
					Grid.ObjectLayers[layer][cell] = value;
				}
				GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.objectLayers[layer], value);
			}
		}
	}

	// Token: 0x0200183D RID: 6205
	public struct PressureIndexer
	{
		// Token: 0x17000A68 RID: 2664
		public unsafe float this[int i]
		{
			get
			{
				return Grid.mass[i] * 101.3f;
			}
		}
	}

	// Token: 0x0200183E RID: 6206
	public struct TransparentIndexer
	{
		// Token: 0x17000A69 RID: 2665
		public unsafe bool this[int i]
		{
			get
			{
				return (Grid.properties[i] & 16) > 0;
			}
		}
	}

	// Token: 0x0200183F RID: 6207
	public struct ElementIdxIndexer
	{
		// Token: 0x17000A6A RID: 2666
		public unsafe ushort this[int i]
		{
			get
			{
				return Grid.elementIdx[i];
			}
		}
	}

	// Token: 0x02001840 RID: 6208
	public struct TemperatureIndexer
	{
		// Token: 0x17000A6B RID: 2667
		public unsafe float this[int i]
		{
			get
			{
				return Grid.temperature[i];
			}
		}
	}

	// Token: 0x02001841 RID: 6209
	public struct RadiationIndexer
	{
		// Token: 0x17000A6C RID: 2668
		public unsafe float this[int i]
		{
			get
			{
				return Grid.radiation[i];
			}
		}
	}

	// Token: 0x02001842 RID: 6210
	public struct MassIndexer
	{
		// Token: 0x17000A6D RID: 2669
		public unsafe float this[int i]
		{
			get
			{
				return Grid.mass[i];
			}
		}
	}

	// Token: 0x02001843 RID: 6211
	public struct PropertiesIndexer
	{
		// Token: 0x17000A6E RID: 2670
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.properties[i];
			}
		}
	}

	// Token: 0x02001844 RID: 6212
	public struct ExposedToSunlightIndexer
	{
		// Token: 0x17000A6F RID: 2671
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.exposedToSunlight[i];
			}
		}
	}

	// Token: 0x02001845 RID: 6213
	public struct StrengthInfoIndexer
	{
		// Token: 0x17000A70 RID: 2672
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.strengthInfo[i];
			}
		}
	}

	// Token: 0x02001846 RID: 6214
	public struct Insulationndexer
	{
		// Token: 0x17000A71 RID: 2673
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.insulation[i];
			}
		}
	}

	// Token: 0x02001847 RID: 6215
	public struct DiseaseIdxIndexer
	{
		// Token: 0x17000A72 RID: 2674
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.diseaseIdx[i];
			}
		}
	}

	// Token: 0x02001848 RID: 6216
	public struct DiseaseCountIndexer
	{
		// Token: 0x17000A73 RID: 2675
		public unsafe int this[int i]
		{
			get
			{
				return Grid.diseaseCount[i];
			}
		}
	}

	// Token: 0x02001849 RID: 6217
	public struct AccumulatedFlowIndexer
	{
		// Token: 0x17000A74 RID: 2676
		public unsafe float this[int i]
		{
			get
			{
				return Grid.AccumulatedFlowValues[i];
			}
		}
	}

	// Token: 0x0200184A RID: 6218
	public struct LightIntensityIndexer
	{
		// Token: 0x17000A75 RID: 2677
		public unsafe int this[int i]
		{
			get
			{
				float num = Game.Instance.currentFallbackSunlightIntensity;
				WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[i]);
				if (world != null)
				{
					num = world.currentSunlightIntensity;
				}
				int num2 = (int)((float)Grid.exposedToSunlight[i] / 255f * num);
				int num3 = Grid.LightCount[i];
				return num2 + num3;
			}
		}
	}

	// Token: 0x0200184B RID: 6219
	public enum SceneLayer
	{
		// Token: 0x04007552 RID: 30034
		WorldSelection = -3,
		// Token: 0x04007553 RID: 30035
		NoLayer,
		// Token: 0x04007554 RID: 30036
		Background,
		// Token: 0x04007555 RID: 30037
		Backwall = 1,
		// Token: 0x04007556 RID: 30038
		Gas,
		// Token: 0x04007557 RID: 30039
		GasConduits,
		// Token: 0x04007558 RID: 30040
		GasConduitBridges,
		// Token: 0x04007559 RID: 30041
		LiquidConduits,
		// Token: 0x0400755A RID: 30042
		LiquidConduitBridges,
		// Token: 0x0400755B RID: 30043
		SolidConduits,
		// Token: 0x0400755C RID: 30044
		SolidConduitContents,
		// Token: 0x0400755D RID: 30045
		SolidConduitBridges,
		// Token: 0x0400755E RID: 30046
		Wires,
		// Token: 0x0400755F RID: 30047
		WireBridges,
		// Token: 0x04007560 RID: 30048
		WireBridgesFront,
		// Token: 0x04007561 RID: 30049
		LogicWires,
		// Token: 0x04007562 RID: 30050
		LogicGates,
		// Token: 0x04007563 RID: 30051
		LogicGatesFront,
		// Token: 0x04007564 RID: 30052
		InteriorWall,
		// Token: 0x04007565 RID: 30053
		GasFront,
		// Token: 0x04007566 RID: 30054
		BuildingBack,
		// Token: 0x04007567 RID: 30055
		Building,
		// Token: 0x04007568 RID: 30056
		BuildingUse,
		// Token: 0x04007569 RID: 30057
		BuildingFront,
		// Token: 0x0400756A RID: 30058
		TransferArm,
		// Token: 0x0400756B RID: 30059
		Ore,
		// Token: 0x0400756C RID: 30060
		Creatures,
		// Token: 0x0400756D RID: 30061
		Move,
		// Token: 0x0400756E RID: 30062
		Front,
		// Token: 0x0400756F RID: 30063
		GlassTile,
		// Token: 0x04007570 RID: 30064
		Liquid,
		// Token: 0x04007571 RID: 30065
		Ground,
		// Token: 0x04007572 RID: 30066
		TileMain,
		// Token: 0x04007573 RID: 30067
		TileFront,
		// Token: 0x04007574 RID: 30068
		FXFront,
		// Token: 0x04007575 RID: 30069
		FXFront2,
		// Token: 0x04007576 RID: 30070
		SceneMAX
	}
}
