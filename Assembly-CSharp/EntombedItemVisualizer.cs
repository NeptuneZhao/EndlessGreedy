using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200088A RID: 2186
[AddComponentMenu("KMonoBehaviour/scripts/EntombedItemVisualizer")]
public class EntombedItemVisualizer : KMonoBehaviour
{
	// Token: 0x06003D53 RID: 15699 RVA: 0x00153404 File Offset: 0x00151604
	public void Clear()
	{
		this.cellEntombedCounts.Clear();
	}

	// Token: 0x06003D54 RID: 15700 RVA: 0x00153411 File Offset: 0x00151611
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.entombedItemPool = new GameObjectPool(new Func<GameObject>(this.InstantiateEntombedObject), 32);
	}

	// Token: 0x06003D55 RID: 15701 RVA: 0x00153434 File Offset: 0x00151634
	public bool AddItem(int cell)
	{
		bool result = false;
		if (Grid.Objects[cell, 9] == null)
		{
			result = true;
			EntombedItemVisualizer.Data data;
			this.cellEntombedCounts.TryGetValue(cell, out data);
			if (data.refCount == 0)
			{
				GameObject instance = this.entombedItemPool.GetInstance();
				instance.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.FXFront));
				instance.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.value * 360f);
				KBatchedAnimController component = instance.GetComponent<KBatchedAnimController>();
				int num = UnityEngine.Random.Range(0, EntombedItemVisualizer.EntombedVisualizerAnims.Length);
				string text = EntombedItemVisualizer.EntombedVisualizerAnims[num];
				component.initialAnim = text;
				instance.SetActive(true);
				component.Play(text, KAnim.PlayMode.Once, 1f, 0f);
				data.controller = component;
			}
			data.refCount++;
			this.cellEntombedCounts[cell] = data;
		}
		return result;
	}

	// Token: 0x06003D56 RID: 15702 RVA: 0x00153524 File Offset: 0x00151724
	public void RemoveItem(int cell)
	{
		EntombedItemVisualizer.Data data;
		if (this.cellEntombedCounts.TryGetValue(cell, out data))
		{
			data.refCount--;
			if (data.refCount == 0)
			{
				this.ReleaseVisualizer(cell, data);
				return;
			}
			this.cellEntombedCounts[cell] = data;
		}
	}

	// Token: 0x06003D57 RID: 15703 RVA: 0x0015356C File Offset: 0x0015176C
	public void ForceClear(int cell)
	{
		EntombedItemVisualizer.Data data;
		if (this.cellEntombedCounts.TryGetValue(cell, out data))
		{
			this.ReleaseVisualizer(cell, data);
		}
	}

	// Token: 0x06003D58 RID: 15704 RVA: 0x00153594 File Offset: 0x00151794
	private void ReleaseVisualizer(int cell, EntombedItemVisualizer.Data data)
	{
		if (data.controller != null)
		{
			data.controller.gameObject.SetActive(false);
			this.entombedItemPool.ReleaseInstance(data.controller.gameObject);
		}
		this.cellEntombedCounts.Remove(cell);
	}

	// Token: 0x06003D59 RID: 15705 RVA: 0x001535E3 File Offset: 0x001517E3
	public bool IsEntombedItem(int cell)
	{
		return this.cellEntombedCounts.ContainsKey(cell) && this.cellEntombedCounts[cell].refCount > 0;
	}

	// Token: 0x06003D5A RID: 15706 RVA: 0x00153609 File Offset: 0x00151809
	private GameObject InstantiateEntombedObject()
	{
		GameObject gameObject = GameUtil.KInstantiate(this.entombedItemPrefab, Grid.SceneLayer.FXFront, null, 0);
		gameObject.SetActive(false);
		return gameObject;
	}

	// Token: 0x04002568 RID: 9576
	[SerializeField]
	private GameObject entombedItemPrefab;

	// Token: 0x04002569 RID: 9577
	private static readonly string[] EntombedVisualizerAnims = new string[]
	{
		"idle1",
		"idle2",
		"idle3",
		"idle4"
	};

	// Token: 0x0400256A RID: 9578
	private GameObjectPool entombedItemPool;

	// Token: 0x0400256B RID: 9579
	private Dictionary<int, EntombedItemVisualizer.Data> cellEntombedCounts = new Dictionary<int, EntombedItemVisualizer.Data>();

	// Token: 0x0200178F RID: 6031
	private struct Data
	{
		// Token: 0x0400731C RID: 29468
		public int refCount;

		// Token: 0x0400731D RID: 29469
		public KBatchedAnimController controller;
	}
}
