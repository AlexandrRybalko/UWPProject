using System;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPProject
{
    public sealed partial class SettingsPage : Page
    {
        private ResourceLoader resourceLoader;
        private int c;

        public ButtonCommand GoBackCommand { get; set; }

        public SettingsPage()
        {
            this.InitializeComponent();

            string language = ApplicationData.Current.LocalSettings.Values["Language"] as string;

            if (string.IsNullOrEmpty(language))
            {
                resourceLoader = ResourceLoader.GetForCurrentView("En-en");
            }
            else
            {
                resourceLoader = ResourceLoader.GetForCurrentView(language);
            }

            this.GoBackCommand = new ButtonCommand(new Action(() => this.Frame.GoBack()), () => true);
        }

        private async void SetTheme(object sender, RoutedEventArgs e)
        {
            if (c != 0)
            {
                var button = (RadioButton)sender;
                string themeTitle = button.Name.Equals("Default") ? "" : button.Name;
                ApplicationData.Current.LocalSettings.Values["Theme"] = themeTitle;

                ContentDialog dialog = new ContentDialog();
                dialog.Content = resourceLoader.GetString("DialogMessage");
                dialog.CloseButtonText = "OK";
                dialog.CloseButtonStyle = (Style)this.Resources["buttonStyle"];

                await dialog.ShowAsync();
            }

            c++;
        }

        private void SetEnglish(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["Language"] = "En-en";
        }

        private void SetRussian(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["Language"] = "Ru-ru";
        }

        private bool IsChecked(string name, string title)
        {
            string setting = ApplicationData.Current.LocalSettings.Values[name] as string;

            if (string.IsNullOrEmpty(setting) && title.Equals("Default"))
            {
                return true;
            }

            if (string.IsNullOrEmpty(setting) || !setting.Equals(title))
            {
                return false;
            }

            return true;
        }
    }
}
