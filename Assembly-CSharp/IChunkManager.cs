using System;
using UnityEngine;

// Token: 0x020005CB RID: 1483
public interface IChunkManager
{
	// Token: 0x0600242A RID: 9258
	SubstanceChunk CreateChunk(Element element, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position);

	// Token: 0x0600242B RID: 9259
	SubstanceChunk CreateChunk(SimHashes element_id, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position);
}
