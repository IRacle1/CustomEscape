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
        public Player ply { get; private set; }
        //private bool IsCuffed = false;
        
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

        public void Awake()
        {
            //Load();
            ply = Player.Get(gameObject);
        }
        
        public void Update()
        {
            if (Vector3.Distance(ply.Position, escapePos) <= 2)
            {
                if (ply.Role == RoleType.ChaosInsurgency)
                    Timing.CallDelayed(0.01f, () => ply.Role = BetterEscape.singleton.Config.ChaosInsurgencyTo);

                foreach (KeyValuePair<RoleType, RoleType> kvp in RoleConversions)
                    if (kvp.Key == ply.Role)
                        Timing.CallDelayed(0.01f, () => ply.Role = kvp.Value);
            }
        }

        //public void OnDestroy() => Unload();

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
        /*
        public void OnCuff(HandcuffingEventArgs ev)
        {
            if (ev.Target == ply)
                IsCuffed = true;
        }

        public void OnRemoveCuff(RemovingHandcuffsEventArgs ev)
        {
            if (ev.Target == ply)
                IsCuffed = false;
        }

        private void Load()
        {
            Exiled.Events.Handlers.Player.Handcuffing += OnCuff;
            Exiled.Events.Handlers.Player.RemovingHandcuffs += OnRemoveCuff;
        }

        private void Unload()
        {
            Exiled.Events.Handlers.Player.Handcuffing += OnCuff;
            Exiled.Events.Handlers.Player.RemovingHandcuffs += OnRemoveCuff;
        }
        */
    }
    
}
