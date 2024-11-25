using System;
using UnityEngine;

// Token: 0x020005E9 RID: 1513
public class SplashTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x060024A9 RID: 9385 RVA: 0x000CC438 File Offset: 0x000CA638
	public SplashTransitionLayer(Navigator navigator) : base(navigator)
	{
		this.lastSplashTime = Time.time;
	}

	// Token: 0x060024AA RID: 9386 RVA: 0x000CC44C File Offset: 0x000CA64C
	private void RefreshSplashes(Navigator navigator, Navigator.ActiveTransition transition)
	{
		if (navigator == null)
		{
			return;
		}
		if (transition.end == NavType.Tube)
		{
			return;
		}
		Vector3 position = navigator.transform.GetPosition();
		if (this.lastSplashTime + 1f < Time.time && Grid.Element[Grid.PosToCell(position)].IsLiquid)
		{
			this.lastSplashTime = Time.time;
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("splash_step_kanim", position + new Vector3(0f, 0.75f, -0.1f), null, false, Grid.SceneLayer.Front, false);
			kbatchedAnimController.Play("fx1", KAnim.PlayMode.Once, 1f, 0f);
			kbatchedAnimController.destroyOnAnimComplete = true;
		}
	}

	// Token: 0x060024AB RID: 9387 RVA: 0x000CC4F4 File Offset: 0x000CA6F4
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		this.RefreshSplashes(navigator, transition);
	}

	// Token: 0x060024AC RID: 9388 RVA: 0x000CC506 File Offset: 0x000CA706
	public override void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.UpdateTransition(navigator, transition);
		this.RefreshSplashes(navigator, transition);
	}

	// Token: 0x060024AD RID: 9389 RVA: 0x000CC518 File Offset: 0x000CA718
	public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.EndTransition(navigator, transition);
		this.RefreshSplashes(navigator, transition);
	}

	// Token: 0x040014C9 RID: 5321
	private float lastSplashTime;

	// Token: 0x040014CA RID: 5322
	private const float SPLASH_INTERVAL = 1f;
}
