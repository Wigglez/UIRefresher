using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Styx.Common;
using Styx.CommonBot;
using Styx.Plugins;
using Styx.TreeSharp;
using Styx.WoWInternals;
using UIRefresher.Helpers;

namespace UIRefresher {
    public class UIRefresher : HBPlugin {
        // ===========================================================
        // Constants
        // ===========================================================

        // ===========================================================
        // Fields
        // ===========================================================

        public static string UIRStatusText;

        public static ThrottleTimer[] ThrottleTimers = new ThrottleTimer[ThrottleTimer.ThrottleTimerCount];




        public override string Author {
            get { return "AknA and Wigglez"; }
        }

        public override string Name {
            get { return "UIRefresher"; }
        }

        public override void Pulse() {

        }

        public override Version Version { get { return new Version(1, 0, 0); } }

        public override void Initialize() {
            base.Initialize();
            BotEvents.OnBotStarted += BotEvent_OnBotStarted;
            BotEvents.OnBotStopped += BotEvent_OnBotStopped;
        }

        private static void BotEvent_OnBotStopped(EventArgs args) {

        }

        private static void BotEvent_OnBotStarted(EventArgs args) {
            UIRStatusText = "{TimerName}: {TimeRemaining} ({TimeDuration})";

            for(var i = 0; i < ThrottleTimer.ThrottleTimerCount; i++) {
                ThrottleTimers[i] = new ThrottleTimer("", 0);
            }

            ThrottleTimers[0].TimerName = ThrottleTimer.RefreshUITimerString;

            UIRLog("Initialization complete.");
        }

        public void Shutdown() {

        }

        public static void UIRLog(string message, params object[] args) {
            Logging.Write(Colors.DeepSkyBlue, "[UIR]: " + message, args);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RefreshLogic() {


            Lua.DoString("ReloadUI()");
        }
    }
}
