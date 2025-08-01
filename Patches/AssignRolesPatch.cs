using AirlockAPI.Handlers;
using HarmonyLib;
using Il2CppSG.Airlock.Roles;

namespace AirlockAPI.Patches
{
    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.AssignRoles))]
    public class AssignRolesPatch
    {
        public static bool Prefix(RoleManager __instance)
        {
            if (CustomGameHandler.Current)
            {
                return CustomGameHandler.Current.OnBeforeAssignRoles();
            }

            return true;
        }
        public static void Postfix(RoleManager __instance)
        {
            if (CustomGameHandler.Current)
            {
                CustomGameHandler.Current.OnAfterAssignRoles();
            }
        }
    }
}
