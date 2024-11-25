using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020004D6 RID: 1238
public class AnimEventManager : Singleton<AnimEventManager>
{
	// Token: 0x06001ABC RID: 6844 RVA: 0x0008C70A File Offset: 0x0008A90A
	public void FreeResources()
	{
	}

	// Token: 0x06001ABD RID: 6845 RVA: 0x0008C70C File Offset: 0x0008A90C
	public HandleVector<int>.Handle PlayAnim(KAnimControllerBase controller, KAnim.Anim anim, KAnim.PlayMode mode, float time, bool use_unscaled_time)
	{
		AnimEventManager.AnimData animData = default(AnimEventManager.AnimData);
		animData.frameRate = anim.frameRate;
		animData.totalTime = anim.totalTime;
		animData.numFrames = anim.numFrames;
		animData.useUnscaledTime = use_unscaled_time;
		AnimEventManager.EventPlayerData eventPlayerData = default(AnimEventManager.EventPlayerData);
		eventPlayerData.elapsedTime = time;
		eventPlayerData.mode = mode;
		eventPlayerData.controller = (controller as KBatchedAnimController);
		eventPlayerData.currentFrame = eventPlayerData.controller.GetFrameIdx(eventPlayerData.elapsedTime, false);
		eventPlayerData.previousFrame = -1;
		eventPlayerData.events = null;
		eventPlayerData.updatingEvents = null;
		eventPlayerData.events = GameAudioSheets.Get().GetEvents(anim.id);
		if (eventPlayerData.events == null)
		{
			eventPlayerData.events = AnimEventManager.emptyEventList;
		}
		HandleVector<int>.Handle result;
		if (animData.useUnscaledTime)
		{
			HandleVector<int>.Handle anim_data_handle = this.uiAnimData.Allocate(animData);
			HandleVector<int>.Handle event_data_handle = this.uiEventData.Allocate(eventPlayerData);
			result = this.indirectionData.Allocate(new AnimEventManager.IndirectionData(anim_data_handle, event_data_handle, true));
		}
		else
		{
			HandleVector<int>.Handle anim_data_handle2 = this.animData.Allocate(animData);
			HandleVector<int>.Handle event_data_handle2 = this.eventData.Allocate(eventPlayerData);
			result = this.indirectionData.Allocate(new AnimEventManager.IndirectionData(anim_data_handle2, event_data_handle2, false));
		}
		return result;
	}

	// Token: 0x06001ABE RID: 6846 RVA: 0x0008C840 File Offset: 0x0008AA40
	public void SetMode(HandleVector<int>.Handle handle, KAnim.PlayMode mode)
	{
		if (!handle.IsValid())
		{
			return;
		}
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		KCompactedVector<AnimEventManager.EventPlayerData> kcompactedVector = data.isUIData ? this.uiEventData : this.eventData;
		AnimEventManager.EventPlayerData data2 = kcompactedVector.GetData(data.eventDataHandle);
		data2.mode = mode;
		kcompactedVector.SetData(data.eventDataHandle, data2);
	}

	// Token: 0x06001ABF RID: 6847 RVA: 0x0008C89C File Offset: 0x0008AA9C
	public void StopAnim(HandleVector<int>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return;
		}
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		KCompactedVector<AnimEventManager.AnimData> kcompactedVector = data.isUIData ? this.uiAnimData : this.animData;
		KCompactedVector<AnimEventManager.EventPlayerData> kcompactedVector2 = data.isUIData ? this.uiEventData : this.eventData;
		AnimEventManager.EventPlayerData data2 = kcompactedVector2.GetData(data.eventDataHandle);
		this.StopEvents(data2);
		kcompactedVector.Free(data.animDataHandle);
		kcompactedVector2.Free(data.eventDataHandle);
		this.indirectionData.Free(handle);
	}

	// Token: 0x06001AC0 RID: 6848 RVA: 0x0008C928 File Offset: 0x0008AB28
	public float GetElapsedTime(HandleVector<int>.Handle handle)
	{
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		return (data.isUIData ? this.uiEventData : this.eventData).GetData(data.eventDataHandle).elapsedTime;
	}

	// Token: 0x06001AC1 RID: 6849 RVA: 0x0008C968 File Offset: 0x0008AB68
	public void SetElapsedTime(HandleVector<int>.Handle handle, float elapsed_time)
	{
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		KCompactedVector<AnimEventManager.EventPlayerData> kcompactedVector = data.isUIData ? this.uiEventData : this.eventData;
		AnimEventManager.EventPlayerData data2 = kcompactedVector.GetData(data.eventDataHandle);
		data2.elapsedTime = elapsed_time;
		kcompactedVector.SetData(data.eventDataHandle, data2);
	}

	// Token: 0x06001AC2 RID: 6850 RVA: 0x0008C9BC File Offset: 0x0008ABBC
	public void Update()
	{
		float deltaTime = Time.deltaTime;
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		this.Update(deltaTime, this.animData.GetDataList(), this.eventData.GetDataList());
		this.Update(unscaledDeltaTime, this.uiAnimData.GetDataList(), this.uiEventData.GetDataList());
		for (int i = 0; i < this.finishedCalls.Count; i++)
		{
			this.finishedCalls[i].TriggerStop();
		}
		this.finishedCalls.Clear();
	}

	// Token: 0x06001AC3 RID: 6851 RVA: 0x0008CA44 File Offset: 0x0008AC44
	private void Update(float dt, List<AnimEventManager.AnimData> anim_data, List<AnimEventManager.EventPlayerData> event_data)
	{
		if (dt <= 0f)
		{
			return;
		}
		for (int i = 0; i < event_data.Count; i++)
		{
			AnimEventManager.EventPlayerData eventPlayerData = event_data[i];
			if (!(eventPlayerData.controller == null) && eventPlayerData.mode != KAnim.PlayMode.Paused)
			{
				eventPlayerData.currentFrame = eventPlayerData.controller.GetFrameIdx(eventPlayerData.elapsedTime, false);
				event_data[i] = eventPlayerData;
				this.PlayEvents(eventPlayerData);
				eventPlayerData.previousFrame = eventPlayerData.currentFrame;
				eventPlayerData.elapsedTime += dt * eventPlayerData.controller.GetPlaySpeed();
				event_data[i] = eventPlayerData;
				if (eventPlayerData.updatingEvents != null)
				{
					for (int j = 0; j < eventPlayerData.updatingEvents.Count; j++)
					{
						eventPlayerData.updatingEvents[j].OnUpdate(eventPlayerData);
					}
				}
				event_data[i] = eventPlayerData;
				if (eventPlayerData.mode != KAnim.PlayMode.Loop && eventPlayerData.currentFrame >= anim_data[i].numFrames - 1)
				{
					this.StopEvents(eventPlayerData);
					this.finishedCalls.Add(eventPlayerData.controller);
				}
			}
		}
	}

	// Token: 0x06001AC4 RID: 6852 RVA: 0x0008CB5C File Offset: 0x0008AD5C
	private void PlayEvents(AnimEventManager.EventPlayerData data)
	{
		for (int i = 0; i < data.events.Count; i++)
		{
			data.events[i].Play(data);
		}
	}

	// Token: 0x06001AC5 RID: 6853 RVA: 0x0008CB94 File Offset: 0x0008AD94
	private void StopEvents(AnimEventManager.EventPlayerData data)
	{
		for (int i = 0; i < data.events.Count; i++)
		{
			data.events[i].Stop(data);
		}
		if (data.updatingEvents != null)
		{
			data.updatingEvents.Clear();
		}
	}

	// Token: 0x06001AC6 RID: 6854 RVA: 0x0008CBDC File Offset: 0x0008ADDC
	public AnimEventManager.DevTools_DebugInfo DevTools_GetDebugInfo()
	{
		return new AnimEventManager.DevTools_DebugInfo(this, this.animData, this.eventData, this.uiAnimData, this.uiEventData);
	}

	// Token: 0x04000F25 RID: 3877
	private static readonly List<AnimEvent> emptyEventList = new List<AnimEvent>();

	// Token: 0x04000F26 RID: 3878
	private const int INITIAL_VECTOR_SIZE = 256;

	// Token: 0x04000F27 RID: 3879
	private KCompactedVector<AnimEventManager.AnimData> animData = new KCompactedVector<AnimEventManager.AnimData>(256);

	// Token: 0x04000F28 RID: 3880
	private KCompactedVector<AnimEventManager.EventPlayerData> eventData = new KCompactedVector<AnimEventManager.EventPlayerData>(256);

	// Token: 0x04000F29 RID: 3881
	private KCompactedVector<AnimEventManager.AnimData> uiAnimData = new KCompactedVector<AnimEventManager.AnimData>(256);

	// Token: 0x04000F2A RID: 3882
	private KCompactedVector<AnimEventManager.EventPlayerData> uiEventData = new KCompactedVector<AnimEventManager.EventPlayerData>(256);

	// Token: 0x04000F2B RID: 3883
	private KCompactedVector<AnimEventManager.IndirectionData> indirectionData = new KCompactedVector<AnimEventManager.IndirectionData>(0);

	// Token: 0x04000F2C RID: 3884
	private List<KBatchedAnimController> finishedCalls = new List<KBatchedAnimController>();

	// Token: 0x020012B0 RID: 4784
	public struct AnimData
	{
		// Token: 0x04006405 RID: 25605
		public float frameRate;

		// Token: 0x04006406 RID: 25606
		public float totalTime;

		// Token: 0x04006407 RID: 25607
		public int numFrames;

		// Token: 0x04006408 RID: 25608
		public bool useUnscaledTime;
	}

	// Token: 0x020012B1 RID: 4785
	[DebuggerDisplay("{controller.name}, Anim={currentAnim}, Frame={currentFrame}, Mode={mode}")]
	public struct EventPlayerData
	{
		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x060084B7 RID: 33975 RVA: 0x003243C7 File Offset: 0x003225C7
		// (set) Token: 0x060084B8 RID: 33976 RVA: 0x003243CF File Offset: 0x003225CF
		public int currentFrame { readonly get; set; }

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x060084B9 RID: 33977 RVA: 0x003243D8 File Offset: 0x003225D8
		// (set) Token: 0x060084BA RID: 33978 RVA: 0x003243E0 File Offset: 0x003225E0
		public int previousFrame { readonly get; set; }

		// Token: 0x060084BB RID: 33979 RVA: 0x003243E9 File Offset: 0x003225E9
		public ComponentType GetComponent<ComponentType>()
		{
			return this.controller.GetComponent<ComponentType>();
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x060084BC RID: 33980 RVA: 0x003243F6 File Offset: 0x003225F6
		public string name
		{
			get
			{
				return this.controller.name;
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x060084BD RID: 33981 RVA: 0x00324403 File Offset: 0x00322603
		public float normalizedTime
		{
			get
			{
				return this.elapsedTime / this.controller.CurrentAnim.totalTime;
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x060084BE RID: 33982 RVA: 0x0032441C File Offset: 0x0032261C
		public Vector3 position
		{
			get
			{
				return this.controller.transform.GetPosition();
			}
		}

		// Token: 0x060084BF RID: 33983 RVA: 0x0032442E File Offset: 0x0032262E
		public void AddUpdatingEvent(AnimEvent ev)
		{
			if (this.updatingEvents == null)
			{
				this.updatingEvents = new List<AnimEvent>();
			}
			this.updatingEvents.Add(ev);
		}

		// Token: 0x060084C0 RID: 33984 RVA: 0x0032444F File Offset: 0x0032264F
		public void SetElapsedTime(float elapsedTime)
		{
			this.elapsedTime = elapsedTime;
		}

		// Token: 0x060084C1 RID: 33985 RVA: 0x00324458 File Offset: 0x00322658
		public void FreeResources()
		{
			this.elapsedTime = 0f;
			this.mode = KAnim.PlayMode.Once;
			this.currentFrame = 0;
			this.previousFrame = 0;
			this.events = null;
			this.updatingEvents = null;
			this.controller = null;
		}

		// Token: 0x04006409 RID: 25609
		public float elapsedTime;

		// Token: 0x0400640A RID: 25610
		public KAnim.PlayMode mode;

		// Token: 0x0400640D RID: 25613
		public List<AnimEvent> events;

		// Token: 0x0400640E RID: 25614
		public List<AnimEvent> updatingEvents;

		// Token: 0x0400640F RID: 25615
		public KBatchedAnimController controller;
	}

	// Token: 0x020012B2 RID: 4786
	private struct IndirectionData
	{
		// Token: 0x060084C2 RID: 33986 RVA: 0x0032448F File Offset: 0x0032268F
		public IndirectionData(HandleVector<int>.Handle anim_data_handle, HandleVector<int>.Handle event_data_handle, bool is_ui_data)
		{
			this.isUIData = is_ui_data;
			this.animDataHandle = anim_data_handle;
			this.eventDataHandle = event_data_handle;
		}

		// Token: 0x04006410 RID: 25616
		public bool isUIData;

		// Token: 0x04006411 RID: 25617
		public HandleVector<int>.Handle animDataHandle;

		// Token: 0x04006412 RID: 25618
		public HandleVector<int>.Handle eventDataHandle;
	}

	// Token: 0x020012B3 RID: 4787
	public readonly struct DevTools_DebugInfo
	{
		// Token: 0x060084C3 RID: 33987 RVA: 0x003244A6 File Offset: 0x003226A6
		public DevTools_DebugInfo(AnimEventManager eventManager, KCompactedVector<AnimEventManager.AnimData> animData, KCompactedVector<AnimEventManager.EventPlayerData> eventData, KCompactedVector<AnimEventManager.AnimData> uiAnimData, KCompactedVector<AnimEventManager.EventPlayerData> uiEventData)
		{
			this.eventManager = eventManager;
			this.animData = animData;
			this.eventData = eventData;
			this.uiAnimData = uiAnimData;
			this.uiEventData = uiEventData;
		}

		// Token: 0x04006413 RID: 25619
		public readonly AnimEventManager eventManager;

		// Token: 0x04006414 RID: 25620
		public readonly KCompactedVector<AnimEventManager.AnimData> animData;

		// Token: 0x04006415 RID: 25621
		public readonly KCompactedVector<AnimEventManager.EventPlayerData> eventData;

		// Token: 0x04006416 RID: 25622
		public readonly KCompactedVector<AnimEventManager.AnimData> uiAnimData;

		// Token: 0x04006417 RID: 25623
		public readonly KCompactedVector<AnimEventManager.EventPlayerData> uiEventData;
	}
}
