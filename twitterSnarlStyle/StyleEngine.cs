using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using libSnarlStyles;
using Microsoft.Win32;
using System.Reflection;
using prefs_kit_d2;

namespace twitterSnarlStyle
{
    [Guid("1d971a56-4fb4-430f-a9e2-575672d2a783"), ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual), ComSourceInterfaces(typeof(libSnarlStyles.IStyleEngine))]
    [ProgId("twitterSnarlStyle.styleengine")]
    
    public class styleengine : IStyleEngine
    {

               
        public styleengine() {
        }

        #region IStyleEngine Members

        [ComVisible(true)]
        int IStyleEngine.CountStyles()
        {
            return 1;
        }

        [ComVisible(true)]
        IStyleInstance IStyleEngine.CreateInstance(string StyleName)
        {
            StyleInstance bla = new StyleInstance();
            return bla;

        }

        [ComVisible(true)]
        string IStyleEngine.Date()
        {
            return "2009-11-12";
        }

        [ComVisible(true)]
        string IStyleEngine.Description()
        {
            return "This style forwards your notification to your Twitter account as tweet";
        }

        [ComVisible(true)]
        int IStyleEngine.GetConfigWindow(string StyleName)
        {

            return 0;
        }

        [ComVisible(true)]
        melon.M_RESULT IStyleEngine.Initialize()
        {
            return melon.M_RESULT.M_OK;

        }

        [ComVisible(true)]
        string IStyleEngine.LastError()
        {
            return "Undefined error";
        }

        [ComVisible(true)]
        string IStyleEngine.Name()
        {
            return "Tweet it";

        }

        [ComVisible(true)]
        string IStyleEngine.Path()
        {
            return Assembly.GetExecutingAssembly().CodeBase;
           // return typeof(twitterSnarlStyle).Assembly.Location;
        }

        [ComVisible(true)]
        int IStyleEngine.Revision()
        {
            return 2;
        }

        [ComVisible(true)]
        void IStyleEngine.StyleAt(int Index, ref style_info Style)
        {
            string pathToIcon = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) + "\\blueBird.png";
            pathToIcon = pathToIcon.Replace("file:\\", "");

            Style.Flags = S_STYLE_FLAGS.S_STYLE_IS_WINDOWLESS;
            Style.IconPath = pathToIcon;
            Style.Major = 1;
            Style.Minor = 1;
            Style.Name = "Tweet it";
            Style.Path = Assembly.GetExecutingAssembly().CodeBase;
            Style.Schemes = "Standard";
            Style.Copyright = "Tlhan Ghun";
            Style.SupportEmail = "info@tlhan-ghun.de";
            Style.URL = "http://tlhan-ghun.de/";
            Style.Description = "This style forwards your notification to your Twitter account as tweet. Your credential will be asked for on first usage automatically";
        }

        [ComVisible(true)]
        void IStyleEngine.TidyUp()
        {
            return;
        }

        [ComVisible(true)]
        int IStyleEngine.Version()
        {
            return 39;
        }

        #endregion

        #region COM Registration Methods


        [ComRegisterFunction()]
        public static void RegisterClass(string key)
        {
            StringBuilder skey = new StringBuilder(key);
            skey.Replace(@"HKEY_CLASSES_ROOT\", "");
            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(skey.ToString(), true);
            RegistryKey ctrl = regKey.CreateSubKey("Control");
            ctrl.Close();
            RegistryKey inprocServer32 = regKey.OpenSubKey("InprocServer32", true);
            inprocServer32.SetValue("CodeBase", Assembly.GetExecutingAssembly().CodeBase);
            inprocServer32.Close();
            regKey.Close();
        }


        [ComUnregisterFunction()]
        public static void UnregisterClass(string key)
        {
            StringBuilder skey = new StringBuilder(key);
            skey.Replace(@"HKEY_CLASSES_ROOT\", "");
            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(skey.ToString(), true);
            regKey.DeleteSubKey("Control", false);
            RegistryKey inprocServer32 = regKey.OpenSubKey("InprocServer32", true);
            regKey.DeleteSubKey("CodeBase", false);
            regKey.Close();
        }
        #endregion
    }
}