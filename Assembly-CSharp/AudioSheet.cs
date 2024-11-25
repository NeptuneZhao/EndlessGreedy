using System;
using UnityEngine;

// Token: 0x0200050A RID: 1290
[Serializable]
public class AudioSheet
{
	// Token: 0x0400102F RID: 4143
	public TextAsset asset;

	// Token: 0x04001030 RID: 4144
	public string defaultType;

	// Token: 0x04001031 RID: 4145
	public AudioSheet.SoundInfo[] soundInfos;

	// Token: 0x020012D0 RID: 4816
	public class SoundInfo : Resource
	{
		// Token: 0x0400648A RID: 25738
		public string File;

		// Token: 0x0400648B RID: 25739
		public string Anim;

		// Token: 0x0400648C RID: 25740
		public string Type;

		// Token: 0x0400648D RID: 25741
		public string RequiredDlcId;

		// Token: 0x0400648E RID: 25742
		public float MinInterval;

		// Token: 0x0400648F RID: 25743
		public string Name0;

		// Token: 0x04006490 RID: 25744
		public int Frame0;

		// Token: 0x04006491 RID: 25745
		public string Name1;

		// Token: 0x04006492 RID: 25746
		public int Frame1;

		// Token: 0x04006493 RID: 25747
		public string Name2;

		// Token: 0x04006494 RID: 25748
		public int Frame2;

		// Token: 0x04006495 RID: 25749
		public string Name3;

		// Token: 0x04006496 RID: 25750
		public int Frame3;

		// Token: 0x04006497 RID: 25751
		public string Name4;

		// Token: 0x04006498 RID: 25752
		public int Frame4;

		// Token: 0x04006499 RID: 25753
		public string Name5;

		// Token: 0x0400649A RID: 25754
		public int Frame5;

		// Token: 0x0400649B RID: 25755
		public string Name6;

		// Token: 0x0400649C RID: 25756
		public int Frame6;

		// Token: 0x0400649D RID: 25757
		public string Name7;

		// Token: 0x0400649E RID: 25758
		public int Frame7;

		// Token: 0x0400649F RID: 25759
		public string Name8;

		// Token: 0x040064A0 RID: 25760
		public int Frame8;

		// Token: 0x040064A1 RID: 25761
		public string Name9;

		// Token: 0x040064A2 RID: 25762
		public int Frame9;

		// Token: 0x040064A3 RID: 25763
		public string Name10;

		// Token: 0x040064A4 RID: 25764
		public int Frame10;

		// Token: 0x040064A5 RID: 25765
		public string Name11;

		// Token: 0x040064A6 RID: 25766
		public int Frame11;
	}
}
