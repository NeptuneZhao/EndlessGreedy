using System;
using UnityEngine;

// Token: 0x02000C63 RID: 3171
[AddComponentMenu("KMonoBehaviour/scripts/HasSortOrder")]
public class HasSortOrder : KMonoBehaviour, IHasSortOrder
{
	// Token: 0x1700073A RID: 1850
	// (get) Token: 0x0600614A RID: 24906 RVA: 0x00244054 File Offset: 0x00242254
	// (set) Token: 0x0600614B RID: 24907 RVA: 0x0024405C File Offset: 0x0024225C
	public int sortOrder { get; set; }
}
