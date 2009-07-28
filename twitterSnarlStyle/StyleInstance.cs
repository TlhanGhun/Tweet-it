using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using libSnarlStyles;

// Provides access to the fluent API; required
using Dimebrain.TweetSharp.Fluent;

// Provides access to the data classes that represent Twitter results
using Dimebrain.TweetSharp.Model;

// Provides access to features like relative time, and casting from XML/JSON to
// Twitter data classes
using Dimebrain.TweetSharp.Extensions;

using twitterSnarlStyle.Properties;

namespace twitterSnarlStyle
{
    [Guid("1d971a56-4fb4-430f-a9e2-575672d2a782"),
    ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]

    public class StyleInstance : IStyleInstance
    {

        public StyleInstance()
        {
            if (!CheckHasAuthorization())
            {
                PerformOAuthAuthorization();
            }

        }

        private static bool VerifyOAuthCredentials()
        {
            bool authorized = false;
            try
            {
                var twitter = FluentTwitter.CreateRequest()
                    .AuthenticateWith(Settings.Default.ConsumerKey, Settings.Default.ConsumerSecret,
                                      Settings.Default.AccessToken, Settings.Default.AccessTokenSecret)
                    .Account().VerifyCredentials();
                var response = twitter.Request();
                var profile = response.AsUser();
                if (profile != null)
                {
                    authorized = true;
                }
            }
            catch
            {
                // not online, broken connection to Twitter, ...
            }
            return authorized;
        }

        private void PerformOAuthAuthorization()
        {
            try
            {
                var dlg = new OAuthDialog();
                bool? result = dlg.ShowDialog();
                if (result.HasValue == result.Value)
                {
                    if (VerifyOAuthCredentials())
                    {
                        /* AuthStatusLabel.Content = "Authorized";
                         AuthStatusLabel.Foreground = Brushes.Green; */
                    }
                }
                else
                {
                    /*  AuthStatusLabel.Content = "Authorization cancelled.";
                      AuthStatusLabel.Foreground = Brushes.Red;
                      tryAgainButton.Visibility = Visibility.Visible; */
                }
            }
            catch
            {
                // no authorisation because no internet
            }
        }

        private bool CheckHasAuthorization()
        {
            bool authorized = false;
            if (!string.IsNullOrEmpty(Settings.Default.AccessToken)
                 && !string.IsNullOrEmpty(Settings.Default.AccessTokenSecret))
            {
                authorized = VerifyOAuthCredentials();
            }
            else
            {
               /* AuthStatusLabel.Content = "Auth tokens not found.";
                AuthStatusLabel.Foreground = Brushes.Red; */
            }
            return authorized;
        }
         

        #region IStyleInstance Members

        [ComVisible(true)]
        void IStyleInstance.AdjustPosition(ref int x, ref int y, ref short Alpha, ref bool Done)
        {
            return;
        }

        [ComVisible(true)]
        melon.MImage IStyleInstance.GetContent()
        {
            
            throw new NotImplementedException();
        }

        [ComVisible(true)]
        bool IStyleInstance.Pulse()
        {
            return false;
            throw new NotImplementedException();
        }

        [ComVisible(true)]
        void IStyleInstance.Show(bool Visible)
        {
            return;
            throw new NotImplementedException();
        }

        [ComVisible(true)]
        void IStyleInstance.UpdateContent(ref notification_info NotificationInfo)
        {
            
            string myUpdate = NotificationInfo.Title + " - " + NotificationInfo.Text;
            if (myUpdate.Length > 140)
            {
                myUpdate = myUpdate.Substring(0, 137) + "...";
            }

            try
            {
                var twitter = FluentTwitter.CreateRequest()
                    .AuthenticateWith(Settings.Default.ConsumerKey, Settings.Default.ConsumerSecret,
                                      Settings.Default.AccessToken, Settings.Default.AccessTokenSecret)
                    .Account().VerifyCredentials();

                var response = twitter.Request();
                var profile = response.AsUser();

                if (profile != null)
                {

                    var twitterPost = FluentTwitter.CreateRequest()
                    .AuthenticateWith(Settings.Default.ConsumerKey, Settings.Default.ConsumerSecret,
                                      Settings.Default.AccessToken, Settings.Default.AccessTokenSecret)
                                      .Statuses().Update(myUpdate);
                    var postresponse = twitterPost.Request();
                }
            }
            catch
            {
                MessageBox.Show("Error in sending notification to Twitter", "Error sending to Twitter", MessageBoxButtons.OK);
            }

   
        }

        #endregion
    }
}