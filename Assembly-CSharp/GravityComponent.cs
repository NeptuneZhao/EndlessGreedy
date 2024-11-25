using System;
using UnityEngine;

// Token: 0x020008D9 RID: 2265
public struct GravityComponent
{
	// Token: 0x0600409A RID: 16538 RVA: 0x0016FC48 File Offset: 0x0016DE48
	public GravityComponent(Transform transform, System.Action on_landed, Vector2 initial_velocity, bool land_on_fake_floors, bool mayLeaveWorld)
	{
		this.transform = transform;
		this.elapsedTime = 0f;
		this.velocity = initial_velocity;
		this.onLanded = on_landed;
		this.landOnFakeFloors = land_on_fake_floors;
		this.mayLeaveWorld = mayLeaveWorld;
		this.collider2D = transform.GetComponent<KCollider2D>();
		this.extents = GravityComponent.GetExtents(this.collider2D);
	}

	// Token: 0x0600409B RID: 16539 RVA: 0x0016FCA4 File Offset: 0x0016DEA4
	public static float GetGroundOffset(KCollider2D collider)
	{
		if (collider != null)
		{
			return collider.bounds.extents.y - collider.offset.y;
		}
		return 0f;
	}

	// Token: 0x0600409C RID: 16540 RVA: 0x0016FCDF File Offset: 0x0016DEDF
	public static float GetGroundOffset(GravityComponent gravityComponent)
	{
		if (gravityComponent.collider2D != null)
		{
			return gravityComponent.extents.y - gravityComponent.collider2D.offset.y;
		}
		return 0f;
	}

	// Token: 0x0600409D RID: 16541 RVA: 0x0016FD14 File Offset: 0x0016DF14
	public static Vector2 GetExtents(KCollider2D collider)
	{
		if (collider != null)
		{
			return collider.bounds.extents;
		}
		return Vector2.zero;
	}

	// Token: 0x0600409E RID: 16542 RVA: 0x0016FD43 File Offset: 0x0016DF43
	public static Vector2 GetOffset(KCollider2D collider)
	{
		if (collider != null)
		{
			return collider.offset;
		}
		return Vector2.zero;
	}

	// Token: 0x04002A9E RID: 10910
	public Transform transform;

	// Token: 0x04002A9F RID: 10911
	public Vector2 velocity;

	// Token: 0x04002AA0 RID: 10912
	public float elapsedTime;

	// Token: 0x04002AA1 RID: 10913
	public System.Action onLanded;

	// Token: 0x04002AA2 RID: 10914
	public bool landOnFakeFloors;

	// Token: 0x04002AA3 RID: 10915
	public bool mayLeaveWorld;

	// Token: 0x04002AA4 RID: 10916
	public Vector2 extents;

	// Token: 0x04002AA5 RID: 10917
	public KCollider2D collider2D;
}
