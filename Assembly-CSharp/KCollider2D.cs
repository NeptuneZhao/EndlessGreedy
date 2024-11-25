using System;
using UnityEngine;

// Token: 0x02000577 RID: 1399
public abstract class KCollider2D : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x17000159 RID: 345
	// (get) Token: 0x0600206B RID: 8299 RVA: 0x000B5C28 File Offset: 0x000B3E28
	// (set) Token: 0x0600206C RID: 8300 RVA: 0x000B5C30 File Offset: 0x000B3E30
	public Vector2 offset
	{
		get
		{
			return this._offset;
		}
		set
		{
			this._offset = value;
			this.MarkDirty(false);
		}
	}

	// Token: 0x0600206D RID: 8301 RVA: 0x000B5C40 File Offset: 0x000B3E40
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.autoRegisterSimRender = false;
	}

	// Token: 0x0600206E RID: 8302 RVA: 0x000B5C4F File Offset: 0x000B3E4F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Singleton<CellChangeMonitor>.Instance.RegisterMovementStateChanged(base.transform, new Action<Transform, bool>(KCollider2D.OnMovementStateChanged));
		this.MarkDirty(true);
	}

	// Token: 0x0600206F RID: 8303 RVA: 0x000B5C7A File Offset: 0x000B3E7A
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Singleton<CellChangeMonitor>.Instance.UnregisterMovementStateChanged(base.transform, new Action<Transform, bool>(KCollider2D.OnMovementStateChanged));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x06002070 RID: 8304 RVA: 0x000B5CB0 File Offset: 0x000B3EB0
	public void MarkDirty(bool force = false)
	{
		bool flag = force || this.partitionerEntry.IsValid();
		if (!flag)
		{
			return;
		}
		Extents extents = this.GetExtents();
		if (!force && this.cachedExtents.x == extents.x && this.cachedExtents.y == extents.y && this.cachedExtents.width == extents.width && this.cachedExtents.height == extents.height)
		{
			return;
		}
		this.cachedExtents = extents;
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		if (flag)
		{
			this.partitionerEntry = GameScenePartitioner.Instance.Add(base.name, this, this.cachedExtents, GameScenePartitioner.Instance.collisionLayer, null);
		}
	}

	// Token: 0x06002071 RID: 8305 RVA: 0x000B5D6C File Offset: 0x000B3F6C
	private void OnMovementStateChanged(bool is_moving)
	{
		if (is_moving)
		{
			this.MarkDirty(false);
			SimAndRenderScheduler.instance.Add(this, false);
			return;
		}
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06002072 RID: 8306 RVA: 0x000B5D90 File Offset: 0x000B3F90
	private static void OnMovementStateChanged(Transform transform, bool is_moving)
	{
		transform.GetComponent<KCollider2D>().OnMovementStateChanged(is_moving);
	}

	// Token: 0x06002073 RID: 8307 RVA: 0x000B5D9E File Offset: 0x000B3F9E
	public void RenderEveryTick(float dt)
	{
		this.MarkDirty(false);
	}

	// Token: 0x06002074 RID: 8308
	public abstract bool Intersects(Vector2 pos);

	// Token: 0x06002075 RID: 8309
	public abstract Extents GetExtents();

	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06002076 RID: 8310
	public abstract Bounds bounds { get; }

	// Token: 0x04001246 RID: 4678
	[SerializeField]
	public Vector2 _offset;

	// Token: 0x04001247 RID: 4679
	private Extents cachedExtents;

	// Token: 0x04001248 RID: 4680
	private HandleVector<int>.Handle partitionerEntry;
}
