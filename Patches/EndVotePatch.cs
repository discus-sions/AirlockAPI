using Il2CppSG.Airlock;
using HarmonyLib;
using AirlockAPI.Handlers;

namespace AirlockAPI.Patches
{
    [HarmonyPatch(typeof(VoteManager), nameof(VoteManager.EndVote))]
    public class EndVotePatch
    {
        public static bool Prefix(VoteManager __instance)
        {
            if (CustomGameHandler.Current)
            {
                return CustomGameHandler.Current.OnAllVotesCast();
            }

            return true;
        }
    }
}
