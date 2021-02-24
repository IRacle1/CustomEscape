using Exiled.API.Interfaces;
using Exiled.API.Features;
using System.Collections.Generic;
using System.ComponentModel;

namespace BetterEscape
{
    public class Configs : IConfig
    {
        [Description("Enables BetterEscape")]
        public bool IsEnabled { get; set; } = false;

        // [Description("What should a Scientist become?")]
        // public RoleType ScientistTo { get; set; } = RoleType.ChaosInsurgency;

        // [Description("What should a ChaosInsurgency become?")]
        // public RoleType ChaosInsurgencyTo { get; set; } = RoleType.NtfCadet;

        // [Description("What should a NtfCommander become?")]
        // public RoleType NtfCommanderTo { get; set; } = RoleType.ChaosInsurgency;

        // [Description("What should a NtfLieutenant become?")]
        // public RoleType NtfLieutenantTo { get; set; } = RoleType.ChaosInsurgency;

        // [Description("What should a NtfCadet become?")]
        // public RoleType NtfCadetTo { get; set; } = RoleType.ChaosInsurgency;

        // [Description("What should a NtfScientist become?")]
        // public RoleType NtfScientistTo { get; set; } = RoleType.ChaosInsurgency;

        // [Description("What should a FacilityGuard become?")]
        // public RoleType FacilityGuardTo { get; set; } = RoleType.ChaosInsurgency;

        // [Description("What should a ClassD become?")]
        // public RoleType ClassDTo { get; set; } = RoleType.NtfLieutenant;
        /*[Description("RoleType to convert from\n#    - RoleType to convert to with cuffs\n#    - RoleType to convert to without cuffs")]
        public Dictionary<RoleType, List<RoleType>> RoleConversions { get; set; } = new Dictionary<RoleType, List<RoleType>>() {
            {RoleType.FacilityGuard, new List<RoleType>(){RoleType.ChaosInsurgency, RoleType.NtfLieutenant}},
            {RoleType.Scientist, new List<RoleType>(){RoleType.ChaosInsurgency, RoleType.NtfScientist}},
            {RoleType.ChaosInsurgency, new List<RoleType>(){RoleType.NtfCadet, RoleType.None}},
            {RoleType.ClassD, new List<RoleType>(){RoleType.NtfCadet, RoleType.ChaosInsurgency}}
        };*/
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
