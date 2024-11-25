using System;
using KSerialization;

// Token: 0x02000747 RID: 1863
[SerializationConfig(MemberSerialization.OptIn)]
public class PartialLightBlocking : KMonoBehaviour
{
	// Token: 0x0600319D RID: 12701 RVA: 0x00110FC5 File Offset: 0x0010F1C5
	protected override void OnSpawn()
	{
		this.SetLightBlocking();
		base.OnSpawn();
	}

	// Token: 0x0600319E RID: 12702 RVA: 0x00110FD3 File Offset: 0x0010F1D3
	protected override void OnCleanUp()
	{
		this.ClearLightBlocking();
		base.OnCleanUp();
	}

	// Token: 0x0600319F RID: 12703 RVA: 0x00110FE4 File Offset: 0x0010F1E4
	public void SetLightBlocking()
	{
		int[] placementCells = base.GetComponent<Building>().PlacementCells;
		for (int i = 0; i < placementCells.Length; i++)
		{
			SimMessages.SetCellProperties(placementCells[i], 48);
		}
	}

	// Token: 0x060031A0 RID: 12704 RVA: 0x00111018 File Offset: 0x0010F218
	public void ClearLightBlocking()
	{
		int[] placementCells = base.GetComponent<Building>().PlacementCells;
		for (int i = 0; i < placementCells.Length; i++)
		{
			SimMessages.ClearCellProperties(placementCells[i], 48);
		}
	}

	// Token: 0x04001D2D RID: 7469
	private const byte PartialLightBlockingProperties = 48;
}
