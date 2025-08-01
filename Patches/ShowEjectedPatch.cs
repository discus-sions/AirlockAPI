using AirlockAPI.Handlers;
using HarmonyLib;
using Il2CppFusion;
using Il2CppSG.Airlock;
using Il2CppSG.Airlock.Cutscenes;
using Il2CppSG.Airlock.Roles;
using UnityEngine;

namespace AirlockAPI.Patches
{
    [HarmonyPatch(typeof(CutsceneManager), nameof(CutsceneManager.ShowEjected))]
    public class ShowEjectedPatch1
    {
        public static bool Prefix(CutsceneManager __instance, PlayerRef playerEjected, GameRole playerEjectedRole, bool onlyShowImpostors, int aliveImposters, int aliveCrewmates)
        {
            if (CustomGameHandler.Current)
            {
                PlayerState ejected = GameObject.Find("PlayerState (" + playerEjected.PlayerId + ")").GetComponent<PlayerState>();
                return CustomGameHandler.Current.OnPlayerEjected(ref ejected, ref playerEjectedRole);
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(CutsceneManager), nameof(CutsceneManager.ShowAnonymousEjected))]
    public class ShowEjectedPatch2
    {
        public static bool Prefix(CutsceneManager __instance, PlayerRef playerEjected, int aliveImposters, int aliveCrewmates)
        {
            if (CustomGameHandler.Current)
            {
                PlayerState ejected = GameObject.Find("PlayerState (" + playerEjected.PlayerId + ")").GetComponent<PlayerState>();
                GameRole role = CustomGameHandler.Current.GetTrueRole(ejected);
                return CustomGameHandler.Current.OnPlayerEjected(ref ejected, ref role);
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(CutsceneManager), nameof(CutsceneManager.ShowNoEjected))]
    public class ShowEjectedPatch3
    {
        public static bool Prefix(CutsceneManager __instance, int aliveImposters)
        {
            if (CustomGameHandler.Current)
            {
                PlayerState ejected = null;
                GameRole role = GameRole.NotSet;

                return CustomGameHandler.Current.OnPlayerEjected(ref ejected, ref role);
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(CutsceneManager), nameof(CutsceneManager.ShowDebugEjected))]
    public class ShowEjectedPatch4
    {
        public static bool Prefix(CutsceneManager __instance, PlayerRef playerEjected, bool wasImposter, int aliveImposters, int cutsceneIndex)
        {
            if (CustomGameHandler.Current)
            {
                PlayerState ejected = GameObject.Find("PlayerState (" + playerEjected.PlayerId + ")").GetComponent<PlayerState>();
                GameRole role = CustomGameHandler.Current.GetTrueRole(ejected);
                return CustomGameHandler.Current.OnPlayerEjected(ref ejected, ref role);
            }

            return true;
        }
    }
}
