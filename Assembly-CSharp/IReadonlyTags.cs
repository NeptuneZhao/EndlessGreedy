using System;

// Token: 0x02000B2D RID: 2861
public interface IReadonlyTags
{
	// Token: 0x0600555A RID: 21850
	bool HasTag(string tag);

	// Token: 0x0600555B RID: 21851
	bool HasTag(int hashtag);

	// Token: 0x0600555C RID: 21852
	bool HasTags(int[] tags);
}
