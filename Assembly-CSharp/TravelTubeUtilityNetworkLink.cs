using System;

// Token: 0x02000B3A RID: 2874
public class TravelTubeUtilityNetworkLink : UtilityNetworkLink, IHaveUtilityNetworkMgr
{
	// Token: 0x060055C7 RID: 21959 RVA: 0x001EA438 File Offset: 0x001E8638
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060055C8 RID: 21960 RVA: 0x001EA440 File Offset: 0x001E8640
	protected override void OnConnect(int cell1, int cell2)
	{
		Game.Instance.travelTubeSystem.AddLink(cell1, cell2);
	}

	// Token: 0x060055C9 RID: 21961 RVA: 0x001EA453 File Offset: 0x001E8653
	protected override void OnDisconnect(int cell1, int cell2)
	{
		Game.Instance.travelTubeSystem.RemoveLink(cell1, cell2);
	}

	// Token: 0x060055CA RID: 21962 RVA: 0x001EA466 File Offset: 0x001E8666
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.travelTubeSystem;
	}
}
