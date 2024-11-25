using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005EB RID: 1515
public class DoorTransitionLayer : TransitionDriver.InterruptOverrideLayer
{
	// Token: 0x060024B0 RID: 9392 RVA: 0x000CC59F File Offset: 0x000CA79F
	public DoorTransitionLayer(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x060024B1 RID: 9393 RVA: 0x000CC5B4 File Offset: 0x000CA7B4
	private bool AreAllDoorsOpen()
	{
		foreach (INavDoor navDoor in this.doors)
		{
			if (navDoor != null && !navDoor.IsOpen())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060024B2 RID: 9394 RVA: 0x000CC614 File Offset: 0x000CA814
	protected override bool IsOverrideComplete()
	{
		return base.IsOverrideComplete() && this.AreAllDoorsOpen();
	}

	// Token: 0x060024B3 RID: 9395 RVA: 0x000CC628 File Offset: 0x000CA828
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		if (this.doors.Count > 0)
		{
			return;
		}
		int cell = Grid.PosToCell(navigator);
		int cell2 = Grid.OffsetCell(cell, transition.x, transition.y);
		this.AddDoor(cell2);
		if (navigator.CurrentNavType != NavType.Tube)
		{
			this.AddDoor(Grid.CellAbove(cell2));
		}
		for (int i = 0; i < transition.navGridTransition.voidOffsets.Length; i++)
		{
			int cell3 = Grid.OffsetCell(cell, transition.navGridTransition.voidOffsets[i]);
			this.AddDoor(cell3);
		}
		if (this.doors.Count == 0)
		{
			return;
		}
		if (!this.AreAllDoorsOpen())
		{
			base.BeginTransition(navigator, transition);
			transition.anim = navigator.NavGrid.GetIdleAnim(navigator.CurrentNavType);
			transition.start = this.originalTransition.start;
			transition.end = this.originalTransition.start;
		}
		foreach (INavDoor navDoor in this.doors)
		{
			navDoor.Open();
		}
	}

	// Token: 0x060024B4 RID: 9396 RVA: 0x000CC74C File Offset: 0x000CA94C
	public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.EndTransition(navigator, transition);
		if (this.doors.Count == 0)
		{
			return;
		}
		foreach (INavDoor navDoor in this.doors)
		{
			if (!navDoor.IsNullOrDestroyed())
			{
				navDoor.Close();
			}
		}
		this.doors.Clear();
	}

	// Token: 0x060024B5 RID: 9397 RVA: 0x000CC7C8 File Offset: 0x000CA9C8
	private void AddDoor(int cell)
	{
		INavDoor door = this.GetDoor(cell);
		if (!door.IsNullOrDestroyed() && !this.doors.Contains(door))
		{
			this.doors.Add(door);
		}
	}

	// Token: 0x060024B6 RID: 9398 RVA: 0x000CC800 File Offset: 0x000CAA00
	private INavDoor GetDoor(int cell)
	{
		if (!Grid.HasDoor[cell])
		{
			return null;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			INavDoor navDoor = gameObject.GetComponent<INavDoor>();
			if (navDoor == null)
			{
				navDoor = gameObject.GetSMI<INavDoor>();
			}
			if (navDoor != null && navDoor.isSpawned)
			{
				return navDoor;
			}
		}
		return null;
	}

	// Token: 0x040014CB RID: 5323
	private List<INavDoor> doors = new List<INavDoor>();
}
