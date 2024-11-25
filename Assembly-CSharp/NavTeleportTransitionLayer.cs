using System;

// Token: 0x020005EF RID: 1519
public class NavTeleportTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x060024C0 RID: 9408 RVA: 0x000CCB48 File Offset: 0x000CAD48
	public NavTeleportTransitionLayer(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x060024C1 RID: 9409 RVA: 0x000CCB54 File Offset: 0x000CAD54
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		if (transition.start == NavType.Teleport)
		{
			int num = Grid.PosToCell(navigator);
			int num2;
			int num3;
			Grid.CellToXY(num, out num2, out num3);
			int num4 = navigator.NavGrid.teleportTransitions[num];
			int num5;
			int num6;
			Grid.CellToXY(navigator.NavGrid.teleportTransitions[num], out num5, out num6);
			transition.x = num5 - num2;
			transition.y = num6 - num3;
		}
	}
}
