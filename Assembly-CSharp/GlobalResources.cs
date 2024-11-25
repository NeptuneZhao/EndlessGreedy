using System;
using FMODUnity;
using UnityEngine;

// Token: 0x02000B7D RID: 2941
public class GlobalResources : ScriptableObject
{
	// Token: 0x06005861 RID: 22625 RVA: 0x001FE2B0 File Offset: 0x001FC4B0
	public static GlobalResources Instance()
	{
		if (GlobalResources._Instance == null)
		{
			GlobalResources._Instance = Resources.Load<GlobalResources>("GlobalResources");
		}
		return GlobalResources._Instance;
	}

	// Token: 0x04003A00 RID: 14848
	public Material AnimMaterial;

	// Token: 0x04003A01 RID: 14849
	public Material AnimUIMaterial;

	// Token: 0x04003A02 RID: 14850
	public Material AnimPlaceMaterial;

	// Token: 0x04003A03 RID: 14851
	public Material AnimMaterialUIDesaturated;

	// Token: 0x04003A04 RID: 14852
	public Material AnimSimpleMaterial;

	// Token: 0x04003A05 RID: 14853
	public Material AnimOverlayMaterial;

	// Token: 0x04003A06 RID: 14854
	public Texture2D WhiteTexture;

	// Token: 0x04003A07 RID: 14855
	public EventReference ConduitOverlaySoundLiquid;

	// Token: 0x04003A08 RID: 14856
	public EventReference ConduitOverlaySoundGas;

	// Token: 0x04003A09 RID: 14857
	public EventReference ConduitOverlaySoundSolid;

	// Token: 0x04003A0A RID: 14858
	public EventReference AcousticDisturbanceSound;

	// Token: 0x04003A0B RID: 14859
	public EventReference AcousticDisturbanceBubbleSound;

	// Token: 0x04003A0C RID: 14860
	public EventReference WallDamageLayerSound;

	// Token: 0x04003A0D RID: 14861
	public Sprite sadDupeAudio;

	// Token: 0x04003A0E RID: 14862
	public Sprite sadDupe;

	// Token: 0x04003A0F RID: 14863
	public Sprite baseGameLogoSmall;

	// Token: 0x04003A10 RID: 14864
	public Sprite expansion1LogoSmall;

	// Token: 0x04003A11 RID: 14865
	private static GlobalResources _Instance;
}
