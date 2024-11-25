using System;
using KSerialization;

// Token: 0x02000741 RID: 1857
[SerializationConfig(MemberSerialization.OptIn)]
public class OperationalControlledSwitch : CircuitSwitch
{
	// Token: 0x0600317A RID: 12666 RVA: 0x0011091D File Offset: 0x0010EB1D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.manuallyControlled = false;
	}

	// Token: 0x0600317B RID: 12667 RVA: 0x0011092C File Offset: 0x0010EB2C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<OperationalControlledSwitch>(-592767678, OperationalControlledSwitch.OnOperationalChangedDelegate);
	}

	// Token: 0x0600317C RID: 12668 RVA: 0x00110948 File Offset: 0x0010EB48
	private void OnOperationalChanged(object data)
	{
		bool state = (bool)data;
		this.SetState(state);
	}

	// Token: 0x04001D18 RID: 7448
	private static readonly EventSystem.IntraObjectHandler<OperationalControlledSwitch> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<OperationalControlledSwitch>(delegate(OperationalControlledSwitch component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
