using System;
using System.Collections.Generic;

// Token: 0x02000640 RID: 1600
public class Accumulators
{
	// Token: 0x06002733 RID: 10035 RVA: 0x000DF579 File Offset: 0x000DD779
	public Accumulators()
	{
		this.elapsedTime = 0f;
		this.accumulated = new KCompactedVector<float>(0);
		this.average = new KCompactedVector<float>(0);
	}

	// Token: 0x06002734 RID: 10036 RVA: 0x000DF5A4 File Offset: 0x000DD7A4
	public HandleVector<int>.Handle Add(string name, KMonoBehaviour owner)
	{
		HandleVector<int>.Handle result = this.accumulated.Allocate(0f);
		this.average.Allocate(0f);
		return result;
	}

	// Token: 0x06002735 RID: 10037 RVA: 0x000DF5C7 File Offset: 0x000DD7C7
	public HandleVector<int>.Handle Remove(HandleVector<int>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return HandleVector<int>.InvalidHandle;
		}
		this.accumulated.Free(handle);
		this.average.Free(handle);
		return HandleVector<int>.InvalidHandle;
	}

	// Token: 0x06002736 RID: 10038 RVA: 0x000DF5F8 File Offset: 0x000DD7F8
	public void Sim200ms(float dt)
	{
		this.elapsedTime += dt;
		if (this.elapsedTime < 3f)
		{
			return;
		}
		this.elapsedTime -= 3f;
		List<float> dataList = this.accumulated.GetDataList();
		List<float> dataList2 = this.average.GetDataList();
		int count = dataList.Count;
		float num = 0.33333334f;
		for (int i = 0; i < count; i++)
		{
			dataList2[i] = dataList[i] * num;
			dataList[i] = 0f;
		}
	}

	// Token: 0x06002737 RID: 10039 RVA: 0x000DF687 File Offset: 0x000DD887
	public float GetAverageRate(HandleVector<int>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return 0f;
		}
		return this.average.GetData(handle);
	}

	// Token: 0x06002738 RID: 10040 RVA: 0x000DF6A4 File Offset: 0x000DD8A4
	public void Accumulate(HandleVector<int>.Handle handle, float amount)
	{
		float data = this.accumulated.GetData(handle);
		this.accumulated.SetData(handle, data + amount);
	}

	// Token: 0x04001687 RID: 5767
	private const float TIME_WINDOW = 3f;

	// Token: 0x04001688 RID: 5768
	private float elapsedTime;

	// Token: 0x04001689 RID: 5769
	private KCompactedVector<float> accumulated;

	// Token: 0x0400168A RID: 5770
	private KCompactedVector<float> average;
}
