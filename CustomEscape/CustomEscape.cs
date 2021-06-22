using System;
using ArithFeather.Points;
using Exiled.API.Enums;
using Exiled.API.Features;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace CustomEscape
{
    public class CustomEscape : Plugin<Configs>
    {
        public static CustomEscape Singleton;
        public override string Author { get; } = "Remindme";
        public override string Name { get; } = "Custom Escapes";
        public override string Prefix { get; } = "bEscape";
        public override Version Version { get; } = new Version(3, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 10, 0);
        public override PluginPriority Priority { get; } = PluginPriority.Highest;

        private EventHandlers EventHandlers { get; set; }

        public override void OnEnabled()
        {
            Singleton = this;

            EventHandlers = new EventHandlers();

            Config.TryCreateFile();

            Player.ChangingRole += EventHandlers.OnChangingRole;
            Player.Escaping += EventHandlers.OnEscaping;
            Server.RoundEnded += EventHandlers.OnRoundEnded;
            Points.OnLoadSpawnPoints += EventHandlers.OnLoadSpawnPoints;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.ChangingRole -= EventHandlers.OnChangingRole;
            Player.Escaping -= EventHandlers.OnEscaping;
            Server.RoundEnded -= EventHandlers.OnRoundEnded;
            Points.OnLoadSpawnPoints -= EventHandlers.OnLoadSpawnPoints;

            EventHandlers = null;

            Singleton = null;

            base.OnDisabled();
        }
    }
}