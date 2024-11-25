using System;
using System.Collections.Generic;

// Token: 0x020004F3 RID: 1267
public class SoundEventVolumeCache : Singleton<SoundEventVolumeCache>
{
	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x06001C1B RID: 7195 RVA: 0x000940E1 File Offset: 0x000922E1
	public static SoundEventVolumeCache instance
	{
		get
		{
			return Singleton<SoundEventVolumeCache>.Instance;
		}
	}

	// Token: 0x06001C1C RID: 7196 RVA: 0x000940E8 File Offset: 0x000922E8
	public void AddVolume(string animFile, string eventName, EffectorValues vals)
	{
		HashedString key = new HashedString(animFile + ":" + eventName);
		if (!this.volumeCache.ContainsKey(key))
		{
			this.volumeCache.Add(key, vals);
			return;
		}
		this.volumeCache[key] = vals;
	}

	// Token: 0x06001C1D RID: 7197 RVA: 0x00094134 File Offset: 0x00092334
	public EffectorValues GetVolume(string animFile, string eventName)
	{
		HashedString key = new HashedString(animFile + ":" + eventName);
		if (!this.volumeCache.ContainsKey(key))
		{
			return default(EffectorValues);
		}
		return this.volumeCache[key];
	}

	// Token: 0x04000FE7 RID: 4071
	public Dictionary<HashedString, EffectorValues> volumeCache = new Dictionary<HashedString, EffectorValues>();
}
