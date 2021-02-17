using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BetterEscape
{
    public class BetterEscapeComponent : MonoBehaviour
    {
        public Player ply { get; private set; }
        private Vector3 escapePos = new Vector3(170, 984, 26);

        public void Awake() => ply = Player.Get(gameObject);
        public void Update()
        {
            if (ply.Team != Team.RIP && ply.Team != Team.SCP)
                if (ply.IsCuffed)
                    if (ply.Position == escapePos || Vector3.Distance(ply.Position, escapePos) <= 5)
                        foreach (KeyValuePair<RoleType, RoleType> kvp in EventHandlers.RoleConversions)
                            if (ply.Role == kvp.Key)
                                ply.Role = kvp.Value;
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
