using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x02000AD1 RID: 2769
internal class UpdateRocketLandingParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06005241 RID: 21057 RVA: 0x001D7EFB File Offset: 0x001D60FB
	public UpdateRocketLandingParameter() : base("rocketLanding")
	{
	}

	// Token: 0x06005242 RID: 21058 RVA: 0x001D7F18 File Offset: 0x001D6118
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateRocketLandingParameter.Entry item = new UpdateRocketLandingParameter.Entry
		{
			rocketModule = sound.transform.GetComponent<RocketModule>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06005243 RID: 21059 RVA: 0x001D7F74 File Offset: 0x001D6174
	public override void Update(float dt)
	{
		foreach (UpdateRocketLandingParameter.Entry entry in this.entries)
		{
			if (!(entry.rocketModule == null))
			{
				LaunchConditionManager conditionManager = entry.rocketModule.conditionManager;
				if (!(conditionManager == null))
				{
					ILaunchableRocket component = conditionManager.GetComponent<ILaunchableRocket>();
					if (component != null)
					{
						if (component.isLanding)
						{
							EventInstance ev = entry.ev;
							ev.setParameterByID(entry.parameterId, 1f, false);
						}
						else
						{
							EventInstance ev = entry.ev;
							ev.setParameterByID(entry.parameterId, 0f, false);
						}
					}
				}
			}
		}
	}

	// Token: 0x06005244 RID: 21060 RVA: 0x001D8030 File Offset: 0x001D6230
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

	// Token: 0x04003645 RID: 13893
	private List<UpdateRocketLandingParameter.Entry> entries = new List<UpdateRocketLandingParameter.Entry>();

	// Token: 0x02001B1B RID: 6939
	private struct Entry
	{
		// Token: 0x04007ECF RID: 32463
		public RocketModule rocketModule;

		// Token: 0x04007ED0 RID: 32464
		public EventInstance ev;

		// Token: 0x04007ED1 RID: 32465
		public PARAMETER_ID parameterId;
	}
}
