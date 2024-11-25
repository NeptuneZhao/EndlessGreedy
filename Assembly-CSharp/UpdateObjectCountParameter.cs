using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x020004F5 RID: 1269
internal class UpdateObjectCountParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06001C44 RID: 7236 RVA: 0x0009484C File Offset: 0x00092A4C
	public static UpdateObjectCountParameter.Settings GetSettings(HashedString path_hash, SoundDescription description)
	{
		UpdateObjectCountParameter.Settings settings = default(UpdateObjectCountParameter.Settings);
		if (!UpdateObjectCountParameter.settings.TryGetValue(path_hash, out settings))
		{
			settings = default(UpdateObjectCountParameter.Settings);
			EventDescription eventDescription = RuntimeManager.GetEventDescription(description.path);
			USER_PROPERTY user_PROPERTY;
			if (eventDescription.getUserProperty("minObj", out user_PROPERTY) == RESULT.OK)
			{
				settings.minObjects = (float)((short)user_PROPERTY.floatValue());
			}
			else
			{
				settings.minObjects = 1f;
			}
			USER_PROPERTY user_PROPERTY2;
			if (eventDescription.getUserProperty("maxObj", out user_PROPERTY2) == RESULT.OK)
			{
				settings.maxObjects = user_PROPERTY2.floatValue();
			}
			else
			{
				settings.maxObjects = 0f;
			}
			USER_PROPERTY user_PROPERTY3;
			if (eventDescription.getUserProperty("curveType", out user_PROPERTY3) == RESULT.OK && user_PROPERTY3.stringValue() == "exp")
			{
				settings.useExponentialCurve = true;
			}
			settings.parameterId = description.GetParameterId(UpdateObjectCountParameter.parameterHash);
			settings.path = path_hash;
			UpdateObjectCountParameter.settings[path_hash] = settings;
		}
		return settings;
	}

	// Token: 0x06001C45 RID: 7237 RVA: 0x00094934 File Offset: 0x00092B34
	public static void ApplySettings(EventInstance ev, int count, UpdateObjectCountParameter.Settings settings)
	{
		float num = 0f;
		if (settings.maxObjects != settings.minObjects)
		{
			num = ((float)count - settings.minObjects) / (settings.maxObjects - settings.minObjects);
			num = Mathf.Clamp01(num);
		}
		if (settings.useExponentialCurve)
		{
			num *= num;
		}
		ev.setParameterByID(settings.parameterId, num, false);
	}

	// Token: 0x06001C46 RID: 7238 RVA: 0x00094990 File Offset: 0x00092B90
	public UpdateObjectCountParameter() : base("objectCount")
	{
	}

	// Token: 0x06001C47 RID: 7239 RVA: 0x000949B0 File Offset: 0x00092BB0
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateObjectCountParameter.Settings settings = UpdateObjectCountParameter.GetSettings(sound.path, sound.description);
		UpdateObjectCountParameter.Entry item = new UpdateObjectCountParameter.Entry
		{
			ev = sound.ev,
			settings = settings
		};
		this.entries.Add(item);
	}

	// Token: 0x06001C48 RID: 7240 RVA: 0x000949FC File Offset: 0x00092BFC
	public override void Update(float dt)
	{
		DictionaryPool<HashedString, int, LoopingSoundManager>.PooledDictionary pooledDictionary = DictionaryPool<HashedString, int, LoopingSoundManager>.Allocate();
		foreach (UpdateObjectCountParameter.Entry entry in this.entries)
		{
			int num = 0;
			pooledDictionary.TryGetValue(entry.settings.path, out num);
			num = (pooledDictionary[entry.settings.path] = num + 1);
		}
		foreach (UpdateObjectCountParameter.Entry entry2 in this.entries)
		{
			int count = pooledDictionary[entry2.settings.path];
			UpdateObjectCountParameter.ApplySettings(entry2.ev, count, entry2.settings);
		}
		pooledDictionary.Recycle();
	}

	// Token: 0x06001C49 RID: 7241 RVA: 0x00094AE8 File Offset: 0x00092CE8
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

	// Token: 0x06001C4A RID: 7242 RVA: 0x00094B40 File Offset: 0x00092D40
	public static void Clear()
	{
		UpdateObjectCountParameter.settings.Clear();
	}

	// Token: 0x04000FF2 RID: 4082
	private List<UpdateObjectCountParameter.Entry> entries = new List<UpdateObjectCountParameter.Entry>();

	// Token: 0x04000FF3 RID: 4083
	private static Dictionary<HashedString, UpdateObjectCountParameter.Settings> settings = new Dictionary<HashedString, UpdateObjectCountParameter.Settings>();

	// Token: 0x04000FF4 RID: 4084
	private static readonly HashedString parameterHash = "objectCount";

	// Token: 0x020012C0 RID: 4800
	private struct Entry
	{
		// Token: 0x04006438 RID: 25656
		public EventInstance ev;

		// Token: 0x04006439 RID: 25657
		public UpdateObjectCountParameter.Settings settings;
	}

	// Token: 0x020012C1 RID: 4801
	public struct Settings
	{
		// Token: 0x0400643A RID: 25658
		public HashedString path;

		// Token: 0x0400643B RID: 25659
		public PARAMETER_ID parameterId;

		// Token: 0x0400643C RID: 25660
		public float minObjects;

		// Token: 0x0400643D RID: 25661
		public float maxObjects;

		// Token: 0x0400643E RID: 25662
		public bool useExponentialCurve;
	}
}
