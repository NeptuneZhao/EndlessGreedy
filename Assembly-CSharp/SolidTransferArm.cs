using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Database;
using FMODUnity;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x0200076E RID: 1902
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidTransferArm : StateMachineComponent<SolidTransferArm.SMInstance>, ISim1000ms, IRenderEveryTick
{
	// Token: 0x0600332A RID: 13098 RVA: 0x00118B9C File Offset: 0x00116D9C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreConsumer.AddProvider(GlobalChoreProvider.Instance);
		this.choreConsumer.SetReach(this.pickupRange);
		Klei.AI.Attributes attributes = this.GetAttributes();
		if (attributes.Get(Db.Get().Attributes.CarryAmount) == null)
		{
			attributes.Add(Db.Get().Attributes.CarryAmount);
		}
		AttributeModifier modifier = new AttributeModifier(Db.Get().Attributes.CarryAmount.Id, this.max_carry_weight, base.gameObject.GetProperName(), false, false, true);
		this.GetAttributes().Add(modifier);
		this.worker.usesMultiTool = false;
		this.storage.fxPrefix = Storage.FXPrefix.PickedUp;
		this.simRenderLoadBalance = false;
	}

	// Token: 0x0600332B RID: 13099 RVA: 0x00118C60 File Offset: 0x00116E60
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		string name = component.name + ".arm";
		this.arm_go = new GameObject(name);
		this.arm_go.SetActive(false);
		this.arm_go.transform.parent = component.transform;
		this.looping_sounds = this.arm_go.AddComponent<LoopingSounds>();
		string sound = GlobalAssets.GetSound(this.rotateSoundName, false);
		this.rotateSound = RuntimeManager.PathToEventReference(sound);
		this.arm_go.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
		this.arm_anim_ctrl = this.arm_go.AddComponent<KBatchedAnimController>();
		this.arm_anim_ctrl.AnimFiles = new KAnimFile[]
		{
			component.AnimFiles[0]
		};
		this.arm_anim_ctrl.initialAnim = "arm";
		this.arm_anim_ctrl.isMovable = true;
		this.arm_anim_ctrl.sceneLayer = Grid.SceneLayer.TransferArm;
		component.SetSymbolVisiblity("arm_target", false);
		bool flag;
		Vector3 position = component.GetSymbolTransform(new HashedString("arm_target"), out flag).GetColumn(3);
		position.z = Grid.GetLayerZ(Grid.SceneLayer.TransferArm);
		this.arm_go.transform.SetPosition(position);
		this.arm_go.SetActive(true);
		this.link = new KAnimLink(component, this.arm_anim_ctrl);
		ChoreGroups choreGroups = Db.Get().ChoreGroups;
		for (int i = 0; i < choreGroups.Count; i++)
		{
			this.choreConsumer.SetPermittedByUser(choreGroups[i], true);
		}
		base.Subscribe<SolidTransferArm>(-592767678, SolidTransferArm.OnOperationalChangedDelegate);
		base.Subscribe<SolidTransferArm>(1745615042, SolidTransferArm.OnEndChoreDelegate);
		this.RotateArm(this.rotatable.GetRotatedOffset(Vector3.up), true, 0f);
		this.DropLeftovers();
		component.enabled = false;
		component.enabled = true;
		MinionGroupProber.Get().SetValidSerialNos(this, this.serial_no, this.serial_no);
		base.smi.StartSM();
	}

	// Token: 0x0600332C RID: 13100 RVA: 0x00118E6D File Offset: 0x0011706D
	protected override void OnCleanUp()
	{
		MinionGroupProber.Get().ReleaseProber(this);
		base.OnCleanUp();
	}

	// Token: 0x0600332D RID: 13101 RVA: 0x00118E84 File Offset: 0x00117084
	public static void BatchUpdate(List<UpdateBucketWithUpdater<ISim1000ms>.Entry> solid_transfer_arms, float time_delta)
	{
		SolidTransferArm.BatchUpdateContext batchUpdateContext = new SolidTransferArm.BatchUpdateContext(solid_transfer_arms);
		if (batchUpdateContext.solid_transfer_arms.Count == 0)
		{
			batchUpdateContext.Finish();
			return;
		}
		SolidTransferArm.batch_update_job.Reset(batchUpdateContext);
		int num = Math.Max(1, batchUpdateContext.solid_transfer_arms.Count / CPUBudget.coreCount);
		int num2 = Math.Min(batchUpdateContext.solid_transfer_arms.Count, CPUBudget.coreCount);
		for (int num3 = 0; num3 != num2; num3++)
		{
			int num4 = num3 * num;
			int end = (num3 == num2 - 1) ? batchUpdateContext.solid_transfer_arms.Count : (num4 + num);
			SolidTransferArm.batch_update_job.Add(new SolidTransferArm.BatchUpdateTask(num4, end));
		}
		GlobalJobManager.Run(SolidTransferArm.batch_update_job);
		for (int num5 = 0; num5 != SolidTransferArm.batch_update_job.Count; num5++)
		{
			SolidTransferArm.batch_update_job.GetWorkItem(num5).Finish();
		}
		batchUpdateContext.Finish();
		SolidTransferArm.batch_update_job.Reset(null);
	}

	// Token: 0x0600332E RID: 13102 RVA: 0x00118F6C File Offset: 0x0011716C
	private void Sim()
	{
		Chore.Precondition.Context context = default(Chore.Precondition.Context);
		if (this.choreConsumer.FindNextChore(ref context))
		{
			if (context.chore is FetchChore)
			{
				this.choreDriver.SetChore(context);
				FetchChore chore = context.chore as FetchChore;
				this.storage.DropUnlessMatching(chore);
				this.arm_anim_ctrl.enabled = false;
				this.arm_anim_ctrl.enabled = true;
			}
			else
			{
				bool condition = false;
				string str = "I am but a lowly transfer arm. I should only acquire FetchChores: ";
				Chore chore2 = context.chore;
				global::Debug.Assert(condition, str + ((chore2 != null) ? chore2.ToString() : null));
			}
		}
		this.operational.SetActive(this.choreDriver.HasChore(), false);
	}

	// Token: 0x0600332F RID: 13103 RVA: 0x00119014 File Offset: 0x00117214
	public void Sim1000ms(float dt)
	{
	}

	// Token: 0x06003330 RID: 13104 RVA: 0x00119018 File Offset: 0x00117218
	private void UpdateArmAnim()
	{
		FetchAreaChore fetchAreaChore = this.choreDriver.GetCurrentChore() as FetchAreaChore;
		if (this.worker.GetWorkable() && fetchAreaChore != null && this.rotation_complete)
		{
			this.StopRotateSound();
			this.SetArmAnim(fetchAreaChore.IsDelivering ? SolidTransferArm.ArmAnim.Drop : SolidTransferArm.ArmAnim.Pickup);
			return;
		}
		this.SetArmAnim(SolidTransferArm.ArmAnim.Idle);
	}

	// Token: 0x06003331 RID: 13105 RVA: 0x00119074 File Offset: 0x00117274
	private bool AsyncUpdate(int cell, HashSet<int> workspace, GameObject game_object)
	{
		workspace.Clear();
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		for (int i = num2 - this.pickupRange; i < num2 + this.pickupRange + 1; i++)
		{
			for (int j = num - this.pickupRange; j < num + this.pickupRange + 1; j++)
			{
				int num3 = Grid.XYToCell(j, i);
				if (Grid.IsValidCell(num3) && Grid.IsPhysicallyAccessible(num, num2, j, i, true))
				{
					workspace.Add(num3);
				}
			}
		}
		bool flag = !this.reachableCells.SetEquals(workspace);
		if (flag)
		{
			this.reachableCells.Clear();
			this.reachableCells.UnionWith(workspace);
		}
		this.pickupables.Clear();
		foreach (object obj in GameScenePartitioner.Instance.AsyncSafeEnumerate(num - this.pickupRange, num2 - this.pickupRange, 2 * this.pickupRange + 1, 2 * this.pickupRange + 1, GameScenePartitioner.Instance.pickupablesLayer).Concat(GameScenePartitioner.Instance.AsyncSafeEnumerate(num - this.pickupRange, num2 - this.pickupRange, 2 * this.pickupRange + 1, 2 * this.pickupRange + 1, GameScenePartitioner.Instance.storedPickupablesLayer)))
		{
			Pickupable pickupable = obj as Pickupable;
			if (Grid.GetCellRange(cell, pickupable.cachedCell) <= this.pickupRange && this.IsPickupableRelevantToMyInterests(pickupable.KPrefabID, pickupable.cachedCell) && pickupable.CouldBePickedUpByTransferArm(game_object))
			{
				this.pickupables.Add(pickupable);
			}
		}
		return flag;
	}

	// Token: 0x06003332 RID: 13106 RVA: 0x00119224 File Offset: 0x00117424
	private void IncrementSerialNo()
	{
		this.serial_no += 1;
		MinionGroupProber.Get().SetValidSerialNos(this, this.serial_no, this.serial_no);
		MinionGroupProber.Get().Occupy(this, this.serial_no, this.reachableCells);
	}

	// Token: 0x06003333 RID: 13107 RVA: 0x00119263 File Offset: 0x00117463
	public bool IsCellReachable(int cell)
	{
		return this.reachableCells.Contains(cell);
	}

	// Token: 0x06003334 RID: 13108 RVA: 0x00119271 File Offset: 0x00117471
	private bool IsPickupableRelevantToMyInterests(KPrefabID prefabID, int storage_cell)
	{
		return Assets.IsTagSolidTransferArmConveyable(prefabID.PrefabTag) && this.IsCellReachable(storage_cell);
	}

	// Token: 0x06003335 RID: 13109 RVA: 0x00119289 File Offset: 0x00117489
	public Pickupable FindFetchTarget(Storage destination, FetchChore chore)
	{
		return FetchManager.FindFetchTarget(this.pickupables, destination, chore);
	}

	// Token: 0x06003336 RID: 13110 RVA: 0x00119298 File Offset: 0x00117498
	public void RenderEveryTick(float dt)
	{
		if (this.worker.GetWorkable())
		{
			Vector3 targetPoint = this.worker.GetWorkable().GetTargetPoint();
			targetPoint.z = 0f;
			Vector3 position = base.transform.GetPosition();
			position.z = 0f;
			Vector3 target_dir = Vector3.Normalize(targetPoint - position);
			this.RotateArm(target_dir, false, dt);
		}
		this.UpdateArmAnim();
	}

	// Token: 0x06003337 RID: 13111 RVA: 0x00119308 File Offset: 0x00117508
	private void OnEndChore(object data)
	{
		this.DropLeftovers();
	}

	// Token: 0x06003338 RID: 13112 RVA: 0x00119310 File Offset: 0x00117510
	private void DropLeftovers()
	{
		if (!this.storage.IsEmpty() && !this.choreDriver.HasChore())
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x06003339 RID: 13113 RVA: 0x00119350 File Offset: 0x00117550
	private void SetArmAnim(SolidTransferArm.ArmAnim new_anim)
	{
		if (new_anim == this.arm_anim)
		{
			return;
		}
		this.arm_anim = new_anim;
		switch (this.arm_anim)
		{
		case SolidTransferArm.ArmAnim.Idle:
			this.arm_anim_ctrl.Play("arm", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		case SolidTransferArm.ArmAnim.Pickup:
			this.arm_anim_ctrl.Play("arm_pickup", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		case SolidTransferArm.ArmAnim.Drop:
			this.arm_anim_ctrl.Play("arm_drop", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		default:
			return;
		}
	}

	// Token: 0x0600333A RID: 13114 RVA: 0x001193EA File Offset: 0x001175EA
	private void OnOperationalChanged(object data)
	{
		if (!(bool)data)
		{
			if (this.choreDriver.HasChore())
			{
				this.choreDriver.StopChore();
			}
			this.UpdateArmAnim();
		}
	}

	// Token: 0x0600333B RID: 13115 RVA: 0x00119412 File Offset: 0x00117612
	private void SetArmRotation(float rot)
	{
		this.arm_rot = rot;
		this.arm_go.transform.rotation = Quaternion.Euler(0f, 0f, this.arm_rot);
	}

	// Token: 0x0600333C RID: 13116 RVA: 0x00119440 File Offset: 0x00117640
	private void RotateArm(Vector3 target_dir, bool warp, float dt)
	{
		float num = MathUtil.AngleSigned(Vector3.up, target_dir, Vector3.forward) - this.arm_rot;
		if (num < -180f)
		{
			num += 360f;
		}
		if (num > 180f)
		{
			num -= 360f;
		}
		if (!warp)
		{
			num = Mathf.Clamp(num, -this.turn_rate * dt, this.turn_rate * dt);
		}
		this.arm_rot += num;
		this.SetArmRotation(this.arm_rot);
		this.rotation_complete = Mathf.Approximately(num, 0f);
		if (!warp && !this.rotation_complete)
		{
			if (!this.rotateSoundPlaying)
			{
				this.StartRotateSound();
			}
			this.SetRotateSoundParameter(this.arm_rot);
			return;
		}
		this.StopRotateSound();
	}

	// Token: 0x0600333D RID: 13117 RVA: 0x001194F7 File Offset: 0x001176F7
	private void StartRotateSound()
	{
		if (!this.rotateSoundPlaying)
		{
			this.looping_sounds.StartSound(this.rotateSound);
			this.rotateSoundPlaying = true;
		}
	}

	// Token: 0x0600333E RID: 13118 RVA: 0x0011951A File Offset: 0x0011771A
	private void SetRotateSoundParameter(float arm_rot)
	{
		if (this.rotateSoundPlaying)
		{
			this.looping_sounds.SetParameter(this.rotateSound, SolidTransferArm.HASH_ROTATION, arm_rot);
		}
	}

	// Token: 0x0600333F RID: 13119 RVA: 0x0011953B File Offset: 0x0011773B
	private void StopRotateSound()
	{
		if (this.rotateSoundPlaying)
		{
			this.looping_sounds.StopSound(this.rotateSound);
			this.rotateSoundPlaying = false;
		}
	}

	// Token: 0x06003340 RID: 13120 RVA: 0x0011955D File Offset: 0x0011775D
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void BeginDetailedSample(string region_name)
	{
	}

	// Token: 0x06003341 RID: 13121 RVA: 0x0011955F File Offset: 0x0011775F
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void BeginDetailedSample(string region_name, int count)
	{
	}

	// Token: 0x06003342 RID: 13122 RVA: 0x00119561 File Offset: 0x00117761
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void EndDetailedSample(string region_name)
	{
	}

	// Token: 0x06003343 RID: 13123 RVA: 0x00119563 File Offset: 0x00117763
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void EndDetailedSample(string region_name, int count)
	{
	}

	// Token: 0x04001E36 RID: 7734
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001E37 RID: 7735
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04001E38 RID: 7736
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04001E39 RID: 7737
	[MyCmpAdd]
	private StandardWorker worker;

	// Token: 0x04001E3A RID: 7738
	[MyCmpAdd]
	private ChoreConsumer choreConsumer;

	// Token: 0x04001E3B RID: 7739
	[MyCmpAdd]
	private ChoreDriver choreDriver;

	// Token: 0x04001E3C RID: 7740
	public int pickupRange = 4;

	// Token: 0x04001E3D RID: 7741
	private float max_carry_weight = 1000f;

	// Token: 0x04001E3E RID: 7742
	private List<Pickupable> pickupables = new List<Pickupable>();

	// Token: 0x04001E3F RID: 7743
	private KBatchedAnimController arm_anim_ctrl;

	// Token: 0x04001E40 RID: 7744
	private GameObject arm_go;

	// Token: 0x04001E41 RID: 7745
	private LoopingSounds looping_sounds;

	// Token: 0x04001E42 RID: 7746
	private bool rotateSoundPlaying;

	// Token: 0x04001E43 RID: 7747
	private string rotateSoundName = "TransferArm_rotate";

	// Token: 0x04001E44 RID: 7748
	private EventReference rotateSound;

	// Token: 0x04001E45 RID: 7749
	private KAnimLink link;

	// Token: 0x04001E46 RID: 7750
	private float arm_rot = 45f;

	// Token: 0x04001E47 RID: 7751
	private float turn_rate = 360f;

	// Token: 0x04001E48 RID: 7752
	private bool rotation_complete;

	// Token: 0x04001E49 RID: 7753
	private SolidTransferArm.ArmAnim arm_anim;

	// Token: 0x04001E4A RID: 7754
	private HashSet<int> reachableCells = new HashSet<int>();

	// Token: 0x04001E4B RID: 7755
	private static readonly EventSystem.IntraObjectHandler<SolidTransferArm> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SolidTransferArm>(delegate(SolidTransferArm component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04001E4C RID: 7756
	private static readonly EventSystem.IntraObjectHandler<SolidTransferArm> OnEndChoreDelegate = new EventSystem.IntraObjectHandler<SolidTransferArm>(delegate(SolidTransferArm component, object data)
	{
		component.OnEndChore(data);
	});

	// Token: 0x04001E4D RID: 7757
	private static WorkItemCollection<SolidTransferArm.BatchUpdateTask, SolidTransferArm.BatchUpdateContext> batch_update_job = new WorkItemCollection<SolidTransferArm.BatchUpdateTask, SolidTransferArm.BatchUpdateContext>();

	// Token: 0x04001E4E RID: 7758
	private short serial_no;

	// Token: 0x04001E4F RID: 7759
	private static HashedString HASH_ROTATION = "rotation";

	// Token: 0x02001600 RID: 5632
	private enum ArmAnim
	{
		// Token: 0x04006E58 RID: 28248
		Idle,
		// Token: 0x04006E59 RID: 28249
		Pickup,
		// Token: 0x04006E5A RID: 28250
		Drop
	}

	// Token: 0x02001601 RID: 5633
	public class SMInstance : GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.GameInstance
	{
		// Token: 0x06009099 RID: 37017 RVA: 0x0034C1EF File Offset: 0x0034A3EF
		public SMInstance(SolidTransferArm master) : base(master)
		{
		}
	}

	// Token: 0x02001602 RID: 5634
	public class States : GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm>
	{
		// Token: 0x0600909A RID: 37018 RVA: 0x0034C1F8 File Offset: 0x0034A3F8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (SolidTransferArm.SMInstance smi) => smi.GetComponent<Operational>().IsOperational).Enter(delegate(SolidTransferArm.SMInstance smi)
			{
				smi.master.StopRotateSound();
			});
			this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (SolidTransferArm.SMInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.on.idle.PlayAnim("on").EventTransition(GameHashes.ActiveChanged, this.on.working, (SolidTransferArm.SMInstance smi) => smi.GetComponent<Operational>().IsActive);
			this.on.working.PlayAnim("working").EventTransition(GameHashes.ActiveChanged, this.on.idle, (SolidTransferArm.SMInstance smi) => !smi.GetComponent<Operational>().IsActive);
		}

		// Token: 0x04006E5B RID: 28251
		public StateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.BoolParameter transferring;

		// Token: 0x04006E5C RID: 28252
		public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State off;

		// Token: 0x04006E5D RID: 28253
		public SolidTransferArm.States.ReadyStates on;

		// Token: 0x02002536 RID: 9526
		public class ReadyStates : GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State
		{
			// Token: 0x0400A5B7 RID: 42423
			public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State idle;

			// Token: 0x0400A5B8 RID: 42424
			public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State working;
		}
	}

	// Token: 0x02001603 RID: 5635
	private class BatchUpdateContext
	{
		// Token: 0x0600909C RID: 37020 RVA: 0x0034C360 File Offset: 0x0034A560
		public BatchUpdateContext(List<UpdateBucketWithUpdater<ISim1000ms>.Entry> solid_transfer_arms)
		{
			this.solid_transfer_arms = ListPool<SolidTransferArm, SolidTransferArm.BatchUpdateContext>.Allocate();
			this.solid_transfer_arms.Capacity = solid_transfer_arms.Count;
			this.refreshed_reachable_cells = ListPool<bool, SolidTransferArm.BatchUpdateContext>.Allocate();
			this.refreshed_reachable_cells.Capacity = solid_transfer_arms.Count;
			this.cells = ListPool<int, SolidTransferArm.BatchUpdateContext>.Allocate();
			this.cells.Capacity = solid_transfer_arms.Count;
			this.game_objects = ListPool<GameObject, SolidTransferArm.BatchUpdateContext>.Allocate();
			this.game_objects.Capacity = solid_transfer_arms.Count;
			for (int num = 0; num != solid_transfer_arms.Count; num++)
			{
				UpdateBucketWithUpdater<ISim1000ms>.Entry entry = solid_transfer_arms[num];
				entry.lastUpdateTime = 0f;
				solid_transfer_arms[num] = entry;
				SolidTransferArm solidTransferArm = (SolidTransferArm)entry.data;
				if (solidTransferArm.operational.IsOperational)
				{
					this.solid_transfer_arms.Add(solidTransferArm);
					this.refreshed_reachable_cells.Add(false);
					this.cells.Add(Grid.PosToCell(solidTransferArm));
					this.game_objects.Add(solidTransferArm.gameObject);
				}
			}
		}

		// Token: 0x0600909D RID: 37021 RVA: 0x0034C464 File Offset: 0x0034A664
		public void Finish()
		{
			for (int num = 0; num != this.solid_transfer_arms.Count; num++)
			{
				if (this.refreshed_reachable_cells[num])
				{
					this.solid_transfer_arms[num].IncrementSerialNo();
				}
				this.solid_transfer_arms[num].Sim();
			}
			this.refreshed_reachable_cells.Recycle();
			this.cells.Recycle();
			this.game_objects.Recycle();
			this.solid_transfer_arms.Recycle();
		}

		// Token: 0x04006E5E RID: 28254
		public ListPool<SolidTransferArm, SolidTransferArm.BatchUpdateContext>.PooledList solid_transfer_arms;

		// Token: 0x04006E5F RID: 28255
		public ListPool<bool, SolidTransferArm.BatchUpdateContext>.PooledList refreshed_reachable_cells;

		// Token: 0x04006E60 RID: 28256
		public ListPool<int, SolidTransferArm.BatchUpdateContext>.PooledList cells;

		// Token: 0x04006E61 RID: 28257
		public ListPool<GameObject, SolidTransferArm.BatchUpdateContext>.PooledList game_objects;
	}

	// Token: 0x02001604 RID: 5636
	private struct BatchUpdateTask : IWorkItem<SolidTransferArm.BatchUpdateContext>
	{
		// Token: 0x0600909E RID: 37022 RVA: 0x0034C4E3 File Offset: 0x0034A6E3
		public BatchUpdateTask(int start, int end)
		{
			this.start = start;
			this.end = end;
			this.reachable_cells_workspace = HashSetPool<int, SolidTransferArm>.Allocate();
		}

		// Token: 0x0600909F RID: 37023 RVA: 0x0034C500 File Offset: 0x0034A700
		public void Run(SolidTransferArm.BatchUpdateContext context)
		{
			for (int num = this.start; num != this.end; num++)
			{
				context.refreshed_reachable_cells[num] = context.solid_transfer_arms[num].AsyncUpdate(context.cells[num], this.reachable_cells_workspace, context.game_objects[num]);
			}
		}

		// Token: 0x060090A0 RID: 37024 RVA: 0x0034C55E File Offset: 0x0034A75E
		public void Finish()
		{
			this.reachable_cells_workspace.Recycle();
		}

		// Token: 0x04006E62 RID: 28258
		private int start;

		// Token: 0x04006E63 RID: 28259
		private int end;

		// Token: 0x04006E64 RID: 28260
		private HashSetPool<int, SolidTransferArm>.PooledHashSet reachable_cells_workspace;
	}

	// Token: 0x02001605 RID: 5637
	public struct CachedPickupable
	{
		// Token: 0x04006E65 RID: 28261
		public Pickupable pickupable;

		// Token: 0x04006E66 RID: 28262
		public int storage_cell;
	}
}
