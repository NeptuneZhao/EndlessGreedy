using System;
using System.Collections.Generic;

// Token: 0x0200040F RID: 1039
public class HashMapObjectPool<PoolKey, PoolValue>
{
	// Token: 0x060015E8 RID: 5608 RVA: 0x000777CD File Offset: 0x000759CD
	public HashMapObjectPool(Func<PoolKey, PoolValue> instantiator, int initialCount = 0)
	{
		this.initialCount = initialCount;
		this.instantiator = instantiator;
	}

	// Token: 0x060015E9 RID: 5609 RVA: 0x000777F0 File Offset: 0x000759F0
	public HashMapObjectPool(HashMapObjectPool<PoolKey, PoolValue>.IPoolDescriptor[] descriptors, int initialCount = 0)
	{
		this.initialCount = initialCount;
		for (int i = 0; i < descriptors.Length; i++)
		{
			if (this.objectPoolMap.ContainsKey(descriptors[i].PoolId))
			{
				Debug.LogWarning(string.Format("HshMapObjectPool alaready contains key of {0}! Skipping!", descriptors[i].PoolId));
			}
			else
			{
				this.objectPoolMap[descriptors[i].PoolId] = new ObjectPool<PoolValue>(new Func<PoolValue>(descriptors[i].GetInstance), initialCount);
			}
		}
	}

	// Token: 0x060015EA RID: 5610 RVA: 0x00077880 File Offset: 0x00075A80
	public PoolValue GetInstance(PoolKey poolId)
	{
		ObjectPool<PoolValue> objectPool;
		if (!this.objectPoolMap.TryGetValue(poolId, out objectPool))
		{
			objectPool = (this.objectPoolMap[poolId] = new ObjectPool<PoolValue>(new Func<PoolValue>(this.PoolInstantiator), this.initialCount));
		}
		this.currentPoolId = poolId;
		return objectPool.GetInstance();
	}

	// Token: 0x060015EB RID: 5611 RVA: 0x000778D4 File Offset: 0x00075AD4
	public void ReleaseInstance(PoolKey poolId, PoolValue inst)
	{
		ObjectPool<PoolValue> objectPool;
		if (inst == null || !this.objectPoolMap.TryGetValue(poolId, out objectPool))
		{
			return;
		}
		objectPool.ReleaseInstance(inst);
	}

	// Token: 0x060015EC RID: 5612 RVA: 0x00077904 File Offset: 0x00075B04
	private PoolValue PoolInstantiator()
	{
		if (this.instantiator == null)
		{
			return default(PoolValue);
		}
		return this.instantiator(this.currentPoolId);
	}

	// Token: 0x04000C75 RID: 3189
	private Dictionary<PoolKey, ObjectPool<PoolValue>> objectPoolMap = new Dictionary<PoolKey, ObjectPool<PoolValue>>();

	// Token: 0x04000C76 RID: 3190
	private int initialCount;

	// Token: 0x04000C77 RID: 3191
	private PoolKey currentPoolId;

	// Token: 0x04000C78 RID: 3192
	private Func<PoolKey, PoolValue> instantiator;

	// Token: 0x0200116A RID: 4458
	public interface IPoolDescriptor
	{
		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06007FAD RID: 32685
		PoolKey PoolId { get; }

		// Token: 0x06007FAE RID: 32686
		PoolValue GetInstance();
	}
}
