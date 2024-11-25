using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x0200053D RID: 1341
[AddComponentMenu("KMonoBehaviour/Workable/ComplexFabricatorWorkable")]
public class ComplexFabricatorWorkable : Workable
{
	// Token: 0x17000132 RID: 306
	// (get) Token: 0x06001EAF RID: 7855 RVA: 0x000AB379 File Offset: 0x000A9579
	// (set) Token: 0x06001EB0 RID: 7856 RVA: 0x000AB381 File Offset: 0x000A9581
	public StatusItem WorkerStatusItem
	{
		get
		{
			return this.workerStatusItem;
		}
		set
		{
			this.workerStatusItem = value;
		}
	}

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x06001EB1 RID: 7857 RVA: 0x000AB38A File Offset: 0x000A958A
	// (set) Token: 0x06001EB2 RID: 7858 RVA: 0x000AB392 File Offset: 0x000A9592
	public AttributeConverter AttributeConverter
	{
		get
		{
			return this.attributeConverter;
		}
		set
		{
			this.attributeConverter = value;
		}
	}

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x06001EB3 RID: 7859 RVA: 0x000AB39B File Offset: 0x000A959B
	// (set) Token: 0x06001EB4 RID: 7860 RVA: 0x000AB3A3 File Offset: 0x000A95A3
	public float AttributeExperienceMultiplier
	{
		get
		{
			return this.attributeExperienceMultiplier;
		}
		set
		{
			this.attributeExperienceMultiplier = value;
		}
	}

	// Token: 0x17000135 RID: 309
	// (set) Token: 0x06001EB5 RID: 7861 RVA: 0x000AB3AC File Offset: 0x000A95AC
	public string SkillExperienceSkillGroup
	{
		set
		{
			this.skillExperienceSkillGroup = value;
		}
	}

	// Token: 0x17000136 RID: 310
	// (set) Token: 0x06001EB6 RID: 7862 RVA: 0x000AB3B5 File Offset: 0x000A95B5
	public float SkillExperienceMultiplier
	{
		set
		{
			this.skillExperienceMultiplier = value;
		}
	}

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x06001EB7 RID: 7863 RVA: 0x000AB3BE File Offset: 0x000A95BE
	public ComplexRecipe CurrentWorkingOrder
	{
		get
		{
			if (!(this.fabricator != null))
			{
				return null;
			}
			return this.fabricator.CurrentWorkingOrder;
		}
	}

	// Token: 0x06001EB8 RID: 7864 RVA: 0x000AB3DC File Offset: 0x000A95DC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
	}

	// Token: 0x06001EB9 RID: 7865 RVA: 0x000AB44C File Offset: 0x000A964C
	public override string GetConversationTopic()
	{
		string conversationTopic = this.fabricator.GetConversationTopic();
		if (conversationTopic == null)
		{
			return base.GetConversationTopic();
		}
		return conversationTopic;
	}

	// Token: 0x06001EBA RID: 7866 RVA: 0x000AB470 File Offset: 0x000A9670
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (!this.operational.IsOperational)
		{
			return;
		}
		if (this.fabricator.CurrentWorkingOrder != null)
		{
			this.InstantiateVisualizer(this.fabricator.CurrentWorkingOrder);
			this.QueueWorkingAnimations();
			return;
		}
		DebugUtil.DevAssertArgs(false, new object[]
		{
			"ComplexFabricatorWorkable.OnStartWork called but CurrentMachineOrder is null",
			base.gameObject
		});
	}

	// Token: 0x06001EBB RID: 7867 RVA: 0x000AB4D4 File Offset: 0x000A96D4
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.OnWorkTickActions != null)
		{
			this.OnWorkTickActions(worker, dt);
		}
		this.UpdateOrderProgress(worker, dt);
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06001EBC RID: 7868 RVA: 0x000AB4FB File Offset: 0x000A96FB
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (worker != null && this.GetDupeInteract != null)
		{
			worker.GetAnimController().onAnimComplete -= this.PlayNextWorkingAnim;
		}
	}

	// Token: 0x06001EBD RID: 7869 RVA: 0x000AB52C File Offset: 0x000A972C
	public override float GetWorkTime()
	{
		ComplexRecipe currentWorkingOrder = this.fabricator.CurrentWorkingOrder;
		if (currentWorkingOrder != null)
		{
			this.workTime = currentWorkingOrder.time;
			return this.workTime;
		}
		return -1f;
	}

	// Token: 0x06001EBE RID: 7870 RVA: 0x000AB560 File Offset: 0x000A9760
	public Chore CreateWorkChore(ChoreType choreType, float order_progress)
	{
		Chore result = new WorkChore<ComplexFabricatorWorkable>(choreType, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.workTimeRemaining = this.GetWorkTime() * (1f - order_progress);
		return result;
	}

	// Token: 0x06001EBF RID: 7871 RVA: 0x000AB599 File Offset: 0x000A9799
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.fabricator.CompleteWorkingOrder();
		this.DestroyVisualizer();
		base.OnStopWork(worker);
	}

	// Token: 0x06001EC0 RID: 7872 RVA: 0x000AB5BC File Offset: 0x000A97BC
	private void InstantiateVisualizer(ComplexRecipe recipe)
	{
		if (this.visualizer != null)
		{
			this.DestroyVisualizer();
		}
		if (this.visualizerLink != null)
		{
			this.visualizerLink.Unregister();
			this.visualizerLink = null;
		}
		if (recipe.FabricationVisualizer == null)
		{
			return;
		}
		this.visualizer = Util.KInstantiate(recipe.FabricationVisualizer, null, null);
		this.visualizer.transform.parent = this.meter.meterController.transform;
		this.visualizer.transform.SetLocalPosition(new Vector3(0f, 0f, 1f));
		this.visualizer.SetActive(true);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		KBatchedAnimController component2 = this.visualizer.GetComponent<KBatchedAnimController>();
		this.visualizerLink = new KAnimLink(component, component2);
	}

	// Token: 0x06001EC1 RID: 7873 RVA: 0x000AB68C File Offset: 0x000A988C
	private void UpdateOrderProgress(WorkerBase worker, float dt)
	{
		float workTime = this.GetWorkTime();
		float num = Mathf.Clamp01((workTime - base.WorkTimeRemaining) / workTime);
		if (this.fabricator)
		{
			this.fabricator.OrderProgress = num;
		}
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(num);
		}
	}

	// Token: 0x06001EC2 RID: 7874 RVA: 0x000AB6DD File Offset: 0x000A98DD
	private void DestroyVisualizer()
	{
		if (this.visualizer != null)
		{
			if (this.visualizerLink != null)
			{
				this.visualizerLink.Unregister();
				this.visualizerLink = null;
			}
			Util.KDestroyGameObject(this.visualizer);
			this.visualizer = null;
		}
	}

	// Token: 0x06001EC3 RID: 7875 RVA: 0x000AB71C File Offset: 0x000A991C
	public void QueueWorkingAnimations()
	{
		KBatchedAnimController animController = base.worker.GetAnimController();
		if (this.GetDupeInteract != null)
		{
			animController.Queue("working_loop", KAnim.PlayMode.Once, 1f, 0f);
			animController.onAnimComplete += this.PlayNextWorkingAnim;
		}
	}

	// Token: 0x06001EC4 RID: 7876 RVA: 0x000AB76C File Offset: 0x000A996C
	private void PlayNextWorkingAnim(HashedString anim)
	{
		if (base.worker == null)
		{
			return;
		}
		if (this.GetDupeInteract != null)
		{
			KBatchedAnimController animController = base.worker.GetAnimController();
			if (base.worker.GetState() == WorkerBase.State.Working)
			{
				animController.Play(this.GetDupeInteract(), KAnim.PlayMode.Once);
				return;
			}
			animController.onAnimComplete -= this.PlayNextWorkingAnim;
		}
	}

	// Token: 0x04001143 RID: 4419
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001144 RID: 4420
	[MyCmpReq]
	private ComplexFabricator fabricator;

	// Token: 0x04001145 RID: 4421
	public Action<WorkerBase, float> OnWorkTickActions;

	// Token: 0x04001146 RID: 4422
	public MeterController meter;

	// Token: 0x04001147 RID: 4423
	protected GameObject visualizer;

	// Token: 0x04001148 RID: 4424
	protected KAnimLink visualizerLink;

	// Token: 0x04001149 RID: 4425
	public Func<HashedString[]> GetDupeInteract;
}
