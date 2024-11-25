using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02000B8D RID: 2957
public class ConduitFlowVisualizer
{
	// Token: 0x06005941 RID: 22849 RVA: 0x0020471C File Offset: 0x0020291C
	public ConduitFlowVisualizer(ConduitFlow flow_manager, Game.ConduitVisInfo vis_info, EventReference overlay_sound, ConduitFlowVisualizer.Tuning tuning)
	{
		this.flowManager = flow_manager;
		this.visInfo = vis_info;
		this.overlaySound = overlay_sound;
		this.tuning = tuning;
		this.movingBallMesh = new ConduitFlowVisualizer.ConduitFlowMesh();
		this.staticBallMesh = new ConduitFlowVisualizer.ConduitFlowMesh();
		ConduitFlowVisualizer.RenderMeshTask.Ball.InitializeResources();
	}

	// Token: 0x06005942 RID: 22850 RVA: 0x002047A8 File Offset: 0x002029A8
	public void FreeResources()
	{
		this.movingBallMesh.Cleanup();
		this.staticBallMesh.Cleanup();
	}

	// Token: 0x06005943 RID: 22851 RVA: 0x002047C0 File Offset: 0x002029C0
	private float CalculateMassScale(float mass)
	{
		float t = (mass - this.visInfo.overlayMassScaleRange.x) / (this.visInfo.overlayMassScaleRange.y - this.visInfo.overlayMassScaleRange.x);
		return Mathf.Lerp(this.visInfo.overlayMassScaleValues.x, this.visInfo.overlayMassScaleValues.y, t);
	}

	// Token: 0x06005944 RID: 22852 RVA: 0x00204828 File Offset: 0x00202A28
	private Color32 GetContentsColor(Element element, Color32 default_color)
	{
		if (element != null)
		{
			Color c = element.substance.conduitColour;
			c.a = 128f;
			return c;
		}
		return default_color;
	}

	// Token: 0x06005945 RID: 22853 RVA: 0x0020485D File Offset: 0x00202A5D
	private Color32 GetTintColour()
	{
		if (!this.showContents)
		{
			return this.visInfo.tint;
		}
		return GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayTintName);
	}

	// Token: 0x06005946 RID: 22854 RVA: 0x0020488D File Offset: 0x00202A8D
	private Color32 GetInsulatedTintColour()
	{
		if (!this.showContents)
		{
			return this.visInfo.insulatedTint;
		}
		return GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayInsulatedTintName);
	}

	// Token: 0x06005947 RID: 22855 RVA: 0x002048BD File Offset: 0x00202ABD
	private Color32 GetRadiantTintColour()
	{
		if (!this.showContents)
		{
			return this.visInfo.radiantTint;
		}
		return GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayRadiantTintName);
	}

	// Token: 0x06005948 RID: 22856 RVA: 0x002048F0 File Offset: 0x00202AF0
	private Color32 GetCellTintColour(int cell)
	{
		Color32 result;
		if (this.insulatedCells.Contains(cell))
		{
			result = this.GetInsulatedTintColour();
		}
		else if (this.radiantCells.Contains(cell))
		{
			result = this.GetRadiantTintColour();
		}
		else
		{
			result = this.GetTintColour();
		}
		return result;
	}

	// Token: 0x06005949 RID: 22857 RVA: 0x00204934 File Offset: 0x00202B34
	public void Render(float z, int render_layer, float lerp_percent, bool trigger_audio = false)
	{
		this.animTime += (double)Time.deltaTime;
		if (trigger_audio)
		{
			if (this.audioInfo == null)
			{
				this.audioInfo = new List<ConduitFlowVisualizer.AudioInfo>();
			}
			for (int i = 0; i < this.audioInfo.Count; i++)
			{
				ConduitFlowVisualizer.AudioInfo audioInfo = this.audioInfo[i];
				audioInfo.distance = float.PositiveInfinity;
				audioInfo.position = Vector3.zero;
				audioInfo.blobCount = (audioInfo.blobCount + 1) % 10;
				this.audioInfo[i] = audioInfo;
			}
		}
		if (this.tuning.renderMesh)
		{
			this.RenderMesh(z, render_layer, lerp_percent, trigger_audio);
		}
		if (trigger_audio)
		{
			this.TriggerAudio();
		}
	}

	// Token: 0x0600594A RID: 22858 RVA: 0x002049E8 File Offset: 0x00202BE8
	private void RenderMesh(float z, int render_layer, float lerp_percent, bool trigger_audio)
	{
		GridArea visibleArea = GridVisibleArea.GetVisibleArea();
		Vector2I min = new Vector2I(Mathf.Max(0, visibleArea.Min.x - 1), Mathf.Max(0, visibleArea.Min.y - 1));
		Vector2I max = new Vector2I(Mathf.Min(Grid.WidthInCells - 1, visibleArea.Max.x + 1), Mathf.Min(Grid.HeightInCells - 1, visibleArea.Max.y + 1));
		ConduitFlowVisualizer.RenderMeshContext renderMeshContext = new ConduitFlowVisualizer.RenderMeshContext(this, lerp_percent, min, max);
		if (renderMeshContext.visible_conduits.Count == 0)
		{
			renderMeshContext.Finish();
			return;
		}
		ConduitFlowVisualizer.render_mesh_job.Reset(renderMeshContext);
		int num = Mathf.Max(1, (int)((float)(renderMeshContext.visible_conduits.Count / CPUBudget.coreCount) / 1.5f));
		int num2 = Mathf.Max(1, renderMeshContext.visible_conduits.Count / num);
		for (int num3 = 0; num3 != num2; num3++)
		{
			int num4 = num3 * num;
			int end = (num3 == num2 - 1) ? renderMeshContext.visible_conduits.Count : (num4 + num);
			ConduitFlowVisualizer.render_mesh_job.Add(new ConduitFlowVisualizer.RenderMeshTask(num4, end));
		}
		GlobalJobManager.Run(ConduitFlowVisualizer.render_mesh_job);
		float z2 = 0f;
		if (this.showContents)
		{
			z2 = 1f;
		}
		float w = (float)((int)(this.animTime / (1.0 / (double)this.tuning.framesPerSecond)) % (int)this.tuning.spriteCount) * (1f / this.tuning.spriteCount);
		this.movingBallMesh.Begin();
		this.movingBallMesh.SetTexture("_BackgroundTex", this.tuning.backgroundTexture);
		this.movingBallMesh.SetTexture("_ForegroundTex", this.tuning.foregroundTexture);
		this.movingBallMesh.SetVector("_SpriteSettings", new Vector4(1f / this.tuning.spriteCount, 1f, z2, w));
		this.movingBallMesh.SetVector("_Highlight", new Vector4((float)this.highlightColour.r / 255f, (float)this.highlightColour.g / 255f, (float)this.highlightColour.b / 255f, 0f));
		this.staticBallMesh.Begin();
		this.staticBallMesh.SetTexture("_BackgroundTex", this.tuning.backgroundTexture);
		this.staticBallMesh.SetTexture("_ForegroundTex", this.tuning.foregroundTexture);
		this.staticBallMesh.SetVector("_SpriteSettings", new Vector4(1f / this.tuning.spriteCount, 1f, z2, 0f));
		this.staticBallMesh.SetVector("_Highlight", new Vector4((float)this.highlightColour.r / 255f, (float)this.highlightColour.g / 255f, (float)this.highlightColour.b / 255f, 0f));
		Vector3 position = CameraController.Instance.transform.GetPosition();
		ConduitFlowVisualizer visualizer = trigger_audio ? this : null;
		for (int num5 = 0; num5 != ConduitFlowVisualizer.render_mesh_job.Count; num5++)
		{
			ConduitFlowVisualizer.render_mesh_job.GetWorkItem(num5).Finish(this.movingBallMesh, this.staticBallMesh, position, visualizer);
		}
		this.movingBallMesh.End(z, this.layer);
		this.staticBallMesh.End(z, this.layer);
		renderMeshContext.Finish();
		ConduitFlowVisualizer.render_mesh_job.Reset(null);
	}

	// Token: 0x0600594B RID: 22859 RVA: 0x00204D7D File Offset: 0x00202F7D
	public void ColourizePipeContents(bool show_contents, bool move_to_overlay_layer)
	{
		this.showContents = show_contents;
		this.layer = ((show_contents && move_to_overlay_layer) ? LayerMask.NameToLayer("MaskedOverlay") : 0);
	}

	// Token: 0x0600594C RID: 22860 RVA: 0x00204DA0 File Offset: 0x00202FA0
	private void AddAudioSource(ConduitFlow.Conduit conduit, Vector3 camera_pos)
	{
		using (new KProfiler.Region("AddAudioSource", null))
		{
			UtilityNetwork network = this.flowManager.GetNetwork(conduit);
			if (network != null)
			{
				Vector3 vector = Grid.CellToPosCCC(conduit.GetCell(this.flowManager), Grid.SceneLayer.Building);
				float num = Vector3.SqrMagnitude(vector - camera_pos);
				bool flag = false;
				for (int i = 0; i < this.audioInfo.Count; i++)
				{
					ConduitFlowVisualizer.AudioInfo audioInfo = this.audioInfo[i];
					if (audioInfo.networkID == network.id)
					{
						if (num < audioInfo.distance)
						{
							audioInfo.distance = num;
							audioInfo.position = vector;
							this.audioInfo[i] = audioInfo;
						}
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					ConduitFlowVisualizer.AudioInfo item = default(ConduitFlowVisualizer.AudioInfo);
					item.networkID = network.id;
					item.position = vector;
					item.distance = num;
					item.blobCount = 0;
					this.audioInfo.Add(item);
				}
			}
		}
	}

	// Token: 0x0600594D RID: 22861 RVA: 0x00204EB8 File Offset: 0x002030B8
	private void TriggerAudio()
	{
		if (SpeedControlScreen.Instance.IsPaused)
		{
			return;
		}
		CameraController instance = CameraController.Instance;
		int num = 0;
		List<ConduitFlowVisualizer.AudioInfo> list = new List<ConduitFlowVisualizer.AudioInfo>();
		for (int i = 0; i < this.audioInfo.Count; i++)
		{
			if (instance.IsVisiblePos(this.audioInfo[i].position))
			{
				list.Add(this.audioInfo[i]);
				num++;
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			ConduitFlowVisualizer.AudioInfo audioInfo = list[j];
			if (audioInfo.distance != float.PositiveInfinity)
			{
				Vector3 position = audioInfo.position;
				position.z = 0f;
				EventInstance instance2 = SoundEvent.BeginOneShot(this.overlaySound, position, 1f, false);
				instance2.setParameterByName("blobCount", (float)audioInfo.blobCount, false);
				instance2.setParameterByName("networkCount", (float)num, false);
				SoundEvent.EndOneShot(instance2);
			}
		}
	}

	// Token: 0x0600594E RID: 22862 RVA: 0x00204FAA File Offset: 0x002031AA
	public void AddThermalConductivity(int cell, float conductivity)
	{
		if (conductivity < 1f)
		{
			this.insulatedCells.Add(cell);
			return;
		}
		if (conductivity > 1f)
		{
			this.radiantCells.Add(cell);
		}
	}

	// Token: 0x0600594F RID: 22863 RVA: 0x00204FD7 File Offset: 0x002031D7
	public void RemoveThermalConductivity(int cell, float conductivity)
	{
		if (conductivity < 1f)
		{
			this.insulatedCells.Remove(cell);
			return;
		}
		if (conductivity > 1f)
		{
			this.radiantCells.Remove(cell);
		}
	}

	// Token: 0x06005950 RID: 22864 RVA: 0x00205004 File Offset: 0x00203204
	public void SetHighlightedCell(int cell)
	{
		this.highlightedCell = cell;
	}

	// Token: 0x04003AA4 RID: 15012
	private ConduitFlow flowManager;

	// Token: 0x04003AA5 RID: 15013
	private EventReference overlaySound;

	// Token: 0x04003AA6 RID: 15014
	private bool showContents;

	// Token: 0x04003AA7 RID: 15015
	private double animTime;

	// Token: 0x04003AA8 RID: 15016
	private int layer;

	// Token: 0x04003AA9 RID: 15017
	private static Vector2 GRID_OFFSET = new Vector2(0.5f, 0.5f);

	// Token: 0x04003AAA RID: 15018
	private List<ConduitFlowVisualizer.AudioInfo> audioInfo;

	// Token: 0x04003AAB RID: 15019
	private HashSet<int> insulatedCells = new HashSet<int>();

	// Token: 0x04003AAC RID: 15020
	private HashSet<int> radiantCells = new HashSet<int>();

	// Token: 0x04003AAD RID: 15021
	private Game.ConduitVisInfo visInfo;

	// Token: 0x04003AAE RID: 15022
	private ConduitFlowVisualizer.ConduitFlowMesh movingBallMesh;

	// Token: 0x04003AAF RID: 15023
	private ConduitFlowVisualizer.ConduitFlowMesh staticBallMesh;

	// Token: 0x04003AB0 RID: 15024
	private int highlightedCell = -1;

	// Token: 0x04003AB1 RID: 15025
	private Color32 highlightColour = new Color(0.2f, 0.2f, 0.2f, 0.2f);

	// Token: 0x04003AB2 RID: 15026
	private ConduitFlowVisualizer.Tuning tuning;

	// Token: 0x04003AB3 RID: 15027
	private static WorkItemCollection<ConduitFlowVisualizer.RenderMeshTask, ConduitFlowVisualizer.RenderMeshContext> render_mesh_job = new WorkItemCollection<ConduitFlowVisualizer.RenderMeshTask, ConduitFlowVisualizer.RenderMeshContext>();

	// Token: 0x02001BF4 RID: 7156
	[Serializable]
	public class Tuning
	{
		// Token: 0x04008138 RID: 33080
		public bool renderMesh;

		// Token: 0x04008139 RID: 33081
		public float size;

		// Token: 0x0400813A RID: 33082
		public float spriteCount;

		// Token: 0x0400813B RID: 33083
		public float framesPerSecond;

		// Token: 0x0400813C RID: 33084
		public Texture2D backgroundTexture;

		// Token: 0x0400813D RID: 33085
		public Texture2D foregroundTexture;
	}

	// Token: 0x02001BF5 RID: 7157
	private class ConduitFlowMesh
	{
		// Token: 0x0600A4F7 RID: 42231 RVA: 0x0038DF24 File Offset: 0x0038C124
		public ConduitFlowMesh()
		{
			this.mesh = new Mesh();
			this.mesh.name = "ConduitMesh";
			this.material = new Material(Shader.Find("Klei/ConduitBall"));
		}

		// Token: 0x0600A4F8 RID: 42232 RVA: 0x0038DF94 File Offset: 0x0038C194
		public void AddQuad(Vector2 pos, Color32 color, float size, float is_foreground, float highlight, Vector2I uvbl, Vector2I uvtl, Vector2I uvbr, Vector2I uvtr)
		{
			float num = size * 0.5f;
			this.positions.Add(new Vector3(pos.x - num, pos.y - num, 0f));
			this.positions.Add(new Vector3(pos.x - num, pos.y + num, 0f));
			this.positions.Add(new Vector3(pos.x + num, pos.y - num, 0f));
			this.positions.Add(new Vector3(pos.x + num, pos.y + num, 0f));
			this.uvs.Add(new Vector4((float)uvbl.x, (float)uvbl.y, is_foreground, highlight));
			this.uvs.Add(new Vector4((float)uvtl.x, (float)uvtl.y, is_foreground, highlight));
			this.uvs.Add(new Vector4((float)uvbr.x, (float)uvbr.y, is_foreground, highlight));
			this.uvs.Add(new Vector4((float)uvtr.x, (float)uvtr.y, is_foreground, highlight));
			this.colors.Add(color);
			this.colors.Add(color);
			this.colors.Add(color);
			this.colors.Add(color);
			this.triangles.Add(this.quadIndex * 4);
			this.triangles.Add(this.quadIndex * 4 + 1);
			this.triangles.Add(this.quadIndex * 4 + 2);
			this.triangles.Add(this.quadIndex * 4 + 2);
			this.triangles.Add(this.quadIndex * 4 + 1);
			this.triangles.Add(this.quadIndex * 4 + 3);
			this.quadIndex++;
		}

		// Token: 0x0600A4F9 RID: 42233 RVA: 0x0038E187 File Offset: 0x0038C387
		public void SetTexture(string id, Texture2D texture)
		{
			this.material.SetTexture(id, texture);
		}

		// Token: 0x0600A4FA RID: 42234 RVA: 0x0038E196 File Offset: 0x0038C396
		public void SetVector(string id, Vector4 data)
		{
			this.material.SetVector(id, data);
		}

		// Token: 0x0600A4FB RID: 42235 RVA: 0x0038E1A5 File Offset: 0x0038C3A5
		public void Begin()
		{
			this.positions.Clear();
			this.uvs.Clear();
			this.triangles.Clear();
			this.colors.Clear();
			this.quadIndex = 0;
		}

		// Token: 0x0600A4FC RID: 42236 RVA: 0x0038E1DC File Offset: 0x0038C3DC
		public void End(float z, int layer)
		{
			this.mesh.Clear();
			this.mesh.SetVertices(this.positions);
			this.mesh.SetUVs(0, this.uvs);
			this.mesh.SetColors(this.colors);
			this.mesh.SetTriangles(this.triangles, 0, false);
			Graphics.DrawMesh(this.mesh, new Vector3(ConduitFlowVisualizer.GRID_OFFSET.x, ConduitFlowVisualizer.GRID_OFFSET.y, z - 0.1f), Quaternion.identity, this.material, layer);
		}

		// Token: 0x0600A4FD RID: 42237 RVA: 0x0038E272 File Offset: 0x0038C472
		public void Cleanup()
		{
			UnityEngine.Object.Destroy(this.mesh);
			this.mesh = null;
			UnityEngine.Object.Destroy(this.material);
			this.material = null;
		}

		// Token: 0x0400813E RID: 33086
		private Mesh mesh;

		// Token: 0x0400813F RID: 33087
		private Material material;

		// Token: 0x04008140 RID: 33088
		private List<Vector3> positions = new List<Vector3>();

		// Token: 0x04008141 RID: 33089
		private List<Vector4> uvs = new List<Vector4>();

		// Token: 0x04008142 RID: 33090
		private List<int> triangles = new List<int>();

		// Token: 0x04008143 RID: 33091
		private List<Color32> colors = new List<Color32>();

		// Token: 0x04008144 RID: 33092
		private int quadIndex;
	}

	// Token: 0x02001BF6 RID: 7158
	private struct AudioInfo
	{
		// Token: 0x04008145 RID: 33093
		public int networkID;

		// Token: 0x04008146 RID: 33094
		public int blobCount;

		// Token: 0x04008147 RID: 33095
		public float distance;

		// Token: 0x04008148 RID: 33096
		public Vector3 position;
	}

	// Token: 0x02001BF7 RID: 7159
	private class RenderMeshContext
	{
		// Token: 0x0600A4FE RID: 42238 RVA: 0x0038E298 File Offset: 0x0038C498
		public RenderMeshContext(ConduitFlowVisualizer outer, float lerp_percent, Vector2I min, Vector2I max)
		{
			this.outer = outer;
			this.lerp_percent = lerp_percent;
			this.visible_conduits = ListPool<int, ConduitFlowVisualizer>.Allocate();
			this.visible_conduits.Capacity = Math.Max(outer.flowManager.soaInfo.NumEntries, this.visible_conduits.Capacity);
			for (int num = 0; num != outer.flowManager.soaInfo.NumEntries; num++)
			{
				Vector2I vector2I = Grid.CellToXY(outer.flowManager.soaInfo.GetCell(num));
				if (min <= vector2I && vector2I <= max)
				{
					this.visible_conduits.Add(num);
				}
			}
		}

		// Token: 0x0600A4FF RID: 42239 RVA: 0x0038E340 File Offset: 0x0038C540
		public void Finish()
		{
			this.visible_conduits.Recycle();
		}

		// Token: 0x04008149 RID: 33097
		public ListPool<int, ConduitFlowVisualizer>.PooledList visible_conduits;

		// Token: 0x0400814A RID: 33098
		public ConduitFlowVisualizer outer;

		// Token: 0x0400814B RID: 33099
		public float lerp_percent;
	}

	// Token: 0x02001BF8 RID: 7160
	private struct RenderMeshTask : IWorkItem<ConduitFlowVisualizer.RenderMeshContext>
	{
		// Token: 0x0600A500 RID: 42240 RVA: 0x0038E350 File Offset: 0x0038C550
		public RenderMeshTask(int start, int end)
		{
			this.start = start;
			this.end = end;
			int capacity = end - start;
			this.moving_balls = ListPool<ConduitFlowVisualizer.RenderMeshTask.Ball, ConduitFlowVisualizer.RenderMeshTask>.Allocate();
			this.moving_balls.Capacity = capacity;
			this.static_balls = ListPool<ConduitFlowVisualizer.RenderMeshTask.Ball, ConduitFlowVisualizer.RenderMeshTask>.Allocate();
			this.static_balls.Capacity = capacity;
			this.moving_conduits = ListPool<ConduitFlow.Conduit, ConduitFlowVisualizer.RenderMeshTask>.Allocate();
			this.moving_conduits.Capacity = capacity;
		}

		// Token: 0x0600A501 RID: 42241 RVA: 0x0038E3B4 File Offset: 0x0038C5B4
		public void Run(ConduitFlowVisualizer.RenderMeshContext context)
		{
			Element element = null;
			for (int num = this.start; num != this.end; num++)
			{
				ConduitFlow.Conduit conduit = context.outer.flowManager.soaInfo.GetConduit(context.visible_conduits[num]);
				ConduitFlow.ConduitFlowInfo lastFlowInfo = conduit.GetLastFlowInfo(context.outer.flowManager);
				ConduitFlow.ConduitContents initialContents = conduit.GetInitialContents(context.outer.flowManager);
				if (lastFlowInfo.contents.mass > 0f)
				{
					int cell = conduit.GetCell(context.outer.flowManager);
					int cellFromDirection = ConduitFlow.GetCellFromDirection(cell, lastFlowInfo.direction);
					Vector2I vector2I = Grid.CellToXY(cell);
					Vector2I vector2I2 = Grid.CellToXY(cellFromDirection);
					Vector2 vector = (cell == -1) ? vector2I : Vector2.Lerp(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y), context.lerp_percent);
					Color32 cellTintColour = context.outer.GetCellTintColour(cell);
					Color32 cellTintColour2 = context.outer.GetCellTintColour(cellFromDirection);
					Color32 color = Color32.Lerp(cellTintColour, cellTintColour2, context.lerp_percent);
					bool highlight = false;
					if (context.outer.showContents)
					{
						if (lastFlowInfo.contents.mass >= initialContents.mass)
						{
							this.moving_balls.Add(new ConduitFlowVisualizer.RenderMeshTask.Ball(lastFlowInfo.direction, vector, color, context.outer.tuning.size, false, false));
						}
						if (element == null || lastFlowInfo.contents.element != element.id)
						{
							element = ElementLoader.FindElementByHash(lastFlowInfo.contents.element);
						}
					}
					else
					{
						element = null;
						highlight = (Grid.PosToCell(new Vector3(vector.x + ConduitFlowVisualizer.GRID_OFFSET.x, vector.y + ConduitFlowVisualizer.GRID_OFFSET.y, 0f)) == context.outer.highlightedCell);
					}
					Color32 contentsColor = context.outer.GetContentsColor(element, color);
					float num2 = 1f;
					if (context.outer.showContents || lastFlowInfo.contents.mass < initialContents.mass)
					{
						num2 = context.outer.CalculateMassScale(lastFlowInfo.contents.mass);
					}
					this.moving_balls.Add(new ConduitFlowVisualizer.RenderMeshTask.Ball(lastFlowInfo.direction, vector, contentsColor, context.outer.tuning.size * num2, true, highlight));
					this.moving_conduits.Add(conduit);
				}
				if (initialContents.mass > lastFlowInfo.contents.mass && initialContents.mass > 0f)
				{
					int cell2 = conduit.GetCell(context.outer.flowManager);
					Vector2 pos = Grid.CellToXY(cell2);
					float mass = initialContents.mass - lastFlowInfo.contents.mass;
					bool highlight2 = false;
					Color32 cellTintColour3 = context.outer.GetCellTintColour(cell2);
					float num3 = context.outer.CalculateMassScale(mass);
					if (context.outer.showContents)
					{
						this.static_balls.Add(new ConduitFlowVisualizer.RenderMeshTask.Ball(ConduitFlow.FlowDirections.None, pos, cellTintColour3, context.outer.tuning.size * num3, false, false));
						if (element == null || initialContents.element != element.id)
						{
							element = ElementLoader.FindElementByHash(initialContents.element);
						}
					}
					else
					{
						element = null;
						highlight2 = (cell2 == context.outer.highlightedCell);
					}
					Color32 contentsColor2 = context.outer.GetContentsColor(element, cellTintColour3);
					this.static_balls.Add(new ConduitFlowVisualizer.RenderMeshTask.Ball(ConduitFlow.FlowDirections.None, pos, contentsColor2, context.outer.tuning.size * num3, true, highlight2));
				}
			}
		}

		// Token: 0x0600A502 RID: 42242 RVA: 0x0038E758 File Offset: 0x0038C958
		public void Finish(ConduitFlowVisualizer.ConduitFlowMesh moving_ball_mesh, ConduitFlowVisualizer.ConduitFlowMesh static_ball_mesh, Vector3 camera_pos, ConduitFlowVisualizer visualizer)
		{
			for (int num = 0; num != this.moving_balls.Count; num++)
			{
				this.moving_balls[num].Consume(moving_ball_mesh);
			}
			this.moving_balls.Recycle();
			for (int num2 = 0; num2 != this.static_balls.Count; num2++)
			{
				this.static_balls[num2].Consume(static_ball_mesh);
			}
			this.static_balls.Recycle();
			if (visualizer != null)
			{
				foreach (ConduitFlow.Conduit conduit in this.moving_conduits)
				{
					visualizer.AddAudioSource(conduit, camera_pos);
				}
			}
			this.moving_conduits.Recycle();
		}

		// Token: 0x0400814C RID: 33100
		private ListPool<ConduitFlowVisualizer.RenderMeshTask.Ball, ConduitFlowVisualizer.RenderMeshTask>.PooledList moving_balls;

		// Token: 0x0400814D RID: 33101
		private ListPool<ConduitFlowVisualizer.RenderMeshTask.Ball, ConduitFlowVisualizer.RenderMeshTask>.PooledList static_balls;

		// Token: 0x0400814E RID: 33102
		private ListPool<ConduitFlow.Conduit, ConduitFlowVisualizer.RenderMeshTask>.PooledList moving_conduits;

		// Token: 0x0400814F RID: 33103
		private int start;

		// Token: 0x04008150 RID: 33104
		private int end;

		// Token: 0x02002632 RID: 9778
		public struct Ball
		{
			// Token: 0x0600C1A6 RID: 49574 RVA: 0x003DEAFD File Offset: 0x003DCCFD
			public Ball(ConduitFlow.FlowDirections direction, Vector2 pos, Color32 color, float size, bool foreground, bool highlight)
			{
				this.pos = pos;
				this.size = size;
				this.color = color;
				this.direction = direction;
				this.foreground = foreground;
				this.highlight = highlight;
			}

			// Token: 0x0600C1A7 RID: 49575 RVA: 0x003DEB2C File Offset: 0x003DCD2C
			public static void InitializeResources()
			{
				ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.None] = new ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack
				{
					bl = new Vector2I(0, 0),
					tl = new Vector2I(0, 1),
					br = new Vector2I(1, 0),
					tr = new Vector2I(1, 1)
				};
				ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.Left] = new ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack
				{
					bl = new Vector2I(0, 0),
					tl = new Vector2I(0, 1),
					br = new Vector2I(1, 0),
					tr = new Vector2I(1, 1)
				};
				ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.Right] = ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.Left];
				ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.Up] = new ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack
				{
					bl = new Vector2I(1, 0),
					tl = new Vector2I(0, 0),
					br = new Vector2I(1, 1),
					tr = new Vector2I(0, 1)
				};
				ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.Down] = ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.Up];
			}

			// Token: 0x0600C1A8 RID: 49576 RVA: 0x003DEC31 File Offset: 0x003DCE31
			private static ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack GetUVPack(ConduitFlow.FlowDirections direction)
			{
				return ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[direction];
			}

			// Token: 0x0600C1A9 RID: 49577 RVA: 0x003DEC40 File Offset: 0x003DCE40
			public void Consume(ConduitFlowVisualizer.ConduitFlowMesh mesh)
			{
				ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack uvpack = ConduitFlowVisualizer.RenderMeshTask.Ball.GetUVPack(this.direction);
				mesh.AddQuad(this.pos, this.color, this.size, (float)(this.foreground ? 1 : 0), (float)(this.highlight ? 1 : 0), uvpack.bl, uvpack.tl, uvpack.br, uvpack.tr);
			}

			// Token: 0x0400A9F7 RID: 43511
			private Vector2 pos;

			// Token: 0x0400A9F8 RID: 43512
			private float size;

			// Token: 0x0400A9F9 RID: 43513
			private Color32 color;

			// Token: 0x0400A9FA RID: 43514
			private ConduitFlow.FlowDirections direction;

			// Token: 0x0400A9FB RID: 43515
			private bool foreground;

			// Token: 0x0400A9FC RID: 43516
			private bool highlight;

			// Token: 0x0400A9FD RID: 43517
			private static Dictionary<ConduitFlow.FlowDirections, ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack> uv_packs = new Dictionary<ConduitFlow.FlowDirections, ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack>();

			// Token: 0x02003544 RID: 13636
			private class UVPack
			{
				// Token: 0x0400D7CC RID: 55244
				public Vector2I bl;

				// Token: 0x0400D7CD RID: 55245
				public Vector2I tl;

				// Token: 0x0400D7CE RID: 55246
				public Vector2I br;

				// Token: 0x0400D7CF RID: 55247
				public Vector2I tr;
			}
		}
	}
}
