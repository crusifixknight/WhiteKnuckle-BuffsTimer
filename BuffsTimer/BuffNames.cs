using System.Collections.Generic;

namespace BuffsTimer;

public static class BuffNames
{
    public static Dictionary<string, string> Dict;

    static BuffNames()
    {
        Dict = new Dictionary<string, string>
        {
            {"addJump", "Jump higher"},
            {"addSpeed", "Running faster"},
            {"addClimb", "Climb faster"},
            {"addGripStrength", "Increased grip strength"},
            {"pilled", "Decreased stamina consumption"},
            {"addStrike", "Increased melee damage"},
            {"addHammer", "Increased piton-hammering ability"},
            {"gripResist", "Reduced grip loss"},
            {"damageResist", "Increased damage resist"},
            {"regenerateGripStrength", "Increased stamina regeneration"},
            {"addClimbSpeed", "Climb faster"},
            {"roided", "Climb faster (???)"},
            {"grabAnything", "Grab any surface"}
            
        };
    }
}