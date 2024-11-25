using System;
using UnityEngine;

// Token: 0x020009C5 RID: 2501
public class Polluter : IPolluter
{
	// Token: 0x17000517 RID: 1303
	// (get) Token: 0x0600488F RID: 18575 RVA: 0x0019F668 File Offset: 0x0019D868
	// (set) Token: 0x06004890 RID: 18576 RVA: 0x0019F670 File Offset: 0x0019D870
	public int radius
	{
		get
		{
			return this._radius;
		}
		private set
		{
			this._radius = value;
			if (this._radius == 0)
			{
				global::Debug.LogFormat("[{0}] has a 0 radius noise, this will disable it", new object[]
				{
					this.GetName()
				});
				return;
			}
		}
	}

	// Token: 0x06004891 RID: 18577 RVA: 0x0019F69B File Offset: 0x0019D89B
	public void SetAttributes(Vector2 pos, int dB, GameObject go, string name)
	{
		this.position = pos;
		this.sourceName = name;
		this.decibels = dB;
		this.gameObject = go;
	}

	// Token: 0x06004892 RID: 18578 RVA: 0x0019F6BA File Offset: 0x0019D8BA
	public string GetName()
	{
		return this.sourceName;
	}

	// Token: 0x06004893 RID: 18579 RVA: 0x0019F6C2 File Offset: 0x0019D8C2
	public int GetRadius()
	{
		return this.radius;
	}

	// Token: 0x06004894 RID: 18580 RVA: 0x0019F6CA File Offset: 0x0019D8CA
	public int GetNoise()
	{
		return this.decibels;
	}

	// Token: 0x06004895 RID: 18581 RVA: 0x0019F6D2 File Offset: 0x0019D8D2
	public GameObject GetGameObject()
	{
		return this.gameObject;
	}

	// Token: 0x06004896 RID: 18582 RVA: 0x0019F6DA File Offset: 0x0019D8DA
	public Polluter(int radius)
	{
		this.radius = radius;
	}

	// Token: 0x06004897 RID: 18583 RVA: 0x0019F6E9 File Offset: 0x0019D8E9
	public void SetSplat(NoiseSplat new_splat)
	{
		if (new_splat == null && this.splat != null)
		{
			this.Clear();
		}
		this.splat = new_splat;
		if (this.splat != null)
		{
			AudioEventManager.Get().AddSplat(this.splat);
		}
	}

	// Token: 0x06004898 RID: 18584 RVA: 0x0019F71B File Offset: 0x0019D91B
	public void Clear()
	{
		if (this.splat != null)
		{
			AudioEventManager.Get().ClearNoiseSplat(this.splat);
			this.splat.Clear();
			this.splat = null;
		}
	}

	// Token: 0x06004899 RID: 18585 RVA: 0x0019F747 File Offset: 0x0019D947
	public Vector2 GetPosition()
	{
		return this.position;
	}

	// Token: 0x04002F82 RID: 12162
	private int _radius;

	// Token: 0x04002F83 RID: 12163
	private int decibels;

	// Token: 0x04002F84 RID: 12164
	private Vector2 position;

	// Token: 0x04002F85 RID: 12165
	private string sourceName;

	// Token: 0x04002F86 RID: 12166
	private GameObject gameObject;

	// Token: 0x04002F87 RID: 12167
	private NoiseSplat splat;
}
