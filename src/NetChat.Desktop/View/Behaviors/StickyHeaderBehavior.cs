using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace NetChat.Desktop.View.Behaviors
{
    public class StickyHeaderBehavior : Behavior<ItemsControl>
    {
        private ScrollViewer _scrollViewer;
        private TextBox _frozenGroupHeaderTextBox;
        private readonly int HIT_VERTICAL_OFFSET = 15;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer = AssociatedObject.FindChild<ScrollViewer>();
            if (_scrollViewer == null)
                throw new ArgumentNullException(nameof(_frozenGroupHeaderTextBox));
            _scrollViewer.ScrollChanged += ScrollView_ScrollChanged;

            _frozenGroupHeaderTextBox = AssociatedObject.FindChild<TextBox>("PART_FrozenGroupHeader");
            if (_frozenGroupHeaderTextBox == null)
                throw new ArgumentNullException(nameof(_frozenGroupHeaderTextBox));

            UpdateFrozenGroupHeader();
        }

        private void ScrollView_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            UpdateFrozenGroupHeader();
        }

        private void UpdateFrozenGroupHeader()
        {
            if (AssociatedObject.HasItems)
            {
                GroupItem group = GetFirstVisibleGroupItem(AssociatedObject);
                var prevGroupName = _frozenGroupHeaderTextBox.Text;
                _frozenGroupHeaderTextBox.Text = "";

                if (group != null)
                {
                    object data = group.Content;
                    DateTime date = (DateTime)data.GetType().GetProperty("Name").GetValue(data, null);
                    _frozenGroupHeaderTextBox.Text = date.Date.ToString("dd-MM-yyyy");
                }
                else
                {
                    _frozenGroupHeaderTextBox.Text = prevGroupName;
                }
                _frozenGroupHeaderTextBox.Visibility = Visibility.Visible;
            }
            else
                _frozenGroupHeaderTextBox.Visibility = Visibility.Collapsed;
        }

        private GroupItem GetFirstVisibleGroupItem(ItemsControl itemsControl)
        {
            HitTestResult hitTest = null;
            VisualTreeHelper.HitTest(
                itemsControl,
                new HitTestFilterCallback((o) =>
                {
                    if (o is FrameworkElement fwe && !fwe.IsHitTestVisible)
                    {
                        return HitTestFilterBehavior.ContinueSkipSelfAndChildren;
                    }
                    return HitTestFilterBehavior.Continue;
                }),
                new HitTestResultCallback((o) =>
                {
                    hitTest = o;
                    return HitTestResultBehavior.Stop;
                }),
                new PointHitTestParameters(new Point(itemsControl.ActualWidth / 2, HIT_VERTICAL_OFFSET)));

            GroupItem group = hitTest.VisualHit.FindParent<GroupItem>();
            return group;
        }
    }
}
