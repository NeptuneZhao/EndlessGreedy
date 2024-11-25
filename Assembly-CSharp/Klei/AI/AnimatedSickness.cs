using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F56 RID: 3926
	public class AnimatedSickness : Sickness.SicknessComponent
	{
		// Token: 0x060078A8 RID: 30888 RVA: 0x002FC394 File Offset: 0x002FA594
		public AnimatedSickness(HashedString[] kanim_filenames, Expression expression)
		{
			this.kanims = new KAnimFile[kanim_filenames.Length];
			for (int i = 0; i < kanim_filenames.Length; i++)
			{
				this.kanims[i] = Assets.GetAnim(kanim_filenames[i]);
			}
			this.expression = expression;
		}

		// Token: 0x060078A9 RID: 30889 RVA: 0x002FC3E0 File Offset: 0x002FA5E0
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			for (int i = 0; i < this.kanims.Length; i++)
			{
				go.GetComponent<KAnimControllerBase>().AddAnimOverrides(this.kanims[i], 10f);
			}
			if (this.expression != null)
			{
				go.GetComponent<FaceGraph>().AddExpression(this.expression);
			}
			return null;
		}

		// Token: 0x060078AA RID: 30890 RVA: 0x002FC434 File Offset: 0x002FA634
		public override void OnCure(GameObject go, object instace_data)
		{
			if (this.expression != null)
			{
				go.GetComponent<FaceGraph>().RemoveExpression(this.expression);
			}
			for (int i = 0; i < this.kanims.Length; i++)
			{
				go.GetComponent<KAnimControllerBase>().RemoveAnimOverrides(this.kanims[i]);
			}
		}

		// Token: 0x04005A28 RID: 23080
		private KAnimFile[] kanims;

		// Token: 0x04005A29 RID: 23081
		private Expression expression;
	}
}
