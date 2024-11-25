using System;

namespace Database
{
	// Token: 0x02000E44 RID: 3652
	public class ArtableStatusItem : StatusItem
	{
		// Token: 0x06007420 RID: 29728 RVA: 0x002C6760 File Offset: 0x002C4960
		public ArtableStatusItem(string id, ArtableStatuses.ArtableStatusType statusType) : base(id, "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null)
		{
			this.StatusType = statusType;
		}

		// Token: 0x04004FF8 RID: 20472
		public ArtableStatuses.ArtableStatusType StatusType;
	}
}
