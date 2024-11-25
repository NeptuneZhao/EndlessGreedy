using System;
using UnityEngine;

// Token: 0x02000DD2 RID: 3538
public class SwapUIAnimationController : MonoBehaviour
{
	// Token: 0x06007076 RID: 28790 RVA: 0x002A8E48 File Offset: 0x002A7048
	public void SetState(bool Primary)
	{
		this.AnimationControllerObject_Primary.SetActive(Primary);
		if (!Primary)
		{
			this.AnimationControllerObject_Alternate.GetComponent<KAnimControllerBase>().TintColour = new Color(1f, 1f, 1f, 0.5f);
			this.AnimationControllerObject_Primary.GetComponent<KAnimControllerBase>().TintColour = Color.clear;
		}
		this.AnimationControllerObject_Alternate.SetActive(!Primary);
		if (Primary)
		{
			this.AnimationControllerObject_Primary.GetComponent<KAnimControllerBase>().TintColour = Color.white;
			this.AnimationControllerObject_Alternate.GetComponent<KAnimControllerBase>().TintColour = Color.clear;
		}
	}

	// Token: 0x04004D4F RID: 19791
	public GameObject AnimationControllerObject_Primary;

	// Token: 0x04004D50 RID: 19792
	public GameObject AnimationControllerObject_Alternate;
}
