using BepInEx.IL2CPP;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RightToBearArms
{
    internal static class ChatCommandsCompatibility
    {
        internal static bool? enabled;
        internal static bool Enabled { get => enabled == null ? (bool)(enabled = IL2CPPChainloader.Instance.Plugins.ContainsKey("lammas123.ChatCommands")) : enabled.Value; }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        internal static void RegisterCommand(Type commandType)
        {
            IL2CPPChainloader.Instance.Plugins["lammas123.ChatCommands"].Instance.GetType()
                .Assembly
                .GetType("ChatCommands.Api")
                .GetMethod("RegisterCommand", BindingFlags.Static | BindingFlags.Public)
                .Invoke(null, [Activator.CreateInstance(commandType)]);
        }
    }
}