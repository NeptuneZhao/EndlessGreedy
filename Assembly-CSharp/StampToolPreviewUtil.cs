﻿using System;
using UnityEngine;

// Token: 0x02000929 RID: 2345
public static class StampToolPreviewUtil
{
	// Token: 0x0600442B RID: 17451 RVA: 0x00182DE5 File Offset: 0x00180FE5
	public static Material MakeMaterial(Texture texture)
	{
		Material material = new Material(Shader.Find("Sprites/Default"));
		material.SetTexture("_MainTex", texture);
		return material;
	}

	// Token: 0x0600442C RID: 17452 RVA: 0x00182E04 File Offset: 0x00181004
	public static void MakeQuad(out GameObject gameObject, out MeshRenderer meshRenderer, float mesh_size, Vector4? uvBox = null)
	{
		gameObject = new GameObject();
		gameObject.layer = LayerMask.NameToLayer("Place");
		float num = mesh_size / 2f;
		float num2 = mesh_size / 2f;
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[]
		{
			new Vector3(-num, -num2, 0f),
			new Vector3(num, -num2, 0f),
			new Vector3(-num, num2, 0f),
			new Vector3(num, num2, 0f)
		};
		mesh.triangles = new int[]
		{
			0,
			2,
			1,
			2,
			3,
			1
		};
		mesh.normals = new Vector3[]
		{
			-Vector3.forward,
			-Vector3.forward,
			-Vector3.forward,
			-Vector3.forward
		};
		Mesh mesh2 = mesh;
		Vector2[] uv;
		if (uvBox != null)
		{
			Vector2[] array = new Vector2[4];
			array[0] = new Vector2(uvBox.Value.x, uvBox.Value.w);
			array[1] = new Vector2(uvBox.Value.z, uvBox.Value.w);
			array[2] = new Vector2(uvBox.Value.x, uvBox.Value.y);
			uv = array;
			array[3] = new Vector2(uvBox.Value.z, uvBox.Value.y);
		}
		else
		{
			Vector2[] array2 = new Vector2[4];
			array2[0] = new Vector2(0f, 0f);
			array2[1] = new Vector2(1f, 0f);
			array2[2] = new Vector2(0f, 1f);
			uv = array2;
			array2[3] = new Vector2(1f, 1f);
		}
		mesh2.uv = uv;
		Mesh mesh3 = mesh;
		gameObject.AddComponent<MeshFilter>().mesh = mesh3;
		meshRenderer = gameObject.AddComponent<MeshRenderer>();
	}

	// Token: 0x04002CA6 RID: 11430
	public static readonly Color COLOR_OK = Color.white;

	// Token: 0x04002CA7 RID: 11431
	public static readonly Color COLOR_ERROR = Color.red;

	// Token: 0x04002CA8 RID: 11432
	public const float SOLID_VIS_ALPHA = 1f;

	// Token: 0x04002CA9 RID: 11433
	public const float LIQUID_VIS_ALPHA = 1f;

	// Token: 0x04002CAA RID: 11434
	public const float GAS_VIS_ALPHA = 1f;

	// Token: 0x04002CAB RID: 11435
	public const float BACKGROUND_ALPHA = 1f;
}
