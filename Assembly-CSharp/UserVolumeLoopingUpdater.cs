using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x02000500 RID: 1280
internal abstract class UserVolumeLoopingUpdater : LoopingSoundParameterUpdater
{
	// Token: 0x06001CA5 RID: 7333 RVA: 0x00096DA0 File Offset: 0x00094FA0
	public UserVolumeLoopingUpdater(string parameter, string player_pref) : base(parameter)
	{
		this.playerPref = player_pref;
	}

	// Token: 0x06001CA6 RID: 7334 RVA: 0x00096DC0 File Offset: 0x00094FC0
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UserVolumeLoopingUpdater.Entry item = new UserVolumeLoopingUpdater.Entry
		{
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06001CA7 RID: 7335 RVA: 0x00096E0C File Offset: 0x0009500C
	public override void Update(float dt)
	{
		if (string.IsNullOrEmpty(this.playerPref))
		{
			return;
		}
		float @float = KPlayerPrefs.GetFloat(this.playerPref);
		foreach (UserVolumeLoopingUpdater.Entry entry in this.entries)
		{
			EventInstance ev = entry.ev;
			ev.setParameterByID(entry.parameterId, @float, false);
		}
	}

	// Token: 0x06001CA8 RID: 7336 RVA: 0x00096E8C File Offset: 0x0009508C
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

	// Token: 0x0400102C RID: 4140
	private List<UserVolumeLoopingUpdater.Entry> entries = new List<UserVolumeLoopingUpdater.Entry>();

	// Token: 0x0400102D RID: 4141
	private string playerPref;

	// Token: 0x020012CF RID: 4815
	private struct Entry
	{
		// Token: 0x04006488 RID: 25736
		public EventInstance ev;

		// Token: 0x04006489 RID: 25737
		public PARAMETER_ID parameterId;
	}
}
