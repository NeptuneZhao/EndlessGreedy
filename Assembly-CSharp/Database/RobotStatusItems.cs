using System;
using UnityEngine;

namespace Database
{
	// Token: 0x02000E77 RID: 3703
	public class RobotStatusItems : StatusItems
	{
		// Token: 0x060074CD RID: 29901 RVA: 0x002D919D File Offset: 0x002D739D
		public RobotStatusItems(ResourceSet parent) : base("RobotStatusItems", parent)
		{
			this.CreateStatusItems();
		}

		// Token: 0x060074CE RID: 29902 RVA: 0x002D91B4 File Offset: 0x002D73B4
		private void CreateStatusItems()
		{
			this.CantReachStation = new StatusItem("CantReachStation", "ROBOTS", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.CantReachStation.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.LowBattery = new StatusItem("LowBattery", "ROBOTS", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.LowBattery.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.LowBatteryNoCharge = new StatusItem("LowBatteryNoCharge", "ROBOTS", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.LowBatteryNoCharge.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.DeadBattery = new StatusItem("DeadBattery", "ROBOTS", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.DeadBattery.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.DeadBatteryFlydo = new StatusItem("DeadBatteryFlydo", "ROBOTS", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.DeadBatteryFlydo.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.DustBinFull = new StatusItem("DustBinFull", "ROBOTS", "status_item_pending_clear", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.DustBinFull.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.Working = new StatusItem("Working", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.Working.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.MovingToChargeStation = new StatusItem("MovingToChargeStation", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.MovingToChargeStation.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.UnloadingStorage = new StatusItem("UnloadingStorage", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.UnloadingStorage.resolveStringCallback = delegate(string str, object data)
			{
				GameObject go = (GameObject)data;
				return str.Replace("{0}", go.GetProperName());
			};
			this.ReactPositive = new StatusItem("ReactPositive", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.ReactPositive.resolveStringCallback = ((string str, object data) => str);
			this.ReactNegative = new StatusItem("ReactNegative", "ROBOTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.ReactNegative.resolveStringCallback = ((string str, object data) => str);
		}

		// Token: 0x04005462 RID: 21602
		public StatusItem LowBattery;

		// Token: 0x04005463 RID: 21603
		public StatusItem LowBatteryNoCharge;

		// Token: 0x04005464 RID: 21604
		public StatusItem DeadBattery;

		// Token: 0x04005465 RID: 21605
		public StatusItem DeadBatteryFlydo;

		// Token: 0x04005466 RID: 21606
		public StatusItem CantReachStation;

		// Token: 0x04005467 RID: 21607
		public StatusItem DustBinFull;

		// Token: 0x04005468 RID: 21608
		public StatusItem Working;

		// Token: 0x04005469 RID: 21609
		public StatusItem UnloadingStorage;

		// Token: 0x0400546A RID: 21610
		public StatusItem ReactPositive;

		// Token: 0x0400546B RID: 21611
		public StatusItem ReactNegative;

		// Token: 0x0400546C RID: 21612
		public StatusItem MovingToChargeStation;
	}
}
