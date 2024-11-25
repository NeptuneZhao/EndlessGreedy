using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000125 RID: 293
public class MorbRoverConfig : IEntityConfig
{
	// Token: 0x0600059D RID: 1437 RVA: 0x000289FC File Offset: 0x00026BFC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseRoverConfig.BaseRover("MorbRover", STRINGS.ROBOTS.MODELS.MORB.NAME, GameTags.Robots.Models.MorbRover, STRINGS.ROBOTS.MODELS.MORB.DESC, "morbRover_kanim", 300f, 1f, 2f, TUNING.ROBOTS.MORBBOT.CARRY_CAPACITY, 1f, 1f, 3f, TUNING.ROBOTS.MORBBOT.HIT_POINTS, 180000f, 30f, Db.Get().Amounts.InternalBioBattery, false);
		gameObject.GetComponent<PrimaryElement>().SetElement(SimHashes.Steel, false);
		gameObject.GetComponent<Deconstructable>().customWorkTime = 10f;
		return gameObject;
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x00028A94 File Offset: 0x00026C94
	public void OnPrefabInit(GameObject inst)
	{
		BaseRoverConfig.OnPrefabInit(inst, Db.Get().Amounts.InternalBioBattery);
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x00028AAB File Offset: 0x00026CAB
	public void OnSpawn(GameObject inst)
	{
		BaseRoverConfig.OnSpawn(inst);
		inst.Subscribe(1623392196, new Action<object>(this.TriggerDeconstructChoreOnDeath));
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x00028ACC File Offset: 0x00026CCC
	public void TriggerDeconstructChoreOnDeath(object obj)
	{
		if (obj != null)
		{
			Deconstructable component = ((GameObject)obj).GetComponent<Deconstructable>();
			if (!component.IsMarkedForDeconstruction())
			{
				component.QueueDeconstruction(false);
			}
		}
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x00028AF7 File Offset: 0x00026CF7
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x040003E8 RID: 1000
	public const string ID = "MorbRover";

	// Token: 0x040003E9 RID: 1001
	public const SimHashes MATERIAL = SimHashes.Steel;

	// Token: 0x040003EA RID: 1002
	public const float MASS = 300f;

	// Token: 0x040003EB RID: 1003
	private const float WIDTH = 1f;

	// Token: 0x040003EC RID: 1004
	private const float HEIGHT = 2f;
}
