using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A74 RID: 2676
public class RunningWeightedAverage
{
	// Token: 0x06004DE3 RID: 19939 RVA: 0x001BF6C4 File Offset: 0x001BD8C4
	public RunningWeightedAverage(float minValue = -3.4028235E+38f, float maxValue = 3.4028235E+38f, int sampleCount = 20, bool allowZero = true)
	{
		this.min = minValue;
		this.max = maxValue;
		this.ignoreZero = !allowZero;
		this.samples = new List<global::Tuple<float, float>>();
	}

	// Token: 0x1700059E RID: 1438
	// (get) Token: 0x06004DE4 RID: 19940 RVA: 0x001BF703 File Offset: 0x001BD903
	public float GetUnweightedAverage
	{
		get
		{
			return this.GetAverageOfLastSeconds(4f);
		}
	}

	// Token: 0x1700059F RID: 1439
	// (get) Token: 0x06004DE5 RID: 19941 RVA: 0x001BF710 File Offset: 0x001BD910
	public bool HasEverHadValidValues
	{
		get
		{
			return this.validSampleCount >= this.maxSamples;
		}
	}

	// Token: 0x06004DE6 RID: 19942 RVA: 0x001BF724 File Offset: 0x001BD924
	public void AddSample(float value, float timeOfRecord)
	{
		if (this.ignoreZero && value == 0f)
		{
			return;
		}
		if (value > this.max)
		{
			value = this.max;
		}
		if (value < this.min)
		{
			value = this.min;
		}
		if (this.validSampleCount <= this.maxSamples)
		{
			this.validSampleCount++;
		}
		this.samples.Add(new global::Tuple<float, float>(value, timeOfRecord));
		if (this.samples.Count > this.maxSamples)
		{
			this.samples.RemoveAt(0);
		}
	}

	// Token: 0x06004DE7 RID: 19943 RVA: 0x001BF7B4 File Offset: 0x001BD9B4
	public int ValidRecordsInLastSeconds(float seconds)
	{
		int num = 0;
		int num2 = this.samples.Count - 1;
		while (num2 >= 0 && Time.time - this.samples[num2].second <= seconds)
		{
			num++;
			num2--;
		}
		return num;
	}

	// Token: 0x06004DE8 RID: 19944 RVA: 0x001BF7FC File Offset: 0x001BD9FC
	private float GetAverageOfLastSeconds(float seconds)
	{
		float num = 0f;
		int num2 = 0;
		int num3 = this.samples.Count - 1;
		while (num3 >= 0 && Time.time - this.samples[num3].second <= seconds)
		{
			num += this.samples[num3].first;
			num2++;
			num3--;
		}
		if (num2 == 0)
		{
			return 0f;
		}
		return num / (float)num2;
	}

	// Token: 0x040033DA RID: 13274
	private List<global::Tuple<float, float>> samples = new List<global::Tuple<float, float>>();

	// Token: 0x040033DB RID: 13275
	private float min;

	// Token: 0x040033DC RID: 13276
	private float max;

	// Token: 0x040033DD RID: 13277
	private bool ignoreZero;

	// Token: 0x040033DE RID: 13278
	private int validSampleCount;

	// Token: 0x040033DF RID: 13279
	private int maxSamples = 20;
}
