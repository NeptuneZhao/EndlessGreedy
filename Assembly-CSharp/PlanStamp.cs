using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D0F RID: 3343
[AddComponentMenu("KMonoBehaviour/scripts/PlanStamp")]
public class PlanStamp : KMonoBehaviour
{
	// Token: 0x0600685E RID: 26718 RVA: 0x00271647 File Offset: 0x0026F847
	public void SetStamp(Sprite sprite, string Text)
	{
		this.StampImage.sprite = sprite;
		this.StampText.text = Text.ToUpper();
	}

	// Token: 0x04004684 RID: 18052
	public PlanStamp.StampArt stampSprites;

	// Token: 0x04004685 RID: 18053
	[SerializeField]
	private Image StampImage;

	// Token: 0x04004686 RID: 18054
	[SerializeField]
	private Text StampText;

	// Token: 0x02001E38 RID: 7736
	[Serializable]
	public struct StampArt
	{
		// Token: 0x040089C8 RID: 35272
		public Sprite UnderConstruction;

		// Token: 0x040089C9 RID: 35273
		public Sprite NeedsResearch;

		// Token: 0x040089CA RID: 35274
		public Sprite SelectResource;

		// Token: 0x040089CB RID: 35275
		public Sprite NeedsRepair;

		// Token: 0x040089CC RID: 35276
		public Sprite NeedsPower;

		// Token: 0x040089CD RID: 35277
		public Sprite NeedsResource;

		// Token: 0x040089CE RID: 35278
		public Sprite NeedsGasPipe;

		// Token: 0x040089CF RID: 35279
		public Sprite NeedsLiquidPipe;
	}
}
