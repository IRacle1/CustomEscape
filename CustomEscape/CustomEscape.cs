using Exiled.API.Enums;
using Exiled.API.Features;
using System;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace CustomEscape
{
    public class CustomEscape : Plugin<Configs>
    {
        public override string Author { get; } = "Remindme";
        public override string Name { get; } = "Custom Escapes";
        public override string Prefix { get; } = "bEscape";
        public override Version Version { get; } = new Version(2, 4, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 34);
        public override PluginPriority Priority { get; } = PluginPriority.Highest;

        private EventHandlers EventHandlers { get; set; }

        public static CustomEscape singleton;

        public override void OnEnabled()
        {
            singleton = this;

            EventHandlers = new EventHandlers();

            Player.Verified += EventHandlers.OnVerified;
            Player.Left += EventHandlers.OnLeft;
            Player.ChangingRole += EventHandlers.OnChangingRole;
            Player.Escaping += EventHandlers.OnEscaping;
            Server.RoundEnded += EventHandlers.OnRoundEnded;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.Verified -= EventHandlers.OnVerified;
            Player.Left -= EventHandlers.OnLeft;
            Player.ChangingRole -= EventHandlers.OnChangingRole;
            Player.Escaping -= EventHandlers.OnEscaping;
            Server.RoundEnded -= EventHandlers.OnRoundEnded;

            EventHandlers = null;

            singleton = null;

            base.OnDisabled();
        }
    }
}
