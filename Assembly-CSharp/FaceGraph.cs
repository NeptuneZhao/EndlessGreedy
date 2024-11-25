using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000961 RID: 2401
[AddComponentMenu("KMonoBehaviour/scripts/FaceGraph")]
public class FaceGraph : KMonoBehaviour
{
	// Token: 0x0600462B RID: 17963 RVA: 0x0019015D File Offset: 0x0018E35D
	public IEnumerator<Expression> GetEnumerator()
	{
		return this.expressions.GetEnumerator();
	}

	// Token: 0x1700050A RID: 1290
	// (get) Token: 0x0600462C RID: 17964 RVA: 0x0019016F File Offset: 0x0018E36F
	// (set) Token: 0x0600462D RID: 17965 RVA: 0x00190177 File Offset: 0x0018E377
	public Expression overrideExpression { get; private set; }

	// Token: 0x1700050B RID: 1291
	// (get) Token: 0x0600462E RID: 17966 RVA: 0x00190180 File Offset: 0x0018E380
	// (set) Token: 0x0600462F RID: 17967 RVA: 0x00190188 File Offset: 0x0018E388
	public Expression currentExpression { get; private set; }

	// Token: 0x06004630 RID: 17968 RVA: 0x00190191 File Offset: 0x0018E391
	public void AddExpression(Expression expression)
	{
		if (this.expressions.Contains(expression))
		{
			return;
		}
		this.expressions.Add(expression);
		this.UpdateFace();
	}

	// Token: 0x06004631 RID: 17969 RVA: 0x001901B4 File Offset: 0x0018E3B4
	public void RemoveExpression(Expression expression)
	{
		if (this.expressions.Remove(expression))
		{
			this.UpdateFace();
		}
	}

	// Token: 0x06004632 RID: 17970 RVA: 0x001901CA File Offset: 0x0018E3CA
	public void SetOverrideExpression(Expression expression)
	{
		if (expression != this.overrideExpression)
		{
			this.overrideExpression = expression;
			this.UpdateFace();
		}
	}

	// Token: 0x06004633 RID: 17971 RVA: 0x001901E4 File Offset: 0x0018E3E4
	public void ApplyShape()
	{
		KAnimFile anim = Assets.GetAnim(FaceGraph.HASH_HEAD_MASTER_SWAP_KANIM);
		bool should_use_sideways_symbol = this.ShouldUseSidewaysSymbol(this.m_controller);
		if (this.m_blinkMonitor == null)
		{
			Accessory accessory = this.m_accessorizer.GetAccessory(Db.Get().AccessorySlots.Eyes);
			this.m_blinkMonitor = this.m_accessorizer.GetSMI<BlinkMonitor.Instance>();
			if (this.m_blinkMonitor != null)
			{
				this.m_blinkMonitor.eye_anim = accessory.Name;
			}
		}
		if (this.m_speechMonitor == null)
		{
			this.m_speechMonitor = this.m_accessorizer.GetSMI<SpeechMonitor.Instance>();
		}
		if (this.m_blinkMonitor.IsNullOrStopped() || !this.m_blinkMonitor.IsBlinking())
		{
			KAnim.Build.Symbol symbol = this.m_accessorizer.GetAccessory(Db.Get().AccessorySlots.Eyes).symbol;
			this.ApplyShape(symbol, this.m_controller, anim, FaceGraph.ANIM_HASH_SNAPTO_EYES, should_use_sideways_symbol);
		}
		if (this.m_speechMonitor.IsNullOrStopped() || !this.m_speechMonitor.IsPlayingSpeech())
		{
			KAnim.Build.Symbol symbol2 = this.m_accessorizer.GetAccessory(Db.Get().AccessorySlots.Mouth).symbol;
			this.ApplyShape(symbol2, this.m_controller, anim, FaceGraph.ANIM_HASH_SNAPTO_MOUTH, should_use_sideways_symbol);
			return;
		}
		this.m_speechMonitor.DrawMouth();
	}

	// Token: 0x06004634 RID: 17972 RVA: 0x0019031C File Offset: 0x0018E51C
	private bool ShouldUseSidewaysSymbol(KBatchedAnimController controller)
	{
		KAnim.Anim currentAnim = controller.GetCurrentAnim();
		if (currentAnim == null)
		{
			return false;
		}
		int currentFrameIndex = controller.GetCurrentFrameIndex();
		if (currentFrameIndex <= 0)
		{
			return false;
		}
		KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(currentAnim.animFile.animBatchTag);
		KAnim.Anim.Frame frame;
		batchGroupData.TryGetFrame(currentFrameIndex, out frame);
		for (int i = 0; i < frame.numElements; i++)
		{
			KAnim.Anim.FrameElement frameElement = batchGroupData.GetFrameElement(frame.firstElementIdx + i);
			if (frameElement.symbol == FaceGraph.ANIM_HASH_SNAPTO_EYES && frameElement.frame >= FaceGraph.FIRST_SIDEWAYS_FRAME)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004635 RID: 17973 RVA: 0x001903AC File Offset: 0x0018E5AC
	private void ApplyShape(KAnim.Build.Symbol variation_symbol, KBatchedAnimController controller, KAnimFile shapes_file, KAnimHashedString symbol_name_in_shape_file, bool should_use_sideways_symbol)
	{
		HashedString hashedString = FaceGraph.ANIM_HASH_NEUTRAL;
		if (this.currentExpression != null)
		{
			hashedString = this.currentExpression.face.hash;
		}
		KAnim.Anim anim = null;
		KAnim.Anim.FrameElement frameElement = default(KAnim.Anim.FrameElement);
		bool flag = false;
		bool flag2 = false;
		int num = 0;
		while (num < shapes_file.GetData().animCount && !flag)
		{
			KAnim.Anim anim2 = shapes_file.GetData().GetAnim(num);
			if (anim2.hash == hashedString)
			{
				anim = anim2;
				KAnim.Anim.Frame frame;
				if (anim.TryGetFrame(shapes_file.GetData().build.batchTag, 0, out frame))
				{
					for (int i = 0; i < frame.numElements; i++)
					{
						frameElement = KAnimBatchManager.Instance().GetBatchGroupData(shapes_file.GetData().animBatchTag).GetFrameElement(frame.firstElementIdx + i);
						if (!(frameElement.symbol != symbol_name_in_shape_file))
						{
							if (flag2 || !should_use_sideways_symbol)
							{
								flag = true;
							}
							flag2 = true;
							break;
						}
					}
				}
			}
			num++;
		}
		if (anim == null)
		{
			DebugUtil.Assert(false, "Could not find shape for expression: " + HashCache.Get().Get(hashedString));
		}
		if (!flag2)
		{
			DebugUtil.Assert(false, "Could not find shape element for shape:" + HashCache.Get().Get(variation_symbol.hash));
		}
		KAnim.Build.Symbol symbol = KAnimBatchManager.Instance().GetBatchGroupData(controller.batchGroupID).GetSymbol(symbol_name_in_shape_file);
		KAnim.Build.SymbolFrameInstance symbolFrameInstance = KAnimBatchManager.Instance().GetBatchGroupData(variation_symbol.build.batchTag).symbolFrameInstances[variation_symbol.firstFrameIdx + frameElement.frame];
		symbolFrameInstance.buildImageIdx = this.m_symbolOverrideController.GetAtlasIdx(variation_symbol.build.GetTexture(0));
		controller.SetSymbolOverride(symbol.firstFrameIdx, ref symbolFrameInstance);
	}

	// Token: 0x06004636 RID: 17974 RVA: 0x0019055C File Offset: 0x0018E75C
	private void UpdateFace()
	{
		Expression expression = null;
		if (this.overrideExpression != null)
		{
			expression = this.overrideExpression;
		}
		else if (this.expressions.Count > 0)
		{
			this.expressions.Sort((Expression a, Expression b) => b.priority.CompareTo(a.priority));
			expression = this.expressions[0];
		}
		if (expression != this.currentExpression || expression == null)
		{
			this.currentExpression = expression;
			this.m_symbolOverrideController.MarkDirty();
		}
		AccessorySlot headEffects = Db.Get().AccessorySlots.HeadEffects;
		if (this.currentExpression != null)
		{
			Accessory accessory = this.m_accessorizer.GetAccessory(Db.Get().AccessorySlots.HeadEffects);
			HashedString hashedString = HashedString.Invalid;
			foreach (Expression expression2 in this.expressions)
			{
				if (expression2.face.headFXHash.IsValid)
				{
					hashedString = expression2.face.headFXHash;
					break;
				}
			}
			Accessory accessory2 = (hashedString != HashedString.Invalid) ? headEffects.Lookup(hashedString) : null;
			if (accessory != accessory2)
			{
				if (accessory != null)
				{
					this.m_accessorizer.RemoveAccessory(accessory);
				}
				if (accessory2 != null)
				{
					this.m_accessorizer.AddAccessory(accessory2);
				}
			}
			this.m_controller.SetSymbolVisiblity(headEffects.targetSymbolId, accessory2 != null);
			return;
		}
		this.m_controller.SetSymbolVisiblity(headEffects.targetSymbolId, false);
	}

	// Token: 0x04002DAA RID: 11690
	private List<Expression> expressions = new List<Expression>();

	// Token: 0x04002DAD RID: 11693
	[MyCmpGet]
	private KBatchedAnimController m_controller;

	// Token: 0x04002DAE RID: 11694
	[MyCmpGet]
	private Accessorizer m_accessorizer;

	// Token: 0x04002DAF RID: 11695
	[MyCmpGet]
	private SymbolOverrideController m_symbolOverrideController;

	// Token: 0x04002DB0 RID: 11696
	private BlinkMonitor.Instance m_blinkMonitor;

	// Token: 0x04002DB1 RID: 11697
	private SpeechMonitor.Instance m_speechMonitor;

	// Token: 0x04002DB2 RID: 11698
	private static HashedString HASH_HEAD_MASTER_SWAP_KANIM = "head_master_swap_kanim";

	// Token: 0x04002DB3 RID: 11699
	private static KAnimHashedString ANIM_HASH_SNAPTO_EYES = "snapto_eyes";

	// Token: 0x04002DB4 RID: 11700
	private static KAnimHashedString ANIM_HASH_SNAPTO_MOUTH = "snapto_mouth";

	// Token: 0x04002DB5 RID: 11701
	private static KAnimHashedString ANIM_HASH_NEUTRAL = "neutral";

	// Token: 0x04002DB6 RID: 11702
	private static int FIRST_SIDEWAYS_FRAME = 29;
}
