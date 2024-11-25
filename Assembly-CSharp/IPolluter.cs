using System;
using UnityEngine;

// Token: 0x020009C4 RID: 2500
public interface IPolluter
{
	// Token: 0x06004887 RID: 18567
	int GetRadius();

	// Token: 0x06004888 RID: 18568
	int GetNoise();

	// Token: 0x06004889 RID: 18569
	GameObject GetGameObject();

	// Token: 0x0600488A RID: 18570
	void SetAttributes(Vector2 pos, int dB, GameObject go, string name = null);

	// Token: 0x0600488B RID: 18571
	string GetName();

	// Token: 0x0600488C RID: 18572
	Vector2 GetPosition();

	// Token: 0x0600488D RID: 18573
	void Clear();

	// Token: 0x0600488E RID: 18574
	void SetSplat(NoiseSplat splat);
}
