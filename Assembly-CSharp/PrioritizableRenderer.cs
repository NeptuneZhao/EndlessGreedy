﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005A4 RID: 1444
public class PrioritizableRenderer
{
	// Token: 0x17000184 RID: 388
	// (get) Token: 0x0600225D RID: 8797 RVA: 0x000BF227 File Offset: 0x000BD427
	// (set) Token: 0x0600225E RID: 8798 RVA: 0x000BF22F File Offset: 0x000BD42F
	public PrioritizeTool currentTool
	{
		get
		{
			return this.tool;
		}
		set
		{
			this.tool = value;
		}
	}

	// Token: 0x0600225F RID: 8799 RVA: 0x000BF238 File Offset: 0x000BD438
	public PrioritizableRenderer()
	{
		this.layer = LayerMask.NameToLayer("UI");
		Shader shader = Shader.Find("Klei/Prioritizable");
		Texture2D texture = Assets.GetTexture("priority_overlay_atlas");
		this.material = new Material(shader);
		this.material.SetTexture(Shader.PropertyToID("_MainTex"), texture);
		this.prioritizables = new List<Prioritizable>();
		this.mesh = new Mesh();
		this.mesh.name = "Prioritizables";
		this.mesh.MarkDynamic();
	}

	// Token: 0x06002260 RID: 8800 RVA: 0x000BF2C4 File Offset: 0x000BD4C4
	public void Cleanup()
	{
		this.material = null;
		this.vertices = null;
		this.uvs = null;
		this.prioritizables = null;
		this.triangles = null;
		UnityEngine.Object.DestroyImmediate(this.mesh);
		this.mesh = null;
	}

	// Token: 0x06002261 RID: 8801 RVA: 0x000BF2FC File Offset: 0x000BD4FC
	public void RenderEveryTick()
	{
		using (new KProfiler.Region("PrioritizableRenderer", null))
		{
			if (!(GameScreenManager.Instance == null))
			{
				if (!(SimDebugView.Instance == null) && !(SimDebugView.Instance.GetMode() != OverlayModes.Priorities.ID))
				{
					this.prioritizables.Clear();
					Vector2I vector2I;
					Vector2I vector2I2;
					Grid.GetVisibleExtents(out vector2I, out vector2I2);
					int height = vector2I2.y - vector2I.y;
					int width = vector2I2.x - vector2I.x;
					Extents extents = new Extents(vector2I.x, vector2I.y, width, height);
					List<ScenePartitionerEntry> list = new List<ScenePartitionerEntry>();
					GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.prioritizableObjects, list);
					foreach (ScenePartitionerEntry scenePartitionerEntry in list)
					{
						Prioritizable prioritizable = (Prioritizable)scenePartitionerEntry.obj;
						if (prioritizable != null && prioritizable.showIcon && prioritizable.IsPrioritizable() && this.tool.IsActiveLayer(this.tool.GetFilterLayerFromGameObject(prioritizable.gameObject)) && prioritizable.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
						{
							this.prioritizables.Add(prioritizable);
						}
					}
					if (this.prioritizableCount != this.prioritizables.Count)
					{
						this.prioritizableCount = this.prioritizables.Count;
						this.vertices = new Vector3[4 * this.prioritizableCount];
						this.uvs = new Vector2[4 * this.prioritizableCount];
						this.triangles = new int[6 * this.prioritizableCount];
					}
					if (this.prioritizableCount != 0)
					{
						for (int i = 0; i < this.prioritizables.Count; i++)
						{
							Prioritizable prioritizable2 = this.prioritizables[i];
							Vector3 vector = Vector3.zero;
							KAnimControllerBase component = prioritizable2.GetComponent<KAnimControllerBase>();
							if (component != null)
							{
								vector = component.GetWorldPivot();
							}
							else
							{
								vector = prioritizable2.transform.GetPosition();
							}
							vector.x += prioritizable2.iconOffset.x;
							vector.y += prioritizable2.iconOffset.y;
							Vector2 vector2 = new Vector2(0.2f, 0.3f) * prioritizable2.iconScale;
							float z = -5f;
							int num = 4 * i;
							this.vertices[num] = new Vector3(vector.x - vector2.x, vector.y - vector2.y, z);
							this.vertices[1 + num] = new Vector3(vector.x - vector2.x, vector.y + vector2.y, z);
							this.vertices[2 + num] = new Vector3(vector.x + vector2.x, vector.y - vector2.y, z);
							this.vertices[3 + num] = new Vector3(vector.x + vector2.x, vector.y + vector2.y, z);
							float num2 = 0.1f;
							PrioritySetting masterPriority = prioritizable2.GetMasterPriority();
							float num3 = -1f;
							if (masterPriority.priority_class >= PriorityScreen.PriorityClass.high)
							{
								num3 += 9f;
							}
							if (masterPriority.priority_class >= PriorityScreen.PriorityClass.topPriority)
							{
								num3 += 0f;
							}
							num3 += (float)masterPriority.priority_value;
							float num4 = num2 * num3;
							float num5 = 0f;
							float num6 = num2;
							float num7 = 1f;
							this.uvs[num] = new Vector2(num4, num5);
							this.uvs[1 + num] = new Vector2(num4, num5 + num7);
							this.uvs[2 + num] = new Vector2(num4 + num6, num5);
							this.uvs[3 + num] = new Vector2(num4 + num6, num5 + num7);
							int num8 = 6 * i;
							this.triangles[num8] = num;
							this.triangles[1 + num8] = num + 1;
							this.triangles[2 + num8] = num + 2;
							this.triangles[3 + num8] = num + 2;
							this.triangles[4 + num8] = num + 1;
							this.triangles[5 + num8] = num + 3;
						}
						this.mesh.Clear();
						this.mesh.vertices = this.vertices;
						this.mesh.uv = this.uvs;
						this.mesh.SetTriangles(this.triangles, 0);
						this.mesh.RecalculateBounds();
						Graphics.DrawMesh(this.mesh, Vector3.zero, Quaternion.identity, this.material, this.layer, GameScreenManager.Instance.worldSpaceCanvas.GetComponent<Canvas>().worldCamera, 0, null, false, false);
					}
				}
			}
		}
	}

	// Token: 0x04001357 RID: 4951
	private Mesh mesh;

	// Token: 0x04001358 RID: 4952
	private int layer;

	// Token: 0x04001359 RID: 4953
	private Material material;

	// Token: 0x0400135A RID: 4954
	private int prioritizableCount;

	// Token: 0x0400135B RID: 4955
	private Vector3[] vertices;

	// Token: 0x0400135C RID: 4956
	private Vector2[] uvs;

	// Token: 0x0400135D RID: 4957
	private int[] triangles;

	// Token: 0x0400135E RID: 4958
	private List<Prioritizable> prioritizables;

	// Token: 0x0400135F RID: 4959
	private PrioritizeTool tool;
}
