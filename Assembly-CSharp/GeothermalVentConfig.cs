using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001FD RID: 509
public class GeothermalVentConfig : IEntityConfig
{
	// Token: 0x06000A6C RID: 2668 RVA: 0x0003E165 File Offset: 0x0003C365
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x0003E16C File Offset: 0x0003C36C
	public virtual GameObject CreatePrefab()
	{
		string id = "GeothermalVentEntity";
		string name = STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.EFFECT + "\n\n" + STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.PENALTY.TIER4;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_geospout_kanim"), "off", Grid.SceneLayer.BuildingBack, 3, 4, tier, tier2, SimHashes.Unobtanium, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddComponent<GeothermalVent>();
		gameObject.AddComponent<GeothermalPlantComponent>();
		gameObject.AddComponent<Operational>();
		gameObject.AddComponent<UserNameable>();
		Storage storage = gameObject.AddComponent<Storage>();
		storage.showCapacityAsMainStatus = false;
		storage.showCapacityStatusItem = false;
		storage.showDescriptor = false;
		return gameObject;
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x0003E240 File Offset: 0x0003C440
	public void OnPrefabInit(GameObject inst)
	{
		LogicPorts logicPorts = inst.AddOrGet<LogicPorts>();
		logicPorts.inputPortInfo = new LogicPorts.Port[0];
		logicPorts.outputPortInfo = new LogicPorts.Port[]
		{
			LogicPorts.Port.OutputPort("GEOTHERMAL_VENT_STATUS_PORT", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.LOGIC_PORT_INACTIVE, false, false)
		};
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x0003E2A7 File Offset: 0x0003C4A7
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006D5 RID: 1749
	public const string ID = "GeothermalVentEntity";

	// Token: 0x040006D6 RID: 1750
	public const string OUTPUT_LOGIC_PORT_ID = "GEOTHERMAL_VENT_STATUS_PORT";

	// Token: 0x040006D7 RID: 1751
	private const string ANIM_FILE = "gravitas_geospout_kanim";

	// Token: 0x040006D8 RID: 1752
	public const string OFFLINE_ANIM = "off";

	// Token: 0x040006D9 RID: 1753
	public const string QUEST_ENTOMBED_ANIM = "pooped";

	// Token: 0x040006DA RID: 1754
	public const string IDLE_ANIM = "on";

	// Token: 0x040006DB RID: 1755
	public const string OBSTRUCTED_ANIM = "over_pressure";

	// Token: 0x040006DC RID: 1756
	public const int EMISSION_RANGE = 1;

	// Token: 0x040006DD RID: 1757
	public const float EMISSION_INTERVAL_SEC = 0.2f;

	// Token: 0x040006DE RID: 1758
	public const float EMISSION_MAX_PRESSURE_KG = 120f;

	// Token: 0x040006DF RID: 1759
	public const float EMISSION_MAX_RATE_PER_TICK = 3f;

	// Token: 0x040006E0 RID: 1760
	public static string TOGGLE_ANIMATION = "working_loop";

	// Token: 0x040006E1 RID: 1761
	public static HashedString TOGGLE_ANIM_OVERRIDE = "anim_interacts_geospout_kanim";

	// Token: 0x040006E2 RID: 1762
	public const float TOGGLE_CHORE_DURATION_SECONDS = 10f;

	// Token: 0x040006E3 RID: 1763
	public static MathUtil.MinMax INITIAL_DEBRIS_VELOCIOTY = new MathUtil.MinMax(1f, 5f);

	// Token: 0x040006E4 RID: 1764
	public static MathUtil.MinMax INITIAL_DEBRIS_ANGLE = new MathUtil.MinMax(200f, 340f);

	// Token: 0x040006E5 RID: 1765
	public static MathUtil.MinMax DEBRIS_MASS_KG = new MathUtil.MinMax(30f, 34f);

	// Token: 0x040006E6 RID: 1766
	public const string BAROMETER_ANIM = "meter";

	// Token: 0x040006E7 RID: 1767
	public const string BAROMETER_TARGET = "meter_target";

	// Token: 0x040006E8 RID: 1768
	public static string[] BAROMETER_SYMBOLS = new string[]
	{
		"meter_target"
	};

	// Token: 0x040006E9 RID: 1769
	public const string CONNECTED_ANIM = "meter_connected";

	// Token: 0x040006EA RID: 1770
	public const string CONNECTED_TARGET = "meter_connected_target";

	// Token: 0x040006EB RID: 1771
	public static string[] CONNECTED_SYMBOLS = new string[]
	{
		"meter_connected_target"
	};

	// Token: 0x040006EC RID: 1772
	public const float CONNECTED_PROGRESS = 1f;

	// Token: 0x040006ED RID: 1773
	public const float DISCONNECTED_PROGRESS = 0f;
}
