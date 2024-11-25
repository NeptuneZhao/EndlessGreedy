using System;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x0200073A RID: 1850
[AddComponentMenu("KMonoBehaviour/scripts/MonumentPart")]
public class MonumentPart : KMonoBehaviour
{
	// Token: 0x06003125 RID: 12581 RVA: 0x0010F5BC File Offset: 0x0010D7BC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.MonumentParts.Add(this);
		if (!string.IsNullOrEmpty(this.chosenState))
		{
			this.SetState(this.chosenState);
		}
		this.UpdateMonumentDecor();
	}

	// Token: 0x06003126 RID: 12582 RVA: 0x0010F5F0 File Offset: 0x0010D7F0
	[OnDeserialized]
	private void OnDeserializedMethod()
	{
		if (Db.GetMonumentParts().TryGet(this.chosenState) == null)
		{
			string id = "";
			if (this.part == MonumentPartResource.Part.Bottom)
			{
				id = "bottom_" + this.chosenState;
			}
			else if (this.part == MonumentPartResource.Part.Middle)
			{
				id = "mid_" + this.chosenState;
			}
			else if (this.part == MonumentPartResource.Part.Top)
			{
				id = "top_" + this.chosenState;
			}
			if (Db.GetMonumentParts().TryGet(id) != null)
			{
				this.chosenState = id;
			}
		}
	}

	// Token: 0x06003127 RID: 12583 RVA: 0x0010F67A File Offset: 0x0010D87A
	protected override void OnCleanUp()
	{
		Components.MonumentParts.Remove(this);
		this.RemoveMonumentPiece();
		base.OnCleanUp();
	}

	// Token: 0x06003128 RID: 12584 RVA: 0x0010F694 File Offset: 0x0010D894
	public void SetState(string state)
	{
		MonumentPartResource monumentPartResource = Db.GetMonumentParts().Get(state);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.SwapAnims(new KAnimFile[]
		{
			monumentPartResource.AnimFile
		});
		component.Play(monumentPartResource.State, KAnim.PlayMode.Once, 1f, 0f);
		this.chosenState = state;
	}

	// Token: 0x06003129 RID: 12585 RVA: 0x0010F6EC File Offset: 0x0010D8EC
	public bool IsMonumentCompleted()
	{
		bool flag = this.GetMonumentPart(MonumentPartResource.Part.Top) != null;
		bool flag2 = this.GetMonumentPart(MonumentPartResource.Part.Middle) != null;
		bool flag3 = this.GetMonumentPart(MonumentPartResource.Part.Bottom) != null;
		return flag && flag3 && flag2;
	}

	// Token: 0x0600312A RID: 12586 RVA: 0x0010F728 File Offset: 0x0010D928
	public void UpdateMonumentDecor()
	{
		GameObject monumentPart = this.GetMonumentPart(MonumentPartResource.Part.Middle);
		if (this.IsMonumentCompleted())
		{
			monumentPart.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.BONUS.MONUMENT.COMPLETE);
			foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
			{
				if (gameObject != monumentPart)
				{
					gameObject.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.NONE);
				}
			}
		}
	}

	// Token: 0x0600312B RID: 12587 RVA: 0x0010F7B4 File Offset: 0x0010D9B4
	public void RemoveMonumentPiece()
	{
		if (this.IsMonumentCompleted())
		{
			foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
			{
				if (gameObject.GetComponent<MonumentPart>() != this)
				{
					gameObject.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.BONUS.MONUMENT.INCOMPLETE);
				}
			}
		}
	}

	// Token: 0x0600312C RID: 12588 RVA: 0x0010F82C File Offset: 0x0010DA2C
	private GameObject GetMonumentPart(MonumentPartResource.Part requestPart)
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
		{
			MonumentPart component = gameObject.GetComponent<MonumentPart>();
			if (!(component == null) && component.part == requestPart)
			{
				return gameObject;
			}
		}
		return null;
	}

	// Token: 0x04001CE8 RID: 7400
	public MonumentPartResource.Part part;

	// Token: 0x04001CE9 RID: 7401
	public string stateUISymbol;

	// Token: 0x04001CEA RID: 7402
	[Serialize]
	private string chosenState;
}
