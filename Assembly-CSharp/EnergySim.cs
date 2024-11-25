using System;
using System.Collections.Generic;

// Token: 0x02000881 RID: 2177
public class EnergySim
{
	// Token: 0x17000472 RID: 1138
	// (get) Token: 0x06003D0F RID: 15631 RVA: 0x001520DD File Offset: 0x001502DD
	public HashSet<Generator> Generators
	{
		get
		{
			return this.generators;
		}
	}

	// Token: 0x06003D10 RID: 15632 RVA: 0x001520E5 File Offset: 0x001502E5
	public void AddGenerator(Generator generator)
	{
		this.generators.Add(generator);
	}

	// Token: 0x06003D11 RID: 15633 RVA: 0x001520F4 File Offset: 0x001502F4
	public void RemoveGenerator(Generator generator)
	{
		this.generators.Remove(generator);
	}

	// Token: 0x06003D12 RID: 15634 RVA: 0x00152103 File Offset: 0x00150303
	public void AddManualGenerator(ManualGenerator manual_generator)
	{
		this.manualGenerators.Add(manual_generator);
	}

	// Token: 0x06003D13 RID: 15635 RVA: 0x00152112 File Offset: 0x00150312
	public void RemoveManualGenerator(ManualGenerator manual_generator)
	{
		this.manualGenerators.Remove(manual_generator);
	}

	// Token: 0x06003D14 RID: 15636 RVA: 0x00152121 File Offset: 0x00150321
	public void AddBattery(Battery battery)
	{
		this.batteries.Add(battery);
	}

	// Token: 0x06003D15 RID: 15637 RVA: 0x00152130 File Offset: 0x00150330
	public void RemoveBattery(Battery battery)
	{
		this.batteries.Remove(battery);
	}

	// Token: 0x06003D16 RID: 15638 RVA: 0x0015213F File Offset: 0x0015033F
	public void AddEnergyConsumer(EnergyConsumer energy_consumer)
	{
		this.energyConsumers.Add(energy_consumer);
	}

	// Token: 0x06003D17 RID: 15639 RVA: 0x0015214E File Offset: 0x0015034E
	public void RemoveEnergyConsumer(EnergyConsumer energy_consumer)
	{
		this.energyConsumers.Remove(energy_consumer);
	}

	// Token: 0x06003D18 RID: 15640 RVA: 0x00152160 File Offset: 0x00150360
	public void EnergySim200ms(float dt)
	{
		foreach (Generator generator in this.generators)
		{
			generator.EnergySim200ms(dt);
		}
		foreach (ManualGenerator manualGenerator in this.manualGenerators)
		{
			manualGenerator.EnergySim200ms(dt);
		}
		foreach (Battery battery in this.batteries)
		{
			battery.EnergySim200ms(dt);
		}
		foreach (EnergyConsumer energyConsumer in this.energyConsumers)
		{
			energyConsumer.EnergySim200ms(dt);
		}
	}

	// Token: 0x04002541 RID: 9537
	private HashSet<Generator> generators = new HashSet<Generator>();

	// Token: 0x04002542 RID: 9538
	private HashSet<ManualGenerator> manualGenerators = new HashSet<ManualGenerator>();

	// Token: 0x04002543 RID: 9539
	private HashSet<Battery> batteries = new HashSet<Battery>();

	// Token: 0x04002544 RID: 9540
	private HashSet<EnergyConsumer> energyConsumers = new HashSet<EnergyConsumer>();
}
