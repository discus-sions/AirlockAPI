using AirlockAPI.Handlers;
using HarmonyLib;
using Il2CppSG.Airlock;

namespace AirlockAPI.Patches
{
    [HarmonyPatch(typeof(GameStateManager), nameof(GameStateManager.StartGame))]
    public class StartGamePatch
    {
        public static bool Prefix(GameStateManager __instance)
        {
            if (CustomGameHandler.Current)
            {
                return CustomGameHandler.Current.OnGameStart();
            }

            return true;
        }
    }
}
