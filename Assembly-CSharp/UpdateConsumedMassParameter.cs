using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x02000548 RID: 1352
internal class UpdateConsumedMassParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06001F17 RID: 7959 RVA: 0x000AE2E7 File Offset: 0x000AC4E7
	public UpdateConsumedMassParameter() : base("consumedMass")
	{
	}

	// Token: 0x06001F18 RID: 7960 RVA: 0x000AE304 File Offset: 0x000AC504
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateConsumedMassParameter.Entry item = new UpdateConsumedMassParameter.Entry
		{
			creatureCalorieMonitor = sound.transform.GetSMI<CreatureCalorieMonitor.Instance>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06001F19 RID: 7961 RVA: 0x000AE360 File Offset: 0x000AC560
	public override void Update(float dt)
	{
		foreach (UpdateConsumedMassParameter.Entry entry in this.entries)
		{
			if (!entry.creatureCalorieMonitor.IsNullOrStopped())
			{
				float fullness = entry.creatureCalorieMonitor.stomach.GetFullness();
				EventInstance ev = entry.ev;
				ev.setParameterByID(entry.parameterId, fullness, false);
			}
		}
	}

	// Token: 0x06001F1A RID: 7962 RVA: 0x000AE3E4 File Offset: 0x000AC5E4
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

	// Token: 0x04001185 RID: 4485
	private List<UpdateConsumedMassParameter.Entry> entries = new List<UpdateConsumedMassParameter.Entry>();

	// Token: 0x02001321 RID: 4897
	private struct Entry
	{
		// Token: 0x040065B0 RID: 26032
		public CreatureCalorieMonitor.Instance creatureCalorieMonitor;

		// Token: 0x040065B1 RID: 26033
		public EventInstance ev;

		// Token: 0x040065B2 RID: 26034
		public PARAMETER_ID parameterId;
	}
}
