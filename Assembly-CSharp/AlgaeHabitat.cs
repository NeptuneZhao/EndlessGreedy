using System;
using UnityEngine;

// Token: 0x0200067D RID: 1661
public class AlgaeHabitat : StateMachineComponent<AlgaeHabitat.SMInstance>
{
	// Token: 0x0600291F RID: 10527 RVA: 0x000E8A94 File Offset: 0x000E6C94
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		GameScheduler.Instance.Schedule("WaterFetchingTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_FetchingWater, true);
		}, null, null);
		this.ConfigurePollutedWaterOutput();
		Tutorial.Instance.oxygenGenerators.Add(base.gameObject);
	}

	// Token: 0x06002920 RID: 10528 RVA: 0x000E8B03 File Offset: 0x000E6D03
	protected override void OnCleanUp()
	{
		Tutorial.Instance.oxygenGenerators.Remove(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x06002921 RID: 10529 RVA: 0x000E8B24 File Offset: 0x000E6D24
	private void ConfigurePollutedWaterOutput()
	{
		Storage storage = null;
		Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
		foreach (Storage storage2 in base.GetComponents<Storage>())
		{
			if (storage2.storageFilters.Contains(tag))
			{
				storage = storage2;
				break;
			}
		}
		foreach (ElementConverter elementConverter in base.GetComponents<ElementConverter>())
		{
			ElementConverter.OutputElement[] outputElements = elementConverter.outputElements;
			for (int j = 0; j < outputElements.Length; j++)
			{
				if (outputElements[j].elementHash == SimHashes.DirtyWater)
				{
					elementConverter.SetStorage(storage);
					break;
				}
			}
		}
		this.pollutedWaterStorage = storage;
	}

	// Token: 0x040017AD RID: 6061
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040017AE RID: 6062
	private Storage pollutedWaterStorage;

	// Token: 0x040017AF RID: 6063
	[SerializeField]
	public float lightBonusMultiplier = 1.1f;

	// Token: 0x040017B0 RID: 6064
	public CellOffset pressureSampleOffset = CellOffset.none;

	// Token: 0x02001461 RID: 5217
	public class SMInstance : GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.GameInstance
	{
		// Token: 0x06008A6F RID: 35439 RVA: 0x00333B00 File Offset: 0x00331D00
		public SMInstance(AlgaeHabitat master) : base(master)
		{
			this.converter = master.GetComponent<ElementConverter>();
		}

		// Token: 0x06008A70 RID: 35440 RVA: 0x00333B15 File Offset: 0x00331D15
		public bool HasEnoughMass(Tag tag)
		{
			return this.converter.HasEnoughMass(tag, false);
		}

		// Token: 0x06008A71 RID: 35441 RVA: 0x00333B24 File Offset: 0x00331D24
		public bool NeedsEmptying()
		{
			return base.smi.master.pollutedWaterStorage.RemainingCapacity() <= 0f;
		}

		// Token: 0x06008A72 RID: 35442 RVA: 0x00333B48 File Offset: 0x00331D48
		public void CreateEmptyChore()
		{
			if (this.emptyChore != null)
			{
				this.emptyChore.Cancel("dupe");
			}
			AlgaeHabitatEmpty component = base.master.GetComponent<AlgaeHabitatEmpty>();
			this.emptyChore = new WorkChore<AlgaeHabitatEmpty>(Db.Get().ChoreTypes.EmptyStorage, component, null, true, new Action<Chore>(this.OnEmptyComplete), null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
		}

		// Token: 0x06008A73 RID: 35443 RVA: 0x00333BB0 File Offset: 0x00331DB0
		public void CancelEmptyChore()
		{
			if (this.emptyChore != null)
			{
				this.emptyChore.Cancel("Cancelled");
				this.emptyChore = null;
			}
		}

		// Token: 0x06008A74 RID: 35444 RVA: 0x00333BD4 File Offset: 0x00331DD4
		private void OnEmptyComplete(Chore chore)
		{
			this.emptyChore = null;
			base.master.pollutedWaterStorage.DropAll(true, false, default(Vector3), true, null);
		}

		// Token: 0x04006996 RID: 27030
		public ElementConverter converter;

		// Token: 0x04006997 RID: 27031
		public Chore emptyChore;
	}

	// Token: 0x02001462 RID: 5218
	public class States : GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat>
	{
		// Token: 0x06008A75 RID: 35445 RVA: 0x00333C08 File Offset: 0x00331E08
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.noAlgae;
			this.root.EventTransition(GameHashes.OperationalChanged, this.notoperational, (AlgaeHabitat.SMInstance smi) => !smi.master.operational.IsOperational).EventTransition(GameHashes.OperationalChanged, this.noAlgae, (AlgaeHabitat.SMInstance smi) => smi.master.operational.IsOperational);
			this.notoperational.QueueAnim("off", false, null);
			this.gotAlgae.PlayAnim("on_pre").OnAnimQueueComplete(this.noWater);
			this.gotEmptied.PlayAnim("on_pre").OnAnimQueueComplete(this.generatingOxygen);
			this.lostAlgae.PlayAnim("on_pst").OnAnimQueueComplete(this.noAlgae);
			this.noAlgae.QueueAnim("off", false, null).EventTransition(GameHashes.OnStorageChange, this.gotAlgae, (AlgaeHabitat.SMInstance smi) => smi.HasEnoughMass(GameTags.Algae)).Enter(delegate(AlgaeHabitat.SMInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
			this.noWater.QueueAnim("on", false, null).Enter(delegate(AlgaeHabitat.SMInstance smi)
			{
				smi.master.GetComponent<PassiveElementConsumer>().EnableConsumption(true);
			}).EventTransition(GameHashes.OnStorageChange, this.lostAlgae, (AlgaeHabitat.SMInstance smi) => !smi.HasEnoughMass(GameTags.Algae)).EventTransition(GameHashes.OnStorageChange, this.gotWater, (AlgaeHabitat.SMInstance smi) => smi.HasEnoughMass(GameTags.Algae) && smi.HasEnoughMass(GameTags.Water));
			this.needsEmptying.QueueAnim("off", false, null).Enter(delegate(AlgaeHabitat.SMInstance smi)
			{
				smi.CreateEmptyChore();
			}).Exit(delegate(AlgaeHabitat.SMInstance smi)
			{
				smi.CancelEmptyChore();
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.HabitatNeedsEmptying, null).EventTransition(GameHashes.OnStorageChange, this.noAlgae, (AlgaeHabitat.SMInstance smi) => !smi.HasEnoughMass(GameTags.Algae) || !smi.HasEnoughMass(GameTags.Water)).EventTransition(GameHashes.OnStorageChange, this.gotEmptied, (AlgaeHabitat.SMInstance smi) => smi.HasEnoughMass(GameTags.Algae) && smi.HasEnoughMass(GameTags.Water) && !smi.NeedsEmptying());
			this.gotWater.PlayAnim("working_pre").OnAnimQueueComplete(this.needsEmptying);
			this.generatingOxygen.Enter(delegate(AlgaeHabitat.SMInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(AlgaeHabitat.SMInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).Update("GeneratingOxygen", delegate(AlgaeHabitat.SMInstance smi, float dt)
			{
				int num = Grid.PosToCell(smi.master.transform.GetPosition());
				smi.converter.OutputMultiplier = ((Grid.LightCount[num] > 0) ? smi.master.lightBonusMultiplier : 1f);
			}, UpdateRate.SIM_200ms, false).QueueAnim("working_loop", true, null).EventTransition(GameHashes.OnStorageChange, this.stoppedGeneratingOxygen, (AlgaeHabitat.SMInstance smi) => !smi.HasEnoughMass(GameTags.Water) || !smi.HasEnoughMass(GameTags.Algae) || smi.NeedsEmptying());
			this.stoppedGeneratingOxygen.PlayAnim("working_pst").OnAnimQueueComplete(this.stoppedGeneratingOxygenTransition);
			this.stoppedGeneratingOxygenTransition.EventTransition(GameHashes.OnStorageChange, this.needsEmptying, (AlgaeHabitat.SMInstance smi) => smi.NeedsEmptying()).EventTransition(GameHashes.OnStorageChange, this.noWater, (AlgaeHabitat.SMInstance smi) => !smi.HasEnoughMass(GameTags.Water)).EventTransition(GameHashes.OnStorageChange, this.lostAlgae, (AlgaeHabitat.SMInstance smi) => !smi.HasEnoughMass(GameTags.Algae)).EventTransition(GameHashes.OnStorageChange, this.gotWater, (AlgaeHabitat.SMInstance smi) => smi.HasEnoughMass(GameTags.Water) && smi.HasEnoughMass(GameTags.Algae));
		}

		// Token: 0x04006998 RID: 27032
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State generatingOxygen;

		// Token: 0x04006999 RID: 27033
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State stoppedGeneratingOxygen;

		// Token: 0x0400699A RID: 27034
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State stoppedGeneratingOxygenTransition;

		// Token: 0x0400699B RID: 27035
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State noWater;

		// Token: 0x0400699C RID: 27036
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State noAlgae;

		// Token: 0x0400699D RID: 27037
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State needsEmptying;

		// Token: 0x0400699E RID: 27038
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State gotAlgae;

		// Token: 0x0400699F RID: 27039
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State gotWater;

		// Token: 0x040069A0 RID: 27040
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State gotEmptied;

		// Token: 0x040069A1 RID: 27041
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State lostAlgae;

		// Token: 0x040069A2 RID: 27042
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State notoperational;
	}
}
