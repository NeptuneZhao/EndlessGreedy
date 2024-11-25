using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x020006F7 RID: 1783
public class LadderBed : GameStateMachine<LadderBed, LadderBed.Instance, IStateMachineTarget, LadderBed.Def>
{
	// Token: 0x06002D90 RID: 11664 RVA: 0x000FFBEE File Offset: 0x000FDDEE
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
	}

	// Token: 0x04001A76 RID: 6774
	public static string lightBedShakeSoundPath = GlobalAssets.GetSound("LadderBed_LightShake", false);

	// Token: 0x04001A77 RID: 6775
	public static string noDupeBedShakeSoundPath = GlobalAssets.GetSound("LadderBed_Shake", false);

	// Token: 0x04001A78 RID: 6776
	public static string LADDER_BED_COUNT_BELOW_PARAMETER = "bed_count";

	// Token: 0x0200152C RID: 5420
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006C23 RID: 27683
		public CellOffset[] offsets;
	}

	// Token: 0x0200152D RID: 5421
	public new class Instance : GameStateMachine<LadderBed, LadderBed.Instance, IStateMachineTarget, LadderBed.Def>.GameInstance
	{
		// Token: 0x06008D78 RID: 36216 RVA: 0x0033F920 File Offset: 0x0033DB20
		public Instance(IStateMachineTarget master, LadderBed.Def def) : base(master, def)
		{
			ScenePartitionerLayer scenePartitionerLayer = GameScenePartitioner.Instance.objectLayers[40];
			this.m_cell = Grid.PosToCell(master.gameObject);
			foreach (CellOffset offset in def.offsets)
			{
				int cell = Grid.OffsetCell(this.m_cell, offset);
				if (Grid.IsValidCell(this.m_cell) && Grid.IsValidCell(cell))
				{
					this.m_partitionEntires.Add(GameScenePartitioner.Instance.Add("LadderBed.Constructor", base.gameObject, cell, GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.OnMoverChanged)));
					this.OnMoverChanged(null);
				}
			}
			AttachableBuilding attachable = this.m_attachable;
			attachable.onAttachmentNetworkChanged = (Action<object>)Delegate.Combine(attachable.onAttachmentNetworkChanged, new Action<object>(this.OnAttachmentChanged));
			this.OnAttachmentChanged(null);
			base.Subscribe(-717201811, new Action<object>(this.OnSleepDisturbedByMovement));
			master.GetComponent<KAnimControllerBase>().GetLayering().GetLink().syncTint = false;
		}

		// Token: 0x06008D79 RID: 36217 RVA: 0x0033FA38 File Offset: 0x0033DC38
		private void OnSleepDisturbedByMovement(object obj)
		{
			base.GetComponent<KAnimControllerBase>().Play("interrupt_light", KAnim.PlayMode.Once, 1f, 0f);
			EventInstance instance = SoundEvent.BeginOneShot(LadderBed.lightBedShakeSoundPath, base.smi.transform.GetPosition(), 1f, false);
			instance.setParameterByName(LadderBed.LADDER_BED_COUNT_BELOW_PARAMETER, (float)this.numBelow, false);
			SoundEvent.EndOneShot(instance);
		}

		// Token: 0x06008D7A RID: 36218 RVA: 0x0033FAA2 File Offset: 0x0033DCA2
		private void OnAttachmentChanged(object data)
		{
			this.numBelow = AttachableBuilding.CountAttachedBelow(this.m_attachable);
		}

		// Token: 0x06008D7B RID: 36219 RVA: 0x0033FAB8 File Offset: 0x0033DCB8
		private void OnMoverChanged(object obj)
		{
			Pickupable pickupable = obj as Pickupable;
			if (pickupable != null && pickupable.gameObject != null && pickupable.KPrefabID.HasTag(GameTags.BaseMinion) && pickupable.GetComponent<Navigator>().CurrentNavType == NavType.Ladder)
			{
				if (this.m_sleepable.worker == null)
				{
					base.GetComponent<KAnimControllerBase>().Play("interrupt_light_nodupe", KAnim.PlayMode.Once, 1f, 0f);
					EventInstance instance = SoundEvent.BeginOneShot(LadderBed.noDupeBedShakeSoundPath, base.smi.transform.GetPosition(), 1f, false);
					instance.setParameterByName(LadderBed.LADDER_BED_COUNT_BELOW_PARAMETER, (float)this.numBelow, false);
					SoundEvent.EndOneShot(instance);
					return;
				}
				if (pickupable.gameObject != this.m_sleepable.worker.gameObject)
				{
					this.m_sleepable.worker.Trigger(-717201811, null);
				}
			}
		}

		// Token: 0x06008D7C RID: 36220 RVA: 0x0033FBB4 File Offset: 0x0033DDB4
		protected override void OnCleanUp()
		{
			foreach (HandleVector<int>.Handle handle in this.m_partitionEntires)
			{
				GameScenePartitioner.Instance.Free(ref handle);
			}
			AttachableBuilding attachable = this.m_attachable;
			attachable.onAttachmentNetworkChanged = (Action<object>)Delegate.Remove(attachable.onAttachmentNetworkChanged, new Action<object>(this.OnAttachmentChanged));
			base.OnCleanUp();
		}

		// Token: 0x04006C24 RID: 27684
		private List<HandleVector<int>.Handle> m_partitionEntires = new List<HandleVector<int>.Handle>();

		// Token: 0x04006C25 RID: 27685
		private int m_cell;

		// Token: 0x04006C26 RID: 27686
		[MyCmpGet]
		private Ownable m_ownable;

		// Token: 0x04006C27 RID: 27687
		[MyCmpGet]
		private Sleepable m_sleepable;

		// Token: 0x04006C28 RID: 27688
		[MyCmpGet]
		private AttachableBuilding m_attachable;

		// Token: 0x04006C29 RID: 27689
		private int numBelow;
	}
}
