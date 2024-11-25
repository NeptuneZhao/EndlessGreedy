using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x0200047F RID: 1151
public class StickerBomber : GameStateMachine<StickerBomber, StickerBomber.Instance>
{
	// Token: 0x060018D8 RID: 6360 RVA: 0x00084840 File Offset: 0x00082A40
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false).Exit(delegate(StickerBomber.Instance smi)
		{
			smi.nextStickerBomb = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.STICKER_BOMBER.TIME_PER_STICKER_BOMB;
		});
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle).ToggleStatusItem(Db.Get().DuplicantStatusItems.JoyResponse_StickerBombing, null);
		this.overjoyed.idle.Transition(this.overjoyed.place_stickers, (StickerBomber.Instance smi) => GameClock.Instance.GetTime() >= smi.nextStickerBomb, UpdateRate.SIM_200ms);
		this.overjoyed.place_stickers.Exit(delegate(StickerBomber.Instance smi)
		{
			smi.nextStickerBomb = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.STICKER_BOMBER.TIME_PER_STICKER_BOMB;
		}).ToggleReactable((StickerBomber.Instance smi) => smi.CreateReactable()).OnSignal(this.doneStickerBomb, this.overjoyed.idle);
	}

	// Token: 0x04000DD0 RID: 3536
	public StateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.Signal doneStickerBomb;

	// Token: 0x04000DD1 RID: 3537
	public GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04000DD2 RID: 3538
	public StickerBomber.OverjoyedStates overjoyed;

	// Token: 0x02001254 RID: 4692
	public class OverjoyedStates : GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040062FF RID: 25343
		public GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04006300 RID: 25344
		public GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State place_stickers;
	}

	// Token: 0x02001255 RID: 4693
	public new class Instance : GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060082DF RID: 33503 RVA: 0x0031D8A7 File Offset: 0x0031BAA7
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060082E0 RID: 33504 RVA: 0x0031D8B0 File Offset: 0x0031BAB0
		public Reactable CreateReactable()
		{
			return new StickerBomber.Instance.StickerBombReactable(base.master.gameObject, base.smi);
		}

		// Token: 0x04006301 RID: 25345
		[Serialize]
		public float nextStickerBomb;

		// Token: 0x020023FF RID: 9215
		private class StickerBombReactable : Reactable
		{
			// Token: 0x0600B88F RID: 47247 RVA: 0x003CF270 File Offset: 0x003CD470
			public StickerBombReactable(GameObject gameObject, StickerBomber.Instance stickerBomber) : base(gameObject, "StickerBombReactable", Db.Get().ChoreTypes.Build, 2, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
			{
				this.preventChoreInterruption = true;
				this.stickerBomber = stickerBomber;
			}

			// Token: 0x0600B890 RID: 47248 RVA: 0x003CF350 File Offset: 0x003CD550
			public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
			{
				if (this.reactor != null)
				{
					return false;
				}
				if (new_reactor == null)
				{
					return false;
				}
				if (this.gameObject != new_reactor)
				{
					return false;
				}
				Navigator component = new_reactor.GetComponent<Navigator>();
				return !(component == null) && component.CurrentNavType != NavType.Tube && component.CurrentNavType != NavType.Ladder && component.CurrentNavType != NavType.Pole;
			}

			// Token: 0x0600B891 RID: 47249 RVA: 0x003CF3B8 File Offset: 0x003CD5B8
			protected override void InternalBegin()
			{
				this.stickersToPlace = UnityEngine.Random.Range(4, 6);
				this.STICKER_PLACE_TIMER = this.TIME_PER_STICKER_PLACED;
				this.placementCell = this.FindPlacementCell();
				if (this.placementCell == 0)
				{
					base.End();
					return;
				}
				this.kbac = this.reactor.GetComponent<KBatchedAnimController>();
				this.kbac.AddAnimOverrides(this.animset, 0f);
				this.kbac.Play(this.pre_anim, KAnim.PlayMode.Once, 1f, 0f);
				this.kbac.Queue(this.loop_anim, KAnim.PlayMode.Loop, 1f, 0f);
			}

			// Token: 0x0600B892 RID: 47250 RVA: 0x003CF458 File Offset: 0x003CD658
			public override void Update(float dt)
			{
				this.STICKER_PLACE_TIMER -= dt;
				if (this.STICKER_PLACE_TIMER <= 0f)
				{
					this.PlaceSticker();
					this.STICKER_PLACE_TIMER = this.TIME_PER_STICKER_PLACED;
				}
				if (this.stickersPlaced >= this.stickersToPlace)
				{
					this.kbac.Play(this.pst_anim, KAnim.PlayMode.Once, 1f, 0f);
					base.End();
				}
			}

			// Token: 0x0600B893 RID: 47251 RVA: 0x003CF4C4 File Offset: 0x003CD6C4
			protected override void InternalEnd()
			{
				if (this.kbac != null)
				{
					this.kbac.RemoveAnimOverrides(this.animset);
					this.kbac = null;
				}
				this.stickerBomber.sm.doneStickerBomb.Trigger(this.stickerBomber);
				this.stickersPlaced = 0;
			}

			// Token: 0x0600B894 RID: 47252 RVA: 0x003CF51C File Offset: 0x003CD71C
			private int FindPlacementCell()
			{
				int cell = Grid.PosToCell(this.reactor.transform.GetPosition() + Vector3.up);
				ListPool<int, PathFinder>.PooledList pooledList = ListPool<int, PathFinder>.Allocate();
				ListPool<int, PathFinder>.PooledList pooledList2 = ListPool<int, PathFinder>.Allocate();
				QueuePool<GameUtil.FloodFillInfo, Comet>.PooledQueue pooledQueue = QueuePool<GameUtil.FloodFillInfo, Comet>.Allocate();
				pooledQueue.Enqueue(new GameUtil.FloodFillInfo
				{
					cell = cell,
					depth = 0
				});
				GameUtil.FloodFillConditional(pooledQueue, this.canPlaceStickerCb, pooledList, pooledList2, 2);
				if (pooledList2.Count > 0)
				{
					int random = pooledList2.GetRandom<int>();
					pooledList.Recycle();
					pooledList2.Recycle();
					pooledQueue.Recycle();
					return random;
				}
				pooledList.Recycle();
				pooledList2.Recycle();
				pooledQueue.Recycle();
				return 0;
			}

			// Token: 0x0600B895 RID: 47253 RVA: 0x003CF5C0 File Offset: 0x003CD7C0
			private void PlaceSticker()
			{
				this.stickersPlaced++;
				Vector3 a = Grid.CellToPos(this.placementCell);
				int i = 10;
				while (i > 0)
				{
					i--;
					Vector3 position = a + new Vector3(UnityEngine.Random.Range(-this.tile_random_range, this.tile_random_range), UnityEngine.Random.Range(-this.tile_random_range, this.tile_random_range), -2.5f);
					if (StickerBomb.CanPlaceSticker(StickerBomb.BuildCellOffsets(position)))
					{
						GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("StickerBomb".ToTag()), position, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-this.tile_random_rotation, this.tile_random_rotation)), null, null, true, 0);
						StickerBomb component = gameObject.GetComponent<StickerBomb>();
						string stickerType = this.reactor.GetComponent<MinionIdentity>().stickerType;
						component.SetStickerType(stickerType);
						gameObject.SetActive(true);
						i = 0;
					}
				}
			}

			// Token: 0x0600B896 RID: 47254 RVA: 0x003CF69B File Offset: 0x003CD89B
			protected override void InternalCleanup()
			{
			}

			// Token: 0x0400A0B2 RID: 41138
			private int stickersToPlace;

			// Token: 0x0400A0B3 RID: 41139
			private int stickersPlaced;

			// Token: 0x0400A0B4 RID: 41140
			private int placementCell;

			// Token: 0x0400A0B5 RID: 41141
			private float tile_random_range = 1f;

			// Token: 0x0400A0B6 RID: 41142
			private float tile_random_rotation = 90f;

			// Token: 0x0400A0B7 RID: 41143
			private float TIME_PER_STICKER_PLACED = 0.66f;

			// Token: 0x0400A0B8 RID: 41144
			private float STICKER_PLACE_TIMER;

			// Token: 0x0400A0B9 RID: 41145
			private KBatchedAnimController kbac;

			// Token: 0x0400A0BA RID: 41146
			private KAnimFile animset = Assets.GetAnim("anim_stickers_kanim");

			// Token: 0x0400A0BB RID: 41147
			private HashedString pre_anim = "working_pre";

			// Token: 0x0400A0BC RID: 41148
			private HashedString loop_anim = "working_loop";

			// Token: 0x0400A0BD RID: 41149
			private HashedString pst_anim = "working_pst";

			// Token: 0x0400A0BE RID: 41150
			private StickerBomber.Instance stickerBomber;

			// Token: 0x0400A0BF RID: 41151
			private Func<int, bool> canPlaceStickerCb = (int cell) => !Grid.Solid[cell] && (!Grid.IsValidCell(Grid.CellLeft(cell)) || !Grid.Solid[Grid.CellLeft(cell)]) && (!Grid.IsValidCell(Grid.CellRight(cell)) || !Grid.Solid[Grid.CellRight(cell)]) && (!Grid.IsValidCell(Grid.OffsetCell(cell, 0, 1)) || !Grid.Solid[Grid.OffsetCell(cell, 0, 1)]) && (!Grid.IsValidCell(Grid.OffsetCell(cell, 0, -1)) || !Grid.Solid[Grid.OffsetCell(cell, 0, -1)]) && !Grid.IsCellOpenToSpace(cell);
		}
	}
}
