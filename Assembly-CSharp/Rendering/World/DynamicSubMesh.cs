using System;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x02000E18 RID: 3608
	public class DynamicSubMesh
	{
		// Token: 0x06007306 RID: 29446 RVA: 0x002C1C1C File Offset: 0x002BFE1C
		public DynamicSubMesh(string name, Bounds bounds, int idx_offset)
		{
			this.IdxOffset = idx_offset;
			this.Mesh = new Mesh();
			this.Mesh.name = name;
			this.Mesh.bounds = bounds;
			this.Mesh.MarkDynamic();
		}

		// Token: 0x06007307 RID: 29447 RVA: 0x002C1C88 File Offset: 0x002BFE88
		public void Reserve(int vertex_count, int triangle_count)
		{
			if (vertex_count > this.Vertices.Length)
			{
				this.Vertices = new Vector3[vertex_count];
				this.UVs = new Vector2[vertex_count];
				this.SetUVs = true;
			}
			else
			{
				this.SetUVs = false;
			}
			if (this.Triangles.Length != triangle_count)
			{
				this.Triangles = new int[triangle_count];
				this.SetTriangles = true;
				return;
			}
			this.SetTriangles = false;
		}

		// Token: 0x06007308 RID: 29448 RVA: 0x002C1CEE File Offset: 0x002BFEEE
		public bool AreTrianglesFull()
		{
			return this.Triangles.Length == this.TriangleIdx;
		}

		// Token: 0x06007309 RID: 29449 RVA: 0x002C1D00 File Offset: 0x002BFF00
		public bool AreVerticesFull()
		{
			return this.Vertices.Length == this.VertexIdx;
		}

		// Token: 0x0600730A RID: 29450 RVA: 0x002C1D12 File Offset: 0x002BFF12
		public bool AreUVsFull()
		{
			return this.UVs.Length == this.UVIdx;
		}

		// Token: 0x0600730B RID: 29451 RVA: 0x002C1D24 File Offset: 0x002BFF24
		public void Commit()
		{
			if (this.SetTriangles)
			{
				this.Mesh.Clear();
			}
			this.Mesh.vertices = this.Vertices;
			if (this.SetUVs || this.SetTriangles)
			{
				this.Mesh.uv = this.UVs;
			}
			if (this.SetTriangles)
			{
				this.Mesh.triangles = this.Triangles;
			}
			this.VertexIdx = 0;
			this.UVIdx = 0;
			this.TriangleIdx = 0;
		}

		// Token: 0x0600730C RID: 29452 RVA: 0x002C1DA4 File Offset: 0x002BFFA4
		public void AddTriangle(int triangle)
		{
			int[] triangles = this.Triangles;
			int triangleIdx = this.TriangleIdx;
			this.TriangleIdx = triangleIdx + 1;
			triangles[triangleIdx] = triangle + this.IdxOffset;
		}

		// Token: 0x0600730D RID: 29453 RVA: 0x002C1DD4 File Offset: 0x002BFFD4
		public void AddUV(Vector2 uv)
		{
			Vector2[] uvs = this.UVs;
			int uvidx = this.UVIdx;
			this.UVIdx = uvidx + 1;
			uvs[uvidx] = uv;
		}

		// Token: 0x0600730E RID: 29454 RVA: 0x002C1E00 File Offset: 0x002C0000
		public void AddVertex(Vector3 vertex)
		{
			Vector3[] vertices = this.Vertices;
			int vertexIdx = this.VertexIdx;
			this.VertexIdx = vertexIdx + 1;
			vertices[vertexIdx] = vertex;
		}

		// Token: 0x0600730F RID: 29455 RVA: 0x002C1E2C File Offset: 0x002C002C
		public void Render(Vector3 position, Quaternion rotation, Material material, int layer, MaterialPropertyBlock property_block)
		{
			Graphics.DrawMesh(this.Mesh, position, rotation, material, layer, null, 0, property_block, false, false);
		}

		// Token: 0x04004F33 RID: 20275
		public Vector3[] Vertices = new Vector3[0];

		// Token: 0x04004F34 RID: 20276
		public Vector2[] UVs = new Vector2[0];

		// Token: 0x04004F35 RID: 20277
		public int[] Triangles = new int[0];

		// Token: 0x04004F36 RID: 20278
		public Mesh Mesh;

		// Token: 0x04004F37 RID: 20279
		public bool SetUVs;

		// Token: 0x04004F38 RID: 20280
		public bool SetTriangles;

		// Token: 0x04004F39 RID: 20281
		private int VertexIdx;

		// Token: 0x04004F3A RID: 20282
		private int UVIdx;

		// Token: 0x04004F3B RID: 20283
		private int TriangleIdx;

		// Token: 0x04004F3C RID: 20284
		private int IdxOffset;
	}
}
