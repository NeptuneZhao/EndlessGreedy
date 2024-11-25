using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000800 RID: 2048
public class DiseaseDropper : GameStateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>
{
	// Token: 0x0600389A RID: 14490 RVA: 0x00134E10 File Offset: 0x00133010
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.stopped;
		this.root.EventHandler(GameHashes.BurstEmitDisease, delegate(DiseaseDropper.Instance smi)
		{
			smi.DropSingleEmit();
		});
		this.working.TagTransition(GameTags.PreventEmittingDisease, this.stopped, false).Update(delegate(DiseaseDropper.Instance smi, float dt)
		{
			smi.DropPeriodic(dt);
		}, UpdateRate.SIM_200ms, false);
		this.stopped.TagTransition(GameTags.PreventEmittingDisease, this.working, true);
	}

	// Token: 0x040021FC RID: 8700
	public GameStateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.State working;

	// Token: 0x040021FD RID: 8701
	public GameStateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.State stopped;

	// Token: 0x040021FE RID: 8702
	public StateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.Signal cellChangedSignal;

	// Token: 0x020016EA RID: 5866
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x060093F0 RID: 37872 RVA: 0x0035A754 File Offset: 0x00358954
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			if (this.singleEmitQuantity > 0)
			{
				list.Add(new Descriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_DROPPER_BURST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.diseaseIdx, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.singleEmitQuantity, GameUtil.TimeSlice.None)), UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.DISEASE_DROPPER_BURST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.diseaseIdx, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.singleEmitQuantity, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
			}
			if (this.averageEmitPerSecond > 0)
			{
				list.Add(new Descriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_DROPPER_CONSTANT.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.diseaseIdx, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.averageEmitPerSecond, GameUtil.TimeSlice.PerSecond)), UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.DISEASE_DROPPER_CONSTANT.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.diseaseIdx, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.averageEmitPerSecond, GameUtil.TimeSlice.PerSecond)), Descriptor.DescriptorType.Effect, false));
			}
			return list;
		}

		// Token: 0x04007125 RID: 28965
		public byte diseaseIdx = byte.MaxValue;

		// Token: 0x04007126 RID: 28966
		public int singleEmitQuantity;

		// Token: 0x04007127 RID: 28967
		public int averageEmitPerSecond;

		// Token: 0x04007128 RID: 28968
		public float emitFrequency = 1f;
	}

	// Token: 0x020016EB RID: 5867
	public new class Instance : GameStateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.GameInstance
	{
		// Token: 0x060093F2 RID: 37874 RVA: 0x0035A876 File Offset: 0x00358A76
		public Instance(IStateMachineTarget master, DiseaseDropper.Def def) : base(master, def)
		{
		}

		// Token: 0x060093F3 RID: 37875 RVA: 0x0035A880 File Offset: 0x00358A80
		public bool ShouldDropDisease()
		{
			return true;
		}

		// Token: 0x060093F4 RID: 37876 RVA: 0x0035A883 File Offset: 0x00358A83
		public void DropSingleEmit()
		{
			this.DropDisease(base.def.diseaseIdx, base.def.singleEmitQuantity);
		}

		// Token: 0x060093F5 RID: 37877 RVA: 0x0035A8A4 File Offset: 0x00358AA4
		public void DropPeriodic(float dt)
		{
			this.timeSinceLastDrop += dt;
			if (base.def.averageEmitPerSecond > 0 && base.def.emitFrequency > 0f)
			{
				while (this.timeSinceLastDrop > base.def.emitFrequency)
				{
					this.DropDisease(base.def.diseaseIdx, (int)((float)base.def.averageEmitPerSecond * base.def.emitFrequency));
					this.timeSinceLastDrop -= base.def.emitFrequency;
				}
			}
		}

		// Token: 0x060093F6 RID: 37878 RVA: 0x0035A938 File Offset: 0x00358B38
		public void DropDisease(byte disease_idx, int disease_count)
		{
			if (disease_count <= 0 || disease_idx == 255)
			{
				return;
			}
			int num = Grid.PosToCell(base.transform.GetPosition());
			if (!Grid.IsValidCell(num))
			{
				return;
			}
			SimMessages.ModifyDiseaseOnCell(num, disease_idx, disease_count);
		}

		// Token: 0x060093F7 RID: 37879 RVA: 0x0035A974 File Offset: 0x00358B74
		public bool IsValidDropCell()
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			return Grid.IsValidCell(num) && Grid.IsGas(num) && Grid.Mass[num] <= 1f;
		}

		// Token: 0x04007129 RID: 28969
		private float timeSinceLastDrop;
	}
}
