using System;

namespace EventSystem2Syntax
{
	// Token: 0x02000E24 RID: 3620
	internal class KMonoBehaviour2
	{
		// Token: 0x06007362 RID: 29538 RVA: 0x002C4168 File Offset: 0x002C2368
		protected virtual void OnPrefabInit()
		{
		}

		// Token: 0x06007363 RID: 29539 RVA: 0x002C416A File Offset: 0x002C236A
		public void Subscribe(int evt, Action<object> cb)
		{
		}

		// Token: 0x06007364 RID: 29540 RVA: 0x002C416C File Offset: 0x002C236C
		public void Trigger(int evt, object data)
		{
		}

		// Token: 0x06007365 RID: 29541 RVA: 0x002C416E File Offset: 0x002C236E
		public void Subscribe<ListenerType, EventType>(Action<ListenerType, EventType> cb) where EventType : IEventData
		{
		}

		// Token: 0x06007366 RID: 29542 RVA: 0x002C4170 File Offset: 0x002C2370
		public void Trigger<EventType>(EventType evt) where EventType : IEventData
		{
		}
	}
}
