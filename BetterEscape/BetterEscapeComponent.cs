using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BetterEscape
{
    public class BetterEscapeComponent : MonoBehaviour
    {
        //public Player ply { get; private set; }
        
        private Vector3 escapePos = new Vector3(170, 984, 26);

        public Dictionary<RoleType, RoleType> RoleConversions = new Dictionary<RoleType, RoleType>()
        {
            { RoleType.Scientist, BetterEscape.singleton.Config.ScientistTo },
            { RoleType.NtfCommander, BetterEscape.singleton.Config.NtfCommanderTo },
            { RoleType.NtfLieutenant, BetterEscape.singleton.Config.NtfLieutenantTo },
            { RoleType.NtfCadet, BetterEscape.singleton.Config.NtfCadetTo },
            { RoleType.FacilityGuard, BetterEscape.singleton.Config.FacilityGuardTo },
            { RoleType.NtfScientist, BetterEscape.singleton.Config.NtfScientistTo },
            { RoleType.ClassD, BetterEscape.singleton.Config.ClassDTo }
        };
        
        public void Update()
        {
            foreach (Player pl in Player.List)
            {
                if (Vector3.Distance(pl.Position, escapePos) <= 5)
                {
                    Log.Debug("ok");
                    if (pl.Role == RoleType.ChaosInsurgency)
                        Timing.CallDelayed(0.01f, () => pl.Role = BetterEscape.singleton.Config.ChaosInsurgencyTo);

                    foreach (KeyValuePair<RoleType, RoleType> kvp in RoleConversions)
                        if (kvp.Key == pl.Role)
                            pl.Role = kvp.Value;
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
