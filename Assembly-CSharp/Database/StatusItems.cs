using System;
using System.Diagnostics;

namespace Database
{
	// Token: 0x02000E84 RID: 3716
	public class StatusItems : ResourceSet<StatusItem>
	{
		// Token: 0x060074F9 RID: 29945 RVA: 0x002DC3D9 File Offset: 0x002DA5D9
		public StatusItems(string id, ResourceSet parent) : base(id, parent)
		{
		}

		// Token: 0x02001F79 RID: 8057
		[DebuggerDisplay("{Id}")]
		public class StatusItemInfo : Resource
		{
			// Token: 0x04008EBB RID: 36539
			public string Type;

			// Token: 0x04008EBC RID: 36540
			public string Tooltip;

			// Token: 0x04008EBD RID: 36541
			public bool IsIconTinted;

			// Token: 0x04008EBE RID: 36542
			public StatusItem.IconType IconType;

			// Token: 0x04008EBF RID: 36543
			public string Icon;

			// Token: 0x04008EC0 RID: 36544
			public string SoundPath;

			// Token: 0x04008EC1 RID: 36545
			public bool ShouldNotify;

			// Token: 0x04008EC2 RID: 36546
			public float NotificationDelay;

			// Token: 0x04008EC3 RID: 36547
			public NotificationType NotificationType;

			// Token: 0x04008EC4 RID: 36548
			public bool AllowMultiples;

			// Token: 0x04008EC5 RID: 36549
			public string Effect;

			// Token: 0x04008EC6 RID: 36550
			public HashedString Overlay;

			// Token: 0x04008EC7 RID: 36551
			public HashedString SecondOverlay;
		}
	}
}
