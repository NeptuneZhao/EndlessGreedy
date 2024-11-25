using System;
using UnityEngine;

// Token: 0x02000CF3 RID: 3315
public class NonLinearSlider : KSlider
{
	// Token: 0x060066CA RID: 26314 RVA: 0x00266521 File Offset: 0x00264721
	public static NonLinearSlider.Range[] GetDefaultRange(float maxValue)
	{
		return new NonLinearSlider.Range[]
		{
			new NonLinearSlider.Range(100f, maxValue)
		};
	}

	// Token: 0x060066CB RID: 26315 RVA: 0x0026653B File Offset: 0x0026473B
	protected override void Start()
	{
		base.Start();
		base.minValue = 0f;
		base.maxValue = 100f;
	}

	// Token: 0x060066CC RID: 26316 RVA: 0x00266559 File Offset: 0x00264759
	public void SetRanges(NonLinearSlider.Range[] ranges)
	{
		this.ranges = ranges;
	}

	// Token: 0x060066CD RID: 26317 RVA: 0x00266564 File Offset: 0x00264764
	public float GetPercentageFromValue(float value)
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < this.ranges.Length; i++)
		{
			if (value >= num2 && value <= this.ranges[i].peakValue)
			{
				float t = (value - num2) / (this.ranges[i].peakValue - num2);
				return Mathf.Lerp(num, num + this.ranges[i].width, t);
			}
			num += this.ranges[i].width;
			num2 = this.ranges[i].peakValue;
		}
		return 100f;
	}

	// Token: 0x060066CE RID: 26318 RVA: 0x00266608 File Offset: 0x00264808
	public float GetValueForPercentage(float percentage)
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < this.ranges.Length; i++)
		{
			if (percentage >= num && num + this.ranges[i].width >= percentage)
			{
				float t = (percentage - num) / this.ranges[i].width;
				return Mathf.Lerp(num2, this.ranges[i].peakValue, t);
			}
			num += this.ranges[i].width;
			num2 = this.ranges[i].peakValue;
		}
		return num2;
	}

	// Token: 0x060066CF RID: 26319 RVA: 0x002666A4 File Offset: 0x002648A4
	protected override void Set(float input, bool sendCallback)
	{
		base.Set(input, sendCallback);
	}

	// Token: 0x04004550 RID: 17744
	public NonLinearSlider.Range[] ranges;

	// Token: 0x02001DFD RID: 7677
	[Serializable]
	public struct Range
	{
		// Token: 0x0600AA38 RID: 43576 RVA: 0x003A118A File Offset: 0x0039F38A
		public Range(float width, float peakValue)
		{
			this.width = width;
			this.peakValue = peakValue;
		}

		// Token: 0x040088EC RID: 35052
		public float width;

		// Token: 0x040088ED RID: 35053
		public float peakValue;
	}
}
