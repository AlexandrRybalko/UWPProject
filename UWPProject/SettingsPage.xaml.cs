using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPProject
{
    public sealed partial class SettingsPage : Page
    {
        private ResourceLoader _resourceLoader;

        public ButtonCommand ApplyChangesCommand { get; }
        public ButtonCommand GoBackCommand { get; set; }

        public SettingsPage()
        {
            this.InitializeComponent();

            string language = ApplicationData.Current.LocalSettings.Values["Language"] as string;

            if (string.IsNullOrEmpty(language))
            {
                _resourceLoader = ResourceLoader.GetForCurrentView("En-en");
            }
            else
            {
                _resourceLoader = ResourceLoader.GetForCurrentView(language);
            }

            ApplyChangesCommand = new ButtonCommand(new Action(ApplyChanges), () => true);            
        }

        private void SetTheme(object sender, RoutedEventArgs e)
        {
            var button = (RadioButton)sender;
            string themeTitle = button.Name.Equals("Default") ? "" : button.Name;
            ApplicationData.Current.LocalSettings.Values["Theme"] = themeTitle;
        }

        private void SetLanguage(object sender, RoutedEventArgs e)
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

        private void ApplyChanges()
        {
            this.GoBackCommand = new ButtonCommand(new Action(this.Frame.GoBack), () => true);
            ContentDialog dialog = new ContentDialog();
            dialog.Content = _resourceLoader.GetString("DialogMessage");
            dialog.CloseButtonText = "OK";
            dialog.CloseButtonStyle = (Style)this.Resources["buttonStyle"];
            dialog.CloseButtonCommand = GoBackCommand;            

            dialog.ShowAsync();
        }
    }
}
