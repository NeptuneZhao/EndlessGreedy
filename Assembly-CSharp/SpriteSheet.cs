using System;
using UnityEngine;

// Token: 0x020005BE RID: 1470
[Serializable]
public struct SpriteSheet
{
	// Token: 0x040013EA RID: 5098
	public string name;

	// Token: 0x040013EB RID: 5099
	public int numFrames;

	// Token: 0x040013EC RID: 5100
	public int numXFrames;

	// Token: 0x040013ED RID: 5101
	public Vector2 uvFrameSize;

	// Token: 0x040013EE RID: 5102
	public int renderLayer;

	// Token: 0x040013EF RID: 5103
	public Material material;

	// Token: 0x040013F0 RID: 5104
	public Texture2D texture;
}
