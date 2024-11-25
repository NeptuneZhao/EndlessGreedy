using System;

namespace EventSystem2Syntax
{
	// Token: 0x02000E23 RID: 3619
	internal class NewExample : KMonoBehaviour2
	{
		// Token: 0x0600735F RID: 29535 RVA: 0x002C4128 File Offset: 0x002C2328
		protected override void OnPrefabInit()
		{
			base.Subscribe<NewExample, NewExample.ObjectDestroyedEvent>(new Action<NewExample, NewExample.ObjectDestroyedEvent>(NewExample.OnObjectDestroyed));
			base.Trigger<NewExample.ObjectDestroyedEvent>(new NewExample.ObjectDestroyedEvent
			{
				parameter = false
			});
		}

		// Token: 0x06007360 RID: 29536 RVA: 0x002C415E File Offset: 0x002C235E
		private static void OnObjectDestroyed(NewExample example, NewExample.ObjectDestroyedEvent evt)
		{
		}

		// Token: 0x02001F4C RID: 8012
		private struct ObjectDestroyedEvent : IEventData
		{
			// Token: 0x04008D37 RID: 36151
			public bool parameter;
		}
	}
}
