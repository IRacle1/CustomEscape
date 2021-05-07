using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;

namespace CustomEscape
{
    public class Configs : IConfig
    {
        [Description("Enables Custom Escape")]
        public bool IsEnabled { get; set; } = true;
        [Description("How big will the escape zone be")]
        public float EscapeRadius { get; set; } = 100f;
        [Description("Self-explanatory. Wrong configs will lead to role changing to Scp173. You can pass None to not change role. Make sure you follow the example formatting or it *can* possibly break")]
        public Dictionary<RoleType, PrettyCuffedConfig> RoleConversions { get; set; } = new Dictionary<RoleType, PrettyCuffedConfig>()
        {
            {RoleType.FacilityGuard, new PrettyCuffedConfig{CuffedRole = RoleType.ChaosInsurgency, UncuffedRole = RoleType.NtfLieutenant} },
            {RoleType.Scientist, new PrettyCuffedConfig{CuffedRole = RoleType.ChaosInsurgency, UncuffedRole = RoleType.NtfScientist} }
        };
        public bool Debug { get; set; } = false;
    }
    public class PrettyCuffedConfig // Because of this there will be "cuffed_role" and "uncuffed_role" config entries instead of just dictionaries
    {
        public RoleType CuffedRole { get; set; } = RoleType.ChaosInsurgency;
        public RoleType UncuffedRole { get; set; } = RoleType.NtfCadet;
    }
}
