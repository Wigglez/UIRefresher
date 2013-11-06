
#region Namespaces

using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;
using Styx.Common;
using Styx.Common.Helpers;
using Styx.CommonBot;

#endregion

namespace UIRefresher {
    public class ThrottleTimer {
        // ===========================================================
        // Constants
        // ===========================================================

        public const int ThrottleTimerCount = 1;

        // ===========================================================
        // Fields
        // ===========================================================

        public static Stopwatch TimerStopwatch = new Stopwatch();

        public const string RefreshUITimerString = "Refresh UI timer";

        public string TimerName;
        public int Time;
        public static string TimerStringName;

        public static WaitTimer WaitTimer;
        public static string WaitTimerAsString;
        public static bool WaitTimerCreated;

        // ===========================================================
        // Constructors
        // ===========================================================

        public ThrottleTimer() {
            TimerName = "";
            Time = 0;
        }

        public ThrottleTimer(string pTimerName, int pTime) {
            TimerName = pTimerName;
            Time = pTime;
        }

        // ===========================================================
        // Getter & Setter
        // ===========================================================

        // ===========================================================
        // Methods for/from SuperClass/Interfaces
        // ===========================================================

        // ===========================================================
        // Methods
        // ===========================================================

        public static void CreateThrottleTimer(Stopwatch pTimer, int pTime, string pTimerStringName) {
            TimerStringName = pTimerStringName;

            foreach(var t in UIRefresher.ThrottleTimers.Where(t => t.TimerName == TimerStringName)) {
                t.Time = pTime;
            }

            // Check a throttle timer using our currently received timer and the random number
            CheckThrottleTimer(pTimer, pTime, TimerStringName);
        }

        public static bool CheckThrottleTimer(Stopwatch pTimer, int pTime, string pTimerStringName) {

            // If the timer isn't running, start it
            if(!pTimer.IsRunning) { pTimer.Start(); }


            // If the timer's time is less than or equal to our specified amount of time
            if(pTimer.ElapsedMilliseconds <= pTime) {
                FormatAndShowTimer(pTimerStringName);

                return false;
            }

            // Otherwise, if the time elapsed is greater than the time we gave, stop and reset the timer
            ResetTimer(pTimer);

            WaitTimerCreated = false;

            return true;
        }

        public static void ResetTimer(Stopwatch pThrottleTimer) {
            if(pThrottleTimer.IsRunning) {
                pThrottleTimer.Reset();
            }
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

        private static void FormatAndShowTimer(string pTimerStringName) {
            // Create a new timer if last one has expired or we didn't have one
            if(!WaitTimerCreated) {
                foreach(var t in UIRefresher.ThrottleTimers.Where(t => t.TimerName == pTimerStringName)) {
                    WaitTimerCreated = true;

                    // Set up a new timer based on the amount of milliseconds in our current timer
                    WaitTimer = new WaitTimer(new TimeSpan(0, 0, 0, 0, t.Time));
                    // Build the string with the proper format
                    WaitTimerAsString = BuildTimeAsString(WaitTimer.WaitTime);
                    WaitTimer.Reset();
                }
            }
            OutputMessage(pTimerStringName);
        }

        private static string BuildTimeAsString(TimeSpan timeSpan) {
            string formatString;

            // Check if the timeSpan has hours, etc
            if(timeSpan.Hours > 0) {
                // {0:D2} means 0 is the hours, with a maximum digit of 2
                formatString = "{0:D2}h:{1:D2}m:{2:D2}s";
            } else if(timeSpan.Minutes > 0) {
                formatString = "{1:D2}m:{2:D2}s";
            } else {
                formatString = "{2:D2}s";
            }

            return String.Format(formatString, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        private static string SubstituteInMessage(string pTimerStringName, string message) {
            // Replaces a given string with that of requested info
            message = message.Replace("{TimerName}", pTimerStringName);
            message = message.Replace("{TimeRemaining}", BuildTimeAsString(WaitTimer.TimeLeft));
            message = message.Replace("{TimeDuration}", WaitTimerAsString);

            return message;
        }

        public static void BPLLog(string message, params object[] args) {
            Logging.Write(Colors.DeepSkyBlue, "[BPL]: " + message, args);
        }

        private static void OutputMessage(string pTimerStringName) {
            // Timername: 1m:15s (5m:2s)
            TreeRoot.GoalText = (String.IsNullOrEmpty(UIRefresher.UIRStatusText) ? "" : SubstituteInMessage(pTimerStringName, UIRefresher.UIRStatusText));
            TreeRoot.StatusText = TreeRoot.GoalText;
        }
    }
}
