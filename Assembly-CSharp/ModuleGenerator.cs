using System;

// Token: 0x020009B4 RID: 2484
public class ModuleGenerator : Generator
{
	// Token: 0x06004828 RID: 18472 RVA: 0x0019D63C File Offset: 0x0019B83C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.connectedTags = new Tag[0];
		base.IsVirtual = true;
	}

	// Token: 0x06004829 RID: 18473 RVA: 0x0019D658 File Offset: 0x0019B858
	protected override void OnSpawn()
	{
		CraftModuleInterface craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		base.VirtualCircuitKey = craftInterface;
		this.clustercraft = craftInterface.GetComponent<Clustercraft>();
		Game.Instance.electricalConduitSystem.AddToVirtualNetworks(base.VirtualCircuitKey, this, true);
		base.OnSpawn();
	}

	// Token: 0x0600482A RID: 18474 RVA: 0x0019D6A1 File Offset: 0x0019B8A1
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.electricalConduitSystem.RemoveFromVirtualNetworks(base.VirtualCircuitKey, this, true);
	}

	// Token: 0x0600482B RID: 18475 RVA: 0x0019D6C0 File Offset: 0x0019B8C0
	public override bool IsProducingPower()
	{
		return this.clustercraft.IsFlightInProgress();
	}

	// Token: 0x0600482C RID: 18476 RVA: 0x0019D6D0 File Offset: 0x0019B8D0
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		if (this.IsProducingPower())
		{
			base.GenerateJoules(base.WattageRating * dt, false);
			if (this.poweringStatusItemHandle == Guid.Empty)
			{
				this.poweringStatusItemHandle = this.selectable.ReplaceStatusItem(this.notPoweringStatusItemHandle, Db.Get().BuildingStatusItems.ModuleGeneratorPowered, this);
				this.notPoweringStatusItemHandle = Guid.Empty;
				return;
			}
		}
		else if (this.notPoweringStatusItemHandle == Guid.Empty)
		{
			this.notPoweringStatusItemHandle = this.selectable.ReplaceStatusItem(this.poweringStatusItemHandle, Db.Get().BuildingStatusItems.ModuleGeneratorNotPowered, this);
			this.poweringStatusItemHandle = Guid.Empty;
		}
	}

	// Token: 0x04002F57 RID: 12119
	private Clustercraft clustercraft;

	// Token: 0x04002F58 RID: 12120
	private Guid poweringStatusItemHandle;

	// Token: 0x04002F59 RID: 12121
	private Guid notPoweringStatusItemHandle;
}
