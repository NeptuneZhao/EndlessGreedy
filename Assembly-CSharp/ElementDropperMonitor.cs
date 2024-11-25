using System;
using UnityEngine;

// Token: 0x02000804 RID: 2052
public class ElementDropperMonitor : GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>
{
	// Token: 0x060038B0 RID: 14512 RVA: 0x001355B8 File Offset: 0x001337B8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.EventHandler(GameHashes.DeathAnimComplete, delegate(ElementDropperMonitor.Instance smi)
		{
			smi.DropDeathElement();
		});
		this.satisfied.OnSignal(this.cellChangedSignal, this.readytodrop, (ElementDropperMonitor.Instance smi) => smi.ShouldDropElement());
		this.readytodrop.ToggleBehaviour(GameTags.Creatures.WantsToDropElements, (ElementDropperMonitor.Instance smi) => true, delegate(ElementDropperMonitor.Instance smi)
		{
			smi.GoTo(this.satisfied);
		}).EventHandler(GameHashes.ObjectMovementStateChanged, delegate(ElementDropperMonitor.Instance smi, object d)
		{
			if ((GameHashes)d == GameHashes.ObjectMovementWakeUp)
			{
				smi.GoTo(this.satisfied);
			}
		});
	}

	// Token: 0x04002214 RID: 8724
	public GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.State satisfied;

	// Token: 0x04002215 RID: 8725
	public GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.State readytodrop;

	// Token: 0x04002216 RID: 8726
	public StateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.Signal cellChangedSignal;

	// Token: 0x020016F2 RID: 5874
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400713E RID: 28990
		public SimHashes dirtyEmitElement;

		// Token: 0x0400713F RID: 28991
		public float dirtyProbabilityPercent;

		// Token: 0x04007140 RID: 28992
		public float dirtyCellToTargetMass;

		// Token: 0x04007141 RID: 28993
		public float dirtyMassPerDirty;

		// Token: 0x04007142 RID: 28994
		public float dirtyMassReleaseOnDeath;

		// Token: 0x04007143 RID: 28995
		public byte emitDiseaseIdx = byte.MaxValue;

		// Token: 0x04007144 RID: 28996
		public float emitDiseasePerKg;
	}

	// Token: 0x020016F3 RID: 5875
	public new class Instance : GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.GameInstance
	{
		// Token: 0x06009411 RID: 37905 RVA: 0x0035AE69 File Offset: 0x00359069
		public Instance(IStateMachineTarget master, ElementDropperMonitor.Def def) : base(master, def)
		{
			Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "ElementDropperMonitor.Instance");
		}

		// Token: 0x06009412 RID: 37906 RVA: 0x0035AE95 File Offset: 0x00359095
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		}

		// Token: 0x06009413 RID: 37907 RVA: 0x0035AEBA File Offset: 0x003590BA
		private void OnCellChange()
		{
			base.sm.cellChangedSignal.Trigger(this);
		}

		// Token: 0x06009414 RID: 37908 RVA: 0x0035AECD File Offset: 0x003590CD
		public bool ShouldDropElement()
		{
			return this.IsValidDropCell() && UnityEngine.Random.Range(0f, 100f) < base.def.dirtyProbabilityPercent;
		}

		// Token: 0x06009415 RID: 37909 RVA: 0x0035AEF8 File Offset: 0x003590F8
		public void DropDeathElement()
		{
			this.DropElement(base.def.dirtyMassReleaseOnDeath, base.def.dirtyEmitElement, base.def.emitDiseaseIdx, Mathf.RoundToInt(base.def.dirtyMassReleaseOnDeath * base.def.dirtyMassPerDirty));
		}

		// Token: 0x06009416 RID: 37910 RVA: 0x0035AF48 File Offset: 0x00359148
		public void DropPeriodicElement()
		{
			this.DropElement(base.def.dirtyMassPerDirty, base.def.dirtyEmitElement, base.def.emitDiseaseIdx, Mathf.RoundToInt(base.def.emitDiseasePerKg * base.def.dirtyMassPerDirty));
		}

		// Token: 0x06009417 RID: 37911 RVA: 0x0035AF98 File Offset: 0x00359198
		public void DropElement(float mass, SimHashes element_id, byte disease_idx, int disease_count)
		{
			if (mass <= 0f)
			{
				return;
			}
			Element element = ElementLoader.FindElementByHash(element_id);
			float temperature = base.GetComponent<PrimaryElement>().Temperature;
			if (element.IsGas || element.IsLiquid)
			{
				SimMessages.AddRemoveSubstance(Grid.PosToCell(base.transform.GetPosition()), element_id, CellEventLogger.Instance.ElementConsumerSimUpdate, mass, temperature, disease_idx, disease_count, true, -1);
			}
			else if (element.IsSolid)
			{
				element.substance.SpawnResource(base.transform.GetPosition() + new Vector3(0f, 0.5f, 0f), mass, temperature, disease_idx, disease_count, false, true, false);
			}
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, element.name, base.gameObject.transform, 1.5f, false);
		}

		// Token: 0x06009418 RID: 37912 RVA: 0x0035B068 File Offset: 0x00359268
		public bool IsValidDropCell()
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			return Grid.IsValidCell(num) && Grid.IsGas(num) && Grid.Mass[num] <= 1f;
		}
	}
}
