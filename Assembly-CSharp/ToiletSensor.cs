using System;

// Token: 0x020004BE RID: 1214
public class ToiletSensor : Sensor
{
	// Token: 0x06001A2C RID: 6700 RVA: 0x0008ADCF File Offset: 0x00088FCF
	public ToiletSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = base.GetComponent<Navigator>();
	}

	// Token: 0x06001A2D RID: 6701 RVA: 0x0008ADE4 File Offset: 0x00088FE4
	public override void Update()
	{
		IUsable usable = null;
		int num = int.MaxValue;
		bool flag = false;
		foreach (IUsable usable2 in Components.Toilets.Items)
		{
			if (usable2.IsUsable())
			{
				flag = true;
				int navigationCost = this.navigator.GetNavigationCost(Grid.PosToCell(usable2.transform.GetPosition()));
				if (navigationCost != -1 && navigationCost < num)
				{
					usable = usable2;
					num = navigationCost;
				}
			}
		}
		bool flag2 = Components.Toilets.Count > 0;
		if (usable != this.toilet || flag2 != this.areThereAnyToilets || this.areThereAnyUsableToilets != flag)
		{
			this.toilet = usable;
			this.areThereAnyToilets = flag2;
			this.areThereAnyUsableToilets = flag;
			base.Trigger(-752545459, null);
		}
	}

	// Token: 0x06001A2E RID: 6702 RVA: 0x0008AEC4 File Offset: 0x000890C4
	public bool AreThereAnyToilets()
	{
		return this.areThereAnyToilets;
	}

	// Token: 0x06001A2F RID: 6703 RVA: 0x0008AECC File Offset: 0x000890CC
	public bool AreThereAnyUsableToilets()
	{
		return this.areThereAnyUsableToilets;
	}

	// Token: 0x06001A30 RID: 6704 RVA: 0x0008AED4 File Offset: 0x000890D4
	public IUsable GetNearestUsableToilet()
	{
		return this.toilet;
	}

	// Token: 0x04000EE3 RID: 3811
	private Navigator navigator;

	// Token: 0x04000EE4 RID: 3812
	private IUsable toilet;

	// Token: 0x04000EE5 RID: 3813
	private bool areThereAnyToilets;

	// Token: 0x04000EE6 RID: 3814
	private bool areThereAnyUsableToilets;
}
