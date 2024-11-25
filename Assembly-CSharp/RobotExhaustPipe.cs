using System;
using TUNING;
using UnityEngine;

// Token: 0x02000A63 RID: 2659
[AddComponentMenu("KMonoBehaviour/scripts/RobotExhaustPipe")]
public class RobotExhaustPipe : KMonoBehaviour, ISim4000ms
{
	// Token: 0x06004D3B RID: 19771 RVA: 0x001BAA44 File Offset: 0x001B8C44
	public void Sim4000ms(float dt)
	{
		Facing component = base.GetComponent<Facing>();
		bool flip = false;
		if (component)
		{
			flip = component.GetFacing();
		}
		CO2Manager.instance.SpawnBreath(Grid.CellToPos(Grid.PosToCell(base.gameObject)), dt * this.CO2_RATE, 303.15f, flip);
	}

	// Token: 0x0400334A RID: 13130
	private float CO2_RATE = DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND * DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_TO_CO2_CONVERSION / 2f;
}
