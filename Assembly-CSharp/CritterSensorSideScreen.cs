using System;
using UnityEngine;

// Token: 0x02000D62 RID: 3426
public class CritterSensorSideScreen : SideScreenContent
{
	// Token: 0x06006BEC RID: 27628 RVA: 0x00289CD6 File Offset: 0x00287ED6
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.countCrittersToggle.onClick += this.ToggleCritters;
		this.countEggsToggle.onClick += this.ToggleEggs;
	}

	// Token: 0x06006BED RID: 27629 RVA: 0x00289D0C File Offset: 0x00287F0C
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicCritterCountSensor>() != null;
	}

	// Token: 0x06006BEE RID: 27630 RVA: 0x00289D1C File Offset: 0x00287F1C
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetSensor = target.GetComponent<LogicCritterCountSensor>();
		this.crittersCheckmark.enabled = this.targetSensor.countCritters;
		this.eggsCheckmark.enabled = this.targetSensor.countEggs;
	}

	// Token: 0x06006BEF RID: 27631 RVA: 0x00289D68 File Offset: 0x00287F68
	private void ToggleCritters()
	{
		this.targetSensor.countCritters = !this.targetSensor.countCritters;
		this.crittersCheckmark.enabled = this.targetSensor.countCritters;
	}

	// Token: 0x06006BF0 RID: 27632 RVA: 0x00289D99 File Offset: 0x00287F99
	private void ToggleEggs()
	{
		this.targetSensor.countEggs = !this.targetSensor.countEggs;
		this.eggsCheckmark.enabled = this.targetSensor.countEggs;
	}

	// Token: 0x04004999 RID: 18841
	public LogicCritterCountSensor targetSensor;

	// Token: 0x0400499A RID: 18842
	public KToggle countCrittersToggle;

	// Token: 0x0400499B RID: 18843
	public KToggle countEggsToggle;

	// Token: 0x0400499C RID: 18844
	public KImage crittersCheckmark;

	// Token: 0x0400499D RID: 18845
	public KImage eggsCheckmark;
}
