using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x02000E16 RID: 3606
	public class Brush
	{
		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x060072F5 RID: 29429 RVA: 0x002C15D0 File Offset: 0x002BF7D0
		// (set) Token: 0x060072F6 RID: 29430 RVA: 0x002C15D8 File Offset: 0x002BF7D8
		public int Id { get; private set; }

		// Token: 0x060072F7 RID: 29431 RVA: 0x002C15E4 File Offset: 0x002BF7E4
		public Brush(int id, string name, Material material, Mask mask, List<Brush> active_brushes, List<Brush> dirty_brushes, int width_in_tiles, MaterialPropertyBlock property_block)
		{
			this.Id = id;
			this.material = material;
			this.mask = mask;
			this.mesh = new DynamicMesh(name, new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, 0f)));
			this.activeBrushes = active_brushes;
			this.dirtyBrushes = dirty_brushes;
			this.layer = LayerMask.NameToLayer("World");
			this.widthInTiles = width_in_tiles;
			this.propertyBlock = property_block;
		}

		// Token: 0x060072F8 RID: 29432 RVA: 0x002C1672 File Offset: 0x002BF872
		public void Add(int tile_idx)
		{
			this.tiles.Add(tile_idx);
			if (!this.dirty)
			{
				this.dirtyBrushes.Add(this);
				this.dirty = true;
			}
		}

		// Token: 0x060072F9 RID: 29433 RVA: 0x002C169C File Offset: 0x002BF89C
		public void Remove(int tile_idx)
		{
			this.tiles.Remove(tile_idx);
			if (!this.dirty)
			{
				this.dirtyBrushes.Add(this);
				this.dirty = true;
			}
		}

		// Token: 0x060072FA RID: 29434 RVA: 0x002C16C6 File Offset: 0x002BF8C6
		public void SetMaskOffset(int offset)
		{
			this.mask.SetOffset(offset);
		}

		// Token: 0x060072FB RID: 29435 RVA: 0x002C16D4 File Offset: 0x002BF8D4
		public void Refresh()
		{
			bool flag = this.mesh.Meshes.Length != 0;
			int count = this.tiles.Count;
			int vertex_count = count * 4;
			int triangle_count = count * 6;
			this.mesh.Reserve(vertex_count, triangle_count);
			if (this.mesh.SetTriangles)
			{
				int num = 0;
				for (int i = 0; i < count; i++)
				{
					this.mesh.AddTriangle(num);
					this.mesh.AddTriangle(2 + num);
					this.mesh.AddTriangle(1 + num);
					this.mesh.AddTriangle(1 + num);
					this.mesh.AddTriangle(2 + num);
					this.mesh.AddTriangle(3 + num);
					num += 4;
				}
			}
			foreach (int num2 in this.tiles)
			{
				float num3 = (float)(num2 % this.widthInTiles);
				float num4 = (float)(num2 / this.widthInTiles);
				float z = 0f;
				this.mesh.AddVertex(new Vector3(num3 - 0.5f, num4 - 0.5f, z));
				this.mesh.AddVertex(new Vector3(num3 + 0.5f, num4 - 0.5f, z));
				this.mesh.AddVertex(new Vector3(num3 - 0.5f, num4 + 0.5f, z));
				this.mesh.AddVertex(new Vector3(num3 + 0.5f, num4 + 0.5f, z));
			}
			if (this.mesh.SetUVs)
			{
				for (int j = 0; j < count; j++)
				{
					this.mesh.AddUV(this.mask.UV0);
					this.mesh.AddUV(this.mask.UV1);
					this.mesh.AddUV(this.mask.UV2);
					this.mesh.AddUV(this.mask.UV3);
				}
			}
			this.dirty = false;
			this.mesh.Commit();
			if (this.mesh.Meshes.Length != 0)
			{
				if (!flag)
				{
					this.activeBrushes.Add(this);
					return;
				}
			}
			else if (flag)
			{
				this.activeBrushes.Remove(this);
			}
		}

		// Token: 0x060072FC RID: 29436 RVA: 0x002C1930 File Offset: 0x002BFB30
		public void Render()
		{
			Vector3 position = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.Ground));
			this.mesh.Render(position, Quaternion.identity, this.material, this.layer, this.propertyBlock);
		}

		// Token: 0x060072FD RID: 29437 RVA: 0x002C1978 File Offset: 0x002BFB78
		public void SetMaterial(Material material, MaterialPropertyBlock property_block)
		{
			this.material = material;
			this.propertyBlock = property_block;
		}

		// Token: 0x04004F1A RID: 20250
		private bool dirty;

		// Token: 0x04004F1B RID: 20251
		private Material material;

		// Token: 0x04004F1C RID: 20252
		private int layer;

		// Token: 0x04004F1D RID: 20253
		private HashSet<int> tiles = new HashSet<int>();

		// Token: 0x04004F1E RID: 20254
		private List<Brush> activeBrushes;

		// Token: 0x04004F1F RID: 20255
		private List<Brush> dirtyBrushes;

		// Token: 0x04004F20 RID: 20256
		private int widthInTiles;

		// Token: 0x04004F21 RID: 20257
		private Mask mask;

		// Token: 0x04004F22 RID: 20258
		private DynamicMesh mesh;

		// Token: 0x04004F23 RID: 20259
		private MaterialPropertyBlock propertyBlock;
	}
}
