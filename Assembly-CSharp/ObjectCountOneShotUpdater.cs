using System;
using System.Collections.Generic;

// Token: 0x020004F6 RID: 1270
internal class ObjectCountOneShotUpdater : OneShotSoundParameterUpdater
{
	// Token: 0x06001C4C RID: 7244 RVA: 0x00094B67 File Offset: 0x00092D67
	public ObjectCountOneShotUpdater() : base("objectCount")
	{
	}

	// Token: 0x06001C4D RID: 7245 RVA: 0x00094B84 File Offset: 0x00092D84
	public override void Update(float dt)
	{
		this.soundCounts.Clear();
	}

	// Token: 0x06001C4E RID: 7246 RVA: 0x00094B94 File Offset: 0x00092D94
	public override void Play(OneShotSoundParameterUpdater.Sound sound)
	{
		UpdateObjectCountParameter.Settings settings = UpdateObjectCountParameter.GetSettings(sound.path, sound.description);
		int num = 0;
		this.soundCounts.TryGetValue(sound.path, out num);
		num = (this.soundCounts[sound.path] = num + 1);
		UpdateObjectCountParameter.ApplySettings(sound.ev, num, settings);
	}

	// Token: 0x04000FF5 RID: 4085
	private Dictionary<HashedString, int> soundCounts = new Dictionary<HashedString, int>();
}
