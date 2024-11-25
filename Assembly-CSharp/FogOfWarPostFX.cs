using System;
using UnityEngine;

// Token: 0x02000A00 RID: 2560
public class FogOfWarPostFX : MonoBehaviour
{
	// Token: 0x06004A24 RID: 18980 RVA: 0x001A7B80 File Offset: 0x001A5D80
	private void Awake()
	{
		if (this.shader != null)
		{
			this.material = new Material(this.shader);
		}
	}

	// Token: 0x06004A25 RID: 18981 RVA: 0x001A7BA1 File Offset: 0x001A5DA1
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.SetupUVs();
		Graphics.Blit(source, destination, this.material, 0);
	}

	// Token: 0x06004A26 RID: 18982 RVA: 0x001A7BB8 File Offset: 0x001A5DB8
	private void SetupUVs()
	{
		if (this.myCamera == null)
		{
			this.myCamera = base.GetComponent<Camera>();
			if (this.myCamera == null)
			{
				return;
			}
		}
		Ray ray = this.myCamera.ViewportPointToRay(Vector3.zero);
		float distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		Vector3 point = ray.GetPoint(distance);
		Vector4 vector;
		vector.x = point.x / Grid.WidthInMeters;
		vector.y = point.y / Grid.HeightInMeters;
		ray = this.myCamera.ViewportPointToRay(Vector3.one);
		distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		point = ray.GetPoint(distance);
		vector.z = point.x / Grid.WidthInMeters - vector.x;
		vector.w = point.y / Grid.HeightInMeters - vector.y;
		this.material.SetVector("_UVOffsetScale", vector);
	}

	// Token: 0x040030A4 RID: 12452
	[SerializeField]
	private Shader shader;

	// Token: 0x040030A5 RID: 12453
	private Material material;

	// Token: 0x040030A6 RID: 12454
	private Camera myCamera;
}
