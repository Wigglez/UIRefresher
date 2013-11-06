/* BotBase created by AknA and Wigglez */

#region Namespaces

using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
#endregion

namespace UIRefresher.Helpers {
    public class Character : UIRefresher
    {
        // ===========================================================
        // Constants
        // ===========================================================

        // ===========================================================
        // Fields
        // ===========================================================

        public static int CharacterCurrentXp;
        public static int CharacterLastXp;

        // ===========================================================
        // Constructors
        // ===========================================================

        // ===========================================================
        // Getter & Setter
        // ===========================================================

        public static LocalPlayer Me { get { return StyxWoW.Me; } }

        // ===========================================================
        // Methods for/from SuperClass/Interfaces
        // ===========================================================

        // ===========================================================
        // Methods
        // ===========================================================

        public static int GetCharacterCurrentXp() {
            var xp = Lua.GetReturnVal<int>("return UnitXP('player')", 0);
            return xp;
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================
    }
}
