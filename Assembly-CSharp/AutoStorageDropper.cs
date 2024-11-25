using System;
using UnityEngine;

// Token: 0x0200052D RID: 1325
public class AutoStorageDropper : GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>
{
	// Token: 0x06001DD4 RID: 7636 RVA: 0x000A5644 File Offset: 0x000A3844
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.root.Update(delegate(AutoStorageDropper.Instance smi, float dt)
		{
			smi.UpdateBlockedStatus();
		}, UpdateRate.SIM_200ms, true);
		this.idle.EventTransition(GameHashes.OnStorageChange, this.pre_drop, null).OnSignal(this.checkCanDrop, this.pre_drop, (AutoStorageDropper.Instance smi) => !smi.GetComponent<Storage>().IsEmpty()).ParamTransition<bool>(this.isBlocked, this.blocked, GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.IsTrue);
		this.pre_drop.ScheduleGoTo((AutoStorageDropper.Instance smi) => smi.def.delay, this.dropping);
		this.dropping.Enter(delegate(AutoStorageDropper.Instance smi)
		{
			smi.Drop();
		}).GoTo(this.idle);
		this.blocked.ParamTransition<bool>(this.isBlocked, this.pre_drop, GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.OutputTileBlocked, null);
	}

	// Token: 0x040010BE RID: 4286
	private GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.State idle;

	// Token: 0x040010BF RID: 4287
	private GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.State pre_drop;

	// Token: 0x040010C0 RID: 4288
	private GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.State dropping;

	// Token: 0x040010C1 RID: 4289
	private GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.State blocked;

	// Token: 0x040010C2 RID: 4290
	private StateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.BoolParameter isBlocked;

	// Token: 0x040010C3 RID: 4291
	public StateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.Signal checkCanDrop;

	// Token: 0x020012E5 RID: 4837
	public class DropperFxConfig
	{
		// Token: 0x040064E4 RID: 25828
		public string animFile;

		// Token: 0x040064E5 RID: 25829
		public string animName;

		// Token: 0x040064E6 RID: 25830
		public Grid.SceneLayer layer = Grid.SceneLayer.FXFront;

		// Token: 0x040064E7 RID: 25831
		public bool useElementTint = true;

		// Token: 0x040064E8 RID: 25832
		public bool flipX;

		// Token: 0x040064E9 RID: 25833
		public bool flipY;
	}

	// Token: 0x020012E6 RID: 4838
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040064EA RID: 25834
		public CellOffset dropOffset;

		// Token: 0x040064EB RID: 25835
		public bool asOre;

		// Token: 0x040064EC RID: 25836
		public SimHashes[] elementFilter;

		// Token: 0x040064ED RID: 25837
		public bool invertElementFilterInitialValue;

		// Token: 0x040064EE RID: 25838
		public bool blockedBySubstantialLiquid;

		// Token: 0x040064EF RID: 25839
		public AutoStorageDropper.DropperFxConfig neutralFx;

		// Token: 0x040064F0 RID: 25840
		public AutoStorageDropper.DropperFxConfig leftFx;

		// Token: 0x040064F1 RID: 25841
		public AutoStorageDropper.DropperFxConfig rightFx;

		// Token: 0x040064F2 RID: 25842
		public AutoStorageDropper.DropperFxConfig upFx;

		// Token: 0x040064F3 RID: 25843
		public AutoStorageDropper.DropperFxConfig downFx;

		// Token: 0x040064F4 RID: 25844
		public Vector3 fxOffset = Vector3.zero;

		// Token: 0x040064F5 RID: 25845
		public float cooldown = 2f;

		// Token: 0x040064F6 RID: 25846
		public float delay;
	}

	// Token: 0x020012E7 RID: 4839
	public new class Instance : GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.GameInstance
	{
		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06008513 RID: 34067 RVA: 0x003253E2 File Offset: 0x003235E2
		// (set) Token: 0x06008514 RID: 34068 RVA: 0x003253EA File Offset: 0x003235EA
		public bool isInvertElementFilter { get; private set; }

		// Token: 0x06008515 RID: 34069 RVA: 0x003253F3 File Offset: 0x003235F3
		public Instance(IStateMachineTarget master, AutoStorageDropper.Def def) : base(master, def)
		{
			this.isInvertElementFilter = def.invertElementFilterInitialValue;
		}

		// Token: 0x06008516 RID: 34070 RVA: 0x00325409 File Offset: 0x00323609
		public void SetInvertElementFilter(bool value)
		{
			base.smi.isInvertElementFilter = value;
			base.smi.sm.checkCanDrop.Trigger(base.smi);
		}

		// Token: 0x06008517 RID: 34071 RVA: 0x00325434 File Offset: 0x00323634
		public void UpdateBlockedStatus()
		{
			int cell = Grid.PosToCell(base.smi.GetDropPosition());
			bool value = Grid.IsSolidCell(cell) || (base.def.blockedBySubstantialLiquid && Grid.IsSubstantialLiquid(cell, 0.35f));
			base.sm.isBlocked.Set(value, base.smi, false);
		}

		// Token: 0x06008518 RID: 34072 RVA: 0x00325494 File Offset: 0x00323694
		private bool IsFilteredElement(SimHashes element)
		{
			for (int num = 0; num != base.def.elementFilter.Length; num++)
			{
				if (base.def.elementFilter[num] == element)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008519 RID: 34073 RVA: 0x003254CC File Offset: 0x003236CC
		private bool AllowedToDrop(SimHashes element)
		{
			return base.def.elementFilter == null || base.def.elementFilter.Length == 0 || (!this.isInvertElementFilter && this.IsFilteredElement(element)) || (this.isInvertElementFilter && !this.IsFilteredElement(element));
		}

		// Token: 0x0600851A RID: 34074 RVA: 0x0032551C File Offset: 0x0032371C
		public void Drop()
		{
			bool flag = false;
			Element element = null;
			for (int i = this.m_storage.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = this.m_storage.items[i];
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (this.AllowedToDrop(component.ElementID))
				{
					if (base.def.asOre)
					{
						this.m_storage.Drop(gameObject, true);
						gameObject.transform.SetPosition(this.GetDropPosition());
						element = component.Element;
						flag = true;
					}
					else
					{
						Dumpable component2 = gameObject.GetComponent<Dumpable>();
						if (!component2.IsNullOrDestroyed())
						{
							component2.Dump(this.GetDropPosition());
							element = component.Element;
							flag = true;
						}
					}
				}
			}
			AutoStorageDropper.DropperFxConfig dropperAnim = this.GetDropperAnim();
			if (flag && dropperAnim != null && GameClock.Instance.GetTime() > this.m_timeSinceLastDrop + base.def.cooldown)
			{
				this.m_timeSinceLastDrop = GameClock.Instance.GetTime();
				Vector3 vector = Grid.CellToPosCCC(Grid.PosToCell(this.GetDropPosition()), dropperAnim.layer);
				vector += ((this.m_rotatable != null) ? this.m_rotatable.GetRotatedOffset(base.def.fxOffset) : base.def.fxOffset);
				KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect(dropperAnim.animFile, vector, null, false, dropperAnim.layer, false);
				kbatchedAnimController.destroyOnAnimComplete = false;
				kbatchedAnimController.FlipX = dropperAnim.flipX;
				kbatchedAnimController.FlipY = dropperAnim.flipY;
				if (dropperAnim.useElementTint)
				{
					kbatchedAnimController.TintColour = element.substance.colour;
				}
				kbatchedAnimController.Play(dropperAnim.animName, KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x0600851B RID: 34075 RVA: 0x003256E4 File Offset: 0x003238E4
		public AutoStorageDropper.DropperFxConfig GetDropperAnim()
		{
			CellOffset cellOffset = (this.m_rotatable != null) ? this.m_rotatable.GetRotatedCellOffset(base.def.dropOffset) : base.def.dropOffset;
			if (cellOffset.x < 0)
			{
				return base.def.leftFx;
			}
			if (cellOffset.x > 0)
			{
				return base.def.rightFx;
			}
			if (cellOffset.y < 0)
			{
				return base.def.downFx;
			}
			if (cellOffset.y > 0)
			{
				return base.def.upFx;
			}
			return base.def.neutralFx;
		}

		// Token: 0x0600851C RID: 34076 RVA: 0x00325784 File Offset: 0x00323984
		public Vector3 GetDropPosition()
		{
			if (!(this.m_rotatable != null))
			{
				return base.transform.GetPosition() + base.def.dropOffset.ToVector3();
			}
			return base.transform.GetPosition() + this.m_rotatable.GetRotatedCellOffset(base.def.dropOffset).ToVector3();
		}

		// Token: 0x040064F7 RID: 25847
		[MyCmpGet]
		private Storage m_storage;

		// Token: 0x040064F8 RID: 25848
		[MyCmpGet]
		private Rotatable m_rotatable;

		// Token: 0x040064FA RID: 25850
		private float m_timeSinceLastDrop;
	}
}
