using System;
using UnityEngine;

// Token: 0x0200057E RID: 1406
[AddComponentMenu("KMonoBehaviour/scripts/LiquidSourceManager")]
public class LiquidSourceManager : KMonoBehaviour, IChunkManager
{
	// Token: 0x060020A9 RID: 8361 RVA: 0x000B6777 File Offset: 0x000B4977
	protected override void OnPrefabInit()
	{
		LiquidSourceManager.Instance = this;
	}

	// Token: 0x060020AA RID: 8362 RVA: 0x000B677F File Offset: 0x000B497F
	public SubstanceChunk CreateChunk(SimHashes element_id, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		return this.CreateChunk(ElementLoader.FindElementByHash(element_id), mass, temperature, diseaseIdx, diseaseCount, position);
	}

	// Token: 0x060020AB RID: 8363 RVA: 0x000B6795 File Offset: 0x000B4995
	public SubstanceChunk CreateChunk(Element element, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		return GeneratedOre.CreateChunk(element, mass, temperature, diseaseIdx, diseaseCount, position);
	}

	// Token: 0x04001259 RID: 4697
	public static LiquidSourceManager Instance;
}
