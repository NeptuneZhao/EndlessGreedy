using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000E78 RID: 3704
	public class RoomTypeCategories : ResourceSet<RoomTypeCategory>
	{
		// Token: 0x060074CF RID: 29903 RVA: 0x002D9554 File Offset: 0x002D7754
		private RoomTypeCategory Add(string id, string name, string colorName, string icon)
		{
			RoomTypeCategory roomTypeCategory = new RoomTypeCategory(id, name, colorName, icon);
			base.Add(roomTypeCategory);
			return roomTypeCategory;
		}

		// Token: 0x060074D0 RID: 29904 RVA: 0x002D9578 File Offset: 0x002D7778
		public RoomTypeCategories(ResourceSet parent) : base("RoomTypeCategories", parent)
		{
			base.Initialize();
			this.None = this.Add("None", ROOMS.CATEGORY.NONE.NAME, "roomNone", "unknown");
			this.Food = this.Add("Food", ROOMS.CATEGORY.FOOD.NAME, "roomFood", "ui_room_food");
			this.Sleep = this.Add("Sleep", ROOMS.CATEGORY.SLEEP.NAME, "roomSleep", "ui_room_sleep");
			this.Recreation = this.Add("Recreation", ROOMS.CATEGORY.RECREATION.NAME, "roomRecreation", "ui_room_recreational");
			if (DlcManager.IsContentSubscribed("DLC3_ID"))
			{
				this.Bionic = this.Add("Bionic", ROOMS.CATEGORY.BIONIC.NAME, "roomBionic", "ui_room_bionicupkeep");
			}
			this.Bathroom = this.Add("Bathroom", ROOMS.CATEGORY.BATHROOM.NAME, "roomBathroom", "ui_room_bathroom");
			this.Hospital = this.Add("Hospital", ROOMS.CATEGORY.HOSPITAL.NAME, "roomHospital", "ui_room_hospital");
			this.Industrial = this.Add("Industrial", ROOMS.CATEGORY.INDUSTRIAL.NAME, "roomIndustrial", "ui_room_industrial");
			this.Agricultural = this.Add("Agricultural", ROOMS.CATEGORY.AGRICULTURAL.NAME, "roomAgricultural", "ui_room_agricultural");
			this.Park = this.Add("Park", ROOMS.CATEGORY.PARK.NAME, "roomPark", "ui_room_park");
			this.Science = this.Add("Science", ROOMS.CATEGORY.SCIENCE.NAME, "roomScience", "ui_room_science");
		}

		// Token: 0x0400546D RID: 21613
		public RoomTypeCategory None;

		// Token: 0x0400546E RID: 21614
		public RoomTypeCategory Food;

		// Token: 0x0400546F RID: 21615
		public RoomTypeCategory Sleep;

		// Token: 0x04005470 RID: 21616
		public RoomTypeCategory Recreation;

		// Token: 0x04005471 RID: 21617
		public RoomTypeCategory Bathroom;

		// Token: 0x04005472 RID: 21618
		public RoomTypeCategory Bionic;

		// Token: 0x04005473 RID: 21619
		public RoomTypeCategory Hospital;

		// Token: 0x04005474 RID: 21620
		public RoomTypeCategory Industrial;

		// Token: 0x04005475 RID: 21621
		public RoomTypeCategory Agricultural;

		// Token: 0x04005476 RID: 21622
		public RoomTypeCategory Park;

		// Token: 0x04005477 RID: 21623
		public RoomTypeCategory Science;
	}
}
