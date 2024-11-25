using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x020005F8 RID: 1528
internal class UpdatePercentCompleteParameter : LoopingSoundParameterUpdater
{
	// Token: 0x0600256C RID: 9580 RVA: 0x000D147E File Offset: 0x000CF67E
	public UpdatePercentCompleteParameter() : base("percentComplete")
	{
	}

	// Token: 0x0600256D RID: 9581 RVA: 0x000D149C File Offset: 0x000CF69C
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdatePercentCompleteParameter.Entry item = new UpdatePercentCompleteParameter.Entry
		{
			worker = sound.transform.GetComponent<WorkerBase>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x0600256E RID: 9582 RVA: 0x000D14F8 File Offset: 0x000CF6F8
	public override void Update(float dt)
	{
		foreach (UpdatePercentCompleteParameter.Entry entry in this.entries)
		{
			if (!(entry.worker == null))
			{
				Workable workable = entry.worker.GetWorkable();
				if (!(workable == null))
				{
					float percentComplete = workable.GetPercentComplete();
					EventInstance ev = entry.ev;
					ev.setParameterByID(entry.parameterId, percentComplete, false);
				}
			}
		}
	}

	// Token: 0x0600256F RID: 9583 RVA: 0x000D1588 File Offset: 0x000CF788
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

	// Token: 0x04001538 RID: 5432
	private List<UpdatePercentCompleteParameter.Entry> entries = new List<UpdatePercentCompleteParameter.Entry>();

	// Token: 0x020013E9 RID: 5097
	private struct Entry
	{
		// Token: 0x0400685E RID: 26718
		public WorkerBase worker;

		// Token: 0x0400685F RID: 26719
		public EventInstance ev;

		// Token: 0x04006860 RID: 26720
		public PARAMETER_ID parameterId;
	}
}
