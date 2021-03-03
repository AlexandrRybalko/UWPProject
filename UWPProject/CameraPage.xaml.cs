using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UWPProject.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace UWPProject
{
    public partial class CameraPage : Page
    {
        public CameraViewModel Camera { get; set; }

        public CameraPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            int cameraId = (int)e.Parameter;
            Camera = new CameraViewModel(cameraId);

            var goBackCommand = new StandardUICommand(StandardUICommandKind.None);
            goBackCommand.ExecuteRequested += GoBack;
            this.BackButton.Command = goBackCommand;
        }

        private void GoBack(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            this.Frame.GoBack();
        }
    }
}
