﻿using System;
using Unity.Collections;
using UnityEngine;

// Token: 0x02000B71 RID: 2929
public class RocketLaunchConditionVisualizerEffect : VisualizerEffect
{
	// Token: 0x06005806 RID: 22534 RVA: 0x001FC244 File Offset: 0x001FA444
	protected override void SetupMaterial()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/RocketLaunchCondition"));
	}

	// Token: 0x06005807 RID: 22535 RVA: 0x001FC25B File Offset: 0x001FA45B
	protected override void SetupOcclusionTex()
	{
		this.OcclusionTex = new Texture2D(512, 1, TextureFormat.RGFloat, false);
		this.OcclusionTex.filterMode = FilterMode.Point;
		this.OcclusionTex.wrapMode = TextureWrapMode.Clamp;
	}

	// Token: 0x06005808 RID: 22536 RVA: 0x001FC28C File Offset: 0x001FA48C
	protected override void OnPostRender()
	{
		RocketLaunchConditionVisualizer rocketLaunchConditionVisualizer = null;
		if (SelectTool.Instance.selected != null)
		{
			rocketLaunchConditionVisualizer = SelectTool.Instance.selected.GetComponent<RocketLaunchConditionVisualizer>();
			if (rocketLaunchConditionVisualizer == null)
			{
				RocketModuleCluster component = SelectTool.Instance.selected.GetComponent<RocketModuleCluster>();
				if (component != null)
				{
					PassengerRocketModule passengerModule = component.CraftInterface.GetPassengerModule();
					if (passengerModule != null)
					{
						rocketLaunchConditionVisualizer = passengerModule.gameObject.GetComponent<RocketLaunchConditionVisualizer>();
					}
				}
			}
		}
		if (rocketLaunchConditionVisualizer != null)
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			RocketLaunchConditionVisualizerEffect.FindWorldBounds(out vector2I, out vector2I2);
			if (vector2I2.x - vector2I.x > this.OcclusionTex.width)
			{
				return;
			}
			NativeArray<float> pixelData = this.OcclusionTex.GetPixelData<float>(0);
			for (int i = 0; i < this.OcclusionTex.width; i++)
			{
				pixelData[2 * i] = 0f;
				pixelData[2 * i + 1] = 0f;
			}
			for (int j = 0; j < RocketLaunchConditionVisualizerEffect.clearPathToSpaceColumn.Length; j++)
			{
				RocketLaunchConditionVisualizerEffect.clearPathToSpaceColumn[j] = RocketLaunchConditionVisualizerEffect.EvaluationState.NotEvaluated;
			}
			for (int k = 0; k < rocketLaunchConditionVisualizer.moduleVisualizeData.Length; k++)
			{
				RocketLaunchConditionVisualizerEffect.ComputeVisibility(rocketLaunchConditionVisualizer.moduleVisualizeData[k], pixelData, vector2I, vector2I2);
			}
			this.OcclusionTex.Apply(false, false);
			Vector2I vector2I3 = vector2I;
			Vector2I vector2I4 = vector2I2;
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
			vector.x = point.x;
			vector.y = point.y;
			ray = this.myCamera.ViewportPointToRay(Vector3.one);
			distance = Mathf.Abs(ray.origin.z / ray.direction.z);
			point = ray.GetPoint(distance);
			vector.z = point.x - vector.x;
			vector.w = point.y - vector.y;
			this.material.SetVector("_UVOffsetScale", vector);
			Vector4 value;
			value.x = (float)vector2I3.x;
			value.y = (float)vector2I3.y;
			value.z = (float)vector2I4.x;
			value.w = (float)vector2I4.y;
			this.material.SetVector("_RangeParams", value);
			this.material.SetColor("_HighlightColor", this.highlightColor);
			this.material.SetColor("_HighlightColor2", this.highlightColor2);
			Vector4 value2;
			value2.x = 1f / (float)this.OcclusionTex.width;
			value2.y = 1f / (float)this.OcclusionTex.height;
			value2.z = 0f;
			value2.w = 0f;
			this.material.SetVector("_OcclusionParams", value2);
			this.material.SetTexture("_OcclusionTex", this.OcclusionTex);
			Vector4 value3;
			value3.x = (float)Grid.WidthInCells;
			value3.y = (float)Grid.HeightInCells;
			value3.z = 1f / (float)Grid.WidthInCells;
			value3.w = 1f / (float)Grid.HeightInCells;
			this.material.SetVector("_WorldParams", value3);
			GL.PushMatrix();
			this.material.SetPass(0);
			GL.LoadOrtho();
			GL.Begin(5);
			GL.Color(Color.white);
			GL.Vertex3(0f, 0f, 0f);
			GL.Vertex3(0f, 1f, 0f);
			GL.Vertex3(1f, 0f, 0f);
			GL.Vertex3(1f, 1f, 0f);
			GL.End();
			GL.PopMatrix();
		}
	}

	// Token: 0x06005809 RID: 22537 RVA: 0x001FC698 File Offset: 0x001FA898
	private static void ComputeVisibility(RocketLaunchConditionVisualizer.RocketModuleVisualizeData moduleData, NativeArray<float> pixels, Vector2I world_min, Vector2I world_max)
	{
		Vector2I u = Grid.PosToXY(moduleData.Module.transform.GetPosition());
		int rangeMin = moduleData.RangeMin;
		int rangeMax = moduleData.RangeMax;
		Vector2I vector2I = u + moduleData.OriginOffset;
		for (int i = 0; i >= rangeMin; i--)
		{
			int x_abs = vector2I.x + i;
			int y = vector2I.y;
			RocketLaunchConditionVisualizerEffect.EvaluationState evaluationState = RocketLaunchConditionVisualizerEffect.clearPathToSpaceColumn[RocketLaunchConditionVisualizerEffect.clearPathToSpaceColumn_middleIndex + i];
			RocketLaunchConditionVisualizerEffect.ComputeVisibility(x_abs, y, pixels, world_min, world_max, ref evaluationState);
			RocketLaunchConditionVisualizerEffect.clearPathToSpaceColumn[RocketLaunchConditionVisualizerEffect.clearPathToSpaceColumn_middleIndex + i] = evaluationState;
		}
		for (int j = 0; j <= rangeMax; j++)
		{
			int x_abs2 = vector2I.x + j;
			int y2 = vector2I.y;
			RocketLaunchConditionVisualizerEffect.EvaluationState evaluationState2 = RocketLaunchConditionVisualizerEffect.clearPathToSpaceColumn[RocketLaunchConditionVisualizerEffect.clearPathToSpaceColumn_middleIndex + j];
			RocketLaunchConditionVisualizerEffect.ComputeVisibility(x_abs2, y2, pixels, world_min, world_max, ref evaluationState2);
			RocketLaunchConditionVisualizerEffect.clearPathToSpaceColumn[RocketLaunchConditionVisualizerEffect.clearPathToSpaceColumn_middleIndex + j] = evaluationState2;
		}
	}

	// Token: 0x0600580A RID: 22538 RVA: 0x001FC770 File Offset: 0x001FA970
	private static void ComputeVisibility(int x_abs, int y_abs, NativeArray<float> pixels, Vector2I world_min, Vector2I world_max, ref RocketLaunchConditionVisualizerEffect.EvaluationState clearPathEvaluation)
	{
		int num = x_abs - world_min.x;
		if (x_abs < world_min.x || x_abs > world_max.x || y_abs < world_min.y || y_abs >= world_max.y)
		{
			return;
		}
		int cell = Grid.XYToCell(x_abs, y_abs);
		if (clearPathEvaluation == RocketLaunchConditionVisualizerEffect.EvaluationState.NotEvaluated)
		{
			clearPathEvaluation = (RocketLaunchConditionVisualizerEffect.HasClearPathToSpace(cell, world_max) ? RocketLaunchConditionVisualizerEffect.EvaluationState.Clear : RocketLaunchConditionVisualizerEffect.EvaluationState.Obstructed);
		}
		bool flag = clearPathEvaluation == RocketLaunchConditionVisualizerEffect.EvaluationState.Clear;
		if (pixels[2 * num] == 2f)
		{
			if (flag)
			{
				pixels[2 * num + 1] = Mathf.Min(pixels[2 * num + 1], (float)y_abs);
			}
			return;
		}
		pixels[2 * num] = (float)(flag ? 2 : 1);
		if (pixels[2 * num] == 1f && pixels[2 * num + 1] != 0f)
		{
			pixels[2 * num + 1] = Mathf.Min(pixels[2 * num + 1], (float)y_abs);
			return;
		}
		pixels[2 * num + 1] = (float)y_abs;
	}

	// Token: 0x0600580B RID: 22539 RVA: 0x001FC86C File Offset: 0x001FAA6C
	private static void FindWorldBounds(out Vector2I world_min, out Vector2I world_max)
	{
		if (ClusterManager.Instance != null)
		{
			WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
			world_min = activeWorld.WorldOffset;
			world_max = activeWorld.WorldOffset + activeWorld.WorldSize;
			return;
		}
		world_min.x = 0;
		world_min.y = 0;
		world_max.x = Grid.WidthInCells;
		world_max.y = Grid.HeightInCells;
	}

	// Token: 0x0600580C RID: 22540 RVA: 0x001FC8DC File Offset: 0x001FAADC
	private static bool HasClearPathToSpace(int cell, Vector2I worldMax)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		int cell2 = cell;
		while (!Grid.IsSolidCell(cell2) && Grid.CellToXY(cell2).y < worldMax.y)
		{
			cell2 = Grid.CellAbove(cell2);
		}
		return !Grid.IsSolidCell(cell2) && Grid.CellToXY(cell2).y == worldMax.y;
	}

	// Token: 0x0400398B RID: 14731
	public Color highlightColor = new Color(0f, 1f, 0.8f, 1f);

	// Token: 0x0400398C RID: 14732
	public Color highlightColor2 = new Color(1f, 0.32f, 0f, 1f);

	// Token: 0x0400398D RID: 14733
	private static RocketLaunchConditionVisualizerEffect.EvaluationState[] clearPathToSpaceColumn = new RocketLaunchConditionVisualizerEffect.EvaluationState[7];

	// Token: 0x0400398E RID: 14734
	private static int clearPathToSpaceColumn_middleIndex = Mathf.FloorToInt(3.5f);

	// Token: 0x02001BD9 RID: 7129
	public enum EvaluationState : byte
	{
		// Token: 0x040080DF RID: 32991
		NotEvaluated,
		// Token: 0x040080E0 RID: 32992
		Clear,
		// Token: 0x040080E1 RID: 32993
		Obstructed
	}
}
