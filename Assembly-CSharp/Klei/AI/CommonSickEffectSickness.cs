﻿using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F58 RID: 3928
	public class CommonSickEffectSickness : Sickness.SicknessComponent
	{
		// Token: 0x060078B0 RID: 30896 RVA: 0x002FC594 File Offset: 0x002FA794
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("contaminated_crew_fx_kanim", go.transform.GetPosition() + new Vector3(0f, 0f, -0.1f), go.transform, true, Grid.SceneLayer.Front, false);
			kbatchedAnimController.Play("fx_loop", KAnim.PlayMode.Loop, 1f, 0f);
			return kbatchedAnimController;
		}

		// Token: 0x060078B1 RID: 30897 RVA: 0x002FC5F4 File Offset: 0x002FA7F4
		public override void OnCure(GameObject go, object instance_data)
		{
			((KAnimControllerBase)instance_data).gameObject.DeleteObject();
		}
	}
}
