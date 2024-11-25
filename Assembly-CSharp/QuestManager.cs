using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000A0D RID: 2573
[SerializationConfig(MemberSerialization.OptIn)]
public class QuestManager : KMonoBehaviour
{
	// Token: 0x06004AA7 RID: 19111 RVA: 0x001AB19C File Offset: 0x001A939C
	protected override void OnPrefabInit()
	{
		if (QuestManager.instance != null)
		{
			UnityEngine.Object.Destroy(QuestManager.instance);
			return;
		}
		QuestManager.instance = this;
		base.OnPrefabInit();
	}

	// Token: 0x06004AA8 RID: 19112 RVA: 0x001AB1C4 File Offset: 0x001A93C4
	public static QuestInstance InitializeQuest(Tag ownerId, Quest quest)
	{
		QuestInstance questInstance;
		if (!QuestManager.TryGetQuest(ownerId.GetHash(), quest, out questInstance))
		{
			questInstance = (QuestManager.instance.ownerToQuests[ownerId.GetHash()][quest.IdHash] = new QuestInstance(quest));
		}
		questInstance.Initialize(quest);
		return questInstance;
	}

	// Token: 0x06004AA9 RID: 19113 RVA: 0x001AB218 File Offset: 0x001A9418
	public static QuestInstance InitializeQuest(HashedString ownerId, Quest quest)
	{
		QuestInstance questInstance;
		if (!QuestManager.TryGetQuest(ownerId.HashValue, quest, out questInstance))
		{
			questInstance = (QuestManager.instance.ownerToQuests[ownerId.HashValue][quest.IdHash] = new QuestInstance(quest));
		}
		questInstance.Initialize(quest);
		return questInstance;
	}

	// Token: 0x06004AAA RID: 19114 RVA: 0x001AB26C File Offset: 0x001A946C
	public static QuestInstance GetInstance(Tag ownerId, Quest quest)
	{
		QuestInstance result;
		QuestManager.TryGetQuest(ownerId.GetHash(), quest, out result);
		return result;
	}

	// Token: 0x06004AAB RID: 19115 RVA: 0x001AB28C File Offset: 0x001A948C
	public static QuestInstance GetInstance(HashedString ownerId, Quest quest)
	{
		QuestInstance result;
		QuestManager.TryGetQuest(ownerId.HashValue, quest, out result);
		return result;
	}

	// Token: 0x06004AAC RID: 19116 RVA: 0x001AB2AC File Offset: 0x001A94AC
	public static bool CheckState(HashedString ownerId, Quest quest, Quest.State state)
	{
		QuestInstance questInstance;
		QuestManager.TryGetQuest(ownerId.HashValue, quest, out questInstance);
		return questInstance != null && questInstance.CurrentState == state;
	}

	// Token: 0x06004AAD RID: 19117 RVA: 0x001AB2D8 File Offset: 0x001A94D8
	public static bool CheckState(Tag ownerId, Quest quest, Quest.State state)
	{
		QuestInstance questInstance;
		QuestManager.TryGetQuest(ownerId.GetHash(), quest, out questInstance);
		return questInstance != null && questInstance.CurrentState == state;
	}

	// Token: 0x06004AAE RID: 19118 RVA: 0x001AB304 File Offset: 0x001A9504
	private static bool TryGetQuest(int ownerId, Quest quest, out QuestInstance qInst)
	{
		qInst = null;
		Dictionary<HashedString, QuestInstance> dictionary;
		if (!QuestManager.instance.ownerToQuests.TryGetValue(ownerId, out dictionary))
		{
			dictionary = (QuestManager.instance.ownerToQuests[ownerId] = new Dictionary<HashedString, QuestInstance>());
		}
		return dictionary.TryGetValue(quest.IdHash, out qInst);
	}

	// Token: 0x040030EB RID: 12523
	private static QuestManager instance;

	// Token: 0x040030EC RID: 12524
	[Serialize]
	private Dictionary<int, Dictionary<HashedString, QuestInstance>> ownerToQuests = new Dictionary<int, Dictionary<HashedString, QuestInstance>>();
}
