# RiotV
A lightweight, aggressive NPC Riot script for Grand Theft Auto V. Uses low-level natives to force AI combat and high-aggression driving tasks.

**Technical Overview:**
The script utilizes a bitmask driving style (`1073741824`) to bypass standard AI collision avoidance and pathfinding. Hostility is enforced via the `HATES_PLAYER` relationship group and the `TASK_COMBAT_PED` native with high-priority flags to prevent NPC fleeing or yielding.

**Usage:**
1. Download the `RiotV.cs` file.
2. Ensure `ScriptHookVDotNet` is installed.
3. Move the file to your `/scripts` folder.
4. Press `F5` in-game to toggle.

**Configuration:**
The following variables are available at the top of the script for easy modification:
* `ToggleKey`: Change the activation key.
* `ScanRange`: Radius of NPC infection (default 500m).
* `UpdateMs`: AI refresh rate (default 300ms).
* `DriveSpeed`: Target speed for ramming vehicles.
