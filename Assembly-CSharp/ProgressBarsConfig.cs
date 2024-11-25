using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D18 RID: 3352
public class ProgressBarsConfig : ScriptableObject
{
	// Token: 0x060068B1 RID: 26801 RVA: 0x00273526 File Offset: 0x00271726
	public static void DestroyInstance()
	{
		ProgressBarsConfig.instance = null;
	}

	// Token: 0x17000772 RID: 1906
	// (get) Token: 0x060068B2 RID: 26802 RVA: 0x0027352E File Offset: 0x0027172E
	public static ProgressBarsConfig Instance
	{
		get
		{
			if (ProgressBarsConfig.instance == null)
			{
				ProgressBarsConfig.instance = Resources.Load<ProgressBarsConfig>("ProgressBarsConfig");
				ProgressBarsConfig.instance.Initialize();
			}
			return ProgressBarsConfig.instance;
		}
	}

	// Token: 0x060068B3 RID: 26803 RVA: 0x0027355C File Offset: 0x0027175C
	public void Initialize()
	{
		foreach (ProgressBarsConfig.BarData barData in this.barColorDataList)
		{
			this.barColorMap.Add(barData.barName, barData);
		}
	}

	// Token: 0x060068B4 RID: 26804 RVA: 0x002735BC File Offset: 0x002717BC
	public string GetBarDescription(string barName)
	{
		string result = "";
		if (this.IsBarNameValid(barName))
		{
			result = Strings.Get(this.barColorMap[barName].barDescriptionKey);
		}
		return result;
	}

	// Token: 0x060068B5 RID: 26805 RVA: 0x002735F8 File Offset: 0x002717F8
	public Color GetBarColor(string barName)
	{
		Color result = Color.clear;
		if (this.IsBarNameValid(barName))
		{
			result = this.barColorMap[barName].barColor;
		}
		return result;
	}

	// Token: 0x060068B6 RID: 26806 RVA: 0x00273627 File Offset: 0x00271827
	public bool IsBarNameValid(string barName)
	{
		if (string.IsNullOrEmpty(barName))
		{
			global::Debug.LogError("The barName provided was null or empty. Don't do that.");
			return false;
		}
		if (!this.barColorMap.ContainsKey(barName))
		{
			global::Debug.LogError(string.Format("No BarData found for the entry [ {0} ]", barName));
			return false;
		}
		return true;
	}

	// Token: 0x040046D3 RID: 18131
	public GameObject progressBarPrefab;

	// Token: 0x040046D4 RID: 18132
	public GameObject progressBarUIPrefab;

	// Token: 0x040046D5 RID: 18133
	public GameObject healthBarPrefab;

	// Token: 0x040046D6 RID: 18134
	public List<ProgressBarsConfig.BarData> barColorDataList = new List<ProgressBarsConfig.BarData>();

	// Token: 0x040046D7 RID: 18135
	public Dictionary<string, ProgressBarsConfig.BarData> barColorMap = new Dictionary<string, ProgressBarsConfig.BarData>();

	// Token: 0x040046D8 RID: 18136
	private static ProgressBarsConfig instance;

	// Token: 0x02001E3A RID: 7738
	[Serializable]
	public struct BarData
	{
		// Token: 0x040089D7 RID: 35287
		public string barName;

		// Token: 0x040089D8 RID: 35288
		public Color barColor;

		// Token: 0x040089D9 RID: 35289
		public string barDescriptionKey;
	}
}
