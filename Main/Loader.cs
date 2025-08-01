using AirlockAPI.Attributes;
using AirlockAPI.Data;
using AirlockAPI.Debug;
using AirlockAPI.Handlers;
using AirlockAPI.Managers;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using System.Reflection;
using UnityEngine;

namespace AirlockAPI.Main
{
    internal class Loader : MelonMod
    {
        List<Type> GamemodeHandlerTypes = new List<Type>();

        public override void OnLateInitializeMelon()
        {
            MelonBase[] melons = MelonBase.RegisteredMelons.ToArray();

            foreach (MelonBase melon in melons)
            {
                Assembly dll = melon.MelonAssembly.Assembly;
                if (dll != null)
                {
                    foreach (Type type in dll.GetTypes())
                    {
                        MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                        foreach (MethodInfo method in methods)
                        {
                            AirlockRpc rpc = method.GetCustomAttribute<AirlockRpc>();

                            if (rpc != null)
                            {
                                NetworkManager.StringToAirlockRpc.Add(rpc.RpcName, rpc);
                                NetworkManager.RegisteredRpcs.Add(rpc, method);
                            }
                        }

                        if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(CustomGameHandler)))
                        {
                            if (!ClassInjector.IsTypeRegisteredInIl2Cpp(type))
                            {
                                ClassInjector.RegisterTypeInIl2Cpp(type);
                            }
                            GamemodeHandlerTypes.Add(type);

                            Logging.Log("Found Gamemode Script: " + type.Name);
                        }
                    }
                }
            }
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (sceneName != "Boot" && sceneName != "Title")
            {
                foreach (Type type in GamemodeHandlerTypes)
                {
                    if (type.Name.ToLower().Replace(" ", "").Contains(CurrentMode.Name.ToLower().Replace(" ", "")))
                    {
                        new GameObject("AIRLOCKAPI").AddComponent(Il2CppType.From(type));
                    }
                }
            }
        }

        public override void OnUpdate()
        {
            GamemodeManager.Update();
        }
    }
}
