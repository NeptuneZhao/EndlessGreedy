using System;

// Token: 0x020005FF RID: 1535
public class OrbitalData : Resource
{
	// Token: 0x060025B3 RID: 9651 RVA: 0x000D2044 File Offset: 0x000D0244
	public OrbitalData(string id, ResourceSet parent, string animFile = "earth_kanim", string initialAnim = "", OrbitalData.OrbitalType orbitalType = OrbitalData.OrbitalType.poi, float periodInCycles = 1f, float xGridPercent = 0.5f, float yGridPercent = 0.5f, float minAngle = -350f, float maxAngle = 350f, float radiusScale = 1.05f, bool rotatesBehind = true, float behindZ = 0.05f, float distance = 25f, float renderZ = 1f) : base(id, parent, null)
	{
		this.animFile = animFile;
		this.initialAnim = initialAnim;
		this.orbitalType = orbitalType;
		this.periodInCycles = periodInCycles;
		this.xGridPercent = xGridPercent;
		this.yGridPercent = yGridPercent;
		this.minAngle = minAngle;
		this.maxAngle = maxAngle;
		this.radiusScale = radiusScale;
		this.rotatesBehind = rotatesBehind;
		this.behindZ = behindZ;
		this.distance = distance;
		this.renderZ = renderZ;
	}

	// Token: 0x0400157F RID: 5503
	public string animFile;

	// Token: 0x04001580 RID: 5504
	public string initialAnim;

	// Token: 0x04001581 RID: 5505
	public float periodInCycles;

	// Token: 0x04001582 RID: 5506
	public float xGridPercent;

	// Token: 0x04001583 RID: 5507
	public float yGridPercent;

	// Token: 0x04001584 RID: 5508
	public float minAngle;

	// Token: 0x04001585 RID: 5509
	public float maxAngle;

	// Token: 0x04001586 RID: 5510
	public float radiusScale;

	// Token: 0x04001587 RID: 5511
	public bool rotatesBehind;

	// Token: 0x04001588 RID: 5512
	public float behindZ;

	// Token: 0x04001589 RID: 5513
	public float distance;

	// Token: 0x0400158A RID: 5514
	public float renderZ;

	// Token: 0x0400158B RID: 5515
	public OrbitalData.OrbitalType orbitalType;

	// Token: 0x0400158C RID: 5516
	public Func<float> GetRenderZ;

	// Token: 0x020013F0 RID: 5104
	public enum OrbitalType
	{
		// Token: 0x0400686E RID: 26734
		world,
		// Token: 0x0400686F RID: 26735
		poi,
		// Token: 0x04006870 RID: 26736
		inOrbit,
		// Token: 0x04006871 RID: 26737
		landed
	}
}
