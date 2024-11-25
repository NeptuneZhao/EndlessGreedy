using System;
using UnityEngine;

// Token: 0x02000525 RID: 1317
[AddComponentMenu("KMonoBehaviour/scripts/AnimEventHandler")]
public class AnimEventHandler : KMonoBehaviour
{
	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06001D93 RID: 7571 RVA: 0x000A4658 File Offset: 0x000A2858
	// (remove) Token: 0x06001D94 RID: 7572 RVA: 0x000A4690 File Offset: 0x000A2890
	private event AnimEventHandler.SetPos onWorkTargetSet;

	// Token: 0x06001D95 RID: 7573 RVA: 0x000A46C5 File Offset: 0x000A28C5
	public int GetCachedCell()
	{
		return this.pickupable.cachedCell;
	}

	// Token: 0x06001D96 RID: 7574 RVA: 0x000A46D4 File Offset: 0x000A28D4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.cachedTransform = base.transform;
		this.pickupable = base.GetComponent<Pickupable>();
		foreach (KBatchedAnimTracker kbatchedAnimTracker in base.GetComponentsInChildren<KBatchedAnimTracker>(true))
		{
			if (kbatchedAnimTracker.useTargetPoint)
			{
				this.onWorkTargetSet += kbatchedAnimTracker.SetTarget;
			}
		}
		this.baseOffset = this.animCollider.offset;
		AnimEventHandlerManager.Instance.Add(this);
	}

	// Token: 0x06001D97 RID: 7575 RVA: 0x000A474F File Offset: 0x000A294F
	protected override void OnCleanUp()
	{
		AnimEventHandlerManager.Instance.Remove(this);
	}

	// Token: 0x06001D98 RID: 7576 RVA: 0x000A475C File Offset: 0x000A295C
	protected override void OnForcedCleanUp()
	{
		this.navigator = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06001D99 RID: 7577 RVA: 0x000A476B File Offset: 0x000A296B
	public HashedString GetContext()
	{
		return this.context;
	}

	// Token: 0x06001D9A RID: 7578 RVA: 0x000A4773 File Offset: 0x000A2973
	public void UpdateWorkTarget(Vector3 pos)
	{
		if (this.onWorkTargetSet != null)
		{
			this.onWorkTargetSet(pos);
		}
	}

	// Token: 0x06001D9B RID: 7579 RVA: 0x000A4789 File Offset: 0x000A2989
	public void SetContext(HashedString context)
	{
		this.context = context;
	}

	// Token: 0x06001D9C RID: 7580 RVA: 0x000A4792 File Offset: 0x000A2992
	public void SetTargetPos(Vector3 target_pos)
	{
		this.targetPos = target_pos;
	}

	// Token: 0x06001D9D RID: 7581 RVA: 0x000A479B File Offset: 0x000A299B
	public Vector3 GetTargetPos()
	{
		return this.targetPos;
	}

	// Token: 0x06001D9E RID: 7582 RVA: 0x000A47A3 File Offset: 0x000A29A3
	public void ClearContext()
	{
		this.context = default(HashedString);
	}

	// Token: 0x06001D9F RID: 7583 RVA: 0x000A47B4 File Offset: 0x000A29B4
	public void UpdateOffset()
	{
		Vector3 pivotSymbolPosition = this.controller.GetPivotSymbolPosition();
		Vector3 vector = this.navigator.NavGrid.GetNavTypeData(this.navigator.CurrentNavType).animControllerOffset;
		Vector3 position = this.cachedTransform.position;
		Vector2 vector2 = new Vector2(this.baseOffset.x + pivotSymbolPosition.x - position.x - vector.x, this.baseOffset.y + pivotSymbolPosition.y - position.y + vector.y);
		if (this.animCollider.offset != vector2)
		{
			this.animCollider.offset = vector2;
		}
	}

	// Token: 0x0400109F RID: 4255
	[MyCmpGet]
	private KBatchedAnimController controller;

	// Token: 0x040010A0 RID: 4256
	[MyCmpGet]
	private KBoxCollider2D animCollider;

	// Token: 0x040010A1 RID: 4257
	[MyCmpGet]
	private Navigator navigator;

	// Token: 0x040010A2 RID: 4258
	private Pickupable pickupable;

	// Token: 0x040010A3 RID: 4259
	private Vector3 targetPos;

	// Token: 0x040010A4 RID: 4260
	public Transform cachedTransform;

	// Token: 0x040010A6 RID: 4262
	public Vector2 baseOffset;

	// Token: 0x040010A7 RID: 4263
	private HashedString context;

	// Token: 0x020012DF RID: 4831
	// (Invoke) Token: 0x060084FE RID: 34046
	private delegate void SetPos(Vector3 pos);
}
