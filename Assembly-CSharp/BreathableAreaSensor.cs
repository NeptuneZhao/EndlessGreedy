using System;

// Token: 0x020004B1 RID: 1201
public class BreathableAreaSensor : Sensor
{
	// Token: 0x060019F5 RID: 6645 RVA: 0x0008A3B4 File Offset: 0x000885B4
	public BreathableAreaSensor(Sensors sensors) : base(sensors)
	{
	}

	// Token: 0x060019F6 RID: 6646 RVA: 0x0008A3C0 File Offset: 0x000885C0
	public override void Update()
	{
		if (this.breather == null)
		{
			this.breather = base.GetComponent<OxygenBreather>();
		}
		bool flag = this.isBreathable;
		this.isBreathable = (this.breather.IsBreathableElement || this.breather.HasTag(GameTags.InTransitTube));
		if (this.isBreathable != flag)
		{
			if (this.isBreathable)
			{
				base.Trigger(99949694, null);
				return;
			}
			base.Trigger(-1189351068, null);
		}
	}

	// Token: 0x060019F7 RID: 6647 RVA: 0x0008A43E File Offset: 0x0008863E
	public bool IsBreathable()
	{
		return this.isBreathable;
	}

	// Token: 0x060019F8 RID: 6648 RVA: 0x0008A446 File Offset: 0x00088646
	public bool IsUnderwater()
	{
		return this.breather.IsUnderLiquid;
	}

	// Token: 0x04000EC2 RID: 3778
	private bool isBreathable;

	// Token: 0x04000EC3 RID: 3779
	private OxygenBreather breather;
}
