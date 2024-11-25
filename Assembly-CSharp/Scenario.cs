using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using ProcGenGame;
using UnityEngine;

// Token: 0x02000A7A RID: 2682
[AddComponentMenu("KMonoBehaviour/scripts/Scenario")]
public class Scenario : KMonoBehaviour
{
	// Token: 0x170005AC RID: 1452
	// (get) Token: 0x06004E5F RID: 20063 RVA: 0x001C321F File Offset: 0x001C141F
	// (set) Token: 0x06004E60 RID: 20064 RVA: 0x001C3227 File Offset: 0x001C1427
	public bool[] ReplaceElementMask { get; set; }

	// Token: 0x06004E61 RID: 20065 RVA: 0x001C3230 File Offset: 0x001C1430
	public static void DestroyInstance()
	{
		Scenario.Instance = null;
	}

	// Token: 0x06004E62 RID: 20066 RVA: 0x001C3238 File Offset: 0x001C1438
	protected override void OnPrefabInit()
	{
		Scenario.Instance = this;
		SaveLoader instance = SaveLoader.Instance;
		instance.OnWorldGenComplete = (Action<Cluster>)Delegate.Combine(instance.OnWorldGenComplete, new Action<Cluster>(this.OnWorldGenComplete));
	}

	// Token: 0x06004E63 RID: 20067 RVA: 0x001C3266 File Offset: 0x001C1466
	private void OnWorldGenComplete(Cluster clusterLayout)
	{
		this.Init();
	}

	// Token: 0x06004E64 RID: 20068 RVA: 0x001C3270 File Offset: 0x001C1470
	private void Init()
	{
		this.Bot = Grid.HeightInCells / 4;
		this.Left = 150;
		this.RootCell = Grid.OffsetCell(0, this.Left, this.Bot);
		this.ReplaceElementMask = new bool[Grid.CellCount];
	}

	// Token: 0x06004E65 RID: 20069 RVA: 0x001C32C0 File Offset: 0x001C14C0
	private void DigHole(int x, int y, int width, int height)
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x + i, y + j), SimHashes.Oxygen, CellEventLogger.Instance.Scenario, 200f, -1f, byte.MaxValue, 0, -1);
				SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x, y + j), SimHashes.Ice, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
				SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x + width, y + j), SimHashes.Ice, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
			}
			SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x + i, y - 1), SimHashes.Ice, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
			SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x + i, y + height), SimHashes.Ice, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
		}
	}

	// Token: 0x06004E66 RID: 20070 RVA: 0x001C33FF File Offset: 0x001C15FF
	private void Fill(int x, int y, SimHashes id = SimHashes.Ice)
	{
		SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x, y), id, CellEventLogger.Instance.Scenario, 10000f, -1f, byte.MaxValue, 0, -1);
	}

	// Token: 0x06004E67 RID: 20071 RVA: 0x001C3430 File Offset: 0x001C1630
	private void PlaceColumn(int x, int y, int height)
	{
		for (int i = 0; i < height; i++)
		{
			SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x, y + i), SimHashes.Ice, CellEventLogger.Instance.Scenario, 10000f, -1f, byte.MaxValue, 0, -1);
		}
	}

	// Token: 0x06004E68 RID: 20072 RVA: 0x001C3480 File Offset: 0x001C1680
	private void PlaceTileX(int left, int bot, int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			this.PlaceBuilding(left + i, bot, "Tile", SimHashes.Cuprite);
		}
	}

	// Token: 0x06004E69 RID: 20073 RVA: 0x001C34B0 File Offset: 0x001C16B0
	private void PlaceTileY(int left, int bot, int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			this.PlaceBuilding(left, bot + i, "Tile", SimHashes.Cuprite);
		}
	}

	// Token: 0x06004E6A RID: 20074 RVA: 0x001C34DE File Offset: 0x001C16DE
	private void Clear(int x, int y)
	{
		SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x, y), SimHashes.Oxygen, CellEventLogger.Instance.Scenario, 10000f, -1f, byte.MaxValue, 0, -1);
	}

	// Token: 0x06004E6B RID: 20075 RVA: 0x001C3514 File Offset: 0x001C1714
	private void PlacerLadder(int x, int y, int amount)
	{
		int num = 1;
		if (amount < 0)
		{
			amount = -amount;
			num = -1;
		}
		for (int i = 0; i < amount; i++)
		{
			this.PlaceBuilding(x, y + i * num, "Ladder", SimHashes.Cuprite);
		}
	}

	// Token: 0x06004E6C RID: 20076 RVA: 0x001C3550 File Offset: 0x001C1750
	private void PlaceBuildings(int left, int bot)
	{
		this.PlaceBuilding(++left, bot, "ManualGenerator", SimHashes.Iron);
		this.PlaceBuilding(left += 2, bot, "OxygenMachine", SimHashes.Steel);
		this.PlaceBuilding(left += 2, bot, "SpaceHeater", SimHashes.Steel);
		this.PlaceBuilding(++left, bot, "Electrolyzer", SimHashes.Steel);
		this.PlaceBuilding(++left, bot, "Smelter", SimHashes.Steel);
		this.SpawnOre(left, bot + 1, SimHashes.Ice);
	}

	// Token: 0x06004E6D RID: 20077 RVA: 0x001C35E4 File Offset: 0x001C17E4
	private IEnumerator TurnOn(GameObject go)
	{
		yield return null;
		yield return null;
		go.GetComponent<BuildingEnabledButton>().IsEnabled = true;
		yield break;
	}

	// Token: 0x06004E6E RID: 20078 RVA: 0x001C35F4 File Offset: 0x001C17F4
	private void SetupPlacerTest(Scenario.Builder b, Element element)
	{
		foreach (BuildingDef buildingDef in Assets.BuildingDefs)
		{
			if (buildingDef.Name != "Excavator")
			{
				b.Placer(buildingDef.PrefabID, element);
			}
		}
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06004E6F RID: 20079 RVA: 0x001C3670 File Offset: 0x001C1870
	private void SetupBuildingTest(Scenario.RowLayout row_layout, bool is_powered, bool break_building)
	{
		Scenario.Builder builder = null;
		int num = 0;
		foreach (BuildingDef buildingDef in Assets.BuildingDefs)
		{
			if (builder == null)
			{
				builder = row_layout.NextRow();
				num = this.Left;
				if (is_powered)
				{
					builder.Minion(null);
					builder.Minion(null);
				}
			}
			if (buildingDef.Name != "Excavator")
			{
				GameObject gameObject = builder.Building(buildingDef.PrefabID);
				if (break_building)
				{
					BuildingHP component = gameObject.GetComponent<BuildingHP>();
					if (component != null)
					{
						component.DoDamage(int.MaxValue);
					}
				}
			}
			if (builder.Left > num + 100)
			{
				builder.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
				builder = null;
			}
		}
		builder.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06004E70 RID: 20080 RVA: 0x001C375C File Offset: 0x001C195C
	private IEnumerator RunAfterNextUpdateRoutine(System.Action action)
	{
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		action();
		yield break;
	}

	// Token: 0x06004E71 RID: 20081 RVA: 0x001C376B File Offset: 0x001C196B
	private void RunAfterNextUpdate(System.Action action)
	{
		base.StartCoroutine(this.RunAfterNextUpdateRoutine(action));
	}

	// Token: 0x06004E72 RID: 20082 RVA: 0x001C377C File Offset: 0x001C197C
	public void SetupFabricatorTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Building("ManualGenerator");
		b.Ore(3, SimHashes.Cuprite);
		b.Minion(null);
		b.Building("Masonry");
		b.InAndOuts();
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06004E73 RID: 20083 RVA: 0x001C37D2 File Offset: 0x001C19D2
	public void SetupDoorTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Jump(1, 0);
		b.Building("Door");
		b.Building("ManualGenerator");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06004E74 RID: 20084 RVA: 0x001C380C File Offset: 0x001C1A0C
	public void SetupHatchTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Building("Door");
		b.Building("ManualGenerator");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06004E75 RID: 20085 RVA: 0x001C383E File Offset: 0x001C1A3E
	public void SetupPropaneGeneratorTest(Scenario.Builder b)
	{
		b.Building("PropaneGenerator");
		b.Building("OxygenMachine");
		b.FinalizeRoom(SimHashes.Propane, SimHashes.Steel);
	}

	// Token: 0x06004E76 RID: 20086 RVA: 0x001C386C File Offset: 0x001C1A6C
	public void SetupAirLockTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Jump(1, 0);
		b.Minion(null);
		b.Jump(1, 0);
		b.Building("PoweredAirlock");
		b.Building("ManualGenerator");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06004E77 RID: 20087 RVA: 0x001C38C0 File Offset: 0x001C1AC0
	public void SetupBedTest(Scenario.Builder b)
	{
		b.Minion(delegate(GameObject go)
		{
			go.GetAmounts().SetValue("Stamina", 10f);
		});
		b.Building("ManualGenerator");
		b.Minion(delegate(GameObject go)
		{
			go.GetAmounts().SetValue("Stamina", 10f);
		});
		b.Building("ComfyBed");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06004E78 RID: 20088 RVA: 0x001C3940 File Offset: 0x001C1B40
	public void SetupHexapedTest(Scenario.Builder b)
	{
		b.Fill(4, 4, SimHashes.Oxygen);
		b.Prefab("Hexaped", null);
		b.Jump(2, 0);
		b.Ore(1, SimHashes.Iron);
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06004E79 RID: 20089 RVA: 0x001C3980 File Offset: 0x001C1B80
	public void SetupElectrolyzerTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Building("ManualGenerator");
		b.Ore(3, SimHashes.Ice);
		b.Minion(null);
		b.Building("Electrolyzer");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06004E7A RID: 20090 RVA: 0x001C39D0 File Offset: 0x001C1BD0
	public void SetupOrePerformanceTest(Scenario.Builder b)
	{
		int num = 20;
		int num2 = 20;
		int left = b.Left;
		int bot = b.Bot;
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j += 2)
			{
				b.Jump(i, j);
				b.Ore(1, SimHashes.Cuprite);
				b.JumpTo(left, bot);
			}
		}
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06004E7B RID: 20091 RVA: 0x001C3A40 File Offset: 0x001C1C40
	public void SetupFeedingTest(Scenario.Builder b)
	{
		b.FillOffsets(SimHashes.IgneousRock, new int[]
		{
			1,
			0,
			3,
			0,
			3,
			1,
			5,
			0,
			5,
			1,
			5,
			2,
			7,
			0,
			7,
			1,
			7,
			2,
			9,
			0,
			9,
			1,
			11,
			0
		});
		b.PrefabOffsets("Hexaped", new int[]
		{
			0,
			0,
			2,
			0,
			4,
			0,
			7,
			3,
			9,
			2,
			11,
			1
		});
		b.OreOffsets(1, SimHashes.IronOre, new int[]
		{
			1,
			1,
			3,
			2,
			5,
			3,
			8,
			0,
			10,
			0,
			12,
			0
		});
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06004E7C RID: 20092 RVA: 0x001C3AB8 File Offset: 0x001C1CB8
	public void SetupLiquifierTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Minion(null);
		b.Ore(2, SimHashes.Ice);
		b.Building("ManualGenerator");
		b.Building("Liquifier");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06004E7D RID: 20093 RVA: 0x001C3B08 File Offset: 0x001C1D08
	public void SetupFallTest(Scenario.Builder b)
	{
		b.Jump(0, 5);
		b.Minion(null);
		b.Jump(0, -1);
		b.Building("Tile");
		b.Building("Tile");
		b.Building("Tile");
		b.Jump(-1, 1);
		b.Minion(null);
		b.Jump(2, 0);
		b.Minion(null);
		b.Jump(0, -1);
		b.Building("Tile");
		b.Jump(2, 1);
		b.Minion(null);
		b.Building("Ladder");
		b.Jump(-1, -1);
		b.Building("Tile");
		b.Jump(-1, -3);
		b.Building("Ladder");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06004E7E RID: 20094 RVA: 0x001C3BD8 File Offset: 0x001C1DD8
	public void SetupClimbTest(int left, int bot)
	{
		this.DigHole(left, bot, 13, 5);
		this.SpawnPrefab(left + 1, bot + 1, "Minion", Grid.SceneLayer.Ore);
		int num = left + 2;
		this.Clear(num++, bot - 1);
		num++;
		this.Fill(num++, bot, SimHashes.Ice);
		num++;
		this.Clear(num, bot - 1);
		this.Clear(num++, bot - 2);
		this.Fill(num++, bot, SimHashes.Ice);
		this.Clear(num, bot - 1);
		this.Clear(num++, bot - 2);
		num++;
		this.Fill(num, bot, SimHashes.Ice);
		this.Fill(num, bot + 1, SimHashes.Ice);
	}

	// Token: 0x06004E7F RID: 20095 RVA: 0x001C3C90 File Offset: 0x001C1E90
	private void SetupSuitRechargeTest(Scenario.Builder b)
	{
		b.Prefab("PressureSuit", delegate(GameObject go)
		{
			go.GetComponent<SuitTank>().Empty();
		});
		b.Building("ManualGenerator");
		b.Minion(null);
		b.Building("SuitRecharger");
		b.Minion(null);
		b.Building("GasVent");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06004E80 RID: 20096 RVA: 0x001C3D0C File Offset: 0x001C1F0C
	private void SetupSuitTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Prefab("PressureSuit", null);
		b.Jump(1, 2);
		b.Building("Tile");
		b.Jump(-1, -2);
		b.Building("Door");
		b.Building("ManualGenerator");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06004E81 RID: 20097 RVA: 0x001C3D74 File Offset: 0x001C1F74
	private void SetupTwoKelvinsOneSuitTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Jump(2, 0);
		b.Building("Door");
		b.Jump(2, 0);
		b.Minion(null);
		b.Prefab("PressureSuit", null);
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06004E82 RID: 20098 RVA: 0x001C3DC8 File Offset: 0x001C1FC8
	public void Clear()
	{
		foreach (Brain brain in Components.Brains.Items)
		{
			UnityEngine.Object.Destroy(brain.gameObject);
		}
		foreach (Pickupable pickupable in Components.Pickupables.Items)
		{
			UnityEngine.Object.Destroy(pickupable.gameObject);
		}
		foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
		{
			UnityEngine.Object.Destroy(buildingComplete.gameObject);
		}
	}

	// Token: 0x06004E83 RID: 20099 RVA: 0x001C3EB4 File Offset: 0x001C20B4
	public void SetupGameplayTest()
	{
		this.Init();
		Vector3 pos = Grid.CellToPosCCC(this.RootCell, Grid.SceneLayer.Background);
		CameraController.Instance.SnapTo(pos);
		if (this.ClearExistingScene)
		{
			this.Clear();
		}
		Scenario.RowLayout rowLayout = new Scenario.RowLayout(0, 0);
		if (this.CementMixerTest)
		{
			this.SetupCementMixerTest(rowLayout.NextRow());
		}
		if (this.RockCrusherTest)
		{
			this.SetupRockCrusherTest(rowLayout.NextRow());
		}
		if (this.PropaneGeneratorTest)
		{
			this.SetupPropaneGeneratorTest(rowLayout.NextRow());
		}
		if (this.DoorTest)
		{
			this.SetupDoorTest(rowLayout.NextRow());
		}
		if (this.HatchTest)
		{
			this.SetupHatchTest(rowLayout.NextRow());
		}
		if (this.AirLockTest)
		{
			this.SetupAirLockTest(rowLayout.NextRow());
		}
		if (this.BedTest)
		{
			this.SetupBedTest(rowLayout.NextRow());
		}
		if (this.LiquifierTest)
		{
			this.SetupLiquifierTest(rowLayout.NextRow());
		}
		if (this.SuitTest)
		{
			this.SetupSuitTest(rowLayout.NextRow());
		}
		if (this.SuitRechargeTest)
		{
			this.SetupSuitRechargeTest(rowLayout.NextRow());
		}
		if (this.TwoKelvinsOneSuitTest)
		{
			this.SetupTwoKelvinsOneSuitTest(rowLayout.NextRow());
		}
		if (this.FabricatorTest)
		{
			this.SetupFabricatorTest(rowLayout.NextRow());
		}
		if (this.ElectrolyzerTest)
		{
			this.SetupElectrolyzerTest(rowLayout.NextRow());
		}
		if (this.HexapedTest)
		{
			this.SetupHexapedTest(rowLayout.NextRow());
		}
		if (this.FallTest)
		{
			this.SetupFallTest(rowLayout.NextRow());
		}
		if (this.FeedingTest)
		{
			this.SetupFeedingTest(rowLayout.NextRow());
		}
		if (this.OrePerformanceTest)
		{
			this.SetupOrePerformanceTest(rowLayout.NextRow());
		}
		if (this.KilnTest)
		{
			this.SetupKilnTest(rowLayout.NextRow());
		}
	}

	// Token: 0x06004E84 RID: 20100 RVA: 0x001C405D File Offset: 0x001C225D
	private GameObject SpawnMinion(int x, int y)
	{
		return this.SpawnPrefab(x, y, "Minion", Grid.SceneLayer.Move);
	}

	// Token: 0x06004E85 RID: 20101 RVA: 0x001C4070 File Offset: 0x001C2270
	private void SetupLadderTest(int left, int bot)
	{
		int num = 5;
		this.DigHole(left, bot, 13, num);
		this.SpawnMinion(left + 1, bot);
		int x = left + 1;
		this.PlacerLadder(x++, bot, num);
		this.PlaceColumn(x++, bot, num);
		this.SpawnMinion(x, bot);
		this.PlacerLadder(x++, bot + 1, num - 1);
		this.PlaceColumn(x++, bot, num);
		this.SpawnMinion(x++, bot);
		this.PlacerLadder(x++, bot, num);
		this.PlaceColumn(x++, bot, num);
		this.SpawnMinion(x++, bot);
		this.PlacerLadder(x++, bot + 1, num - 1);
		this.PlaceColumn(x++, bot, num);
		this.SpawnMinion(x++, bot);
		this.PlacerLadder(x++, bot - 1, -num);
	}

	// Token: 0x06004E86 RID: 20102 RVA: 0x001C414C File Offset: 0x001C234C
	public void PlaceUtilitiesX(int left, int bot, int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			this.PlaceUtilities(left + i, bot);
		}
	}

	// Token: 0x06004E87 RID: 20103 RVA: 0x001C416F File Offset: 0x001C236F
	public void PlaceUtilities(int left, int bot)
	{
		this.PlaceBuilding(left, bot, "Wire", SimHashes.Cuprite);
		this.PlaceBuilding(left, bot, "GasConduit", SimHashes.Cuprite);
	}

	// Token: 0x06004E88 RID: 20104 RVA: 0x001C4198 File Offset: 0x001C2398
	public void SetupVisualTest()
	{
		this.Init();
		Scenario.RowLayout row_layout = new Scenario.RowLayout(this.Left, this.Bot);
		this.SetupBuildingTest(row_layout, false, false);
	}

	// Token: 0x06004E89 RID: 20105 RVA: 0x001C41C8 File Offset: 0x001C23C8
	private void SpawnMaterialTest(Scenario.Builder b)
	{
		foreach (Element element in ElementLoader.elements)
		{
			if (element.IsSolid)
			{
				b.Element = element.id;
				b.Building("Generator");
			}
		}
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06004E8A RID: 20106 RVA: 0x001C4244 File Offset: 0x001C2444
	public GameObject PlaceBuilding(int x, int y, string prefab_id, SimHashes element = SimHashes.Cuprite)
	{
		return Scenario.PlaceBuilding(this.RootCell, x, y, prefab_id, element);
	}

	// Token: 0x06004E8B RID: 20107 RVA: 0x001C4258 File Offset: 0x001C2458
	public static GameObject PlaceBuilding(int root_cell, int x, int y, string prefab_id, SimHashes element = SimHashes.Cuprite)
	{
		int cell = Grid.OffsetCell(root_cell, x, y);
		BuildingDef buildingDef = Assets.GetBuildingDef(prefab_id);
		if (buildingDef == null || buildingDef.PlacementOffsets == null)
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Missing def for",
				prefab_id
			});
		}
		Element element2 = ElementLoader.FindElementByHash(element);
		global::Debug.Assert(element2 != null, string.Concat(new string[]
		{
			"Missing primary element '",
			Enum.GetName(typeof(SimHashes), element),
			"' in '",
			prefab_id,
			"'"
		}));
		GameObject gameObject = buildingDef.Build(buildingDef.GetBuildingCell(cell), Orientation.Neutral, null, new Tag[]
		{
			element2.tag,
			ElementLoader.FindElementByHash(SimHashes.SedimentaryRock).tag
		}, 293.15f, false, -1f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.InternalTemperature = 300f;
		component.Temperature = 300f;
		return gameObject;
	}

	// Token: 0x06004E8C RID: 20108 RVA: 0x001C434C File Offset: 0x001C254C
	private void SpawnOre(int x, int y, SimHashes element = SimHashes.Cuprite)
	{
		this.RunAfterNextUpdate(delegate
		{
			Vector3 position = Grid.CellToPosCCC(Grid.OffsetCell(this.RootCell, x, y), Grid.SceneLayer.Ore);
			position.x += UnityEngine.Random.Range(-0.1f, 0.1f);
			ElementLoader.FindElementByHash(element).substance.SpawnResource(position, 4000f, 293f, byte.MaxValue, 0, false, false, false);
		});
	}

	// Token: 0x06004E8D RID: 20109 RVA: 0x001C438D File Offset: 0x001C258D
	public GameObject SpawnPrefab(int x, int y, string name, Grid.SceneLayer scene_layer = Grid.SceneLayer.Ore)
	{
		return Scenario.SpawnPrefab(this.RootCell, x, y, name, scene_layer);
	}

	// Token: 0x06004E8E RID: 20110 RVA: 0x001C43A0 File Offset: 0x001C25A0
	public void SpawnPrefabLate(int x, int y, string name, Grid.SceneLayer scene_layer = Grid.SceneLayer.Ore)
	{
		this.RunAfterNextUpdate(delegate
		{
			Scenario.SpawnPrefab(this.RootCell, x, y, name, scene_layer);
		});
	}

	// Token: 0x06004E8F RID: 20111 RVA: 0x001C43EC File Offset: 0x001C25EC
	public static GameObject SpawnPrefab(int RootCell, int x, int y, string name, Grid.SceneLayer scene_layer = Grid.SceneLayer.Ore)
	{
		int cell = Grid.OffsetCell(RootCell, x, y);
		GameObject prefab = Assets.GetPrefab(TagManager.Create(name));
		if (prefab == null)
		{
			return null;
		}
		return GameUtil.KInstantiate(prefab, Grid.CellToPosCBC(cell, scene_layer), scene_layer, null, 0);
	}

	// Token: 0x06004E90 RID: 20112 RVA: 0x001C442C File Offset: 0x001C262C
	public void SetupElementTest()
	{
		this.Init();
		PropertyTextures.FogOfWarScale = 1f;
		Vector3 pos = Grid.CellToPosCCC(this.RootCell, Grid.SceneLayer.Background);
		CameraController.Instance.SnapTo(pos);
		this.Clear();
		Scenario.Builder builder = new Scenario.RowLayout(0, 0).NextRow();
		HashSet<Element> elements = new HashSet<Element>();
		int bot = builder.Bot;
		foreach (Element element5 in (from element in ElementLoader.elements
		where element.IsSolid
		orderby element.highTempTransitionTarget
		select element).ToList<Element>())
		{
			if (element5.IsSolid)
			{
				Element element2 = element5;
				int left = builder.Left;
				bool hasTransitionUp;
				do
				{
					elements.Add(element2);
					builder.Hole(2, 3);
					builder.Fill(2, 2, element2.id);
					builder.FinalizeRoom(SimHashes.Vacuum, SimHashes.Unobtanium);
					builder = new Scenario.Builder(left, builder.Bot + 4, SimHashes.Copper);
					hasTransitionUp = element2.HasTransitionUp;
					if (hasTransitionUp)
					{
						element2 = element2.highTempTransition;
					}
				}
				while (hasTransitionUp);
				builder = new Scenario.Builder(left + 3, bot, SimHashes.Copper);
			}
		}
		foreach (Element element3 in (from element in ElementLoader.elements
		where element.IsLiquid && !elements.Contains(element)
		orderby element.highTempTransitionTarget
		select element).ToList<Element>())
		{
			int left2 = builder.Left;
			bool hasTransitionUp2;
			do
			{
				elements.Add(element3);
				builder.Hole(2, 3);
				builder.Fill(2, 2, element3.id);
				builder.FinalizeRoom(SimHashes.Vacuum, SimHashes.Unobtanium);
				builder = new Scenario.Builder(left2, builder.Bot + 4, SimHashes.Copper);
				hasTransitionUp2 = element3.HasTransitionUp;
				if (hasTransitionUp2)
				{
					element3 = element3.highTempTransition;
				}
			}
			while (hasTransitionUp2);
			builder = new Scenario.Builder(left2 + 3, bot, SimHashes.Copper);
		}
		foreach (Element element4 in (from element in ElementLoader.elements
		where element.state == Element.State.Gas && !elements.Contains(element)
		select element).ToList<Element>())
		{
			int left3 = builder.Left;
			builder.Hole(2, 3);
			builder.Fill(2, 2, element4.id);
			builder.FinalizeRoom(SimHashes.Vacuum, SimHashes.Unobtanium);
			builder = new Scenario.Builder(left3, builder.Bot + 4, SimHashes.Copper);
			builder = new Scenario.Builder(left3 + 3, bot, SimHashes.Copper);
		}
	}

	// Token: 0x06004E91 RID: 20113 RVA: 0x001C4750 File Offset: 0x001C2950
	private void InitDebugScenario()
	{
		this.Init();
		PropertyTextures.FogOfWarScale = 1f;
		Vector3 pos = Grid.CellToPosCCC(this.RootCell, Grid.SceneLayer.Background);
		CameraController.Instance.SnapTo(pos);
		this.Clear();
	}

	// Token: 0x06004E92 RID: 20114 RVA: 0x001C478C File Offset: 0x001C298C
	public void SetupTileTest()
	{
		this.InitDebugScenario();
		for (int i = 0; i < Grid.HeightInCells; i++)
		{
			for (int j = 0; j < Grid.WidthInCells; j++)
			{
				SimMessages.ReplaceElement(Grid.XYToCell(j, i), SimHashes.Oxygen, CellEventLogger.Instance.Scenario, 100f, -1f, byte.MaxValue, 0, -1);
			}
		}
		Scenario.Builder builder = new Scenario.RowLayout(0, 0).NextRow();
		for (int k = 0; k < 16; k++)
		{
			builder.Jump(0, 0);
			builder.Fill(1, 1, ((k & 1) != 0) ? SimHashes.Copper : SimHashes.Diamond);
			builder.Jump(1, 0);
			builder.Fill(1, 1, ((k & 2) != 0) ? SimHashes.Copper : SimHashes.Diamond);
			builder.Jump(-1, 1);
			builder.Fill(1, 1, ((k & 4) != 0) ? SimHashes.Copper : SimHashes.Diamond);
			builder.Jump(1, 0);
			builder.Fill(1, 1, ((k & 8) != 0) ? SimHashes.Copper : SimHashes.Diamond);
			builder.Jump(2, -1);
		}
	}

	// Token: 0x06004E93 RID: 20115 RVA: 0x001C4898 File Offset: 0x001C2A98
	public void SetupRiverTest()
	{
		this.InitDebugScenario();
		int num = Mathf.Min(64, Grid.WidthInCells);
		int num2 = Mathf.Min(64, Grid.HeightInCells);
		List<Element> list = new List<Element>();
		foreach (Element element in ElementLoader.elements)
		{
			if (element.IsLiquid)
			{
				list.Add(element);
			}
		}
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				SimHashes new_element = (i == 0) ? SimHashes.Unobtanium : SimHashes.Oxygen;
				SimMessages.ReplaceElement(Grid.XYToCell(j, i), new_element, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
			}
		}
	}

	// Token: 0x06004E94 RID: 20116 RVA: 0x001C4978 File Offset: 0x001C2B78
	public void SetupRockCrusherTest(Scenario.Builder b)
	{
		this.InitDebugScenario();
		b.Building("ManualGenerator");
		b.Minion(null);
		b.Building("Crusher");
		b.Minion(null);
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06004E95 RID: 20117 RVA: 0x001C49B8 File Offset: 0x001C2BB8
	public void SetupCementMixerTest(Scenario.Builder b)
	{
		this.InitDebugScenario();
		b.Building("Generator");
		b.Minion(null);
		b.Building("Crusher");
		b.Minion(null);
		b.Minion(null);
		b.Building("Mixer");
		b.Ore(20, SimHashes.SedimentaryRock);
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06004E96 RID: 20118 RVA: 0x001C4A24 File Offset: 0x001C2C24
	public void SetupKilnTest(Scenario.Builder b)
	{
		this.InitDebugScenario();
		b.Building("ManualGenerator");
		b.Minion(null);
		b.Building("Kiln");
		b.Minion(null);
		b.Ore(20, SimHashes.SandCement);
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x0400341F RID: 13343
	private int Bot;

	// Token: 0x04003420 RID: 13344
	private int Left;

	// Token: 0x04003421 RID: 13345
	public int RootCell;

	// Token: 0x04003423 RID: 13347
	public static Scenario Instance;

	// Token: 0x04003424 RID: 13348
	public bool PropaneGeneratorTest = true;

	// Token: 0x04003425 RID: 13349
	public bool HatchTest = true;

	// Token: 0x04003426 RID: 13350
	public bool DoorTest = true;

	// Token: 0x04003427 RID: 13351
	public bool AirLockTest = true;

	// Token: 0x04003428 RID: 13352
	public bool BedTest = true;

	// Token: 0x04003429 RID: 13353
	public bool SuitTest = true;

	// Token: 0x0400342A RID: 13354
	public bool SuitRechargeTest = true;

	// Token: 0x0400342B RID: 13355
	public bool FabricatorTest = true;

	// Token: 0x0400342C RID: 13356
	public bool ElectrolyzerTest = true;

	// Token: 0x0400342D RID: 13357
	public bool HexapedTest = true;

	// Token: 0x0400342E RID: 13358
	public bool FallTest = true;

	// Token: 0x0400342F RID: 13359
	public bool FeedingTest = true;

	// Token: 0x04003430 RID: 13360
	public bool OrePerformanceTest = true;

	// Token: 0x04003431 RID: 13361
	public bool TwoKelvinsOneSuitTest = true;

	// Token: 0x04003432 RID: 13362
	public bool LiquifierTest = true;

	// Token: 0x04003433 RID: 13363
	public bool RockCrusherTest = true;

	// Token: 0x04003434 RID: 13364
	public bool CementMixerTest = true;

	// Token: 0x04003435 RID: 13365
	public bool KilnTest = true;

	// Token: 0x04003436 RID: 13366
	public bool ClearExistingScene = true;

	// Token: 0x02001AA7 RID: 6823
	public class RowLayout
	{
		// Token: 0x0600A0B0 RID: 41136 RVA: 0x00380773 File Offset: 0x0037E973
		public RowLayout(int left, int bot)
		{
			this.Left = left;
			this.Bot = bot;
		}

		// Token: 0x0600A0B1 RID: 41137 RVA: 0x0038078C File Offset: 0x0037E98C
		public Scenario.Builder NextRow()
		{
			if (this.Builder != null)
			{
				this.Bot = this.Builder.Max.y + 1;
			}
			this.Builder = new Scenario.Builder(this.Left, this.Bot, SimHashes.Copper);
			return this.Builder;
		}

		// Token: 0x04007D34 RID: 32052
		public int Left;

		// Token: 0x04007D35 RID: 32053
		public int Bot;

		// Token: 0x04007D36 RID: 32054
		public Scenario.Builder Builder;
	}

	// Token: 0x02001AA8 RID: 6824
	public class Builder
	{
		// Token: 0x0600A0B2 RID: 41138 RVA: 0x003807DC File Offset: 0x0037E9DC
		public Builder(int left, int bot, SimHashes element = SimHashes.Copper)
		{
			this.Left = left;
			this.Bot = bot;
			this.Element = element;
			this.Scenario = Scenario.Instance;
			this.PlaceUtilities = true;
			this.Min = new Vector2I(left, bot);
			this.Max = new Vector2I(left, bot);
		}

		// Token: 0x0600A0B3 RID: 41139 RVA: 0x00380830 File Offset: 0x0037EA30
		private void UpdateMinMax(int x, int y)
		{
			this.Min.x = Math.Min(x, this.Min.x);
			this.Min.y = Math.Min(y, this.Min.y);
			this.Max.x = Math.Max(x + 1, this.Max.x);
			this.Max.y = Math.Max(y + 1, this.Max.y);
		}

		// Token: 0x0600A0B4 RID: 41140 RVA: 0x003808B4 File Offset: 0x0037EAB4
		public void Utilities(int count)
		{
			for (int i = 0; i < count; i++)
			{
				this.Scenario.PlaceUtilities(this.Left, this.Bot);
				this.Left++;
			}
		}

		// Token: 0x0600A0B5 RID: 41141 RVA: 0x003808F4 File Offset: 0x0037EAF4
		public void BuildingOffsets(string prefab_id, params int[] offsets)
		{
			int left = this.Left;
			int bot = this.Bot;
			for (int i = 0; i < offsets.Length / 2; i++)
			{
				this.Jump(offsets[i * 2], offsets[i * 2 + 1]);
				this.Building(prefab_id);
				this.JumpTo(left, bot);
			}
		}

		// Token: 0x0600A0B6 RID: 41142 RVA: 0x00380944 File Offset: 0x0037EB44
		public void Placer(string prefab_id, Element element)
		{
			BuildingDef buildingDef = Assets.GetBuildingDef(prefab_id);
			int buildingCell = buildingDef.GetBuildingCell(Grid.OffsetCell(Scenario.Instance.RootCell, this.Left, this.Bot));
			Vector3 pos = Grid.CellToPosCBC(buildingCell, Grid.SceneLayer.Building);
			this.UpdateMinMax(this.Left, this.Bot);
			this.UpdateMinMax(this.Left + buildingDef.WidthInCells - 1, this.Bot + buildingDef.HeightInCells - 1);
			this.Left += buildingDef.WidthInCells;
			this.Scenario.RunAfterNextUpdate(delegate
			{
				Assets.GetBuildingDef(prefab_id).TryPlace(null, pos, Orientation.Neutral, new Tag[]
				{
					element.tag,
					ElementLoader.FindElementByHash(SimHashes.SedimentaryRock).tag
				}, 0);
			});
		}

		// Token: 0x0600A0B7 RID: 41143 RVA: 0x00380A04 File Offset: 0x0037EC04
		public GameObject Building(string prefab_id)
		{
			GameObject result = this.Scenario.PlaceBuilding(this.Left, this.Bot, prefab_id, this.Element);
			BuildingDef buildingDef = Assets.GetBuildingDef(prefab_id);
			this.UpdateMinMax(this.Left, this.Bot);
			this.UpdateMinMax(this.Left + buildingDef.WidthInCells - 1, this.Bot + buildingDef.HeightInCells - 1);
			if (this.PlaceUtilities)
			{
				for (int i = 0; i < buildingDef.WidthInCells; i++)
				{
					this.UpdateMinMax(this.Left + i, this.Bot);
					this.Scenario.PlaceUtilities(this.Left + i, this.Bot);
				}
			}
			this.Left += buildingDef.WidthInCells;
			return result;
		}

		// Token: 0x0600A0B8 RID: 41144 RVA: 0x00380AC8 File Offset: 0x0037ECC8
		public void Minion(Action<GameObject> on_spawn = null)
		{
			this.UpdateMinMax(this.Left, this.Bot);
			int left = this.Left;
			int bot = this.Bot;
			this.Scenario.RunAfterNextUpdate(delegate
			{
				GameObject obj = this.Scenario.SpawnMinion(left, bot);
				if (on_spawn != null)
				{
					on_spawn(obj);
				}
			});
		}

		// Token: 0x0600A0B9 RID: 41145 RVA: 0x00380B2A File Offset: 0x0037ED2A
		private GameObject Hexaped()
		{
			return this.Scenario.SpawnPrefab(this.Left, this.Bot, "Hexaped", Grid.SceneLayer.Front);
		}

		// Token: 0x0600A0BA RID: 41146 RVA: 0x00380B4C File Offset: 0x0037ED4C
		public void OreOffsets(int count, SimHashes element, params int[] offsets)
		{
			int left = this.Left;
			int bot = this.Bot;
			for (int i = 0; i < offsets.Length / 2; i++)
			{
				this.Jump(offsets[i * 2], offsets[i * 2 + 1]);
				this.Ore(count, element);
				this.JumpTo(left, bot);
			}
		}

		// Token: 0x0600A0BB RID: 41147 RVA: 0x00380B9C File Offset: 0x0037ED9C
		public void Ore(int count = 1, SimHashes element = SimHashes.Cuprite)
		{
			this.UpdateMinMax(this.Left, this.Bot);
			for (int i = 0; i < count; i++)
			{
				this.Scenario.SpawnOre(this.Left, this.Bot, element);
			}
		}

		// Token: 0x0600A0BC RID: 41148 RVA: 0x00380BE0 File Offset: 0x0037EDE0
		public void PrefabOffsets(string prefab_id, params int[] offsets)
		{
			int left = this.Left;
			int bot = this.Bot;
			for (int i = 0; i < offsets.Length / 2; i++)
			{
				this.Jump(offsets[i * 2], offsets[i * 2 + 1]);
				this.Prefab(prefab_id, null);
				this.JumpTo(left, bot);
			}
		}

		// Token: 0x0600A0BD RID: 41149 RVA: 0x00380C30 File Offset: 0x0037EE30
		public void Prefab(string prefab_id, Action<GameObject> on_spawn = null)
		{
			this.UpdateMinMax(this.Left, this.Bot);
			int left = this.Left;
			int bot = this.Bot;
			this.Scenario.RunAfterNextUpdate(delegate
			{
				GameObject obj = this.Scenario.SpawnPrefab(left, bot, prefab_id, Grid.SceneLayer.Ore);
				if (on_spawn != null)
				{
					on_spawn(obj);
				}
			});
		}

		// Token: 0x0600A0BE RID: 41150 RVA: 0x00380C9C File Offset: 0x0037EE9C
		public void Wall(int height)
		{
			for (int i = 0; i < height; i++)
			{
				this.Scenario.PlaceBuilding(this.Left, this.Bot + i, "Tile", SimHashes.Cuprite);
				this.UpdateMinMax(this.Left, this.Bot + i);
				if (this.PlaceUtilities)
				{
					this.Scenario.PlaceUtilities(this.Left, this.Bot + i);
				}
			}
			this.Left++;
		}

		// Token: 0x0600A0BF RID: 41151 RVA: 0x00380D1C File Offset: 0x0037EF1C
		public void Jump(int x = 0, int y = 0)
		{
			this.Left += x;
			this.Bot += y;
		}

		// Token: 0x0600A0C0 RID: 41152 RVA: 0x00380D3A File Offset: 0x0037EF3A
		public void JumpTo(int left, int bot)
		{
			this.Left = left;
			this.Bot = bot;
		}

		// Token: 0x0600A0C1 RID: 41153 RVA: 0x00380D4C File Offset: 0x0037EF4C
		public void Hole(int width, int height)
		{
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					int num = Grid.OffsetCell(this.Scenario.RootCell, this.Left + i, this.Bot + j);
					this.UpdateMinMax(this.Left + i, this.Bot + j);
					SimMessages.ReplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.Scenario, 0f, -1f, byte.MaxValue, 0, -1);
					this.Scenario.ReplaceElementMask[num] = true;
				}
			}
		}

		// Token: 0x0600A0C2 RID: 41154 RVA: 0x00380DDC File Offset: 0x0037EFDC
		public void FillOffsets(SimHashes element, params int[] offsets)
		{
			int left = this.Left;
			int bot = this.Bot;
			for (int i = 0; i < offsets.Length / 2; i++)
			{
				this.Jump(offsets[i * 2], offsets[i * 2 + 1]);
				this.Fill(1, 1, element);
				this.JumpTo(left, bot);
			}
		}

		// Token: 0x0600A0C3 RID: 41155 RVA: 0x00380E2C File Offset: 0x0037F02C
		public void Fill(int width, int height, SimHashes element)
		{
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					int num = Grid.OffsetCell(this.Scenario.RootCell, this.Left + i, this.Bot + j);
					this.UpdateMinMax(this.Left + i, this.Bot + j);
					SimMessages.ReplaceElement(num, element, CellEventLogger.Instance.Scenario, 5000f, -1f, byte.MaxValue, 0, -1);
					this.Scenario.ReplaceElementMask[num] = true;
				}
			}
		}

		// Token: 0x0600A0C4 RID: 41156 RVA: 0x00380EB8 File Offset: 0x0037F0B8
		public void InAndOuts()
		{
			this.Wall(3);
			this.Building("GasVent");
			this.Hole(3, 3);
			this.Utilities(2);
			this.Wall(3);
			this.Building("LiquidVent");
			this.Hole(3, 3);
			this.Utilities(2);
			this.Wall(3);
			this.Fill(3, 3, SimHashes.Water);
			this.Utilities(2);
			GameObject pump = this.Building("Pump");
			this.Scenario.RunAfterNextUpdate(delegate
			{
				pump.GetComponent<BuildingEnabledButton>().IsEnabled = true;
			});
		}

		// Token: 0x0600A0C5 RID: 41157 RVA: 0x00380F54 File Offset: 0x0037F154
		public Scenario.Builder FinalizeRoom(SimHashes element = SimHashes.Oxygen, SimHashes tileElement = SimHashes.Steel)
		{
			for (int i = this.Min.x - 1; i <= this.Max.x; i++)
			{
				if (i == this.Min.x - 1 || i == this.Max.x)
				{
					for (int j = this.Min.y - 1; j <= this.Max.y; j++)
					{
						this.Scenario.PlaceBuilding(i, j, "Tile", tileElement);
					}
				}
				else
				{
					int num = 500;
					if (element == SimHashes.Void)
					{
						num = 0;
					}
					for (int k = this.Min.y; k < this.Max.y; k++)
					{
						int num2 = Grid.OffsetCell(this.Scenario.RootCell, i, k);
						if (!this.Scenario.ReplaceElementMask[num2])
						{
							SimMessages.ReplaceElement(num2, element, CellEventLogger.Instance.Scenario, (float)num, -1f, byte.MaxValue, 0, -1);
						}
					}
				}
				this.Scenario.PlaceBuilding(i, this.Min.y - 1, "Tile", tileElement);
				this.Scenario.PlaceBuilding(i, this.Max.y, "Tile", tileElement);
			}
			return new Scenario.Builder(this.Max.x + 1, this.Min.y, SimHashes.Copper);
		}

		// Token: 0x04007D37 RID: 32055
		public bool PlaceUtilities;

		// Token: 0x04007D38 RID: 32056
		public int Left;

		// Token: 0x04007D39 RID: 32057
		public int Bot;

		// Token: 0x04007D3A RID: 32058
		public Vector2I Min;

		// Token: 0x04007D3B RID: 32059
		public Vector2I Max;

		// Token: 0x04007D3C RID: 32060
		public SimHashes Element;

		// Token: 0x04007D3D RID: 32061
		private Scenario Scenario;
	}
}
