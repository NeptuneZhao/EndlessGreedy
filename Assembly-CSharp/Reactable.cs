using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004A4 RID: 1188
public abstract class Reactable
{
	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x0600199F RID: 6559 RVA: 0x00089172 File Offset: 0x00087372
	public bool IsValid
	{
		get
		{
			return this.partitionerEntry.IsValid();
		}
	}

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x060019A0 RID: 6560 RVA: 0x0008917F File Offset: 0x0008737F
	// (set) Token: 0x060019A1 RID: 6561 RVA: 0x00089187 File Offset: 0x00087387
	public float creationTime { get; private set; }

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x060019A2 RID: 6562 RVA: 0x00089190 File Offset: 0x00087390
	public bool IsReacting
	{
		get
		{
			return this.reactor != null;
		}
	}

	// Token: 0x060019A3 RID: 6563 RVA: 0x000891A0 File Offset: 0x000873A0
	public Reactable(GameObject gameObject, HashedString id, ChoreType chore_type, int range_width = 15, int range_height = 8, bool follow_transform = false, float globalCooldown = 0f, float localCooldown = 0f, float lifeSpan = float.PositiveInfinity, float max_initial_delay = 0f, ObjectLayer overrideLayer = ObjectLayer.NumLayers)
	{
		this.rangeHeight = range_height;
		this.rangeWidth = range_width;
		this.id = id;
		this.gameObject = gameObject;
		this.choreType = chore_type;
		this.globalCooldown = globalCooldown;
		this.localCooldown = localCooldown;
		this.lifeSpan = lifeSpan;
		this.initialDelay = ((max_initial_delay > 0f) ? UnityEngine.Random.Range(0f, max_initial_delay) : 0f);
		this.creationTime = GameClock.Instance.GetTime();
		ObjectLayer objectLayer = (overrideLayer == ObjectLayer.NumLayers) ? this.reactionLayer : overrideLayer;
		ReactionMonitor.Def def = gameObject.GetDef<ReactionMonitor.Def>();
		if (overrideLayer != objectLayer && def != null)
		{
			objectLayer = def.ReactionLayer;
		}
		this.reactionLayer = objectLayer;
		this.Initialize(follow_transform);
	}

	// Token: 0x060019A4 RID: 6564 RVA: 0x0008927C File Offset: 0x0008747C
	public void Initialize(bool followTransform)
	{
		this.UpdateLocation();
		if (followTransform)
		{
			this.transformId = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.gameObject.transform, new System.Action(this.UpdateLocation), "Reactable follow transform");
		}
	}

	// Token: 0x060019A5 RID: 6565 RVA: 0x000892B3 File Offset: 0x000874B3
	public void Begin(GameObject reactor)
	{
		this.reactor = reactor;
		this.lastTriggerTime = GameClock.Instance.GetTime();
		this.InternalBegin();
	}

	// Token: 0x060019A6 RID: 6566 RVA: 0x000892D4 File Offset: 0x000874D4
	public void End()
	{
		this.InternalEnd();
		if (this.reactor != null)
		{
			GameObject gameObject = this.reactor;
			this.InternalEnd();
			this.reactor = null;
			if (gameObject != null)
			{
				ReactionMonitor.Instance smi = gameObject.GetSMI<ReactionMonitor.Instance>();
				if (smi != null)
				{
					smi.StopReaction();
				}
			}
		}
	}

	// Token: 0x060019A7 RID: 6567 RVA: 0x00089324 File Offset: 0x00087524
	public bool CanBegin(GameObject reactor, Navigator.ActiveTransition transition)
	{
		float time = GameClock.Instance.GetTime();
		float num = time - this.creationTime;
		float num2 = time - this.lastTriggerTime;
		if (num < this.initialDelay || num2 < this.globalCooldown)
		{
			return false;
		}
		ChoreConsumer component = reactor.GetComponent<ChoreConsumer>();
		Chore chore = (component != null) ? component.choreDriver.GetCurrentChore() : null;
		if (chore == null || this.choreType.priority <= chore.choreType.priority)
		{
			return false;
		}
		int num3 = 0;
		while (this.additionalPreconditions != null && num3 < this.additionalPreconditions.Count)
		{
			if (!this.additionalPreconditions[num3](reactor, transition))
			{
				return false;
			}
			num3++;
		}
		return this.InternalCanBegin(reactor, transition);
	}

	// Token: 0x060019A8 RID: 6568 RVA: 0x000893DE File Offset: 0x000875DE
	public bool IsExpired()
	{
		return GameClock.Instance.GetTime() - this.creationTime > this.lifeSpan;
	}

	// Token: 0x060019A9 RID: 6569
	public abstract bool InternalCanBegin(GameObject reactor, Navigator.ActiveTransition transition);

	// Token: 0x060019AA RID: 6570
	public abstract void Update(float dt);

	// Token: 0x060019AB RID: 6571
	protected abstract void InternalBegin();

	// Token: 0x060019AC RID: 6572
	protected abstract void InternalEnd();

	// Token: 0x060019AD RID: 6573
	protected abstract void InternalCleanup();

	// Token: 0x060019AE RID: 6574 RVA: 0x000893FC File Offset: 0x000875FC
	public void Cleanup()
	{
		this.End();
		this.InternalCleanup();
		if (this.transformId != -1)
		{
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transformId, new System.Action(this.UpdateLocation));
			this.transformId = -1;
		}
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x060019AF RID: 6575 RVA: 0x00089454 File Offset: 0x00087654
	private void UpdateLocation()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		if (this.gameObject != null)
		{
			this.sourceCell = Grid.PosToCell(this.gameObject);
			Extents extents = new Extents(Grid.PosToXY(this.gameObject.transform.GetPosition()).x - this.rangeWidth / 2, Grid.PosToXY(this.gameObject.transform.GetPosition()).y - this.rangeHeight / 2, this.rangeWidth, this.rangeHeight);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("Reactable", this, extents, GameScenePartitioner.Instance.objectLayers[(int)this.reactionLayer], null);
		}
	}

	// Token: 0x060019B0 RID: 6576 RVA: 0x00089515 File Offset: 0x00087715
	public Reactable AddPrecondition(Reactable.ReactablePrecondition precondition)
	{
		if (this.additionalPreconditions == null)
		{
			this.additionalPreconditions = new List<Reactable.ReactablePrecondition>();
		}
		this.additionalPreconditions.Add(precondition);
		return this;
	}

	// Token: 0x060019B1 RID: 6577 RVA: 0x00089537 File Offset: 0x00087737
	public void InsertPrecondition(int index, Reactable.ReactablePrecondition precondition)
	{
		if (this.additionalPreconditions == null)
		{
			this.additionalPreconditions = new List<Reactable.ReactablePrecondition>();
		}
		index = Math.Min(index, this.additionalPreconditions.Count);
		this.additionalPreconditions.Insert(index, precondition);
	}

	// Token: 0x04000E96 RID: 3734
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04000E97 RID: 3735
	protected GameObject gameObject;

	// Token: 0x04000E98 RID: 3736
	public HashedString id;

	// Token: 0x04000E99 RID: 3737
	public bool preventChoreInterruption = true;

	// Token: 0x04000E9A RID: 3738
	public int sourceCell;

	// Token: 0x04000E9B RID: 3739
	private int rangeWidth;

	// Token: 0x04000E9C RID: 3740
	private int rangeHeight;

	// Token: 0x04000E9D RID: 3741
	private int transformId = -1;

	// Token: 0x04000E9E RID: 3742
	public float globalCooldown;

	// Token: 0x04000E9F RID: 3743
	public float localCooldown;

	// Token: 0x04000EA0 RID: 3744
	public float lifeSpan = float.PositiveInfinity;

	// Token: 0x04000EA1 RID: 3745
	private float lastTriggerTime = -2.1474836E+09f;

	// Token: 0x04000EA2 RID: 3746
	private float initialDelay;

	// Token: 0x04000EA4 RID: 3748
	protected GameObject reactor;

	// Token: 0x04000EA5 RID: 3749
	private ChoreType choreType;

	// Token: 0x04000EA6 RID: 3750
	protected LoggerFSS log;

	// Token: 0x04000EA7 RID: 3751
	private List<Reactable.ReactablePrecondition> additionalPreconditions;

	// Token: 0x04000EA8 RID: 3752
	private ObjectLayer reactionLayer;

	// Token: 0x02001270 RID: 4720
	// (Invoke) Token: 0x0600832C RID: 33580
	public delegate bool ReactablePrecondition(GameObject go, Navigator.ActiveTransition transition);
}
