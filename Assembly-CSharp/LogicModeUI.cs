using System;
using UnityEngine;

// Token: 0x02000BA8 RID: 2984
public class LogicModeUI : ScriptableObject
{
	// Token: 0x04003B62 RID: 15202
	[Header("Base Assets")]
	public Sprite inputSprite;

	// Token: 0x04003B63 RID: 15203
	public Sprite outputSprite;

	// Token: 0x04003B64 RID: 15204
	public Sprite resetSprite;

	// Token: 0x04003B65 RID: 15205
	public GameObject prefab;

	// Token: 0x04003B66 RID: 15206
	public GameObject ribbonInputPrefab;

	// Token: 0x04003B67 RID: 15207
	public GameObject ribbonOutputPrefab;

	// Token: 0x04003B68 RID: 15208
	public GameObject controlInputPrefab;

	// Token: 0x04003B69 RID: 15209
	[Header("Colouring")]
	public Color32 colourOn = new Color32(0, byte.MaxValue, 0, 0);

	// Token: 0x04003B6A RID: 15210
	public Color32 colourOff = new Color32(byte.MaxValue, 0, 0, 0);

	// Token: 0x04003B6B RID: 15211
	public Color32 colourDisconnected = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04003B6C RID: 15212
	public Color32 colourOnProtanopia = new Color32(179, 204, 0, 0);

	// Token: 0x04003B6D RID: 15213
	public Color32 colourOffProtanopia = new Color32(166, 51, 102, 0);

	// Token: 0x04003B6E RID: 15214
	public Color32 colourOnDeuteranopia = new Color32(128, 0, 128, 0);

	// Token: 0x04003B6F RID: 15215
	public Color32 colourOffDeuteranopia = new Color32(byte.MaxValue, 153, 0, 0);

	// Token: 0x04003B70 RID: 15216
	public Color32 colourOnTritanopia = new Color32(51, 102, byte.MaxValue, 0);

	// Token: 0x04003B71 RID: 15217
	public Color32 colourOffTritanopia = new Color32(byte.MaxValue, 153, 0, 0);
}
