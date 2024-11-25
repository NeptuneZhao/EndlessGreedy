using System;
using UnityEngine;

// Token: 0x0200056B RID: 1387
[AddComponentMenu("KMonoBehaviour/scripts/GasSourceManager")]
public class GasSourceManager : KMonoBehaviour, IChunkManager
{
	// Token: 0x06002021 RID: 8225 RVA: 0x000B4B88 File Offset: 0x000B2D88
	protected override void OnPrefabInit()
	{
		GasSourceManager.Instance = this;
	}

	// Token: 0x06002022 RID: 8226 RVA: 0x000B4B90 File Offset: 0x000B2D90
	public SubstanceChunk CreateChunk(SimHashes element_id, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		return this.CreateChunk(ElementLoader.FindElementByHash(element_id), mass, temperature, diseaseIdx, diseaseCount, position);
	}

	// Token: 0x06002023 RID: 8227 RVA: 0x000B4BA6 File Offset: 0x000B2DA6
	public SubstanceChunk CreateChunk(Element element, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		return GeneratedOre.CreateChunk(element, mass, temperature, diseaseIdx, diseaseCount, position);
	}

	// Token: 0x0400122B RID: 4651
	public static GasSourceManager Instance;
}
