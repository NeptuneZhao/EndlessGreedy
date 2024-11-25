using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000B5A RID: 2906
[AddComponentMenu("KMonoBehaviour/scripts/WaterCubes")]
public class WaterCubes : KMonoBehaviour
{
	// Token: 0x17000671 RID: 1649
	// (get) Token: 0x060056D9 RID: 22233 RVA: 0x001F08A0 File Offset: 0x001EEAA0
	// (set) Token: 0x060056DA RID: 22234 RVA: 0x001F08A7 File Offset: 0x001EEAA7
	public static WaterCubes Instance { get; private set; }

	// Token: 0x060056DB RID: 22235 RVA: 0x001F08AF File Offset: 0x001EEAAF
	public static void DestroyInstance()
	{
		WaterCubes.Instance = null;
	}

	// Token: 0x060056DC RID: 22236 RVA: 0x001F08B7 File Offset: 0x001EEAB7
	protected override void OnPrefabInit()
	{
		WaterCubes.Instance = this;
	}

	// Token: 0x060056DD RID: 22237 RVA: 0x001F08C0 File Offset: 0x001EEAC0
	public void Init()
	{
		this.cubes = Util.NewGameObject(base.gameObject, "WaterCubes");
		GameObject gameObject = new GameObject();
		gameObject.name = "WaterCubesMesh";
		gameObject.transform.parent = this.cubes.transform;
		this.material.renderQueue = RenderQueues.Liquid;
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		meshRenderer.sharedMaterial = this.material;
		meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		meshRenderer.receiveShadows = false;
		meshRenderer.lightProbeUsage = LightProbeUsage.Off;
		meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
		meshRenderer.sharedMaterial.SetTexture("_MainTex2", this.waveTexture);
		meshFilter.sharedMesh = this.CreateNewMesh();
		meshRenderer.gameObject.layer = 0;
		meshRenderer.gameObject.transform.parent = base.transform;
		meshRenderer.gameObject.transform.SetPosition(new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.Liquid)));
	}

	// Token: 0x060056DE RID: 22238 RVA: 0x001F09B8 File Offset: 0x001EEBB8
	private Mesh CreateNewMesh()
	{
		Mesh mesh = new Mesh();
		mesh.name = "WaterCubes";
		int num = 4;
		Vector3[] vertices = new Vector3[num];
		Vector2[] array = new Vector2[num];
		Vector3[] normals = new Vector3[num];
		Vector4[] tangents = new Vector4[num];
		int[] triangles = new int[6];
		float layerZ = Grid.GetLayerZ(Grid.SceneLayer.Liquid);
		vertices = new Vector3[]
		{
			new Vector3(0f, 0f, layerZ),
			new Vector3((float)Grid.WidthInCells, 0f, layerZ),
			new Vector3(0f, Grid.HeightInMeters, layerZ),
			new Vector3(Grid.WidthInMeters, Grid.HeightInMeters, layerZ)
		};
		array = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		normals = new Vector3[]
		{
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, 0f, -1f)
		};
		tangents = new Vector4[]
		{
			new Vector4(0f, 1f, 0f, -1f),
			new Vector4(0f, 1f, 0f, -1f),
			new Vector4(0f, 1f, 0f, -1f),
			new Vector4(0f, 1f, 0f, -1f)
		};
		triangles = new int[]
		{
			0,
			2,
			1,
			1,
			2,
			3
		};
		mesh.vertices = vertices;
		mesh.uv = array;
		mesh.uv2 = array;
		mesh.normals = normals;
		mesh.tangents = tangents;
		mesh.triangles = triangles;
		mesh.bounds = new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, 0f));
		return mesh;
	}

	// Token: 0x040038EC RID: 14572
	public Material material;

	// Token: 0x040038ED RID: 14573
	public Texture2D waveTexture;

	// Token: 0x040038EE RID: 14574
	private GameObject cubes;
}
