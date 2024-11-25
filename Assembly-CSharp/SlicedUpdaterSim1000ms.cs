using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000AA2 RID: 2722
public abstract class SlicedUpdaterSim1000ms<T> : KMonoBehaviour, ISim200ms where T : KMonoBehaviour, ISlicedSim1000ms
{
	// Token: 0x06005010 RID: 20496 RVA: 0x001CCC0F File Offset: 0x001CAE0F
	protected override void OnPrefabInit()
	{
		this.InitializeSlices();
		base.OnPrefabInit();
		SlicedUpdaterSim1000ms<T>.instance = this;
	}

	// Token: 0x06005011 RID: 20497 RVA: 0x001CCC23 File Offset: 0x001CAE23
	protected override void OnForcedCleanUp()
	{
		SlicedUpdaterSim1000ms<T>.instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06005012 RID: 20498 RVA: 0x001CCC34 File Offset: 0x001CAE34
	private void InitializeSlices()
	{
		int num = SlicedUpdaterSim1000ms<T>.NUM_200MS_BUCKETS * this.numSlicesPer200ms;
		this.m_slices = new List<SlicedUpdaterSim1000ms<T>.Slice>();
		for (int i = 0; i < num; i++)
		{
			this.m_slices.Add(new SlicedUpdaterSim1000ms<T>.Slice());
		}
		this.m_nextSliceIdx = 0;
	}

	// Token: 0x06005013 RID: 20499 RVA: 0x001CCC7C File Offset: 0x001CAE7C
	private int GetSliceIdx(T toBeUpdated)
	{
		return toBeUpdated.GetComponent<KPrefabID>().InstanceID % this.m_slices.Count;
	}

	// Token: 0x06005014 RID: 20500 RVA: 0x001CCC9C File Offset: 0x001CAE9C
	public void RegisterUpdate1000ms(T toBeUpdated)
	{
		SlicedUpdaterSim1000ms<T>.Slice slice = this.m_slices[this.GetSliceIdx(toBeUpdated)];
		slice.Register(toBeUpdated);
		DebugUtil.DevAssert(slice.Count < this.maxUpdatesPer200ms, string.Format("The SlicedUpdaterSim1000ms for {0} wants to update no more than {1} instances per 200ms tick, but a slice has grown more than the SlicedUpdaterSim1000ms can support.", typeof(T).Name, this.maxUpdatesPer200ms), null);
	}

	// Token: 0x06005015 RID: 20501 RVA: 0x001CCCF9 File Offset: 0x001CAEF9
	public void UnregisterUpdate1000ms(T toBeUpdated)
	{
		this.m_slices[this.GetSliceIdx(toBeUpdated)].Unregister(toBeUpdated);
	}

	// Token: 0x06005016 RID: 20502 RVA: 0x001CCD14 File Offset: 0x001CAF14
	public void Sim200ms(float dt)
	{
		foreach (SlicedUpdaterSim1000ms<T>.Slice slice in this.m_slices)
		{
			slice.IncrementDt(dt);
		}
		int num = 0;
		int i = 0;
		while (i < this.numSlicesPer200ms)
		{
			SlicedUpdaterSim1000ms<T>.Slice slice2 = this.m_slices[this.m_nextSliceIdx];
			num += slice2.Count;
			if (num > this.maxUpdatesPer200ms && i > 0)
			{
				break;
			}
			slice2.Update();
			i++;
			this.m_nextSliceIdx = (this.m_nextSliceIdx + 1) % this.m_slices.Count;
		}
	}

	// Token: 0x04003533 RID: 13619
	private static int NUM_200MS_BUCKETS = 5;

	// Token: 0x04003534 RID: 13620
	public static SlicedUpdaterSim1000ms<T> instance;

	// Token: 0x04003535 RID: 13621
	[Serialize]
	public int maxUpdatesPer200ms = 300;

	// Token: 0x04003536 RID: 13622
	[Serialize]
	public int numSlicesPer200ms = 3;

	// Token: 0x04003537 RID: 13623
	private List<SlicedUpdaterSim1000ms<T>.Slice> m_slices;

	// Token: 0x04003538 RID: 13624
	private int m_nextSliceIdx;

	// Token: 0x02001AD3 RID: 6867
	private class Slice
	{
		// Token: 0x0600A12F RID: 41263 RVA: 0x0038241E File Offset: 0x0038061E
		public void Register(T toBeUpdated)
		{
			if (this.m_timeSinceLastUpdate == 0f)
			{
				this.m_updateList.Add(toBeUpdated);
				return;
			}
			this.m_recentlyAdded[toBeUpdated] = 0f;
		}

		// Token: 0x0600A130 RID: 41264 RVA: 0x0038244B File Offset: 0x0038064B
		public void Unregister(T toBeUpdated)
		{
			if (!this.m_updateList.Remove(toBeUpdated))
			{
				this.m_recentlyAdded.Remove(toBeUpdated);
			}
		}

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x0600A131 RID: 41265 RVA: 0x00382468 File Offset: 0x00380668
		public int Count
		{
			get
			{
				return this.m_updateList.Count + this.m_recentlyAdded.Count;
			}
		}

		// Token: 0x0600A132 RID: 41266 RVA: 0x00382481 File Offset: 0x00380681
		public List<T> GetUpdateList()
		{
			List<T> list = new List<T>();
			list.AddRange(this.m_updateList);
			list.AddRange(this.m_recentlyAdded.Keys);
			return list;
		}

		// Token: 0x0600A133 RID: 41267 RVA: 0x003824A8 File Offset: 0x003806A8
		public void Update()
		{
			foreach (T t in this.m_updateList)
			{
				t.SlicedSim1000ms(this.m_timeSinceLastUpdate);
			}
			foreach (KeyValuePair<T, float> keyValuePair in this.m_recentlyAdded)
			{
				keyValuePair.Key.SlicedSim1000ms(keyValuePair.Value);
				this.m_updateList.Add(keyValuePair.Key);
			}
			this.m_recentlyAdded.Clear();
			this.m_timeSinceLastUpdate = 0f;
		}

		// Token: 0x0600A134 RID: 41268 RVA: 0x00382580 File Offset: 0x00380780
		public void IncrementDt(float dt)
		{
			this.m_timeSinceLastUpdate += dt;
			if (this.m_recentlyAdded.Count > 0)
			{
				foreach (T t in new List<T>(this.m_recentlyAdded.Keys))
				{
					Dictionary<T, float> recentlyAdded = this.m_recentlyAdded;
					T key = t;
					recentlyAdded[key] += dt;
				}
			}
		}

		// Token: 0x04007DDD RID: 32221
		private float m_timeSinceLastUpdate;

		// Token: 0x04007DDE RID: 32222
		private List<T> m_updateList = new List<T>();

		// Token: 0x04007DDF RID: 32223
		private Dictionary<T, float> m_recentlyAdded = new Dictionary<T, float>();
	}
}
