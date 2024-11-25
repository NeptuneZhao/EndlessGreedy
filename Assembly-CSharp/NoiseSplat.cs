using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000510 RID: 1296
public class NoiseSplat : IUniformGridObject
{
	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x06001CC7 RID: 7367 RVA: 0x00097FD2 File Offset: 0x000961D2
	// (set) Token: 0x06001CC8 RID: 7368 RVA: 0x00097FDA File Offset: 0x000961DA
	public int dB { get; private set; }

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06001CC9 RID: 7369 RVA: 0x00097FE3 File Offset: 0x000961E3
	// (set) Token: 0x06001CCA RID: 7370 RVA: 0x00097FEB File Offset: 0x000961EB
	public float deathTime { get; private set; }

	// Token: 0x06001CCB RID: 7371 RVA: 0x00097FF4 File Offset: 0x000961F4
	public string GetName()
	{
		return this.provider.GetName();
	}

	// Token: 0x06001CCC RID: 7372 RVA: 0x00098001 File Offset: 0x00096201
	public IPolluter GetProvider()
	{
		return this.provider;
	}

	// Token: 0x06001CCD RID: 7373 RVA: 0x00098009 File Offset: 0x00096209
	public Vector2 PosMin()
	{
		return new Vector2(this.position.x - (float)this.radius, this.position.y - (float)this.radius);
	}

	// Token: 0x06001CCE RID: 7374 RVA: 0x00098036 File Offset: 0x00096236
	public Vector2 PosMax()
	{
		return new Vector2(this.position.x + (float)this.radius, this.position.y + (float)this.radius);
	}

	// Token: 0x06001CCF RID: 7375 RVA: 0x00098064 File Offset: 0x00096264
	public NoiseSplat(NoisePolluter setProvider, float death_time = 0f)
	{
		this.deathTime = death_time;
		this.dB = 0;
		this.radius = 5;
		if (setProvider.dB != null)
		{
			this.dB = (int)setProvider.dB.GetTotalValue();
		}
		int cell = Grid.PosToCell(setProvider.gameObject);
		if (!NoisePolluter.IsNoiseableCell(cell))
		{
			this.dB = 0;
		}
		if (this.dB == 0)
		{
			return;
		}
		setProvider.Clear();
		OccupyArea occupyArea = setProvider.occupyArea;
		this.baseExtents = occupyArea.GetExtents();
		this.provider = setProvider;
		this.position = setProvider.transform.GetPosition();
		if (setProvider.dBRadius != null)
		{
			this.radius = (int)setProvider.dBRadius.GetTotalValue();
		}
		if (this.radius == 0)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int widthInCells = occupyArea.GetWidthInCells();
		int heightInCells = occupyArea.GetHeightInCells();
		Vector2I vector2I = new Vector2I(num - this.radius, num2 - this.radius);
		Vector2I vector2I2 = vector2I + new Vector2I(this.radius * 2 + widthInCells, this.radius * 2 + heightInCells);
		vector2I = Vector2I.Max(vector2I, Vector2I.zero);
		vector2I2 = Vector2I.Min(vector2I2, new Vector2I(Grid.WidthInCells - 1, Grid.HeightInCells - 1));
		this.effectExtents = new Extents(vector2I.x, vector2I.y, vector2I2.x - vector2I.x, vector2I2.y - vector2I.y);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("NoiseSplat.SplatCollectNoisePolluters", setProvider.gameObject, this.effectExtents, GameScenePartitioner.Instance.noisePolluterLayer, setProvider.onCollectNoisePollutersCallback);
		this.solidChangedPartitionerEntry = GameScenePartitioner.Instance.Add("NoiseSplat.SplatSolidCheck", setProvider.gameObject, this.effectExtents, GameScenePartitioner.Instance.solidChangedLayer, setProvider.refreshPartionerCallback);
	}

	// Token: 0x06001CD0 RID: 7376 RVA: 0x0009824C File Offset: 0x0009644C
	public NoiseSplat(IPolluter setProvider, float death_time = 0f)
	{
		this.deathTime = death_time;
		this.provider = setProvider;
		this.provider.Clear();
		this.position = this.provider.GetPosition();
		this.dB = this.provider.GetNoise();
		int cell = Grid.PosToCell(this.position);
		if (!NoisePolluter.IsNoiseableCell(cell))
		{
			this.dB = 0;
		}
		if (this.dB == 0)
		{
			return;
		}
		this.radius = this.provider.GetRadius();
		if (this.radius == 0)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		Vector2I vector2I = new Vector2I(num - this.radius, num2 - this.radius);
		Vector2I vector2I2 = vector2I + new Vector2I(this.radius * 2, this.radius * 2);
		vector2I = Vector2I.Max(vector2I, Vector2I.zero);
		vector2I2 = Vector2I.Min(vector2I2, new Vector2I(Grid.WidthInCells - 1, Grid.HeightInCells - 1));
		this.effectExtents = new Extents(vector2I.x, vector2I.y, vector2I2.x - vector2I.x, vector2I2.y - vector2I.y);
		this.baseExtents = new Extents(num, num2, 1, 1);
		this.AddNoise();
	}

	// Token: 0x06001CD1 RID: 7377 RVA: 0x00098395 File Offset: 0x00096595
	public void Clear()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
		this.RemoveNoise();
	}

	// Token: 0x06001CD2 RID: 7378 RVA: 0x000983C0 File Offset: 0x000965C0
	private void AddNoise()
	{
		int cell = Grid.PosToCell(this.position);
		int num = this.effectExtents.x + this.effectExtents.width;
		int num2 = this.effectExtents.y + this.effectExtents.height;
		int num3 = this.effectExtents.x;
		int num4 = this.effectExtents.y;
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		num = Math.Min(num, Grid.WidthInCells);
		num2 = Math.Min(num2, Grid.HeightInCells);
		num3 = Math.Max(0, num3);
		num4 = Math.Max(0, num4);
		for (int i = num4; i < num2; i++)
		{
			for (int j = num3; j < num; j++)
			{
				if (Grid.VisibilityTest(x, y, j, i, false))
				{
					int num5 = Grid.XYToCell(j, i);
					float dbforCell = this.GetDBForCell(num5);
					if (dbforCell > 0f)
					{
						float num6 = AudioEventManager.DBToLoudness(dbforCell);
						Grid.Loudness[num5] += num6;
						Pair<int, float> item = new Pair<int, float>(num5, num6);
						this.decibels.Add(item);
					}
				}
			}
		}
	}

	// Token: 0x06001CD3 RID: 7379 RVA: 0x000984D8 File Offset: 0x000966D8
	public float GetDBForCell(int cell)
	{
		Vector2 vector = Grid.CellToPos2D(cell);
		float num = Mathf.Floor(Vector2.Distance(this.position, vector));
		if (vector.x >= (float)this.baseExtents.x && vector.x < (float)(this.baseExtents.x + this.baseExtents.width) && vector.y >= (float)this.baseExtents.y && vector.y < (float)(this.baseExtents.y + this.baseExtents.height))
		{
			num = 0f;
		}
		return Mathf.Round((float)this.dB - (float)this.dB * num * 0.05f);
	}

	// Token: 0x06001CD4 RID: 7380 RVA: 0x00098590 File Offset: 0x00096790
	private void RemoveNoise()
	{
		for (int i = 0; i < this.decibels.Count; i++)
		{
			Pair<int, float> pair = this.decibels[i];
			float num = Math.Max(0f, Grid.Loudness[pair.first] - pair.second);
			Grid.Loudness[pair.first] = ((num < 1f) ? 0f : num);
		}
		this.decibels.Clear();
	}

	// Token: 0x06001CD5 RID: 7381 RVA: 0x00098608 File Offset: 0x00096808
	public float GetLoudness(int cell)
	{
		float result = 0f;
		for (int i = 0; i < this.decibels.Count; i++)
		{
			Pair<int, float> pair = this.decibels[i];
			if (pair.first == cell)
			{
				result = pair.second;
				break;
			}
		}
		return result;
	}

	// Token: 0x04001039 RID: 4153
	public const float noiseFalloff = 0.05f;

	// Token: 0x0400103C RID: 4156
	private IPolluter provider;

	// Token: 0x0400103D RID: 4157
	private Vector2 position;

	// Token: 0x0400103E RID: 4158
	private int radius;

	// Token: 0x0400103F RID: 4159
	private Extents effectExtents;

	// Token: 0x04001040 RID: 4160
	private Extents baseExtents;

	// Token: 0x04001041 RID: 4161
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001042 RID: 4162
	private HandleVector<int>.Handle solidChangedPartitionerEntry;

	// Token: 0x04001043 RID: 4163
	private List<Pair<int, float>> decibels = new List<Pair<int, float>>();
}
