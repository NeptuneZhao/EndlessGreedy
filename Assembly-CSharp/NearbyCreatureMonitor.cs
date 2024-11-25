using System;
using System.Collections.Generic;

// Token: 0x02000815 RID: 2069
public class NearbyCreatureMonitor : GameStateMachine<NearbyCreatureMonitor, NearbyCreatureMonitor.Instance, IStateMachineTarget>
{
	// Token: 0x0600393D RID: 14653 RVA: 0x00138320 File Offset: 0x00136520
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update("UpdateNearbyCreatures", delegate(NearbyCreatureMonitor.Instance smi, float dt)
		{
			smi.UpdateNearbyCreatures(dt);
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x02001720 RID: 5920
	public new class Instance : GameStateMachine<NearbyCreatureMonitor, NearbyCreatureMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x14000035 RID: 53
		// (add) Token: 0x060094C3 RID: 38083 RVA: 0x0035D9A4 File Offset: 0x0035BBA4
		// (remove) Token: 0x060094C4 RID: 38084 RVA: 0x0035D9DC File Offset: 0x0035BBDC
		public event Action<float, List<KPrefabID>, List<KPrefabID>> OnUpdateNearbyCreatures;

		// Token: 0x060094C5 RID: 38085 RVA: 0x0035DA11 File Offset: 0x0035BC11
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060094C6 RID: 38086 RVA: 0x0035DA1C File Offset: 0x0035BC1C
		public void UpdateNearbyCreatures(float dt)
		{
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(base.gameObject));
			if (cavityForCell != null)
			{
				this.OnUpdateNearbyCreatures(dt, cavityForCell.creatures, cavityForCell.eggs);
			}
		}
	}
}
