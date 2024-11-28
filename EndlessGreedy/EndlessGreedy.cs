using HarmonyLib;

namespace EndlessGreedy
{
	public class EndlessGreedy
	{
		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch("Initialize")]
		public class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				Debug.Log("I execute before Db.Initialize!");
			}

			public static void Postfix()
			{
				Debug.Log("I execute after Db.Initialize!");
				Finalizer(msg: "Oh my godness!");
			}

			public static void Finalizer(string msg)
			{
				Debug.Log($"I execute after Db.Initialize and all postfixes!{msg}");
			}
		}


	}

}