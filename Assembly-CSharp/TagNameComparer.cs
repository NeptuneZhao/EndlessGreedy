using System;
using System.Collections.Generic;

// Token: 0x020008C5 RID: 2245
public class TagNameComparer : IComparer<Tag>
{
	// Token: 0x06003F2F RID: 16175 RVA: 0x00163CD1 File Offset: 0x00161ED1
	public TagNameComparer()
	{
	}

	// Token: 0x06003F30 RID: 16176 RVA: 0x00163CD9 File Offset: 0x00161ED9
	public TagNameComparer(Tag firstTag)
	{
		this.firstTag = firstTag;
	}

	// Token: 0x06003F31 RID: 16177 RVA: 0x00163CE8 File Offset: 0x00161EE8
	public int Compare(Tag x, Tag y)
	{
		if (x == y)
		{
			return 0;
		}
		if (this.firstTag.IsValid)
		{
			if (x == this.firstTag && y != this.firstTag)
			{
				return 1;
			}
			if (x != this.firstTag && y == this.firstTag)
			{
				return -1;
			}
		}
		return x.ProperNameStripLink().CompareTo(y.ProperNameStripLink());
	}

	// Token: 0x04002906 RID: 10502
	private Tag firstTag;
}
