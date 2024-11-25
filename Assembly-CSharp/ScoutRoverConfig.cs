using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200013C RID: 316
public class ScoutRoverConfig : IEntityConfig
{
	// Token: 0x06000624 RID: 1572 RVA: 0x0002A704 File Offset: 0x00028904
	public GameObject CreatePrefab()
	{
		return BaseRoverConfig.BaseRover("ScoutRover", STRINGS.ROBOTS.MODELS.SCOUT.NAME, GameTags.Robots.Models.ScoutRover, STRINGS.ROBOTS.MODELS.SCOUT.DESC, "scout_bot_kanim", 100f, 1f, 2f, TUNING.ROBOTS.SCOUTBOT.CARRY_CAPACITY, TUNING.ROBOTS.SCOUTBOT.DIGGING, TUNING.ROBOTS.SCOUTBOT.CONSTRUCTION, TUNING.ROBOTS.SCOUTBOT.ATHLETICS, TUNING.ROBOTS.SCOUTBOT.HIT_POINTS, TUNING.ROBOTS.SCOUTBOT.BATTERY_CAPACITY, TUNING.ROBOTS.SCOUTBOT.BATTERY_DEPLETION_RATE, Db.Get().Amounts.InternalChemicalBattery, false);
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x0002A77B File Offset: 0x0002897B
	public void OnPrefabInit(GameObject inst)
	{
		BaseRoverConfig.OnPrefabInit(inst, Db.Get().Amounts.InternalChemicalBattery);
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x0002A794 File Offset: 0x00028994
	public void OnSpawn(GameObject inst)
	{
		BaseRoverConfig.OnSpawn(inst);
		Effects effects = inst.GetComponent<Effects>();
		if (inst.transform.parent == null)
		{
			if (effects.HasEffect("ScoutBotCharging"))
			{
				effects.Remove("ScoutBotCharging");
			}
		}
		else if (!effects.HasEffect("ScoutBotCharging"))
		{
			effects.Add("ScoutBotCharging", false);
		}
		inst.Subscribe(856640610, delegate(object data)
		{
			if (inst.transform.parent == null)
			{
				if (effects.HasEffect("ScoutBotCharging"))
				{
					effects.Remove("ScoutBotCharging");
					return;
				}
			}
			else if (!effects.HasEffect("ScoutBotCharging"))
			{
				effects.Add("ScoutBotCharging", false);
			}
		});
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x0002A847 File Offset: 0x00028A47
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0400044F RID: 1103
	public const string ID = "ScoutRover";

	// Token: 0x04000450 RID: 1104
	public const float MASS = 100f;

	// Token: 0x04000451 RID: 1105
	private const float WIDTH = 1f;

	// Token: 0x04000452 RID: 1106
	private const float HEIGHT = 2f;

	// Token: 0x04000453 RID: 1107
	public const int MAXIMUM_TECH_CONSTRUCTION_TIER = 2;
}
