using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A36 RID: 2614
public class LightBuffer : MonoBehaviour
{
	// Token: 0x06004BD2 RID: 19410 RVA: 0x001AFFEC File Offset: 0x001AE1EC
	private void Awake()
	{
		LightBuffer.Instance = this;
		this.ColorRangeTag = Shader.PropertyToID("_ColorRange");
		this.LightPosTag = Shader.PropertyToID("_LightPos");
		this.LightDirectionAngleTag = Shader.PropertyToID("_LightDirectionAngle");
		this.TintColorTag = Shader.PropertyToID("_TintColor");
		this.Camera = base.GetComponent<Camera>();
		this.Layer = LayerMask.NameToLayer("Lights");
		this.Mesh = new Mesh();
		this.Mesh.name = "Light Mesh";
		this.Mesh.vertices = new Vector3[]
		{
			new Vector3(-1f, -1f, 0f),
			new Vector3(-1f, 1f, 0f),
			new Vector3(1f, -1f, 0f),
			new Vector3(1f, 1f, 0f)
		};
		this.Mesh.uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 0f),
			new Vector2(1f, 1f)
		};
		this.Mesh.triangles = new int[]
		{
			0,
			1,
			2,
			2,
			1,
			3
		};
		this.Mesh.bounds = new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));
		this.Texture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBHalf);
		this.Texture.name = "LightBuffer";
		this.Camera.targetTexture = this.Texture;
	}

	// Token: 0x06004BD3 RID: 19411 RVA: 0x001B01DC File Offset: 0x001AE3DC
	private void LateUpdate()
	{
		if (PropertyTextures.instance == null)
		{
			return;
		}
		if (this.Texture.width != Screen.width || this.Texture.height != Screen.height)
		{
			this.Texture.DestroyRenderTexture();
			this.Texture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBHalf);
			this.Texture.name = "LightBuffer";
			this.Camera.targetTexture = this.Texture;
		}
		Matrix4x4 matrix = default(Matrix4x4);
		this.WorldLight = PropertyTextures.instance.GetTexture(PropertyTextures.Property.WorldLight);
		this.Material.SetTexture("_PropertyWorldLight", this.WorldLight);
		this.CircleMaterial.SetTexture("_PropertyWorldLight", this.WorldLight);
		this.ConeMaterial.SetTexture("_PropertyWorldLight", this.WorldLight);
		List<Light2D> list = Components.Light2Ds.Items;
		if (ClusterManager.Instance != null)
		{
			list = Components.Light2Ds.GetWorldItems(ClusterManager.Instance.activeWorldId, false);
		}
		if (list == null)
		{
			return;
		}
		foreach (Light2D light2D in list)
		{
			if (!(light2D == null) && light2D.enabled)
			{
				MaterialPropertyBlock materialPropertyBlock = light2D.materialPropertyBlock;
				materialPropertyBlock.SetVector(this.ColorRangeTag, new Vector4(light2D.Color.r * light2D.IntensityAnimation, light2D.Color.g * light2D.IntensityAnimation, light2D.Color.b * light2D.IntensityAnimation, light2D.Range));
				Vector3 position = light2D.transform.GetPosition();
				position.x += light2D.Offset.x;
				position.y += light2D.Offset.y;
				materialPropertyBlock.SetVector(this.LightPosTag, new Vector4(position.x, position.y, 0f, 0f));
				Vector2 normalized = light2D.Direction.normalized;
				materialPropertyBlock.SetVector(this.LightDirectionAngleTag, new Vector4(normalized.x, normalized.y, 0f, light2D.Angle));
				Graphics.DrawMesh(this.Mesh, Vector3.zero, Quaternion.identity, this.Material, this.Layer, this.Camera, 0, materialPropertyBlock, false, false);
				if (light2D.drawOverlay)
				{
					materialPropertyBlock.SetColor(this.TintColorTag, light2D.overlayColour);
					global::LightShape shape = light2D.shape;
					if (shape != global::LightShape.Circle)
					{
						if (shape == global::LightShape.Cone)
						{
							matrix.SetTRS(position - Vector3.up * (light2D.Range * 0.5f), Quaternion.identity, new Vector3(1f, 0.5f, 1f) * light2D.Range);
							Graphics.DrawMesh(this.Mesh, matrix, this.ConeMaterial, this.Layer, this.Camera, 0, materialPropertyBlock);
						}
					}
					else
					{
						matrix.SetTRS(position, Quaternion.identity, Vector3.one * light2D.Range);
						Graphics.DrawMesh(this.Mesh, matrix, this.CircleMaterial, this.Layer, this.Camera, 0, materialPropertyBlock);
					}
				}
			}
		}
	}

	// Token: 0x06004BD4 RID: 19412 RVA: 0x001B054C File Offset: 0x001AE74C
	private void OnDestroy()
	{
		LightBuffer.Instance = null;
	}

	// Token: 0x04003199 RID: 12697
	private Mesh Mesh;

	// Token: 0x0400319A RID: 12698
	private Camera Camera;

	// Token: 0x0400319B RID: 12699
	[NonSerialized]
	public Material Material;

	// Token: 0x0400319C RID: 12700
	[NonSerialized]
	public Material CircleMaterial;

	// Token: 0x0400319D RID: 12701
	[NonSerialized]
	public Material ConeMaterial;

	// Token: 0x0400319E RID: 12702
	private int ColorRangeTag;

	// Token: 0x0400319F RID: 12703
	private int LightPosTag;

	// Token: 0x040031A0 RID: 12704
	private int LightDirectionAngleTag;

	// Token: 0x040031A1 RID: 12705
	private int TintColorTag;

	// Token: 0x040031A2 RID: 12706
	private int Layer;

	// Token: 0x040031A3 RID: 12707
	public RenderTexture Texture;

	// Token: 0x040031A4 RID: 12708
	public Texture WorldLight;

	// Token: 0x040031A5 RID: 12709
	public static LightBuffer Instance;

	// Token: 0x040031A6 RID: 12710
	private const RenderTextureFormat RTFormat = RenderTextureFormat.ARGBHalf;
}
