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

        public Dictionary<RoleType, List<RoleType>> RoleConversions = BetterEscape.singleton.Config.RoleConversions;

        public void Awake()
        {
            //Load();
            ply = Player.Get(gameObject);
        }
        
        public void Update()
        {
            if (Vector3.Distance(ply.Position, escapePos) <= 2)
            {
                foreach (KeyValuePair<RoleType, List<RoleType>> kvp in RoleConversions)
                    if (kvp.Key == ply.Role)
                        Timing.CallDelayed(0.01f, () => ply.Role = ply.IsCuffed ? kvp.Value[0] : kvp.Value[1]);
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
