using System;
using UnityEngine;

// Token: 0x02000A2E RID: 2606
public class CameraReferenceTexture : MonoBehaviour
{
	// Token: 0x06004B80 RID: 19328 RVA: 0x001AE390 File Offset: 0x001AC590
	private void OnPreCull()
	{
		if (this.quad == null)
		{
			this.quad = new FullScreenQuad("CameraReferenceTexture", base.GetComponent<Camera>(), this.referenceCamera.GetComponent<CameraRenderTexture>().ShouldFlip());
		}
		if (this.referenceCamera != null)
		{
			this.quad.Draw(this.referenceCamera.GetComponent<CameraRenderTexture>().GetTexture());
		}
	}

	// Token: 0x04003177 RID: 12663
	public Camera referenceCamera;

	// Token: 0x04003178 RID: 12664
	private FullScreenQuad quad;
}
