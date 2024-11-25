using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x020007D3 RID: 2003
internal class UpdateDistanceToImpactParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06003748 RID: 14152 RVA: 0x0012D635 File Offset: 0x0012B835
	public UpdateDistanceToImpactParameter() : base("distanceToImpact")
	{
	}

	// Token: 0x06003749 RID: 14153 RVA: 0x0012D654 File Offset: 0x0012B854
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateDistanceToImpactParameter.Entry item = new UpdateDistanceToImpactParameter.Entry
		{
			comet = sound.transform.GetComponent<Comet>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x0600374A RID: 14154 RVA: 0x0012D6B0 File Offset: 0x0012B8B0
	public override void Update(float dt)
	{
		foreach (UpdateDistanceToImpactParameter.Entry entry in this.entries)
		{
			if (!(entry.comet == null))
			{
				float soundDistance = entry.comet.GetSoundDistance();
				EventInstance ev = entry.ev;
				ev.setParameterByID(entry.parameterId, soundDistance, false);
			}
		}
	}

	// Token: 0x0600374B RID: 14155 RVA: 0x0012D730 File Offset: 0x0012B930
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

	// Token: 0x040020DA RID: 8410
	private List<UpdateDistanceToImpactParameter.Entry> entries = new List<UpdateDistanceToImpactParameter.Entry>();

	// Token: 0x02001699 RID: 5785
	private struct Entry
	{
		// Token: 0x04007023 RID: 28707
		public Comet comet;

		// Token: 0x04007024 RID: 28708
		public EventInstance ev;

		// Token: 0x04007025 RID: 28709
		public PARAMETER_ID parameterId;
	}
}
