using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering
{
	// Token: 0x02000E15 RID: 3605
	public class BlockTileRenderer : MonoBehaviour
	{
		// Token: 0x060072E2 RID: 29410 RVA: 0x002C0D95 File Offset: 0x002BEF95
		public static BlockTileRenderer.RenderInfoLayer GetRenderInfoLayer(bool isReplacement, SimHashes element)
		{
			if (isReplacement)
			{
				return BlockTileRenderer.RenderInfoLayer.Replacement;
			}
			if (element == SimHashes.Void)
			{
				return BlockTileRenderer.RenderInfoLayer.UnderConstruction;
			}
			return BlockTileRenderer.RenderInfoLayer.Built;
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x060072E3 RID: 29411 RVA: 0x002C0DA7 File Offset: 0x002BEFA7
		public bool ForceRebuild
		{
			get
			{
				return this.forceRebuild;
			}
		}

		// Token: 0x060072E4 RID: 29412 RVA: 0x002C0DB0 File Offset: 0x002BEFB0
		public BlockTileRenderer()
		{
			this.forceRebuild = false;
		}

		// Token: 0x060072E5 RID: 29413 RVA: 0x002C0E34 File Offset: 0x002BF034
		public void FreeResources()
		{
			foreach (KeyValuePair<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> keyValuePair in this.renderInfo)
			{
				if (keyValuePair.Value != null)
				{
					keyValuePair.Value.FreeResources();
				}
			}
			this.renderInfo.Clear();
		}

		// Token: 0x060072E6 RID: 29414 RVA: 0x002C0EA0 File Offset: 0x002BF0A0
		private static bool MatchesDef(GameObject go, BuildingDef def)
		{
			return go != null && go.GetComponent<Building>().Def == def;
		}

		// Token: 0x060072E7 RID: 29415 RVA: 0x002C0EC0 File Offset: 0x002BF0C0
		public virtual BlockTileRenderer.Bits GetConnectionBits(int x, int y, int query_layer)
		{
			BlockTileRenderer.Bits bits = (BlockTileRenderer.Bits)0;
			GameObject gameObject = Grid.Objects[y * Grid.WidthInCells + x, query_layer];
			BuildingDef def = (gameObject != null) ? gameObject.GetComponent<Building>().Def : null;
			if (y > 0)
			{
				int num = (y - 1) * Grid.WidthInCells + x;
				if (x > 0 && BlockTileRenderer.MatchesDef(Grid.Objects[num - 1, query_layer], def))
				{
					bits |= BlockTileRenderer.Bits.DownLeft;
				}
				if (BlockTileRenderer.MatchesDef(Grid.Objects[num, query_layer], def))
				{
					bits |= BlockTileRenderer.Bits.Down;
				}
				if (x < Grid.WidthInCells - 1 && BlockTileRenderer.MatchesDef(Grid.Objects[num + 1, query_layer], def))
				{
					bits |= BlockTileRenderer.Bits.DownRight;
				}
			}
			int num2 = y * Grid.WidthInCells + x;
			if (x > 0 && BlockTileRenderer.MatchesDef(Grid.Objects[num2 - 1, query_layer], def))
			{
				bits |= BlockTileRenderer.Bits.Left;
			}
			if (x < Grid.WidthInCells - 1 && BlockTileRenderer.MatchesDef(Grid.Objects[num2 + 1, query_layer], def))
			{
				bits |= BlockTileRenderer.Bits.Right;
			}
			if (y < Grid.HeightInCells - 1)
			{
				int num3 = (y + 1) * Grid.WidthInCells + x;
				if (x > 0 && BlockTileRenderer.MatchesDef(Grid.Objects[num3 - 1, query_layer], def))
				{
					bits |= BlockTileRenderer.Bits.UpLeft;
				}
				if (BlockTileRenderer.MatchesDef(Grid.Objects[num3, query_layer], def))
				{
					bits |= BlockTileRenderer.Bits.Up;
				}
				if (x < Grid.WidthInCells + 1 && BlockTileRenderer.MatchesDef(Grid.Objects[num3 + 1, query_layer], def))
				{
					bits |= BlockTileRenderer.Bits.UpRight;
				}
			}
			return bits;
		}

		// Token: 0x060072E8 RID: 29416 RVA: 0x002C1034 File Offset: 0x002BF234
		private bool IsDecorConnectable(GameObject src, GameObject target)
		{
			if (src != null && target != null)
			{
				IBlockTileInfo component = src.GetComponent<IBlockTileInfo>();
				IBlockTileInfo component2 = target.GetComponent<IBlockTileInfo>();
				if (component != null && component2 != null)
				{
					return component.GetBlockTileConnectorID() == component2.GetBlockTileConnectorID();
				}
			}
			return false;
		}

		// Token: 0x060072E9 RID: 29417 RVA: 0x002C1078 File Offset: 0x002BF278
		public virtual BlockTileRenderer.Bits GetDecorConnectionBits(int x, int y, int query_layer)
		{
			BlockTileRenderer.Bits bits = (BlockTileRenderer.Bits)0;
			GameObject src = Grid.Objects[y * Grid.WidthInCells + x, query_layer];
			if (y > 0)
			{
				int num = (y - 1) * Grid.WidthInCells + x;
				if (x > 0 && Grid.Objects[num - 1, query_layer] != null)
				{
					bits |= BlockTileRenderer.Bits.DownLeft;
				}
				if (Grid.Objects[num, query_layer] != null)
				{
					bits |= BlockTileRenderer.Bits.Down;
				}
				if (x < Grid.WidthInCells - 1 && Grid.Objects[num + 1, query_layer] != null)
				{
					bits |= BlockTileRenderer.Bits.DownRight;
				}
			}
			int num2 = y * Grid.WidthInCells + x;
			if (x > 0 && this.IsDecorConnectable(src, Grid.Objects[num2 - 1, query_layer]))
			{
				bits |= BlockTileRenderer.Bits.Left;
			}
			if (x < Grid.WidthInCells - 1 && this.IsDecorConnectable(src, Grid.Objects[num2 + 1, query_layer]))
			{
				bits |= BlockTileRenderer.Bits.Right;
			}
			if (y < Grid.HeightInCells - 1)
			{
				int num3 = (y + 1) * Grid.WidthInCells + x;
				if (x > 0 && Grid.Objects[num3 - 1, query_layer] != null)
				{
					bits |= BlockTileRenderer.Bits.UpLeft;
				}
				if (Grid.Objects[num3, query_layer] != null)
				{
					bits |= BlockTileRenderer.Bits.Up;
				}
				if (x < Grid.WidthInCells + 1 && Grid.Objects[num3 + 1, query_layer] != null)
				{
					bits |= BlockTileRenderer.Bits.UpRight;
				}
			}
			return bits;
		}

		// Token: 0x060072EA RID: 29418 RVA: 0x002C11D0 File Offset: 0x002BF3D0
		public void LateUpdate()
		{
			this.Render();
		}

		// Token: 0x060072EB RID: 29419 RVA: 0x002C11D8 File Offset: 0x002BF3D8
		private void Render()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			if (GameUtil.IsCapturingTimeLapse())
			{
				vector2I = new Vector2I(0, 0);
				vector2I2 = new Vector2I(Grid.WidthInCells / 16, Grid.HeightInCells / 16);
			}
			else
			{
				GridArea visibleArea = GridVisibleArea.GetVisibleArea();
				vector2I = new Vector2I(visibleArea.Min.x / 16, visibleArea.Min.y / 16);
				vector2I2 = new Vector2I((visibleArea.Max.x + 16 - 1) / 16, (visibleArea.Max.y + 16 - 1) / 16);
			}
			foreach (KeyValuePair<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> keyValuePair in this.renderInfo)
			{
				BlockTileRenderer.RenderInfo value = keyValuePair.Value;
				for (int i = vector2I.y; i < vector2I2.y; i++)
				{
					for (int j = vector2I.x; j < vector2I2.x; j++)
					{
						value.Rebuild(this, j, i, MeshUtil.vertices, MeshUtil.uvs, MeshUtil.indices, MeshUtil.colours);
						value.Render(j, i);
					}
				}
			}
		}

		// Token: 0x060072EC RID: 29420 RVA: 0x002C1310 File Offset: 0x002BF510
		public Color GetCellColour(int cell, SimHashes element)
		{
			Color white;
			if (cell == this.selectedCell)
			{
				white = this.selectColour;
			}
			else if (cell == this.invalidPlaceCell && element == SimHashes.Void)
			{
				white = this.invalidPlaceColour;
			}
			else if (cell == this.highlightCell)
			{
				white = this.highlightColour;
			}
			else
			{
				white = Color.white;
			}
			return white;
		}

		// Token: 0x060072ED RID: 29421 RVA: 0x002C1364 File Offset: 0x002BF564
		public static Vector2I GetChunkIdx(int cell)
		{
			Vector2I vector2I = Grid.CellToXY(cell);
			return new Vector2I(vector2I.x / 16, vector2I.y / 16);
		}

		// Token: 0x060072EE RID: 29422 RVA: 0x002C1390 File Offset: 0x002BF590
		public void AddBlock(int renderLayer, BuildingDef def, bool isReplacement, SimHashes element, int cell)
		{
			KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer> key = new KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>(def, BlockTileRenderer.GetRenderInfoLayer(isReplacement, element));
			BlockTileRenderer.RenderInfo renderInfo;
			if (!this.renderInfo.TryGetValue(key, out renderInfo))
			{
				int queryLayer = (int)(isReplacement ? def.ReplacementLayer : def.TileLayer);
				renderInfo = new BlockTileRenderer.RenderInfo(this, queryLayer, renderLayer, def, element);
				this.renderInfo[key] = renderInfo;
			}
			renderInfo.AddCell(cell);
		}

		// Token: 0x060072EF RID: 29423 RVA: 0x002C13F0 File Offset: 0x002BF5F0
		public void RemoveBlock(BuildingDef def, bool isReplacement, SimHashes element, int cell)
		{
			KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer> key = new KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>(def, BlockTileRenderer.GetRenderInfoLayer(isReplacement, element));
			BlockTileRenderer.RenderInfo renderInfo;
			if (this.renderInfo.TryGetValue(key, out renderInfo))
			{
				renderInfo.RemoveCell(cell);
			}
		}

		// Token: 0x060072F0 RID: 29424 RVA: 0x002C1424 File Offset: 0x002BF624
		public void Rebuild(ObjectLayer layer, int cell)
		{
			foreach (KeyValuePair<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> keyValuePair in this.renderInfo)
			{
				if (keyValuePair.Key.Key.TileLayer == layer)
				{
					keyValuePair.Value.MarkDirty(cell);
				}
			}
		}

		// Token: 0x060072F1 RID: 29425 RVA: 0x002C1494 File Offset: 0x002BF694
		public void SelectCell(int cell, bool enabled)
		{
			this.UpdateCellStatus(ref this.selectedCell, cell, enabled);
		}

		// Token: 0x060072F2 RID: 29426 RVA: 0x002C14A4 File Offset: 0x002BF6A4
		public void HighlightCell(int cell, bool enabled)
		{
			this.UpdateCellStatus(ref this.highlightCell, cell, enabled);
		}

		// Token: 0x060072F3 RID: 29427 RVA: 0x002C14B4 File Offset: 0x002BF6B4
		public void SetInvalidPlaceCell(int cell, bool enabled)
		{
			this.UpdateCellStatus(ref this.invalidPlaceCell, cell, enabled);
		}

		// Token: 0x060072F4 RID: 29428 RVA: 0x002C14C4 File Offset: 0x002BF6C4
		private void UpdateCellStatus(ref int cell_status, int cell, bool enabled)
		{
			if (enabled)
			{
				if (cell == cell_status)
				{
					return;
				}
				if (cell_status != -1)
				{
					foreach (KeyValuePair<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> keyValuePair in this.renderInfo)
					{
						keyValuePair.Value.MarkDirtyIfOccupied(cell_status);
					}
				}
				cell_status = cell;
				using (Dictionary<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo>.Enumerator enumerator = this.renderInfo.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> keyValuePair2 = enumerator.Current;
						keyValuePair2.Value.MarkDirtyIfOccupied(cell_status);
					}
					return;
				}
			}
			if (cell_status == cell)
			{
				foreach (KeyValuePair<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> keyValuePair3 in this.renderInfo)
				{
					keyValuePair3.Value.MarkDirty(cell_status);
				}
				cell_status = -1;
			}
		}

		// Token: 0x04004F0E RID: 20238
		[SerializeField]
		private bool forceRebuild;

		// Token: 0x04004F0F RID: 20239
		[SerializeField]
		private Color highlightColour = new Color(1.25f, 1.25f, 1.25f, 1f);

		// Token: 0x04004F10 RID: 20240
		[SerializeField]
		private Color selectColour = new Color(1.5f, 1.5f, 1.5f, 1f);

		// Token: 0x04004F11 RID: 20241
		[SerializeField]
		private Color invalidPlaceColour = Color.red;

		// Token: 0x04004F12 RID: 20242
		private const float TILE_ATLAS_WIDTH = 2048f;

		// Token: 0x04004F13 RID: 20243
		private const float TILE_ATLAS_HEIGHT = 2048f;

		// Token: 0x04004F14 RID: 20244
		private const int chunkEdgeSize = 16;

		// Token: 0x04004F15 RID: 20245
		protected Dictionary<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo> renderInfo = new Dictionary<KeyValuePair<BuildingDef, BlockTileRenderer.RenderInfoLayer>, BlockTileRenderer.RenderInfo>();

		// Token: 0x04004F16 RID: 20246
		private int selectedCell = -1;

		// Token: 0x04004F17 RID: 20247
		private int highlightCell = -1;

		// Token: 0x04004F18 RID: 20248
		private int invalidPlaceCell = -1;

		// Token: 0x02001F45 RID: 8005
		public enum RenderInfoLayer
		{
			// Token: 0x04008D08 RID: 36104
			Built,
			// Token: 0x04008D09 RID: 36105
			UnderConstruction,
			// Token: 0x04008D0A RID: 36106
			Replacement
		}

		// Token: 0x02001F46 RID: 8006
		[Flags]
		public enum Bits
		{
			// Token: 0x04008D0C RID: 36108
			UpLeft = 128,
			// Token: 0x04008D0D RID: 36109
			Up = 64,
			// Token: 0x04008D0E RID: 36110
			UpRight = 32,
			// Token: 0x04008D0F RID: 36111
			Left = 16,
			// Token: 0x04008D10 RID: 36112
			Right = 8,
			// Token: 0x04008D11 RID: 36113
			DownLeft = 4,
			// Token: 0x04008D12 RID: 36114
			Down = 2,
			// Token: 0x04008D13 RID: 36115
			DownRight = 1
		}

		// Token: 0x02001F47 RID: 8007
		protected class RenderInfo
		{
			// Token: 0x0600ADCD RID: 44493 RVA: 0x003AA088 File Offset: 0x003A8288
			public RenderInfo(BlockTileRenderer renderer, int queryLayer, int renderLayer, BuildingDef def, SimHashes element)
			{
				this.queryLayer = queryLayer;
				this.renderLayer = renderLayer;
				this.rootPosition = new Vector3(0f, 0f, Grid.GetLayerZ(def.SceneLayer));
				this.element = element;
				this.material = new Material(def.BlockTileMaterial);
				if (def.BlockTileIsTransparent)
				{
					this.material.renderQueue = RenderQueues.Liquid;
					this.decorZOffset = Grid.GetLayerZ(Grid.SceneLayer.TileFront) - Grid.GetLayerZ(Grid.SceneLayer.Liquid) - 1f;
				}
				else if (def.SceneLayer == Grid.SceneLayer.TileMain)
				{
					this.material.renderQueue = RenderQueues.BlockTiles;
				}
				this.material.DisableKeyword("ENABLE_SHINE");
				if (element != SimHashes.Void)
				{
					this.material.SetTexture("_MainTex", def.BlockTileAtlas.texture);
					this.material.name = def.BlockTileAtlas.name + "Mat";
					if (def.BlockTileShineAtlas != null)
					{
						this.material.SetTexture("_SpecularTex", def.BlockTileShineAtlas.texture);
						this.material.EnableKeyword("ENABLE_SHINE");
					}
				}
				else
				{
					this.material.SetTexture("_MainTex", def.BlockTilePlaceAtlas.texture);
					this.material.name = def.BlockTilePlaceAtlas.name + "Mat";
				}
				int num = Grid.WidthInCells / 16 + 1;
				int num2 = Grid.HeightInCells / 16 + 1;
				this.meshChunks = new Mesh[num, num2];
				this.dirtyChunks = new bool[num, num2];
				for (int i = 0; i < num2; i++)
				{
					for (int j = 0; j < num; j++)
					{
						this.dirtyChunks[j, i] = true;
					}
				}
				BlockTileDecorInfo blockTileDecorInfo = (element == SimHashes.Void) ? def.DecorPlaceBlockTileInfo : def.DecorBlockTileInfo;
				if (blockTileDecorInfo)
				{
					this.decorRenderInfo = new BlockTileRenderer.DecorRenderInfo(num, num2, queryLayer, def, blockTileDecorInfo);
				}
				int num3 = def.BlockTileAtlas.items[0].name.Length - 4 - 8;
				int startIndex = num3 - 1 - 8;
				this.atlasInfo = new BlockTileRenderer.RenderInfo.AtlasInfo[def.BlockTileAtlas.items.Length];
				for (int k = 0; k < this.atlasInfo.Length; k++)
				{
					TextureAtlas.Item item = def.BlockTileAtlas.items[k];
					string value = item.name.Substring(startIndex, 8);
					string value2 = item.name.Substring(num3, 8);
					int requiredConnections = Convert.ToInt32(value, 2);
					int forbiddenConnections = Convert.ToInt32(value2, 2);
					this.atlasInfo[k].requiredConnections = (BlockTileRenderer.Bits)requiredConnections;
					this.atlasInfo[k].forbiddenConnections = (BlockTileRenderer.Bits)forbiddenConnections;
					this.atlasInfo[k].uvBox = item.uvBox;
					this.atlasInfo[k].name = item.name;
				}
				this.trimUVSize = new Vector2(0.03125f, 0.03125f);
			}

			// Token: 0x0600ADCE RID: 44494 RVA: 0x003AA3C4 File Offset: 0x003A85C4
			public void FreeResources()
			{
				UnityEngine.Object.DestroyImmediate(this.material);
				this.material = null;
				this.atlasInfo = null;
				for (int i = 0; i < this.meshChunks.GetLength(0); i++)
				{
					for (int j = 0; j < this.meshChunks.GetLength(1); j++)
					{
						if (this.meshChunks[i, j] != null)
						{
							UnityEngine.Object.DestroyImmediate(this.meshChunks[i, j]);
							this.meshChunks[i, j] = null;
						}
					}
				}
				this.meshChunks = null;
				this.decorRenderInfo = null;
				this.occupiedCells.Clear();
			}

			// Token: 0x0600ADCF RID: 44495 RVA: 0x003AA468 File Offset: 0x003A8668
			public void AddCell(int cell)
			{
				int num = 0;
				this.occupiedCells.TryGetValue(cell, out num);
				this.occupiedCells[cell] = num + 1;
				this.MarkDirty(cell);
			}

			// Token: 0x0600ADD0 RID: 44496 RVA: 0x003AA49C File Offset: 0x003A869C
			public void RemoveCell(int cell)
			{
				int num = 0;
				this.occupiedCells.TryGetValue(cell, out num);
				if (num > 1)
				{
					this.occupiedCells[cell] = num - 1;
				}
				else
				{
					this.occupiedCells.Remove(cell);
				}
				this.MarkDirty(cell);
			}

			// Token: 0x0600ADD1 RID: 44497 RVA: 0x003AA4E4 File Offset: 0x003A86E4
			public void MarkDirty(int cell)
			{
				Vector2I chunkIdx = BlockTileRenderer.GetChunkIdx(cell);
				this.dirtyChunks[chunkIdx.x, chunkIdx.y] = true;
			}

			// Token: 0x0600ADD2 RID: 44498 RVA: 0x003AA510 File Offset: 0x003A8710
			public void MarkDirtyIfOccupied(int cell)
			{
				if (this.occupiedCells.ContainsKey(cell))
				{
					this.MarkDirty(cell);
				}
			}

			// Token: 0x0600ADD3 RID: 44499 RVA: 0x003AA528 File Offset: 0x003A8728
			public void Render(int x, int y)
			{
				if (this.meshChunks[x, y] != null)
				{
					Graphics.DrawMesh(this.meshChunks[x, y], this.rootPosition, Quaternion.identity, this.material, this.renderLayer);
				}
				if (this.decorRenderInfo != null)
				{
					this.decorRenderInfo.Render(x, y, this.rootPosition - new Vector3(0f, 0f, 0.5f), this.renderLayer);
				}
			}

			// Token: 0x0600ADD4 RID: 44500 RVA: 0x003AA5B0 File Offset: 0x003A87B0
			public void Rebuild(BlockTileRenderer renderer, int chunk_x, int chunk_y, List<Vector3> vertices, List<Vector2> uvs, List<int> indices, List<Color> colours)
			{
				if (!this.dirtyChunks[chunk_x, chunk_y] && !renderer.ForceRebuild)
				{
					return;
				}
				this.dirtyChunks[chunk_x, chunk_y] = false;
				vertices.Clear();
				uvs.Clear();
				indices.Clear();
				colours.Clear();
				for (int i = chunk_y * 16; i < chunk_y * 16 + 16; i++)
				{
					for (int j = chunk_x * 16; j < chunk_x * 16 + 16; j++)
					{
						int num = i * Grid.WidthInCells + j;
						if (this.occupiedCells.ContainsKey(num))
						{
							BlockTileRenderer.Bits connectionBits = renderer.GetConnectionBits(j, i, this.queryLayer);
							for (int k = 0; k < this.atlasInfo.Length; k++)
							{
								bool flag = (this.atlasInfo[k].requiredConnections & connectionBits) == this.atlasInfo[k].requiredConnections;
								bool flag2 = (this.atlasInfo[k].forbiddenConnections & connectionBits) > (BlockTileRenderer.Bits)0;
								if (flag && !flag2)
								{
									Color cellColour = renderer.GetCellColour(num, this.element);
									this.AddVertexInfo(this.atlasInfo[k], this.trimUVSize, j, i, connectionBits, cellColour, vertices, uvs, indices, colours);
									break;
								}
							}
						}
					}
				}
				Mesh mesh = this.meshChunks[chunk_x, chunk_y];
				if (vertices.Count > 0)
				{
					if (mesh == null)
					{
						mesh = new Mesh();
						mesh.name = "BlockTile";
						this.meshChunks[chunk_x, chunk_y] = mesh;
					}
					mesh.Clear();
					mesh.SetVertices(vertices);
					mesh.SetUVs(0, uvs);
					mesh.SetColors(colours);
					mesh.SetTriangles(indices, 0);
				}
				else if (mesh != null)
				{
					this.meshChunks[chunk_x, chunk_y] = null;
				}
				if (this.decorRenderInfo != null)
				{
					this.decorRenderInfo.Rebuild(renderer, this.occupiedCells, chunk_x, chunk_y, this.decorZOffset, 16, vertices, uvs, colours, indices, this.element);
				}
			}

			// Token: 0x0600ADD5 RID: 44501 RVA: 0x003AA7B4 File Offset: 0x003A89B4
			private void AddVertexInfo(BlockTileRenderer.RenderInfo.AtlasInfo atlas_info, Vector2 uv_trim_size, int x, int y, BlockTileRenderer.Bits connection_bits, Color color, List<Vector3> vertices, List<Vector2> uvs, List<int> indices, List<Color> colours)
			{
				Vector2 vector = new Vector2((float)x, (float)y);
				Vector2 vector2 = vector + new Vector2(1f, 1f);
				Vector2 vector3 = new Vector2(atlas_info.uvBox.x, atlas_info.uvBox.w);
				Vector2 vector4 = new Vector2(atlas_info.uvBox.z, atlas_info.uvBox.y);
				if ((connection_bits & BlockTileRenderer.Bits.Left) == (BlockTileRenderer.Bits)0)
				{
					vector.x -= 0.25f;
				}
				else
				{
					vector3.x += uv_trim_size.x;
				}
				if ((connection_bits & BlockTileRenderer.Bits.Right) == (BlockTileRenderer.Bits)0)
				{
					vector2.x += 0.25f;
				}
				else
				{
					vector4.x -= uv_trim_size.x;
				}
				if ((connection_bits & BlockTileRenderer.Bits.Up) == (BlockTileRenderer.Bits)0)
				{
					vector2.y += 0.25f;
				}
				else
				{
					vector4.y -= uv_trim_size.y;
				}
				if ((connection_bits & BlockTileRenderer.Bits.Down) == (BlockTileRenderer.Bits)0)
				{
					vector.y -= 0.25f;
				}
				else
				{
					vector3.y += uv_trim_size.y;
				}
				int count = vertices.Count;
				vertices.Add(vector);
				vertices.Add(new Vector2(vector2.x, vector.y));
				vertices.Add(vector2);
				vertices.Add(new Vector2(vector.x, vector2.y));
				uvs.Add(vector3);
				uvs.Add(new Vector2(vector4.x, vector3.y));
				uvs.Add(vector4);
				uvs.Add(new Vector2(vector3.x, vector4.y));
				indices.Add(count);
				indices.Add(count + 1);
				indices.Add(count + 2);
				indices.Add(count);
				indices.Add(count + 2);
				indices.Add(count + 3);
				colours.Add(color);
				colours.Add(color);
				colours.Add(color);
				colours.Add(color);
			}

			// Token: 0x04008D14 RID: 36116
			private BlockTileRenderer.RenderInfo.AtlasInfo[] atlasInfo;

			// Token: 0x04008D15 RID: 36117
			private bool[,] dirtyChunks;

			// Token: 0x04008D16 RID: 36118
			private int queryLayer;

			// Token: 0x04008D17 RID: 36119
			private Material material;

			// Token: 0x04008D18 RID: 36120
			private int renderLayer;

			// Token: 0x04008D19 RID: 36121
			private Mesh[,] meshChunks;

			// Token: 0x04008D1A RID: 36122
			private BlockTileRenderer.DecorRenderInfo decorRenderInfo;

			// Token: 0x04008D1B RID: 36123
			private Vector2 trimUVSize;

			// Token: 0x04008D1C RID: 36124
			private Vector3 rootPosition;

			// Token: 0x04008D1D RID: 36125
			private Dictionary<int, int> occupiedCells = new Dictionary<int, int>();

			// Token: 0x04008D1E RID: 36126
			private SimHashes element;

			// Token: 0x04008D1F RID: 36127
			private float decorZOffset = -1f;

			// Token: 0x04008D20 RID: 36128
			private const float scale = 0.5f;

			// Token: 0x04008D21 RID: 36129
			private const float core_size = 256f;

			// Token: 0x04008D22 RID: 36130
			private const float trim_size = 64f;

			// Token: 0x04008D23 RID: 36131
			private const float cell_size = 1f;

			// Token: 0x04008D24 RID: 36132
			private const float world_trim_size = 0.25f;

			// Token: 0x02002664 RID: 9828
			private struct AtlasInfo
			{
				// Token: 0x0400AA7F RID: 43647
				public BlockTileRenderer.Bits requiredConnections;

				// Token: 0x0400AA80 RID: 43648
				public BlockTileRenderer.Bits forbiddenConnections;

				// Token: 0x0400AA81 RID: 43649
				public Vector4 uvBox;

				// Token: 0x0400AA82 RID: 43650
				public string name;
			}
		}

		// Token: 0x02001F48 RID: 8008
		private class DecorRenderInfo
		{
			// Token: 0x0600ADD6 RID: 44502 RVA: 0x003AA9C4 File Offset: 0x003A8BC4
			public DecorRenderInfo(int num_x_chunks, int num_y_chunks, int query_layer, BuildingDef def, BlockTileDecorInfo decorInfo)
			{
				this.decorInfo = decorInfo;
				this.queryLayer = query_layer;
				this.material = new Material(def.BlockTileMaterial);
				if (def.BlockTileIsTransparent)
				{
					this.material.renderQueue = RenderQueues.Liquid;
				}
				else if (def.SceneLayer == Grid.SceneLayer.TileMain)
				{
					this.material.renderQueue = RenderQueues.BlockTiles;
				}
				this.material.SetTexture("_MainTex", decorInfo.atlas.texture);
				if (decorInfo.atlasSpec != null)
				{
					this.material.SetTexture("_SpecularTex", decorInfo.atlasSpec.texture);
					this.material.EnableKeyword("ENABLE_SHINE");
				}
				else
				{
					this.material.DisableKeyword("ENABLE_SHINE");
				}
				this.meshChunks = new Mesh[num_x_chunks, num_y_chunks];
			}

			// Token: 0x0600ADD7 RID: 44503 RVA: 0x003AAAB0 File Offset: 0x003A8CB0
			public void FreeResources()
			{
				this.decorInfo = null;
				UnityEngine.Object.DestroyImmediate(this.material);
				this.material = null;
				for (int i = 0; i < this.meshChunks.GetLength(0); i++)
				{
					for (int j = 0; j < this.meshChunks.GetLength(1); j++)
					{
						if (this.meshChunks[i, j] != null)
						{
							UnityEngine.Object.DestroyImmediate(this.meshChunks[i, j]);
							this.meshChunks[i, j] = null;
						}
					}
				}
				this.meshChunks = null;
				this.triangles.Clear();
			}

			// Token: 0x0600ADD8 RID: 44504 RVA: 0x003AAB4B File Offset: 0x003A8D4B
			public void Render(int x, int y, Vector3 position, int renderLayer)
			{
				if (this.meshChunks[x, y] != null)
				{
					Graphics.DrawMesh(this.meshChunks[x, y], position, Quaternion.identity, this.material, renderLayer);
				}
			}

			// Token: 0x0600ADD9 RID: 44505 RVA: 0x003AAB84 File Offset: 0x003A8D84
			public void Rebuild(BlockTileRenderer renderer, Dictionary<int, int> occupiedCells, int chunk_x, int chunk_y, float z_offset, int chunkEdgeSize, List<Vector3> vertices, List<Vector2> uvs, List<Color> colours, List<int> indices, SimHashes element)
			{
				vertices.Clear();
				uvs.Clear();
				this.triangles.Clear();
				colours.Clear();
				indices.Clear();
				for (int i = chunk_y * chunkEdgeSize; i < chunk_y * chunkEdgeSize + chunkEdgeSize; i++)
				{
					for (int j = chunk_x * chunkEdgeSize; j < chunk_x * chunkEdgeSize + chunkEdgeSize; j++)
					{
						int num = i * Grid.WidthInCells + j;
						if (occupiedCells.ContainsKey(num))
						{
							Color cellColour = renderer.GetCellColour(num, element);
							BlockTileRenderer.Bits decorConnectionBits = renderer.GetDecorConnectionBits(j, i, this.queryLayer);
							this.AddDecor(j, i, z_offset, decorConnectionBits, cellColour, vertices, uvs, this.triangles, colours);
						}
					}
				}
				if (vertices.Count > 0)
				{
					Mesh mesh = this.meshChunks[chunk_x, chunk_y];
					if (mesh == null)
					{
						mesh = new Mesh();
						mesh.name = "DecorRender";
						this.meshChunks[chunk_x, chunk_y] = mesh;
					}
					this.triangles.Sort((BlockTileRenderer.DecorRenderInfo.TriangleInfo a, BlockTileRenderer.DecorRenderInfo.TriangleInfo b) => a.sortOrder.CompareTo(b.sortOrder));
					for (int k = 0; k < this.triangles.Count; k++)
					{
						indices.Add(this.triangles[k].i0);
						indices.Add(this.triangles[k].i1);
						indices.Add(this.triangles[k].i2);
					}
					mesh.Clear();
					mesh.SetVertices(vertices);
					mesh.SetUVs(0, uvs);
					mesh.SetColors(colours);
					mesh.SetTriangles(indices, 0);
					return;
				}
				this.meshChunks[chunk_x, chunk_y] = null;
			}

			// Token: 0x0600ADDA RID: 44506 RVA: 0x003AAD40 File Offset: 0x003A8F40
			private void AddDecor(int x, int y, float z_offset, BlockTileRenderer.Bits connection_bits, Color colour, List<Vector3> vertices, List<Vector2> uvs, List<BlockTileRenderer.DecorRenderInfo.TriangleInfo> triangles, List<Color> colours)
			{
				for (int i = 0; i < this.decorInfo.decor.Length; i++)
				{
					BlockTileDecorInfo.Decor decor = this.decorInfo.decor[i];
					if (decor.variants != null && decor.variants.Length != 0)
					{
						bool flag = (connection_bits & decor.requiredConnections) == decor.requiredConnections;
						bool flag2 = (connection_bits & decor.forbiddenConnections) > (BlockTileRenderer.Bits)0;
						if (flag && !flag2)
						{
							float num = PerlinSimplexNoise.noise((float)(i + x + connection_bits) * BlockTileRenderer.DecorRenderInfo.simplex_scale.x, (float)(i + y + connection_bits) * BlockTileRenderer.DecorRenderInfo.simplex_scale.y);
							if (num >= decor.probabilityCutoff)
							{
								int num2 = (int)((float)(decor.variants.Length - 1) * num);
								int count = vertices.Count;
								Vector3 b = new Vector3((float)x, (float)y, z_offset) + decor.variants[num2].offset;
								foreach (Vector3 a in decor.variants[num2].atlasItem.vertices)
								{
									vertices.Add(a + b);
									colours.Add(colour);
								}
								uvs.AddRange(decor.variants[num2].atlasItem.uvs);
								int[] indices = decor.variants[num2].atlasItem.indices;
								for (int k = 0; k < indices.Length; k += 3)
								{
									triangles.Add(new BlockTileRenderer.DecorRenderInfo.TriangleInfo
									{
										sortOrder = decor.sortOrder,
										i0 = indices[k] + count,
										i1 = indices[k + 1] + count,
										i2 = indices[k + 2] + count
									});
								}
							}
						}
					}
				}
			}

			// Token: 0x04008D25 RID: 36133
			private int queryLayer;

			// Token: 0x04008D26 RID: 36134
			private BlockTileDecorInfo decorInfo;

			// Token: 0x04008D27 RID: 36135
			private Mesh[,] meshChunks;

			// Token: 0x04008D28 RID: 36136
			private Material material;

			// Token: 0x04008D29 RID: 36137
			private List<BlockTileRenderer.DecorRenderInfo.TriangleInfo> triangles = new List<BlockTileRenderer.DecorRenderInfo.TriangleInfo>();

			// Token: 0x04008D2A RID: 36138
			private static Vector2 simplex_scale = new Vector2(92.41f, 87.16f);

			// Token: 0x02002665 RID: 9829
			private struct TriangleInfo
			{
				// Token: 0x0400AA83 RID: 43651
				public int sortOrder;

				// Token: 0x0400AA84 RID: 43652
				public int i0;

				// Token: 0x0400AA85 RID: 43653
				public int i1;

				// Token: 0x0400AA86 RID: 43654
				public int i2;
			}
		}
	}
}
