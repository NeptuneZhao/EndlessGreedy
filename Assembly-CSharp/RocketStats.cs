using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000AC3 RID: 2755
public class RocketStats
{
	// Token: 0x060051A0 RID: 20896 RVA: 0x001D454E File Offset: 0x001D274E
	public RocketStats(CommandModule commandModule)
	{
		this.commandModule = commandModule;
	}

	// Token: 0x060051A1 RID: 20897 RVA: 0x001D4560 File Offset: 0x001D2760
	public float GetRocketMaxDistance()
	{
		float totalMass = this.GetTotalMass();
		float totalThrust = this.GetTotalThrust();
		float num = ROCKETRY.CalculateMassWithPenalty(totalMass);
		return Mathf.Max(0f, totalThrust - num);
	}

	// Token: 0x060051A2 RID: 20898 RVA: 0x001D458D File Offset: 0x001D278D
	public float GetTotalMass()
	{
		return this.GetDryMass() + this.GetWetMass();
	}

	// Token: 0x060051A3 RID: 20899 RVA: 0x001D459C File Offset: 0x001D279C
	public float GetDryMass()
	{
		float num = 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			RocketModule component = gameObject.GetComponent<RocketModule>();
			if (component != null)
			{
				num += component.GetComponent<PrimaryElement>().Mass;
			}
		}
		return num;
	}

	// Token: 0x060051A4 RID: 20900 RVA: 0x001D4618 File Offset: 0x001D2818
	public float GetWetMass()
	{
		float num = 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			RocketModule component = gameObject.GetComponent<RocketModule>();
			if (component != null)
			{
				FuelTank component2 = component.GetComponent<FuelTank>();
				OxidizerTank component3 = component.GetComponent<OxidizerTank>();
				SolidBooster component4 = component.GetComponent<SolidBooster>();
				if (component2 != null)
				{
					num += component2.storage.MassStored();
				}
				if (component3 != null)
				{
					num += component3.storage.MassStored();
				}
				if (component4 != null)
				{
					num += component4.fuelStorage.MassStored();
				}
			}
		}
		return num;
	}

	// Token: 0x060051A5 RID: 20901 RVA: 0x001D46E4 File Offset: 0x001D28E4
	public Tag GetEngineFuelTag()
	{
		RocketEngine mainEngine = this.GetMainEngine();
		if (mainEngine != null)
		{
			return mainEngine.fuelTag;
		}
		return null;
	}

	// Token: 0x060051A6 RID: 20902 RVA: 0x001D4710 File Offset: 0x001D2910
	public float GetTotalFuel(bool includeBoosters = false)
	{
		float num = 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			FuelTank component = gameObject.GetComponent<FuelTank>();
			Tag engineFuelTag = this.GetEngineFuelTag();
			if (component != null)
			{
				num += component.storage.GetAmountAvailable(engineFuelTag);
			}
			if (includeBoosters)
			{
				SolidBooster component2 = gameObject.GetComponent<SolidBooster>();
				if (component2 != null)
				{
					num += component2.fuelStorage.GetAmountAvailable(component2.fuelTag);
				}
			}
		}
		return num;
	}

	// Token: 0x060051A7 RID: 20903 RVA: 0x001D47C0 File Offset: 0x001D29C0
	public float GetTotalOxidizer(bool includeBoosters = false)
	{
		float num = 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			OxidizerTank component = gameObject.GetComponent<OxidizerTank>();
			if (component != null)
			{
				num += component.GetTotalOxidizerAvailable();
			}
			if (includeBoosters)
			{
				SolidBooster component2 = gameObject.GetComponent<SolidBooster>();
				if (component2 != null)
				{
					num += component2.fuelStorage.GetAmountAvailable(GameTags.OxyRock);
				}
			}
		}
		return num;
	}

	// Token: 0x060051A8 RID: 20904 RVA: 0x001D4860 File Offset: 0x001D2A60
	public float GetAverageOxidizerEfficiency()
	{
		Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
		dictionary[SimHashes.LiquidOxygen.CreateTag()] = 0f;
		dictionary[SimHashes.OxyRock.CreateTag()] = 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			OxidizerTank component = gameObject.GetComponent<OxidizerTank>();
			if (component != null)
			{
				foreach (KeyValuePair<Tag, float> keyValuePair in component.GetOxidizersAvailable())
				{
					if (dictionary.ContainsKey(keyValuePair.Key))
					{
						Dictionary<Tag, float> dictionary2 = dictionary;
						Tag key = keyValuePair.Key;
						dictionary2[key] += keyValuePair.Value;
					}
				}
			}
		}
		float num = 0f;
		float num2 = 0f;
		foreach (KeyValuePair<Tag, float> keyValuePair2 in dictionary)
		{
			num += keyValuePair2.Value * RocketStats.oxidizerEfficiencies[keyValuePair2.Key];
			num2 += keyValuePair2.Value;
		}
		if (num2 == 0f)
		{
			return 0f;
		}
		return num / num2 * 100f;
	}

	// Token: 0x060051A9 RID: 20905 RVA: 0x001D49F0 File Offset: 0x001D2BF0
	public float GetTotalThrust()
	{
		float totalFuel = this.GetTotalFuel(false);
		float totalOxidizer = this.GetTotalOxidizer(false);
		float averageOxidizerEfficiency = this.GetAverageOxidizerEfficiency();
		RocketEngine mainEngine = this.GetMainEngine();
		if (mainEngine == null)
		{
			return 0f;
		}
		return (mainEngine.requireOxidizer ? (Mathf.Min(totalFuel, totalOxidizer) * (mainEngine.efficiency * (averageOxidizerEfficiency / 100f))) : (totalFuel * mainEngine.efficiency)) + this.GetBoosterThrust();
	}

	// Token: 0x060051AA RID: 20906 RVA: 0x001D4A5C File Offset: 0x001D2C5C
	public float GetBoosterThrust()
	{
		float num = 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			SolidBooster component = gameObject.GetComponent<SolidBooster>();
			if (component != null)
			{
				float amountAvailable = component.fuelStorage.GetAmountAvailable(ElementLoader.FindElementByHash(SimHashes.OxyRock).tag);
				float amountAvailable2 = component.fuelStorage.GetAmountAvailable(ElementLoader.FindElementByHash(SimHashes.Iron).tag);
				num += component.efficiency * Mathf.Min(amountAvailable, amountAvailable2);
			}
		}
		return num;
	}

	// Token: 0x060051AB RID: 20907 RVA: 0x001D4B10 File Offset: 0x001D2D10
	public float GetEngineEfficiency()
	{
		RocketEngine mainEngine = this.GetMainEngine();
		if (mainEngine != null)
		{
			return mainEngine.efficiency;
		}
		return 0f;
	}

	// Token: 0x060051AC RID: 20908 RVA: 0x001D4B3C File Offset: 0x001D2D3C
	public RocketEngine GetMainEngine()
	{
		RocketEngine rocketEngine = null;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			rocketEngine = gameObject.GetComponent<RocketEngine>();
			if (rocketEngine != null && rocketEngine.mainEngine)
			{
				break;
			}
		}
		return rocketEngine;
	}

	// Token: 0x060051AD RID: 20909 RVA: 0x001D4BB0 File Offset: 0x001D2DB0
	public float GetTotalOxidizableFuel()
	{
		float totalFuel = this.GetTotalFuel(false);
		float totalOxidizer = this.GetTotalOxidizer(false);
		return Mathf.Min(totalFuel, totalOxidizer);
	}

	// Token: 0x0400360A RID: 13834
	private CommandModule commandModule;

	// Token: 0x0400360B RID: 13835
	public static Dictionary<Tag, float> oxidizerEfficiencies = new Dictionary<Tag, float>
	{
		{
			SimHashes.OxyRock.CreateTag(),
			ROCKETRY.OXIDIZER_EFFICIENCY.LOW
		},
		{
			SimHashes.LiquidOxygen.CreateTag(),
			ROCKETRY.OXIDIZER_EFFICIENCY.HIGH
		}
	};
}
