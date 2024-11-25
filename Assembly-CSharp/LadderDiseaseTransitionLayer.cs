using System;
using Klei;
using UnityEngine;

// Token: 0x020005ED RID: 1517
public class LadderDiseaseTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x060024BB RID: 9403 RVA: 0x000CC993 File Offset: 0x000CAB93
	public LadderDiseaseTransitionLayer(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x060024BC RID: 9404 RVA: 0x000CC99C File Offset: 0x000CAB9C
	public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.EndTransition(navigator, transition);
		if (transition.end == NavType.Ladder)
		{
			int cell = Grid.PosToCell(navigator);
			GameObject gameObject = Grid.Objects[cell, 1];
			if (gameObject != null)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component != null)
				{
					PrimaryElement component2 = navigator.GetComponent<PrimaryElement>();
					if (component2 != null)
					{
						SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
						invalid.idx = component2.DiseaseIdx;
						invalid.count = (int)((float)component2.DiseaseCount * 0.005f);
						SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid;
						invalid2.idx = component.DiseaseIdx;
						invalid2.count = (int)((float)component.DiseaseCount * 0.005f);
						component2.ModifyDiseaseCount(-invalid.count, "Navigator.EndTransition");
						component.ModifyDiseaseCount(-invalid2.count, "Navigator.EndTransition");
						if (invalid.count > 0)
						{
							component.AddDisease(invalid.idx, invalid.count, "TransitionDriver.EndTransition");
						}
						if (invalid2.count > 0)
						{
							component2.AddDisease(invalid2.idx, invalid2.count, "TransitionDriver.EndTransition");
						}
					}
				}
			}
		}
	}
}
