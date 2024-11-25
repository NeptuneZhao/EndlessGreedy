using System;
using UnityEngine;

// Token: 0x02000B34 RID: 2868
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/TileTemperature")]
public class TileTemperature : KMonoBehaviour
{
	// Token: 0x06005581 RID: 21889 RVA: 0x001E8D4C File Offset: 0x001E6F4C
	protected override void OnPrefabInit()
	{
		this.primaryElement.getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(TileTemperature.OnGetTemperature);
		this.primaryElement.setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(TileTemperature.OnSetTemperature);
		base.OnPrefabInit();
	}

	// Token: 0x06005582 RID: 21890 RVA: 0x001E8D82 File Offset: 0x001E6F82
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06005583 RID: 21891 RVA: 0x001E8D8C File Offset: 0x001E6F8C
	private static float OnGetTemperature(PrimaryElement primary_element)
	{
		SimCellOccupier component = primary_element.GetComponent<SimCellOccupier>();
		if (component != null && component.IsReady())
		{
			int i = Grid.PosToCell(primary_element.transform.GetPosition());
			return Grid.Temperature[i];
		}
		return primary_element.InternalTemperature;
	}

	// Token: 0x06005584 RID: 21892 RVA: 0x001E8DD4 File Offset: 0x001E6FD4
	private static void OnSetTemperature(PrimaryElement primary_element, float temperature)
	{
		SimCellOccupier component = primary_element.GetComponent<SimCellOccupier>();
		if (component != null && component.IsReady())
		{
			global::Debug.LogWarning("Only set a tile's temperature during initialization. Otherwise you should be modifying the cell via the sim!");
			return;
		}
		primary_element.InternalTemperature = temperature;
	}

	// Token: 0x04003803 RID: 14339
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04003804 RID: 14340
	[MyCmpReq]
	private KSelectable selectable;
}
