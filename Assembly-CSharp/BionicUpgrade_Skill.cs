using System;

// Token: 0x02000666 RID: 1638
public class BionicUpgrade_Skill : GameStateMachine<BionicUpgrade_Skill, BionicUpgrade_Skill.Instance, IStateMachineTarget, BionicUpgrade_Skill.Def>
{
	// Token: 0x06002865 RID: 10341 RVA: 0x000E4E04 File Offset: 0x000E3004
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
		this.root.Enter(new StateMachine<BionicUpgrade_Skill, BionicUpgrade_Skill.Instance, IStateMachineTarget, BionicUpgrade_Skill.Def>.State.Callback(BionicUpgrade_Skill.EnableEffect)).Exit(new StateMachine<BionicUpgrade_Skill, BionicUpgrade_Skill.Instance, IStateMachineTarget, BionicUpgrade_Skill.Def>.State.Callback(BionicUpgrade_Skill.DisableEffect));
	}

	// Token: 0x06002866 RID: 10342 RVA: 0x000E4E3E File Offset: 0x000E303E
	public static void EnableEffect(BionicUpgrade_Skill.Instance smi)
	{
		smi.ApplySkill();
	}

	// Token: 0x06002867 RID: 10343 RVA: 0x000E4E46 File Offset: 0x000E3046
	public static void DisableEffect(BionicUpgrade_Skill.Instance smi)
	{
		smi.RemoveSkill();
	}

	// Token: 0x02001448 RID: 5192
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400694B RID: 26955
		public string SKILL_ID;
	}

	// Token: 0x02001449 RID: 5193
	public new class Instance : GameStateMachine<BionicUpgrade_Skill, BionicUpgrade_Skill.Instance, IStateMachineTarget, BionicUpgrade_Skill.Def>.GameInstance
	{
		// Token: 0x06008A04 RID: 35332 RVA: 0x0033206F File Offset: 0x0033026F
		public Instance(IStateMachineTarget master, BionicUpgrade_Skill.Def def) : base(master, def)
		{
			this.resume = base.GetComponent<MinionResume>();
		}

		// Token: 0x06008A05 RID: 35333 RVA: 0x00332085 File Offset: 0x00330285
		public void ApplySkill()
		{
			this.resume.GrantSkill(base.def.SKILL_ID);
		}

		// Token: 0x06008A06 RID: 35334 RVA: 0x0033209D File Offset: 0x0033029D
		public void RemoveSkill()
		{
			this.resume.UngrantSkill(base.def.SKILL_ID);
		}

		// Token: 0x0400694C RID: 26956
		private MinionResume resume;
	}
}
