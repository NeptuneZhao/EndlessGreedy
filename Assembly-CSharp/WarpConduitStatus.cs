using System;
using UnityEngine;

// Token: 0x02000795 RID: 1941
public static class WarpConduitStatus
{
	// Token: 0x06003517 RID: 13591 RVA: 0x00121510 File Offset: 0x0011F710
	public static void UpdateWarpConduitsOperational(GameObject sender, GameObject receiver)
	{
		object obj = sender != null && sender.GetComponent<Activatable>().IsActivated;
		bool flag = receiver != null && receiver.GetComponent<Activatable>().IsActivated;
		object obj2 = obj;
		bool value = (obj2 & flag) != null;
		int num = 0;
		if (obj2 != null)
		{
			num++;
		}
		if (flag)
		{
			num++;
		}
		if (sender != null)
		{
			sender.GetComponent<Operational>().SetFlag(WarpConduitStatus.warpConnectedFlag, value);
			if (num != 2)
			{
				sender.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled, false);
				sender.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled, num);
			}
			else
			{
				sender.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled, false);
			}
		}
		if (receiver != null)
		{
			receiver.GetComponent<Operational>().SetFlag(WarpConduitStatus.warpConnectedFlag, value);
			if (num != 2)
			{
				receiver.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled, false);
				receiver.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled, num);
				return;
			}
			receiver.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled, false);
		}
	}

	// Token: 0x04001F85 RID: 8069
	public static readonly Operational.Flag warpConnectedFlag = new Operational.Flag("warp_conduit_connected", Operational.Flag.Type.Requirement);
}
