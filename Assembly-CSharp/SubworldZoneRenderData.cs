using System;
using Delaunay.Geo;
using Klei;
using ProcGen;
using UnityEngine;

// Token: 0x02000A45 RID: 2629
[AddComponentMenu("KMonoBehaviour/scripts/SubworldZoneRenderData")]
public class SubworldZoneRenderData : KMonoBehaviour
{
	// Token: 0x06004C1C RID: 19484 RVA: 0x001B23A4 File Offset: 0x001B05A4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
		this.GenerateTexture();
		this.OnActiveWorldChanged();
		Game.Instance.Subscribe(1983128072, delegate(object worlds)
		{
			this.OnActiveWorldChanged();
		});
	}

	// Token: 0x06004C1D RID: 19485 RVA: 0x001B23F0 File Offset: 0x001B05F0
	public void OnActiveWorldChanged()
	{
		byte[] rawTextureData = this.colourTex.GetRawTextureData();
		byte[] rawTextureData2 = this.indexTex.GetRawTextureData();
		WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
		Vector2 zero = Vector2.zero;
		for (int i = 0; i < clusterDetailSave.overworldCells.Count; i++)
		{
			WorldDetailSave.OverworldCell overworldCell = clusterDetailSave.overworldCells[i];
			Polygon poly = overworldCell.poly;
			zero.y = (float)((int)Mathf.Floor(poly.bounds.yMin));
			while (zero.y < Mathf.Ceil(poly.bounds.yMax))
			{
				zero.x = (float)((int)Mathf.Floor(poly.bounds.xMin));
				while (zero.x < Mathf.Ceil(poly.bounds.xMax))
				{
					if (poly.Contains(zero))
					{
						int num = Grid.XYToCell((int)zero.x, (int)zero.y);
						if (Grid.IsValidCell(num))
						{
							if (Grid.IsActiveWorld(num))
							{
								rawTextureData2[num] = ((overworldCell.zoneType == SubWorld.ZoneType.Space) ? byte.MaxValue : ((byte)this.zoneTextureArrayIndices[(int)overworldCell.zoneType]));
								Color32 color = this.zoneColours[(int)overworldCell.zoneType];
								rawTextureData[num * 3] = color.r;
								rawTextureData[num * 3 + 1] = color.g;
								rawTextureData[num * 3 + 2] = color.b;
							}
							else
							{
								rawTextureData2[num] = byte.MaxValue;
								Color32 color2 = this.zoneColours[7];
								rawTextureData[num * 3] = color2.r;
								rawTextureData[num * 3 + 1] = color2.g;
								rawTextureData[num * 3 + 2] = color2.b;
							}
						}
					}
					zero.x += 1f;
				}
				zero.y += 1f;
			}
		}
		this.colourTex.LoadRawTextureData(rawTextureData);
		this.indexTex.LoadRawTextureData(rawTextureData2);
		this.colourTex.Apply();
		this.indexTex.Apply();
		this.OnShadersReloaded();
	}

	// Token: 0x06004C1E RID: 19486 RVA: 0x001B2618 File Offset: 0x001B0818
	public void GenerateTexture()
	{
		byte[] array = new byte[Grid.WidthInCells * Grid.HeightInCells];
		byte[] array2 = new byte[Grid.WidthInCells * Grid.HeightInCells * 3];
		this.worldZoneTypes = new SubWorld.ZoneType[Grid.CellCount];
		this.colourTex = new Texture2D(Grid.WidthInCells, Grid.HeightInCells, TextureFormat.RGB24, false);
		this.colourTex.name = "SubworldRegionColourData";
		this.colourTex.filterMode = FilterMode.Bilinear;
		this.colourTex.wrapMode = TextureWrapMode.Clamp;
		this.colourTex.anisoLevel = 0;
		this.indexTex = new Texture2D(Grid.WidthInCells, Grid.HeightInCells, TextureFormat.Alpha8, false);
		this.indexTex.name = "SubworldRegionIndexData";
		this.indexTex.filterMode = FilterMode.Point;
		this.indexTex.wrapMode = TextureWrapMode.Clamp;
		this.indexTex.anisoLevel = 0;
		for (int i = 0; i < Grid.CellCount; i++)
		{
			array[i] = byte.MaxValue;
			Color32 color = this.zoneColours[7];
			array2[i * 3] = color.r;
			array2[i * 3 + 1] = color.g;
			array2[i * 3 + 2] = color.b;
			this.worldZoneTypes[i] = SubWorld.ZoneType.Space;
		}
		this.colourTex.LoadRawTextureData(array2);
		this.indexTex.LoadRawTextureData(array);
		this.colourTex.Apply();
		this.indexTex.Apply();
		WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
		Vector2 zero = Vector2.zero;
		for (int j = 0; j < clusterDetailSave.overworldCells.Count; j++)
		{
			WorldDetailSave.OverworldCell overworldCell = clusterDetailSave.overworldCells[j];
			Polygon poly = overworldCell.poly;
			zero.y = (float)((int)Mathf.Floor(poly.bounds.yMin));
			while (zero.y < Mathf.Ceil(poly.bounds.yMax))
			{
				zero.x = (float)((int)Mathf.Floor(poly.bounds.xMin));
				while (zero.x < Mathf.Ceil(poly.bounds.xMax))
				{
					if (poly.Contains(zero))
					{
						int num = Grid.XYToCell((int)zero.x, (int)zero.y);
						if (Grid.IsValidCell(num))
						{
							array[num] = ((overworldCell.zoneType == SubWorld.ZoneType.Space) ? byte.MaxValue : ((byte)overworldCell.zoneType));
							this.worldZoneTypes[num] = overworldCell.zoneType;
						}
					}
					zero.x += 1f;
				}
				zero.y += 1f;
			}
		}
		this.InitSimZones(array);
	}

	// Token: 0x06004C1F RID: 19487 RVA: 0x001B28C5 File Offset: 0x001B0AC5
	private void OnShadersReloaded()
	{
		Shader.SetGlobalTexture("_WorldZoneTex", this.colourTex);
		Shader.SetGlobalTexture("_WorldZoneIndexTex", this.indexTex);
	}

	// Token: 0x06004C20 RID: 19488 RVA: 0x001B28E7 File Offset: 0x001B0AE7
	public SubWorld.ZoneType GetSubWorldZoneType(int cell)
	{
		if (cell >= 0 && cell < this.worldZoneTypes.Length)
		{
			return this.worldZoneTypes[cell];
		}
		return SubWorld.ZoneType.Sandstone;
	}

	// Token: 0x06004C21 RID: 19489 RVA: 0x001B2904 File Offset: 0x001B0B04
	private unsafe void InitSimZones(byte[] bytes)
	{
		fixed (byte[] array = bytes)
		{
			byte* msg;
			if (bytes == null || array.Length == 0)
			{
				msg = null;
			}
			else
			{
				msg = &array[0];
			}
			Sim.SIM_HandleMessage(-457308393, bytes.Length, msg);
		}
	}

	// Token: 0x04003290 RID: 12944
	[SerializeField]
	private Texture2D colourTex;

	// Token: 0x04003291 RID: 12945
	[SerializeField]
	private Texture2D indexTex;

	// Token: 0x04003292 RID: 12946
	[HideInInspector]
	public SubWorld.ZoneType[] worldZoneTypes;

	// Token: 0x04003293 RID: 12947
	[SerializeField]
	[HideInInspector]
	public Color32[] zoneColours = new Color32[]
	{
		new Color32(145, 198, 213, 0),
		new Color32(135, 82, 160, 1),
		new Color32(123, 151, 75, 2),
		new Color32(236, 189, 89, 3),
		new Color32(201, 152, 181, 4),
		new Color32(222, 90, 59, 5),
		new Color32(201, 152, 181, 6),
		new Color32(byte.MaxValue, 0, 0, 7),
		new Color32(201, 201, 151, 8),
		new Color32(236, 90, 110, 9),
		new Color32(110, 236, 110, 10),
		new Color32(145, 198, 213, 11),
		new Color32(145, 198, 213, 12),
		new Color32(145, 198, 213, 13),
		new Color32(173, 222, 212, 14),
		new Color32(100, 100, 222, 18),
		new Color32(222, 100, 222, 19),
		new Color32(100, 222, 100, 20)
	};

	// Token: 0x04003294 RID: 12948
	private const int NUM_COLOUR_BYTES = 3;

	// Token: 0x04003295 RID: 12949
	public int[] zoneTextureArrayIndices = new int[]
	{
		0,
		1,
		2,
		3,
		4,
		5,
		5,
		3,
		6,
		7,
		8,
		9,
		10,
		11,
		12,
		7,
		3,
		13,
		0,
		0,
		0
	};
}
