using System;
using UnityEngine;

// Token: 0x02000B74 RID: 2932
public abstract class VisualizerEffect : MonoBehaviour
{
	// Token: 0x0600581B RID: 22555
	protected abstract void SetupMaterial();

	// Token: 0x0600581C RID: 22556
	protected abstract void SetupOcclusionTex();

	// Token: 0x0600581D RID: 22557
	protected abstract void OnPostRender();

	// Token: 0x0600581E RID: 22558 RVA: 0x001FD805 File Offset: 0x001FBA05
	protected virtual void Start()
	{
		this.SetupMaterial();
		this.SetupOcclusionTex();
		this.myCamera = base.GetComponent<Camera>();
	}

	// Token: 0x04003998 RID: 14744
	protected Material material;

	// Token: 0x04003999 RID: 14745
	protected Camera myCamera;

	// Token: 0x0400399A RID: 14746
	protected Texture2D OcclusionTex;
}
