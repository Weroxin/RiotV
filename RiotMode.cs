using System;
using System.Windows.Forms;
using GTA;
using GTA.Native;

public class RiotMode : Script
{
    // ===========================================================
    //                    USER CONFIGURATION
    // ===========================================================
    public static Keys ToggleKey = Keys.F5;      // Key to toggle mod
    public static float ScanRange = 500f;       // Radius to affect NPCs (meters)
    public static int UpdateMs = 300;           // Update speed (ms)
    public static float DriveSpeed = 150f;      // Aggressive driving speed (max speed)
    public static int ChaosDrivingStyle = 1073741824 | 524288 | 262144 | 32;
    // ===========================================================

    private bool _active = false;
    private DateTime _nextTick = DateTime.MinValue;

    public RiotMode()
    {
        KeyDown += (s, e) => {
            if (e.KeyCode == ToggleKey)
            {
                _active = !_active;
                Notify(_active ? "~g~Riot Mode: ON" : "~r~Riot Mode: OFF");
            }
        };
        Tick += OnTick;
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (!_active || DateTime.Now < _nextTick) return;
        _nextTick = DateTime.Now.AddMilliseconds(UpdateMs);

        Ped player = Game.Player.Character;
        Ped[] peds = World.GetNearbyPeds(player, ScanRange);

        foreach (Ped npc in peds)
        {
            if (npc == player || !npc.Exists() || npc.IsDead) continue;

            Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, npc, true);
            
            Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, npc, 0x842FC010); // HATES_PLAYER

            Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, npc, 0, false);      // Never flee
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, npc, 46, true);   // Always fight
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, npc, 14, true);   // Ignore shocking events
            Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, npc, 5, true);    // Fight armed peds
            Function.Call(Hash.SET_PED_CONFIG_FLAG, npc, 100, false);       // Explicitly disable flee flag

            if (!npc.IsInCombatAgainst(player))
            {
                if (npc.IsInVehicle())
                {
                    Function.Call(Hash.SET_DRIVER_ABILITY, npc, 1.0f);
                    Function.Call(Hash.SET_DRIVER_AGGRESSIVENESS, npc, 1.0f);
                    Function.Call(Hash.SET_PED_CONFIG_FLAG, npc, 156, true);

                    Function.Call(Hash.TASK_VEHICLE_MISSION_PED_TARGET, 
                        npc, npc.CurrentVehicle, player, 6, DriveSpeed, ChaosDrivingStyle, 0f, 0f, true);
                }
                else
                {
                    Function.Call(Hash.TASK_COMBAT_PED, npc, player, 0, 16);
                }
            }
        }
    }

    void Notify(string msg)
    {
        Function.Call(Hash.BEGIN_TEXT_COMMAND_PRINT, "STRING");
        Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, msg);
        Function.Call(Hash.END_TEXT_COMMAND_PRINT, 3000, true);
    }
}