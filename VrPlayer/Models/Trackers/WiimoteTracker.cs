﻿using System;
using System.Windows.Media.Media3D;

using WiimoteLib;

using VrPlayer.Helpers;
using System.Threading;

namespace VrPlayer.Models.Trackers
{
    public class WiimoteTracker : TrackerBase, ITracker
    {
        private Wiimote _wiimote;

        public WiimoteTracker()
        {
            //Todo: Use a connecion method in the Tracker Interface
            try
            {
                IsEnabled = true;
                _wiimote = new Wiimote();
                _wiimote.Connect();
                _wiimote.InitializeMotionPlus();
                _wiimote.WiimoteChanged += new EventHandler<WiimoteChangedEventArgs>(wiimote_WiimoteChanged);

                _wiimote.SetRumble(true);
                _wiimote.SetLEDs(true, false, false, true);
                Thread.Sleep(40);
                _wiimote.SetRumble(false);
            }
            catch (Exception exc)
            {
                _wiimote.SetLEDs(false, false, false, false);
                IsEnabled = false;
            }
        }

        void wiimote_WiimoteChanged(object sender, WiimoteChangedEventArgs e)
        {
            Quaternion rotation = QuaternionHelper.FromEulerAngles(
                e.WiimoteState.MotionPlusState.Values.Y,
                e.WiimoteState.MotionPlusState.Values.X,
                e.WiimoteState.MotionPlusState.Values.Z);

            if (e.WiimoteState.ButtonState.Plus)
            {
                Calibrate(new Vector3D(0,0,0), rotation);
            }

            Rotation = BaseRotation * rotation;
        }

        public override void Dispose()
        {
            _wiimote.SetLEDs(false, false, false, false);
            _wiimote.Disconnect();
            _wiimote = null;
        }
    }
}
