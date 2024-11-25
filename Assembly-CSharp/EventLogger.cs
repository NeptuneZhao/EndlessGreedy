using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;

// Token: 0x020008A6 RID: 2214
[SerializationConfig(MemberSerialization.OptIn)]
public class EventLogger<EventInstanceType, EventType> : KMonoBehaviour, ISaveLoadable where EventInstanceType : EventInstanceBase where EventType : EventBase
{
	// Token: 0x06003DE3 RID: 15843 RVA: 0x001560C6 File Offset: 0x001542C6
	public IEnumerator<EventInstanceType> GetEnumerator()
	{
		return this.EventInstances.GetEnumerator();
	}

	// Token: 0x06003DE4 RID: 15844 RVA: 0x001560D8 File Offset: 0x001542D8
	public EventType AddEvent(EventType ev)
	{
		for (int i = 0; i < this.Events.Count; i++)
		{
			if (this.Events[i].hash == ev.hash)
			{
				this.Events[i] = ev;
				return this.Events[i];
			}
		}
		this.Events.Add(ev);
		return ev;
	}

	// Token: 0x06003DE5 RID: 15845 RVA: 0x00156145 File Offset: 0x00154345
	public EventInstanceType Add(EventInstanceType ev)
	{
		if (this.EventInstances.Count > 10000)
		{
			this.EventInstances.RemoveAt(0);
		}
		this.EventInstances.Add(ev);
		return ev;
	}

	// Token: 0x06003DE6 RID: 15846 RVA: 0x00156174 File Offset: 0x00154374
	[OnDeserialized]
	protected internal void OnDeserialized()
	{
		if (this.EventInstances.Count > 10000)
		{
			this.EventInstances.RemoveRange(0, this.EventInstances.Count - 10000);
		}
		for (int i = 0; i < this.EventInstances.Count; i++)
		{
			for (int j = 0; j < this.Events.Count; j++)
			{
				if (this.Events[j].hash == this.EventInstances[i].eventHash)
				{
					this.EventInstances[i].ev = this.Events[j];
					break;
				}
			}
		}
	}

	// Token: 0x06003DE7 RID: 15847 RVA: 0x00156233 File Offset: 0x00154433
	public void Clear()
	{
		this.EventInstances.Clear();
	}

	// Token: 0x040025FA RID: 9722
	private const int MAX_NUM_EVENTS = 10000;

	// Token: 0x040025FB RID: 9723
	private List<EventType> Events = new List<EventType>();

	// Token: 0x040025FC RID: 9724
	[Serialize]
	private List<EventInstanceType> EventInstances = new List<EventInstanceType>();
}
