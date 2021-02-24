# BetterEscape

Plugin to ultimately manage all of the Roles that Players get when they escape the facility, having roles for if they are either cuffed or not.

Requires [EXILED](https://github.com/Exiled-Team/EXILED) installed.

## Configs

Configs are actually very easy to do. You can find all the `RoleType`s under the example config or [here](https://canary.discord.com/channels/656673194693885975/668962626780397569/668964776147288094)

### Example

```yaml
bEscape:
  is_enabled: true
  # Self-explanatory. Wrong configs will lead to role changing to Scp173. You can pass None to not change the role at all. Make sure you follow the example formatting.
  RoleConversions:
    FacilityGuard:
      cuffed_role: ChaosInsurgency
      uncuffed_role: NtfLieutenant
    Scientist:
      cuffed_role: ChaosInsurgency
      uncuffed_role: NtfScientist
  # And so on.
  debug: false
```

### `RoleType`s

```yaml
None
## SCPs
Scp173
Scp106
Scp049
Scp079
Scp096
Scp0492
Scp93953
Scp93989
## Humans
NtfScientist
ChaosInsurgency
NtfLieutenant
NtfCommander
NtfCadet
FacilityGuard
Scientist
ClassD
## Misc Roles. Yes, they can 'escape' too
Spectator
Tutorial
```
