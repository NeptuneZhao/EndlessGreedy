using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000771 RID: 1905
public class StaterpillarGenerator : Generator
{
	// Token: 0x0600336C RID: 13164 RVA: 0x00119D04 File Offset: 0x00117F04
	protected override void OnSpawn()
	{
		Staterpillar staterpillar = this.parent.Get();
		if (staterpillar == null || staterpillar.GetGenerator() != this)
		{
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		this.smi = new StaterpillarGenerator.StatesInstance(this);
		this.smi.StartSM();
		base.OnSpawn();
	}

	// Token: 0x0600336D RID: 13165 RVA: 0x00119D60 File Offset: 0x00117F60
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		if (!this.operational.IsOperational)
		{
			return;
		}
		float num = base.GetComponent<Generator>().WattageRating;
		if (num > 0f)
		{
			num *= dt;
			num = Mathf.Max(num, 1f * dt);
			base.GenerateJoules(num, false);
		}
	}

	// Token: 0x04001E62 RID: 7778
	private StaterpillarGenerator.StatesInstance smi;

	// Token: 0x04001E63 RID: 7779
	[Serialize]
	public Ref<Staterpillar> parent = new Ref<Staterpillar>();

	// Token: 0x0200160B RID: 5643
	public class StatesInstance : GameStateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator, object>.GameInstance
	{
		// Token: 0x060090AE RID: 37038 RVA: 0x0034CA0B File Offset: 0x0034AC0B
		public StatesInstance(StaterpillarGenerator master) : base(master)
		{
		}

		// Token: 0x04006E76 RID: 28278
		private Attributes attributes;
	}

	// Token: 0x0200160C RID: 5644
	public class States : GameStateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator>
	{
		// Token: 0x060090AF RID: 37039 RVA: 0x0034CA14 File Offset: 0x0034AC14
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.EventTransition(GameHashes.OperationalChanged, this.idle, (StaterpillarGenerator.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.idle.EventTransition(GameHashes.OperationalChanged, this.root, (StaterpillarGenerator.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).Enter(delegate(StaterpillarGenerator.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
			});
		}

		// Token: 0x04006E77 RID: 28279
		public GameStateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator, object>.State idle;
	}
}
