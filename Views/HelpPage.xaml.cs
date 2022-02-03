using AlgebraCalculatorApp.ViewModels;
using System;
using Windows.UI.Xaml.Controls;

namespace AlgebraCalculatorApp.Views
{
    public sealed partial class HelpPage : Page
    {
        public HelpViewModel ViewModel { get; } = new HelpViewModel();

        public HelpPage()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //feedback button
            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }

        private void Button_Click_1(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string uriToLaunch = @"https://drive.google.com/file/d/16EN2MHE6mMZJO5_5PUXejUurCjNrkJGv/view?usp=sharing";
            var uri = new Uri(uriToLaunch);
            async void DefaultLaunch()
            {
                var success = await Windows.System.Launcher.LaunchUriAsync(uri);
            }
            DefaultLaunch();
        }

        private void Button_Click_2(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // more apps button
            string uriToLaunch = @"https://www.microsoft.com/en-us/search/shop/Apps?q=noku+development";
            var uri = new Uri(uriToLaunch);
            async void DefaultLaunch2()
            {
                var success = await Windows.System.Launcher.LaunchUriAsync(uri);
            }
            DefaultLaunch2();
        }

        private void VersionNumber_SelectionChanged(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
    }
}
