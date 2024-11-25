using System;
using System.Diagnostics;

// Token: 0x0200074F RID: 1871
[DebuggerDisplay("{name}")]
public class PowerTransformer : Generator
{
	// Token: 0x060031DD RID: 12765 RVA: 0x0011266C File Offset: 0x0011086C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.battery = base.GetComponent<Battery>();
		base.Subscribe<PowerTransformer>(-592767678, PowerTransformer.OnOperationalChangedDelegate);
		this.UpdateJoulesLostPerSecond();
	}

	// Token: 0x060031DE RID: 12766 RVA: 0x00112697 File Offset: 0x00110897
	public override void ApplyDeltaJoules(float joules_delta, bool can_over_power = false)
	{
		this.battery.ConsumeEnergy(-joules_delta);
		base.ApplyDeltaJoules(joules_delta, can_over_power);
	}

	// Token: 0x060031DF RID: 12767 RVA: 0x001126AE File Offset: 0x001108AE
	public override void ConsumeEnergy(float joules)
	{
		this.battery.ConsumeEnergy(joules);
		base.ConsumeEnergy(joules);
	}

	// Token: 0x060031E0 RID: 12768 RVA: 0x001126C3 File Offset: 0x001108C3
	private void OnOperationalChanged(object data)
	{
		this.UpdateJoulesLostPerSecond();
	}

	// Token: 0x060031E1 RID: 12769 RVA: 0x001126CB File Offset: 0x001108CB
	private void UpdateJoulesLostPerSecond()
	{
		if (this.operational.IsOperational)
		{
			this.battery.joulesLostPerSecond = 0f;
			return;
		}
		this.battery.joulesLostPerSecond = 3.3333333f;
	}

	// Token: 0x060031E2 RID: 12770 RVA: 0x001126FC File Offset: 0x001108FC
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		float joulesAvailable = this.operational.IsOperational ? Math.Min(this.battery.JoulesAvailable, base.WattageRating * dt) : 0f;
		base.AssignJoulesAvailable(joulesAvailable);
		ushort circuitID = this.battery.CircuitID;
		ushort circuitID2 = base.CircuitID;
		bool flag = circuitID == circuitID2 && circuitID != ushort.MaxValue;
		if (this.mLoopDetected != flag)
		{
			this.mLoopDetected = flag;
			this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.PowerLoopDetected, this.mLoopDetected, this);
		}
	}

	// Token: 0x04001D60 RID: 7520
	private Battery battery;

	// Token: 0x04001D61 RID: 7521
	private bool mLoopDetected;

	// Token: 0x04001D62 RID: 7522
	private static readonly EventSystem.IntraObjectHandler<PowerTransformer> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<PowerTransformer>(delegate(PowerTransformer component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
