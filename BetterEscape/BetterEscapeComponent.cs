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

        private Vector3 EscapePos { get; set; }

        private Dictionary<RoleType, PrettyCuffedConfig> RoleConversions { get; set; } = BetterEscape.singleton.Config.RoleConversions;
        private bool Debug { get; set; } = BetterEscape.singleton.Config.Debug;

        public void Awake()
        {
            Ply = Player.Get(gameObject);
            EscapePos = Ply.GameObject.GetComponent<Escape>().worldPosition;
        }

        public void Update()
        {
            if (Ply.Role == RoleType.ClassD || Ply.Role == RoleType.Scientist) return;
            if (Vector3.Distance(Ply.Position, EscapePos) > 2) return;

            foreach (KeyValuePair<RoleType, PrettyCuffedConfig> kvp in RoleConversions)
            {
                if (kvp.Key == Ply.Role)
                {
                    RoleType role = Ply.IsCuffed ? kvp.Value.CuffedRole : kvp.Value.UncuffedRole;
                    Log.Debug($"update: {Ply.Role} to {role}, cuffed: {Ply.IsCuffed}", Debug);
                    if (role != RoleType.None)
                    {
                        Timing.CallDelayed(0.01f, () => Ply.SetRole(role, false, true));
                    }
                }
            }
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
