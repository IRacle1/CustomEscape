namespace CustomEscape
{
    using System;
    using System.Reflection;

    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events;

    using HarmonyLib;

    using Points;

    using Server = Exiled.Events.Handlers.Server;

    // ReSharper disable once ClassNeverInstantiated.Global
    public class CustomEscape : Plugin<Configs>
    {
        public static CustomEscape Singleton;

        public override string Author => "Remindme";
        public override string Name => "Custom Escapes";
        public override string Prefix => "bEscape";
        public override PluginPriority Priority => PluginPriority.Higher;
        public override Version Version { get; } = new Version(3, 2, 0);
        public override Version RequiredExiledVersion { get; } = new Version(3, 0, 0);

        private Harmony Harmony { get; set; }

        public override void OnEnabled()
        {
            Singleton = this;
            Harmony = new Harmony($"com.remindme.ce-{DateTime.Now.Ticks}");

            try
            {
                foreach (MethodBase method in Events.Instance.Harmony.GetPatchedMethods())
                    if (method.DeclaringType != null && method.Name == "UserCode_CmdRegisterEscape")
                        Events.DisabledPatchesHashSet.Add(method);

                Events.Instance.ReloadDisabledPatches();
            }
            catch (Exception e)
            {
                Log.Error($"Exiled broke: {e}");
            }

            Harmony.PatchAll();

            Config.TryCreateFile();

            Server.RoundEnded += EventHandlers.OnRoundEnded;
            Points.LoadedSpawnPoints += EventHandlers.OnLoadedSpawnPoints;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Server.RoundEnded -= EventHandlers.OnRoundEnded;
            Points.LoadedSpawnPoints -= EventHandlers.OnLoadedSpawnPoints;

            Harmony.UnpatchAll();
            Singleton = null;

            base.OnDisabled();
        }
    }
}
