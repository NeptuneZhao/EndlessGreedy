using System;

// Token: 0x020004BA RID: 1210
public class PickupableSensor : Sensor
{
	// Token: 0x06001A0E RID: 6670 RVA: 0x0008A96B File Offset: 0x00088B6B
	public PickupableSensor(Sensors sensors) : base(sensors)
	{
		this.worker = base.GetComponent<WorkerBase>();
		this.pathProber = base.GetComponent<PathProber>();
	}

	// Token: 0x06001A0F RID: 6671 RVA: 0x0008A98C File Offset: 0x00088B8C
	public override void Update()
	{
		GlobalChoreProvider.Instance.UpdateFetches(this.pathProber);
		Game.Instance.fetchManager.UpdatePickups(this.pathProber, this.worker);
	}

	// Token: 0x04000ED8 RID: 3800
	private PathProber pathProber;

	// Token: 0x04000ED9 RID: 3801
	private WorkerBase worker;
}
