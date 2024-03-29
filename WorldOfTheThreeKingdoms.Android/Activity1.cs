using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using Java.Lang;
using Android.Telephony;
using System;
using Android.Graphics;
using Platforms;
using Tools;
using Android;
using Android.Support.V4.App;

namespace WorldOfTheThreeKingdoms.Android
{
    [Activity(Label = "中華三國志"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@android:style/Theme.NoTitleBar" //Translucent" //NoTitleBar "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , ScreenOrientation = ScreenOrientation.Landscape
        //, LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        //, ScreenOrientation = ScreenOrientation.SensorLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class Activity1 : AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                this.Window.AddFlags(WindowManagerFlags.Fullscreen);
                this.Window.AddFlags(WindowManagerFlags.KeepScreenOn);
                AskPermission();
                Platform.Activity1 = this;

                base.OnCreate(bundle);
               
                SetFullScreen();

                AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
                {
                    WebTools.TakeWarnMsg("UnhandledExceptionRaiser", "", args != null ? args.Exception : null);
                    //Season.SeasonGame.err = args.Exception.ToString();
                    //SeasonTools.SendErrMsg("UnhandledExceptionRaiser", args != null ? args.Exception : null);
                    //Game1.SendErrMsg("", args.Exception);
                    // Do something...
                };
                GameTime time = new GameTime();
                var g = new MainGame(time);
                
                g.IsFixedTimeStep = true;
                g.TargetElapsedTime = System.TimeSpan.FromMilliseconds(System.Math.Round(1000.0f / (60.0f * (int)GameManager.Setting.Current.SpeedUp)));
                var view = (View)g.Services.GetService(typeof(View));

                SetContentView(view);//SetContentView(g.Window);  //temple remove
                g.Run();

            }
            catch (System.Exception ex)
            {
                //WebTools.TakeWarnMsg("OnCreate", "", ex);
            }
        }
        private void AskPermission()
        {
            bool sSRPR = ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.WriteExternalStorage) |
                         ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.ReadExternalStorage);
            if (!sSRPR)
            { 
                ActivityCompat.RequestPermissions(this, new System.String[]{
                    Manifest.Permission.WriteExternalStorage,
                    Manifest.Permission.ReadExternalStorage}, 0);
                Platform.Sleep(5000);
            }                     
        }
        public void SetFullScreen()
        {
            View decorView = Window.DecorView;
            var uiOptions = (int)decorView.SystemUiVisibility;
            var newUiOptions = (int)uiOptions;
            //newUiOptions |= (int)SystemUiFlags.LowProfile;
            //newUiOptions |= (int)SystemUiFlags.Fullscreen;
            //newUiOptions |= (int)SystemUiFlags.HideNavigation;
            //newUiOptions |= (int)SystemUiFlags.Immersive;
            //newUiOptions |= (int)SystemUiFlags.Fullscreen;
            //newUiOptions |= (int)SystemUiFlags.LayoutStable;
            //newUiOptions |= (int)SystemUiFlags.LayoutHideNavigation;
            //newUiOptions |= (int)SystemUiFlags.LayoutFullscreen;
            newUiOptions |= (int)SystemUiFlags.Fullscreen;
            newUiOptions |= (int)SystemUiFlags.HideNavigation;
            newUiOptions |= (int)SystemUiFlags.ImmersiveSticky;
            //("setSystemUiVisibility", SYSTEM_UI_FLAG_FULLSCREEN | SYSTEM_UI_FLAG_HIDE_NAVIGATION | SYSTEM_UI_FLAG_IMMERSIVE_STICKY);
            decorView.SystemUiVisibility = (StatusBarVisibility)newUiOptions;
        }

        public override void OnConfigurationChanged(global::Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
        }

        protected override void OnPause()
        {
            //Session.PauseGame();
            try
            {
                base.OnPause();
            }
            catch (System.Exception ex)
            {
                WebTools.TakeWarnMsg("游戏暂停处理失败:" + "Activity1.OnPause", "Activity1.OnPause:", ex);
            }
        }

        protected override void OnResume()
        {
            SetFullScreen();

            //if (Session.InitReady)
            //{
            //    Session.ResumeGame();
            //}
            base.OnResume(); 
        }

        protected override void OnDestroy()
        {
            // Call base method
            base.OnDestroy();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            Platform.Current.HandleActivityResult(requestCode, resultCode, data);
        }
    }
}

