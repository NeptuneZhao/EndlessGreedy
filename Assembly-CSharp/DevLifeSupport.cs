using System;
using UnityEngine;

// Token: 0x020006B9 RID: 1721
public class DevLifeSupport : KMonoBehaviour, ISim200ms
{
	// Token: 0x06002B63 RID: 11107 RVA: 0x000F3C01 File Offset: 0x000F1E01
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.elementConsumer != null)
		{
			this.elementConsumer.EnableConsumption(true);
		}
	}

	// Token: 0x06002B64 RID: 11108 RVA: 0x000F3C24 File Offset: 0x000F1E24
	public void Sim200ms(float dt)
	{
		Vector2I vector2I = new Vector2I(-this.effectRadius, -this.effectRadius);
		Vector2I vector2I2 = new Vector2I(this.effectRadius, this.effectRadius);
		int num;
		int num2;
		Grid.PosToXY(base.transform.GetPosition(), out num, out num2);
		int num3 = Grid.XYToCell(num, num2);
		if (Grid.IsValidCell(num3))
		{
			int world = (int)Grid.WorldIdx[num3];
			for (int i = vector2I.y; i <= vector2I2.y; i++)
			{
				for (int j = vector2I.x; j <= vector2I2.x; j++)
				{
					int num4 = Grid.XYToCell(num + j, num2 + i);
					if (Grid.IsValidCellInWorld(num4, world))
					{
						float num5 = (this.targetTemperature - Grid.Temperature[num4]) * Grid.Element[num4].specificHeatCapacity * Grid.Mass[num4];
						if (!Mathf.Approximately(0f, num5))
						{
							SimMessages.ModifyEnergy(num4, num5 * 0.2f, 5000f, (num5 > 0f) ? SimMessages.EnergySourceID.DebugHeat : SimMessages.EnergySourceID.DebugCool);
						}
					}
				}
			}
		}
	}

	// Token: 0x040018E4 RID: 6372
	[MyCmpReq]
	private ElementConsumer elementConsumer;

	// Token: 0x040018E5 RID: 6373
	public float targetTemperature = 303.15f;

	// Token: 0x040018E6 RID: 6374
	public int effectRadius = 7;

	// Token: 0x040018E7 RID: 6375
	private const float temperatureControlK = 0.2f;
}
