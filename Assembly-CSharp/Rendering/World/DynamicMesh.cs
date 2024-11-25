using System;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x02000E17 RID: 3607
	public class DynamicMesh
	{
		// Token: 0x060072FE RID: 29438 RVA: 0x002C1988 File Offset: 0x002BFB88
		public DynamicMesh(string name, Bounds bounds)
		{
			this.Name = name;
			this.Bounds = bounds;
		}

		// Token: 0x060072FF RID: 29439 RVA: 0x002C19AC File Offset: 0x002BFBAC
		public void Reserve(int vertex_count, int triangle_count)
		{
			if (vertex_count > this.VertexCount)
			{
				this.SetUVs = true;
			}
			else
			{
				this.SetUVs = false;
			}
			if (this.TriangleCount != triangle_count)
			{
				this.SetTriangles = true;
			}
			else
			{
				this.SetTriangles = false;
			}
			int num = (int)Mathf.Ceil((float)triangle_count / (float)DynamicMesh.TrianglesPerMesh);
			if (num != this.Meshes.Length)
			{
				this.Meshes = new DynamicSubMesh[num];
				for (int i = 0; i < this.Meshes.Length; i++)
				{
					int idx_offset = -i * DynamicMesh.VerticesPerMesh;
					this.Meshes[i] = new DynamicSubMesh(this.Name, this.Bounds, idx_offset);
				}
				this.SetUVs = true;
				this.SetTriangles = true;
			}
			for (int j = 0; j < this.Meshes.Length; j++)
			{
				if (j == this.Meshes.Length - 1)
				{
					this.Meshes[j].Reserve(vertex_count % DynamicMesh.VerticesPerMesh, triangle_count % DynamicMesh.TrianglesPerMesh);
				}
				else
				{
					this.Meshes[j].Reserve(DynamicMesh.VerticesPerMesh, DynamicMesh.TrianglesPerMesh);
				}
			}
			this.VertexCount = vertex_count;
			this.TriangleCount = triangle_count;
		}

		// Token: 0x06007300 RID: 29440 RVA: 0x002C1AB8 File Offset: 0x002BFCB8
		public void Commit()
		{
			DynamicSubMesh[] meshes = this.Meshes;
			for (int i = 0; i < meshes.Length; i++)
			{
				meshes[i].Commit();
			}
			this.TriangleMeshIdx = 0;
			this.UVMeshIdx = 0;
			this.VertexMeshIdx = 0;
		}

		// Token: 0x06007301 RID: 29441 RVA: 0x002C1AF8 File Offset: 0x002BFCF8
		public void AddTriangle(int triangle)
		{
			if (this.Meshes[this.TriangleMeshIdx].AreTrianglesFull())
			{
				DynamicSubMesh[] meshes = this.Meshes;
				int num = this.TriangleMeshIdx + 1;
				this.TriangleMeshIdx = num;
				object obj = meshes[num];
			}
			this.Meshes[this.TriangleMeshIdx].AddTriangle(triangle);
		}

		// Token: 0x06007302 RID: 29442 RVA: 0x002C1B48 File Offset: 0x002BFD48
		public void AddUV(Vector2 uv)
		{
			DynamicSubMesh dynamicSubMesh = this.Meshes[this.UVMeshIdx];
			if (dynamicSubMesh.AreUVsFull())
			{
				DynamicSubMesh[] meshes = this.Meshes;
				int num = this.UVMeshIdx + 1;
				this.UVMeshIdx = num;
				dynamicSubMesh = meshes[num];
			}
			dynamicSubMesh.AddUV(uv);
		}

		// Token: 0x06007303 RID: 29443 RVA: 0x002C1B8C File Offset: 0x002BFD8C
		public void AddVertex(Vector3 vertex)
		{
			DynamicSubMesh dynamicSubMesh = this.Meshes[this.VertexMeshIdx];
			if (dynamicSubMesh.AreVerticesFull())
			{
				DynamicSubMesh[] meshes = this.Meshes;
				int num = this.VertexMeshIdx + 1;
				this.VertexMeshIdx = num;
				dynamicSubMesh = meshes[num];
			}
			dynamicSubMesh.AddVertex(vertex);
		}

		// Token: 0x06007304 RID: 29444 RVA: 0x002C1BD0 File Offset: 0x002BFDD0
		public void Render(Vector3 position, Quaternion rotation, Material material, int layer, MaterialPropertyBlock property_block)
		{
			DynamicSubMesh[] meshes = this.Meshes;
			for (int i = 0; i < meshes.Length; i++)
			{
				meshes[i].Render(position, rotation, material, layer, property_block);
			}
		}

		// Token: 0x04004F24 RID: 20260
		private static int TrianglesPerMesh = 65004;

		// Token: 0x04004F25 RID: 20261
		private static int VerticesPerMesh = 4 * DynamicMesh.TrianglesPerMesh / 6;

		// Token: 0x04004F26 RID: 20262
		public bool SetUVs;

		// Token: 0x04004F27 RID: 20263
		public bool SetTriangles;

		// Token: 0x04004F28 RID: 20264
		public string Name;

		// Token: 0x04004F29 RID: 20265
		public Bounds Bounds;

		// Token: 0x04004F2A RID: 20266
		public DynamicSubMesh[] Meshes = new DynamicSubMesh[0];

		// Token: 0x04004F2B RID: 20267
		private int VertexCount;

		// Token: 0x04004F2C RID: 20268
		private int TriangleCount;

		// Token: 0x04004F2D RID: 20269
		private int VertexIdx;

		// Token: 0x04004F2E RID: 20270
		private int UVIdx;

		// Token: 0x04004F2F RID: 20271
		private int TriangleIdx;

		// Token: 0x04004F30 RID: 20272
		private int TriangleMeshIdx;

		// Token: 0x04004F31 RID: 20273
		private int VertexMeshIdx;

		// Token: 0x04004F32 RID: 20274
		private int UVMeshIdx;
	}
}
