using AirlockAPI.Handlers;
using HarmonyLib;
using Il2CppFusion;
using Il2CppSG.Airlock;
using Il2CppSG.Airlock.Network;
using UnityEngine;

namespace AirlockAPI.Patches
{
    [HarmonyPatch(typeof(NetworkedKillBehaviour), nameof(NetworkedKillBehaviour.RPC_TargetedAction))]
    public class TargetedActionPatch
    {
        public static bool Prefix(NetworkedKillBehaviour __instance, ref PlayerRef targetedPlayer, ref PlayerRef perpetrator, ref int action)
        {
            PlayerState perp = GameObject.Find("PlayerState (" + perpetrator.PlayerId + ")").GetComponent<PlayerState>();
            PlayerState target = GameObject.Find("PlayerState (" + targetedPlayer.PlayerId + ")").GetComponent<PlayerState>();

            if (CustomGameHandler.Current)
            {
                return CustomGameHandler.Current.OnTargetedAction(ref perp, ref target, ref action);
            }

            return true;
        }
    }
}
