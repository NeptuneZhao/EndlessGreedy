using System;
using System.Collections.Generic;

// Token: 0x02000962 RID: 2402
public interface IGroupProber
{
	// Token: 0x06004639 RID: 17977
	void Occupy(object prober, short serial_no, IEnumerable<int> cells);

	// Token: 0x0600463A RID: 17978
	void SetValidSerialNos(object prober, short previous_serial_no, short serial_no);

	// Token: 0x0600463B RID: 17979
	bool ReleaseProber(object prober);
}
