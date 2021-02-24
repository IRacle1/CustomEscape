using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BetterEscape
{

    public class BetterEscapeComponent : MonoBehaviour
    {
        private Player Ply { get; set; }

        private Vector3 escapePos = new Vector3(170, 984, 26);

        private Dictionary<RoleType, PrettyCuffedConfig> RoleConversions { get; set; } = BetterEscape.singleton.Config.RoleConversions;
        private bool Debug { get; set; } = BetterEscape.singleton.Config.Debug;

        public void Awake()
        {
            Ply = Player.Get(gameObject);
        }

        public void Update()
        {
            if (Ply.Role == RoleType.ClassD || Ply.Role == RoleType.Scientist) return;
            if (Vector3.Distance(Ply.Position, escapePos) > 2) return;

            Timing.CallDelayed(0.01f, () => Ply.SetRole(Ply.Role, false, true));
        }
        public void Destroy()
        {
            try
            {
                Destroy(this);
            }
            catch (Exception e)
            {
                Log.Error($"Can't Destroy: {e}");
            }
        }
    }

}
