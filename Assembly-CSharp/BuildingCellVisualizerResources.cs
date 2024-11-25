using System;
using UnityEngine;

// Token: 0x02000B7B RID: 2939
public class BuildingCellVisualizerResources : ScriptableObject
{
	// Token: 0x1700069B RID: 1691
	// (get) Token: 0x0600583A RID: 22586 RVA: 0x001FDED8 File Offset: 0x001FC0D8
	public string heatSourceAnimFile
	{
		get
		{
			return "heat_fx_kanim";
		}
	}

	// Token: 0x1700069C RID: 1692
	// (get) Token: 0x0600583B RID: 22587 RVA: 0x001FDEDF File Offset: 0x001FC0DF
	public string heatAnimName
	{
		get
		{
			return "heatfx_a";
		}
	}

	// Token: 0x1700069D RID: 1693
	// (get) Token: 0x0600583C RID: 22588 RVA: 0x001FDEE6 File Offset: 0x001FC0E6
	public string heatSinkAnimFile
	{
		get
		{
			return "heat_fx_kanim";
		}
	}

	// Token: 0x1700069E RID: 1694
	// (get) Token: 0x0600583D RID: 22589 RVA: 0x001FDEED File Offset: 0x001FC0ED
	public string heatSinkAnimName
	{
		get
		{
			return "heatfx_b";
		}
	}

	// Token: 0x1700069F RID: 1695
	// (get) Token: 0x0600583E RID: 22590 RVA: 0x001FDEF4 File Offset: 0x001FC0F4
	// (set) Token: 0x0600583F RID: 22591 RVA: 0x001FDEFC File Offset: 0x001FC0FC
	public Material backgroundMaterial { get; set; }

	// Token: 0x170006A0 RID: 1696
	// (get) Token: 0x06005840 RID: 22592 RVA: 0x001FDF05 File Offset: 0x001FC105
	// (set) Token: 0x06005841 RID: 22593 RVA: 0x001FDF0D File Offset: 0x001FC10D
	public Material iconBackgroundMaterial { get; set; }

	// Token: 0x170006A1 RID: 1697
	// (get) Token: 0x06005842 RID: 22594 RVA: 0x001FDF16 File Offset: 0x001FC116
	// (set) Token: 0x06005843 RID: 22595 RVA: 0x001FDF1E File Offset: 0x001FC11E
	public Material powerInputMaterial { get; set; }

	// Token: 0x170006A2 RID: 1698
	// (get) Token: 0x06005844 RID: 22596 RVA: 0x001FDF27 File Offset: 0x001FC127
	// (set) Token: 0x06005845 RID: 22597 RVA: 0x001FDF2F File Offset: 0x001FC12F
	public Material powerOutputMaterial { get; set; }

	// Token: 0x170006A3 RID: 1699
	// (get) Token: 0x06005846 RID: 22598 RVA: 0x001FDF38 File Offset: 0x001FC138
	// (set) Token: 0x06005847 RID: 22599 RVA: 0x001FDF40 File Offset: 0x001FC140
	public Material liquidInputMaterial { get; set; }

	// Token: 0x170006A4 RID: 1700
	// (get) Token: 0x06005848 RID: 22600 RVA: 0x001FDF49 File Offset: 0x001FC149
	// (set) Token: 0x06005849 RID: 22601 RVA: 0x001FDF51 File Offset: 0x001FC151
	public Material liquidOutputMaterial { get; set; }

	// Token: 0x170006A5 RID: 1701
	// (get) Token: 0x0600584A RID: 22602 RVA: 0x001FDF5A File Offset: 0x001FC15A
	// (set) Token: 0x0600584B RID: 22603 RVA: 0x001FDF62 File Offset: 0x001FC162
	public Material gasInputMaterial { get; set; }

	// Token: 0x170006A6 RID: 1702
	// (get) Token: 0x0600584C RID: 22604 RVA: 0x001FDF6B File Offset: 0x001FC16B
	// (set) Token: 0x0600584D RID: 22605 RVA: 0x001FDF73 File Offset: 0x001FC173
	public Material gasOutputMaterial { get; set; }

	// Token: 0x170006A7 RID: 1703
	// (get) Token: 0x0600584E RID: 22606 RVA: 0x001FDF7C File Offset: 0x001FC17C
	// (set) Token: 0x0600584F RID: 22607 RVA: 0x001FDF84 File Offset: 0x001FC184
	public Material highEnergyParticleInputMaterial { get; set; }

	// Token: 0x170006A8 RID: 1704
	// (get) Token: 0x06005850 RID: 22608 RVA: 0x001FDF8D File Offset: 0x001FC18D
	// (set) Token: 0x06005851 RID: 22609 RVA: 0x001FDF95 File Offset: 0x001FC195
	public Material highEnergyParticleOutputMaterial { get; set; }

	// Token: 0x170006A9 RID: 1705
	// (get) Token: 0x06005852 RID: 22610 RVA: 0x001FDF9E File Offset: 0x001FC19E
	// (set) Token: 0x06005853 RID: 22611 RVA: 0x001FDFA6 File Offset: 0x001FC1A6
	public Mesh backgroundMesh { get; set; }

	// Token: 0x170006AA RID: 1706
	// (get) Token: 0x06005854 RID: 22612 RVA: 0x001FDFAF File Offset: 0x001FC1AF
	// (set) Token: 0x06005855 RID: 22613 RVA: 0x001FDFB7 File Offset: 0x001FC1B7
	public Mesh iconMesh { get; set; }

	// Token: 0x170006AB RID: 1707
	// (get) Token: 0x06005856 RID: 22614 RVA: 0x001FDFC0 File Offset: 0x001FC1C0
	// (set) Token: 0x06005857 RID: 22615 RVA: 0x001FDFC8 File Offset: 0x001FC1C8
	public int backgroundLayer { get; set; }

	// Token: 0x170006AC RID: 1708
	// (get) Token: 0x06005858 RID: 22616 RVA: 0x001FDFD1 File Offset: 0x001FC1D1
	// (set) Token: 0x06005859 RID: 22617 RVA: 0x001FDFD9 File Offset: 0x001FC1D9
	public int iconLayer { get; set; }

	// Token: 0x0600585A RID: 22618 RVA: 0x001FDFE2 File Offset: 0x001FC1E2
	public static void DestroyInstance()
	{
		BuildingCellVisualizerResources._Instance = null;
	}

	// Token: 0x0600585B RID: 22619 RVA: 0x001FDFEA File Offset: 0x001FC1EA
	public static BuildingCellVisualizerResources Instance()
	{
		if (BuildingCellVisualizerResources._Instance == null)
		{
			BuildingCellVisualizerResources._Instance = Resources.Load<BuildingCellVisualizerResources>("BuildingCellVisualizerResources");
			BuildingCellVisualizerResources._Instance.Initialize();
		}
		return BuildingCellVisualizerResources._Instance;
	}

	// Token: 0x0600585C RID: 22620 RVA: 0x001FE018 File Offset: 0x001FC218
	private void Initialize()
	{
		Shader shader = Shader.Find("Klei/BuildingCell");
		this.backgroundMaterial = new Material(shader);
		this.backgroundMaterial.mainTexture = GlobalResources.Instance().WhiteTexture;
		this.iconBackgroundMaterial = new Material(shader);
		this.iconBackgroundMaterial.mainTexture = GlobalResources.Instance().WhiteTexture;
		this.powerInputMaterial = new Material(shader);
		this.powerOutputMaterial = new Material(shader);
		this.liquidInputMaterial = new Material(shader);
		this.liquidOutputMaterial = new Material(shader);
		this.gasInputMaterial = new Material(shader);
		this.gasOutputMaterial = new Material(shader);
		this.highEnergyParticleInputMaterial = new Material(shader);
		this.highEnergyParticleOutputMaterial = new Material(shader);
		this.backgroundMesh = this.CreateMesh("BuildingCellVisualizer", Vector2.zero, 0.5f);
		float num = 0.5f;
		this.iconMesh = this.CreateMesh("BuildingCellVisualizerIcon", Vector2.zero, num * 0.5f);
		this.backgroundLayer = LayerMask.NameToLayer("Default");
		this.iconLayer = LayerMask.NameToLayer("Place");
	}

	// Token: 0x0600585D RID: 22621 RVA: 0x001FE130 File Offset: 0x001FC330
	private Mesh CreateMesh(string name, Vector2 base_offset, float half_size)
	{
		Mesh mesh = new Mesh();
		mesh.name = name;
		mesh.vertices = new Vector3[]
		{
			new Vector3(-half_size + base_offset.x, -half_size + base_offset.y, 0f),
			new Vector3(half_size + base_offset.x, -half_size + base_offset.y, 0f),
			new Vector3(-half_size + base_offset.x, half_size + base_offset.y, 0f),
			new Vector3(half_size + base_offset.x, half_size + base_offset.y, 0f)
		};
		mesh.uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		mesh.triangles = new int[]
		{
			0,
			1,
			2,
			2,
			1,
			3
		};
		mesh.RecalculateBounds();
		return mesh;
	}

	// Token: 0x040039D7 RID: 14807
	[Header("Electricity")]
	public Color electricityInputColor;

	// Token: 0x040039D8 RID: 14808
	public Color electricityOutputColor;

	// Token: 0x040039D9 RID: 14809
	public Sprite electricityInputIcon;

	// Token: 0x040039DA RID: 14810
	public Sprite electricityOutputIcon;

	// Token: 0x040039DB RID: 14811
	public Sprite electricityConnectedIcon;

	// Token: 0x040039DC RID: 14812
	public Sprite electricityBridgeIcon;

	// Token: 0x040039DD RID: 14813
	public Sprite electricityBridgeConnectedIcon;

	// Token: 0x040039DE RID: 14814
	public Sprite electricityArrowIcon;

	// Token: 0x040039DF RID: 14815
	public Sprite switchIcon;

	// Token: 0x040039E0 RID: 14816
	public Color32 switchColor;

	// Token: 0x040039E1 RID: 14817
	public Color32 switchOffColor = Color.red;

	// Token: 0x040039E2 RID: 14818
	[Header("Gas")]
	public Sprite gasInputIcon;

	// Token: 0x040039E3 RID: 14819
	public Sprite gasOutputIcon;

	// Token: 0x040039E4 RID: 14820
	public BuildingCellVisualizerResources.IOColours gasIOColours;

	// Token: 0x040039E5 RID: 14821
	[Header("Liquid")]
	public Sprite liquidInputIcon;

	// Token: 0x040039E6 RID: 14822
	public Sprite liquidOutputIcon;

	// Token: 0x040039E7 RID: 14823
	public BuildingCellVisualizerResources.IOColours liquidIOColours;

	// Token: 0x040039E8 RID: 14824
	[Header("High Energy Particle")]
	public Sprite highEnergyParticleInputIcon;

	// Token: 0x040039E9 RID: 14825
	public Sprite[] highEnergyParticleOutputIcons;

	// Token: 0x040039EA RID: 14826
	public Color highEnergyParticleInputColour;

	// Token: 0x040039EB RID: 14827
	public Color highEnergyParticleOutputColour;

	// Token: 0x040039EC RID: 14828
	[Header("Heat Sources and Sinks")]
	public Sprite heatSourceIcon;

	// Token: 0x040039ED RID: 14829
	public Sprite heatSinkIcon;

	// Token: 0x040039EE RID: 14830
	[Header("Alternate IO Colours")]
	public BuildingCellVisualizerResources.IOColours alternateIOColours;

	// Token: 0x040039FD RID: 14845
	private static BuildingCellVisualizerResources _Instance;

	// Token: 0x02001BDD RID: 7133
	[Serializable]
	public struct ConnectedDisconnectedColours
	{
		// Token: 0x040080EF RID: 33007
		public Color32 connected;

		// Token: 0x040080F0 RID: 33008
		public Color32 disconnected;
	}

	// Token: 0x02001BDE RID: 7134
	[Serializable]
	public struct IOColours
	{
		// Token: 0x040080F1 RID: 33009
		public BuildingCellVisualizerResources.ConnectedDisconnectedColours input;

		// Token: 0x040080F2 RID: 33010
		public BuildingCellVisualizerResources.ConnectedDisconnectedColours output;
	}
}
