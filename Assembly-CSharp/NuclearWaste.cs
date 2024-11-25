using System;
using KSerialization;

// Token: 0x020009CA RID: 2506
public class NuclearWaste : GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>
{
	// Token: 0x060048CA RID: 18634 RVA: 0x001A0298 File Offset: 0x0019E498
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.PlayAnim((NuclearWaste.Instance smi) => smi.GetAnimToPlay(), KAnim.PlayMode.Once).Update(delegate(NuclearWaste.Instance smi, float dt)
		{
			smi.timeAlive += dt;
			string animToPlay = smi.GetAnimToPlay();
			if (smi.GetComponent<KBatchedAnimController>().GetCurrentAnim().name != animToPlay)
			{
				smi.Play(animToPlay, KAnim.PlayMode.Once);
			}
			if (smi.timeAlive >= 600f)
			{
				smi.GoTo(this.decayed);
			}
		}, UpdateRate.SIM_4000ms, false).EventHandler(GameHashes.Absorb, delegate(NuclearWaste.Instance smi, object otherObject)
		{
			Pickupable pickupable = (Pickupable)otherObject;
			float timeAlive = pickupable.GetSMI<NuclearWaste.Instance>().timeAlive;
			float mass = pickupable.PrimaryElement.Mass;
			float mass2 = smi.master.GetComponent<PrimaryElement>().Mass;
			float timeAlive2 = ((mass2 - mass) * smi.timeAlive + mass * timeAlive) / mass2;
			smi.timeAlive = timeAlive2;
			string animToPlay = smi.GetAnimToPlay();
			if (smi.GetComponent<KBatchedAnimController>().GetCurrentAnim().name != animToPlay)
			{
				smi.Play(animToPlay, KAnim.PlayMode.Once);
			}
			if (smi.timeAlive >= 600f)
			{
				smi.GoTo(this.decayed);
			}
		});
		this.decayed.Enter(delegate(NuclearWaste.Instance smi)
		{
			smi.GetComponent<Dumpable>().Dump();
			Util.KDestroyGameObject(smi.master.gameObject);
		});
	}

	// Token: 0x04002FA5 RID: 12197
	private const float lifetime = 600f;

	// Token: 0x04002FA6 RID: 12198
	public GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>.State idle;

	// Token: 0x04002FA7 RID: 12199
	public GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>.State decayed;

	// Token: 0x020019D7 RID: 6615
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020019D8 RID: 6616
	public new class Instance : GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>.GameInstance
	{
		// Token: 0x06009E34 RID: 40500 RVA: 0x00377545 File Offset: 0x00375745
		public Instance(IStateMachineTarget master, NuclearWaste.Def def) : base(master, def)
		{
		}

		// Token: 0x06009E35 RID: 40501 RVA: 0x00377550 File Offset: 0x00375750
		public string GetAnimToPlay()
		{
			this.percentageRemaining = 1f - base.smi.timeAlive / 600f;
			if (this.percentageRemaining <= 0.33f)
			{
				return "idle1";
			}
			if (this.percentageRemaining <= 0.66f)
			{
				return "idle2";
			}
			return "idle3";
		}

		// Token: 0x04007AB5 RID: 31413
		[Serialize]
		public float timeAlive;

		// Token: 0x04007AB6 RID: 31414
		private float percentageRemaining;
	}
}
