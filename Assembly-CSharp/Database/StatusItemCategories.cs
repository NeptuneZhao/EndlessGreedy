using System;

namespace Database
{
	// Token: 0x02000E83 RID: 3715
	public class StatusItemCategories : ResourceSet<StatusItemCategory>
	{
		// Token: 0x060074F8 RID: 29944 RVA: 0x002DC208 File Offset: 0x002DA408
		public StatusItemCategories(ResourceSet parent) : base("StatusItemCategories", parent)
		{
			this.Main = new StatusItemCategory("Main", this, "Main");
			this.Role = new StatusItemCategory("Role", this, "Role");
			this.Power = new StatusItemCategory("Power", this, "Power");
			this.Toilet = new StatusItemCategory("Toilet", this, "Toilet");
			this.Research = new StatusItemCategory("Research", this, "Research");
			this.Hitpoints = new StatusItemCategory("Hitpoints", this, "Hitpoints");
			this.Suffocation = new StatusItemCategory("Suffocation", this, "Suffocation");
			this.WoundEffects = new StatusItemCategory("WoundEffects", this, "WoundEffects");
			this.EntityReceptacle = new StatusItemCategory("EntityReceptacle", this, "EntityReceptacle");
			this.PreservationState = new StatusItemCategory("PreservationState", this, "PreservationState");
			this.PreservationTemperature = new StatusItemCategory("PreservationTemperature", this, "PreservationTemperature");
			this.PreservationAtmosphere = new StatusItemCategory("PreservationAtmosphere", this, "PreservationAtmosphere");
			this.ExhaustTemperature = new StatusItemCategory("ExhaustTemperature", this, "ExhaustTemperature");
			this.OperatingEnergy = new StatusItemCategory("OperatingEnergy", this, "OperatingEnergy");
			this.AccessControl = new StatusItemCategory("AccessControl", this, "AccessControl");
			this.RequiredRoom = new StatusItemCategory("RequiredRoom", this, "RequiredRoom");
			this.Yield = new StatusItemCategory("Yield", this, "Yield");
			this.Heat = new StatusItemCategory("Heat", this, "Heat");
			this.Stored = new StatusItemCategory("Stored", this, "Stored");
			this.Ownable = new StatusItemCategory("Ownable", this, "Ownable");
		}

		// Token: 0x040054D4 RID: 21716
		public StatusItemCategory Main;

		// Token: 0x040054D5 RID: 21717
		public StatusItemCategory Role;

		// Token: 0x040054D6 RID: 21718
		public StatusItemCategory Power;

		// Token: 0x040054D7 RID: 21719
		public StatusItemCategory Toilet;

		// Token: 0x040054D8 RID: 21720
		public StatusItemCategory Research;

		// Token: 0x040054D9 RID: 21721
		public StatusItemCategory Hitpoints;

		// Token: 0x040054DA RID: 21722
		public StatusItemCategory Suffocation;

		// Token: 0x040054DB RID: 21723
		public StatusItemCategory WoundEffects;

		// Token: 0x040054DC RID: 21724
		public StatusItemCategory EntityReceptacle;

		// Token: 0x040054DD RID: 21725
		public StatusItemCategory PreservationState;

		// Token: 0x040054DE RID: 21726
		public StatusItemCategory PreservationAtmosphere;

		// Token: 0x040054DF RID: 21727
		public StatusItemCategory PreservationTemperature;

		// Token: 0x040054E0 RID: 21728
		public StatusItemCategory ExhaustTemperature;

		// Token: 0x040054E1 RID: 21729
		public StatusItemCategory OperatingEnergy;

		// Token: 0x040054E2 RID: 21730
		public StatusItemCategory AccessControl;

		// Token: 0x040054E3 RID: 21731
		public StatusItemCategory RequiredRoom;

		// Token: 0x040054E4 RID: 21732
		public StatusItemCategory Yield;

		// Token: 0x040054E5 RID: 21733
		public StatusItemCategory Heat;

		// Token: 0x040054E6 RID: 21734
		public StatusItemCategory Stored;

		// Token: 0x040054E7 RID: 21735
		public StatusItemCategory Ownable;
	}
}
