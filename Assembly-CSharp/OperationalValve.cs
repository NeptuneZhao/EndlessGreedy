using System;
using KSerialization;

// Token: 0x02000742 RID: 1858
[SerializationConfig(MemberSerialization.OptIn)]
public class OperationalValve : ValveBase
{
	// Token: 0x0600317F RID: 12671 RVA: 0x00110987 File Offset: 0x0010EB87
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<OperationalValve>(-592767678, OperationalValve.OnOperationalChangedDelegate);
	}

	// Token: 0x06003180 RID: 12672 RVA: 0x001109A0 File Offset: 0x0010EBA0
	protected override void OnSpawn()
	{
		this.OnOperationalChanged(this.operational.IsOperational);
		base.OnSpawn();
	}

	// Token: 0x06003181 RID: 12673 RVA: 0x001109BE File Offset: 0x0010EBBE
	protected override void OnCleanUp()
	{
		base.Unsubscribe<OperationalValve>(-592767678, OperationalValve.OnOperationalChangedDelegate, false);
		base.OnCleanUp();
	}

	// Token: 0x06003182 RID: 12674 RVA: 0x001109D8 File Offset: 0x0010EBD8
	private void OnOperationalChanged(object data)
	{
		bool flag = (bool)data;
		if (flag)
		{
			base.CurrentFlow = base.MaxFlow;
		}
		else
		{
			base.CurrentFlow = 0f;
		}
		this.operational.SetActive(flag, false);
	}

	// Token: 0x06003183 RID: 12675 RVA: 0x00110A15 File Offset: 0x0010EC15
	protected override void OnMassTransfer(float amount)
	{
		this.isDispensing = (amount > 0f);
	}

	// Token: 0x06003184 RID: 12676 RVA: 0x00110A28 File Offset: 0x0010EC28
	public override void UpdateAnim()
	{
		if (!this.operational.IsOperational)
		{
			this.controller.Queue("off", KAnim.PlayMode.Once, 1f, 0f);
			return;
		}
		if (this.isDispensing)
		{
			this.controller.Queue("on_flow", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		this.controller.Queue("on", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04001D19 RID: 7449
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001D1A RID: 7450
	private bool isDispensing;

	// Token: 0x04001D1B RID: 7451
	private static readonly EventSystem.IntraObjectHandler<OperationalValve> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<OperationalValve>(delegate(OperationalValve component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
