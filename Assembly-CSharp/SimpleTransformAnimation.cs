using System;
using UnityEngine;

// Token: 0x02000DBE RID: 3518
public class SimpleTransformAnimation : MonoBehaviour
{
	// Token: 0x06006F86 RID: 28550 RVA: 0x0029F5E7 File Offset: 0x0029D7E7
	private void Start()
	{
	}

	// Token: 0x06006F87 RID: 28551 RVA: 0x0029F5E9 File Offset: 0x0029D7E9
	private void Update()
	{
		base.transform.Rotate(this.rotationSpeed * Time.unscaledDeltaTime);
		base.transform.Translate(this.translateSpeed * Time.unscaledDeltaTime);
	}

	// Token: 0x04004C31 RID: 19505
	[SerializeField]
	private Vector3 rotationSpeed;

	// Token: 0x04004C32 RID: 19506
	[SerializeField]
	private Vector3 translateSpeed;
}
