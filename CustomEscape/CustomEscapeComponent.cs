using Exiled.API.Features;
using MEC;
using System;
using UnityEngine;

namespace CustomEscape
{
    public class CustomEscapeComponent : MonoBehaviour
    {
        private bool Debug { get; set; } = CustomEscape.singleton.Config.Debug;

        public void Awake()
        {
        }

        public void Update()
        {
        }
        public void OnTriggerEnter(Collider col)
        {
            Log.Debug("triggered", Debug);
            Player Ply = Player.Get(col.gameObject);

            if (Ply.Role == RoleType.ClassD || Ply.Role == RoleType.Scientist) return;

            Log.Debug($"setting role: {Ply.Nickname}, {Ply.Role}, IsCuffed:{Ply.IsCuffed}", Debug);

            Timing.CallDelayed(0.01f, () => Ply.SetRole(Ply.Role, false, true));
        }
    }
}
