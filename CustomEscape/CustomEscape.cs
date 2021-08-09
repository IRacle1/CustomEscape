using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;
using Map = Exiled.Events.Handlers.Map;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace CustomEscape
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CustomEscape : Plugin<Configs>
    {
        public static CustomEscape Singleton;

        public override string Author => "Remindme";
        public override string Name => "Custom Escapes";
        public override string Prefix => "bEscape";
        public override PluginPriority Priority => PluginPriority.Highest;
        public override Version RequiredExiledVersion { get; } = new Version(3, 1, 0);

        public override void OnEnabled()
        {
            Singleton = this;

            Config.TryCreateFile();

            Player.ChangingRole += EventHandlers.OnChangingRole;
            Player.Escaping += EventHandlers.OnEscaping;
            Server.RoundEnded += EventHandlers.OnRoundEnded;
            Map.Generated += EventHandlers.OnGenerated;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.ChangingRole -= EventHandlers.OnChangingRole;
            Player.Escaping -= EventHandlers.OnEscaping;
            Server.RoundEnded -= EventHandlers.OnRoundEnded;
            Map.Generated -= EventHandlers.OnGenerated;

            Singleton = null;

            base.OnDisabled();
        }
    }
}