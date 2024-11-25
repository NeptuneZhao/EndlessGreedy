using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x02000AD2 RID: 2770
internal class UpdateRocketSpeedParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06005245 RID: 21061 RVA: 0x001D8088 File Offset: 0x001D6288
	public UpdateRocketSpeedParameter() : base("rocketSpeed")
	{
	}

	// Token: 0x06005246 RID: 21062 RVA: 0x001D80A8 File Offset: 0x001D62A8
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateRocketSpeedParameter.Entry item = new UpdateRocketSpeedParameter.Entry
		{
			rocketModule = sound.transform.GetComponent<RocketModule>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06005247 RID: 21063 RVA: 0x001D8104 File Offset: 0x001D6304
	public override void Update(float dt)
	{
		foreach (UpdateRocketSpeedParameter.Entry entry in this.entries)
		{
			if (!(entry.rocketModule == null))
			{
				LaunchConditionManager conditionManager = entry.rocketModule.conditionManager;
				if (!(conditionManager == null))
				{
					ILaunchableRocket component = conditionManager.GetComponent<ILaunchableRocket>();
					if (component != null)
					{
						EventInstance ev = entry.ev;
						ev.setParameterByID(entry.parameterId, component.rocketSpeed, false);
					}
				}
			}
		}
	}

	// Token: 0x06005248 RID: 21064 RVA: 0x001D819C File Offset: 0x001D639C
	public override void Remove(LoopingSoundParameterUpdater.Sound sound)
	{
		for (int i = 0; i < this.entries.Count; i++)
		{
			if (this.entries[i].ev.handle == sound.ev.handle)
			{
				this.entries.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x04003646 RID: 13894
	private List<UpdateRocketSpeedParameter.Entry> entries = new List<UpdateRocketSpeedParameter.Entry>();

	// Token: 0x02001B1C RID: 6940
	private struct Entry
	{
		// Token: 0x04007ED2 RID: 32466
		public RocketModule rocketModule;

		// Token: 0x04007ED3 RID: 32467
		public EventInstance ev;

		// Token: 0x04007ED4 RID: 32468
		public PARAMETER_ID parameterId;
	}
}
