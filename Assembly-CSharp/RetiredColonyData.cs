using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A5D RID: 2653
public class RetiredColonyData
{
	// Token: 0x06004CF7 RID: 19703 RVA: 0x001B864E File Offset: 0x001B684E
	public RetiredColonyData()
	{
	}

	// Token: 0x06004CF8 RID: 19704 RVA: 0x001B8658 File Offset: 0x001B6858
	public RetiredColonyData(string colonyName, int cycleCount, string date, string[] achievements, MinionAssignablesProxy[] minions, BuildingComplete[] buildingCompletes, string startWorld, Dictionary<string, string> worldIdentities)
	{
		this.colonyName = colonyName;
		this.cycleCount = cycleCount;
		this.achievements = achievements;
		this.date = date;
		this.Duplicants = new RetiredColonyData.RetiredDuplicantData[(minions == null) ? 0 : minions.Length];
		int i = 0;
		while (i < this.Duplicants.Length)
		{
			this.Duplicants[i] = new RetiredColonyData.RetiredDuplicantData();
			this.Duplicants[i].name = minions[i].GetProperName();
			this.Duplicants[i].age = (int)Mathf.Floor((float)GameClock.Instance.GetCycle() - minions[i].GetArrivalTime());
			this.Duplicants[i].skillPointsGained = minions[i].GetTotalSkillpoints();
			this.Duplicants[i].accessories = new Dictionary<string, string>();
			if (minions[i].GetTargetGameObject().GetComponent<Accessorizer>() != null)
			{
				using (List<ResourceRef<Accessory>>.Enumerator enumerator = minions[i].GetTargetGameObject().GetComponent<Accessorizer>().GetAccessories().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ResourceRef<Accessory> resourceRef = enumerator.Current;
						if (resourceRef.Get() != null)
						{
							this.Duplicants[i].accessories.Add(resourceRef.Get().slot.Id, resourceRef.Get().Id);
						}
					}
					goto IL_3AF;
				}
				goto IL_14E;
			}
			goto IL_14E;
			IL_3AF:
			i++;
			continue;
			IL_14E:
			StoredMinionIdentity component = minions[i].GetTargetGameObject().GetComponent<StoredMinionIdentity>();
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Eyes.Id, Db.Get().Accessories.Get(component.bodyData.eyes).Id);
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Arm.Id, Db.Get().Accessories.Get(component.bodyData.arms).Id);
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.ArmLower.Id, Db.Get().Accessories.Get(component.bodyData.armslower).Id);
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Body.Id, Db.Get().Accessories.Get(component.bodyData.body).Id);
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Hair.Id, Db.Get().Accessories.Get(component.bodyData.hair).Id);
			if (component.bodyData.hat != HashedString.Invalid)
			{
				this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Hat.Id, Db.Get().Accessories.Get(component.bodyData.hat).Id);
			}
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.HeadShape.Id, Db.Get().Accessories.Get(component.bodyData.headShape).Id);
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Mouth.Id, Db.Get().Accessories.Get(component.bodyData.mouth).Id);
			goto IL_3AF;
		}
		Dictionary<Tag, int> dictionary = new Dictionary<Tag, int>();
		if (buildingCompletes != null)
		{
			foreach (BuildingComplete cmp in buildingCompletes)
			{
				if (!dictionary.ContainsKey(cmp.PrefabID()))
				{
					dictionary[cmp.PrefabID()] = 0;
				}
				Dictionary<Tag, int> dictionary2 = dictionary;
				Tag key = cmp.PrefabID();
				int num = dictionary2[key];
				dictionary2[key] = num + 1;
			}
		}
		this.buildings = new List<global::Tuple<string, int>>();
		foreach (KeyValuePair<Tag, int> keyValuePair in dictionary)
		{
			this.buildings.Add(new global::Tuple<string, int>(keyValuePair.Key.ToString(), keyValuePair.Value));
		}
		this.Stats = null;
		if (ReportManager.Instance != null)
		{
			global::Tuple<float, float>[] array = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int k = 0; k < array.Length; k++)
			{
				array[k] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[k].day, ReportManager.Instance.reports[k].GetEntry(ReportManager.ReportType.OxygenCreated).accPositive);
			}
			global::Tuple<float, float>[] array2 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int l = 0; l < array2.Length; l++)
			{
				array2[l] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[l].day, ReportManager.Instance.reports[l].GetEntry(ReportManager.ReportType.OxygenCreated).accNegative * -1f);
			}
			global::Tuple<float, float>[] array3 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int m = 0; m < array3.Length; m++)
			{
				array3[m] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[m].day, ReportManager.Instance.reports[m].GetEntry(ReportManager.ReportType.CaloriesCreated).accPositive * 0.001f);
			}
			global::Tuple<float, float>[] array4 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int n = 0; n < array4.Length; n++)
			{
				array4[n] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[n].day, ReportManager.Instance.reports[n].GetEntry(ReportManager.ReportType.CaloriesCreated).accNegative * 0.001f * -1f);
			}
			global::Tuple<float, float>[] array5 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num2 = 0; num2 < array5.Length; num2++)
			{
				array5[num2] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num2].day, ReportManager.Instance.reports[num2].GetEntry(ReportManager.ReportType.EnergyCreated).accPositive * 0.001f);
			}
			global::Tuple<float, float>[] array6 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num3 = 0; num3 < array6.Length; num3++)
			{
				array6[num3] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num3].day, ReportManager.Instance.reports[num3].GetEntry(ReportManager.ReportType.EnergyWasted).accNegative * -1f * 0.001f);
			}
			global::Tuple<float, float>[] array7 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num4 = 0; num4 < array7.Length; num4++)
			{
				array7[num4] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num4].day, ReportManager.Instance.reports[num4].GetEntry(ReportManager.ReportType.WorkTime).accPositive);
			}
			global::Tuple<float, float>[] array8 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num5 = 0; num5 < array7.Length; num5++)
			{
				int num6 = 0;
				float num7 = 0f;
				ReportManager.ReportEntry entry = ReportManager.Instance.reports[num5].GetEntry(ReportManager.ReportType.WorkTime);
				for (int num8 = 0; num8 < entry.contextEntries.Count; num8++)
				{
					num6++;
					num7 += entry.contextEntries[num8].accPositive;
				}
				num7 /= (float)num6;
				num7 /= 600f;
				num7 *= 100f;
				array8[num5] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num5].day, num7);
			}
			global::Tuple<float, float>[] array9 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num9 = 0; num9 < array9.Length; num9++)
			{
				array9[num9] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num9].day, ReportManager.Instance.reports[num9].GetEntry(ReportManager.ReportType.TravelTime).accPositive);
			}
			global::Tuple<float, float>[] array10 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num10 = 0; num10 < array9.Length; num10++)
			{
				int num11 = 0;
				float num12 = 0f;
				ReportManager.ReportEntry entry2 = ReportManager.Instance.reports[num10].GetEntry(ReportManager.ReportType.TravelTime);
				for (int num13 = 0; num13 < entry2.contextEntries.Count; num13++)
				{
					num11++;
					num12 += entry2.contextEntries[num13].accPositive;
				}
				num12 /= (float)num11;
				num12 /= 600f;
				num12 *= 100f;
				array10[num10] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num10].day, num12);
			}
			global::Tuple<float, float>[] array11 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num14 = 0; num14 < array7.Length; num14++)
			{
				array11[num14] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num14].day, (float)ReportManager.Instance.reports[num14].GetEntry(ReportManager.ReportType.WorkTime).contextEntries.Count);
			}
			global::Tuple<float, float>[] array12 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num15 = 0; num15 < array12.Length; num15++)
			{
				int num16 = 0;
				float num17 = 0f;
				ReportManager.ReportEntry entry3 = ReportManager.Instance.reports[num15].GetEntry(ReportManager.ReportType.StressDelta);
				for (int num18 = 0; num18 < entry3.contextEntries.Count; num18++)
				{
					num16++;
					num17 += entry3.contextEntries[num18].accPositive;
				}
				array12[num15] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num15].day, num17 / (float)num16);
			}
			global::Tuple<float, float>[] array13 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num19 = 0; num19 < array13.Length; num19++)
			{
				int num20 = 0;
				float num21 = 0f;
				ReportManager.ReportEntry entry4 = ReportManager.Instance.reports[num19].GetEntry(ReportManager.ReportType.StressDelta);
				for (int num22 = 0; num22 < entry4.contextEntries.Count; num22++)
				{
					num20++;
					num21 += entry4.contextEntries[num22].accNegative;
				}
				num21 *= -1f;
				array13[num19] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num19].day, num21 / (float)num20);
			}
			global::Tuple<float, float>[] array14 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num23 = 0; num23 < array14.Length; num23++)
			{
				array14[num23] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num23].day, ReportManager.Instance.reports[num23].GetEntry(ReportManager.ReportType.DomesticatedCritters).accPositive);
			}
			global::Tuple<float, float>[] array15 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num24 = 0; num24 < array15.Length; num24++)
			{
				array15[num24] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num24].day, ReportManager.Instance.reports[num24].GetEntry(ReportManager.ReportType.WildCritters).accPositive);
			}
			global::Tuple<float, float>[] array16 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num25 = 0; num25 < array16.Length; num25++)
			{
				array16[num25] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num25].day, ReportManager.Instance.reports[num25].GetEntry(ReportManager.ReportType.RocketsInFlight).accPositive);
			}
			this.Stats = new RetiredColonyData.RetiredColonyStatistic[]
			{
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.OxygenProduced, array, UI.RETIRED_COLONY_INFO_SCREEN.STATS.OXYGEN_CREATED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.MASS.KILOGRAM),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.OxygenConsumed, array2, UI.RETIRED_COLONY_INFO_SCREEN.STATS.OXYGEN_CONSUMED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.MASS.KILOGRAM),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.CaloriesProduced, array3, UI.RETIRED_COLONY_INFO_SCREEN.STATS.CALORIES_CREATED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.CALORIES.KILOCALORIE),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.CaloriesRemoved, array4, UI.RETIRED_COLONY_INFO_SCREEN.STATS.CALORIES_CONSUMED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.CALORIES.KILOCALORIE),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.PowerProduced, array5, UI.RETIRED_COLONY_INFO_SCREEN.STATS.POWER_CREATED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.PowerWasted, array6, UI.RETIRED_COLONY_INFO_SCREEN.STATS.POWER_WASTED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.WorkTime, array7, UI.RETIRED_COLONY_INFO_SCREEN.STATS.WORK_TIME, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.SECONDS),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.AverageWorkTime, array8, UI.RETIRED_COLONY_INFO_SCREEN.STATS.AVERAGE_WORK_TIME, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.PERCENT),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.TravelTime, array9, UI.RETIRED_COLONY_INFO_SCREEN.STATS.TRAVEL_TIME, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.SECONDS),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.AverageTravelTime, array10, UI.RETIRED_COLONY_INFO_SCREEN.STATS.AVERAGE_TRAVEL_TIME, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.PERCENT),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.LiveDuplicants, array11, UI.RETIRED_COLONY_INFO_SCREEN.STATS.LIVE_DUPLICANTS, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.DUPLICANTS),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.RocketsInFlight, array16, UI.RETIRED_COLONY_INFO_SCREEN.STATS.ROCKET_MISSIONS, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.ROCKET_MISSIONS),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.AverageStressCreated, array12, UI.RETIRED_COLONY_INFO_SCREEN.STATS.AVERAGE_STRESS_CREATED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.PERCENT),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.AverageStressRemoved, array13, UI.RETIRED_COLONY_INFO_SCREEN.STATS.AVERAGE_STRESS_REMOVED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.PERCENT),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.DomesticatedCritters, array14, UI.RETIRED_COLONY_INFO_SCREEN.STATS.NUMBER_DOMESTICATED_CRITTERS, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.CRITTERS),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.WildCritters, array15, UI.RETIRED_COLONY_INFO_SCREEN.STATS.NUMBER_WILD_CRITTERS, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.CRITTERS)
			};
			this.startWorld = startWorld;
			this.worldIdentities = worldIdentities;
		}
	}

	// Token: 0x17000584 RID: 1412
	// (get) Token: 0x06004CF9 RID: 19705 RVA: 0x001B9640 File Offset: 0x001B7840
	// (set) Token: 0x06004CFA RID: 19706 RVA: 0x001B9648 File Offset: 0x001B7848
	public string colonyName { get; set; }

	// Token: 0x17000585 RID: 1413
	// (get) Token: 0x06004CFB RID: 19707 RVA: 0x001B9651 File Offset: 0x001B7851
	// (set) Token: 0x06004CFC RID: 19708 RVA: 0x001B9659 File Offset: 0x001B7859
	public int cycleCount { get; set; }

	// Token: 0x17000586 RID: 1414
	// (get) Token: 0x06004CFD RID: 19709 RVA: 0x001B9662 File Offset: 0x001B7862
	// (set) Token: 0x06004CFE RID: 19710 RVA: 0x001B966A File Offset: 0x001B786A
	public string date { get; set; }

	// Token: 0x17000587 RID: 1415
	// (get) Token: 0x06004CFF RID: 19711 RVA: 0x001B9673 File Offset: 0x001B7873
	// (set) Token: 0x06004D00 RID: 19712 RVA: 0x001B967B File Offset: 0x001B787B
	public string[] achievements { get; set; }

	// Token: 0x17000588 RID: 1416
	// (get) Token: 0x06004D01 RID: 19713 RVA: 0x001B9684 File Offset: 0x001B7884
	// (set) Token: 0x06004D02 RID: 19714 RVA: 0x001B968C File Offset: 0x001B788C
	public RetiredColonyData.RetiredDuplicantData[] Duplicants { get; set; }

	// Token: 0x17000589 RID: 1417
	// (get) Token: 0x06004D03 RID: 19715 RVA: 0x001B9695 File Offset: 0x001B7895
	// (set) Token: 0x06004D04 RID: 19716 RVA: 0x001B969D File Offset: 0x001B789D
	public List<global::Tuple<string, int>> buildings { get; set; }

	// Token: 0x1700058A RID: 1418
	// (get) Token: 0x06004D05 RID: 19717 RVA: 0x001B96A6 File Offset: 0x001B78A6
	// (set) Token: 0x06004D06 RID: 19718 RVA: 0x001B96AE File Offset: 0x001B78AE
	public RetiredColonyData.RetiredColonyStatistic[] Stats { get; set; }

	// Token: 0x1700058B RID: 1419
	// (get) Token: 0x06004D07 RID: 19719 RVA: 0x001B96B7 File Offset: 0x001B78B7
	// (set) Token: 0x06004D08 RID: 19720 RVA: 0x001B96BF File Offset: 0x001B78BF
	public Dictionary<string, string> worldIdentities { get; set; }

	// Token: 0x1700058C RID: 1420
	// (get) Token: 0x06004D09 RID: 19721 RVA: 0x001B96C8 File Offset: 0x001B78C8
	// (set) Token: 0x06004D0A RID: 19722 RVA: 0x001B96D0 File Offset: 0x001B78D0
	public string startWorld { get; set; }

	// Token: 0x02001A5D RID: 6749
	public static class DataIDs
	{
		// Token: 0x04007C2B RID: 31787
		public static string OxygenProduced = "oxygenProduced";

		// Token: 0x04007C2C RID: 31788
		public static string OxygenConsumed = "oxygenConsumed";

		// Token: 0x04007C2D RID: 31789
		public static string CaloriesProduced = "caloriesProduced";

		// Token: 0x04007C2E RID: 31790
		public static string CaloriesRemoved = "caloriesRemoved";

		// Token: 0x04007C2F RID: 31791
		public static string PowerProduced = "powerProduced";

		// Token: 0x04007C30 RID: 31792
		public static string PowerWasted = "powerWasted";

		// Token: 0x04007C31 RID: 31793
		public static string WorkTime = "workTime";

		// Token: 0x04007C32 RID: 31794
		public static string TravelTime = "travelTime";

		// Token: 0x04007C33 RID: 31795
		public static string AverageWorkTime = "averageWorkTime";

		// Token: 0x04007C34 RID: 31796
		public static string AverageTravelTime = "averageTravelTime";

		// Token: 0x04007C35 RID: 31797
		public static string LiveDuplicants = "liveDuplicants";

		// Token: 0x04007C36 RID: 31798
		public static string AverageStressCreated = "averageStressCreated";

		// Token: 0x04007C37 RID: 31799
		public static string AverageStressRemoved = "averageStressRemoved";

		// Token: 0x04007C38 RID: 31800
		public static string DomesticatedCritters = "domesticatedCritters";

		// Token: 0x04007C39 RID: 31801
		public static string WildCritters = "wildCritters";

		// Token: 0x04007C3A RID: 31802
		public static string AverageGerms = "averageGerms";

		// Token: 0x04007C3B RID: 31803
		public static string RocketsInFlight = "rocketsInFlight";
	}

	// Token: 0x02001A5E RID: 6750
	public class RetiredColonyStatistic
	{
		// Token: 0x06009FC0 RID: 40896 RVA: 0x0037DC1F File Offset: 0x0037BE1F
		public RetiredColonyStatistic()
		{
		}

		// Token: 0x06009FC1 RID: 40897 RVA: 0x0037DC27 File Offset: 0x0037BE27
		public RetiredColonyStatistic(string id, global::Tuple<float, float>[] data, string name, string axisNameX, string axisNameY)
		{
			this.id = id;
			this.value = data;
			this.name = name;
			this.nameX = axisNameX;
			this.nameY = axisNameY;
		}

		// Token: 0x06009FC2 RID: 40898 RVA: 0x0037DC54 File Offset: 0x0037BE54
		public global::Tuple<float, float> GetByMaxValue()
		{
			if (this.value.Length == 0)
			{
				return new global::Tuple<float, float>(0f, 0f);
			}
			int num = -1;
			float num2 = -1f;
			for (int i = 0; i < this.value.Length; i++)
			{
				if (this.value[i].second > num2)
				{
					num2 = this.value[i].second;
					num = i;
				}
			}
			if (num == -1)
			{
				num = 0;
			}
			return this.value[num];
		}

		// Token: 0x06009FC3 RID: 40899 RVA: 0x0037DCC4 File Offset: 0x0037BEC4
		public global::Tuple<float, float> GetByMaxKey()
		{
			if (this.value.Length == 0)
			{
				return new global::Tuple<float, float>(0f, 0f);
			}
			int num = -1;
			float num2 = -1f;
			for (int i = 0; i < this.value.Length; i++)
			{
				if (this.value[i].first > num2)
				{
					num2 = this.value[i].first;
					num = i;
				}
			}
			return this.value[num];
		}

		// Token: 0x04007C3C RID: 31804
		public string id;

		// Token: 0x04007C3D RID: 31805
		public global::Tuple<float, float>[] value;

		// Token: 0x04007C3E RID: 31806
		public string name;

		// Token: 0x04007C3F RID: 31807
		public string nameX;

		// Token: 0x04007C40 RID: 31808
		public string nameY;
	}

	// Token: 0x02001A5F RID: 6751
	public class RetiredDuplicantData
	{
		// Token: 0x04007C41 RID: 31809
		public string name;

		// Token: 0x04007C42 RID: 31810
		public int age;

		// Token: 0x04007C43 RID: 31811
		public int skillPointsGained;

		// Token: 0x04007C44 RID: 31812
		public Dictionary<string, string> accessories;
	}
}
