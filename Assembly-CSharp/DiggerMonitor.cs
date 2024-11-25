using System;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x020007FF RID: 2047
public class DiggerMonitor : GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>
{
	// Token: 0x06003898 RID: 14488 RVA: 0x00134D60 File Offset: 0x00132F60
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		this.loop.EventTransition(GameHashes.BeginMeteorBombardment, (DiggerMonitor.Instance smi) => Game.Instance, this.dig, (DiggerMonitor.Instance smi) => smi.CanTunnel());
		this.dig.ToggleBehaviour(GameTags.Creatures.Tunnel, (DiggerMonitor.Instance smi) => true, null).GoTo(this.loop);
	}

	// Token: 0x040021FA RID: 8698
	public GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.State loop;

	// Token: 0x040021FB RID: 8699
	public GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.State dig;

	// Token: 0x020016E7 RID: 5863
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x060093E0 RID: 37856 RVA: 0x0035A3F5 File Offset: 0x003585F5
		// (set) Token: 0x060093E1 RID: 37857 RVA: 0x0035A3FD File Offset: 0x003585FD
		public int depthToDig { get; set; }
	}

	// Token: 0x020016E8 RID: 5864
	public new class Instance : GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.GameInstance
	{
		// Token: 0x060093E3 RID: 37859 RVA: 0x0035A410 File Offset: 0x00358610
		public Instance(IStateMachineTarget master, DiggerMonitor.Def def) : base(master, def)
		{
			global::World instance = global::World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
			this.OnDestinationReachedDelegate = new Action<object>(this.OnDestinationReached);
			master.Subscribe(387220196, this.OnDestinationReachedDelegate);
			master.Subscribe(-766531887, this.OnDestinationReachedDelegate);
		}

		// Token: 0x060093E4 RID: 37860 RVA: 0x0035A488 File Offset: 0x00358688
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			global::World instance = global::World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
			base.master.Unsubscribe(387220196, this.OnDestinationReachedDelegate);
			base.master.Unsubscribe(-766531887, this.OnDestinationReachedDelegate);
		}

		// Token: 0x060093E5 RID: 37861 RVA: 0x0035A4ED File Offset: 0x003586ED
		private void OnDestinationReached(object data)
		{
			this.CheckInSolid();
		}

		// Token: 0x060093E6 RID: 37862 RVA: 0x0035A4F8 File Offset: 0x003586F8
		private void CheckInSolid()
		{
			Navigator component = base.gameObject.GetComponent<Navigator>();
			if (component == null)
			{
				return;
			}
			int cell = Grid.PosToCell(base.gameObject);
			if (component.CurrentNavType != NavType.Solid && Grid.IsSolidCell(cell))
			{
				component.SetCurrentNavType(NavType.Solid);
				return;
			}
			if (component.CurrentNavType == NavType.Solid && !Grid.IsSolidCell(cell))
			{
				component.SetCurrentNavType(NavType.Floor);
				base.gameObject.AddTag(GameTags.Creatures.Falling);
			}
		}

		// Token: 0x060093E7 RID: 37863 RVA: 0x0035A56B File Offset: 0x0035876B
		private void OnSolidChanged(int cell)
		{
			this.CheckInSolid();
		}

		// Token: 0x060093E8 RID: 37864 RVA: 0x0035A574 File Offset: 0x00358774
		public bool CanTunnel()
		{
			int num = Grid.PosToCell(this);
			if (global::World.Instance.zoneRenderData.GetSubWorldZoneType(num) == SubWorld.ZoneType.Space)
			{
				int num2 = num;
				while (Grid.IsValidCell(num2) && !Grid.Solid[num2])
				{
					num2 = Grid.CellAbove(num2);
				}
				if (!Grid.IsValidCell(num2))
				{
					return this.FoundValidDigCell();
				}
			}
			return false;
		}

		// Token: 0x060093E9 RID: 37865 RVA: 0x0035A5CC File Offset: 0x003587CC
		private bool FoundValidDigCell()
		{
			int num = base.smi.def.depthToDig;
			int num2 = Grid.PosToCell(base.smi.master.gameObject);
			this.lastDigCell = num2;
			int cell = Grid.CellBelow(num2);
			while (this.IsValidDigCell(cell, null) && num > 0)
			{
				cell = Grid.CellBelow(cell);
				num--;
			}
			if (num > 0)
			{
				cell = GameUtil.FloodFillFind<object>(new Func<int, object, bool>(this.IsValidDigCell), null, num2, base.smi.def.depthToDig, false, true);
			}
			this.lastDigCell = cell;
			return this.lastDigCell != -1;
		}

		// Token: 0x060093EA RID: 37866 RVA: 0x0035A668 File Offset: 0x00358868
		private bool IsValidDigCell(int cell, object arg = null)
		{
			if (Grid.IsValidCell(cell) && Grid.Solid[cell])
			{
				if (!Grid.HasDoor[cell] && !Grid.Foundation[cell])
				{
					ushort index = Grid.ElementIdx[cell];
					Element element = ElementLoader.elements[(int)index];
					return Grid.Element[cell].hardness < 150 && !element.HasTag(GameTags.RefinedMetal);
				}
				GameObject gameObject = Grid.Objects[cell, 1];
				if (gameObject != null)
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					return Grid.Element[cell].hardness < 150 && !component.Element.HasTag(GameTags.RefinedMetal);
				}
			}
			return false;
		}

		// Token: 0x0400711F RID: 28959
		[Serialize]
		public int lastDigCell = -1;

		// Token: 0x04007120 RID: 28960
		private Action<object> OnDestinationReachedDelegate;
	}
}
