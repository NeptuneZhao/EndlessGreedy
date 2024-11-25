using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000889 RID: 2185
[AddComponentMenu("KMonoBehaviour/scripts/EntombedItemManager")]
public class EntombedItemManager : KMonoBehaviour, ISim33ms
{
	// Token: 0x06003D47 RID: 15687 RVA: 0x00152CDC File Offset: 0x00150EDC
	[OnDeserialized]
	private void OnDeserialized()
	{
		this.SpawnUncoveredObjects();
		this.AddMassToWorldIfPossible();
		this.PopulateEntombedItemVisualizers();
	}

	// Token: 0x06003D48 RID: 15688 RVA: 0x00152CF0 File Offset: 0x00150EF0
	public static bool CanEntomb(Pickupable pickupable)
	{
		if (pickupable == null)
		{
			return false;
		}
		if (pickupable.storage != null)
		{
			return false;
		}
		int num = Grid.PosToCell(pickupable);
		return Grid.IsValidCell(num) && Grid.Solid[num] && !(Grid.Objects[num, 9] != null) && (pickupable.PrimaryElement.Element.IsSolid && pickupable.GetComponent<ElementChunk>() != null);
	}

	// Token: 0x06003D49 RID: 15689 RVA: 0x00152D72 File Offset: 0x00150F72
	public void Add(Pickupable pickupable)
	{
		this.pickupables.Add(pickupable);
	}

	// Token: 0x06003D4A RID: 15690 RVA: 0x00152D80 File Offset: 0x00150F80
	public void Sim33ms(float dt)
	{
		EntombedItemVisualizer component = Game.Instance.GetComponent<EntombedItemVisualizer>();
		HashSetPool<Pickupable, EntombedItemManager>.PooledHashSet pooledHashSet = HashSetPool<Pickupable, EntombedItemManager>.Allocate();
		foreach (Pickupable pickupable in this.pickupables)
		{
			if (EntombedItemManager.CanEntomb(pickupable))
			{
				pooledHashSet.Add(pickupable);
			}
		}
		this.pickupables.Clear();
		foreach (Pickupable pickupable2 in pooledHashSet)
		{
			int num = Grid.PosToCell(pickupable2);
			PrimaryElement primaryElement = pickupable2.PrimaryElement;
			SimHashes elementID = primaryElement.ElementID;
			float mass = primaryElement.Mass;
			float temperature = primaryElement.Temperature;
			byte diseaseIdx = primaryElement.DiseaseIdx;
			int diseaseCount = primaryElement.DiseaseCount;
			Element element = Grid.Element[num];
			if (elementID == element.id && mass > 0.010000001f && Grid.Mass[num] + mass < element.maxMass)
			{
				SimMessages.AddRemoveSubstance(num, ElementLoader.FindElementByHash(elementID).idx, CellEventLogger.Instance.ElementConsumerSimUpdate, mass, temperature, diseaseIdx, diseaseCount, true, -1);
			}
			else
			{
				component.AddItem(num);
				this.cells.Add(num);
				this.elementIds.Add((int)elementID);
				this.masses.Add(mass);
				this.temperatures.Add(temperature);
				this.diseaseIndices.Add(diseaseIdx);
				this.diseaseCounts.Add(diseaseCount);
			}
			Util.KDestroyGameObject(pickupable2.gameObject);
		}
		pooledHashSet.Recycle();
	}

	// Token: 0x06003D4B RID: 15691 RVA: 0x00152F48 File Offset: 0x00151148
	public void OnSolidChanged(List<int> solid_changed_cells)
	{
		ListPool<int, EntombedItemManager>.PooledList pooledList = ListPool<int, EntombedItemManager>.Allocate();
		foreach (int num in solid_changed_cells)
		{
			if (!Grid.Solid[num])
			{
				pooledList.Add(num);
			}
		}
		ListPool<int, EntombedItemManager>.PooledList pooledList2 = ListPool<int, EntombedItemManager>.Allocate();
		for (int i = 0; i < this.cells.Count; i++)
		{
			int num2 = this.cells[i];
			foreach (int num3 in pooledList)
			{
				if (num2 == num3)
				{
					pooledList2.Add(i);
					break;
				}
			}
		}
		pooledList.Recycle();
		this.SpawnObjects(pooledList2);
		pooledList2.Recycle();
	}

	// Token: 0x06003D4C RID: 15692 RVA: 0x00153034 File Offset: 0x00151234
	private void SpawnUncoveredObjects()
	{
		ListPool<int, EntombedItemManager>.PooledList pooledList = ListPool<int, EntombedItemManager>.Allocate();
		for (int i = 0; i < this.cells.Count; i++)
		{
			int i2 = this.cells[i];
			if (!Grid.Solid[i2])
			{
				pooledList.Add(i);
			}
		}
		this.SpawnObjects(pooledList);
		pooledList.Recycle();
	}

	// Token: 0x06003D4D RID: 15693 RVA: 0x0015308C File Offset: 0x0015128C
	private void AddMassToWorldIfPossible()
	{
		ListPool<int, EntombedItemManager>.PooledList pooledList = ListPool<int, EntombedItemManager>.Allocate();
		for (int i = 0; i < this.cells.Count; i++)
		{
			int num = this.cells[i];
			if (Grid.Solid[num] && Grid.Element[num].id == (SimHashes)this.elementIds[i])
			{
				pooledList.Add(i);
			}
		}
		pooledList.Sort();
		pooledList.Reverse();
		foreach (int item_idx in pooledList)
		{
			EntombedItemManager.Item item = this.GetItem(item_idx);
			this.RemoveItem(item_idx);
			if (item.mass > 1E-45f)
			{
				SimMessages.AddRemoveSubstance(item.cell, ElementLoader.FindElementByHash((SimHashes)item.elementId).idx, CellEventLogger.Instance.ElementConsumerSimUpdate, item.mass, item.temperature, item.diseaseIdx, item.diseaseCount, false, -1);
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06003D4E RID: 15694 RVA: 0x001531A4 File Offset: 0x001513A4
	private void RemoveItem(int item_idx)
	{
		this.cells.RemoveAt(item_idx);
		this.elementIds.RemoveAt(item_idx);
		this.masses.RemoveAt(item_idx);
		this.temperatures.RemoveAt(item_idx);
		this.diseaseIndices.RemoveAt(item_idx);
		this.diseaseCounts.RemoveAt(item_idx);
	}

	// Token: 0x06003D4F RID: 15695 RVA: 0x001531FC File Offset: 0x001513FC
	private EntombedItemManager.Item GetItem(int item_idx)
	{
		return new EntombedItemManager.Item
		{
			cell = this.cells[item_idx],
			elementId = this.elementIds[item_idx],
			mass = this.masses[item_idx],
			temperature = this.temperatures[item_idx],
			diseaseIdx = this.diseaseIndices[item_idx],
			diseaseCount = this.diseaseCounts[item_idx]
		};
	}

	// Token: 0x06003D50 RID: 15696 RVA: 0x00153284 File Offset: 0x00151484
	private void SpawnObjects(List<int> uncovered_item_indices)
	{
		uncovered_item_indices.Sort();
		uncovered_item_indices.Reverse();
		EntombedItemVisualizer component = Game.Instance.GetComponent<EntombedItemVisualizer>();
		foreach (int item_idx in uncovered_item_indices)
		{
			EntombedItemManager.Item item = this.GetItem(item_idx);
			component.RemoveItem(item.cell);
			this.RemoveItem(item_idx);
			Element element = ElementLoader.FindElementByHash((SimHashes)item.elementId);
			if (element != null)
			{
				element.substance.SpawnResource(Grid.CellToPosCCC(item.cell, Grid.SceneLayer.Ore), item.mass, item.temperature, item.diseaseIdx, item.diseaseCount, false, false, false);
			}
		}
	}

	// Token: 0x06003D51 RID: 15697 RVA: 0x00153344 File Offset: 0x00151544
	private void PopulateEntombedItemVisualizers()
	{
		EntombedItemVisualizer component = Game.Instance.GetComponent<EntombedItemVisualizer>();
		foreach (int cell in this.cells)
		{
			component.AddItem(cell);
		}
	}

	// Token: 0x04002561 RID: 9569
	[Serialize]
	private List<int> cells = new List<int>();

	// Token: 0x04002562 RID: 9570
	[Serialize]
	private List<int> elementIds = new List<int>();

	// Token: 0x04002563 RID: 9571
	[Serialize]
	private List<float> masses = new List<float>();

	// Token: 0x04002564 RID: 9572
	[Serialize]
	private List<float> temperatures = new List<float>();

	// Token: 0x04002565 RID: 9573
	[Serialize]
	private List<byte> diseaseIndices = new List<byte>();

	// Token: 0x04002566 RID: 9574
	[Serialize]
	private List<int> diseaseCounts = new List<int>();

	// Token: 0x04002567 RID: 9575
	private List<Pickupable> pickupables = new List<Pickupable>();

	// Token: 0x0200178E RID: 6030
	private struct Item
	{
		// Token: 0x04007316 RID: 29462
		public int cell;

		// Token: 0x04007317 RID: 29463
		public int elementId;

		// Token: 0x04007318 RID: 29464
		public float mass;

		// Token: 0x04007319 RID: 29465
		public float temperature;

		// Token: 0x0400731A RID: 29466
		public byte diseaseIdx;

		// Token: 0x0400731B RID: 29467
		public int diseaseCount;
	}
}
