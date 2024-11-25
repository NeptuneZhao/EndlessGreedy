using System;
using UnityEngine;

// Token: 0x020000A3 RID: 163
[AddComponentMenu("KMonoBehaviour/scripts/LightSymbolTracker")]
public class LightSymbolTracker : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x06000300 RID: 768 RVA: 0x00017A40 File Offset: 0x00015C40
	public void RenderEveryTick(float dt)
	{
		Vector3 v = Vector3.zero;
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		bool flag;
		v = (component.GetTransformMatrix() * component.GetSymbolLocalTransform(this.targetSymbol, out flag)).MultiplyPoint(Vector3.zero) - base.transform.GetPosition();
		base.GetComponent<Light2D>().Offset = v;
	}

	// Token: 0x040001FA RID: 506
	public HashedString targetSymbol;
}
