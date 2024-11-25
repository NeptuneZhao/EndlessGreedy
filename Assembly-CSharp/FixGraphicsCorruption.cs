using System;
using UnityEngine;

// Token: 0x02000B78 RID: 2936
public class FixGraphicsCorruption : MonoBehaviour
{
	// Token: 0x0600582C RID: 22572 RVA: 0x001FDC78 File Offset: 0x001FBE78
	private void Start()
	{
		Camera component = base.GetComponent<Camera>();
		component.transparencySortMode = TransparencySortMode.Orthographic;
		component.tag = "Untagged";
	}

	// Token: 0x0600582D RID: 22573 RVA: 0x001FDC91 File Offset: 0x001FBE91
	private void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		Graphics.Blit(source, dest);
	}
}
