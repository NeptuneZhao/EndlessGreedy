using System;
using UnityEngine;

// Token: 0x020005EC RID: 1516
public class TubeTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x060024B7 RID: 9399 RVA: 0x000CC851 File Offset: 0x000CAA51
	public TubeTransitionLayer(Navigator navigator) : base(navigator)
	{
		this.tube_traveller = navigator.GetSMI<TubeTraveller.Instance>();
		if (this.tube_traveller != null && navigator.CurrentNavType == NavType.Tube && !this.tube_traveller.inTube)
		{
			this.tube_traveller.OnTubeTransition(true);
		}
	}

	// Token: 0x060024B8 RID: 9400 RVA: 0x000CC890 File Offset: 0x000CAA90
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		this.tube_traveller.OnPathAdvanced(null);
		if (transition.start != NavType.Tube && transition.end == NavType.Tube)
		{
			int cell = Grid.PosToCell(navigator);
			this.entrance = this.GetEntrance(cell);
			return;
		}
		this.entrance = null;
	}

	// Token: 0x060024B9 RID: 9401 RVA: 0x000CC8E0 File Offset: 0x000CAAE0
	public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.EndTransition(navigator, transition);
		if (transition.start != NavType.Tube && transition.end == NavType.Tube && this.entrance)
		{
			this.entrance.ConsumeCharge(navigator.gameObject);
			this.entrance = null;
		}
		this.tube_traveller.OnTubeTransition(transition.end == NavType.Tube);
	}

	// Token: 0x060024BA RID: 9402 RVA: 0x000CC940 File Offset: 0x000CAB40
	private TravelTubeEntrance GetEntrance(int cell)
	{
		if (!Grid.HasUsableTubeEntrance(cell, this.tube_traveller.prefabInstanceID))
		{
			return null;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			TravelTubeEntrance component = gameObject.GetComponent<TravelTubeEntrance>();
			if (component != null && component.isSpawned)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x040014CC RID: 5324
	private TubeTraveller.Instance tube_traveller;

	// Token: 0x040014CD RID: 5325
	private TravelTubeEntrance entrance;
}
