using System;
using UnityEngine;

// Token: 0x02000923 RID: 2339
[AddComponentMenu("KMonoBehaviour/scripts/SelectMarker")]
public class SelectMarker : KMonoBehaviour
{
	// Token: 0x06004406 RID: 17414 RVA: 0x001822FD File Offset: 0x001804FD
	public void SetTargetTransform(Transform target_transform)
	{
		this.targetTransform = target_transform;
		this.LateUpdate();
	}

	// Token: 0x06004407 RID: 17415 RVA: 0x0018230C File Offset: 0x0018050C
	private void LateUpdate()
	{
		if (this.targetTransform == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		Vector3 position = this.targetTransform.GetPosition();
		KCollider2D component = this.targetTransform.GetComponent<KCollider2D>();
		if (component != null)
		{
			position.x = component.bounds.center.x;
			position.y = component.bounds.center.y + component.bounds.size.y / 2f + 0.1f;
		}
		else
		{
			position.y += 2f;
		}
		Vector3 b = new Vector3(0f, (Mathf.Sin(Time.unscaledTime * 4f) + 1f) * this.animationOffset, 0f);
		base.transform.SetPosition(position + b);
	}

	// Token: 0x04002C8A RID: 11402
	public float animationOffset = 0.1f;

	// Token: 0x04002C8B RID: 11403
	private Transform targetTransform;
}
