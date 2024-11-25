using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

// Token: 0x02000A9A RID: 2714
[AddComponentMenu("KMonoBehaviour/scripts/SimDebugView")]
public class SimDebugView : KMonoBehaviour
{
	// Token: 0x06004F94 RID: 20372 RVA: 0x001C9567 File Offset: 0x001C7767
	public static void DestroyInstance()
	{
		SimDebugView.Instance = null;
	}

	// Token: 0x06004F95 RID: 20373 RVA: 0x001C956F File Offset: 0x001C776F
	protected override void OnPrefabInit()
	{
		SimDebugView.Instance = this;
		this.material = UnityEngine.Object.Instantiate<Material>(this.material);
		this.diseaseMaterial = UnityEngine.Object.Instantiate<Material>(this.diseaseMaterial);
	}

	// Token: 0x06004F96 RID: 20374 RVA: 0x001C959C File Offset: 0x001C779C
	protected override void OnSpawn()
	{
		SimDebugViewCompositor.Instance.material.SetColor("_Color0", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[0].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color1", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[1].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color2", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[2].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color3", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[3].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color4", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[4].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color5", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[5].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color6", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[6].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color7", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[7].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color0", GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[0].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color1", GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[1].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color2", GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[2].colorName));
		this.SetMode(global::OverlayModes.None.ID);
	}

	// Token: 0x06004F97 RID: 20375 RVA: 0x001C9828 File Offset: 0x001C7A28
	public void OnReset()
	{
		this.plane = SimDebugView.CreatePlane("SimDebugView", base.transform);
		this.tex = SimDebugView.CreateTexture(out this.texBytes, Grid.WidthInCells, Grid.HeightInCells);
		this.plane.GetComponent<Renderer>().sharedMaterial = this.material;
		this.plane.GetComponent<Renderer>().sharedMaterial.mainTexture = this.tex;
		this.plane.transform.SetLocalPosition(new Vector3(0f, 0f, -6f));
		this.SetMode(global::OverlayModes.None.ID);
	}

	// Token: 0x06004F98 RID: 20376 RVA: 0x001C98C7 File Offset: 0x001C7AC7
	public static Texture2D CreateTexture(int width, int height)
	{
		return new Texture2D(width, height)
		{
			name = "SimDebugView",
			wrapMode = TextureWrapMode.Clamp,
			filterMode = FilterMode.Point
		};
	}

	// Token: 0x06004F99 RID: 20377 RVA: 0x001C98E9 File Offset: 0x001C7AE9
	public static Texture2D CreateTexture(out byte[] textureBytes, int width, int height)
	{
		textureBytes = new byte[width * height * 4];
		return new Texture2D(width, height, TextureUtil.TextureFormatToGraphicsFormat(TextureFormat.RGBA32), TextureCreationFlags.None)
		{
			name = "SimDebugView",
			wrapMode = TextureWrapMode.Clamp,
			filterMode = FilterMode.Point
		};
	}

	// Token: 0x06004F9A RID: 20378 RVA: 0x001C9920 File Offset: 0x001C7B20
	public static Texture2D CreateTexture(int width, int height, Color col)
	{
		Color[] array = new Color[width * height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = col;
		}
		Texture2D texture2D = new Texture2D(width, height);
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x06004F9B RID: 20379 RVA: 0x001C9960 File Offset: 0x001C7B60
	public static GameObject CreatePlane(string layer, Transform parent)
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "overlayViewDisplayPlane";
		gameObject.SetLayerRecursively(LayerMask.NameToLayer(layer));
		gameObject.transform.SetParent(parent);
		gameObject.transform.SetPosition(Vector3.zero);
		gameObject.AddComponent<MeshRenderer>().reflectionProbeUsage = ReflectionProbeUsage.Off;
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		Mesh mesh = new Mesh();
		meshFilter.mesh = mesh;
		int num = 4;
		Vector3[] vertices = new Vector3[num];
		Vector2[] uv = new Vector2[num];
		int[] triangles = new int[6];
		float y = 2f * (float)Grid.HeightInCells;
		vertices = new Vector3[]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3((float)Grid.WidthInCells, 0f, 0f),
			new Vector3(0f, y, 0f),
			new Vector3(Grid.WidthInMeters, y, 0f)
		};
		uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 2f),
			new Vector2(1f, 2f)
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
		mesh.uv = uv;
		mesh.triangles = triangles;
		Vector2 vector = new Vector2((float)Grid.WidthInCells, y);
		mesh.bounds = new Bounds(new Vector3(0.5f * vector.x, 0.5f * vector.y, 0f), new Vector3(vector.x, vector.y, 0f));
		return gameObject;
	}

	// Token: 0x06004F9C RID: 20380 RVA: 0x001C9B38 File Offset: 0x001C7D38
	private void Update()
	{
		if (this.plane == null)
		{
			return;
		}
		bool flag = this.mode != global::OverlayModes.None.ID;
		this.plane.SetActive(flag);
		SimDebugViewCompositor.Instance.Toggle(flag && !GameUtil.IsCapturingTimeLapse());
		SimDebugViewCompositor.Instance.material.SetVector("_Thresholds0", new Vector4(0.1f, 0.2f, 0.3f, 0.4f));
		SimDebugViewCompositor.Instance.material.SetVector("_Thresholds1", new Vector4(0.5f, 0.6f, 0.7f, 0.8f));
		float x = 0f;
		if (this.mode == global::OverlayModes.ThermalConductivity.ID || this.mode == global::OverlayModes.Temperature.ID)
		{
			x = 1f;
		}
		SimDebugViewCompositor.Instance.material.SetVector("_ThresholdParameters", new Vector4(x, this.thresholdRange, this.thresholdOpacity, 0f));
		if (flag)
		{
			this.UpdateData(this.tex, this.texBytes, this.mode, 192);
		}
	}

	// Token: 0x06004F9D RID: 20381 RVA: 0x001C9C5E File Offset: 0x001C7E5E
	private static void SetDefaultBilinear(SimDebugView instance, Texture texture)
	{
		Renderer component = instance.plane.GetComponent<Renderer>();
		component.sharedMaterial = instance.material;
		component.sharedMaterial.mainTexture = instance.tex;
		texture.filterMode = FilterMode.Bilinear;
	}

	// Token: 0x06004F9E RID: 20382 RVA: 0x001C9C8E File Offset: 0x001C7E8E
	private static void SetDefaultPoint(SimDebugView instance, Texture texture)
	{
		Renderer component = instance.plane.GetComponent<Renderer>();
		component.sharedMaterial = instance.material;
		component.sharedMaterial.mainTexture = instance.tex;
		texture.filterMode = FilterMode.Point;
	}

	// Token: 0x06004F9F RID: 20383 RVA: 0x001C9CBE File Offset: 0x001C7EBE
	private static void SetDisease(SimDebugView instance, Texture texture)
	{
		Renderer component = instance.plane.GetComponent<Renderer>();
		component.sharedMaterial = instance.diseaseMaterial;
		component.sharedMaterial.mainTexture = instance.tex;
		texture.filterMode = FilterMode.Bilinear;
	}

	// Token: 0x06004FA0 RID: 20384 RVA: 0x001C9CF0 File Offset: 0x001C7EF0
	public void UpdateData(Texture2D texture, byte[] textureBytes, HashedString viewMode, byte alpha)
	{
		Action<SimDebugView, Texture> action;
		if (!this.dataUpdateFuncs.TryGetValue(viewMode, out action))
		{
			action = new Action<SimDebugView, Texture>(SimDebugView.SetDefaultPoint);
		}
		action(this, texture);
		int x;
		int num;
		int x2;
		int num2;
		Grid.GetVisibleExtents(out x, out num, out x2, out num2);
		this.selectedPathProber = null;
		KSelectable selected = SelectTool.Instance.selected;
		if (selected != null)
		{
			this.selectedPathProber = selected.GetComponent<PathProber>();
		}
		this.updateSimViewWorkItems.Reset(new SimDebugView.UpdateSimViewSharedData(this, this.texBytes, viewMode, this));
		int num3 = 16;
		for (int i = num; i <= num2; i += num3)
		{
			int y = Math.Min(i + num3 - 1, num2);
			this.updateSimViewWorkItems.Add(new SimDebugView.UpdateSimViewWorkItem(x, i, x2, y));
		}
		this.currentFrame = Time.frameCount;
		this.selectedCell = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
		GlobalJobManager.Run(this.updateSimViewWorkItems);
		texture.LoadRawTextureData(textureBytes);
		texture.Apply();
	}

	// Token: 0x06004FA1 RID: 20385 RVA: 0x001C9DEB File Offset: 0x001C7FEB
	public void SetGameGridMode(SimDebugView.GameGridMode mode)
	{
		this.gameGridMode = mode;
	}

	// Token: 0x06004FA2 RID: 20386 RVA: 0x001C9DF4 File Offset: 0x001C7FF4
	public SimDebugView.GameGridMode GetGameGridMode()
	{
		return this.gameGridMode;
	}

	// Token: 0x06004FA3 RID: 20387 RVA: 0x001C9DFC File Offset: 0x001C7FFC
	public void SetMode(HashedString mode)
	{
		this.mode = mode;
		Game.Instance.gameObject.Trigger(1798162660, mode);
	}

	// Token: 0x06004FA4 RID: 20388 RVA: 0x001C9E1F File Offset: 0x001C801F
	public HashedString GetMode()
	{
		return this.mode;
	}

	// Token: 0x06004FA5 RID: 20389 RVA: 0x001C9E28 File Offset: 0x001C8028
	public static Color TemperatureToColor(float temperature, float minTempExpected, float maxTempExpected)
	{
		float num = Mathf.Clamp((temperature - minTempExpected) / (maxTempExpected - minTempExpected), 0f, 1f);
		return Color.HSVToRGB((10f + (1f - num) * 171f) / 360f, 1f, 1f);
	}

	// Token: 0x06004FA6 RID: 20390 RVA: 0x001C9E74 File Offset: 0x001C8074
	public static Color LiquidTemperatureToColor(float temperature, float minTempExpected, float maxTempExpected)
	{
		float value = (temperature - minTempExpected) / (maxTempExpected - minTempExpected);
		float num = Mathf.Clamp(value, 0.5f, 1f);
		float s = Mathf.Clamp(value, 0f, 1f);
		return Color.HSVToRGB((10f + (1f - num) * 171f) / 360f, s, 1f);
	}

	// Token: 0x06004FA7 RID: 20391 RVA: 0x001C9ED0 File Offset: 0x001C80D0
	public static Color SolidTemperatureToColor(float temperature, float minTempExpected, float maxTempExpected)
	{
		float num = Mathf.Clamp((temperature - minTempExpected) / (maxTempExpected - minTempExpected), 0.5f, 1f);
		float s = 1f;
		return Color.HSVToRGB((10f + (1f - num) * 171f) / 360f, s, 1f);
	}

	// Token: 0x06004FA8 RID: 20392 RVA: 0x001C9F20 File Offset: 0x001C8120
	public static Color GasTemperatureToColor(float temperature, float minTempExpected, float maxTempExpected)
	{
		float num = Mathf.Clamp((temperature - minTempExpected) / (maxTempExpected - minTempExpected), 0f, 0.5f);
		float s = 1f;
		return Color.HSVToRGB((10f + (1f - num) * 171f) / 360f, s, 1f);
	}

	// Token: 0x06004FA9 RID: 20393 RVA: 0x001C9F70 File Offset: 0x001C8170
	public Color NormalizedTemperature(float actualTemperature)
	{
		float num = this.user_temperatureThresholds[0];
		float num2 = this.user_temperatureThresholds[1];
		float num3 = num2 - num;
		if (actualTemperature < num)
		{
			return GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[0].colorName);
		}
		if (actualTemperature > num2)
		{
			return GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[this.temperatureThresholds.Length - 1].colorName);
		}
		int num4 = 0;
		float t = 0f;
		Game.TemperatureOverlayModes temperatureOverlayMode = Game.Instance.temperatureOverlayMode;
		if (temperatureOverlayMode != Game.TemperatureOverlayModes.AbsoluteTemperature)
		{
			if (temperatureOverlayMode == Game.TemperatureOverlayModes.RelativeTemperature)
			{
				float num5 = num;
				for (int i = 0; i < SimDebugView.relativeTemperatureColorIntervals.Length; i++)
				{
					if (actualTemperature < num5 + SimDebugView.relativeTemperatureColorIntervals[i] * num3)
					{
						num4 = i;
						break;
					}
					num5 += SimDebugView.relativeTemperatureColorIntervals[i] * num3;
				}
				t = (actualTemperature - num5) / (SimDebugView.relativeTemperatureColorIntervals[num4] * num3);
			}
		}
		else
		{
			float num6 = num;
			for (int j = 0; j < SimDebugView.absoluteTemperatureColorIntervals.Length; j++)
			{
				if (actualTemperature < num6 + SimDebugView.absoluteTemperatureColorIntervals[j])
				{
					num4 = j;
					break;
				}
				num6 += SimDebugView.absoluteTemperatureColorIntervals[j];
			}
			t = (actualTemperature - num6) / SimDebugView.absoluteTemperatureColorIntervals[num4];
		}
		return Color.Lerp(GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[num4].colorName), GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[num4 + 1].colorName), t);
	}

	// Token: 0x06004FAA RID: 20394 RVA: 0x001CA104 File Offset: 0x001C8304
	public Color NormalizedHeatFlow(int cell)
	{
		int num = 0;
		int num2 = 0;
		float thermalComfort = GameUtil.GetThermalComfort(GameTags.Minions.Models.Standard, cell, -DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_BASE_GENERATION_KILOWATTS);
		for (int i = 0; i < this.heatFlowThresholds.Length; i++)
		{
			if (thermalComfort <= this.heatFlowThresholds[i].value)
			{
				num2 = i;
				break;
			}
			num = i;
			num2 = i;
		}
		float num3 = 0f;
		if (num != num2)
		{
			num3 = (thermalComfort - this.heatFlowThresholds[num].value) / (this.heatFlowThresholds[num2].value - this.heatFlowThresholds[num].value);
		}
		num3 = Mathf.Max(num3, 0f);
		num3 = Mathf.Min(num3, 1f);
		Color result = Color.Lerp(GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[num].colorName), GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[num2].colorName), num3);
		if (Grid.Solid[cell])
		{
			result = Color.black;
		}
		return result;
	}

	// Token: 0x06004FAB RID: 20395 RVA: 0x001CA22A File Offset: 0x001C842A
	private static bool IsInsulated(int cell)
	{
		return (Grid.Element[cell].state & Element.State.TemperatureInsulated) > Element.State.Vacuum;
	}

	// Token: 0x06004FAC RID: 20396 RVA: 0x001CA240 File Offset: 0x001C8440
	private static Color GetDiseaseColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (Grid.DiseaseIdx[cell] != 255)
		{
			Disease disease = Db.Get().Diseases[(int)Grid.DiseaseIdx[cell]];
			result = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
			result.a = SimUtil.DiseaseCountToAlpha(Grid.DiseaseCount[cell]);
		}
		else
		{
			result.a = 0f;
		}
		return result;
	}

	// Token: 0x06004FAD RID: 20397 RVA: 0x001CA2C1 File Offset: 0x001C84C1
	private static Color GetHeatFlowColour(SimDebugView instance, int cell)
	{
		return instance.NormalizedHeatFlow(cell);
	}

	// Token: 0x06004FAE RID: 20398 RVA: 0x001CA2CA File Offset: 0x001C84CA
	private static Color GetBlack(SimDebugView instance, int cell)
	{
		return Color.black;
	}

	// Token: 0x06004FAF RID: 20399 RVA: 0x001CA2D4 File Offset: 0x001C84D4
	public static Color GetLightColour(SimDebugView instance, int cell)
	{
		Color result = GlobalAssets.Instance.colorSet.lightOverlay;
		result.a = Mathf.Clamp(Mathf.Sqrt((float)(Grid.LightIntensity[cell] + LightGridManager.previewLux[cell])) / Mathf.Sqrt(80000f), 0f, 1f);
		if (Grid.LightIntensity[cell] > DUPLICANTSTATS.STANDARD.Light.LUX_SUNBURN)
		{
			float num = ((float)Grid.LightIntensity[cell] + (float)LightGridManager.previewLux[cell] - (float)DUPLICANTSTATS.STANDARD.Light.LUX_SUNBURN) / (float)(80000 - DUPLICANTSTATS.STANDARD.Light.LUX_SUNBURN);
			num /= 10f;
			result.r += Mathf.Min(0.1f, PerlinSimplexNoise.noise(Grid.CellToPos2D(cell).x / 8f, Grid.CellToPos2D(cell).y / 8f + (float)instance.currentFrame / 32f) * num);
		}
		return result;
	}

	// Token: 0x06004FB0 RID: 20400 RVA: 0x001CA3E4 File Offset: 0x001C85E4
	public static Color GetRadiationColour(SimDebugView instance, int cell)
	{
		float a = Mathf.Clamp(Mathf.Sqrt(Grid.Radiation[cell]) / 30f, 0f, 1f);
		return new Color(0.2f, 0.9f, 0.3f, a);
	}

	// Token: 0x06004FB1 RID: 20401 RVA: 0x001CA42C File Offset: 0x001C862C
	public static Color GetRoomsColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (Grid.IsValidCell(instance.selectedCell))
		{
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			if (cavityForCell != null && cavityForCell.room != null)
			{
				Room room = cavityForCell.room;
				result = GlobalAssets.Instance.colorSet.GetColorByName(room.roomType.category.colorName);
				result.a = 0.45f;
				if (Game.Instance.roomProber.GetCavityForCell(instance.selectedCell) == cavityForCell)
				{
					result.a += 0.3f;
				}
			}
		}
		return result;
	}

	// Token: 0x06004FB2 RID: 20402 RVA: 0x001CA4CC File Offset: 0x001C86CC
	public static Color GetJoulesColour(SimDebugView instance, int cell)
	{
		float num = Grid.Element[cell].specificHeatCapacity * Grid.Temperature[cell] * (Grid.Mass[cell] * 1000f);
		float t = 0.5f * num / (ElementLoader.FindElementByHash(SimHashes.SandStone).specificHeatCapacity * 294f * 1000000f);
		return Color.Lerp(Color.black, Color.red, t);
	}

	// Token: 0x06004FB3 RID: 20403 RVA: 0x001CA538 File Offset: 0x001C8738
	public static Color GetNormalizedTemperatureColourMode(SimDebugView instance, int cell)
	{
		switch (Game.Instance.temperatureOverlayMode)
		{
		case Game.TemperatureOverlayModes.AbsoluteTemperature:
			return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
		case Game.TemperatureOverlayModes.AdaptiveTemperature:
			return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
		case Game.TemperatureOverlayModes.HeatFlow:
			return SimDebugView.GetHeatFlowColour(instance, cell);
		case Game.TemperatureOverlayModes.StateChange:
			return SimDebugView.GetStateChangeProximityColour(instance, cell);
		default:
			return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
		}
	}

	// Token: 0x06004FB4 RID: 20404 RVA: 0x001CA590 File Offset: 0x001C8790
	public static Color GetStateChangeProximityColour(SimDebugView instance, int cell)
	{
		float temperature = Grid.Temperature[cell];
		Element element = Grid.Element[cell];
		float num = element.lowTemp;
		float num2 = element.highTemp;
		if (element.IsGas)
		{
			num2 = Mathf.Min(num + 150f, num2);
			return SimDebugView.GasTemperatureToColor(temperature, num, num2);
		}
		if (element.IsSolid)
		{
			num = Mathf.Max(num2 - 150f, num);
			return SimDebugView.SolidTemperatureToColor(temperature, num, num2);
		}
		return SimDebugView.TemperatureToColor(temperature, num, num2);
	}

	// Token: 0x06004FB5 RID: 20405 RVA: 0x001CA608 File Offset: 0x001C8808
	public static Color GetNormalizedTemperatureColour(SimDebugView instance, int cell)
	{
		float actualTemperature = Grid.Temperature[cell];
		return instance.NormalizedTemperature(actualTemperature);
	}

	// Token: 0x06004FB6 RID: 20406 RVA: 0x001CA628 File Offset: 0x001C8828
	private static Color GetGameGridColour(SimDebugView instance, int cell)
	{
		Color result = new Color32(0, 0, 0, byte.MaxValue);
		switch (instance.gameGridMode)
		{
		case SimDebugView.GameGridMode.GameSolidMap:
			result = (Grid.Solid[cell] ? Color.white : Color.black);
			break;
		case SimDebugView.GameGridMode.Lighting:
			result = ((Grid.LightCount[cell] > 0 || LightGridManager.previewLux[cell] > 0) ? Color.white : Color.black);
			break;
		case SimDebugView.GameGridMode.DigAmount:
			if (Grid.Element[cell].IsSolid)
			{
				float num = Grid.Damage[cell] / 255f;
				result = Color.HSVToRGB(1f - num, 1f, 1f);
			}
			break;
		case SimDebugView.GameGridMode.DupePassable:
			result = (Grid.DupePassable[cell] ? Color.white : Color.black);
			break;
		}
		return result;
	}

	// Token: 0x06004FB7 RID: 20407 RVA: 0x001CA709 File Offset: 0x001C8909
	public Color32 GetColourForID(int id)
	{
		return this.networkColours[id % this.networkColours.Length];
	}

	// Token: 0x06004FB8 RID: 20408 RVA: 0x001CA720 File Offset: 0x001C8920
	private static Color GetThermalConductivityColour(SimDebugView instance, int cell)
	{
		bool flag = SimDebugView.IsInsulated(cell);
		Color black = Color.black;
		float num = instance.maxThermalConductivity - instance.minThermalConductivity;
		if (!flag && num != 0f)
		{
			float num2 = (Grid.Element[cell].thermalConductivity - instance.minThermalConductivity) / num;
			num2 = Mathf.Max(num2, 0f);
			num2 = Mathf.Min(num2, 1f);
			black = new Color(num2, num2, num2);
		}
		return black;
	}

	// Token: 0x06004FB9 RID: 20409 RVA: 0x001CA78C File Offset: 0x001C898C
	private static Color GetPressureMapColour(SimDebugView instance, int cell)
	{
		Color32 c = Color.black;
		if (Grid.Pressure[cell] > 0f)
		{
			float num = Mathf.Clamp((Grid.Pressure[cell] - instance.minPressureExpected) / (instance.maxPressureExpected - instance.minPressureExpected), 0f, 1f) * 0.9f;
			c = new Color(num, num, num, 1f);
		}
		return c;
	}

	// Token: 0x06004FBA RID: 20410 RVA: 0x001CA804 File Offset: 0x001C8A04
	private static Color GetOxygenMapColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (!Grid.IsLiquid(cell) && !Grid.Solid[cell])
		{
			if (Grid.Mass[cell] > SimDebugView.minimumBreathable && (Grid.Element[cell].id == SimHashes.Oxygen || Grid.Element[cell].id == SimHashes.ContaminatedOxygen))
			{
				float time = Mathf.Clamp((Grid.Mass[cell] - SimDebugView.minimumBreathable) / SimDebugView.optimallyBreathable, 0f, 1f);
				result = instance.breathableGradient.Evaluate(time);
			}
			else
			{
				result = instance.unbreathableColour;
			}
		}
		return result;
	}

	// Token: 0x06004FBB RID: 20411 RVA: 0x001CA8AC File Offset: 0x001C8AAC
	private static Color GetTileColour(SimDebugView instance, int cell)
	{
		float num = 0.33f;
		Color result = new Color(num, num, num);
		Element element = Grid.Element[cell];
		bool flag = false;
		foreach (Tag search_tag in Game.Instance.tileOverlayFilters)
		{
			if (element.HasTag(search_tag))
			{
				flag = true;
			}
		}
		if (flag)
		{
			result = element.substance.uiColour;
		}
		return result;
	}

	// Token: 0x06004FBC RID: 20412 RVA: 0x001CA93C File Offset: 0x001C8B3C
	private static Color GetTileTypeColour(SimDebugView instance, int cell)
	{
		return Grid.Element[cell].substance.uiColour;
	}

	// Token: 0x06004FBD RID: 20413 RVA: 0x001CA954 File Offset: 0x001C8B54
	private static Color GetStateMapColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		switch (Grid.Element[cell].state & Element.State.Solid)
		{
		case Element.State.Gas:
			result = Color.yellow;
			break;
		case Element.State.Liquid:
			result = Color.green;
			break;
		case Element.State.Solid:
			result = Color.blue;
			break;
		}
		return result;
	}

	// Token: 0x06004FBE RID: 20414 RVA: 0x001CA9A8 File Offset: 0x001C8BA8
	private static Color GetSolidLiquidMapColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		switch (Grid.Element[cell].state & Element.State.Solid)
		{
		case Element.State.Liquid:
			result = Color.green;
			break;
		case Element.State.Solid:
			result = Color.blue;
			break;
		}
		return result;
	}

	// Token: 0x06004FBF RID: 20415 RVA: 0x001CA9F4 File Offset: 0x001C8BF4
	private static Color GetStateChangeColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		Element element = Grid.Element[cell];
		if (!element.IsVacuum)
		{
			float num = Grid.Temperature[cell];
			float num2 = element.lowTemp * 0.05f;
			float a = Mathf.Abs(num - element.lowTemp) / num2;
			float num3 = element.highTemp * 0.05f;
			float b = Mathf.Abs(num - element.highTemp) / num3;
			float t = Mathf.Max(0f, 1f - Mathf.Min(a, b));
			result = Color.Lerp(Color.black, Color.red, t);
		}
		return result;
	}

	// Token: 0x06004FC0 RID: 20416 RVA: 0x001CAA8C File Offset: 0x001C8C8C
	private static Color GetDecorColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (!Grid.Solid[cell])
		{
			float num = GameUtil.GetDecorAtCell(cell) / 100f;
			if (num > 0f)
			{
				result = Color.Lerp(GlobalAssets.Instance.colorSet.decorBaseline, GlobalAssets.Instance.colorSet.decorPositive, Mathf.Abs(num));
			}
			else
			{
				result = Color.Lerp(GlobalAssets.Instance.colorSet.decorBaseline, GlobalAssets.Instance.colorSet.decorNegative, Mathf.Abs(num));
			}
		}
		return result;
	}

	// Token: 0x06004FC1 RID: 20417 RVA: 0x001CAB2C File Offset: 0x001C8D2C
	private static Color GetDangerColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		SimDebugView.DangerAmount dangerAmount = SimDebugView.DangerAmount.None;
		if (!Grid.Element[cell].IsSolid)
		{
			float num = 0f;
			if (Grid.Temperature[cell] < SimDebugView.minMinionTemperature)
			{
				num = Mathf.Abs(Grid.Temperature[cell] - SimDebugView.minMinionTemperature);
			}
			if (Grid.Temperature[cell] > SimDebugView.maxMinionTemperature)
			{
				num = Mathf.Abs(Grid.Temperature[cell] - SimDebugView.maxMinionTemperature);
			}
			if (num > 0f)
			{
				if (num < 10f)
				{
					dangerAmount = SimDebugView.DangerAmount.VeryLow;
				}
				else if (num < 30f)
				{
					dangerAmount = SimDebugView.DangerAmount.Low;
				}
				else if (num < 100f)
				{
					dangerAmount = SimDebugView.DangerAmount.Moderate;
				}
				else if (num < 200f)
				{
					dangerAmount = SimDebugView.DangerAmount.High;
				}
				else if (num < 400f)
				{
					dangerAmount = SimDebugView.DangerAmount.VeryHigh;
				}
				else if (num > 800f)
				{
					dangerAmount = SimDebugView.DangerAmount.Extreme;
				}
			}
		}
		if (dangerAmount < SimDebugView.DangerAmount.VeryHigh && (Grid.Element[cell].IsVacuum || (Grid.Element[cell].IsGas && (Grid.Element[cell].id != SimHashes.Oxygen || Grid.Pressure[cell] < SimDebugView.minMinionPressure))))
		{
			dangerAmount++;
		}
		if (dangerAmount != SimDebugView.DangerAmount.None)
		{
			float num2 = (float)dangerAmount / 6f;
			result = Color.HSVToRGB((80f - num2 * 80f) / 360f, 1f, 1f);
		}
		return result;
	}

	// Token: 0x06004FC2 RID: 20418 RVA: 0x001CAC74 File Offset: 0x001C8E74
	private static Color GetSimCheckErrorMapColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		Element element = Grid.Element[cell];
		float num = Grid.Mass[cell];
		float num2 = Grid.Temperature[cell];
		if (float.IsNaN(num) || float.IsNaN(num2) || num > 10000f || num2 > 10000f)
		{
			return Color.red;
		}
		if (element.IsVacuum)
		{
			if (num2 != 0f)
			{
				result = Color.yellow;
			}
			else if (num != 0f)
			{
				result = Color.blue;
			}
			else
			{
				result = Color.gray;
			}
		}
		else if (num2 < 10f)
		{
			result = Color.red;
		}
		else if (Grid.Mass[cell] < 1f && Grid.Pressure[cell] < 1f)
		{
			result = Color.green;
		}
		else if (num2 > element.highTemp + 3f && element.highTempTransition != null)
		{
			result = Color.magenta;
		}
		else if (num2 < element.lowTemp + 3f && element.lowTempTransition != null)
		{
			result = Color.cyan;
		}
		return result;
	}

	// Token: 0x06004FC3 RID: 20419 RVA: 0x001CAD7C File Offset: 0x001C8F7C
	private static Color GetFakeFloorColour(SimDebugView instance, int cell)
	{
		if (!Grid.FakeFloor[cell])
		{
			return Color.black;
		}
		return Color.cyan;
	}

	// Token: 0x06004FC4 RID: 20420 RVA: 0x001CAD96 File Offset: 0x001C8F96
	private static Color GetFoundationColour(SimDebugView instance, int cell)
	{
		if (!Grid.Foundation[cell])
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06004FC5 RID: 20421 RVA: 0x001CADB0 File Offset: 0x001C8FB0
	private static Color GetDupePassableColour(SimDebugView instance, int cell)
	{
		if (!Grid.DupePassable[cell])
		{
			return Color.black;
		}
		return Color.green;
	}

	// Token: 0x06004FC6 RID: 20422 RVA: 0x001CADCA File Offset: 0x001C8FCA
	private static Color GetCritterImpassableColour(SimDebugView instance, int cell)
	{
		if (!Grid.CritterImpassable[cell])
		{
			return Color.black;
		}
		return Color.yellow;
	}

	// Token: 0x06004FC7 RID: 20423 RVA: 0x001CADE4 File Offset: 0x001C8FE4
	private static Color GetDupeImpassableColour(SimDebugView instance, int cell)
	{
		if (!Grid.DupeImpassable[cell])
		{
			return Color.black;
		}
		return Color.red;
	}

	// Token: 0x06004FC8 RID: 20424 RVA: 0x001CADFE File Offset: 0x001C8FFE
	private static Color GetMinionOccupiedColour(SimDebugView instance, int cell)
	{
		if (!(Grid.Objects[cell, 0] != null))
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06004FC9 RID: 20425 RVA: 0x001CAE1F File Offset: 0x001C901F
	private static Color GetMinionGroupProberColour(SimDebugView instance, int cell)
	{
		if (!MinionGroupProber.Get().IsReachable(cell))
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06004FCA RID: 20426 RVA: 0x001CAE39 File Offset: 0x001C9039
	private static Color GetPathProberColour(SimDebugView instance, int cell)
	{
		if (!(instance.selectedPathProber != null) || instance.selectedPathProber.GetCost(cell) == -1)
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06004FCB RID: 20427 RVA: 0x001CAE63 File Offset: 0x001C9063
	private static Color GetReservedColour(SimDebugView instance, int cell)
	{
		if (!Grid.Reserved[cell])
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06004FCC RID: 20428 RVA: 0x001CAE7D File Offset: 0x001C907D
	private static Color GetAllowPathFindingColour(SimDebugView instance, int cell)
	{
		if (!Grid.AllowPathfinding[cell])
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06004FCD RID: 20429 RVA: 0x001CAE98 File Offset: 0x001C9098
	private static Color GetMassColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (!SimDebugView.IsInsulated(cell))
		{
			float num = Grid.Mass[cell];
			if (num > 0f)
			{
				float num2 = (num - SimDebugView.Instance.minMassExpected) / (SimDebugView.Instance.maxMassExpected - SimDebugView.Instance.minMassExpected);
				result = Color.HSVToRGB(1f - num2, 1f, 1f);
			}
		}
		return result;
	}

	// Token: 0x06004FCE RID: 20430 RVA: 0x001CAF02 File Offset: 0x001C9102
	public static Color GetScenePartitionerColour(SimDebugView instance, int cell)
	{
		if (!GameScenePartitioner.Instance.DoDebugLayersContainItemsOnCell(cell))
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x040034E2 RID: 13538
	[SerializeField]
	public Material material;

	// Token: 0x040034E3 RID: 13539
	public Material diseaseMaterial;

	// Token: 0x040034E4 RID: 13540
	public bool hideFOW;

	// Token: 0x040034E5 RID: 13541
	public const int colourSize = 4;

	// Token: 0x040034E6 RID: 13542
	private byte[] texBytes;

	// Token: 0x040034E7 RID: 13543
	private int currentFrame;

	// Token: 0x040034E8 RID: 13544
	[SerializeField]
	private Texture2D tex;

	// Token: 0x040034E9 RID: 13545
	[SerializeField]
	private GameObject plane;

	// Token: 0x040034EA RID: 13546
	private HashedString mode = global::OverlayModes.Power.ID;

	// Token: 0x040034EB RID: 13547
	private SimDebugView.GameGridMode gameGridMode = SimDebugView.GameGridMode.DigAmount;

	// Token: 0x040034EC RID: 13548
	private PathProber selectedPathProber;

	// Token: 0x040034ED RID: 13549
	public float minTempExpected = 173.15f;

	// Token: 0x040034EE RID: 13550
	public float maxTempExpected = 423.15f;

	// Token: 0x040034EF RID: 13551
	public float minMassExpected = 1.0001f;

	// Token: 0x040034F0 RID: 13552
	public float maxMassExpected = 10000f;

	// Token: 0x040034F1 RID: 13553
	public float minPressureExpected = 1.300003f;

	// Token: 0x040034F2 RID: 13554
	public float maxPressureExpected = 201.3f;

	// Token: 0x040034F3 RID: 13555
	public float minThermalConductivity;

	// Token: 0x040034F4 RID: 13556
	public float maxThermalConductivity = 30f;

	// Token: 0x040034F5 RID: 13557
	public float thresholdRange = 0.001f;

	// Token: 0x040034F6 RID: 13558
	public float thresholdOpacity = 0.8f;

	// Token: 0x040034F7 RID: 13559
	public static float minimumBreathable = 0.05f;

	// Token: 0x040034F8 RID: 13560
	public static float optimallyBreathable = 1f;

	// Token: 0x040034F9 RID: 13561
	public SimDebugView.ColorThreshold[] temperatureThresholds;

	// Token: 0x040034FA RID: 13562
	public Vector2 user_temperatureThresholds = Vector2.zero;

	// Token: 0x040034FB RID: 13563
	public SimDebugView.ColorThreshold[] heatFlowThresholds;

	// Token: 0x040034FC RID: 13564
	public Color32[] networkColours;

	// Token: 0x040034FD RID: 13565
	public Gradient breathableGradient = new Gradient();

	// Token: 0x040034FE RID: 13566
	public Color32 unbreathableColour = new Color(0.5f, 0f, 0f);

	// Token: 0x040034FF RID: 13567
	public Color32[] toxicColour = new Color32[]
	{
		new Color(0.5f, 0f, 0.5f),
		new Color(1f, 0f, 1f)
	};

	// Token: 0x04003500 RID: 13568
	public static SimDebugView Instance;

	// Token: 0x04003501 RID: 13569
	private WorkItemCollection<SimDebugView.UpdateSimViewWorkItem, SimDebugView.UpdateSimViewSharedData> updateSimViewWorkItems = new WorkItemCollection<SimDebugView.UpdateSimViewWorkItem, SimDebugView.UpdateSimViewSharedData>();

	// Token: 0x04003502 RID: 13570
	private int selectedCell;

	// Token: 0x04003503 RID: 13571
	private Dictionary<HashedString, Action<SimDebugView, Texture>> dataUpdateFuncs = new Dictionary<HashedString, Action<SimDebugView, Texture>>
	{
		{
			global::OverlayModes.Temperature.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear)
		},
		{
			global::OverlayModes.Oxygen.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear)
		},
		{
			global::OverlayModes.Decor.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear)
		},
		{
			global::OverlayModes.TileMode.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDefaultPoint)
		},
		{
			global::OverlayModes.Disease.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDisease)
		}
	};

	// Token: 0x04003504 RID: 13572
	private static float[] relativeTemperatureColorIntervals = new float[]
	{
		0.4f,
		0.05f,
		0.05f,
		0.05f,
		0.05f,
		0.2f,
		0.2f
	};

	// Token: 0x04003505 RID: 13573
	private static float[] absoluteTemperatureColorIntervals = new float[]
	{
		273.15f,
		10f,
		10f,
		10f,
		7f,
		63f,
		1700f,
		10000f
	};

	// Token: 0x04003506 RID: 13574
	private Dictionary<HashedString, Func<SimDebugView, int, Color>> getColourFuncs = new Dictionary<HashedString, Func<SimDebugView, int, Color>>
	{
		{
			global::OverlayModes.ThermalConductivity.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetThermalConductivityColour)
		},
		{
			global::OverlayModes.Temperature.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetNormalizedTemperatureColourMode)
		},
		{
			global::OverlayModes.Disease.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDiseaseColour)
		},
		{
			global::OverlayModes.Decor.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDecorColour)
		},
		{
			global::OverlayModes.Oxygen.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetOxygenMapColour)
		},
		{
			global::OverlayModes.Light.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetLightColour)
		},
		{
			global::OverlayModes.Radiation.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetRadiationColour)
		},
		{
			global::OverlayModes.Rooms.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetRoomsColour)
		},
		{
			global::OverlayModes.TileMode.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetTileColour)
		},
		{
			global::OverlayModes.Suit.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
		},
		{
			global::OverlayModes.Priorities.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
		},
		{
			global::OverlayModes.Crop.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
		},
		{
			global::OverlayModes.Harvest.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
		},
		{
			SimDebugView.OverlayModes.GameGrid,
			new Func<SimDebugView, int, Color>(SimDebugView.GetGameGridColour)
		},
		{
			SimDebugView.OverlayModes.StateChange,
			new Func<SimDebugView, int, Color>(SimDebugView.GetStateChangeColour)
		},
		{
			SimDebugView.OverlayModes.SimCheckErrorMap,
			new Func<SimDebugView, int, Color>(SimDebugView.GetSimCheckErrorMapColour)
		},
		{
			SimDebugView.OverlayModes.Foundation,
			new Func<SimDebugView, int, Color>(SimDebugView.GetFoundationColour)
		},
		{
			SimDebugView.OverlayModes.FakeFloor,
			new Func<SimDebugView, int, Color>(SimDebugView.GetFakeFloorColour)
		},
		{
			SimDebugView.OverlayModes.DupePassable,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDupePassableColour)
		},
		{
			SimDebugView.OverlayModes.DupeImpassable,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDupeImpassableColour)
		},
		{
			SimDebugView.OverlayModes.CritterImpassable,
			new Func<SimDebugView, int, Color>(SimDebugView.GetCritterImpassableColour)
		},
		{
			SimDebugView.OverlayModes.MinionGroupProber,
			new Func<SimDebugView, int, Color>(SimDebugView.GetMinionGroupProberColour)
		},
		{
			SimDebugView.OverlayModes.PathProber,
			new Func<SimDebugView, int, Color>(SimDebugView.GetPathProberColour)
		},
		{
			SimDebugView.OverlayModes.Reserved,
			new Func<SimDebugView, int, Color>(SimDebugView.GetReservedColour)
		},
		{
			SimDebugView.OverlayModes.AllowPathFinding,
			new Func<SimDebugView, int, Color>(SimDebugView.GetAllowPathFindingColour)
		},
		{
			SimDebugView.OverlayModes.Danger,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDangerColour)
		},
		{
			SimDebugView.OverlayModes.MinionOccupied,
			new Func<SimDebugView, int, Color>(SimDebugView.GetMinionOccupiedColour)
		},
		{
			SimDebugView.OverlayModes.Pressure,
			new Func<SimDebugView, int, Color>(SimDebugView.GetPressureMapColour)
		},
		{
			SimDebugView.OverlayModes.TileType,
			new Func<SimDebugView, int, Color>(SimDebugView.GetTileTypeColour)
		},
		{
			SimDebugView.OverlayModes.State,
			new Func<SimDebugView, int, Color>(SimDebugView.GetStateMapColour)
		},
		{
			SimDebugView.OverlayModes.SolidLiquid,
			new Func<SimDebugView, int, Color>(SimDebugView.GetSolidLiquidMapColour)
		},
		{
			SimDebugView.OverlayModes.Mass,
			new Func<SimDebugView, int, Color>(SimDebugView.GetMassColour)
		},
		{
			SimDebugView.OverlayModes.Joules,
			new Func<SimDebugView, int, Color>(SimDebugView.GetJoulesColour)
		},
		{
			SimDebugView.OverlayModes.ScenePartitioner,
			new Func<SimDebugView, int, Color>(SimDebugView.GetScenePartitionerColour)
		}
	};

	// Token: 0x04003507 RID: 13575
	public static readonly Color[] dbColours = new Color[]
	{
		new Color(0f, 0f, 0f, 0f),
		new Color(1f, 1f, 1f, 0.3f),
		new Color(0.7058824f, 0.8235294f, 1f, 0.2f),
		new Color(0f, 0.3137255f, 1f, 0.3f),
		new Color(0.7058824f, 1f, 0.7058824f, 0.5f),
		new Color(0.078431375f, 1f, 0f, 0.7f),
		new Color(1f, 0.9019608f, 0.7058824f, 0.9f),
		new Color(1f, 0.8235294f, 0f, 0.9f),
		new Color(1f, 0.7176471f, 0.3019608f, 0.9f),
		new Color(1f, 0.41568628f, 0f, 0.9f),
		new Color(1f, 0.7058824f, 0.7058824f, 1f),
		new Color(1f, 0f, 0f, 1f),
		new Color(1f, 0f, 0f, 1f)
	};

	// Token: 0x04003508 RID: 13576
	private static float minMinionTemperature = 260f;

	// Token: 0x04003509 RID: 13577
	private static float maxMinionTemperature = 310f;

	// Token: 0x0400350A RID: 13578
	private static float minMinionPressure = 80f;

	// Token: 0x02001AC9 RID: 6857
	public static class OverlayModes
	{
		// Token: 0x04007D9D RID: 32157
		public static readonly HashedString Mass = "Mass";

		// Token: 0x04007D9E RID: 32158
		public static readonly HashedString Pressure = "Pressure";

		// Token: 0x04007D9F RID: 32159
		public static readonly HashedString GameGrid = "GameGrid";

		// Token: 0x04007DA0 RID: 32160
		public static readonly HashedString ScenePartitioner = "ScenePartitioner";

		// Token: 0x04007DA1 RID: 32161
		public static readonly HashedString ConduitUpdates = "ConduitUpdates";

		// Token: 0x04007DA2 RID: 32162
		public static readonly HashedString Flow = "Flow";

		// Token: 0x04007DA3 RID: 32163
		public static readonly HashedString StateChange = "StateChange";

		// Token: 0x04007DA4 RID: 32164
		public static readonly HashedString SimCheckErrorMap = "SimCheckErrorMap";

		// Token: 0x04007DA5 RID: 32165
		public static readonly HashedString DupePassable = "DupePassable";

		// Token: 0x04007DA6 RID: 32166
		public static readonly HashedString Foundation = "Foundation";

		// Token: 0x04007DA7 RID: 32167
		public static readonly HashedString FakeFloor = "FakeFloor";

		// Token: 0x04007DA8 RID: 32168
		public static readonly HashedString CritterImpassable = "CritterImpassable";

		// Token: 0x04007DA9 RID: 32169
		public static readonly HashedString DupeImpassable = "DupeImpassable";

		// Token: 0x04007DAA RID: 32170
		public static readonly HashedString MinionGroupProber = "MinionGroupProber";

		// Token: 0x04007DAB RID: 32171
		public static readonly HashedString PathProber = "PathProber";

		// Token: 0x04007DAC RID: 32172
		public static readonly HashedString Reserved = "Reserved";

		// Token: 0x04007DAD RID: 32173
		public static readonly HashedString AllowPathFinding = "AllowPathFinding";

		// Token: 0x04007DAE RID: 32174
		public static readonly HashedString Danger = "Danger";

		// Token: 0x04007DAF RID: 32175
		public static readonly HashedString MinionOccupied = "MinionOccupied";

		// Token: 0x04007DB0 RID: 32176
		public static readonly HashedString TileType = "TileType";

		// Token: 0x04007DB1 RID: 32177
		public static readonly HashedString State = "State";

		// Token: 0x04007DB2 RID: 32178
		public static readonly HashedString SolidLiquid = "SolidLiquid";

		// Token: 0x04007DB3 RID: 32179
		public static readonly HashedString Joules = "Joules";
	}

	// Token: 0x02001ACA RID: 6858
	public enum GameGridMode
	{
		// Token: 0x04007DB5 RID: 32181
		GameSolidMap,
		// Token: 0x04007DB6 RID: 32182
		Lighting,
		// Token: 0x04007DB7 RID: 32183
		RoomMap,
		// Token: 0x04007DB8 RID: 32184
		Style,
		// Token: 0x04007DB9 RID: 32185
		PlantDensity,
		// Token: 0x04007DBA RID: 32186
		DigAmount,
		// Token: 0x04007DBB RID: 32187
		DupePassable
	}

	// Token: 0x02001ACB RID: 6859
	[Serializable]
	public struct ColorThreshold
	{
		// Token: 0x04007DBC RID: 32188
		public string colorName;

		// Token: 0x04007DBD RID: 32189
		public float value;
	}

	// Token: 0x02001ACC RID: 6860
	private struct UpdateSimViewSharedData
	{
		// Token: 0x0600A123 RID: 41251 RVA: 0x0038216A File Offset: 0x0038036A
		public UpdateSimViewSharedData(SimDebugView instance, byte[] texture_bytes, HashedString sim_view_mode, SimDebugView sim_debug_view)
		{
			this.instance = instance;
			this.textureBytes = texture_bytes;
			this.simViewMode = sim_view_mode;
			this.simDebugView = sim_debug_view;
		}

		// Token: 0x04007DBE RID: 32190
		public SimDebugView instance;

		// Token: 0x04007DBF RID: 32191
		public HashedString simViewMode;

		// Token: 0x04007DC0 RID: 32192
		public SimDebugView simDebugView;

		// Token: 0x04007DC1 RID: 32193
		public byte[] textureBytes;
	}

	// Token: 0x02001ACD RID: 6861
	private struct UpdateSimViewWorkItem : IWorkItem<SimDebugView.UpdateSimViewSharedData>
	{
		// Token: 0x0600A124 RID: 41252 RVA: 0x0038218C File Offset: 0x0038038C
		public UpdateSimViewWorkItem(int x0, int y0, int x1, int y1)
		{
			this.x0 = Mathf.Clamp(x0, 0, Grid.WidthInCells - 1);
			this.x1 = Mathf.Clamp(x1, 0, Grid.WidthInCells - 1);
			this.y0 = Mathf.Clamp(y0, 0, Grid.HeightInCells - 1);
			this.y1 = Mathf.Clamp(y1, 0, Grid.HeightInCells - 1);
		}

		// Token: 0x0600A125 RID: 41253 RVA: 0x003821EC File Offset: 0x003803EC
		public void Run(SimDebugView.UpdateSimViewSharedData shared_data)
		{
			Func<SimDebugView, int, Color> func;
			if (!shared_data.instance.getColourFuncs.TryGetValue(shared_data.simViewMode, out func))
			{
				func = new Func<SimDebugView, int, Color>(SimDebugView.GetBlack);
			}
			for (int i = this.y0; i <= this.y1; i++)
			{
				int num = Grid.XYToCell(this.x0, i);
				int num2 = Grid.XYToCell(this.x1, i);
				for (int j = num; j <= num2; j++)
				{
					int num3 = j * 4;
					if (Grid.IsActiveWorld(j))
					{
						Color color = func(shared_data.instance, j);
						shared_data.textureBytes[num3] = (byte)(Mathf.Min(color.r, 1f) * 255f);
						shared_data.textureBytes[num3 + 1] = (byte)(Mathf.Min(color.g, 1f) * 255f);
						shared_data.textureBytes[num3 + 2] = (byte)(Mathf.Min(color.b, 1f) * 255f);
						shared_data.textureBytes[num3 + 3] = (byte)(Mathf.Min(color.a, 1f) * 255f);
					}
					else
					{
						shared_data.textureBytes[num3] = 0;
						shared_data.textureBytes[num3 + 1] = 0;
						shared_data.textureBytes[num3 + 2] = 0;
						shared_data.textureBytes[num3 + 3] = 0;
					}
				}
			}
		}

		// Token: 0x04007DC2 RID: 32194
		private int x0;

		// Token: 0x04007DC3 RID: 32195
		private int y0;

		// Token: 0x04007DC4 RID: 32196
		private int x1;

		// Token: 0x04007DC5 RID: 32197
		private int y1;
	}

	// Token: 0x02001ACE RID: 6862
	public enum DangerAmount
	{
		// Token: 0x04007DC7 RID: 32199
		None,
		// Token: 0x04007DC8 RID: 32200
		VeryLow,
		// Token: 0x04007DC9 RID: 32201
		Low,
		// Token: 0x04007DCA RID: 32202
		Moderate,
		// Token: 0x04007DCB RID: 32203
		High,
		// Token: 0x04007DCC RID: 32204
		VeryHigh,
		// Token: 0x04007DCD RID: 32205
		Extreme,
		// Token: 0x04007DCE RID: 32206
		MAX_DANGERAMOUNT = 6
	}
}
