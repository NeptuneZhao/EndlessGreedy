using System;

// Token: 0x020005EA RID: 1514
public class FullPuftTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x060024AE RID: 9390 RVA: 0x000CC52A File Offset: 0x000CA72A
	public FullPuftTransitionLayer(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x060024AF RID: 9391 RVA: 0x000CC534 File Offset: 0x000CA734
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		CreatureCalorieMonitor.Instance smi = navigator.GetSMI<CreatureCalorieMonitor.Instance>();
		if (smi != null && smi.stomach.IsReadyToPoop())
		{
			string s = HashCache.Get().Get(transition.anim.HashValue) + "_full";
			if (navigator.animController.HasAnimation(s))
			{
				transition.anim = s;
			}
		}
	}
}
