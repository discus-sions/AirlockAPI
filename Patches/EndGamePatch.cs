using Il2CppSG.Airlock;
using Il2CppSG.Airlock.Roles;
using HarmonyLib;
using AirlockAPI.Handlers;

namespace AirlockAPI.Patches
{
    [HarmonyPatch(typeof(GameStateManager), nameof(GameStateManager.EndGame))]
    public class EndGamePatch
    {
        public static bool Prefix(GameStateManager __instance, GameTeam winningTeam)
        {
            if (CustomGameHandler.Current)
            {
                return CustomGameHandler.Current.OnGameEnd(ref winningTeam);
            }

            return true;
        }
    }
}
