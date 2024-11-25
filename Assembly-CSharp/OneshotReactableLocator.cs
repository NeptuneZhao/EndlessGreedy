using System;
using UnityEngine;

// Token: 0x020002D9 RID: 729
public class OneshotReactableLocator : IEntityConfig
{
	// Token: 0x06000F3C RID: 3900 RVA: 0x0005847C File Offset: 0x0005667C
	public static EmoteReactable CreateOneshotReactable(GameObject source, float lifetime, string id, ChoreType chore_type, int range_width = 15, int range_height = 15, float min_reactor_time = 20f)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(OneshotReactableLocator.ID), source.transform.GetPosition());
		EmoteReactable emoteReactable = new EmoteReactable(gameObject, id, chore_type, range_width, range_height, 100000f, min_reactor_time, float.PositiveInfinity, 0f);
		emoteReactable.AddPrecondition(OneshotReactableLocator.ReactorIsNotSource(source));
		OneshotReactableHost component = gameObject.GetComponent<OneshotReactableHost>();
		component.lifetime = lifetime;
		component.SetReactable(emoteReactable);
		gameObject.SetActive(true);
		return emoteReactable;
	}

	// Token: 0x06000F3D RID: 3901 RVA: 0x000584F2 File Offset: 0x000566F2
	private static Reactable.ReactablePrecondition ReactorIsNotSource(GameObject source)
	{
		return (GameObject reactor, Navigator.ActiveTransition transition) => reactor != source;
	}

	// Token: 0x06000F3E RID: 3902 RVA: 0x0005850B File Offset: 0x0005670B
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F3F RID: 3903 RVA: 0x00058512 File Offset: 0x00056712
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(OneshotReactableLocator.ID, OneshotReactableLocator.ID, false);
		gameObject.AddTag(GameTags.NotConversationTopic);
		gameObject.AddOrGet<OneshotReactableHost>();
		return gameObject;
	}

	// Token: 0x06000F40 RID: 3904 RVA: 0x00058536 File Offset: 0x00056736
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06000F41 RID: 3905 RVA: 0x00058538 File Offset: 0x00056738
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000958 RID: 2392
	public static readonly string ID = "OneshotReactableLocator";
}
