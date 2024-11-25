using System;
using UnityEngine;

// Token: 0x020004A7 RID: 1191
public class WorkableReactable : Reactable
{
	// Token: 0x060019BF RID: 6591 RVA: 0x00089874 File Offset: 0x00087A74
	public WorkableReactable(Workable workable, HashedString id, ChoreType chore_type, WorkableReactable.AllowedDirection allowed_direction = WorkableReactable.AllowedDirection.Any) : base(workable.gameObject, id, chore_type, 1, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
	{
		this.workable = workable;
		this.allowedDirection = allowed_direction;
	}

	// Token: 0x060019C0 RID: 6592 RVA: 0x000898B8 File Offset: 0x00087AB8
	public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
	{
		if (this.workable == null)
		{
			return false;
		}
		if (this.reactor != null)
		{
			return false;
		}
		Brain component = new_reactor.GetComponent<Brain>();
		if (component == null)
		{
			return false;
		}
		if (!component.IsRunning())
		{
			return false;
		}
		Navigator component2 = new_reactor.GetComponent<Navigator>();
		if (component2 == null)
		{
			return false;
		}
		if (!component2.IsMoving())
		{
			return false;
		}
		if (this.allowedDirection == WorkableReactable.AllowedDirection.Any)
		{
			return true;
		}
		Facing component3 = new_reactor.GetComponent<Facing>();
		if (component3 == null)
		{
			return false;
		}
		bool facing = component3.GetFacing();
		return (!facing || this.allowedDirection != WorkableReactable.AllowedDirection.Right) && (facing || this.allowedDirection != WorkableReactable.AllowedDirection.Left);
	}

	// Token: 0x060019C1 RID: 6593 RVA: 0x0008995D File Offset: 0x00087B5D
	protected override void InternalBegin()
	{
		this.worker = this.reactor.GetComponent<WorkerBase>();
		this.worker.StartWork(new WorkerBase.StartWorkInfo(this.workable));
	}

	// Token: 0x060019C2 RID: 6594 RVA: 0x00089986 File Offset: 0x00087B86
	public override void Update(float dt)
	{
		if (this.worker.GetWorkable() == null)
		{
			base.End();
			return;
		}
		if (this.worker.Work(dt) != WorkerBase.WorkResult.InProgress)
		{
			base.End();
		}
	}

	// Token: 0x060019C3 RID: 6595 RVA: 0x000899B7 File Offset: 0x00087BB7
	protected override void InternalEnd()
	{
		if (this.worker != null)
		{
			this.worker.StopWork();
		}
	}

	// Token: 0x060019C4 RID: 6596 RVA: 0x000899D2 File Offset: 0x00087BD2
	protected override void InternalCleanup()
	{
	}

	// Token: 0x04000EAE RID: 3758
	protected Workable workable;

	// Token: 0x04000EAF RID: 3759
	private WorkerBase worker;

	// Token: 0x04000EB0 RID: 3760
	public WorkableReactable.AllowedDirection allowedDirection;

	// Token: 0x02001274 RID: 4724
	public enum AllowedDirection
	{
		// Token: 0x04006381 RID: 25473
		Any,
		// Token: 0x04006382 RID: 25474
		Left,
		// Token: 0x04006383 RID: 25475
		Right
	}
}
