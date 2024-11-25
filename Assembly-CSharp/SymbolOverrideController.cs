using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004F8 RID: 1272
[AddComponentMenu("KMonoBehaviour/scripts/SymbolOverrideController")]
public class SymbolOverrideController : KMonoBehaviour
{
	// Token: 0x170000DE RID: 222
	// (get) Token: 0x06001C55 RID: 7253 RVA: 0x00094E28 File Offset: 0x00093028
	public SymbolOverrideController.SymbolEntry[] GetSymbolOverrides
	{
		get
		{
			return this.symbolOverrides.ToArray();
		}
	}

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x06001C56 RID: 7254 RVA: 0x00094E35 File Offset: 0x00093035
	// (set) Token: 0x06001C57 RID: 7255 RVA: 0x00094E3D File Offset: 0x0009303D
	public int version { get; private set; }

	// Token: 0x06001C58 RID: 7256 RVA: 0x00094E48 File Offset: 0x00093048
	protected override void OnPrefabInit()
	{
		this.animController = base.GetComponent<KBatchedAnimController>();
		DebugUtil.Assert(base.GetComponent<KBatchedAnimController>() != null, "SymbolOverrideController requires KBatchedAnimController");
		DebugUtil.Assert(base.GetComponent<KBatchedAnimController>().usingNewSymbolOverrideSystem, "SymbolOverrideController requires usingNewSymbolOverrideSystem to be set to true. Try adding the component by calling: SymbolOverrideControllerUtil.AddToPrefab");
		for (int i = 0; i < this.symbolOverrides.Count; i++)
		{
			SymbolOverrideController.SymbolEntry symbolEntry = this.symbolOverrides[i];
			symbolEntry.sourceSymbol = KAnimBatchManager.Instance().GetBatchGroupData(symbolEntry.sourceSymbolBatchTag).GetSymbol(symbolEntry.sourceSymbolId);
			this.symbolOverrides[i] = symbolEntry;
		}
		this.atlases = new KAnimBatch.AtlasList(0, KAnimBatchManager.MaxAtlasesByMaterialType[(int)this.animController.materialType]);
		this.faceGraph = base.GetComponent<FaceGraph>();
	}

	// Token: 0x06001C59 RID: 7257 RVA: 0x00094F0C File Offset: 0x0009310C
	public int AddSymbolOverride(HashedString target_symbol, KAnim.Build.Symbol source_symbol, int priority = 0)
	{
		if (source_symbol == null)
		{
			throw new Exception("NULL source symbol when overriding: " + target_symbol.ToString());
		}
		SymbolOverrideController.SymbolEntry symbolEntry = new SymbolOverrideController.SymbolEntry
		{
			targetSymbol = target_symbol,
			sourceSymbol = source_symbol,
			sourceSymbolId = new HashedString(source_symbol.hash.HashValue),
			sourceSymbolBatchTag = source_symbol.build.batchTag,
			priority = priority
		};
		int num = this.GetSymbolOverrideIdx(target_symbol, priority);
		if (num >= 0)
		{
			this.symbolOverrides[num] = symbolEntry;
		}
		else
		{
			num = this.symbolOverrides.Count;
			this.symbolOverrides.Add(symbolEntry);
		}
		this.MarkDirty();
		return num;
	}

	// Token: 0x06001C5A RID: 7258 RVA: 0x00094FC0 File Offset: 0x000931C0
	public bool RemoveSymbolOverride(HashedString target_symbol, int priority = 0)
	{
		for (int i = 0; i < this.symbolOverrides.Count; i++)
		{
			SymbolOverrideController.SymbolEntry symbolEntry = this.symbolOverrides[i];
			if (symbolEntry.targetSymbol == target_symbol && symbolEntry.priority == priority)
			{
				this.symbolOverrides.RemoveAt(i);
				this.MarkDirty();
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001C5B RID: 7259 RVA: 0x0009501C File Offset: 0x0009321C
	public void RemoveAllSymbolOverrides(int priority = 0)
	{
		this.symbolOverrides.RemoveAll((SymbolOverrideController.SymbolEntry x) => x.priority >= priority);
		this.MarkDirty();
	}

	// Token: 0x06001C5C RID: 7260 RVA: 0x00095054 File Offset: 0x00093254
	public int GetSymbolOverrideIdx(HashedString target_symbol, int priority = 0)
	{
		for (int i = 0; i < this.symbolOverrides.Count; i++)
		{
			SymbolOverrideController.SymbolEntry symbolEntry = this.symbolOverrides[i];
			if (symbolEntry.targetSymbol == target_symbol && symbolEntry.priority == priority)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06001C5D RID: 7261 RVA: 0x0009509E File Offset: 0x0009329E
	public int GetAtlasIdx(Texture2D atlas)
	{
		return this.atlases.GetAtlasIdx(atlas);
	}

	// Token: 0x06001C5E RID: 7262 RVA: 0x000950AC File Offset: 0x000932AC
	public void ApplyOverrides()
	{
		if (this.requiresSorting)
		{
			this.symbolOverrides.Sort((SymbolOverrideController.SymbolEntry x, SymbolOverrideController.SymbolEntry y) => x.priority - y.priority);
			this.requiresSorting = false;
		}
		KAnimBatch batch = this.animController.GetBatch();
		DebugUtil.Assert(batch != null);
		KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(this.animController.batchGroupID);
		int count = batch.atlases.Count;
		this.atlases.Clear(count);
		DictionaryPool<HashedString, Pair<int, int>, SymbolOverrideController>.PooledDictionary pooledDictionary = DictionaryPool<HashedString, Pair<int, int>, SymbolOverrideController>.Allocate();
		ListPool<SymbolOverrideController.SymbolEntry, SymbolOverrideController>.PooledList pooledList = ListPool<SymbolOverrideController.SymbolEntry, SymbolOverrideController>.Allocate();
		for (int i = 0; i < this.symbolOverrides.Count; i++)
		{
			SymbolOverrideController.SymbolEntry symbolEntry = this.symbolOverrides[i];
			Pair<int, int> pair;
			if (pooledDictionary.TryGetValue(symbolEntry.targetSymbol, out pair))
			{
				int first = pair.first;
				if (symbolEntry.priority > first)
				{
					int second = pair.second;
					pooledDictionary[symbolEntry.targetSymbol] = new Pair<int, int>(symbolEntry.priority, second);
					pooledList[second] = symbolEntry;
				}
			}
			else
			{
				pooledDictionary[symbolEntry.targetSymbol] = new Pair<int, int>(symbolEntry.priority, pooledList.Count);
				pooledList.Add(symbolEntry);
			}
		}
		DictionaryPool<KAnim.Build, SymbolOverrideController.BatchGroupInfo, SymbolOverrideController>.PooledDictionary pooledDictionary2 = DictionaryPool<KAnim.Build, SymbolOverrideController.BatchGroupInfo, SymbolOverrideController>.Allocate();
		for (int j = 0; j < pooledList.Count; j++)
		{
			SymbolOverrideController.SymbolEntry symbolEntry2 = pooledList[j];
			SymbolOverrideController.BatchGroupInfo batchGroupInfo;
			if (!pooledDictionary2.TryGetValue(symbolEntry2.sourceSymbol.build, out batchGroupInfo))
			{
				batchGroupInfo = new SymbolOverrideController.BatchGroupInfo
				{
					build = symbolEntry2.sourceSymbol.build,
					data = KAnimBatchManager.Instance().GetBatchGroupData(symbolEntry2.sourceSymbol.build.batchTag)
				};
				Texture2D texture = symbolEntry2.sourceSymbol.build.GetTexture(0);
				int num = batch.atlases.GetAtlasIdx(texture);
				if (num < 0)
				{
					num = this.atlases.Add(texture);
				}
				batchGroupInfo.atlasIdx = num;
				pooledDictionary2[batchGroupInfo.build] = batchGroupInfo;
			}
			KAnim.Build.Symbol symbol = batchGroupData.GetSymbol(symbolEntry2.targetSymbol);
			if (symbol != null)
			{
				this.animController.SetSymbolOverrides(symbol.firstFrameIdx, symbol.numFrames, batchGroupInfo.atlasIdx, batchGroupInfo.data, symbolEntry2.sourceSymbol.firstFrameIdx, symbolEntry2.sourceSymbol.numFrames);
			}
		}
		pooledDictionary2.Recycle();
		pooledList.Recycle();
		pooledDictionary.Recycle();
		if (this.faceGraph != null)
		{
			this.faceGraph.ApplyShape();
		}
	}

	// Token: 0x06001C5F RID: 7263 RVA: 0x00095348 File Offset: 0x00093548
	public void ApplyAtlases()
	{
		KAnimBatch batch = this.animController.GetBatch();
		this.atlases.Apply(batch.matProperties);
	}

	// Token: 0x06001C60 RID: 7264 RVA: 0x00095372 File Offset: 0x00093572
	public KAnimBatch.AtlasList GetAtlasList()
	{
		return this.atlases;
	}

	// Token: 0x06001C61 RID: 7265 RVA: 0x0009537C File Offset: 0x0009357C
	public void MarkDirty()
	{
		if (this.animController != null)
		{
			this.animController.SetDirty();
		}
		int version = this.version + 1;
		this.version = version;
		this.requiresSorting = true;
	}

	// Token: 0x04000FF6 RID: 4086
	public bool applySymbolOverridesEveryFrame;

	// Token: 0x04000FF7 RID: 4087
	[SerializeField]
	private List<SymbolOverrideController.SymbolEntry> symbolOverrides = new List<SymbolOverrideController.SymbolEntry>();

	// Token: 0x04000FF8 RID: 4088
	private KAnimBatch.AtlasList atlases;

	// Token: 0x04000FF9 RID: 4089
	private KBatchedAnimController animController;

	// Token: 0x04000FFA RID: 4090
	private FaceGraph faceGraph;

	// Token: 0x04000FFC RID: 4092
	private bool requiresSorting;

	// Token: 0x020012C2 RID: 4802
	[Serializable]
	public struct SymbolEntry
	{
		// Token: 0x0400643F RID: 25663
		public HashedString targetSymbol;

		// Token: 0x04006440 RID: 25664
		[NonSerialized]
		public KAnim.Build.Symbol sourceSymbol;

		// Token: 0x04006441 RID: 25665
		public HashedString sourceSymbolId;

		// Token: 0x04006442 RID: 25666
		public HashedString sourceSymbolBatchTag;

		// Token: 0x04006443 RID: 25667
		public int priority;
	}

	// Token: 0x020012C3 RID: 4803
	private struct SymbolToOverride
	{
		// Token: 0x04006444 RID: 25668
		public KAnim.Build.Symbol sourceSymbol;

		// Token: 0x04006445 RID: 25669
		public HashedString targetSymbol;

		// Token: 0x04006446 RID: 25670
		public KBatchGroupData data;

		// Token: 0x04006447 RID: 25671
		public int atlasIdx;
	}

	// Token: 0x020012C4 RID: 4804
	private class BatchGroupInfo
	{
		// Token: 0x04006448 RID: 25672
		public KAnim.Build build;

		// Token: 0x04006449 RID: 25673
		public int atlasIdx;

		// Token: 0x0400644A RID: 25674
		public KBatchGroupData data;
	}
}
