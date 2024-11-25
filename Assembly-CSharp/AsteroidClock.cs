using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BE2 RID: 3042
public class AsteroidClock : MonoBehaviour
{
	// Token: 0x06005C8A RID: 23690 RVA: 0x0021DA94 File Offset: 0x0021BC94
	private void Awake()
	{
		this.UpdateOverlay();
	}

	// Token: 0x06005C8B RID: 23691 RVA: 0x0021DA9C File Offset: 0x0021BC9C
	private void Start()
	{
	}

	// Token: 0x06005C8C RID: 23692 RVA: 0x0021DA9E File Offset: 0x0021BC9E
	private void Update()
	{
		if (GameClock.Instance != null)
		{
			this.rotationTransform.rotation = Quaternion.Euler(0f, 0f, 360f * -GameClock.Instance.GetCurrentCycleAsPercentage());
		}
	}

	// Token: 0x06005C8D RID: 23693 RVA: 0x0021DAD8 File Offset: 0x0021BCD8
	private void UpdateOverlay()
	{
		float fillAmount = 0.125f;
		this.NightOverlay.fillAmount = fillAmount;
	}

	// Token: 0x04003DC3 RID: 15811
	public Transform rotationTransform;

	// Token: 0x04003DC4 RID: 15812
	public Image NightOverlay;
}
