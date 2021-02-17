using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace BetterEscape
{
    public class Configs : IConfig
    {
        [Description("Enables BetterEscape")]
        public bool IsEnabled { get; set; } = true;

        [Description("What should a Scientist become?")]
        public RoleType ScientistTo { get; set; } = RoleType.ChaosInsurgency;

        [Description("What should a ChaosInsurgency become?")]
        public RoleType ChaosInsurgencyTo { get; set; } = RoleType.NtfCadet;

        [Description("What should a NtfCommander become?")]
        public RoleType NtfCommanderTo { get; set; } = RoleType.ChaosInsurgency;

        [Description("What should a NtfLieutenant become?")]
        public RoleType NtfLieutenantTo { get; set; } = RoleType.ChaosInsurgency;

        [Description("What should a NtfCadet become?")]
        public RoleType NtfCadetTo { get; set; } = RoleType.ChaosInsurgency;

        [Description("What should a NtfScientist become?")]
        public RoleType NtfScientistTo { get; set; } = RoleType.ChaosInsurgency;

        [Description("What should a FacilityGuard become?")]
        public RoleType FacilityGuardTo { get; set; } = RoleType.ChaosInsurgency;

        [Description("What should a ClassD become?")]
        public RoleType ClassDTo { get; set; } = RoleType.NtfLieutenant;
    }
}
