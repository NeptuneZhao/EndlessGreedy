using System;
using UnityEngine;

// Token: 0x020008AF RID: 2223
public struct FallerComponent
{
	// Token: 0x06003E0B RID: 15883 RVA: 0x00156A7C File Offset: 0x00154C7C
	public FallerComponent(Transform transform, Vector2 initial_velocity)
	{
		this.transform = transform;
		this.transformInstanceId = transform.GetInstanceID();
		this.isFalling = false;
		this.initialVelocity = initial_velocity;
		this.partitionerEntry = default(HandleVector<int>.Handle);
		this.solidChangedCB = null;
		this.cellChangedCB = null;
		KCircleCollider2D component = transform.GetComponent<KCircleCollider2D>();
		if (component != null)
		{
			this.offset = component.radius;
			return;
		}
		KCollider2D component2 = transform.GetComponent<KCollider2D>();
		if (component2 != null)
		{
			this.offset = transform.GetPosition().y - component2.bounds.min.y;
			return;
		}
		this.offset = 0f;
	}

	// Token: 0x0400261B RID: 9755
	public Transform transform;

	// Token: 0x0400261C RID: 9756
	public int transformInstanceId;

	// Token: 0x0400261D RID: 9757
	public bool isFalling;

	// Token: 0x0400261E RID: 9758
	public float offset;

	// Token: 0x0400261F RID: 9759
	public Vector2 initialVelocity;

	// Token: 0x04002620 RID: 9760
	public HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04002621 RID: 9761
	public Action<object> solidChangedCB;

	// Token: 0x04002622 RID: 9762
	public System.Action cellChangedCB;
}
