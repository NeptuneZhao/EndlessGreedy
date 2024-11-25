using System;
using UnityEngine;

// Token: 0x02000AF5 RID: 2805
public class ClusterMapTravelAnimator : GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer>
{
	// Token: 0x060053B3 RID: 21427 RVA: 0x001DFE28 File Offset: 0x001DE028
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.idle;
		this.root.OnTargetLost(this.entityTarget, null);
		this.idle.Target(this.entityTarget).Transition(this.grounded, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsGrounded), UpdateRate.SIM_200ms).Transition(this.surfaceTransitioning, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning), UpdateRate.SIM_200ms).EventHandlerTransition(GameHashes.ClusterLocationChanged, (ClusterMapTravelAnimator.StatesInstance smi) => Game.Instance, this.repositioning, new Func<ClusterMapTravelAnimator.StatesInstance, object, bool>(this.ClusterChangedAtMyLocation)).EventTransition(GameHashes.ClusterDestinationChanged, this.traveling, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling)).Target(this.masterTarget);
		this.grounded.Transition(this.surfaceTransitioning, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning), UpdateRate.SIM_200ms);
		this.surfaceTransitioning.Update(delegate(ClusterMapTravelAnimator.StatesInstance smi, float dt)
		{
			this.DoOrientToPath(smi);
		}, UpdateRate.SIM_200ms, false).Transition(this.repositioning, GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning)), UpdateRate.SIM_200ms);
		this.repositioning.Transition(this.traveling.orientToIdle, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoReposition), UpdateRate.RENDER_EVERY_TICK);
		this.traveling.DefaultState(this.traveling.orientToPath);
		this.traveling.travelIdle.Target(this.entityTarget).EventHandlerTransition(GameHashes.ClusterLocationChanged, (ClusterMapTravelAnimator.StatesInstance smi) => Game.Instance, this.repositioning, new Func<ClusterMapTravelAnimator.StatesInstance, object, bool>(this.ClusterChangedAtMyLocation)).EventTransition(GameHashes.ClusterDestinationChanged, this.traveling.orientToIdle, GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling))).EventTransition(GameHashes.ClusterDestinationChanged, this.traveling.orientToPath, GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoOrientToPath))).EventTransition(GameHashes.ClusterLocationChanged, this.traveling.move, GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoMove))).Target(this.masterTarget);
		this.traveling.orientToPath.Transition(this.traveling.travelIdle, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoOrientToPath), UpdateRate.RENDER_EVERY_TICK).Target(this.entityTarget).EventHandlerTransition(GameHashes.ClusterLocationChanged, (ClusterMapTravelAnimator.StatesInstance smi) => Game.Instance, this.repositioning, new Func<ClusterMapTravelAnimator.StatesInstance, object, bool>(this.ClusterChangedAtMyLocation)).Target(this.masterTarget);
		this.traveling.move.Transition(this.traveling.travelIdle, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoMove), UpdateRate.RENDER_EVERY_TICK);
		this.traveling.orientToIdle.Transition(this.idle, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoOrientToIdle), UpdateRate.RENDER_EVERY_TICK);
	}

	// Token: 0x060053B4 RID: 21428 RVA: 0x001E011B File Offset: 0x001DE31B
	private bool IsTraveling(ClusterMapTravelAnimator.StatesInstance smi)
	{
		return smi.entity.GetComponent<ClusterTraveler>().IsTraveling();
	}

	// Token: 0x060053B5 RID: 21429 RVA: 0x001E0130 File Offset: 0x001DE330
	private bool IsSurfaceTransitioning(ClusterMapTravelAnimator.StatesInstance smi)
	{
		Clustercraft clustercraft = smi.entity as Clustercraft;
		return clustercraft != null && (clustercraft.Status == Clustercraft.CraftStatus.Landing || clustercraft.Status == Clustercraft.CraftStatus.Launching);
	}

	// Token: 0x060053B6 RID: 21430 RVA: 0x001E0168 File Offset: 0x001DE368
	private bool IsGrounded(ClusterMapTravelAnimator.StatesInstance smi)
	{
		Clustercraft clustercraft = smi.entity as Clustercraft;
		return clustercraft != null && clustercraft.Status == Clustercraft.CraftStatus.Grounded;
	}

	// Token: 0x060053B7 RID: 21431 RVA: 0x001E0198 File Offset: 0x001DE398
	private bool DoReposition(ClusterMapTravelAnimator.StatesInstance smi)
	{
		Vector3 position = ClusterGrid.Instance.GetPosition(smi.entity);
		return smi.MoveTowards(position, Time.unscaledDeltaTime);
	}

	// Token: 0x060053B8 RID: 21432 RVA: 0x001E01C4 File Offset: 0x001DE3C4
	private bool DoMove(ClusterMapTravelAnimator.StatesInstance smi)
	{
		Vector3 position = ClusterGrid.Instance.GetPosition(smi.entity);
		return smi.MoveTowards(position, Time.unscaledDeltaTime);
	}

	// Token: 0x060053B9 RID: 21433 RVA: 0x001E01F0 File Offset: 0x001DE3F0
	private bool DoOrientToPath(ClusterMapTravelAnimator.StatesInstance smi)
	{
		float pathAngle = smi.GetComponent<ClusterMapVisualizer>().GetPathAngle();
		return smi.RotateTowards(pathAngle, Time.unscaledDeltaTime);
	}

	// Token: 0x060053BA RID: 21434 RVA: 0x001E0215 File Offset: 0x001DE415
	private bool DoOrientToIdle(ClusterMapTravelAnimator.StatesInstance smi)
	{
		return smi.keepRotationOnIdle || smi.RotateTowards(0f, Time.unscaledDeltaTime);
	}

	// Token: 0x060053BB RID: 21435 RVA: 0x001E0234 File Offset: 0x001DE434
	private bool ClusterChangedAtMyLocation(ClusterMapTravelAnimator.StatesInstance smi, object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		return clusterLocationChangedEvent.oldLocation == smi.entity.Location || clusterLocationChangedEvent.newLocation == smi.entity.Location;
	}

	// Token: 0x04003716 RID: 14102
	public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State idle;

	// Token: 0x04003717 RID: 14103
	public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State grounded;

	// Token: 0x04003718 RID: 14104
	public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State repositioning;

	// Token: 0x04003719 RID: 14105
	public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State surfaceTransitioning;

	// Token: 0x0400371A RID: 14106
	public ClusterMapTravelAnimator.TravelingStates traveling;

	// Token: 0x0400371B RID: 14107
	public StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;

	// Token: 0x02001B55 RID: 6997
	private class Tuning : TuningData<ClusterMapTravelAnimator.Tuning>
	{
		// Token: 0x04007F71 RID: 32625
		public float visualizerTransitionSpeed = 1f;

		// Token: 0x04007F72 RID: 32626
		public float visualizerRotationSpeed = 1f;
	}

	// Token: 0x02001B56 RID: 6998
	public class TravelingStates : GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State
	{
		// Token: 0x04007F73 RID: 32627
		public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State travelIdle;

		// Token: 0x04007F74 RID: 32628
		public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State orientToPath;

		// Token: 0x04007F75 RID: 32629
		public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State move;

		// Token: 0x04007F76 RID: 32630
		public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State orientToIdle;
	}

	// Token: 0x02001B57 RID: 6999
	public class StatesInstance : GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
	{
		// Token: 0x0600A336 RID: 41782 RVA: 0x0038920F File Offset: 0x0038740F
		public StatesInstance(ClusterMapVisualizer master, ClusterGridEntity entity) : base(master)
		{
			this.entity = entity;
			base.sm.entityTarget.Set(entity, this);
		}

		// Token: 0x0600A337 RID: 41783 RVA: 0x00389234 File Offset: 0x00387434
		public bool MoveTowards(Vector3 targetPosition, float dt)
		{
			RectTransform component = base.GetComponent<RectTransform>();
			ClusterMapVisualizer component2 = base.GetComponent<ClusterMapVisualizer>();
			Vector3 localPosition = component.GetLocalPosition();
			Vector3 vector = targetPosition - localPosition;
			Vector3 normalized = vector.normalized;
			float magnitude = vector.magnitude;
			float num = TuningData<ClusterMapTravelAnimator.Tuning>.Get().visualizerTransitionSpeed * dt;
			if (num < magnitude)
			{
				Vector3 b = normalized * num;
				component.SetLocalPosition(localPosition + b);
				component2.RefreshPathDrawing();
				return false;
			}
			component.SetLocalPosition(targetPosition);
			component2.RefreshPathDrawing();
			return true;
		}

		// Token: 0x0600A338 RID: 41784 RVA: 0x003892B8 File Offset: 0x003874B8
		public bool RotateTowards(float targetAngle, float dt)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			float num = targetAngle - this.simpleAngle;
			if (num > 180f)
			{
				num -= 360f;
			}
			else if (num < -180f)
			{
				num += 360f;
			}
			float num2 = TuningData<ClusterMapTravelAnimator.Tuning>.Get().visualizerRotationSpeed * dt;
			if (num > 0f && num2 < num)
			{
				this.simpleAngle += num2;
				component.SetAnimRotation(this.simpleAngle);
				return false;
			}
			if (num < 0f && -num2 > num)
			{
				this.simpleAngle -= num2;
				component.SetAnimRotation(this.simpleAngle);
				return false;
			}
			this.simpleAngle = targetAngle;
			component.SetAnimRotation(this.simpleAngle);
			return true;
		}

		// Token: 0x04007F77 RID: 32631
		public ClusterGridEntity entity;

		// Token: 0x04007F78 RID: 32632
		private float simpleAngle;

		// Token: 0x04007F79 RID: 32633
		public bool keepRotationOnIdle;
	}
}
