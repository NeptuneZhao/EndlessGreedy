﻿using System;
using KSerialization;
using UnityEngine;

// Token: 0x020008EB RID: 2283
[SerializationConfig(MemberSerialization.OptIn)]
public class HighEnergyParticle : StateMachineComponent<HighEnergyParticle.StatesInstance>
{
	// Token: 0x06004193 RID: 16787 RVA: 0x001741F7 File Offset: 0x001723F7
	protected override void OnPrefabInit()
	{
		this.loopingSounds = base.gameObject.GetComponent<LoopingSounds>();
		this.flyingSound = GlobalAssets.GetSound("Radbolt_travel_LP", false);
		base.OnPrefabInit();
	}

	// Token: 0x06004194 RID: 16788 RVA: 0x00174224 File Offset: 0x00172424
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.HighEnergyParticles.Add(this);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.HighEnergyParticleCount, base.gameObject);
		this.emitter.SetEmitting(false);
		this.emitter.Refresh();
		this.SetDirection(this.direction);
		base.gameObject.layer = LayerMask.NameToLayer("PlaceWithDepth");
		this.StartLoopingSound();
		base.smi.StartSM();
	}

	// Token: 0x06004195 RID: 16789 RVA: 0x001742AC File Offset: 0x001724AC
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.StopLoopingSound();
		Components.HighEnergyParticles.Remove(this);
		if (this.capturedBy != null && this.capturedBy.currentParticle == this)
		{
			this.capturedBy.currentParticle = null;
		}
	}

	// Token: 0x06004196 RID: 16790 RVA: 0x00174300 File Offset: 0x00172500
	public void SetDirection(EightDirection direction)
	{
		this.direction = direction;
		float angle = EightDirectionUtil.GetAngle(direction);
		base.smi.master.transform.rotation = Quaternion.Euler(0f, 0f, angle);
	}

	// Token: 0x06004197 RID: 16791 RVA: 0x00174340 File Offset: 0x00172540
	public void Collide(HighEnergyParticle.CollisionType collisionType)
	{
		this.collision = collisionType;
		GameObject gameObject = new GameObject("HEPcollideFX");
		gameObject.SetActive(false);
		gameObject.transform.SetPosition(Grid.CellToPosCCC(Grid.PosToCell(base.smi.master.transform.position), Grid.SceneLayer.FXFront));
		KBatchedAnimController fxAnim = gameObject.AddComponent<KBatchedAnimController>();
		fxAnim.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("hep_impact_kanim")
		};
		fxAnim.initialAnim = "graze";
		gameObject.SetActive(true);
		switch (collisionType)
		{
		case HighEnergyParticle.CollisionType.Captured:
			fxAnim.Play("full", KAnim.PlayMode.Once, 1f, 0f);
			break;
		case HighEnergyParticle.CollisionType.CaptureAndRelease:
			fxAnim.Play("partial", KAnim.PlayMode.Once, 1f, 0f);
			break;
		case HighEnergyParticle.CollisionType.PassThrough:
			fxAnim.Play("graze", KAnim.PlayMode.Once, 1f, 0f);
			break;
		}
		fxAnim.onAnimComplete += delegate(HashedString arg)
		{
			Util.KDestroyGameObject(fxAnim);
		};
		if (collisionType == HighEnergyParticle.CollisionType.PassThrough)
		{
			this.collision = HighEnergyParticle.CollisionType.None;
			return;
		}
		base.smi.sm.destroySignal.Trigger(base.smi);
	}

	// Token: 0x06004198 RID: 16792 RVA: 0x0017449B File Offset: 0x0017269B
	public void DestroyNow()
	{
		base.smi.sm.destroySimpleSignal.Trigger(base.smi);
	}

	// Token: 0x06004199 RID: 16793 RVA: 0x001744B8 File Offset: 0x001726B8
	private void Capture(HighEnergyParticlePort input)
	{
		if (input.currentParticle != null)
		{
			DebugUtil.LogArgs(new object[]
			{
				"Particle was backed up and caused an explosion!"
			});
			base.smi.sm.destroySignal.Trigger(base.smi);
			return;
		}
		this.capturedBy = input;
		input.currentParticle = this;
		input.Capture(this);
		if (input.currentParticle == this)
		{
			input.currentParticle = null;
			this.capturedBy = null;
			this.Collide(HighEnergyParticle.CollisionType.Captured);
			return;
		}
		this.capturedBy = null;
		this.Collide(HighEnergyParticle.CollisionType.CaptureAndRelease);
	}

	// Token: 0x0600419A RID: 16794 RVA: 0x00174549 File Offset: 0x00172749
	public void Uncapture()
	{
		if (this.capturedBy != null)
		{
			this.capturedBy.currentParticle = null;
		}
		this.capturedBy = null;
	}

	// Token: 0x0600419B RID: 16795 RVA: 0x0017456C File Offset: 0x0017276C
	public void CheckCollision()
	{
		if (this.collision != HighEnergyParticle.CollisionType.None)
		{
			return;
		}
		int cell = Grid.PosToCell(base.smi.master.transform.GetPosition());
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			gameObject.GetComponent<Operational>();
			HighEnergyParticlePort component = gameObject.GetComponent<HighEnergyParticlePort>();
			if (component != null)
			{
				Vector2 pos = Grid.CellToPosCCC(component.GetHighEnergyParticleInputPortPosition(), Grid.SceneLayer.NoLayer);
				if (base.GetComponent<KCircleCollider2D>().Intersects(pos))
				{
					if (component.InputActive() && component.AllowCapture(this))
					{
						this.Capture(component);
						return;
					}
					this.Collide(HighEnergyParticle.CollisionType.PassThrough);
				}
			}
		}
		KCircleCollider2D component2 = base.GetComponent<KCircleCollider2D>();
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		ListPool<ScenePartitionerEntry, HighEnergyParticle>.PooledList pooledList = ListPool<ScenePartitionerEntry, HighEnergyParticle>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(num - 1, num2 - 1, 3, 3, GameScenePartitioner.Instance.collisionLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			KCollider2D kcollider2D = scenePartitionerEntry.obj as KCollider2D;
			HighEnergyParticle component3 = kcollider2D.gameObject.GetComponent<HighEnergyParticle>();
			if (!(component3 == null) && !(component3 == this) && component3.isCollideable)
			{
				bool flag = component2.Intersects(component3.transform.position);
				bool flag2 = kcollider2D.Intersects(base.transform.position);
				if (flag && flag2)
				{
					this.payload += component3.payload;
					component3.DestroyNow();
					this.Collide(HighEnergyParticle.CollisionType.HighEnergyParticle);
					return;
				}
			}
		}
		pooledList.Recycle();
		GameObject gameObject2 = Grid.Objects[cell, 3];
		if (gameObject2 != null)
		{
			ObjectLayerListItem objectLayerListItem = gameObject2.GetComponent<Pickupable>().objectLayerListItem;
			while (objectLayerListItem != null)
			{
				GameObject gameObject3 = objectLayerListItem.gameObject;
				objectLayerListItem = objectLayerListItem.nextItem;
				if (!(gameObject3 == null))
				{
					KPrefabID component4 = gameObject3.GetComponent<KPrefabID>();
					Health component5 = gameObject2.GetComponent<Health>();
					if (component5 != null && component4 != null && component4.HasTag(GameTags.Creature) && !component5.IsDefeated())
					{
						component5.Damage(20f);
						this.Collide(HighEnergyParticle.CollisionType.Creature);
						return;
					}
				}
			}
		}
		GameObject gameObject4 = Grid.Objects[cell, 0];
		if (gameObject4 != null)
		{
			Health component6 = gameObject4.GetComponent<Health>();
			if (component6 != null && !component6.IsDefeated() && !gameObject4.HasTag(GameTags.Dead) && !gameObject4.HasTag(GameTags.Dying))
			{
				component6.Damage(20f);
				WoundMonitor.Instance smi = gameObject4.GetSMI<WoundMonitor.Instance>();
				if (smi != null && !component6.IsDefeated())
				{
					smi.PlayKnockedOverImpactAnimation();
				}
				gameObject4.GetComponent<PrimaryElement>().AddDisease(Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id), Mathf.FloorToInt(this.payload * 0.5f / 0.01f), "HEPImpact");
				this.Collide(HighEnergyParticle.CollisionType.Minion);
				return;
			}
		}
		if (Grid.IsSolidCell(cell))
		{
			GameObject gameObject5 = Grid.Objects[cell, 9];
			if (gameObject5 == null || !gameObject5.HasTag(GameTags.HEPPassThrough) || this.capturedBy == null || this.capturedBy.gameObject != gameObject5)
			{
				this.Collide(HighEnergyParticle.CollisionType.Solid);
			}
			return;
		}
	}

	// Token: 0x0600419C RID: 16796 RVA: 0x00174900 File Offset: 0x00172B00
	public void MovingUpdate(float dt)
	{
		if (this.collision != HighEnergyParticle.CollisionType.None)
		{
			return;
		}
		Vector3 position = base.transform.GetPosition();
		int num = Grid.PosToCell(position);
		Vector3 vector = position + EightDirectionUtil.GetNormal(this.direction) * this.speed * dt;
		int num2 = Grid.PosToCell(vector);
		SaveGame.Instance.ColonyAchievementTracker.radBoltTravelDistance += this.speed * dt;
		this.loopingSounds.UpdateVelocity(this.flyingSound, vector - position);
		if (!Grid.IsValidCell(num2))
		{
			base.smi.sm.destroySimpleSignal.Trigger(base.smi);
			return;
		}
		if (num != num2)
		{
			this.payload -= 0.1f;
			byte index = Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id);
			int disease_delta = Mathf.FloorToInt(5f);
			if (!Grid.Element[num2].IsVacuum)
			{
				SimMessages.ModifyDiseaseOnCell(num2, index, disease_delta);
			}
		}
		if (this.payload <= 0f)
		{
			base.smi.sm.destroySimpleSignal.Trigger(base.smi);
		}
		base.transform.SetPosition(vector);
	}

	// Token: 0x0600419D RID: 16797 RVA: 0x00174A4B File Offset: 0x00172C4B
	private void StartLoopingSound()
	{
		this.loopingSounds.StartSound(this.flyingSound);
	}

	// Token: 0x0600419E RID: 16798 RVA: 0x00174A5F File Offset: 0x00172C5F
	private void StopLoopingSound()
	{
		this.loopingSounds.StopSound(this.flyingSound);
	}

	// Token: 0x04002B6B RID: 11115
	[Serialize]
	private EightDirection direction;

	// Token: 0x04002B6C RID: 11116
	[Serialize]
	public float speed;

	// Token: 0x04002B6D RID: 11117
	[Serialize]
	public float payload;

	// Token: 0x04002B6E RID: 11118
	[MyCmpReq]
	private RadiationEmitter emitter;

	// Token: 0x04002B6F RID: 11119
	[Serialize]
	public float perCellFalloff;

	// Token: 0x04002B70 RID: 11120
	[Serialize]
	public HighEnergyParticle.CollisionType collision;

	// Token: 0x04002B71 RID: 11121
	[Serialize]
	public HighEnergyParticlePort capturedBy;

	// Token: 0x04002B72 RID: 11122
	public short emitRadius;

	// Token: 0x04002B73 RID: 11123
	public float emitRate;

	// Token: 0x04002B74 RID: 11124
	public float emitSpeed;

	// Token: 0x04002B75 RID: 11125
	private LoopingSounds loopingSounds;

	// Token: 0x04002B76 RID: 11126
	public string flyingSound;

	// Token: 0x04002B77 RID: 11127
	public bool isCollideable;

	// Token: 0x02001855 RID: 6229
	public enum CollisionType
	{
		// Token: 0x0400759B RID: 30107
		None,
		// Token: 0x0400759C RID: 30108
		Solid,
		// Token: 0x0400759D RID: 30109
		Creature,
		// Token: 0x0400759E RID: 30110
		Minion,
		// Token: 0x0400759F RID: 30111
		Captured,
		// Token: 0x040075A0 RID: 30112
		HighEnergyParticle,
		// Token: 0x040075A1 RID: 30113
		CaptureAndRelease,
		// Token: 0x040075A2 RID: 30114
		PassThrough
	}

	// Token: 0x02001856 RID: 6230
	public class StatesInstance : GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.GameInstance
	{
		// Token: 0x060097F7 RID: 38903 RVA: 0x003665C3 File Offset: 0x003647C3
		public StatesInstance(HighEnergyParticle smi) : base(smi)
		{
		}
	}

	// Token: 0x02001857 RID: 6231
	public class States : GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle>
	{
		// Token: 0x060097F8 RID: 38904 RVA: 0x003665CC File Offset: 0x003647CC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.ready.pre;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.ready.OnSignal(this.destroySimpleSignal, this.destroying.instant).OnSignal(this.destroySignal, this.destroying.explode, (HighEnergyParticle.StatesInstance smi) => smi.master.collision == HighEnergyParticle.CollisionType.Creature).OnSignal(this.destroySignal, this.destroying.explode, (HighEnergyParticle.StatesInstance smi) => smi.master.collision == HighEnergyParticle.CollisionType.Minion).OnSignal(this.destroySignal, this.destroying.explode, (HighEnergyParticle.StatesInstance smi) => smi.master.collision == HighEnergyParticle.CollisionType.Solid).OnSignal(this.destroySignal, this.destroying.blackhole, (HighEnergyParticle.StatesInstance smi) => smi.master.collision == HighEnergyParticle.CollisionType.HighEnergyParticle).OnSignal(this.destroySignal, this.destroying.captured, (HighEnergyParticle.StatesInstance smi) => smi.master.collision == HighEnergyParticle.CollisionType.Captured).OnSignal(this.destroySignal, this.catchAndRelease, (HighEnergyParticle.StatesInstance smi) => smi.master.collision == HighEnergyParticle.CollisionType.CaptureAndRelease).Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				smi.master.emitter.SetEmitting(true);
				smi.master.isCollideable = true;
			}).Update(delegate(HighEnergyParticle.StatesInstance smi, float dt)
			{
				smi.master.MovingUpdate(dt);
				smi.master.CheckCollision();
			}, UpdateRate.SIM_EVERY_TICK, false);
			this.ready.pre.PlayAnim("travel_pre").OnAnimQueueComplete(this.ready.moving);
			this.ready.moving.PlayAnim("travel_loop", KAnim.PlayMode.Loop);
			this.catchAndRelease.Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				smi.master.collision = HighEnergyParticle.CollisionType.None;
			}).PlayAnim("explode", KAnim.PlayMode.Once).OnAnimQueueComplete(this.ready.pre);
			this.destroying.Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				smi.master.isCollideable = false;
				smi.master.StopLoopingSound();
			});
			this.destroying.instant.Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				UnityEngine.Object.Destroy(smi.master.gameObject);
			});
			this.destroying.explode.PlayAnim("explode").Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				this.EmitRemainingPayload(smi);
			});
			this.destroying.blackhole.PlayAnim("collision").Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				this.EmitRemainingPayload(smi);
			});
			this.destroying.captured.PlayAnim("travel_pst").OnAnimQueueComplete(this.destroying.instant).Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				smi.master.emitter.SetEmitting(false);
			});
		}

		// Token: 0x060097F9 RID: 38905 RVA: 0x00366904 File Offset: 0x00364B04
		private void EmitRemainingPayload(HighEnergyParticle.StatesInstance smi)
		{
			smi.master.GetComponent<KBatchedAnimController>().GetCurrentAnim();
			smi.master.emitter.emitRadiusX = 6;
			smi.master.emitter.emitRadiusY = 6;
			smi.master.emitter.emitRads = smi.master.payload * 0.5f * 600f / 9f;
			smi.master.emitter.Refresh();
			SimMessages.AddRemoveSubstance(Grid.PosToCell(smi.master.gameObject), SimHashes.Fallout, CellEventLogger.Instance.ElementEmitted, smi.master.payload * 0.001f, 5000f, Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id), Mathf.FloorToInt(smi.master.payload * 0.5f / 0.01f), true, -1);
			smi.Schedule(1f, delegate(object obj)
			{
				UnityEngine.Object.Destroy(smi.master.gameObject);
			}, null);
		}

		// Token: 0x040075A3 RID: 30115
		public HighEnergyParticle.States.ReadyStates ready;

		// Token: 0x040075A4 RID: 30116
		public HighEnergyParticle.States.DestructionStates destroying;

		// Token: 0x040075A5 RID: 30117
		public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State catchAndRelease;

		// Token: 0x040075A6 RID: 30118
		public StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.Signal destroySignal;

		// Token: 0x040075A7 RID: 30119
		public StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.Signal destroySimpleSignal;

		// Token: 0x020025A3 RID: 9635
		public class ReadyStates : GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State
		{
			// Token: 0x0400A79E RID: 42910
			public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State pre;

			// Token: 0x0400A79F RID: 42911
			public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State moving;
		}

		// Token: 0x020025A4 RID: 9636
		public class DestructionStates : GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State
		{
			// Token: 0x0400A7A0 RID: 42912
			public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State instant;

			// Token: 0x0400A7A1 RID: 42913
			public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State explode;

			// Token: 0x0400A7A2 RID: 42914
			public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State captured;

			// Token: 0x0400A7A3 RID: 42915
			public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State blackhole;
		}
	}
}
