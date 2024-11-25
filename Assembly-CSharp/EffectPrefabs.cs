using System;
using UnityEngine;

// Token: 0x0200086A RID: 2154
public class EffectPrefabs : MonoBehaviour
{
	// Token: 0x17000442 RID: 1090
	// (get) Token: 0x06003C19 RID: 15385 RVA: 0x0014DD9D File Offset: 0x0014BF9D
	// (set) Token: 0x06003C1A RID: 15386 RVA: 0x0014DDA4 File Offset: 0x0014BFA4
	public static EffectPrefabs Instance { get; private set; }

	// Token: 0x06003C1B RID: 15387 RVA: 0x0014DDAC File Offset: 0x0014BFAC
	private void Awake()
	{
		EffectPrefabs.Instance = this;
	}

	// Token: 0x04002467 RID: 9319
	public GameObject DreamBubble;

	// Token: 0x04002468 RID: 9320
	public GameObject ThoughtBubble;

	// Token: 0x04002469 RID: 9321
	public GameObject ThoughtBubbleConvo;

	// Token: 0x0400246A RID: 9322
	public GameObject MeteorBackground;

	// Token: 0x0400246B RID: 9323
	public GameObject SparkleStreakFX;

	// Token: 0x0400246C RID: 9324
	public GameObject HappySingerFX;

	// Token: 0x0400246D RID: 9325
	public GameObject HugFrenzyFX;

	// Token: 0x0400246E RID: 9326
	public GameObject GameplayEventDisplay;

	// Token: 0x0400246F RID: 9327
	public GameObject OpenTemporalTearBeam;

	// Token: 0x04002470 RID: 9328
	public GameObject MissileSmokeTrailFX;
}
