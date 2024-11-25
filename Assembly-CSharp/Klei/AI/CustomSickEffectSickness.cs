using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F59 RID: 3929
	public class CustomSickEffectSickness : Sickness.SicknessComponent
	{
		// Token: 0x060078B3 RID: 30899 RVA: 0x002FC60E File Offset: 0x002FA80E
		public CustomSickEffectSickness(string effect_kanim, string effect_anim_name)
		{
			this.kanim = effect_kanim;
			this.animName = effect_anim_name;
		}

		// Token: 0x060078B4 RID: 30900 RVA: 0x002FC624 File Offset: 0x002FA824
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect(this.kanim, go.transform.GetPosition() + new Vector3(0f, 0f, -0.1f), go.transform, true, Grid.SceneLayer.Front, false);
			kbatchedAnimController.Play(this.animName, KAnim.PlayMode.Loop, 1f, 0f);
			return kbatchedAnimController;
		}

		// Token: 0x060078B5 RID: 30901 RVA: 0x002FC686 File Offset: 0x002FA886
		public override void OnCure(GameObject go, object instance_data)
		{
			((KAnimControllerBase)instance_data).gameObject.DeleteObject();
		}

		// Token: 0x04005A2B RID: 23083
		private string kanim;

		// Token: 0x04005A2C RID: 23084
		private string animName;
	}
}
