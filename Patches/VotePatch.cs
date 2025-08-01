using AirlockAPI.Handlers;
using HarmonyLib;
using Il2CppFusion;
using Il2CppSG.Airlock;
using UnityEngine;

namespace AirlockAPI.Patches
{
    [HarmonyPatch()]
    public class VotePatch
    {
        [HarmonyPatch(typeof(VoteManager), nameof(VoteManager.RPC_Vote), new Type[] { typeof(PlayerRef), typeof(PlayerRef), typeof(RpcInfo) })]
        [HarmonyPrefix]
        public static bool Prefix1(PlayerRef voteAgainstPlayer, PlayerRef sourcePlayer, RpcInfo info)
        {
            PlayerState voter = GameObject.Find("PlayerState (" + voteAgainstPlayer.PlayerId + ")").GetComponent<PlayerState>();
            PlayerState voted = GameObject.Find("PlayerState (" + sourcePlayer.PlayerId + ")").GetComponent<PlayerState>();

            if (CustomGameHandler.Current)
            {
                return CustomGameHandler.Current.OnPlayerVoted(ref voter, ref voted);
            }

            return true;
        }

        [HarmonyPatch(typeof(VoteManager), nameof(VoteManager.RPC_Vote), new Type[] { typeof(PlayerRef), typeof(RpcInfo) })]
        [HarmonyPrefix]
        public static bool Prefix2(PlayerRef sourcePlayer, RpcInfo info)
        {
            PlayerState voter = GameObject.Find("PlayerState (" + sourcePlayer.PlayerId + ")").GetComponent<PlayerState>();

            if (CustomGameHandler.Current)
            {
                return CustomGameHandler.Current.OnPlayerVotedSkip(ref voter);
            }

            return true;
        }
    }

    [HarmonyPatch()]
    public class CallVote
    {
        [HarmonyPatch(typeof(VoteManager), nameof(VoteManager.RPC_CallVote), new Type[] { typeof(int), typeof(PlayerRef), typeof(NetworkBool), typeof(RpcInfo) })]
        [HarmonyPrefix]
        public static bool Prefix1(int foundPlayer, PlayerRef sourcePlayer, NetworkBool forceVote, RpcInfo info)
        {
            PlayerState caller = GameObject.Find("PlayerState (" + sourcePlayer.PlayerId + ")").GetComponent<PlayerState>();
            PlayerState bodyFound = GameObject.Find("PlayerState (" + foundPlayer + ")").GetComponent<PlayerState>();

            if (CustomGameHandler.Current)
            {
                return CustomGameHandler.Current.OnVotingBegan(ref bodyFound, ref caller) && CustomGameHandler.Current.OnBodyReported(ref bodyFound, ref caller);
            }

            return true;
        }

        [HarmonyPatch(typeof(VoteManager), nameof(VoteManager.RPC_CallVote), new Type[] { typeof(PlayerRef), typeof(NetworkBool), typeof(RpcInfo) })]
        [HarmonyPrefix]
        public static bool Prefix(PlayerRef sourcePlayer, NetworkBool forceVote, RpcInfo info)
        {
            PlayerState caller = GameObject.Find("PlayerState (" + sourcePlayer.PlayerId + ")").GetComponent<PlayerState>();
            PlayerState bodyFound = null;

            if (CustomGameHandler.Current)
            {
                return CustomGameHandler.Current.OnVotingBegan(ref bodyFound, ref caller) && CustomGameHandler.Current.OnMeetingCalled(ref caller);
            }

            return true;
        }
    }
}
