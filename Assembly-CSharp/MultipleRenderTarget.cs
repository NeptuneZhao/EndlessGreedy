using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A3E RID: 2622
public class MultipleRenderTarget : MonoBehaviour
{
	// Token: 0x1400001E RID: 30
	// (add) Token: 0x06004BF3 RID: 19443 RVA: 0x001B1B40 File Offset: 0x001AFD40
	// (remove) Token: 0x06004BF4 RID: 19444 RVA: 0x001B1B78 File Offset: 0x001AFD78
	public event Action<Camera> onSetupComplete;

	// Token: 0x06004BF5 RID: 19445 RVA: 0x001B1BAD File Offset: 0x001AFDAD
	private void Start()
	{
		base.StartCoroutine(this.SetupProxy());
	}

	// Token: 0x06004BF6 RID: 19446 RVA: 0x001B1BBC File Offset: 0x001AFDBC
	private IEnumerator SetupProxy()
	{
		yield return null;
		Camera component = base.GetComponent<Camera>();
		Camera camera = new GameObject().AddComponent<Camera>();
		camera.CopyFrom(component);
		this.renderProxy = camera.gameObject.AddComponent<MultipleRenderTargetProxy>();
		camera.name = component.name + " MRT";
		camera.transform.parent = component.transform;
		camera.transform.SetLocalPosition(Vector3.zero);
		camera.depth = component.depth - 1f;
		component.cullingMask = 0;
		component.clearFlags = CameraClearFlags.Color;
		this.quad = new FullScreenQuad("MultipleRenderTarget", component, true);
		if (this.onSetupComplete != null)
		{
			this.onSetupComplete(camera);
		}
		yield break;
	}

	// Token: 0x06004BF7 RID: 19447 RVA: 0x001B1BCB File Offset: 0x001AFDCB
	private void OnPreCull()
	{
		if (this.renderProxy != null)
		{
			this.quad.Draw(this.renderProxy.Textures[0]);
		}
	}

	// Token: 0x06004BF8 RID: 19448 RVA: 0x001B1BF3 File Offset: 0x001AFDF3
	public void ToggleColouredOverlayView(bool enabled)
	{
		if (this.renderProxy != null)
		{
			this.renderProxy.ToggleColouredOverlayView(enabled);
		}
	}

	// Token: 0x04003271 RID: 12913
	private MultipleRenderTargetProxy renderProxy;

	// Token: 0x04003272 RID: 12914
	private FullScreenQuad quad;

	// Token: 0x04003274 RID: 12916
	public bool isFrontEnd;
}
