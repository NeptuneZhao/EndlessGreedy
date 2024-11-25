using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000DC9 RID: 3529
public class SpeedLoopingSoundUpdater : LoopingSoundParameterUpdater
{
	// Token: 0x06007008 RID: 28680 RVA: 0x002A4121 File Offset: 0x002A2321
	public SpeedLoopingSoundUpdater() : base("Speed")
	{
	}

	// Token: 0x06007009 RID: 28681 RVA: 0x002A4140 File Offset: 0x002A2340
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		SpeedLoopingSoundUpdater.Entry item = new SpeedLoopingSoundUpdater.Entry
		{
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x0600700A RID: 28682 RVA: 0x002A418C File Offset: 0x002A238C
	public override void Update(float dt)
	{
		float speedParameterValue = SpeedLoopingSoundUpdater.GetSpeedParameterValue();
		foreach (SpeedLoopingSoundUpdater.Entry entry in this.entries)
		{
			EventInstance ev = entry.ev;
			ev.setParameterByID(entry.parameterId, speedParameterValue, false);
		}
	}

	// Token: 0x0600700B RID: 28683 RVA: 0x002A41F8 File Offset: 0x002A23F8
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

	// Token: 0x0600700C RID: 28684 RVA: 0x002A4250 File Offset: 0x002A2450
	public static float GetSpeedParameterValue()
	{
		return Time.timeScale * 1f;
	}

	// Token: 0x04004CCA RID: 19658
	private List<SpeedLoopingSoundUpdater.Entry> entries = new List<SpeedLoopingSoundUpdater.Entry>();

	// Token: 0x02001EDA RID: 7898
	private struct Entry
	{
		// Token: 0x04008BB9 RID: 35769
		public EventInstance ev;

		// Token: 0x04008BBA RID: 35770
		public PARAMETER_ID parameterId;
	}
}
