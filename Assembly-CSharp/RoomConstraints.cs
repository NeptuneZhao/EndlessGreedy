﻿using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;

// Token: 0x02000A6E RID: 2670
public static class RoomConstraints
{
	// Token: 0x06004DB8 RID: 19896 RVA: 0x001BD79F File Offset: 0x001BB99F
	public static Tag AddAndReturn(this List<Tag> list, Tag tag)
	{
		list.Add(tag);
		return tag;
	}

	// Token: 0x06004DB9 RID: 19897 RVA: 0x001BD7AC File Offset: 0x001BB9AC
	public static string RoomCriteriaString(Room room)
	{
		string text = "";
		RoomType roomType = room.roomType;
		if (roomType != Db.Get().RoomTypes.Neutral)
		{
			text = text + "<b>" + ROOMS.CRITERIA.HEADER + "</b>";
			text = text + "\n    • " + roomType.primary_constraint.name;
			if (roomType.additional_constraints != null)
			{
				foreach (RoomConstraints.Constraint constraint in roomType.additional_constraints)
				{
					if (constraint.isSatisfied(room))
					{
						text = text + "\n    • " + constraint.name;
					}
					else
					{
						text = text + "\n<color=#F44A47FF>    • " + constraint.name + "</color>";
					}
				}
			}
			return text;
		}
		RoomTypes.RoomTypeQueryResult[] possibleRoomTypes = Db.Get().RoomTypes.GetPossibleRoomTypes(room);
		text += ((possibleRoomTypes.Length > 1) ? ("<b>" + ROOMS.CRITERIA.POSSIBLE_TYPES_HEADER + "</b>") : "");
		foreach (RoomTypes.RoomTypeQueryResult roomTypeQueryResult in possibleRoomTypes)
		{
			RoomType type = roomTypeQueryResult.Type;
			if (type != Db.Get().RoomTypes.Neutral)
			{
				if (text != "")
				{
					text += "\n";
				}
				text = string.Concat(new string[]
				{
					text,
					"<b><color=#BCBCBC>    • ",
					type.Name,
					"</b> (",
					type.primary_constraint.conflictDescription,
					")</color>"
				});
				if (roomTypeQueryResult.SatisfactionRating == RoomType.RoomIdentificationResult.all_satisfied)
				{
					bool flag = false;
					RoomTypes.RoomTypeQueryResult[] array2 = possibleRoomTypes;
					for (int j = 0; j < array2.Length; j++)
					{
						RoomType type2 = array2[j].Type;
						if (type2 != type && type2 != Db.Get().RoomTypes.Neutral && Db.Get().RoomTypes.HasAmbiguousRoomType(room, type, type2))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						text += string.Format("\n<color=#F44A47FF>{0}{1}{2}</color>", "    ", "    • ", ROOMS.CRITERIA.NO_TYPE_CONFLICTS);
					}
				}
				else
				{
					foreach (RoomConstraints.Constraint constraint2 in type.additional_constraints)
					{
						if (!constraint2.isSatisfied(room))
						{
							string str = string.Empty;
							if (constraint2.building_criteria != null)
							{
								str = string.Format(ROOMS.CRITERIA.CRITERIA_FAILED.MISSING_BUILDING, constraint2.name);
							}
							else
							{
								str = string.Format(ROOMS.CRITERIA.CRITERIA_FAILED.FAILED, constraint2.name);
							}
							text = text + "\n<color=#F44A47FF>        • " + str + "</color>";
						}
					}
				}
			}
		}
		return text;
	}

	// Token: 0x04003384 RID: 13188
	public static RoomConstraints.Constraint CEILING_HEIGHT_4 = new RoomConstraints.Constraint(null, (Room room) => 1 + room.cavity.maxY - room.cavity.minY >= 4, 1, string.Format(ROOMS.CRITERIA.CEILING_HEIGHT.NAME, "4"), string.Format(ROOMS.CRITERIA.CEILING_HEIGHT.DESCRIPTION, "4"), null, null);

	// Token: 0x04003385 RID: 13189
	public static RoomConstraints.Constraint CEILING_HEIGHT_6 = new RoomConstraints.Constraint(null, (Room room) => 1 + room.cavity.maxY - room.cavity.minY >= 6, 1, string.Format(ROOMS.CRITERIA.CEILING_HEIGHT.NAME, "6"), string.Format(ROOMS.CRITERIA.CEILING_HEIGHT.DESCRIPTION, "6"), null, null);

	// Token: 0x04003386 RID: 13190
	public static RoomConstraints.Constraint MINIMUM_SIZE_12 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.numCells >= 12, 1, string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.NAME, "12"), string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, "12"), null, null);

	// Token: 0x04003387 RID: 13191
	public static RoomConstraints.Constraint MINIMUM_SIZE_24 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.numCells >= 24, 1, string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.NAME, "24"), string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, "24"), null, null);

	// Token: 0x04003388 RID: 13192
	public static RoomConstraints.Constraint MINIMUM_SIZE_32 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.numCells >= 32, 1, string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.NAME, "32"), string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, "32"), null, null);

	// Token: 0x04003389 RID: 13193
	public static RoomConstraints.Constraint MAXIMUM_SIZE_64 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.numCells <= 64, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "64"), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "64"), null, null);

	// Token: 0x0400338A RID: 13194
	public static RoomConstraints.Constraint MAXIMUM_SIZE_96 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.numCells <= 96, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "96"), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "96"), null, null);

	// Token: 0x0400338B RID: 13195
	public static RoomConstraints.Constraint MAXIMUM_SIZE_120 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.numCells <= 120, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "120"), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "120"), null, null);

	// Token: 0x0400338C RID: 13196
	public static RoomConstraints.Constraint NO_INDUSTRIAL_MACHINERY = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		using (List<KPrefabID>.Enumerator enumerator = room.buildings.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasTag(RoomConstraints.ConstraintTags.IndustrialMachinery))
				{
					return false;
				}
			}
		}
		return true;
	}, 1, ROOMS.CRITERIA.NO_INDUSTRIAL_MACHINERY.NAME, ROOMS.CRITERIA.NO_INDUSTRIAL_MACHINERY.DESCRIPTION, null, null);

	// Token: 0x0400338D RID: 13197
	public static RoomConstraints.Constraint NO_COTS = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		foreach (KPrefabID kprefabID in room.buildings)
		{
			if (kprefabID.HasTag(RoomConstraints.ConstraintTags.BedType) && !kprefabID.HasTag(RoomConstraints.ConstraintTags.LuxuryBedType))
			{
				return false;
			}
		}
		return true;
	}, 1, ROOMS.CRITERIA.NO_COTS.NAME, ROOMS.CRITERIA.NO_COTS.DESCRIPTION, null, null);

	// Token: 0x0400338E RID: 13198
	public static RoomConstraints.Constraint NO_LUXURY_BEDS = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		using (List<KPrefabID>.Enumerator enumerator = room.buildings.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasTag(RoomConstraints.ConstraintTags.LuxuryBedType))
				{
					return false;
				}
			}
		}
		return true;
	}, 1, ROOMS.CRITERIA.NO_COTS.NAME, ROOMS.CRITERIA.NO_COTS.DESCRIPTION, null, null);

	// Token: 0x0400338F RID: 13199
	public static RoomConstraints.Constraint NO_OUTHOUSES = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		foreach (KPrefabID kprefabID in room.buildings)
		{
			if (kprefabID.HasTag(RoomConstraints.ConstraintTags.ToiletType) && !kprefabID.HasTag(RoomConstraints.ConstraintTags.FlushToiletType))
			{
				return false;
			}
		}
		return true;
	}, 1, ROOMS.CRITERIA.NO_OUTHOUSES.NAME, ROOMS.CRITERIA.NO_OUTHOUSES.DESCRIPTION, null, null);

	// Token: 0x04003390 RID: 13200
	public static RoomConstraints.Constraint NO_MESS_STATION = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		bool flag = false;
		int num = 0;
		while (!flag && num < room.buildings.Count)
		{
			flag = room.buildings[num].HasTag(RoomConstraints.ConstraintTags.MessTable);
			num++;
		}
		return !flag;
	}, 1, ROOMS.CRITERIA.NO_MESS_STATION.NAME, ROOMS.CRITERIA.NO_MESS_STATION.DESCRIPTION, null, null);

	// Token: 0x04003391 RID: 13201
	public static RoomConstraints.Constraint HAS_LUXURY_BED = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.LuxuryBedType), null, 1, ROOMS.CRITERIA.HAS_LUXURY_BED.NAME, ROOMS.CRITERIA.HAS_LUXURY_BED.DESCRIPTION, null, null);

	// Token: 0x04003392 RID: 13202
	public static RoomConstraints.Constraint HAS_BED = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.BedType) && !bc.HasTag(RoomConstraints.ConstraintTags.Clinic), null, 1, ROOMS.CRITERIA.HAS_BED.NAME, ROOMS.CRITERIA.HAS_BED.DESCRIPTION, null, null);

	// Token: 0x04003393 RID: 13203
	public static RoomConstraints.Constraint SCIENCE_BUILDINGS = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.ScienceBuilding), null, 2, ROOMS.CRITERIA.SCIENCE_BUILDINGS.NAME, ROOMS.CRITERIA.SCIENCE_BUILDINGS.DESCRIPTION, null, null);

	// Token: 0x04003394 RID: 13204
	public static RoomConstraints.Constraint BED_SINGLE = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.BedType) && !bc.HasTag(RoomConstraints.ConstraintTags.Clinic), delegate(Room room)
	{
		short num = 0;
		int num2 = 0;
		while (num < 2 && num2 < room.buildings.Count)
		{
			if (room.buildings[num2].HasTag(RoomConstraints.ConstraintTags.BedType))
			{
				num += 1;
			}
			num2++;
		}
		return num == 1;
	}, 1, ROOMS.CRITERIA.BED_SINGLE.NAME, ROOMS.CRITERIA.BED_SINGLE.DESCRIPTION, null, null);

	// Token: 0x04003395 RID: 13205
	public static RoomConstraints.Constraint LUXURY_BED_SINGLE = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.LuxuryBedType), delegate(Room room)
	{
		short num = 0;
		int num2 = 0;
		while (num <= 2 && num2 < room.buildings.Count)
		{
			if (room.buildings[num2].HasTag(RoomConstraints.ConstraintTags.LuxuryBedType))
			{
				num += 1;
			}
			num2++;
		}
		return num == 1;
	}, 1, ROOMS.CRITERIA.LUXURYBEDTYPE.NAME, ROOMS.CRITERIA.LUXURYBEDTYPE.DESCRIPTION, null, null);

	// Token: 0x04003396 RID: 13206
	public static RoomConstraints.Constraint BUILDING_DECOR_POSITIVE = new RoomConstraints.Constraint(delegate(KPrefabID bc)
	{
		DecorProvider component = bc.GetComponent<DecorProvider>();
		return component != null && component.baseDecor > 0f;
	}, null, 1, ROOMS.CRITERIA.BUILDING_DECOR_POSITIVE.NAME, ROOMS.CRITERIA.BUILDING_DECOR_POSITIVE.DESCRIPTION, null, null);

	// Token: 0x04003397 RID: 13207
	public static RoomConstraints.Constraint DECORATIVE_ITEM = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(GameTags.Decoration), null, 1, string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM.NAME, 1), string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM.DESCRIPTION, 1), null, null);

	// Token: 0x04003398 RID: 13208
	public static RoomConstraints.Constraint DECORATIVE_ITEM_2 = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(GameTags.Decoration), null, 2, string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM.NAME, 2), string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM.DESCRIPTION, 2), null, null);

	// Token: 0x04003399 RID: 13209
	public static RoomConstraints.Constraint DECORATIVE_ITEM_SCORE_20 = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(GameTags.Decoration) && bc.HasTag(RoomConstraints.ConstraintTags.Decor20), null, 1, ROOMS.CRITERIA.DECOR20.NAME, ROOMS.CRITERIA.DECOR20.DESCRIPTION, null, null);

	// Token: 0x0400339A RID: 13210
	public static RoomConstraints.Constraint POWER_STATION = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.HeavyDutyGeneratorType), delegate(Room room)
	{
		int num = 0;
		bool flag = false;
		foreach (KPrefabID kprefabID in room.buildings)
		{
			flag = (flag || kprefabID.HasTag(RoomConstraints.ConstraintTags.HeavyDutyGeneratorType));
			num += (kprefabID.HasTag(RoomConstraints.ConstraintTags.PowerBuilding) ? 1 : 0);
		}
		return flag && num >= 2;
	}, 1, ROOMS.CRITERIA.POWERPLANT.NAME, ROOMS.CRITERIA.POWERPLANT.DESCRIPTION, null, ROOMS.CRITERIA.POWERPLANT.CONFLICT_DESCRIPTION);

	// Token: 0x0400339B RID: 13211
	public static RoomConstraints.Constraint FARM_STATION = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.FarmStationType), null, 1, ROOMS.CRITERIA.FARMSTATIONTYPE.NAME, ROOMS.CRITERIA.FARMSTATIONTYPE.DESCRIPTION, null, null);

	// Token: 0x0400339C RID: 13212
	public static RoomConstraints.Constraint RANCH_STATION = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.RanchStationType), null, 1, ROOMS.CRITERIA.RANCHSTATIONTYPE.NAME, ROOMS.CRITERIA.RANCHSTATIONTYPE.DESCRIPTION, null, null);

	// Token: 0x0400339D RID: 13213
	public static RoomConstraints.Constraint SPICE_STATION = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.SpiceStation), null, 1, ROOMS.CRITERIA.SPICESTATION.NAME, ROOMS.CRITERIA.SPICESTATION.DESCRIPTION, null, null);

	// Token: 0x0400339E RID: 13214
	public static RoomConstraints.Constraint COOK_TOP = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.CookTop), null, 1, ROOMS.CRITERIA.COOKTOP.NAME, ROOMS.CRITERIA.COOKTOP.DESCRIPTION, null, null);

	// Token: 0x0400339F RID: 13215
	public static RoomConstraints.Constraint REFRIGERATOR = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.Refrigerator), null, 1, ROOMS.CRITERIA.REFRIGERATOR.NAME, ROOMS.CRITERIA.REFRIGERATOR.DESCRIPTION, null, null);

	// Token: 0x040033A0 RID: 13216
	public static RoomConstraints.Constraint REC_BUILDING = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.RecBuilding), null, 1, ROOMS.CRITERIA.RECBUILDING.NAME, ROOMS.CRITERIA.RECBUILDING.DESCRIPTION, null, null);

	// Token: 0x040033A1 RID: 13217
	public static RoomConstraints.Constraint MACHINE_SHOP = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.MachineShopType), null, 1, ROOMS.CRITERIA.MACHINESHOPTYPE.NAME, ROOMS.CRITERIA.MACHINESHOPTYPE.DESCRIPTION, null, null);

	// Token: 0x040033A2 RID: 13218
	public static RoomConstraints.Constraint LIGHT = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		foreach (KPrefabID kprefabID in room.cavity.creatures)
		{
			if (kprefabID != null && kprefabID.GetComponent<Light2D>() != null)
			{
				return true;
			}
		}
		foreach (KPrefabID kprefabID2 in room.buildings)
		{
			if (!(kprefabID2 == null))
			{
				Light2D component = kprefabID2.GetComponent<Light2D>();
				if (component != null)
				{
					RequireInputs component2 = kprefabID2.GetComponent<RequireInputs>();
					if (component.enabled || (component2 != null && component2.RequirementsMet))
					{
						return true;
					}
				}
			}
		}
		return false;
	}, 1, ROOMS.CRITERIA.LIGHTSOURCE.NAME, ROOMS.CRITERIA.LIGHTSOURCE.DESCRIPTION, null, null);

	// Token: 0x040033A3 RID: 13219
	public static RoomConstraints.Constraint DESTRESSING_BUILDING = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.DeStressingBuilding), null, 1, ROOMS.CRITERIA.DESTRESSINGBUILDING.NAME, ROOMS.CRITERIA.DESTRESSINGBUILDING.DESCRIPTION, null, null);

	// Token: 0x040033A4 RID: 13220
	public static RoomConstraints.Constraint MASSAGE_TABLE = new RoomConstraints.Constraint((KPrefabID bc) => bc.IsPrefabID(RoomConstraints.ConstraintTags.MassageTable), null, 1, ROOMS.CRITERIA.MASSAGE_TABLE.NAME, ROOMS.CRITERIA.MASSAGE_TABLE.DESCRIPTION, null, null);

	// Token: 0x040033A5 RID: 13221
	public static RoomConstraints.Constraint MESS_STATION_SINGLE = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.MessTable), null, 1, ROOMS.CRITERIA.MESSTABLE.NAME, ROOMS.CRITERIA.MESSTABLE.DESCRIPTION, new List<RoomConstraints.Constraint>
	{
		RoomConstraints.REC_BUILDING
	}, null);

	// Token: 0x040033A6 RID: 13222
	public static RoomConstraints.Constraint TOILET = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.ToiletType), null, 1, ROOMS.CRITERIA.TOILETTYPE.NAME, ROOMS.CRITERIA.TOILETTYPE.DESCRIPTION, null, null);

	// Token: 0x040033A7 RID: 13223
	public static RoomConstraints.Constraint BIONICUPKEEP = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.BionicUpkeepType), null, 2, ROOMS.CRITERIA.BIONICUPKEEP.NAME, ROOMS.CRITERIA.BIONICUPKEEP.DESCRIPTION, null, null);

	// Token: 0x040033A8 RID: 13224
	public static RoomConstraints.Constraint BIONIC_LUBRICATION = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag("OilChanger"), null, 1, ROOMS.CRITERIA.BIONIC_LUBRICATION.NAME, ROOMS.CRITERIA.BIONIC_LUBRICATION.DESCRIPTION, null, null);

	// Token: 0x040033A9 RID: 13225
	public static RoomConstraints.Constraint BIONIC_GUNKEMPTIER = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag("GunkEmptier"), null, 1, ROOMS.CRITERIA.BIONIC_GUNKEMPTIER.NAME, ROOMS.CRITERIA.BIONIC_GUNKEMPTIER.DESCRIPTION, null, null);

	// Token: 0x040033AA RID: 13226
	public static RoomConstraints.Constraint FLUSH_TOILET = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.FlushToiletType), null, 1, ROOMS.CRITERIA.FLUSHTOILETTYPE.NAME, ROOMS.CRITERIA.FLUSHTOILETTYPE.DESCRIPTION, null, null);

	// Token: 0x040033AB RID: 13227
	public static RoomConstraints.Constraint WASH_STATION = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.WashStation), null, 1, ROOMS.CRITERIA.WASHSTATION.NAME, ROOMS.CRITERIA.WASHSTATION.DESCRIPTION, null, null);

	// Token: 0x040033AC RID: 13228
	public static RoomConstraints.Constraint ADVANCEDWASHSTATION = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.AdvancedWashStation), null, 1, ROOMS.CRITERIA.ADVANCEDWASHSTATION.NAME, ROOMS.CRITERIA.ADVANCEDWASHSTATION.DESCRIPTION, null, null);

	// Token: 0x040033AD RID: 13229
	public static RoomConstraints.Constraint CLINIC = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.Clinic), null, 1, ROOMS.CRITERIA.CLINIC.NAME, ROOMS.CRITERIA.CLINIC.DESCRIPTION, new List<RoomConstraints.Constraint>
	{
		RoomConstraints.TOILET,
		RoomConstraints.FLUSH_TOILET,
		RoomConstraints.MESS_STATION_SINGLE
	}, null);

	// Token: 0x040033AE RID: 13230
	public static RoomConstraints.Constraint PARK_BUILDING = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.Park), null, 1, ROOMS.CRITERIA.PARK.NAME, ROOMS.CRITERIA.PARK.DESCRIPTION, null, null);

	// Token: 0x040033AF RID: 13231
	public static RoomConstraints.Constraint ORIGINALTILES = new RoomConstraints.Constraint(null, (Room room) => 1 + room.cavity.maxY - room.cavity.minY >= 4, 1, "", "", null, null);

	// Token: 0x040033B0 RID: 13232
	public static RoomConstraints.Constraint IS_BACKWALLED = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		bool flag = true;
		int num = (room.cavity.maxX - room.cavity.minX + 1) / 2 + 1;
		int num2 = 0;
		while (flag && num2 < num)
		{
			int x = room.cavity.minX + num2;
			int x2 = room.cavity.maxX - num2;
			int num3 = room.cavity.minY;
			while (flag && num3 <= room.cavity.maxY)
			{
				int cell = Grid.XYToCell(x, num3);
				int cell2 = Grid.XYToCell(x2, num3);
				if (Game.Instance.roomProber.GetCavityForCell(cell) == room.cavity)
				{
					GameObject gameObject = Grid.Objects[cell, 2];
					flag &= (gameObject != null && !gameObject.HasTag(GameTags.UnderConstruction));
				}
				if (Game.Instance.roomProber.GetCavityForCell(cell2) == room.cavity)
				{
					GameObject gameObject2 = Grid.Objects[cell2, 2];
					flag &= (gameObject2 != null && !gameObject2.HasTag(GameTags.UnderConstruction));
				}
				if (!flag)
				{
					return false;
				}
				num3++;
			}
			num2++;
		}
		return flag;
	}, 1, ROOMS.CRITERIA.IS_BACKWALLED.NAME, ROOMS.CRITERIA.IS_BACKWALLED.DESCRIPTION, null, null);

	// Token: 0x040033B1 RID: 13233
	public static RoomConstraints.Constraint WILDANIMAL = new RoomConstraints.Constraint(null, (Room room) => room.cavity.creatures.Count + room.cavity.eggs.Count > 0, 1, ROOMS.CRITERIA.WILDANIMAL.NAME, ROOMS.CRITERIA.WILDANIMAL.DESCRIPTION, null, null);

	// Token: 0x040033B2 RID: 13234
	public static RoomConstraints.Constraint WILDANIMALS = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		int num = 0;
		using (List<KPrefabID>.Enumerator enumerator = room.cavity.creatures.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasTag(GameTags.Creatures.Wild))
				{
					num++;
				}
			}
		}
		return num >= 2;
	}, 1, ROOMS.CRITERIA.WILDANIMALS.NAME, ROOMS.CRITERIA.WILDANIMALS.DESCRIPTION, null, null);

	// Token: 0x040033B3 RID: 13235
	public static RoomConstraints.Constraint WILDPLANT = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		int num = 0;
		foreach (KPrefabID kprefabID in room.cavity.plants)
		{
			if (kprefabID != null)
			{
				BasicForagePlantPlanted component = kprefabID.GetComponent<BasicForagePlantPlanted>();
				ReceptacleMonitor component2 = kprefabID.GetComponent<ReceptacleMonitor>();
				if (component2 != null && !component2.Replanted)
				{
					num++;
				}
				else if (component != null)
				{
					num++;
				}
			}
		}
		return num >= 2;
	}, 1, ROOMS.CRITERIA.WILDPLANT.NAME, ROOMS.CRITERIA.WILDPLANT.DESCRIPTION, null, null);

	// Token: 0x040033B4 RID: 13236
	public static RoomConstraints.Constraint WILDPLANTS = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		int num = 0;
		foreach (KPrefabID kprefabID in room.cavity.plants)
		{
			if (kprefabID != null)
			{
				BasicForagePlantPlanted component = kprefabID.GetComponent<BasicForagePlantPlanted>();
				ReceptacleMonitor component2 = kprefabID.GetComponent<ReceptacleMonitor>();
				if (component2 != null && !component2.Replanted)
				{
					num++;
				}
				else if (component != null)
				{
					num++;
				}
			}
		}
		return num >= 4;
	}, 1, ROOMS.CRITERIA.WILDPLANTS.NAME, ROOMS.CRITERIA.WILDPLANTS.DESCRIPTION, null, null);

	// Token: 0x02001A84 RID: 6788
	public static class ConstraintTags
	{
		// Token: 0x0600A02C RID: 41004 RVA: 0x0037EA64 File Offset: 0x0037CC64
		public static string GetRoomConstraintLabelText(Tag tag)
		{
			StringEntry entry = null;
			string text = "STRINGS.ROOMS.CRITERIA." + tag.ToString().ToUpper() + ".NAME";
			if (!Strings.TryGet(new StringKey(text), out entry))
			{
				return ROOMS.CRITERIA.IN_CODE_ERROR.text.Replace("{0}", text);
			}
			return entry;
		}

		// Token: 0x04007CB3 RID: 31923
		public static List<Tag> AllTags = new List<Tag>();

		// Token: 0x04007CB4 RID: 31924
		public static Tag BedType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("BedType".ToTag());

		// Token: 0x04007CB5 RID: 31925
		public static Tag LuxuryBedType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("LuxuryBedType".ToTag());

		// Token: 0x04007CB6 RID: 31926
		public static Tag ToiletType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("ToiletType".ToTag());

		// Token: 0x04007CB7 RID: 31927
		public static Tag BionicUpkeepType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("BionicUpkeep".ToTag());

		// Token: 0x04007CB8 RID: 31928
		public static Tag FlushToiletType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("FlushToiletType".ToTag());

		// Token: 0x04007CB9 RID: 31929
		public static Tag MessTable = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("MessTable".ToTag());

		// Token: 0x04007CBA RID: 31930
		public static Tag Clinic = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("Clinic".ToTag());

		// Token: 0x04007CBB RID: 31931
		public static Tag WashStation = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("WashStation".ToTag());

		// Token: 0x04007CBC RID: 31932
		public static Tag AdvancedWashStation = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("AdvancedWashStation".ToTag());

		// Token: 0x04007CBD RID: 31933
		public static Tag ScienceBuilding = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("ScienceBuilding".ToTag());

		// Token: 0x04007CBE RID: 31934
		public static Tag LightSource = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("LightSource".ToTag());

		// Token: 0x04007CBF RID: 31935
		public static Tag MassageTable = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("MassageTable".ToTag());

		// Token: 0x04007CC0 RID: 31936
		public static Tag DeStressingBuilding = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("DeStressingBuilding".ToTag());

		// Token: 0x04007CC1 RID: 31937
		public static Tag IndustrialMachinery = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("IndustrialMachinery".ToTag());

		// Token: 0x04007CC2 RID: 31938
		public static Tag GeneratorType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("GeneratorType".ToTag());

		// Token: 0x04007CC3 RID: 31939
		public static Tag HeavyDutyGeneratorType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("HeavyDutyGeneratorType".ToTag());

		// Token: 0x04007CC4 RID: 31940
		public static Tag LightDutyGeneratorType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("LightDutyGeneratorType".ToTag());

		// Token: 0x04007CC5 RID: 31941
		public static Tag PowerBuilding = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("PowerBuilding".ToTag());

		// Token: 0x04007CC6 RID: 31942
		public static Tag FarmStationType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("FarmStationType".ToTag());

		// Token: 0x04007CC7 RID: 31943
		public static Tag CreatureRelocator = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("CreatureRelocator".ToTag());

		// Token: 0x04007CC8 RID: 31944
		public static Tag RanchStationType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("RanchStationType".ToTag());

		// Token: 0x04007CC9 RID: 31945
		public static Tag SpiceStation = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("SpiceStation".ToTag());

		// Token: 0x04007CCA RID: 31946
		public static Tag CookTop = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("CookTop".ToTag());

		// Token: 0x04007CCB RID: 31947
		public static Tag Refrigerator = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("Refrigerator".ToTag());

		// Token: 0x04007CCC RID: 31948
		public static Tag RecBuilding = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("RecBuilding".ToTag());

		// Token: 0x04007CCD RID: 31949
		public static Tag MachineShopType = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("MachineShopType".ToTag());

		// Token: 0x04007CCE RID: 31950
		public static Tag Park = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("Park".ToTag());

		// Token: 0x04007CCF RID: 31951
		public static Tag NatureReserve = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("NatureReserve".ToTag());

		// Token: 0x04007CD0 RID: 31952
		public static Tag Decor20 = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("Decor20".ToTag());

		// Token: 0x04007CD1 RID: 31953
		public static Tag RocketInterior = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("RocketInterior".ToTag());

		// Token: 0x04007CD2 RID: 31954
		public static Tag Decoration = RoomConstraints.ConstraintTags.AllTags.AddAndReturn(GameTags.Decoration);

		// Token: 0x04007CD3 RID: 31955
		public static Tag WarmingStation = RoomConstraints.ConstraintTags.AllTags.AddAndReturn("WarmingStation".ToTag());
	}

	// Token: 0x02001A85 RID: 6789
	public class Constraint
	{
		// Token: 0x0600A02E RID: 41006 RVA: 0x0037EDF4 File Offset: 0x0037CFF4
		public Constraint(Func<KPrefabID, bool> building_criteria, Func<Room, bool> room_criteria, int times_required = 1, string name = "", string description = "", List<RoomConstraints.Constraint> stomp_in_conflict = null, string overrideConstraintConflictName = null)
		{
			this.room_criteria = room_criteria;
			this.building_criteria = building_criteria;
			this.times_required = times_required;
			this.name = name;
			this.description = description;
			this.stomp_in_conflict = stomp_in_conflict;
			this.conflictDescription = ((overrideConstraintConflictName == null) ? name : overrideConstraintConflictName);
		}

		// Token: 0x0600A02F RID: 41007 RVA: 0x0037EE4C File Offset: 0x0037D04C
		public bool isSatisfied(Room room)
		{
			int num = 0;
			if (this.room_criteria != null && !this.room_criteria(room))
			{
				return false;
			}
			if (this.building_criteria != null)
			{
				int num2 = 0;
				while (num < this.times_required && num2 < room.buildings.Count)
				{
					KPrefabID kprefabID = room.buildings[num2];
					if (!(kprefabID == null) && this.building_criteria(kprefabID))
					{
						num++;
					}
					num2++;
				}
				int num3 = 0;
				while (num < this.times_required && num3 < room.plants.Count)
				{
					KPrefabID kprefabID2 = room.plants[num3];
					if (!(kprefabID2 == null) && this.building_criteria(kprefabID2))
					{
						num++;
					}
					num3++;
				}
				return num >= this.times_required;
			}
			return true;
		}

		// Token: 0x04007CD4 RID: 31956
		public string name;

		// Token: 0x04007CD5 RID: 31957
		public string description;

		// Token: 0x04007CD6 RID: 31958
		public string conflictDescription;

		// Token: 0x04007CD7 RID: 31959
		public int times_required = 1;

		// Token: 0x04007CD8 RID: 31960
		public Func<Room, bool> room_criteria;

		// Token: 0x04007CD9 RID: 31961
		public Func<KPrefabID, bool> building_criteria;

		// Token: 0x04007CDA RID: 31962
		public List<RoomConstraints.Constraint> stomp_in_conflict;
	}
}
