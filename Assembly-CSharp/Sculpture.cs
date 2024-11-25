using System;
using UnityEngine;

// Token: 0x020005B7 RID: 1463
public class Sculpture : Artable
{
	// Token: 0x060022F2 RID: 8946 RVA: 0x000C313C File Offset: 0x000C133C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (Sculpture.sculptureOverrides == null)
		{
			Sculpture.sculptureOverrides = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_sculpture_kanim")
			};
		}
		this.overrideAnims = Sculpture.sculptureOverrides;
		this.synchronizeAnims = false;
	}

	// Token: 0x060022F3 RID: 8947 RVA: 0x000C317C File Offset: 0x000C137C
	public override void SetStage(string stage_id, bool skip_effect)
	{
		base.SetStage(stage_id, skip_effect);
		bool flag = base.CurrentStage == "Default";
		if (Db.GetArtableStages().Get(stage_id) == null)
		{
			global::Debug.LogError("Missing stage: " + stage_id);
		}
		if (!skip_effect && !flag)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("sculpture_fx_kanim", base.transform.GetPosition(), base.transform, false, Grid.SceneLayer.Front, false);
			kbatchedAnimController.destroyOnAnimComplete = true;
			kbatchedAnimController.transform.SetLocalPosition(Vector3.zero);
			kbatchedAnimController.Play("poof", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x040013D8 RID: 5080
	private static KAnimFile[] sculptureOverrides;
}
