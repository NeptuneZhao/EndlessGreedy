using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000AD7 RID: 2775
[SerializationConfig(MemberSerialization.OptIn)]
public class PodLander : StateMachineComponent<PodLander.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06005281 RID: 21121 RVA: 0x001D952C File Offset: 0x001D772C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06005282 RID: 21122 RVA: 0x001D9540 File Offset: 0x001D7740
	public void ReleaseAstronaut()
	{
		if (this.releasingAstronaut)
		{
			return;
		}
		this.releasingAstronaut = true;
		MinionStorage component = base.GetComponent<MinionStorage>();
		List<MinionStorage.Info> storedMinionInfo = component.GetStoredMinionInfo();
		for (int i = storedMinionInfo.Count - 1; i >= 0; i--)
		{
			MinionStorage.Info info = storedMinionInfo[i];
			component.DeserializeMinion(info.id, Grid.CellToPos(Grid.PosToCell(base.smi.master.transform.GetPosition())));
		}
		this.releasingAstronaut = false;
	}

	// Token: 0x06005283 RID: 21123 RVA: 0x001D95B9 File Offset: 0x001D77B9
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

	// Token: 0x04003664 RID: 13924
	[Serialize]
	private int landOffLocation;

	// Token: 0x04003665 RID: 13925
	[Serialize]
	private float flightAnimOffset;

	// Token: 0x04003666 RID: 13926
	private float rocketSpeed;

	// Token: 0x04003667 RID: 13927
	public float exhaustEmitRate = 2f;

	// Token: 0x04003668 RID: 13928
	public float exhaustTemperature = 1000f;

	// Token: 0x04003669 RID: 13929
	public SimHashes exhaustElement = SimHashes.CarbonDioxide;

	// Token: 0x0400366A RID: 13930
	private GameObject soundSpeakerObject;

	// Token: 0x0400366B RID: 13931
	private bool releasingAstronaut;

	// Token: 0x02001B27 RID: 6951
	public class StatesInstance : GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander, object>.GameInstance
	{
		// Token: 0x0600A2A2 RID: 41634 RVA: 0x00387741 File Offset: 0x00385941
		public StatesInstance(PodLander master) : base(master)
		{
		}
	}

	// Token: 0x02001B28 RID: 6952
	public class States : GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander>
	{
		// Token: 0x0600A2A3 RID: 41635 RVA: 0x0038774C File Offset: 0x0038594C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.landing;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.landing.PlayAnim("launch_loop", KAnim.PlayMode.Loop).Enter(delegate(PodLander.StatesInstance smi)
			{
				smi.master.flightAnimOffset = 50f;
			}).Update(delegate(PodLander.StatesInstance smi, float dt)
			{
				float num = 10f;
				smi.master.rocketSpeed = num - Mathf.Clamp(Mathf.Pow(smi.timeinstate / 3.5f, 4f), 0f, num - 2f);
				smi.master.flightAnimOffset -= dt * smi.master.rocketSpeed;
				KBatchedAnimController component = smi.master.GetComponent<KBatchedAnimController>();
				component.Offset = Vector3.up * smi.master.flightAnimOffset;
				Vector3 positionIncludingOffset = component.PositionIncludingOffset;
				int num2 = Grid.PosToCell(smi.master.gameObject.transform.GetPosition() + smi.master.GetComponent<KBatchedAnimController>().Offset);
				if (Grid.IsValidCell(num2))
				{
					SimMessages.EmitMass(num2, ElementLoader.GetElementIndex(smi.master.exhaustElement), dt * smi.master.exhaustEmitRate, smi.master.exhaustTemperature, 0, 0, -1);
				}
				if (component.Offset.y <= 0f)
				{
					smi.GoTo(this.crashed);
				}
			}, UpdateRate.SIM_33ms, false);
			this.crashed.PlayAnim("grounded").Enter(delegate(PodLander.StatesInstance smi)
			{
				smi.master.GetComponent<KBatchedAnimController>().Offset = Vector3.zero;
				smi.master.rocketSpeed = 0f;
				smi.master.ReleaseAstronaut();
			});
		}

		// Token: 0x04007EF0 RID: 32496
		public GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander, object>.State landing;

		// Token: 0x04007EF1 RID: 32497
		public GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander, object>.State crashed;
	}
}
