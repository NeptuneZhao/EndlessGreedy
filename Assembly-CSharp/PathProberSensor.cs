using System;

// Token: 0x020004B9 RID: 1209
public class PathProberSensor : Sensor
{
	// Token: 0x06001A0C RID: 6668 RVA: 0x0008A948 File Offset: 0x00088B48
	public PathProberSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = sensors.GetComponent<Navigator>();
	}

	// Token: 0x06001A0D RID: 6669 RVA: 0x0008A95D File Offset: 0x00088B5D
	public override void Update()
	{
		this.navigator.UpdateProbe(false);
	}

	// Token: 0x04000ED7 RID: 3799
	private Navigator navigator;
}
