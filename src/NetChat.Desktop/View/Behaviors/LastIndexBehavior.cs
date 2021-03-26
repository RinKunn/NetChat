using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace NetChat.Desktop.View.Behaviors
{
    public class LastIndexBehavior : Behavior<ItemsControl>
    {
        private ScrollViewer _scrollViewer;
        private bool _autoScroll = true;

        protected override void OnAttached()
        {
            base.OnAttached();
            LastVisibleIndex = -1;
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.Unloaded += AssociatedObject_Unloaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer = AssociatedObject.FindChild<ScrollViewer>();
            if (_scrollViewer == null)
                throw new ArgumentNullException(nameof(_scrollViewer));
            _scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            UpdateAutoScroll(e);
            UpdateLastVisibleIndex(e);
        }

        private void UpdateAutoScroll(ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange == 0)
            {
                if (_scrollViewer.VerticalOffset == _scrollViewer.ScrollableHeight)
                    _autoScroll = true;
                else
                    _autoScroll = false;
            }
            if (_autoScroll && e.ExtentHeightChange != 0)
            {
                _scrollViewer.ScrollToVerticalOffset(_scrollViewer.ExtentHeight);
            }
        }
        private void UpdateLastVisibleIndex(ScrollChangedEventArgs e)
        {
            if (_autoScroll)
                LastVisibleIndex = AssociatedObject.Items.Count - 1;
            else
            {
                for (int i = AssociatedObject.Items.Count - 1; i > 0; i--)
                {
                    FrameworkElement elm = (FrameworkElement)AssociatedObject?
                        .ItemContainerGenerator.ContainerFromIndex(i);
                    if (ItemIsVisible(elm, AssociatedObject))
                    {
                        LastVisibleIndex = i;
                        break;
                    }
                }
            }
        }

        private bool ItemIsVisible(FrameworkElement element, FrameworkElement container)
        {
            if (!element.IsVisible)
                return false;

            Rect bounds = element
                .TransformToAncestor(container)
                .TransformBounds(
                    new Rect(0.0, 0.0, element.ActualWidth * 0.5, element.ActualHeight * 0.5));
            var rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
            return rect.Contains(bounds.BottomRight);
        }



        public static DependencyProperty LastVisibleIndexProperty =
            DependencyProperty.Register("LastVisibleIndex",
                typeof(int),
                typeof(LastIndexBehavior));

        public int LastVisibleIndex
        {
            get { return (int)GetValue(LastVisibleIndexProperty); }
            set { SetValue(LastVisibleIndexProperty, value); }
        }
    }
}
