using System;
using System.Collections.Generic;

// Token: 0x02000BC2 RID: 3010
public class UIFloatFormatter
{
	// Token: 0x06005BBA RID: 23482 RVA: 0x002171B3 File Offset: 0x002153B3
	public string Format(string format, float value)
	{
		return this.Replace(format, "{0}", value);
	}

	// Token: 0x06005BBB RID: 23483 RVA: 0x002171C4 File Offset: 0x002153C4
	private string Replace(string format, string key, float value)
	{
		UIFloatFormatter.Entry entry = default(UIFloatFormatter.Entry);
		if (this.activeStringCount >= this.entries.Count)
		{
			entry.format = format;
			entry.key = key;
			entry.value = value;
			entry.result = entry.format.Replace(key, value.ToString());
			this.entries.Add(entry);
		}
		else
		{
			entry = this.entries[this.activeStringCount];
			if (entry.format != format || entry.key != key || entry.value != value)
			{
				entry.format = format;
				entry.key = key;
				entry.value = value;
				entry.result = entry.format.Replace(key, value.ToString());
				this.entries[this.activeStringCount] = entry;
			}
		}
		this.activeStringCount++;
		return entry.result;
	}

	// Token: 0x06005BBC RID: 23484 RVA: 0x002172BB File Offset: 0x002154BB
	public void BeginDrawing()
	{
		this.activeStringCount = 0;
	}

	// Token: 0x06005BBD RID: 23485 RVA: 0x002172C4 File Offset: 0x002154C4
	public void EndDrawing()
	{
	}

	// Token: 0x04003D69 RID: 15721
	private int activeStringCount;

	// Token: 0x04003D6A RID: 15722
	private List<UIFloatFormatter.Entry> entries = new List<UIFloatFormatter.Entry>();

	// Token: 0x02001CAE RID: 7342
	private struct Entry
	{
		// Token: 0x040084CA RID: 33994
		public string format;

		// Token: 0x040084CB RID: 33995
		public string key;

		// Token: 0x040084CC RID: 33996
		public float value;

		// Token: 0x040084CD RID: 33997
		public string result;
	}
}
