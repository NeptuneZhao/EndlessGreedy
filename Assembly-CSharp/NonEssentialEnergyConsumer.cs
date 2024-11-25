using System;

// Token: 0x020009C7 RID: 2503
public class NonEssentialEnergyConsumer : EnergyConsumer
{
	// Token: 0x1700051A RID: 1306
	// (get) Token: 0x060048B6 RID: 18614 RVA: 0x0019FE05 File Offset: 0x0019E005
	// (set) Token: 0x060048B7 RID: 18615 RVA: 0x0019FE0D File Offset: 0x0019E00D
	public override bool IsPowered
	{
		get
		{
			return this.isPowered;
		}
		protected set
		{
			if (value == this.isPowered)
			{
				return;
			}
			this.isPowered = value;
			Action<bool> poweredStateChanged = this.PoweredStateChanged;
			if (poweredStateChanged == null)
			{
				return;
			}
			poweredStateChanged(this.isPowered);
		}
	}

	// Token: 0x04002F96 RID: 12182
	public Action<bool> PoweredStateChanged;

	// Token: 0x04002F97 RID: 12183
	private bool isPowered;
}
