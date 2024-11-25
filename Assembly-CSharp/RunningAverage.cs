using System;

// Token: 0x02000A73 RID: 2675
public class RunningAverage
{
	// Token: 0x06004DDF RID: 19935 RVA: 0x001BF5BA File Offset: 0x001BD7BA
	public RunningAverage(float minValue = -3.4028235E+38f, float maxValue = 3.4028235E+38f, int sampleCount = 15, bool allowZero = true)
	{
		this.min = minValue;
		this.max = maxValue;
		this.ignoreZero = !allowZero;
		this.samples = new float[sampleCount];
	}

	// Token: 0x1700059D RID: 1437
	// (get) Token: 0x06004DE0 RID: 19936 RVA: 0x001BF5E7 File Offset: 0x001BD7E7
	public float AverageValue
	{
		get
		{
			return this.GetAverage();
		}
	}

	// Token: 0x06004DE1 RID: 19937 RVA: 0x001BF5F0 File Offset: 0x001BD7F0
	public void AddSample(float value)
	{
		if (value < this.min || value > this.max || (this.ignoreZero && value == 0f))
		{
			return;
		}
		if (this.validValues < this.samples.Length)
		{
			this.validValues++;
		}
		for (int i = 0; i < this.samples.Length - 1; i++)
		{
			this.samples[i] = this.samples[i + 1];
		}
		this.samples[this.samples.Length - 1] = value;
	}

	// Token: 0x06004DE2 RID: 19938 RVA: 0x001BF678 File Offset: 0x001BD878
	private float GetAverage()
	{
		float num = 0f;
		for (int i = this.samples.Length - 1; i > this.samples.Length - 1 - this.validValues; i--)
		{
			num += this.samples[i];
		}
		return num / (float)this.validValues;
	}

	// Token: 0x040033D5 RID: 13269
	private float[] samples;

	// Token: 0x040033D6 RID: 13270
	private float min;

	// Token: 0x040033D7 RID: 13271
	private float max;

	// Token: 0x040033D8 RID: 13272
	private bool ignoreZero;

	// Token: 0x040033D9 RID: 13273
	private int validValues;
}
