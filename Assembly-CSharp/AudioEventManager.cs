using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004FE RID: 1278
[AddComponentMenu("KMonoBehaviour/scripts/AudioEventManager")]
public class AudioEventManager : KMonoBehaviour
{
	// Token: 0x06001C79 RID: 7289 RVA: 0x00095DEC File Offset: 0x00093FEC
	public static AudioEventManager Get()
	{
		if (AudioEventManager.instance == null)
		{
			if (App.IsExiting)
			{
				return null;
			}
			GameObject gameObject = GameObject.Find("/AudioEventManager");
			if (gameObject == null)
			{
				gameObject = new GameObject();
				gameObject.name = "AudioEventManager";
			}
			AudioEventManager.instance = gameObject.GetComponent<AudioEventManager>();
			if (AudioEventManager.instance == null)
			{
				AudioEventManager.instance = gameObject.AddComponent<AudioEventManager>();
			}
		}
		return AudioEventManager.instance;
	}

	// Token: 0x06001C7A RID: 7290 RVA: 0x00095E5C File Offset: 0x0009405C
	protected override void OnSpawn()
	{
		base.OnPrefabInit();
		this.spatialSplats.Reset(Grid.WidthInCells, Grid.HeightInCells, 16, 16);
	}

	// Token: 0x06001C7B RID: 7291 RVA: 0x00095E7D File Offset: 0x0009407D
	public static float LoudnessToDB(float loudness)
	{
		if (loudness <= 0f)
		{
			return 0f;
		}
		return 10f * Mathf.Log10(loudness);
	}

	// Token: 0x06001C7C RID: 7292 RVA: 0x00095E99 File Offset: 0x00094099
	public static float DBToLoudness(float src_db)
	{
		return Mathf.Pow(10f, src_db / 10f);
	}

	// Token: 0x06001C7D RID: 7293 RVA: 0x00095EAC File Offset: 0x000940AC
	public float GetDecibelsAtCell(int cell)
	{
		return Mathf.Round(AudioEventManager.LoudnessToDB(Grid.Loudness[cell]) * 2f) / 2f;
	}

	// Token: 0x06001C7E RID: 7294 RVA: 0x00095ECC File Offset: 0x000940CC
	public static string GetLoudestNoisePolluterAtCell(int cell)
	{
		float negativeInfinity = float.NegativeInfinity;
		string result = null;
		AudioEventManager audioEventManager = AudioEventManager.Get();
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2 pos = new Vector2((float)vector2I.x, (float)vector2I.y);
		foreach (object obj in audioEventManager.spatialSplats.GetAllIntersecting(pos))
		{
			NoiseSplat noiseSplat = (NoiseSplat)obj;
			if (noiseSplat.GetLoudness(cell) > negativeInfinity)
			{
				result = noiseSplat.GetProvider().GetName();
			}
		}
		return result;
	}

	// Token: 0x06001C7F RID: 7295 RVA: 0x00095F70 File Offset: 0x00094170
	public void ClearNoiseSplat(NoiseSplat splat)
	{
		if (this.splats.Contains(splat))
		{
			this.splats.Remove(splat);
			this.spatialSplats.Remove(splat);
		}
	}

	// Token: 0x06001C80 RID: 7296 RVA: 0x00095F99 File Offset: 0x00094199
	public void AddSplat(NoiseSplat splat)
	{
		this.splats.Add(splat);
		this.spatialSplats.Add(splat);
	}

	// Token: 0x06001C81 RID: 7297 RVA: 0x00095FB4 File Offset: 0x000941B4
	public NoiseSplat CreateNoiseSplat(Vector2 pos, int dB, int radius, string name, GameObject go)
	{
		Polluter polluter = this.GetPolluter(radius);
		polluter.SetAttributes(pos, dB, go, name);
		NoiseSplat noiseSplat = new NoiseSplat(polluter, 0f);
		polluter.SetSplat(noiseSplat);
		return noiseSplat;
	}

	// Token: 0x06001C82 RID: 7298 RVA: 0x00095FE8 File Offset: 0x000941E8
	public List<AudioEventManager.PolluterDisplay> GetPollutersForCell(int cell)
	{
		this.polluters.Clear();
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2 pos = new Vector2((float)vector2I.x, (float)vector2I.y);
		foreach (object obj in this.spatialSplats.GetAllIntersecting(pos))
		{
			NoiseSplat noiseSplat = (NoiseSplat)obj;
			float loudness = noiseSplat.GetLoudness(cell);
			if (loudness > 0f)
			{
				AudioEventManager.PolluterDisplay item = default(AudioEventManager.PolluterDisplay);
				item.name = noiseSplat.GetName();
				item.value = AudioEventManager.LoudnessToDB(loudness);
				item.provider = noiseSplat.GetProvider();
				this.polluters.Add(item);
			}
		}
		return this.polluters;
	}

	// Token: 0x06001C83 RID: 7299 RVA: 0x000960C0 File Offset: 0x000942C0
	private void RemoveExpiredSplats()
	{
		if (this.removeTime.Count > 1)
		{
			this.removeTime.Sort((Pair<float, NoiseSplat> a, Pair<float, NoiseSplat> b) => a.first.CompareTo(b.first));
		}
		int num = -1;
		int num2 = 0;
		while (num2 < this.removeTime.Count && this.removeTime[num2].first <= Time.time)
		{
			NoiseSplat second = this.removeTime[num2].second;
			if (second != null)
			{
				IPolluter provider = second.GetProvider();
				this.FreePolluter(provider as Polluter);
			}
			num = num2;
			num2++;
		}
		for (int i = num; i >= 0; i--)
		{
			this.removeTime.RemoveAt(i);
		}
	}

	// Token: 0x06001C84 RID: 7300 RVA: 0x0009617C File Offset: 0x0009437C
	private void Update()
	{
		this.RemoveExpiredSplats();
	}

	// Token: 0x06001C85 RID: 7301 RVA: 0x00096184 File Offset: 0x00094384
	private Polluter GetPolluter(int radius)
	{
		if (!this.freePool.ContainsKey(radius))
		{
			this.freePool.Add(radius, new List<Polluter>());
		}
		Polluter polluter;
		if (this.freePool[radius].Count > 0)
		{
			polluter = this.freePool[radius][0];
			this.freePool[radius].RemoveAt(0);
		}
		else
		{
			polluter = new Polluter(radius);
		}
		if (!this.inusePool.ContainsKey(radius))
		{
			this.inusePool.Add(radius, new List<Polluter>());
		}
		this.inusePool[radius].Add(polluter);
		return polluter;
	}

	// Token: 0x06001C86 RID: 7302 RVA: 0x00096228 File Offset: 0x00094428
	private void FreePolluter(Polluter pol)
	{
		if (pol != null)
		{
			pol.Clear();
			global::Debug.Assert(this.inusePool[pol.radius].Contains(pol));
			this.inusePool[pol.radius].Remove(pol);
			this.freePool[pol.radius].Add(pol);
		}
	}

	// Token: 0x06001C87 RID: 7303 RVA: 0x0009628C File Offset: 0x0009448C
	public void PlayTimedOnceOff(Vector2 pos, int dB, int radius, string name, GameObject go, float time = 1f)
	{
		if (dB > 0 && radius > 0 && time > 0f)
		{
			Polluter polluter = this.GetPolluter(radius);
			polluter.SetAttributes(pos, dB, go, name);
			this.AddTimedInstance(polluter, time);
		}
	}

	// Token: 0x06001C88 RID: 7304 RVA: 0x000962C8 File Offset: 0x000944C8
	private void AddTimedInstance(Polluter p, float time)
	{
		NoiseSplat noiseSplat = new NoiseSplat(p, time + Time.time);
		p.SetSplat(noiseSplat);
		this.removeTime.Add(new Pair<float, NoiseSplat>(time + Time.time, noiseSplat));
	}

	// Token: 0x06001C89 RID: 7305 RVA: 0x00096302 File Offset: 0x00094502
	private static void SoundLog(long itemId, string message)
	{
		global::Debug.Log(" [" + itemId.ToString() + "] \t" + message);
	}

	// Token: 0x0400100C RID: 4108
	public const float NO_NOISE_EFFECTORS = 0f;

	// Token: 0x0400100D RID: 4109
	public const float MIN_LOUDNESS_THRESHOLD = 1f;

	// Token: 0x0400100E RID: 4110
	private static AudioEventManager instance;

	// Token: 0x0400100F RID: 4111
	private List<Pair<float, NoiseSplat>> removeTime = new List<Pair<float, NoiseSplat>>();

	// Token: 0x04001010 RID: 4112
	private Dictionary<int, List<Polluter>> freePool = new Dictionary<int, List<Polluter>>();

	// Token: 0x04001011 RID: 4113
	private Dictionary<int, List<Polluter>> inusePool = new Dictionary<int, List<Polluter>>();

	// Token: 0x04001012 RID: 4114
	private HashSet<NoiseSplat> splats = new HashSet<NoiseSplat>();

	// Token: 0x04001013 RID: 4115
	private UniformGrid<NoiseSplat> spatialSplats = new UniformGrid<NoiseSplat>();

	// Token: 0x04001014 RID: 4116
	private List<AudioEventManager.PolluterDisplay> polluters = new List<AudioEventManager.PolluterDisplay>();

	// Token: 0x020012CB RID: 4811
	public enum NoiseEffect
	{
		// Token: 0x0400647B RID: 25723
		Peaceful,
		// Token: 0x0400647C RID: 25724
		Quiet = 36,
		// Token: 0x0400647D RID: 25725
		TossAndTurn = 45,
		// Token: 0x0400647E RID: 25726
		WakeUp = 60,
		// Token: 0x0400647F RID: 25727
		Passive = 80,
		// Token: 0x04006480 RID: 25728
		Active = 106
	}

	// Token: 0x020012CC RID: 4812
	public struct PolluterDisplay
	{
		// Token: 0x04006481 RID: 25729
		public string name;

		// Token: 0x04006482 RID: 25730
		public float value;

		// Token: 0x04006483 RID: 25731
		public IPolluter provider;
	}
}
