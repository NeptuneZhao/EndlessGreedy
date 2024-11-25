using System;
using Klei;
using UnityEngine;

// Token: 0x02000692 RID: 1682
public class Chlorinator : GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>
{
	// Token: 0x06002A01 RID: 10753 RVA: 0x000ECA24 File Offset: 0x000EAC24
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.TagTransition(GameTags.Operational, this.ready, false);
		this.ready.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle);
		this.ready.idle.EventTransition(GameHashes.OnStorageChange, this.ready.wait, (Chlorinator.StatesInstance smi) => smi.CanEmit()).EnterTransition(this.ready.wait, (Chlorinator.StatesInstance smi) => smi.CanEmit()).Target(this.hopper).PlayAnim("hopper_idle_loop");
		this.ready.wait.ScheduleGoTo(new Func<Chlorinator.StatesInstance, float>(Chlorinator.GetPoppingDelay), this.ready.popPre).EnterTransition(this.ready.idle, (Chlorinator.StatesInstance smi) => !smi.CanEmit()).Target(this.hopper).PlayAnim("hopper_idle_loop");
		this.ready.popPre.Target(this.hopper).PlayAnim("meter_hopper_pre").OnAnimQueueComplete(this.ready.pop);
		this.ready.pop.Enter(delegate(Chlorinator.StatesInstance smi)
		{
			smi.TryEmit();
		}).Target(this.hopper).PlayAnim("meter_hopper_loop").OnAnimQueueComplete(this.ready.popPst);
		this.ready.popPst.Target(this.hopper).PlayAnim("meter_hopper_pst").OnAnimQueueComplete(this.ready.wait);
	}

	// Token: 0x06002A02 RID: 10754 RVA: 0x000ECC20 File Offset: 0x000EAE20
	public static float GetPoppingDelay(Chlorinator.StatesInstance smi)
	{
		return smi.def.popWaitRange.Get();
	}

	// Token: 0x04001832 RID: 6194
	private GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State inoperational;

	// Token: 0x04001833 RID: 6195
	private Chlorinator.ReadyStates ready;

	// Token: 0x04001834 RID: 6196
	public StateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.TargetParameter hopper;

	// Token: 0x0200147E RID: 5246
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040069E5 RID: 27109
		public MathUtil.MinMax popWaitRange = new MathUtil.MinMax(0.2f, 0.8f);

		// Token: 0x040069E6 RID: 27110
		public Tag primaryOreTag;

		// Token: 0x040069E7 RID: 27111
		public float primaryOreMassPerOre;

		// Token: 0x040069E8 RID: 27112
		public MathUtil.MinMaxInt primaryOreCount = new MathUtil.MinMaxInt(1, 1);

		// Token: 0x040069E9 RID: 27113
		public Tag secondaryOreTag;

		// Token: 0x040069EA RID: 27114
		public float secondaryOreMassPerOre;

		// Token: 0x040069EB RID: 27115
		public MathUtil.MinMaxInt secondaryOreCount = new MathUtil.MinMaxInt(1, 1);

		// Token: 0x040069EC RID: 27116
		public Vector3 offset = Vector3.zero;

		// Token: 0x040069ED RID: 27117
		public MathUtil.MinMax initialVelocity = new MathUtil.MinMax(1f, 3f);

		// Token: 0x040069EE RID: 27118
		public MathUtil.MinMax initialDirectionHalfAngleDegreesRange = new MathUtil.MinMax(160f, 20f);
	}

	// Token: 0x0200147F RID: 5247
	public class ReadyStates : GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State
	{
		// Token: 0x040069EF RID: 27119
		public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State idle;

		// Token: 0x040069F0 RID: 27120
		public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State wait;

		// Token: 0x040069F1 RID: 27121
		public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State popPre;

		// Token: 0x040069F2 RID: 27122
		public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State pop;

		// Token: 0x040069F3 RID: 27123
		public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State popPst;
	}

	// Token: 0x02001480 RID: 5248
	public class StatesInstance : GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.GameInstance
	{
		// Token: 0x06008AD2 RID: 35538 RVA: 0x00334DF0 File Offset: 0x00332FF0
		public StatesInstance(IStateMachineTarget master, Chlorinator.Def def) : base(master, def)
		{
			this.storage = base.GetComponent<ComplexFabricator>().outStorage;
			KAnimControllerBase component = master.GetComponent<KAnimControllerBase>();
			this.hopperMeter = new MeterController(component, "meter_target", "meter_hopper_pre", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target"
			});
			base.sm.hopper.Set(this.hopperMeter.gameObject, this, false);
		}

		// Token: 0x06008AD3 RID: 35539 RVA: 0x00334E62 File Offset: 0x00333062
		public bool CanEmit()
		{
			return !this.storage.IsEmpty();
		}

		// Token: 0x06008AD4 RID: 35540 RVA: 0x00334E74 File Offset: 0x00333074
		public void TryEmit()
		{
			this.TryEmit(base.smi.def.primaryOreCount.Get(), base.def.primaryOreTag, base.def.primaryOreMassPerOre);
			this.TryEmit(base.smi.def.secondaryOreCount.Get(), base.def.secondaryOreTag, base.def.secondaryOreMassPerOre);
		}

		// Token: 0x06008AD5 RID: 35541 RVA: 0x00334EE4 File Offset: 0x003330E4
		private void TryEmit(int oreSpawnCount, Tag emitTag, float amount)
		{
			GameObject gameObject = this.storage.FindFirst(emitTag);
			if (gameObject == null)
			{
				return;
			}
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			Substance substance = component.Element.substance;
			float num;
			SimUtil.DiseaseInfo diseaseInfo;
			float temperature;
			this.storage.ConsumeAndGetDisease(emitTag, amount, out num, out diseaseInfo, out temperature);
			if (num <= 0f)
			{
				return;
			}
			float mass = num * component.MassPerUnit / (float)oreSpawnCount;
			Vector3 vector = base.smi.gameObject.transform.position;
			vector += base.def.offset;
			bool flag = UnityEngine.Random.value >= 0.5f;
			for (int i = 0; i < oreSpawnCount; i++)
			{
				float f = base.def.initialDirectionHalfAngleDegreesRange.Get() * 3.1415927f / 180f;
				Vector2 normalized = new Vector2(-Mathf.Cos(f), Mathf.Sin(f));
				if (flag)
				{
					normalized.x = -normalized.x;
				}
				flag = !flag;
				normalized = normalized.normalized;
				Vector3 v = normalized * base.def.initialVelocity.Get();
				Vector3 vector2 = vector;
				vector2 += normalized * 0.1f;
				GameObject go = substance.SpawnResource(vector2, mass, temperature, diseaseInfo.idx, diseaseInfo.count / oreSpawnCount, false, false, false);
				KFMOD.PlayOneShot(GlobalAssets.GetSound("Chlorinator_popping", false), CameraController.Instance.GetVerticallyScaledPosition(vector2, false), 1f);
				if (GameComps.Fallers.Has(go))
				{
					GameComps.Fallers.Remove(go);
				}
				GameComps.Fallers.Add(go, v);
			}
		}

		// Token: 0x040069F4 RID: 27124
		public Storage storage;

		// Token: 0x040069F5 RID: 27125
		public MeterController hopperMeter;
	}
}
