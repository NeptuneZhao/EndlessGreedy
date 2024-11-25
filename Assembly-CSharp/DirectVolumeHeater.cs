using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020006BD RID: 1725
public class DirectVolumeHeater : KMonoBehaviour, ISim33ms, ISim200ms, ISim1000ms, ISim4000ms, IGameObjectEffectDescriptor
{
	// Token: 0x06002B79 RID: 11129 RVA: 0x000F3F6C File Offset: 0x000F216C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.primaryElement = base.GetComponent<PrimaryElement>();
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
	}

	// Token: 0x06002B7A RID: 11130 RVA: 0x000F3F98 File Offset: 0x000F2198
	public void Sim33ms(float dt)
	{
		if (this.impulseFrequency == DirectVolumeHeater.TimeMode.ms33)
		{
			float num = 0f;
			num += this.AddHeatToVolume(dt);
			num += this.AddSelfHeat(dt);
			this.heatEffect.SetHeatBeingProducedValue(num);
		}
	}

	// Token: 0x06002B7B RID: 11131 RVA: 0x000F3FD4 File Offset: 0x000F21D4
	public void Sim200ms(float dt)
	{
		if (this.impulseFrequency == DirectVolumeHeater.TimeMode.ms200)
		{
			float num = 0f;
			num += this.AddHeatToVolume(dt);
			num += this.AddSelfHeat(dt);
			this.heatEffect.SetHeatBeingProducedValue(num);
		}
	}

	// Token: 0x06002B7C RID: 11132 RVA: 0x000F4010 File Offset: 0x000F2210
	public void Sim1000ms(float dt)
	{
		if (this.impulseFrequency == DirectVolumeHeater.TimeMode.ms1000)
		{
			float num = 0f;
			num += this.AddHeatToVolume(dt);
			num += this.AddSelfHeat(dt);
			this.heatEffect.SetHeatBeingProducedValue(num);
		}
	}

	// Token: 0x06002B7D RID: 11133 RVA: 0x000F404C File Offset: 0x000F224C
	public void Sim4000ms(float dt)
	{
		if (this.impulseFrequency == DirectVolumeHeater.TimeMode.ms4000)
		{
			float num = 0f;
			num += this.AddHeatToVolume(dt);
			num += this.AddSelfHeat(dt);
			this.heatEffect.SetHeatBeingProducedValue(num);
		}
	}

	// Token: 0x06002B7E RID: 11134 RVA: 0x000F4088 File Offset: 0x000F2288
	private float CalculateCellWeight(int dx, int dy, int maxDistance)
	{
		return 1f + (float)(maxDistance - Math.Abs(dx) - Math.Abs(dy));
	}

	// Token: 0x06002B7F RID: 11135 RVA: 0x000F40A0 File Offset: 0x000F22A0
	private bool TestLineOfSight(int offsetCell)
	{
		int cell = Grid.PosToCell(base.gameObject);
		int x;
		int y;
		Grid.CellToXY(offsetCell, out x, out y);
		int x2;
		int y2;
		Grid.CellToXY(cell, out x2, out y2);
		return Grid.FastTestLineOfSightSolid(x2, y2, x, y);
	}

	// Token: 0x06002B80 RID: 11136 RVA: 0x000F40D4 File Offset: 0x000F22D4
	private float AddSelfHeat(float dt)
	{
		if (!this.EnableEmission)
		{
			return 0f;
		}
		if (this.primaryElement.Temperature > this.maximumInternalTemperature)
		{
			return 0f;
		}
		float result = 8f;
		GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, 8f * dt, BUILDINGS.PREFABS.STEAMTURBINE2.HEAT_SOURCE, dt);
		return result;
	}

	// Token: 0x06002B81 RID: 11137 RVA: 0x000F4130 File Offset: 0x000F2330
	private float AddHeatToVolume(float dt)
	{
		if (!this.EnableEmission)
		{
			return 0f;
		}
		int num = Grid.PosToCell(base.gameObject);
		int num2 = this.width / 2;
		int num3 = this.width % 2;
		int maxDistance = num2 + this.height;
		float num4 = 0f;
		float num5 = this.DTUs * dt / 1000f;
		for (int i = -num2; i < num2 + num3; i++)
		{
			for (int j = 0; j < this.height; j++)
			{
				if (Grid.IsCellOffsetValid(num, i, j))
				{
					int num6 = Grid.OffsetCell(num, i, j);
					if (!Grid.Solid[num6] && Grid.Mass[num6] != 0f && Grid.WorldIdx[num6] == Grid.WorldIdx[num] && this.TestLineOfSight(num6) && Grid.Temperature[num6] < this.maximumExternalTemperature)
					{
						num4 += this.CalculateCellWeight(i, j, maxDistance);
					}
				}
			}
		}
		float num7 = num5;
		if (num4 > 0f)
		{
			num7 /= num4;
		}
		float num8 = 0f;
		for (int k = -num2; k < num2 + num3; k++)
		{
			for (int l = 0; l < this.height; l++)
			{
				if (Grid.IsCellOffsetValid(num, k, l))
				{
					int num9 = Grid.OffsetCell(num, k, l);
					if (!Grid.Solid[num9] && Grid.Mass[num9] != 0f && Grid.WorldIdx[num9] == Grid.WorldIdx[num] && this.TestLineOfSight(num9) && Grid.Temperature[num9] < this.maximumExternalTemperature)
					{
						float num10 = num7 * this.CalculateCellWeight(k, l, maxDistance);
						num8 += num10;
						SimMessages.ModifyEnergy(num9, num10, 10000f, SimMessages.EnergySourceID.HeatBulb);
					}
				}
			}
		}
		return num8;
	}

	// Token: 0x06002B82 RID: 11138 RVA: 0x000F4318 File Offset: 0x000F2518
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string formattedHeatEnergy = GameUtil.GetFormattedHeatEnergy(this.DTUs, GameUtil.HeatEnergyFormatterUnit.Automatic);
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATGENERATED, formattedHeatEnergy), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED, formattedHeatEnergy), Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x040018EC RID: 6380
	[SerializeField]
	public int width = 12;

	// Token: 0x040018ED RID: 6381
	[SerializeField]
	public int height = 4;

	// Token: 0x040018EE RID: 6382
	[SerializeField]
	public float DTUs = 100000f;

	// Token: 0x040018EF RID: 6383
	[SerializeField]
	public float maximumInternalTemperature = 773.15f;

	// Token: 0x040018F0 RID: 6384
	[SerializeField]
	public float maximumExternalTemperature = 340f;

	// Token: 0x040018F1 RID: 6385
	[SerializeField]
	public Operational operational;

	// Token: 0x040018F2 RID: 6386
	[MyCmpAdd]
	private KBatchedAnimHeatPostProcessingEffect heatEffect;

	// Token: 0x040018F3 RID: 6387
	public bool EnableEmission;

	// Token: 0x040018F4 RID: 6388
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x040018F5 RID: 6389
	private PrimaryElement primaryElement;

	// Token: 0x040018F6 RID: 6390
	[SerializeField]
	private DirectVolumeHeater.TimeMode impulseFrequency = DirectVolumeHeater.TimeMode.ms1000;

	// Token: 0x020014B9 RID: 5305
	private enum TimeMode
	{
		// Token: 0x04006ABA RID: 27322
		ms33,
		// Token: 0x04006ABB RID: 27323
		ms200,
		// Token: 0x04006ABC RID: 27324
		ms1000,
		// Token: 0x04006ABD RID: 27325
		ms4000
	}
}
