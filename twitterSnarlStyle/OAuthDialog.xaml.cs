﻿using System.Windows;
using Dimebrain.TweetSharp.Fluent;
using Dimebrain.TweetSharp.Extensions;
using Dimebrain.TweetSharp.Model;
using twitterSnarlStyle.Properties;


namespace twitterSnarlStyle
{
    /// <summary>
    /// Interaction logic for OAuthDialog.xaml
    /// </summary>
    public partial class OAuthDialog : Window
    {
        private readonly OAuthToken _requestToken;
        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        public OAuthDialog()
        {
            InitializeComponent();


            pinTextBox.Visibility = Visibility.Hidden;
            pinLbl.Visibility = Visibility.Hidden;
            pinInstruction.Visibility = Visibility.Hidden;

            _consumerKey = Settings.Default.ConsumerKey;
            _consumerSecret = Settings.Default.ConsumerSecret;
            //get a request token.  this is only used during 
            //this process. 
            var getRequestTokenRequest = FluentTwitter.CreateRequest()
               .Authentication.GetRequestToken(_consumerKey, _consumerSecret);

            var response = getRequestTokenRequest.Request();
            _requestToken = response.AsToken();


            //TODO: handle the case where the token is NULL because 
            //twitter is down or broken in a manner suitable to your app

            //wait for the user to click the "Authorize button" 

        }

        private void AuthorizeDesktopBtn_Click(object sender, RoutedEventArgs e)
        {
            AuthorizeDesktopBtn.IsEnabled = false;
            pinTextBox.Visibility = System.Windows.Visibility.Visible;
            pinLbl.Visibility = Visibility.Visible;
            pinInstruction.Visibility = Visibility.Visible;
            var twitter = FluentTwitter.CreateRequest()
                .Authentication
                .AuthorizeDesktop(_consumerKey, _consumerSecret, _requestToken.Token);

            twitter.Request();

            //wait again until the user has authorized the desktop app
            //entered the PIN, and clicked "OK"
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pinTextBox.Text))
            {
                MessageBox.Show("Enter the PIN provided by twitter.com", "Can't complete Authorization",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string pin = pinTextBox.Text;
            var twitter = FluentTwitter.CreateRequest().Authentication.GetAccessToken(_consumerKey, _consumerSecret,
                                                                        _requestToken.Token, pin);
            var response = twitter.Request();
            var result = response.AsToken();

            if (result == null || string.IsNullOrEmpty(result.Token))
            {
                MessageBox.Show(response.AsError().ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                //TODO: handle this error condition. 
                //the user may have incorrectly entered the PIN or twitter 
                //may be down. Handle this in a way that makes sense for your
                //application. 
                DialogResult = false;
                return;
            }
            Settings.Default.AccessToken = result.Token;
            Settings.Default.AccessTokenSecret = result.TokenSecret;
            Settings.Default.Save();
            
            this.DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}

