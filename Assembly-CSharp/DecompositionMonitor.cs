using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000979 RID: 2425
public class DecompositionMonitor : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance>
{
	// Token: 0x060046F8 RID: 18168 RVA: 0x00195D80 File Offset: 0x00193F80
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.satisfied.Update("UpdateDecomposition", delegate(DecompositionMonitor.Instance smi, float dt)
		{
			smi.UpdateDecomposition(dt);
		}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.decomposition, this.rotten, GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.IsGTEOne).ToggleAttributeModifier("Dead", (DecompositionMonitor.Instance smi) => smi.satisfiedDecorModifier, null).ToggleAttributeModifier("Dead", (DecompositionMonitor.Instance smi) => smi.satisfiedDecorRadiusModifier, null);
		this.rotten.DefaultState(this.rotten.exposed).ToggleStatusItem(Db.Get().DuplicantStatusItems.Rotten, null).ToggleAttributeModifier("Rotten", (DecompositionMonitor.Instance smi) => smi.rottenDecorModifier, null).ToggleAttributeModifier("Rotten", (DecompositionMonitor.Instance smi) => smi.rottenDecorRadiusModifier, null);
		this.rotten.exposed.DefaultState(this.rotten.exposed.openair).EventTransition(GameHashes.OnStore, this.rotten.stored, (DecompositionMonitor.Instance smi) => !smi.IsExposed());
		this.rotten.exposed.openair.Enter(delegate(DecompositionMonitor.Instance smi)
		{
			if (smi.spawnsRotMonsters)
			{
				smi.ScheduleGoTo(UnityEngine.Random.Range(150f, 300f), this.rotten.spawningmonster);
			}
		}).Transition(this.rotten.exposed.submerged, (DecompositionMonitor.Instance smi) => smi.IsSubmerged(), UpdateRate.SIM_200ms).ToggleFX((DecompositionMonitor.Instance smi) => this.CreateFX(smi));
		this.rotten.exposed.submerged.DefaultState(this.rotten.exposed.submerged.idle).Transition(this.rotten.exposed.openair, (DecompositionMonitor.Instance smi) => !smi.IsSubmerged(), UpdateRate.SIM_200ms);
		this.rotten.exposed.submerged.idle.ScheduleGoTo(0.25f, this.rotten.exposed.submerged.dirtywater);
		this.rotten.exposed.submerged.dirtywater.Enter("DirtyWater", delegate(DecompositionMonitor.Instance smi)
		{
			smi.DirtyWater(smi.dirtyWaterMaxRange);
		}).GoTo(this.rotten.exposed.submerged.idle);
		this.rotten.spawningmonster.Enter(delegate(DecompositionMonitor.Instance smi)
		{
			if (this.remainingRotMonsters > 0)
			{
				this.remainingRotMonsters--;
				GameUtil.KInstantiate(Assets.GetPrefab(new Tag("Glom")), smi.transform.GetPosition(), Grid.SceneLayer.Creatures, null, 0).SetActive(true);
			}
			smi.GoTo(this.rotten.exposed);
		});
		this.rotten.stored.EventTransition(GameHashes.OnStore, this.rotten.exposed, (DecompositionMonitor.Instance smi) => smi.IsExposed());
	}

	// Token: 0x060046F9 RID: 18169 RVA: 0x001960C0 File Offset: 0x001942C0
	private FliesFX.Instance CreateFX(DecompositionMonitor.Instance smi)
	{
		if (!smi.isMasterNull)
		{
			return new FliesFX.Instance(smi.master, new Vector3(0f, 0f, -0.1f));
		}
		return null;
	}

	// Token: 0x04002E41 RID: 11841
	public StateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.FloatParameter decomposition;

	// Token: 0x04002E42 RID: 11842
	[SerializeField]
	public int remainingRotMonsters = 3;

	// Token: 0x04002E43 RID: 11843
	public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002E44 RID: 11844
	public DecompositionMonitor.RottenState rotten;

	// Token: 0x0200191D RID: 6429
	public class SubmergedState : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400785C RID: 30812
		public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x0400785D RID: 30813
		public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State dirtywater;
	}

	// Token: 0x0200191E RID: 6430
	public class ExposedState : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400785E RID: 30814
		public DecompositionMonitor.SubmergedState submerged;

		// Token: 0x0400785F RID: 30815
		public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State openair;
	}

	// Token: 0x0200191F RID: 6431
	public class RottenState : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04007860 RID: 30816
		public DecompositionMonitor.ExposedState exposed;

		// Token: 0x04007861 RID: 30817
		public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State stored;

		// Token: 0x04007862 RID: 30818
		public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State spawningmonster;
	}

	// Token: 0x02001920 RID: 6432
	public new class Instance : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009B3D RID: 39741 RVA: 0x0036F038 File Offset: 0x0036D238
		public Instance(IStateMachineTarget master, Disease disease, float decompositionRate = 0.00083333335f, bool spawnRotMonsters = true) : base(master)
		{
			base.gameObject.AddComponent<DecorProvider>();
			this.decompositionRate = decompositionRate;
			this.disease = disease;
			this.spawnsRotMonsters = spawnRotMonsters;
		}

		// Token: 0x06009B3E RID: 39742 RVA: 0x0036F140 File Offset: 0x0036D340
		public void UpdateDecomposition(float dt)
		{
			float delta_value = dt * this.decompositionRate;
			base.sm.decomposition.Delta(delta_value, base.smi);
		}

		// Token: 0x06009B3F RID: 39743 RVA: 0x0036F170 File Offset: 0x0036D370
		public bool IsExposed()
		{
			KPrefabID component = base.smi.GetComponent<KPrefabID>();
			return component == null || !component.HasTag(GameTags.Preserved);
		}

		// Token: 0x06009B40 RID: 39744 RVA: 0x0036F1A2 File Offset: 0x0036D3A2
		public bool IsRotten()
		{
			return base.IsInsideState(base.sm.rotten);
		}

		// Token: 0x06009B41 RID: 39745 RVA: 0x0036F1B5 File Offset: 0x0036D3B5
		public bool IsSubmerged()
		{
			return PathFinder.IsSubmerged(Grid.PosToCell(base.master.transform.GetPosition()));
		}

		// Token: 0x06009B42 RID: 39746 RVA: 0x0036F1D4 File Offset: 0x0036D3D4
		public void DirtyWater(int maxCellRange = 3)
		{
			int num = Grid.PosToCell(base.master.transform.GetPosition());
			if (Grid.Element[num].id == SimHashes.Water)
			{
				SimMessages.ReplaceElement(num, SimHashes.DirtyWater, CellEventLogger.Instance.DecompositionDirtyWater, Grid.Mass[num], Grid.Temperature[num], Grid.DiseaseIdx[num], Grid.DiseaseCount[num], -1);
				return;
			}
			if (Grid.Element[num].id == SimHashes.DirtyWater)
			{
				int[] array = new int[4];
				for (int i = 0; i < maxCellRange; i++)
				{
					for (int j = 0; j < maxCellRange; j++)
					{
						array[0] = Grid.OffsetCell(num, new CellOffset(-i, j));
						array[1] = Grid.OffsetCell(num, new CellOffset(i, j));
						array[2] = Grid.OffsetCell(num, new CellOffset(-i, -j));
						array[3] = Grid.OffsetCell(num, new CellOffset(i, -j));
						array.Shuffle<int>();
						foreach (int num2 in array)
						{
							if (Grid.GetCellDistance(num, num2) < maxCellRange - 1 && Grid.IsValidCell(num2) && Grid.Element[num2].id == SimHashes.Water)
							{
								SimMessages.ReplaceElement(num2, SimHashes.DirtyWater, CellEventLogger.Instance.DecompositionDirtyWater, Grid.Mass[num2], Grid.Temperature[num2], Grid.DiseaseIdx[num2], Grid.DiseaseCount[num2], -1);
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x04007863 RID: 30819
		public float decompositionRate;

		// Token: 0x04007864 RID: 30820
		public Disease disease;

		// Token: 0x04007865 RID: 30821
		public int dirtyWaterMaxRange = 3;

		// Token: 0x04007866 RID: 30822
		public bool spawnsRotMonsters = true;

		// Token: 0x04007867 RID: 30823
		public AttributeModifier satisfiedDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, -65f, DUPLICANTS.MODIFIERS.DEAD.NAME, false, false, true);

		// Token: 0x04007868 RID: 30824
		public AttributeModifier satisfiedDecorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, 4f, DUPLICANTS.MODIFIERS.DEAD.NAME, false, false, true);

		// Token: 0x04007869 RID: 30825
		public AttributeModifier rottenDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, -100f, DUPLICANTS.MODIFIERS.ROTTING.NAME, false, false, true);

		// Token: 0x0400786A RID: 30826
		public AttributeModifier rottenDecorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, 4f, DUPLICANTS.MODIFIERS.ROTTING.NAME, false, false, true);
	}
}
