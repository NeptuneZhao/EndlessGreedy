using System;
using KSerialization;
using UnityEngine;

// Token: 0x020009F1 RID: 2545
[AddComponentMenu("KMonoBehaviour/scripts/HeatBulb")]
public class HeatBulb : KMonoBehaviour, ISim200ms
{
	// Token: 0x060049BB RID: 18875 RVA: 0x001A5E21 File Offset: 0x001A4021
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.kanim.Play("off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x060049BC RID: 18876 RVA: 0x001A5E4C File Offset: 0x001A404C
	public void Sim200ms(float dt)
	{
		float num = this.kjConsumptionRate * dt;
		Vector2I vector2I = this.maxCheckOffset - this.minCheckOffset + 1;
		int num2 = vector2I.x * vector2I.y;
		float num3 = num / (float)num2;
		int num4;
		int num5;
		Grid.PosToXY(base.transform.GetPosition(), out num4, out num5);
		for (int i = this.minCheckOffset.y; i <= this.maxCheckOffset.y; i++)
		{
			for (int j = this.minCheckOffset.x; j <= this.maxCheckOffset.x; j++)
			{
				int num6 = Grid.XYToCell(num4 + j, num5 + i);
				if (Grid.IsValidCell(num6) && Grid.Temperature[num6] > this.minTemperature)
				{
					this.kjConsumed += num3;
					SimMessages.ModifyEnergy(num6, -num3, 5000f, SimMessages.EnergySourceID.HeatBulb);
				}
			}
		}
		float num7 = this.lightKJConsumptionRate * dt;
		if (this.kjConsumed > num7)
		{
			if (!this.lightSource.enabled)
			{
				this.kanim.Play("open", KAnim.PlayMode.Once, 1f, 0f);
				this.kanim.Queue("on", KAnim.PlayMode.Once, 1f, 0f);
				this.lightSource.enabled = true;
			}
			this.kjConsumed -= num7;
			return;
		}
		if (this.lightSource.enabled)
		{
			this.kanim.Play("close", KAnim.PlayMode.Once, 1f, 0f);
			this.kanim.Queue("off", KAnim.PlayMode.Once, 1f, 0f);
		}
		this.lightSource.enabled = false;
	}

	// Token: 0x04003051 RID: 12369
	[SerializeField]
	private float minTemperature;

	// Token: 0x04003052 RID: 12370
	[SerializeField]
	private float kjConsumptionRate;

	// Token: 0x04003053 RID: 12371
	[SerializeField]
	private float lightKJConsumptionRate;

	// Token: 0x04003054 RID: 12372
	[SerializeField]
	private Vector2I minCheckOffset;

	// Token: 0x04003055 RID: 12373
	[SerializeField]
	private Vector2I maxCheckOffset;

	// Token: 0x04003056 RID: 12374
	[MyCmpGet]
	private Light2D lightSource;

	// Token: 0x04003057 RID: 12375
	[MyCmpGet]
	private KBatchedAnimController kanim;

	// Token: 0x04003058 RID: 12376
	[Serialize]
	private float kjConsumed;
}
