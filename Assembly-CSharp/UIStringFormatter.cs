using System;
using System.Collections.Generic;

// Token: 0x02000BC3 RID: 3011
public class UIStringFormatter
{
	// Token: 0x04003D6B RID: 15723
	private List<UIStringFormatter.Entry> entries = new List<UIStringFormatter.Entry>();

	// Token: 0x02001CAF RID: 7343
	private struct Entry
	{
		// Token: 0x040084CE RID: 33998
		public string format;

		// Token: 0x040084CF RID: 33999
		public string key;

		// Token: 0x040084D0 RID: 34000
		public string value;

		// Token: 0x040084D1 RID: 34001
		public string result;
	}
}
