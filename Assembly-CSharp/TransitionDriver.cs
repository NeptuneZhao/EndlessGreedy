using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005F0 RID: 1520
public class TransitionDriver
{
	// Token: 0x170001AF RID: 431
	// (get) Token: 0x060024C2 RID: 9410 RVA: 0x000CCBC3 File Offset: 0x000CADC3
	public Navigator.ActiveTransition GetTransition
	{
		get
		{
			return this.transition;
		}
	}

	// Token: 0x060024C3 RID: 9411 RVA: 0x000CCBCB File Offset: 0x000CADCB
	public TransitionDriver(Navigator navigator)
	{
		this.log = new LoggerFS("TransitionDriver", 35);
	}

	// Token: 0x060024C4 RID: 9412 RVA: 0x000CCBFC File Offset: 0x000CADFC
	public void BeginTransition(Navigator navigator, NavGrid.Transition transition, float defaultSpeed)
	{
		Navigator.ActiveTransition instance = TransitionDriver.TransitionPool.GetInstance();
		instance.Init(transition, defaultSpeed);
		this.BeginTransition(navigator, instance);
	}

	// Token: 0x060024C5 RID: 9413 RVA: 0x000CCC24 File Offset: 0x000CAE24
	private void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		bool flag = this.interruptOverrideStack.Count != 0;
		foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
		{
			if (!flag || !(overrideLayer is TransitionDriver.InterruptOverrideLayer))
			{
				overrideLayer.BeginTransition(navigator, transition);
			}
		}
		this.navigator = navigator;
		this.transition = transition;
		this.isComplete = false;
		Grid.SceneLayer sceneLayer = navigator.sceneLayer;
		if (transition.navGridTransition.start == NavType.Tube || transition.navGridTransition.end == NavType.Tube)
		{
			sceneLayer = Grid.SceneLayer.BuildingUse;
		}
		else if (transition.navGridTransition.start == NavType.Solid && transition.navGridTransition.end == NavType.Solid)
		{
			sceneLayer = Grid.SceneLayer.FXFront;
			navigator.animController.SetSceneLayer(sceneLayer);
		}
		else if (transition.navGridTransition.start == NavType.Solid || transition.navGridTransition.end == NavType.Solid)
		{
			navigator.animController.SetSceneLayer(sceneLayer);
		}
		int cell = Grid.OffsetCell(Grid.PosToCell(navigator), transition.x, transition.y);
		this.targetPos = Grid.CellToPosCBC(cell, sceneLayer);
		if (transition.isLooping)
		{
			KAnimControllerBase animController = navigator.animController;
			animController.PlaySpeedMultiplier = transition.animSpeed;
			bool flag2 = transition.preAnim != "";
			bool flag3 = animController.CurrentAnim != null && animController.CurrentAnim.name == transition.anim;
			if (flag2 && animController.CurrentAnim != null && animController.CurrentAnim.name == transition.preAnim)
			{
				animController.ClearQueue();
				animController.Queue(transition.anim, KAnim.PlayMode.Loop, 1f, 0f);
			}
			else if (flag3)
			{
				if (animController.PlayMode != KAnim.PlayMode.Loop)
				{
					animController.ClearQueue();
					animController.Queue(transition.anim, KAnim.PlayMode.Loop, 1f, 0f);
				}
			}
			else if (flag2)
			{
				animController.Play(transition.preAnim, KAnim.PlayMode.Once, 1f, 0f);
				animController.Queue(transition.anim, KAnim.PlayMode.Loop, 1f, 0f);
			}
			else
			{
				animController.Play(transition.anim, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}
		else if (transition.anim != null)
		{
			KBatchedAnimController animController2 = navigator.animController;
			animController2.PlaySpeedMultiplier = transition.animSpeed;
			animController2.Play(transition.anim, KAnim.PlayMode.Once, 1f, 0f);
			navigator.Subscribe(-1061186183, new Action<object>(this.OnAnimComplete));
		}
		if (transition.navGridTransition.y != 0)
		{
			if (transition.navGridTransition.start == NavType.RightWall)
			{
				navigator.facing.SetFacing(transition.navGridTransition.y < 0);
			}
			else if (transition.navGridTransition.start == NavType.LeftWall)
			{
				navigator.facing.SetFacing(transition.navGridTransition.y > 0);
			}
		}
		if (transition.navGridTransition.x != 0)
		{
			if (transition.navGridTransition.start == NavType.Ceiling)
			{
				navigator.facing.SetFacing(transition.navGridTransition.x > 0);
			}
			else if (transition.navGridTransition.start != NavType.LeftWall && transition.navGridTransition.start != NavType.RightWall)
			{
				navigator.facing.SetFacing(transition.navGridTransition.x < 0);
			}
		}
		this.brain = navigator.GetComponent<Brain>();
	}

	// Token: 0x060024C6 RID: 9414 RVA: 0x000CCFB0 File Offset: 0x000CB1B0
	public void UpdateTransition(float dt)
	{
		if (this.navigator == null)
		{
			return;
		}
		foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
		{
			bool flag = this.interruptOverrideStack.Count != 0;
			bool flag2 = overrideLayer is TransitionDriver.InterruptOverrideLayer;
			if (!flag || !flag2 || this.interruptOverrideStack.Peek() == overrideLayer)
			{
				overrideLayer.UpdateTransition(this.navigator, this.transition);
			}
		}
		if (!this.isComplete && this.transition.isCompleteCB != null)
		{
			this.isComplete = this.transition.isCompleteCB();
		}
		if (this.brain != null)
		{
			bool flag3 = this.isComplete;
		}
		if (this.transition.isLooping)
		{
			float speed = this.transition.speed;
			Vector3 position = this.navigator.transform.GetPosition();
			int num = Grid.PosToCell(position);
			if (this.transition.x > 0)
			{
				position.x += dt * speed;
				if (position.x > this.targetPos.x)
				{
					this.isComplete = true;
				}
			}
			else if (this.transition.x < 0)
			{
				position.x -= dt * speed;
				if (position.x < this.targetPos.x)
				{
					this.isComplete = true;
				}
			}
			else
			{
				position.x = this.targetPos.x;
			}
			if (this.transition.y > 0)
			{
				position.y += dt * speed;
				if (position.y > this.targetPos.y)
				{
					this.isComplete = true;
				}
			}
			else if (this.transition.y < 0)
			{
				position.y -= dt * speed;
				if (position.y < this.targetPos.y)
				{
					this.isComplete = true;
				}
			}
			else
			{
				position.y = this.targetPos.y;
			}
			this.navigator.transform.SetPosition(position);
			int num2 = Grid.PosToCell(position);
			if (num2 != num)
			{
				this.navigator.Trigger(915392638, num2);
			}
		}
		if (this.isComplete)
		{
			this.isComplete = false;
			Navigator navigator = this.navigator;
			navigator.SetCurrentNavType(this.transition.end);
			navigator.transform.SetPosition(this.targetPos);
			this.EndTransition();
			navigator.AdvancePath(true);
		}
	}

	// Token: 0x060024C7 RID: 9415 RVA: 0x000CD254 File Offset: 0x000CB454
	public void EndTransition()
	{
		if (this.navigator != null)
		{
			this.interruptOverrideStack.Clear();
			foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
			{
				overrideLayer.EndTransition(this.navigator, this.transition);
			}
			this.navigator.animController.PlaySpeedMultiplier = 1f;
			this.navigator.Unsubscribe(-1061186183, new Action<object>(this.OnAnimComplete));
			if (this.brain != null)
			{
				this.brain.Resume("move_handler");
			}
			TransitionDriver.TransitionPool.ReleaseInstance(this.transition);
			this.transition = null;
			this.navigator = null;
			this.brain = null;
		}
	}

	// Token: 0x060024C8 RID: 9416 RVA: 0x000CD340 File Offset: 0x000CB540
	private void OnAnimComplete(object data)
	{
		if (this.navigator != null)
		{
			this.navigator.Unsubscribe(-1061186183, new Action<object>(this.OnAnimComplete));
		}
		this.isComplete = true;
	}

	// Token: 0x060024C9 RID: 9417 RVA: 0x000CD373 File Offset: 0x000CB573
	public static Navigator.ActiveTransition SwapTransitionWithEmpty(Navigator.ActiveTransition src)
	{
		Navigator.ActiveTransition instance = TransitionDriver.TransitionPool.GetInstance();
		instance.Copy(src);
		src.Copy(TransitionDriver.emptyTransition);
		return instance;
	}

	// Token: 0x040014CF RID: 5327
	private static Navigator.ActiveTransition emptyTransition = new Navigator.ActiveTransition();

	// Token: 0x040014D0 RID: 5328
	public static ObjectPool<Navigator.ActiveTransition> TransitionPool = new ObjectPool<Navigator.ActiveTransition>(() => new Navigator.ActiveTransition(), 128);

	// Token: 0x040014D1 RID: 5329
	private Stack<TransitionDriver.InterruptOverrideLayer> interruptOverrideStack = new Stack<TransitionDriver.InterruptOverrideLayer>(8);

	// Token: 0x040014D2 RID: 5330
	private Navigator.ActiveTransition transition;

	// Token: 0x040014D3 RID: 5331
	private Navigator navigator;

	// Token: 0x040014D4 RID: 5332
	private Vector3 targetPos;

	// Token: 0x040014D5 RID: 5333
	private bool isComplete;

	// Token: 0x040014D6 RID: 5334
	private Brain brain;

	// Token: 0x040014D7 RID: 5335
	public List<TransitionDriver.OverrideLayer> overrideLayers = new List<TransitionDriver.OverrideLayer>();

	// Token: 0x040014D8 RID: 5336
	private LoggerFS log;

	// Token: 0x020013D1 RID: 5073
	public class OverrideLayer
	{
		// Token: 0x06008869 RID: 34921 RVA: 0x0032E626 File Offset: 0x0032C826
		public OverrideLayer(Navigator navigator)
		{
		}

		// Token: 0x0600886A RID: 34922 RVA: 0x0032E62E File Offset: 0x0032C82E
		public virtual void Destroy()
		{
		}

		// Token: 0x0600886B RID: 34923 RVA: 0x0032E630 File Offset: 0x0032C830
		public virtual void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
		}

		// Token: 0x0600886C RID: 34924 RVA: 0x0032E632 File Offset: 0x0032C832
		public virtual void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
		}

		// Token: 0x0600886D RID: 34925 RVA: 0x0032E634 File Offset: 0x0032C834
		public virtual void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
		}
	}

	// Token: 0x020013D2 RID: 5074
	public class InterruptOverrideLayer : TransitionDriver.OverrideLayer
	{
		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x0600886E RID: 34926 RVA: 0x0032E636 File Offset: 0x0032C836
		protected bool InterruptInProgress
		{
			get
			{
				return this.originalTransition != null;
			}
		}

		// Token: 0x0600886F RID: 34927 RVA: 0x0032E641 File Offset: 0x0032C841
		public InterruptOverrideLayer(Navigator navigator) : base(navigator)
		{
			this.driver = navigator.transitionDriver;
		}

		// Token: 0x06008870 RID: 34928 RVA: 0x0032E656 File Offset: 0x0032C856
		public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
			this.driver.interruptOverrideStack.Push(this);
			this.originalTransition = TransitionDriver.SwapTransitionWithEmpty(transition);
		}

		// Token: 0x06008871 RID: 34929 RVA: 0x0032E678 File Offset: 0x0032C878
		public override void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
			if (!this.IsOverrideComplete())
			{
				return;
			}
			this.driver.interruptOverrideStack.Pop();
			transition.Copy(this.originalTransition);
			TransitionDriver.TransitionPool.ReleaseInstance(this.originalTransition);
			this.originalTransition = null;
			this.EndTransition(navigator, transition);
			this.driver.BeginTransition(navigator, transition);
		}

		// Token: 0x06008872 RID: 34930 RVA: 0x0032E6D7 File Offset: 0x0032C8D7
		public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
			base.EndTransition(navigator, transition);
			if (this.originalTransition == null)
			{
				return;
			}
			TransitionDriver.TransitionPool.ReleaseInstance(this.originalTransition);
			this.originalTransition = null;
		}

		// Token: 0x06008873 RID: 34931 RVA: 0x0032E701 File Offset: 0x0032C901
		protected virtual bool IsOverrideComplete()
		{
			return this.originalTransition != null && this.driver.interruptOverrideStack.Count != 0 && this.driver.interruptOverrideStack.Peek() == this;
		}

		// Token: 0x040067F3 RID: 26611
		protected Navigator.ActiveTransition originalTransition;

		// Token: 0x040067F4 RID: 26612
		protected TransitionDriver driver;
	}
}
