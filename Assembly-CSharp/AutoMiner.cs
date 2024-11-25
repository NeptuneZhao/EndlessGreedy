using System;
using FMODUnity;
using KSerialization;
using UnityEngine;

// Token: 0x02000683 RID: 1667
[SerializationConfig(MemberSerialization.OptIn)]
public class AutoMiner : StateMachineComponent<AutoMiner.Instance>, ISim1000ms
{
	// Token: 0x1700020A RID: 522
	// (get) Token: 0x06002942 RID: 10562 RVA: 0x000E94D5 File Offset: 0x000E76D5
	private bool HasDigCell
	{
		get
		{
			return this.dig_cell != Grid.InvalidCell;
		}
	}

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x06002943 RID: 10563 RVA: 0x000E94E7 File Offset: 0x000E76E7
	private bool RotationComplete
	{
		get
		{
			return this.HasDigCell && this.rotation_complete;
		}
	}

	// Token: 0x06002944 RID: 10564 RVA: 0x000E94F9 File Offset: 0x000E76F9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.simRenderLoadBalance = true;
	}

	// Token: 0x06002945 RID: 10565 RVA: 0x000E9508 File Offset: 0x000E7708
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.hitEffectPrefab = Assets.GetPrefab("fx_dig_splash");
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		string name = component.name + ".gun";
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
		this.arm_anim_ctrl.initialAnim = "gun";
		this.arm_anim_ctrl.isMovable = true;
		this.arm_anim_ctrl.sceneLayer = Grid.SceneLayer.TransferArm;
		component.SetSymbolVisiblity("gun_target", false);
		bool flag;
		Vector3 position = component.GetSymbolTransform(new HashedString("gun_target"), out flag).GetColumn(3);
		position.z = Grid.GetLayerZ(Grid.SceneLayer.TransferArm);
		this.arm_go.transform.SetPosition(position);
		this.arm_go.SetActive(true);
		this.link = new KAnimLink(component, this.arm_anim_ctrl);
		base.Subscribe<AutoMiner>(-592767678, AutoMiner.OnOperationalChangedDelegate);
		this.RotateArm(this.rotatable.GetRotatedOffset(Quaternion.Euler(0f, 0f, -45f) * Vector3.up), true, 0f);
		this.StopDig();
		base.smi.StartSM();
	}

	// Token: 0x06002946 RID: 10566 RVA: 0x000E96D6 File Offset: 0x000E78D6
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06002947 RID: 10567 RVA: 0x000E96DE File Offset: 0x000E78DE
	public void Sim1000ms(float dt)
	{
		if (!this.operational.IsOperational)
		{
			return;
		}
		this.RefreshDiggableCell();
		this.operational.SetActive(this.HasDigCell, false);
	}

	// Token: 0x06002948 RID: 10568 RVA: 0x000E9706 File Offset: 0x000E7906
	private void OnOperationalChanged(object data)
	{
		if (!(bool)data)
		{
			this.dig_cell = Grid.InvalidCell;
			this.rotation_complete = false;
		}
	}

	// Token: 0x06002949 RID: 10569 RVA: 0x000E9724 File Offset: 0x000E7924
	public void UpdateRotation(float dt)
	{
		if (this.HasDigCell)
		{
			Vector3 a = Grid.CellToPosCCC(this.dig_cell, Grid.SceneLayer.TileMain);
			a.z = 0f;
			Vector3 position = this.arm_go.transform.GetPosition();
			position.z = 0f;
			Vector3 target_dir = Vector3.Normalize(a - position);
			this.RotateArm(target_dir, false, dt);
		}
	}

	// Token: 0x0600294A RID: 10570 RVA: 0x000E9786 File Offset: 0x000E7986
	private Element GetTargetElement()
	{
		if (this.HasDigCell)
		{
			return Grid.Element[this.dig_cell];
		}
		return null;
	}

	// Token: 0x0600294B RID: 10571 RVA: 0x000E97A0 File Offset: 0x000E79A0
	public void StartDig()
	{
		Element targetElement = this.GetTargetElement();
		base.Trigger(-1762453998, targetElement);
		this.CreateHitEffect();
		this.arm_anim_ctrl.Play("gun_digging", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x0600294C RID: 10572 RVA: 0x000E97E6 File Offset: 0x000E79E6
	public void StopDig()
	{
		base.Trigger(939543986, null);
		this.DestroyHitEffect();
		this.arm_anim_ctrl.Play("gun", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x0600294D RID: 10573 RVA: 0x000E981C File Offset: 0x000E7A1C
	public void UpdateDig(float dt)
	{
		if (!this.HasDigCell)
		{
			return;
		}
		if (!this.rotation_complete)
		{
			return;
		}
		Diggable.DoDigTick(this.dig_cell, dt, WorldDamage.DamageType.NoBuildingDamage);
		float percentComplete = Grid.Damage[this.dig_cell];
		this.mining_sounds.SetPercentComplete(percentComplete);
		Vector3 a = Grid.CellToPosCCC(this.dig_cell, Grid.SceneLayer.FXFront2);
		a.z = 0f;
		Vector3 position = this.arm_go.transform.GetPosition();
		position.z = 0f;
		float sqrMagnitude = (a - position).sqrMagnitude;
		this.arm_anim_ctrl.GetBatchInstanceData().SetClipRadius(position.x, position.y, sqrMagnitude, true);
		if (!AutoMiner.ValidDigCell(this.dig_cell))
		{
			this.dig_cell = Grid.InvalidCell;
			this.rotation_complete = false;
		}
	}

	// Token: 0x0600294E RID: 10574 RVA: 0x000E98E8 File Offset: 0x000E7AE8
	private void CreateHitEffect()
	{
		if (this.hitEffectPrefab == null)
		{
			return;
		}
		if (this.hitEffect != null)
		{
			this.DestroyHitEffect();
		}
		Vector3 position = Grid.CellToPosCCC(this.dig_cell, Grid.SceneLayer.FXFront2);
		this.hitEffect = GameUtil.KInstantiate(this.hitEffectPrefab, position, Grid.SceneLayer.FXFront2, null, 0);
		this.hitEffect.SetActive(true);
		KBatchedAnimController component = this.hitEffect.GetComponent<KBatchedAnimController>();
		component.sceneLayer = Grid.SceneLayer.FXFront2;
		component.initialMode = KAnim.PlayMode.Loop;
		component.enabled = false;
		component.enabled = true;
	}

	// Token: 0x0600294F RID: 10575 RVA: 0x000E996F File Offset: 0x000E7B6F
	private void DestroyHitEffect()
	{
		if (this.hitEffectPrefab == null)
		{
			return;
		}
		if (this.hitEffect != null)
		{
			this.hitEffect.DeleteObject();
			this.hitEffect = null;
		}
	}

	// Token: 0x06002950 RID: 10576 RVA: 0x000E99A0 File Offset: 0x000E7BA0
	private void RefreshDiggableCell()
	{
		CellOffset rotatedCellOffset = this.vision_offset;
		if (this.rotatable)
		{
			rotatedCellOffset = this.rotatable.GetRotatedCellOffset(this.vision_offset);
		}
		int cell = Grid.PosToCell(base.transform.gameObject);
		int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
		int num;
		int num2;
		Grid.CellToXY(cell2, out num, out num2);
		float num3 = float.MaxValue;
		int num4 = Grid.InvalidCell;
		Vector3 a = Grid.CellToPos(cell2);
		bool flag = false;
		for (int i = 0; i < this.height; i++)
		{
			for (int j = 0; j < this.width; j++)
			{
				CellOffset rotatedCellOffset2 = new CellOffset(this.x + j, this.y + i);
				if (this.rotatable)
				{
					rotatedCellOffset2 = this.rotatable.GetRotatedCellOffset(rotatedCellOffset2);
				}
				int num5 = Grid.OffsetCell(cell, rotatedCellOffset2);
				if (Grid.IsValidCell(num5))
				{
					int x;
					int y;
					Grid.CellToXY(num5, out x, out y);
					if (Grid.IsValidCell(num5) && AutoMiner.ValidDigCell(num5) && Grid.TestLineOfSight(num, num2, x, y, new Func<int, bool>(AutoMiner.DigBlockingCB), false, false))
					{
						if (num5 == this.dig_cell)
						{
							flag = true;
						}
						Vector3 b = Grid.CellToPos(num5);
						float num6 = Vector3.Distance(a, b);
						if (num6 < num3)
						{
							num3 = num6;
							num4 = num5;
						}
					}
				}
			}
		}
		if (!flag && this.dig_cell != num4)
		{
			this.dig_cell = num4;
			this.rotation_complete = false;
		}
	}

	// Token: 0x06002951 RID: 10577 RVA: 0x000E9B10 File Offset: 0x000E7D10
	private static bool ValidDigCell(int cell)
	{
		bool flag = Grid.HasDoor[cell] && Grid.Foundation[cell] && Grid.ObjectLayers[9].ContainsKey(cell);
		if (flag)
		{
			Door component = Grid.ObjectLayers[9][cell].GetComponent<Door>();
			flag = (component != null && component.IsOpen() && !component.IsPendingClose());
		}
		return Grid.Solid[cell] && (!Grid.Foundation[cell] || flag) && Grid.Element[cell].hardness < 150;
	}

	// Token: 0x06002952 RID: 10578 RVA: 0x000E9BB8 File Offset: 0x000E7DB8
	public static bool DigBlockingCB(int cell)
	{
		bool flag = Grid.HasDoor[cell] && Grid.Foundation[cell] && Grid.ObjectLayers[9].ContainsKey(cell);
		if (flag)
		{
			Door component = Grid.ObjectLayers[9][cell].GetComponent<Door>();
			flag = (component != null && component.IsOpen() && !component.IsPendingClose());
		}
		return (Grid.Foundation[cell] && Grid.Solid[cell] && !flag) || Grid.Element[cell].hardness >= 150;
	}

	// Token: 0x06002953 RID: 10579 RVA: 0x000E9C5C File Offset: 0x000E7E5C
	private void RotateArm(Vector3 target_dir, bool warp, float dt)
	{
		if (this.rotation_complete)
		{
			return;
		}
		float num = MathUtil.AngleSigned(Vector3.up, target_dir, Vector3.forward) - this.arm_rot;
		num = MathUtil.Wrap(-180f, 180f, num);
		this.rotation_complete = Mathf.Approximately(num, 0f);
		float num2 = num;
		if (warp)
		{
			this.rotation_complete = true;
		}
		else
		{
			num2 = Mathf.Clamp(num2, -this.turn_rate * dt, this.turn_rate * dt);
		}
		this.arm_rot += num2;
		this.arm_rot = MathUtil.Wrap(-180f, 180f, this.arm_rot);
		this.arm_go.transform.rotation = Quaternion.Euler(0f, 0f, this.arm_rot);
		if (!this.rotation_complete)
		{
			this.StartRotateSound();
			this.looping_sounds.SetParameter(this.rotateSound, AutoMiner.HASH_ROTATION, this.arm_rot);
			return;
		}
		this.StopRotateSound();
	}

	// Token: 0x06002954 RID: 10580 RVA: 0x000E9D51 File Offset: 0x000E7F51
	private void StartRotateSound()
	{
		if (!this.rotate_sound_playing)
		{
			this.looping_sounds.StartSound(this.rotateSound);
			this.rotate_sound_playing = true;
		}
	}

	// Token: 0x06002955 RID: 10581 RVA: 0x000E9D74 File Offset: 0x000E7F74
	private void StopRotateSound()
	{
		if (this.rotate_sound_playing)
		{
			this.looping_sounds.StopSound(this.rotateSound);
			this.rotate_sound_playing = false;
		}
	}

	// Token: 0x040017C0 RID: 6080
	private static HashedString HASH_ROTATION = "rotation";

	// Token: 0x040017C1 RID: 6081
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040017C2 RID: 6082
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x040017C3 RID: 6083
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x040017C4 RID: 6084
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x040017C5 RID: 6085
	[MyCmpReq]
	private MiningSounds mining_sounds;

	// Token: 0x040017C6 RID: 6086
	public int x;

	// Token: 0x040017C7 RID: 6087
	public int y;

	// Token: 0x040017C8 RID: 6088
	public int width;

	// Token: 0x040017C9 RID: 6089
	public int height;

	// Token: 0x040017CA RID: 6090
	public CellOffset vision_offset;

	// Token: 0x040017CB RID: 6091
	private KBatchedAnimController arm_anim_ctrl;

	// Token: 0x040017CC RID: 6092
	private GameObject arm_go;

	// Token: 0x040017CD RID: 6093
	private LoopingSounds looping_sounds;

	// Token: 0x040017CE RID: 6094
	private string rotateSoundName = "AutoMiner_rotate";

	// Token: 0x040017CF RID: 6095
	private EventReference rotateSound;

	// Token: 0x040017D0 RID: 6096
	private KAnimLink link;

	// Token: 0x040017D1 RID: 6097
	private float arm_rot = 45f;

	// Token: 0x040017D2 RID: 6098
	private float turn_rate = 180f;

	// Token: 0x040017D3 RID: 6099
	private bool rotation_complete;

	// Token: 0x040017D4 RID: 6100
	private bool rotate_sound_playing;

	// Token: 0x040017D5 RID: 6101
	private GameObject hitEffectPrefab;

	// Token: 0x040017D6 RID: 6102
	private GameObject hitEffect;

	// Token: 0x040017D7 RID: 6103
	private int dig_cell = Grid.InvalidCell;

	// Token: 0x040017D8 RID: 6104
	private static readonly EventSystem.IntraObjectHandler<AutoMiner> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<AutoMiner>(delegate(AutoMiner component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x02001467 RID: 5223
	public class Instance : GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.GameInstance
	{
		// Token: 0x06008A80 RID: 35456 RVA: 0x003340F0 File Offset: 0x003322F0
		public Instance(AutoMiner master) : base(master)
		{
		}
	}

	// Token: 0x02001468 RID: 5224
	public class States : GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner>
	{
		// Token: 0x06008A81 RID: 35457 RVA: 0x003340FC File Offset: 0x003322FC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (AutoMiner.Instance smi) => smi.GetComponent<Operational>().IsOperational);
			this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (AutoMiner.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.on.idle.PlayAnim("on").EventTransition(GameHashes.ActiveChanged, this.on.moving, (AutoMiner.Instance smi) => smi.GetComponent<Operational>().IsActive);
			this.on.moving.Exit(delegate(AutoMiner.Instance smi)
			{
				smi.master.StopRotateSound();
			}).PlayAnim("working").EventTransition(GameHashes.ActiveChanged, this.on.idle, (AutoMiner.Instance smi) => !smi.GetComponent<Operational>().IsActive).Update(delegate(AutoMiner.Instance smi, float dt)
			{
				smi.master.UpdateRotation(dt);
			}, UpdateRate.SIM_33ms, false).Transition(this.on.digging, new StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Transition.ConditionCallback(AutoMiner.States.RotationComplete), UpdateRate.SIM_200ms);
			this.on.digging.Enter(delegate(AutoMiner.Instance smi)
			{
				smi.master.StartDig();
			}).Exit(delegate(AutoMiner.Instance smi)
			{
				smi.master.StopDig();
			}).PlayAnim("working").EventTransition(GameHashes.ActiveChanged, this.on.idle, (AutoMiner.Instance smi) => !smi.GetComponent<Operational>().IsActive).Update(delegate(AutoMiner.Instance smi, float dt)
			{
				smi.master.UpdateDig(dt);
			}, UpdateRate.SIM_200ms, false).Transition(this.on.moving, GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Not(new StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Transition.ConditionCallback(AutoMiner.States.RotationComplete)), UpdateRate.SIM_200ms);
		}

		// Token: 0x06008A82 RID: 35458 RVA: 0x00334378 File Offset: 0x00332578
		public static bool RotationComplete(AutoMiner.Instance smi)
		{
			return smi.master.RotationComplete;
		}

		// Token: 0x040069AB RID: 27051
		public StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.BoolParameter transferring;

		// Token: 0x040069AC RID: 27052
		public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State off;

		// Token: 0x040069AD RID: 27053
		public AutoMiner.States.ReadyStates on;

		// Token: 0x020024BD RID: 9405
		public class ReadyStates : GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State
		{
			// Token: 0x0400A2EF RID: 41711
			public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State idle;

			// Token: 0x0400A2F0 RID: 41712
			public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State moving;

			// Token: 0x0400A2F1 RID: 41713
			public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State digging;
		}
	}
}
