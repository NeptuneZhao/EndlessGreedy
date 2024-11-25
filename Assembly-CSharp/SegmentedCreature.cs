using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F2 RID: 242
public class SegmentedCreature : GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>
{
	// Token: 0x06000465 RID: 1125 RVA: 0x00023278 File Offset: 0x00021478
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.freeMovement.idle;
		this.root.Enter(new StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State.Callback(this.SetRetractedPath));
		this.retracted.DefaultState(this.retracted.pre).Enter(delegate(SegmentedCreature.Instance smi)
		{
			this.PlayBodySegmentsAnim(smi, "idle_loop", KAnim.PlayMode.Loop, false, 0);
		}).Exit(new StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State.Callback(this.SetRetractedPath));
		this.retracted.pre.Update(new Action<SegmentedCreature.Instance, float>(this.UpdateRetractedPre), UpdateRate.SIM_EVERY_TICK, false);
		this.retracted.loop.ParamTransition<bool>(this.isRetracted, this.freeMovement, (SegmentedCreature.Instance smi, bool p) => !this.isRetracted.Get(smi)).Update(new Action<SegmentedCreature.Instance, float>(this.UpdateRetractedLoop), UpdateRate.SIM_EVERY_TICK, false);
		this.freeMovement.DefaultState(this.freeMovement.idle).ParamTransition<bool>(this.isRetracted, this.retracted, (SegmentedCreature.Instance smi, bool p) => this.isRetracted.Get(smi)).Update(new Action<SegmentedCreature.Instance, float>(this.UpdateFreeMovement), UpdateRate.SIM_EVERY_TICK, false);
		this.freeMovement.idle.Transition(this.freeMovement.moving, (SegmentedCreature.Instance smi) => smi.GetComponent<Navigator>().IsMoving(), UpdateRate.SIM_200ms).Enter(delegate(SegmentedCreature.Instance smi)
		{
			this.PlayBodySegmentsAnim(smi, "idle_loop", KAnim.PlayMode.Loop, true, 0);
		});
		this.freeMovement.moving.Transition(this.freeMovement.idle, (SegmentedCreature.Instance smi) => !smi.GetComponent<Navigator>().IsMoving(), UpdateRate.SIM_200ms).Enter(delegate(SegmentedCreature.Instance smi)
		{
			this.PlayBodySegmentsAnim(smi, "walking_pre", KAnim.PlayMode.Once, false, 0);
			this.PlayBodySegmentsAnim(smi, "walking_loop", KAnim.PlayMode.Loop, false, smi.def.animFrameOffset);
		}).Exit(delegate(SegmentedCreature.Instance smi)
		{
			this.PlayBodySegmentsAnim(smi, "walking_pst", KAnim.PlayMode.Once, true, 0);
		});
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x00023430 File Offset: 0x00021630
	private void PlayBodySegmentsAnim(SegmentedCreature.Instance smi, string animName, KAnim.PlayMode playMode, bool queue = false, int frameOffset = 0)
	{
		LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.GetFirstBodySegmentNode();
		int num = 0;
		while (linkedListNode != null)
		{
			if (queue)
			{
				linkedListNode.Value.animController.Queue(animName, playMode, 1f, 0f);
			}
			else
			{
				linkedListNode.Value.animController.Play(animName, playMode, 1f, 0f);
			}
			if (frameOffset > 0)
			{
				float num2 = (float)linkedListNode.Value.animController.GetCurrentNumFrames();
				float elapsedTime = (float)num * ((float)frameOffset / num2);
				linkedListNode.Value.animController.SetElapsedTime(elapsedTime);
			}
			num++;
			linkedListNode = linkedListNode.Next;
		}
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x000234D8 File Offset: 0x000216D8
	private void UpdateRetractedPre(SegmentedCreature.Instance smi, float dt)
	{
		if (this.UpdateHeadPosition(smi) == 0f)
		{
			return;
		}
		bool flag = true;
		for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			linkedListNode.Value.distanceToPreviousSegment = Mathf.Max(smi.def.minSegmentSpacing, linkedListNode.Value.distanceToPreviousSegment - dt * smi.def.retractionSegmentSpeed);
			if (linkedListNode.Value.distanceToPreviousSegment > smi.def.minSegmentSpacing)
			{
				flag = false;
			}
		}
		SegmentedCreature.CreatureSegment value = smi.GetHeadSegmentNode().Value;
		LinkedListNode<SegmentedCreature.PathNode> linkedListNode2 = smi.path.First;
		Vector3 forward = value.Forward;
		Quaternion rotation = value.Rotation;
		int num = 0;
		while (linkedListNode2 != null)
		{
			Vector3 b = value.Position - smi.def.pathSpacing * (float)num * forward;
			linkedListNode2.Value.position = Vector3.Lerp(linkedListNode2.Value.position, b, dt * smi.def.retractionPathSpeed);
			linkedListNode2.Value.rotation = Quaternion.Slerp(linkedListNode2.Value.rotation, rotation, dt * smi.def.retractionPathSpeed);
			num++;
			linkedListNode2 = linkedListNode2.Next;
		}
		this.UpdateBodyPosition(smi);
		if (flag)
		{
			smi.GoTo(this.retracted.loop);
		}
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x0002362C File Offset: 0x0002182C
	private void UpdateRetractedLoop(SegmentedCreature.Instance smi, float dt)
	{
		if (this.UpdateHeadPosition(smi) != 0f)
		{
			this.SetRetractedPath(smi);
			this.UpdateBodyPosition(smi);
		}
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x0002364C File Offset: 0x0002184C
	private void SetRetractedPath(SegmentedCreature.Instance smi)
	{
		SegmentedCreature.CreatureSegment value = smi.GetHeadSegmentNode().Value;
		LinkedListNode<SegmentedCreature.PathNode> linkedListNode = smi.path.First;
		Vector3 position = value.Position;
		Quaternion rotation = value.Rotation;
		Vector3 forward = value.Forward;
		int num = 0;
		while (linkedListNode != null)
		{
			linkedListNode.Value.position = position - smi.def.pathSpacing * (float)num * forward;
			linkedListNode.Value.rotation = rotation;
			num++;
			linkedListNode = linkedListNode.Next;
		}
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x000236CC File Offset: 0x000218CC
	private void UpdateFreeMovement(SegmentedCreature.Instance smi, float dt)
	{
		float num = this.UpdateHeadPosition(smi);
		if (num != 0f)
		{
			this.AdjustBodySegmentsSpacing(smi, num);
			this.UpdateBodyPosition(smi);
		}
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x000236F8 File Offset: 0x000218F8
	private float UpdateHeadPosition(SegmentedCreature.Instance smi)
	{
		SegmentedCreature.CreatureSegment value = smi.GetHeadSegmentNode().Value;
		if (value.Position == smi.previousHeadPosition)
		{
			return 0f;
		}
		SegmentedCreature.PathNode value2 = smi.path.First.Value;
		SegmentedCreature.PathNode value3 = smi.path.First.Next.Value;
		float magnitude = (value2.position - value3.position).magnitude;
		float magnitude2 = (value.Position - value3.position).magnitude;
		float result = magnitude2 - magnitude;
		value2.position = value.Position;
		value2.rotation = value.Rotation;
		smi.previousHeadPosition = value2.position;
		Vector3 normalized = (value2.position - value3.position).normalized;
		int num = Mathf.FloorToInt(magnitude2 / smi.def.pathSpacing);
		for (int i = 0; i < num; i++)
		{
			Vector3 position = value3.position + normalized * smi.def.pathSpacing;
			LinkedListNode<SegmentedCreature.PathNode> last = smi.path.Last;
			last.Value.position = position;
			last.Value.rotation = value2.rotation;
			float num2 = magnitude2 - (float)i * smi.def.pathSpacing;
			float t = num2 - smi.def.pathSpacing / num2;
			last.Value.rotation = Quaternion.Lerp(value2.rotation, value3.rotation, t);
			smi.path.RemoveLast();
			smi.path.AddAfter(smi.path.First, last);
			value3 = last.Value;
		}
		return result;
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x000238BC File Offset: 0x00021ABC
	private void AdjustBodySegmentsSpacing(SegmentedCreature.Instance smi, float spacing)
	{
		for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			linkedListNode.Value.distanceToPreviousSegment += spacing;
			if (linkedListNode.Value.distanceToPreviousSegment < smi.def.minSegmentSpacing)
			{
				spacing = linkedListNode.Value.distanceToPreviousSegment - smi.def.minSegmentSpacing;
				linkedListNode.Value.distanceToPreviousSegment = smi.def.minSegmentSpacing;
			}
			else
			{
				if (linkedListNode.Value.distanceToPreviousSegment <= smi.def.maxSegmentSpacing)
				{
					break;
				}
				spacing = linkedListNode.Value.distanceToPreviousSegment - smi.def.maxSegmentSpacing;
				linkedListNode.Value.distanceToPreviousSegment = smi.def.maxSegmentSpacing;
			}
		}
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x00023988 File Offset: 0x00021B88
	private void UpdateBodyPosition(SegmentedCreature.Instance smi)
	{
		LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.GetFirstBodySegmentNode();
		LinkedListNode<SegmentedCreature.PathNode> linkedListNode2 = smi.path.First;
		float num = 0f;
		float num2 = smi.LengthPercentage();
		int num3 = 0;
		while (linkedListNode != null)
		{
			float num4 = linkedListNode.Value.distanceToPreviousSegment;
			float num5 = 0f;
			while (linkedListNode2.Next != null)
			{
				num5 = (linkedListNode2.Value.position - linkedListNode2.Next.Value.position).magnitude - num;
				if (num4 < num5)
				{
					break;
				}
				num4 -= num5;
				num = 0f;
				linkedListNode2 = linkedListNode2.Next;
			}
			if (linkedListNode2.Next == null)
			{
				linkedListNode.Value.SetPosition(linkedListNode2.Value.position);
				linkedListNode.Value.SetRotation(smi.path.Last.Value.rotation);
			}
			else
			{
				SegmentedCreature.PathNode value = linkedListNode2.Value;
				SegmentedCreature.PathNode value2 = linkedListNode2.Next.Value;
				linkedListNode.Value.SetPosition(linkedListNode2.Value.position + (linkedListNode2.Next.Value.position - linkedListNode2.Value.position).normalized * num4);
				linkedListNode.Value.SetRotation(Quaternion.Slerp(value.rotation, value2.rotation, num4 / num5));
				num = num4;
			}
			linkedListNode.Value.animController.FlipX = (linkedListNode.Previous.Value.Position.x < linkedListNode.Value.Position.x);
			linkedListNode.Value.animController.animScale = smi.baseAnimScale + smi.baseAnimScale * smi.def.compressedMaxScale * ((float)(smi.def.numBodySegments - num3) / (float)smi.def.numBodySegments) * (1f - num2);
			linkedListNode = linkedListNode.Next;
			num3++;
		}
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x00023B84 File Offset: 0x00021D84
	private void DrawDebug(SegmentedCreature.Instance smi, float dt)
	{
		SegmentedCreature.CreatureSegment value = smi.GetHeadSegmentNode().Value;
		DrawUtil.Arrow(value.Position, value.Position + value.Up, 0.05f, Color.red, 0f);
		DrawUtil.Arrow(value.Position, value.Position + value.Forward * 0.06f, 0.05f, Color.cyan, 0f);
		int num = 0;
		foreach (SegmentedCreature.PathNode pathNode in smi.path)
		{
			Color color = Color.HSVToRGB((float)num / (float)smi.def.numPathNodes, 1f, 1f);
			DrawUtil.Gnomon(pathNode.position, 0.05f, Color.cyan, 0f);
			DrawUtil.Arrow(pathNode.position, pathNode.position + pathNode.rotation * Vector3.up * 0.5f, 0.025f, color, 0f);
			num++;
		}
		for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.segments.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			DrawUtil.Circle(linkedListNode.Value.Position, 0.05f, Color.white, new Vector3?(Vector3.forward), 0f);
			DrawUtil.Gnomon(linkedListNode.Value.Position, 0.05f, Color.white, 0f);
		}
	}

	// Token: 0x040002F9 RID: 761
	public SegmentedCreature.RectractStates retracted;

	// Token: 0x040002FA RID: 762
	public SegmentedCreature.FreeMovementStates freeMovement;

	// Token: 0x040002FB RID: 763
	private StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.BoolParameter isRetracted;

	// Token: 0x0200109D RID: 4253
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005D3F RID: 23871
		public HashedString segmentTrackerSymbol;

		// Token: 0x04005D40 RID: 23872
		public Vector3 headOffset = Vector3.zero;

		// Token: 0x04005D41 RID: 23873
		public Vector3 bodyPivot = Vector3.zero;

		// Token: 0x04005D42 RID: 23874
		public Vector3 tailPivot = Vector3.zero;

		// Token: 0x04005D43 RID: 23875
		public int numBodySegments;

		// Token: 0x04005D44 RID: 23876
		public float minSegmentSpacing;

		// Token: 0x04005D45 RID: 23877
		public float maxSegmentSpacing;

		// Token: 0x04005D46 RID: 23878
		public int numPathNodes;

		// Token: 0x04005D47 RID: 23879
		public float pathSpacing;

		// Token: 0x04005D48 RID: 23880
		public KAnimFile midAnim;

		// Token: 0x04005D49 RID: 23881
		public KAnimFile tailAnim;

		// Token: 0x04005D4A RID: 23882
		public string movingAnimName;

		// Token: 0x04005D4B RID: 23883
		public string idleAnimName;

		// Token: 0x04005D4C RID: 23884
		public float retractionSegmentSpeed = 1f;

		// Token: 0x04005D4D RID: 23885
		public float retractionPathSpeed = 1f;

		// Token: 0x04005D4E RID: 23886
		public float compressedMaxScale = 1.2f;

		// Token: 0x04005D4F RID: 23887
		public int animFrameOffset;

		// Token: 0x04005D50 RID: 23888
		public HashSet<HashedString> hideBoddyWhenStartingAnimNames = new HashSet<HashedString>
		{
			"rocket_biological"
		};

		// Token: 0x04005D51 RID: 23889
		public HashSet<HashedString> retractWhenStartingAnimNames = new HashSet<HashedString>
		{
			"trapped",
			"trussed",
			"escape",
			"drown_pre",
			"drown_loop",
			"drown_pst",
			"rocket_biological"
		};

		// Token: 0x04005D52 RID: 23890
		public HashSet<HashedString> retractWhenEndingAnimNames = new HashSet<HashedString>
		{
			"floor_floor_2_0",
			"grooming_pst",
			"fall"
		};
	}

	// Token: 0x0200109E RID: 4254
	public class RectractStates : GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State
	{
		// Token: 0x04005D53 RID: 23891
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State pre;

		// Token: 0x04005D54 RID: 23892
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State loop;
	}

	// Token: 0x0200109F RID: 4255
	public class FreeMovementStates : GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State
	{
		// Token: 0x04005D55 RID: 23893
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State idle;

		// Token: 0x04005D56 RID: 23894
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State moving;

		// Token: 0x04005D57 RID: 23895
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State layEgg;

		// Token: 0x04005D58 RID: 23896
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State poop;

		// Token: 0x04005D59 RID: 23897
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State dead;
	}

	// Token: 0x020010A0 RID: 4256
	public new class Instance : GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.GameInstance
	{
		// Token: 0x06007C77 RID: 31863 RVA: 0x00305980 File Offset: 0x00303B80
		public Instance(IStateMachineTarget master, SegmentedCreature.Def def) : base(master, def)
		{
			global::Debug.Assert((float)def.numBodySegments * def.maxSegmentSpacing < (float)def.numPathNodes * def.pathSpacing);
			this.CreateSegments();
		}

		// Token: 0x06007C78 RID: 31864 RVA: 0x003059D4 File Offset: 0x00303BD4
		private void CreateSegments()
		{
			float num = (float)SegmentedCreature.Instance.creatureBatchSlot * 0.01f;
			SegmentedCreature.Instance.creatureBatchSlot = (SegmentedCreature.Instance.creatureBatchSlot + 1) % 10;
			SegmentedCreature.CreatureSegment value = this.segments.AddFirst(new SegmentedCreature.CreatureSegment(base.GetComponent<KBatchedAnimController>(), base.gameObject, num, base.smi.def.headOffset, Vector3.zero)).Value;
			base.gameObject.SetActive(false);
			value.animController = base.GetComponent<KBatchedAnimController>();
			value.animController.SetSymbolVisiblity(base.smi.def.segmentTrackerSymbol, false);
			value.symbol = base.smi.def.segmentTrackerSymbol;
			value.SetPosition(base.transform.position);
			base.gameObject.SetActive(true);
			this.baseAnimScale = value.animController.animScale;
			value.animController.onAnimEnter += this.AnimEntered;
			value.animController.onAnimComplete += this.AnimComplete;
			for (int i = 0; i < base.def.numBodySegments; i++)
			{
				GameObject gameObject = new GameObject(base.gameObject.GetProperName() + string.Format(" Segment {0}", i));
				gameObject.SetActive(false);
				gameObject.transform.parent = base.transform;
				gameObject.transform.position = value.Position;
				KAnimFile kanimFile = base.def.midAnim;
				Vector3 pivot = base.def.bodyPivot;
				if (i == base.def.numBodySegments - 1)
				{
					kanimFile = base.def.tailAnim;
					pivot = base.def.tailPivot;
				}
				KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
				kbatchedAnimController.AnimFiles = new KAnimFile[]
				{
					kanimFile
				};
				kbatchedAnimController.isMovable = true;
				kbatchedAnimController.SetSymbolVisiblity(base.smi.def.segmentTrackerSymbol, false);
				kbatchedAnimController.sceneLayer = value.animController.sceneLayer;
				SegmentedCreature.CreatureSegment creatureSegment = new SegmentedCreature.CreatureSegment(value.animController, gameObject, num + (float)(i + 1) * 0.0001f, Vector3.zero, pivot);
				creatureSegment.animController = kbatchedAnimController;
				creatureSegment.symbol = base.smi.def.segmentTrackerSymbol;
				creatureSegment.distanceToPreviousSegment = base.smi.def.minSegmentSpacing;
				creatureSegment.animLink = new KAnimLink(value.animController, kbatchedAnimController);
				this.segments.AddLast(creatureSegment);
				gameObject.SetActive(true);
			}
			for (int j = 0; j < base.def.numPathNodes; j++)
			{
				this.path.AddLast(new SegmentedCreature.PathNode(value.Position));
			}
		}

		// Token: 0x06007C79 RID: 31865 RVA: 0x00305C94 File Offset: 0x00303E94
		public void AnimEntered(HashedString name)
		{
			if (base.smi.def.retractWhenStartingAnimNames.Contains(name))
			{
				base.smi.sm.isRetracted.Set(true, base.smi, false);
			}
			else
			{
				base.smi.sm.isRetracted.Set(false, base.smi, false);
			}
			if (base.smi.def.hideBoddyWhenStartingAnimNames.Contains(name))
			{
				this.SetBodySegmentsVisibility(false);
				return;
			}
			this.SetBodySegmentsVisibility(true);
		}

		// Token: 0x06007C7A RID: 31866 RVA: 0x00305D20 File Offset: 0x00303F20
		public void SetBodySegmentsVisibility(bool visible)
		{
			for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = base.smi.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value.animController.SetVisiblity(visible);
			}
		}

		// Token: 0x06007C7B RID: 31867 RVA: 0x00305D56 File Offset: 0x00303F56
		public void AnimComplete(HashedString name)
		{
			if (base.smi.def.retractWhenEndingAnimNames.Contains(name))
			{
				base.smi.sm.isRetracted.Set(true, base.smi, false);
			}
		}

		// Token: 0x06007C7C RID: 31868 RVA: 0x00305D8E File Offset: 0x00303F8E
		public LinkedListNode<SegmentedCreature.CreatureSegment> GetHeadSegmentNode()
		{
			return base.smi.segments.First;
		}

		// Token: 0x06007C7D RID: 31869 RVA: 0x00305DA0 File Offset: 0x00303FA0
		public LinkedListNode<SegmentedCreature.CreatureSegment> GetFirstBodySegmentNode()
		{
			return base.smi.segments.First.Next;
		}

		// Token: 0x06007C7E RID: 31870 RVA: 0x00305DB8 File Offset: 0x00303FB8
		public float LengthPercentage()
		{
			float num = 0f;
			for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = this.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				num += linkedListNode.Value.distanceToPreviousSegment;
			}
			float num2 = this.MinLength();
			float num3 = this.MaxLength();
			return Mathf.Clamp(num - num2, 0f, num3) / (num3 - num2);
		}

		// Token: 0x06007C7F RID: 31871 RVA: 0x00305E0C File Offset: 0x0030400C
		public float MinLength()
		{
			return base.smi.def.minSegmentSpacing * (float)base.smi.def.numBodySegments;
		}

		// Token: 0x06007C80 RID: 31872 RVA: 0x00305E30 File Offset: 0x00304030
		public float MaxLength()
		{
			return base.smi.def.maxSegmentSpacing * (float)base.smi.def.numBodySegments;
		}

		// Token: 0x06007C81 RID: 31873 RVA: 0x00305E54 File Offset: 0x00304054
		protected override void OnCleanUp()
		{
			this.GetHeadSegmentNode().Value.animController.onAnimEnter -= this.AnimEntered;
			this.GetHeadSegmentNode().Value.animController.onAnimComplete -= this.AnimComplete;
			for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = this.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value.CleanUp();
			}
		}

		// Token: 0x04005D5A RID: 23898
		private const int NUM_CREATURE_SLOTS = 10;

		// Token: 0x04005D5B RID: 23899
		private static int creatureBatchSlot;

		// Token: 0x04005D5C RID: 23900
		public float baseAnimScale;

		// Token: 0x04005D5D RID: 23901
		public Vector3 previousHeadPosition;

		// Token: 0x04005D5E RID: 23902
		public float previousDist;

		// Token: 0x04005D5F RID: 23903
		public LinkedList<SegmentedCreature.PathNode> path = new LinkedList<SegmentedCreature.PathNode>();

		// Token: 0x04005D60 RID: 23904
		public LinkedList<SegmentedCreature.CreatureSegment> segments = new LinkedList<SegmentedCreature.CreatureSegment>();
	}

	// Token: 0x020010A1 RID: 4257
	public class PathNode
	{
		// Token: 0x06007C82 RID: 31874 RVA: 0x00305EC1 File Offset: 0x003040C1
		public PathNode(Vector3 position)
		{
			this.position = position;
			this.rotation = Quaternion.identity;
		}

		// Token: 0x04005D61 RID: 23905
		public Vector3 position;

		// Token: 0x04005D62 RID: 23906
		public Quaternion rotation;
	}

	// Token: 0x020010A2 RID: 4258
	public class CreatureSegment
	{
		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06007C83 RID: 31875 RVA: 0x00305EDB File Offset: 0x003040DB
		public float ZOffset
		{
			get
			{
				return Grid.GetLayerZ(this.head.sceneLayer) + this.zRelativeOffset;
			}
		}

		// Token: 0x06007C84 RID: 31876 RVA: 0x00305EF4 File Offset: 0x003040F4
		public CreatureSegment(KBatchedAnimController head, GameObject go, float zRelativeOffset, Vector3 offset, Vector3 pivot)
		{
			this.head = head;
			this.m_transform = go.transform;
			this.zRelativeOffset = zRelativeOffset;
			this.offset = offset;
			this.pivot = pivot;
			this.SetPosition(go.transform.position);
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06007C85 RID: 31877 RVA: 0x00305F44 File Offset: 0x00304144
		public Vector3 Position
		{
			get
			{
				Vector3 vector = this.offset;
				vector.x *= (float)(this.animController.FlipX ? -1 : 1);
				if (vector != Vector3.zero)
				{
					vector = this.Rotation * vector;
				}
				if (this.symbol.IsValid)
				{
					bool flag;
					Vector3 a = this.animController.GetSymbolTransform(this.symbol, out flag).GetColumn(3);
					a.z = this.ZOffset;
					return a + vector;
				}
				return this.m_transform.position + vector;
			}
		}

		// Token: 0x06007C86 RID: 31878 RVA: 0x00305FE8 File Offset: 0x003041E8
		public void SetPosition(Vector3 value)
		{
			bool flag = false;
			if (this.animController != null && this.animController.sceneLayer != this.head.sceneLayer)
			{
				this.animController.SetSceneLayer(this.head.sceneLayer);
				flag = true;
			}
			value.z = this.ZOffset;
			this.m_transform.position = value;
			if (flag)
			{
				this.animController.enabled = false;
				this.animController.enabled = true;
			}
		}

		// Token: 0x06007C87 RID: 31879 RVA: 0x00306069 File Offset: 0x00304269
		public void SetRotation(Quaternion rotation)
		{
			this.m_transform.rotation = rotation;
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06007C88 RID: 31880 RVA: 0x00306078 File Offset: 0x00304278
		public Quaternion Rotation
		{
			get
			{
				if (this.symbol.IsValid)
				{
					bool flag;
					Vector3 toDirection = this.animController.GetSymbolLocalTransform(this.symbol, out flag).MultiplyVector(Vector3.right);
					if (!this.animController.FlipX)
					{
						toDirection.y *= -1f;
					}
					return Quaternion.FromToRotation(Vector3.right, toDirection);
				}
				return this.m_transform.rotation;
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06007C89 RID: 31881 RVA: 0x003060E7 File Offset: 0x003042E7
		public Vector3 Forward
		{
			get
			{
				return this.Rotation * (this.animController.FlipX ? Vector3.left : Vector3.right);
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06007C8A RID: 31882 RVA: 0x0030610D File Offset: 0x0030430D
		public Vector3 Up
		{
			get
			{
				return this.Rotation * Vector3.up;
			}
		}

		// Token: 0x06007C8B RID: 31883 RVA: 0x0030611F File Offset: 0x0030431F
		public void CleanUp()
		{
			UnityEngine.Object.Destroy(this.m_transform.gameObject);
		}

		// Token: 0x04005D63 RID: 23907
		public KBatchedAnimController animController;

		// Token: 0x04005D64 RID: 23908
		public KAnimLink animLink;

		// Token: 0x04005D65 RID: 23909
		public float distanceToPreviousSegment;

		// Token: 0x04005D66 RID: 23910
		public HashedString symbol;

		// Token: 0x04005D67 RID: 23911
		public Vector3 offset;

		// Token: 0x04005D68 RID: 23912
		public Vector3 pivot;

		// Token: 0x04005D69 RID: 23913
		public KBatchedAnimController head;

		// Token: 0x04005D6A RID: 23914
		private float zRelativeOffset;

		// Token: 0x04005D6B RID: 23915
		private Transform m_transform;
	}
}
