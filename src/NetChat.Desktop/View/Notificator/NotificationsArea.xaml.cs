using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NetChat.Desktop.View.Notificator
{
    /// <summary>
    /// Логика взаимодействия для NotificationsView.xaml
    /// </summary>
    public partial class NotificationsArea : Window
    {
        private Point _corner;

        public NotificationsArea()
        {
            InitializeComponent();
            this.SizeChanged += NotificationsView_SizeChanged;
        }

        private void NotificationsView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Left = _corner.X - this.ActualWidth - this.Margin.Right;
            this.Top = _corner.Y - this.ActualHeight - this.Margin.Top;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var workingArea = System.Windows.SystemParameters.WorkArea;
            var transform = PresentationSource.FromVisual(Application.Current.MainWindow).CompositionTarget.TransformFromDevice;
            _corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));
            
            this.Left = _corner.X - this.Width - this.Margin.Right;
            this.Top = _corner.Y - this.Height - this.Margin.Top;
        }
    }
}
