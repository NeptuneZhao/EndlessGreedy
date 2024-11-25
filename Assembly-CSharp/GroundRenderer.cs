using System;
using System.Collections.Generic;
using ProcGen;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000B6F RID: 2927
[AddComponentMenu("KMonoBehaviour/scripts/GroundRenderer")]
public class GroundRenderer : KMonoBehaviour
{
	// Token: 0x060057F3 RID: 22515 RVA: 0x001FB114 File Offset: 0x001F9314
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
		this.OnShadersReloaded();
		this.masks.Initialize();
		SubWorld.ZoneType[] array = (SubWorld.ZoneType[])Enum.GetValues(typeof(SubWorld.ZoneType));
		this.biomeMasks = new GroundMasks.BiomeMaskData[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			SubWorld.ZoneType zone_type = array[i];
			this.biomeMasks[i] = this.GetBiomeMask(zone_type);
		}
	}

	// Token: 0x060057F4 RID: 22516 RVA: 0x001FB190 File Offset: 0x001F9390
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.size = new Vector2I((Grid.WidthInCells + 16 - 1) / 16, (Grid.HeightInCells + 16 - 1) / 16);
		this.dirtyChunks = new bool[this.size.x, this.size.y];
		this.worldChunks = new GroundRenderer.WorldChunk[this.size.x, this.size.y];
		for (int i = 0; i < this.size.y; i++)
		{
			for (int j = 0; j < this.size.x; j++)
			{
				this.worldChunks[j, i] = new GroundRenderer.WorldChunk(j, i);
				this.dirtyChunks[j, i] = true;
			}
		}
	}

	// Token: 0x060057F5 RID: 22517 RVA: 0x001FB258 File Offset: 0x001F9458
	public void Render(Vector2I vis_min, Vector2I vis_max, bool forceVisibleRebuild = false)
	{
		if (!base.enabled)
		{
			return;
		}
		int layer = LayerMask.NameToLayer("World");
		Vector2I vector2I = new Vector2I(vis_min.x / 16, vis_min.y / 16);
		Vector2I vector2I2 = new Vector2I((vis_max.x + 16 - 1) / 16, (vis_max.y + 16 - 1) / 16);
		for (int i = vector2I.y; i < vector2I2.y; i++)
		{
			for (int j = vector2I.x; j < vector2I2.x; j++)
			{
				GroundRenderer.WorldChunk worldChunk = this.worldChunks[j, i];
				if (this.dirtyChunks[j, i] || forceVisibleRebuild)
				{
					this.dirtyChunks[j, i] = false;
					worldChunk.Rebuild(this.biomeMasks, this.elementMaterials);
				}
				worldChunk.Render(layer);
			}
		}
		this.RebuildDirtyChunks();
	}

	// Token: 0x060057F6 RID: 22518 RVA: 0x001FB337 File Offset: 0x001F9537
	public void RenderAll()
	{
		this.Render(new Vector2I(0, 0), new Vector2I(this.worldChunks.GetLength(0) * 16, this.worldChunks.GetLength(1) * 16), true);
	}

	// Token: 0x060057F7 RID: 22519 RVA: 0x001FB36C File Offset: 0x001F956C
	private void RebuildDirtyChunks()
	{
		for (int i = 0; i < this.dirtyChunks.GetLength(1); i++)
		{
			for (int j = 0; j < this.dirtyChunks.GetLength(0); j++)
			{
				if (this.dirtyChunks[j, i])
				{
					this.dirtyChunks[j, i] = false;
					this.worldChunks[j, i].Rebuild(this.biomeMasks, this.elementMaterials);
				}
			}
		}
	}

	// Token: 0x060057F8 RID: 22520 RVA: 0x001FB3E4 File Offset: 0x001F95E4
	public void MarkDirty(int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2I vector2I2 = new Vector2I(vector2I.x / 16, vector2I.y / 16);
		this.dirtyChunks[vector2I2.x, vector2I2.y] = true;
		bool flag = vector2I.x % 16 == 0 && vector2I2.x > 0;
		bool flag2 = vector2I.x % 16 == 15 && vector2I2.x < this.size.x - 1;
		bool flag3 = vector2I.y % 16 == 0 && vector2I2.y > 0;
		bool flag4 = vector2I.y % 16 == 15 && vector2I2.y < this.size.y - 1;
		if (flag)
		{
			this.dirtyChunks[vector2I2.x - 1, vector2I2.y] = true;
			if (flag3)
			{
				this.dirtyChunks[vector2I2.x - 1, vector2I2.y - 1] = true;
			}
			if (flag4)
			{
				this.dirtyChunks[vector2I2.x - 1, vector2I2.y + 1] = true;
			}
		}
		if (flag3)
		{
			this.dirtyChunks[vector2I2.x, vector2I2.y - 1] = true;
		}
		if (flag4)
		{
			this.dirtyChunks[vector2I2.x, vector2I2.y + 1] = true;
		}
		if (flag2)
		{
			this.dirtyChunks[vector2I2.x + 1, vector2I2.y] = true;
			if (flag3)
			{
				this.dirtyChunks[vector2I2.x + 1, vector2I2.y - 1] = true;
			}
			if (flag4)
			{
				this.dirtyChunks[vector2I2.x + 1, vector2I2.y + 1] = true;
			}
		}
	}

	// Token: 0x060057F9 RID: 22521 RVA: 0x001FB598 File Offset: 0x001F9798
	private Vector2I GetChunkIdx(int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		return new Vector2I(vector2I.x / 16, vector2I.y / 16);
	}

	// Token: 0x060057FA RID: 22522 RVA: 0x001FB5C4 File Offset: 0x001F97C4
	private GroundMasks.BiomeMaskData GetBiomeMask(SubWorld.ZoneType zone_type)
	{
		GroundMasks.BiomeMaskData result = null;
		string key = zone_type.ToString().ToLower();
		this.masks.biomeMasks.TryGetValue(key, out result);
		return result;
	}

	// Token: 0x060057FB RID: 22523 RVA: 0x001FB5FC File Offset: 0x001F97FC
	private void InitOpaqueMaterial(Material material, Element element)
	{
		material.name = element.id.ToString() + "_opaque";
		material.renderQueue = RenderQueues.WorldOpaque;
		material.EnableKeyword("OPAQUE");
		material.DisableKeyword("ALPHA");
		this.ConfigureMaterialShine(material);
		material.SetInt("_SrcAlpha", 1);
		material.SetInt("_DstAlpha", 0);
		material.SetInt("_ZWrite", 1);
		material.SetTexture("_AlphaTestMap", Texture2D.whiteTexture);
	}

	// Token: 0x060057FC RID: 22524 RVA: 0x001FB688 File Offset: 0x001F9888
	private void InitAlphaMaterial(Material material, Element element)
	{
		material.name = element.id.ToString() + "_alpha";
		material.renderQueue = RenderQueues.WorldTransparent;
		material.EnableKeyword("ALPHA");
		material.DisableKeyword("OPAQUE");
		this.ConfigureMaterialShine(material);
		material.SetTexture("_AlphaTestMap", this.masks.maskAtlas.texture);
		material.SetInt("_SrcAlpha", 5);
		material.SetInt("_DstAlpha", 10);
		material.SetInt("_ZWrite", 0);
	}

	// Token: 0x060057FD RID: 22525 RVA: 0x001FB720 File Offset: 0x001F9920
	private void ConfigureMaterialShine(Material material)
	{
		if (material.GetTexture("_ShineMask") != null)
		{
			material.DisableKeyword("MATTE");
			material.EnableKeyword("SHINY");
			return;
		}
		material.EnableKeyword("MATTE");
		material.DisableKeyword("SHINY");
	}

	// Token: 0x060057FE RID: 22526 RVA: 0x001FB770 File Offset: 0x001F9970
	[ContextMenu("Reload Shaders")]
	public void OnShadersReloaded()
	{
		this.FreeMaterials();
		foreach (Element element in ElementLoader.elements)
		{
			if (element.IsSolid)
			{
				if (element.substance.material == null)
				{
					DebugUtil.LogErrorArgs(new object[]
					{
						element.name,
						"must have material associated with it in the substance table"
					});
				}
				Material material = new Material(element.substance.material);
				this.InitOpaqueMaterial(material, element);
				Material material2 = new Material(material);
				this.InitAlphaMaterial(material2, element);
				GroundRenderer.Materials value = new GroundRenderer.Materials(material, material2);
				this.elementMaterials[element.id] = value;
			}
		}
		if (this.worldChunks != null)
		{
			for (int i = 0; i < this.dirtyChunks.GetLength(1); i++)
			{
				for (int j = 0; j < this.dirtyChunks.GetLength(0); j++)
				{
					this.dirtyChunks[j, i] = true;
				}
			}
			GroundRenderer.WorldChunk[,] array = this.worldChunks;
			int upperBound = array.GetUpperBound(0);
			int upperBound2 = array.GetUpperBound(1);
			for (int k = array.GetLowerBound(0); k <= upperBound; k++)
			{
				for (int l = array.GetLowerBound(1); l <= upperBound2; l++)
				{
					GroundRenderer.WorldChunk worldChunk = array[k, l];
					worldChunk.Clear();
					worldChunk.Rebuild(this.biomeMasks, this.elementMaterials);
				}
			}
		}
	}

	// Token: 0x060057FF RID: 22527 RVA: 0x001FB908 File Offset: 0x001F9B08
	public void FreeResources()
	{
		this.FreeMaterials();
		this.elementMaterials.Clear();
		this.elementMaterials = null;
		if (this.worldChunks != null)
		{
			GroundRenderer.WorldChunk[,] array = this.worldChunks;
			int upperBound = array.GetUpperBound(0);
			int upperBound2 = array.GetUpperBound(1);
			for (int i = array.GetLowerBound(0); i <= upperBound; i++)
			{
				for (int j = array.GetLowerBound(1); j <= upperBound2; j++)
				{
					GroundRenderer.WorldChunk worldChunk = array[i, j];
					worldChunk.FreeResources();
				}
			}
			this.worldChunks = null;
		}
	}

	// Token: 0x06005800 RID: 22528 RVA: 0x001FB990 File Offset: 0x001F9B90
	private void FreeMaterials()
	{
		foreach (GroundRenderer.Materials materials in this.elementMaterials.Values)
		{
			UnityEngine.Object.Destroy(materials.opaque);
			UnityEngine.Object.Destroy(materials.alpha);
		}
		this.elementMaterials.Clear();
	}

	// Token: 0x0400397F RID: 14719
	[SerializeField]
	private GroundMasks masks;

	// Token: 0x04003980 RID: 14720
	private GroundMasks.BiomeMaskData[] biomeMasks;

	// Token: 0x04003981 RID: 14721
	private Dictionary<SimHashes, GroundRenderer.Materials> elementMaterials = new Dictionary<SimHashes, GroundRenderer.Materials>();

	// Token: 0x04003982 RID: 14722
	private bool[,] dirtyChunks;

	// Token: 0x04003983 RID: 14723
	private GroundRenderer.WorldChunk[,] worldChunks;

	// Token: 0x04003984 RID: 14724
	private const int ChunkEdgeSize = 16;

	// Token: 0x04003985 RID: 14725
	private Vector2I size;

	// Token: 0x02001BD5 RID: 7125
	[Serializable]
	private struct Materials
	{
		// Token: 0x0600A4A5 RID: 42149 RVA: 0x0038D014 File Offset: 0x0038B214
		public Materials(Material opaque, Material alpha)
		{
			this.opaque = opaque;
			this.alpha = alpha;
		}

		// Token: 0x040080CF RID: 32975
		public Material opaque;

		// Token: 0x040080D0 RID: 32976
		public Material alpha;
	}

	// Token: 0x02001BD6 RID: 7126
	private class ElementChunk
	{
		// Token: 0x0600A4A6 RID: 42150 RVA: 0x0038D024 File Offset: 0x0038B224
		public ElementChunk(SimHashes element, Dictionary<SimHashes, GroundRenderer.Materials> materials)
		{
			this.element = element;
			GroundRenderer.Materials materials2 = materials[element];
			this.alpha = new GroundRenderer.ElementChunk.RenderData(materials2.alpha);
			this.opaque = new GroundRenderer.ElementChunk.RenderData(materials2.opaque);
			this.Clear();
		}

		// Token: 0x0600A4A7 RID: 42151 RVA: 0x0038D06E File Offset: 0x0038B26E
		public void Clear()
		{
			this.opaque.Clear();
			this.alpha.Clear();
			this.tileCount = 0;
		}

		// Token: 0x0600A4A8 RID: 42152 RVA: 0x0038D08D File Offset: 0x0038B28D
		public void AddOpaqueQuad(int x, int y, GroundMasks.UVData uvs)
		{
			this.opaque.AddQuad(x, y, uvs);
			this.tileCount++;
		}

		// Token: 0x0600A4A9 RID: 42153 RVA: 0x0038D0AB File Offset: 0x0038B2AB
		public void AddAlphaQuad(int x, int y, GroundMasks.UVData uvs)
		{
			this.alpha.AddQuad(x, y, uvs);
			this.tileCount++;
		}

		// Token: 0x0600A4AA RID: 42154 RVA: 0x0038D0C9 File Offset: 0x0038B2C9
		public void Build()
		{
			this.opaque.Build();
			this.alpha.Build();
		}

		// Token: 0x0600A4AB RID: 42155 RVA: 0x0038D0E4 File Offset: 0x0038B2E4
		public void Render(int layer, int element_idx)
		{
			float num = Grid.GetLayerZ(Grid.SceneLayer.Ground);
			num -= 0.0001f * (float)element_idx;
			this.opaque.Render(new Vector3(0f, 0f, num), layer);
			this.alpha.Render(new Vector3(0f, 0f, num), layer);
		}

		// Token: 0x0600A4AC RID: 42156 RVA: 0x0038D13C File Offset: 0x0038B33C
		public void FreeResources()
		{
			this.alpha.FreeResources();
			this.opaque.FreeResources();
			this.alpha = null;
			this.opaque = null;
		}

		// Token: 0x040080D1 RID: 32977
		public SimHashes element;

		// Token: 0x040080D2 RID: 32978
		private GroundRenderer.ElementChunk.RenderData alpha;

		// Token: 0x040080D3 RID: 32979
		private GroundRenderer.ElementChunk.RenderData opaque;

		// Token: 0x040080D4 RID: 32980
		public int tileCount;

		// Token: 0x02002630 RID: 9776
		private class RenderData
		{
			// Token: 0x0600C19F RID: 49567 RVA: 0x003DE818 File Offset: 0x003DCA18
			public RenderData(Material material)
			{
				this.material = material;
				this.mesh = new Mesh();
				this.mesh.MarkDynamic();
				this.mesh.name = "ElementChunk";
				this.pos = new List<Vector3>();
				this.uv = new List<Vector2>();
				this.indices = new List<int>();
			}

			// Token: 0x0600C1A0 RID: 49568 RVA: 0x003DE879 File Offset: 0x003DCA79
			public void ClearMesh()
			{
				if (this.mesh != null)
				{
					this.mesh.Clear();
					UnityEngine.Object.DestroyImmediate(this.mesh);
					this.mesh = null;
				}
			}

			// Token: 0x0600C1A1 RID: 49569 RVA: 0x003DE8A8 File Offset: 0x003DCAA8
			public void Clear()
			{
				if (this.mesh != null)
				{
					this.mesh.Clear();
				}
				if (this.pos != null)
				{
					this.pos.Clear();
				}
				if (this.uv != null)
				{
					this.uv.Clear();
				}
				if (this.indices != null)
				{
					this.indices.Clear();
				}
			}

			// Token: 0x0600C1A2 RID: 49570 RVA: 0x003DE907 File Offset: 0x003DCB07
			public void FreeResources()
			{
				this.ClearMesh();
				this.Clear();
				this.pos = null;
				this.uv = null;
				this.indices = null;
				this.material = null;
			}

			// Token: 0x0600C1A3 RID: 49571 RVA: 0x003DE931 File Offset: 0x003DCB31
			public void Build()
			{
				this.mesh.SetVertices(this.pos);
				this.mesh.SetUVs(0, this.uv);
				this.mesh.SetTriangles(this.indices, 0);
			}

			// Token: 0x0600C1A4 RID: 49572 RVA: 0x003DE968 File Offset: 0x003DCB68
			public void AddQuad(int x, int y, GroundMasks.UVData uvs)
			{
				int count = this.pos.Count;
				this.indices.Add(count);
				this.indices.Add(count + 1);
				this.indices.Add(count + 3);
				this.indices.Add(count);
				this.indices.Add(count + 3);
				this.indices.Add(count + 2);
				this.pos.Add(new Vector3((float)x + -0.5f, (float)y + -0.5f, 0f));
				this.pos.Add(new Vector3((float)x + 1f + -0.5f, (float)y + -0.5f, 0f));
				this.pos.Add(new Vector3((float)x + -0.5f, (float)y + 1f + -0.5f, 0f));
				this.pos.Add(new Vector3((float)x + 1f + -0.5f, (float)y + 1f + -0.5f, 0f));
				this.uv.Add(uvs.bl);
				this.uv.Add(uvs.br);
				this.uv.Add(uvs.tl);
				this.uv.Add(uvs.tr);
			}

			// Token: 0x0600C1A5 RID: 49573 RVA: 0x003DEAC4 File Offset: 0x003DCCC4
			public void Render(Vector3 position, int layer)
			{
				if (this.pos.Count != 0)
				{
					Graphics.DrawMesh(this.mesh, position, Quaternion.identity, this.material, layer, null, 0, null, ShadowCastingMode.Off, false, null, false);
				}
			}

			// Token: 0x0400A9ED RID: 43501
			public Material material;

			// Token: 0x0400A9EE RID: 43502
			public Mesh mesh;

			// Token: 0x0400A9EF RID: 43503
			public List<Vector3> pos;

			// Token: 0x0400A9F0 RID: 43504
			public List<Vector2> uv;

			// Token: 0x0400A9F1 RID: 43505
			public List<int> indices;
		}
	}

	// Token: 0x02001BD7 RID: 7127
	private struct WorldChunk
	{
		// Token: 0x0600A4AD RID: 42157 RVA: 0x0038D162 File Offset: 0x0038B362
		public WorldChunk(int x, int y)
		{
			this.chunkX = x;
			this.chunkY = y;
			this.elementChunks = new List<GroundRenderer.ElementChunk>();
		}

		// Token: 0x0600A4AE RID: 42158 RVA: 0x0038D17D File Offset: 0x0038B37D
		public void Clear()
		{
			this.elementChunks.Clear();
		}

		// Token: 0x0600A4AF RID: 42159 RVA: 0x0038D18C File Offset: 0x0038B38C
		private static void InsertSorted(Element element, Element[] array, int size)
		{
			int id = (int)element.id;
			for (int i = 0; i < size; i++)
			{
				Element element2 = array[i];
				if (element2.id > (SimHashes)id)
				{
					array[i] = element;
					element = element2;
					id = (int)element2.id;
				}
			}
			array[size] = element;
		}

		// Token: 0x0600A4B0 RID: 42160 RVA: 0x0038D1CC File Offset: 0x0038B3CC
		public void Rebuild(GroundMasks.BiomeMaskData[] biomeMasks, Dictionary<SimHashes, GroundRenderer.Materials> materials)
		{
			foreach (GroundRenderer.ElementChunk elementChunk in this.elementChunks)
			{
				elementChunk.Clear();
			}
			Vector2I vector2I = new Vector2I(this.chunkX * 16, this.chunkY * 16);
			Vector2I vector2I2 = new Vector2I(Math.Min(Grid.WidthInCells, (this.chunkX + 1) * 16), Math.Min(Grid.HeightInCells, (this.chunkY + 1) * 16));
			for (int i = vector2I.y; i < vector2I2.y; i++)
			{
				int num = Math.Max(0, i - 1);
				int num2 = i;
				for (int j = vector2I.x; j < vector2I2.x; j++)
				{
					int num3 = Math.Max(0, j - 1);
					int num4 = j;
					int num5 = num * Grid.WidthInCells + num3;
					int num6 = num * Grid.WidthInCells + num4;
					int num7 = num2 * Grid.WidthInCells + num3;
					int num8 = num2 * Grid.WidthInCells + num4;
					GroundRenderer.WorldChunk.elements[0] = Grid.Element[num5];
					GroundRenderer.WorldChunk.elements[1] = Grid.Element[num6];
					GroundRenderer.WorldChunk.elements[2] = Grid.Element[num7];
					GroundRenderer.WorldChunk.elements[3] = Grid.Element[num8];
					GroundRenderer.WorldChunk.substances[0] = ((Grid.RenderedByWorld[num5] && GroundRenderer.WorldChunk.elements[0].IsSolid) ? GroundRenderer.WorldChunk.elements[0].substance.idx : -1);
					GroundRenderer.WorldChunk.substances[1] = ((Grid.RenderedByWorld[num6] && GroundRenderer.WorldChunk.elements[1].IsSolid) ? GroundRenderer.WorldChunk.elements[1].substance.idx : -1);
					GroundRenderer.WorldChunk.substances[2] = ((Grid.RenderedByWorld[num7] && GroundRenderer.WorldChunk.elements[2].IsSolid) ? GroundRenderer.WorldChunk.elements[2].substance.idx : -1);
					GroundRenderer.WorldChunk.substances[3] = ((Grid.RenderedByWorld[num8] && GroundRenderer.WorldChunk.elements[3].IsSolid) ? GroundRenderer.WorldChunk.elements[3].substance.idx : -1);
					GroundRenderer.WorldChunk.uniqueElements[0] = GroundRenderer.WorldChunk.elements[0];
					GroundRenderer.WorldChunk.InsertSorted(GroundRenderer.WorldChunk.elements[1], GroundRenderer.WorldChunk.uniqueElements, 1);
					GroundRenderer.WorldChunk.InsertSorted(GroundRenderer.WorldChunk.elements[2], GroundRenderer.WorldChunk.uniqueElements, 2);
					GroundRenderer.WorldChunk.InsertSorted(GroundRenderer.WorldChunk.elements[3], GroundRenderer.WorldChunk.uniqueElements, 3);
					int num9 = -1;
					int biomeIdx = GroundRenderer.WorldChunk.GetBiomeIdx(i * Grid.WidthInCells + j);
					GroundMasks.BiomeMaskData biomeMaskData = biomeMasks[biomeIdx];
					if (biomeMaskData == null)
					{
						biomeMaskData = biomeMasks[3];
					}
					for (int k = 0; k < GroundRenderer.WorldChunk.uniqueElements.Length; k++)
					{
						Element element = GroundRenderer.WorldChunk.uniqueElements[k];
						if (element.IsSolid)
						{
							int idx = element.substance.idx;
							if (idx != num9)
							{
								num9 = idx;
								int num10 = ((GroundRenderer.WorldChunk.substances[2] >= idx) ? 1 : 0) << 3 | ((GroundRenderer.WorldChunk.substances[3] >= idx) ? 1 : 0) << 2 | ((GroundRenderer.WorldChunk.substances[0] >= idx) ? 1 : 0) << 1 | ((GroundRenderer.WorldChunk.substances[1] >= idx) ? 1 : 0);
								if (num10 > 0)
								{
									GroundMasks.UVData[] variationUVs = biomeMaskData.tiles[num10].variationUVs;
									float staticRandom = GroundRenderer.WorldChunk.GetStaticRandom(j, i);
									int num11 = Mathf.Min(variationUVs.Length - 1, (int)((float)variationUVs.Length * staticRandom));
									GroundMasks.UVData uvs = variationUVs[num11 % variationUVs.Length];
									GroundRenderer.ElementChunk elementChunk2 = this.GetElementChunk(element.id, materials);
									if (num10 == 15)
									{
										elementChunk2.AddOpaqueQuad(j, i, uvs);
									}
									else
									{
										elementChunk2.AddAlphaQuad(j, i, uvs);
									}
								}
							}
						}
					}
				}
			}
			foreach (GroundRenderer.ElementChunk elementChunk3 in this.elementChunks)
			{
				elementChunk3.Build();
			}
			for (int l = this.elementChunks.Count - 1; l >= 0; l--)
			{
				if (this.elementChunks[l].tileCount == 0)
				{
					int index = this.elementChunks.Count - 1;
					this.elementChunks[l] = this.elementChunks[index];
					this.elementChunks.RemoveAt(index);
				}
			}
		}

		// Token: 0x0600A4B1 RID: 42161 RVA: 0x0038D628 File Offset: 0x0038B828
		private GroundRenderer.ElementChunk GetElementChunk(SimHashes elementID, Dictionary<SimHashes, GroundRenderer.Materials> materials)
		{
			GroundRenderer.ElementChunk elementChunk = null;
			for (int i = 0; i < this.elementChunks.Count; i++)
			{
				if (this.elementChunks[i].element == elementID)
				{
					elementChunk = this.elementChunks[i];
					break;
				}
			}
			if (elementChunk == null)
			{
				elementChunk = new GroundRenderer.ElementChunk(elementID, materials);
				this.elementChunks.Add(elementChunk);
			}
			return elementChunk;
		}

		// Token: 0x0600A4B2 RID: 42162 RVA: 0x0038D688 File Offset: 0x0038B888
		private static int GetBiomeIdx(int cell)
		{
			if (!Grid.IsValidCell(cell))
			{
				return 0;
			}
			SubWorld.ZoneType result = SubWorld.ZoneType.Sandstone;
			if (global::World.Instance != null && global::World.Instance.zoneRenderData != null)
			{
				result = global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell);
			}
			return (int)result;
		}

		// Token: 0x0600A4B3 RID: 42163 RVA: 0x0038D6D2 File Offset: 0x0038B8D2
		private static float GetStaticRandom(int x, int y)
		{
			return PerlinSimplexNoise.noise((float)x * GroundRenderer.WorldChunk.NoiseScale.x, (float)y * GroundRenderer.WorldChunk.NoiseScale.y);
		}

		// Token: 0x0600A4B4 RID: 42164 RVA: 0x0038D6F4 File Offset: 0x0038B8F4
		public void Render(int layer)
		{
			for (int i = 0; i < this.elementChunks.Count; i++)
			{
				GroundRenderer.ElementChunk elementChunk = this.elementChunks[i];
				elementChunk.Render(layer, ElementLoader.FindElementByHash(elementChunk.element).substance.idx);
			}
		}

		// Token: 0x0600A4B5 RID: 42165 RVA: 0x0038D740 File Offset: 0x0038B940
		public void FreeResources()
		{
			foreach (GroundRenderer.ElementChunk elementChunk in this.elementChunks)
			{
				elementChunk.FreeResources();
			}
			this.elementChunks.Clear();
			this.elementChunks = null;
		}

		// Token: 0x040080D5 RID: 32981
		public readonly int chunkX;

		// Token: 0x040080D6 RID: 32982
		public readonly int chunkY;

		// Token: 0x040080D7 RID: 32983
		private List<GroundRenderer.ElementChunk> elementChunks;

		// Token: 0x040080D8 RID: 32984
		private static Element[] elements = new Element[4];

		// Token: 0x040080D9 RID: 32985
		private static Element[] uniqueElements = new Element[4];

		// Token: 0x040080DA RID: 32986
		private static int[] substances = new int[4];

		// Token: 0x040080DB RID: 32987
		private static Vector2 NoiseScale = new Vector3(1f, 1f);
	}
}
