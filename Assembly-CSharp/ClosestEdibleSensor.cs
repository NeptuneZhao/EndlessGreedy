using System;
using System.Collections.Generic;

// Token: 0x020004B2 RID: 1202
public class ClosestEdibleSensor : Sensor
{
	// Token: 0x060019F9 RID: 6649 RVA: 0x0008A453 File Offset: 0x00088653
	public ClosestEdibleSensor(Sensors sensors) : base(sensors)
	{
	}

	// Token: 0x060019FA RID: 6650 RVA: 0x0008A45C File Offset: 0x0008865C
	public override void Update()
	{
		HashSet<Tag> forbiddenTagSet = base.GetComponent<ConsumableConsumer>().forbiddenTagSet;
		Pickupable pickupable = Game.Instance.fetchManager.FindEdibleFetchTarget(base.GetComponent<Storage>(), forbiddenTagSet, ClosestEdibleSensor.requiredSearchTags);
		bool flag = this.edibleInReachButNotPermitted;
		Edible x = null;
		bool flag2 = false;
		if (pickupable != null)
		{
			x = pickupable.GetComponent<Edible>();
			flag2 = true;
			flag = false;
		}
		else
		{
			flag = (Game.Instance.fetchManager.FindEdibleFetchTarget(base.GetComponent<Storage>(), new HashSet<Tag>(), ClosestEdibleSensor.requiredSearchTags) != null);
		}
		if (x != this.edible || this.hasEdible != flag2)
		{
			this.edible = x;
			this.hasEdible = flag2;
			this.edibleInReachButNotPermitted = flag;
			base.Trigger(86328522, this.edible);
		}
	}

	// Token: 0x060019FB RID: 6651 RVA: 0x0008A519 File Offset: 0x00088719
	public Edible GetEdible()
	{
		return this.edible;
	}

	// Token: 0x04000EC4 RID: 3780
	private Edible edible;

	// Token: 0x04000EC5 RID: 3781
	private bool hasEdible;

	// Token: 0x04000EC6 RID: 3782
	public bool edibleInReachButNotPermitted;

	// Token: 0x04000EC7 RID: 3783
	public static Tag[] requiredSearchTags = new Tag[]
	{
		GameTags.Edible
	};
}
