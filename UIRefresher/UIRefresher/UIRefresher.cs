using System;
using System.Windows.Media;
using Styx.Common;
using Styx.CommonBot;
using Styx.Plugins;
using Styx.WoWInternals;

namespace UIRefresher {
    public class UIRefresher : HBPlugin {
        // ===========================================================
        // Constants
        // ===========================================================

        // ===========================================================
        // Fields
        // ===========================================================

        public static bool InitHasRun;

        public static string UIRStatusText;

        public static ThrottleTimer[] ThrottleTimers = new ThrottleTimer[ThrottleTimer.ThrottleTimerCount];


        // ===========================================================
        // Getter & Setter
        // ===========================================================

        // ===========================================================
        // Methods for/from SuperClass/Interfaces
        // ===========================================================

        public override string Author {
            get { return "AknA and Wigglez"; }
        }

        public override string Name {
            get { return "UIRefresher"; }
        }

        public override void Pulse() {
            if(!InitHasRun) {
                Init();
            }

            RefreshLogic();
        }

        public override Version Version { get { return new Version(1, 0, 0); } }

        public void Init() {
            InitHasRun = true;

            UIRStatusText = "{TimerName}: {TimeRemaining} ({TimeDuration})";

            for(var i = 0; i < ThrottleTimer.ThrottleTimerCount; i++) {
                ThrottleTimers[i] = new ThrottleTimer("", 0);
            }

            ThrottleTimers[0].TimerName = ThrottleTimer.RefreshUITimerString;

            BotEvents.OnBotStopped += BotEvent_OnBotStopped;

            UIRLog("Initialization complete.");
        }

        // ===========================================================
        // Methods
        // ===========================================================

        public static void UIRLog(string message, params object[] args) {
            Logging.Write(Colors.DeepSkyBlue, "[UIR]: " + message, args);
        }

        public static void RefreshTimer() {
            if(!ThrottleTimer.TimerStopwatch.IsRunning) {
                ThrottleTimer.CreateThrottleTimer(ThrottleTimer.TimerStopwatch, 1800000, ThrottleTimer.RefreshUITimerString);
            } else {
                ThrottleTimer.CheckThrottleTimer(ThrottleTimer.TimerStopwatch, ThrottleTimers[0].Time, ThrottleTimer.RefreshUITimerString);
            }
        }

        public static void RefreshLogic() {
            if(!ThrottleTimer.TimerStopwatch.IsRunning) {
                // Reload the ui
                Lua.DoString("ReloadUI()");

                ThrottleTimer.WaitTimerCreated = false;
            }

            RefreshTimer();
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================


        private static void BotEvent_OnBotStopped(EventArgs args)
        {
            InitHasRun = false;

            BotEvents.OnBotStopped -= BotEvent_OnBotStopped;


            TreeRoot.GoalText = string.Empty;
            TreeRoot.StatusText = string.Empty;

            // Clear the throttle timers
            foreach(var t in ThrottleTimers) {
                t.TimerName = "";
                t.Time = 0;
            }

            UIRLog("Shutdown complete.");
        }
    }
}
