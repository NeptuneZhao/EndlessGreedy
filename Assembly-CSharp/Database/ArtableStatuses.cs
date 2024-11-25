using System;

namespace Database
{
	// Token: 0x02000E43 RID: 3651
	public class ArtableStatuses : ResourceSet<ArtableStatusItem>
	{
		// Token: 0x0600741E RID: 29726 RVA: 0x002C66D8 File Offset: 0x002C48D8
		public ArtableStatuses(ResourceSet parent) : base("ArtableStatuses", parent)
		{
			this.AwaitingArting = this.Add("AwaitingArting", ArtableStatuses.ArtableStatusType.AwaitingArting);
			this.LookingUgly = this.Add("LookingUgly", ArtableStatuses.ArtableStatusType.LookingUgly);
			this.LookingOkay = this.Add("LookingOkay", ArtableStatuses.ArtableStatusType.LookingOkay);
			this.LookingGreat = this.Add("LookingGreat", ArtableStatuses.ArtableStatusType.LookingGreat);
		}

		// Token: 0x0600741F RID: 29727 RVA: 0x002C673C File Offset: 0x002C493C
		public ArtableStatusItem Add(string id, ArtableStatuses.ArtableStatusType statusType)
		{
			ArtableStatusItem artableStatusItem = new ArtableStatusItem(id, statusType);
			this.resources.Add(artableStatusItem);
			return artableStatusItem;
		}

		// Token: 0x04004FF4 RID: 20468
		public ArtableStatusItem AwaitingArting;

		// Token: 0x04004FF5 RID: 20469
		public ArtableStatusItem LookingUgly;

		// Token: 0x04004FF6 RID: 20470
		public ArtableStatusItem LookingOkay;

		// Token: 0x04004FF7 RID: 20471
		public ArtableStatusItem LookingGreat;

		// Token: 0x02001F5A RID: 8026
		public enum ArtableStatusType
		{
			// Token: 0x04008D50 RID: 36176
			AwaitingArting,
			// Token: 0x04008D51 RID: 36177
			LookingUgly,
			// Token: 0x04008D52 RID: 36178
			LookingOkay,
			// Token: 0x04008D53 RID: 36179
			LookingGreat
		}
	}
}
