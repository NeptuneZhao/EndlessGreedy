using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000773 RID: 1907
public class StickerBomb : StateMachineComponent<StickerBomb.StatesInstance>
{
	// Token: 0x06003385 RID: 13189 RVA: 0x0011A874 File Offset: 0x00118A74
	protected override void OnSpawn()
	{
		if (this.stickerName.IsNullOrWhiteSpace())
		{
			global::Debug.LogError("Missing sticker db entry for " + this.stickerType);
		}
		else
		{
			DbStickerBomb dbStickerBomb = Db.GetStickerBombs().Get(this.stickerName);
			base.GetComponent<KBatchedAnimController>().SwapAnims(new KAnimFile[]
			{
				dbStickerBomb.animFile
			});
		}
		this.cellOffsets = StickerBomb.BuildCellOffsets(base.transform.GetPosition());
		base.smi.destroyTime = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.STICKER_BOMBER.STICKER_DURATION;
		base.smi.StartSM();
		Extents extents = base.GetComponent<OccupyArea>().GetExtents();
		Extents extents2 = new Extents(extents.x - 1, extents.y - 1, extents.width + 2, extents.height + 2);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("StickerBomb.OnSpawn", base.gameObject, extents2, GameScenePartitioner.Instance.objectLayers[2], new Action<object>(this.OnFoundationCellChanged));
		base.OnSpawn();
	}

	// Token: 0x06003386 RID: 13190 RVA: 0x0011A97C File Offset: 0x00118B7C
	[OnDeserialized]
	public void OnDeserialized()
	{
		if (this.stickerName.IsNullOrWhiteSpace() && !this.stickerType.IsNullOrWhiteSpace())
		{
			string[] array = this.stickerType.Split('_', StringSplitOptions.None);
			if (array.Length == 2)
			{
				this.stickerName = array[1];
			}
		}
	}

	// Token: 0x06003387 RID: 13191 RVA: 0x0011A9C1 File Offset: 0x00118BC1
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06003388 RID: 13192 RVA: 0x0011A9D9 File Offset: 0x00118BD9
	private void OnFoundationCellChanged(object data)
	{
		if (!StickerBomb.CanPlaceSticker(this.cellOffsets))
		{
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x06003389 RID: 13193 RVA: 0x0011A9F4 File Offset: 0x00118BF4
	public static List<int> BuildCellOffsets(Vector3 position)
	{
		List<int> list = new List<int>();
		bool flag = position.x % 1f < 0.5f;
		bool flag2 = position.y % 1f > 0.5f;
		int num = Grid.PosToCell(position);
		list.Add(num);
		if (flag)
		{
			list.Add(Grid.CellLeft(num));
			if (flag2)
			{
				list.Add(Grid.CellAbove(num));
				list.Add(Grid.CellUpLeft(num));
			}
			else
			{
				list.Add(Grid.CellBelow(num));
				list.Add(Grid.CellDownLeft(num));
			}
		}
		else
		{
			list.Add(Grid.CellRight(num));
			if (flag2)
			{
				list.Add(Grid.CellAbove(num));
				list.Add(Grid.CellUpRight(num));
			}
			else
			{
				list.Add(Grid.CellBelow(num));
				list.Add(Grid.CellDownRight(num));
			}
		}
		return list;
	}

	// Token: 0x0600338A RID: 13194 RVA: 0x0011AAC4 File Offset: 0x00118CC4
	public static bool CanPlaceSticker(List<int> offsets)
	{
		using (List<int>.Enumerator enumerator = offsets.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (Grid.IsCellOpenToSpace(enumerator.Current))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x0600338B RID: 13195 RVA: 0x0011AB18 File Offset: 0x00118D18
	public void SetStickerType(string newStickerType)
	{
		if (newStickerType == null)
		{
			newStickerType = "sticker";
		}
		DbStickerBomb randomSticker = Db.GetStickerBombs().GetRandomSticker();
		this.stickerName = randomSticker.Id;
		this.stickerType = string.Format("{0}_{1}", newStickerType, randomSticker.Id);
		base.GetComponent<KBatchedAnimController>().SwapAnims(new KAnimFile[]
		{
			randomSticker.animFile
		});
	}

	// Token: 0x04001E88 RID: 7816
	[Serialize]
	public string stickerType;

	// Token: 0x04001E89 RID: 7817
	[Serialize]
	public string stickerName;

	// Token: 0x04001E8A RID: 7818
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001E8B RID: 7819
	private List<int> cellOffsets;

	// Token: 0x0200160F RID: 5647
	public class StatesInstance : GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.GameInstance
	{
		// Token: 0x060090BC RID: 37052 RVA: 0x0034D173 File Offset: 0x0034B373
		public StatesInstance(StickerBomb master) : base(master)
		{
		}

		// Token: 0x060090BD RID: 37053 RVA: 0x0034D17C File Offset: 0x0034B37C
		public string GetStickerAnim(string type)
		{
			return string.Format("{0}_{1}", type, base.master.stickerType);
		}

		// Token: 0x04006E85 RID: 28293
		[Serialize]
		public float destroyTime;
	}

	// Token: 0x02001610 RID: 5648
	public class States : GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb>
	{
		// Token: 0x060090BE RID: 37054 RVA: 0x0034D194 File Offset: 0x0034B394
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Transition(this.destroy, (StickerBomb.StatesInstance smi) => GameClock.Instance.GetTime() >= smi.destroyTime, UpdateRate.SIM_200ms).DefaultState(this.idle);
			this.idle.PlayAnim((StickerBomb.StatesInstance smi) => smi.GetStickerAnim("idle"), KAnim.PlayMode.Once).ScheduleGoTo((StickerBomb.StatesInstance smi) => (float)UnityEngine.Random.Range(20, 30), this.sparkle);
			this.sparkle.PlayAnim((StickerBomb.StatesInstance smi) => smi.GetStickerAnim("sparkle"), KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
			this.destroy.Enter(delegate(StickerBomb.StatesInstance smi)
			{
				Util.KDestroyGameObject(smi.master);
			});
		}

		// Token: 0x04006E86 RID: 28294
		public GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.State destroy;

		// Token: 0x04006E87 RID: 28295
		public GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.State sparkle;

		// Token: 0x04006E88 RID: 28296
		public GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.State idle;
	}
}
