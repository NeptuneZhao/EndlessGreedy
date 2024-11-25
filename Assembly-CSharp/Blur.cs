using System;
using UnityEngine;

// Token: 0x02000A2D RID: 2605
public static class Blur
{
	// Token: 0x06004B7F RID: 19327 RVA: 0x001AE36A File Offset: 0x001AC56A
	public static RenderTexture Run(Texture2D image)
	{
		if (Blur.blurMaterial == null)
		{
			Blur.blurMaterial = new Material(Shader.Find("Klei/PostFX/Blur"));
		}
		return null;
	}

	// Token: 0x04003176 RID: 12662
	private static Material blurMaterial;
}
