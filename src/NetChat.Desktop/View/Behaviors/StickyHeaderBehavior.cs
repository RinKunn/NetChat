using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace NetChat.Desktop.View.Behaviors
{
    //public class StickyHeaderBehavior : Behavior<ItemsControl>
    //{
    //    private ScrollViewer _scrollViewer;
    //    private TextBox _frozenGroupHeaderTextBox;
    //    private readonly int HIT_VERTICAL_OFFSET = 15;

    //    protected override void OnAttached()
    //    {
    //        base.OnAttached();
    //        AssociatedObject.Loaded += OnLoaded;
    //    }

    //    private void OnLoaded(object sender, RoutedEventArgs e)
    //    {
    //        _scrollViewer = AssociatedObject.FindChild<ScrollViewer>();
    //        if (_scrollViewer == null)
    //            throw new ArgumentNullException(nameof(_frozenGroupHeaderTextBox));
    //        _scrollViewer.ScrollChanged += ScrollView_ScrollChanged;

    //        UpdateFrozenGroupHeader();
    //    }

    //    private void ScrollView_ScrollChanged(object sender, ScrollChangedEventArgs e)
    //    {
    //        UpdateFrozenGroupHeader();
    //    }

    //    private void UpdateFrozenGroupHeader()
    //    {
    //        if (AssociatedObject.HasItems)
    //        {
    //            GroupItem group = GetFirstVisibleGroupItem(AssociatedObject);
    //            var prevGroupName = _frozenGroupHeaderTextBox.Text;
    //            _frozenGroupHeaderTextBox.Text = "";

    //            if (group != null)
    //            {
    //                object data = group.Content;
    //                Console.WriteLine("gorup content = {0}", data.GetType());
    //                DateTime date = (DateTime)data.GetType().GetProperty("Name").GetValue(data, null);
    //                _frozenGroupHeaderTextBox.Text = date.ToString("dd-MM-yyyy");
    //            }
    //            else
    //            {
    //                _frozenGroupHeaderTextBox.Text = prevGroupName;
    //            }
    //            _frozenGroupHeaderTextBox.Visibility = string.IsNullOrEmpty(prevGroupName) ? Visibility.Collapsed : Visibility.Visible;
    //        }
    //        else
    //        {
    //            _frozenGroupHeaderTextBox.Visibility = Visibility.Collapsed;
    //        }
                
    //    }

    //    private GroupItem GetFirstVisibleGroupItem(ItemsControl itemsControl)
    //    {
    //        HitTestResult hitTest = null;
    //        VisualTreeHelper.HitTest(
    //            itemsControl,
    //            new HitTestFilterCallback((o) =>
    //            {
    //                if (o is FrameworkElement fwe && !fwe.IsHitTestVisible)
    //                {
    //                    return HitTestFilterBehavior.ContinueSkipSelfAndChildren;
    //                }
    //                return HitTestFilterBehavior.Continue;
    //            }),
    //            new HitTestResultCallback((o) =>
    //            {
    //                hitTest = o;
    //                return HitTestResultBehavior.Stop;
    //            }),
    //            new PointHitTestParameters(new Point(itemsControl.ActualWidth / 2, HIT_VERTICAL_OFFSET)));
    //        GroupItem group = hitTest.VisualHit.FindParent<GroupItem>();
    //        return group;
    //    }



    //    public static readonly DependencyProperty FrozenGroupHeaderProperty =
    //        DependencyProperty.RegisterAttached("FrozenGroupHeader", 
    //            typeof(FrameworkElement), 
    //            typeof(StickyHeaderBehavior), 
    //            new PropertyMetadata(default(FrameworkElement), FrozenGroupHeaderChangedCallback));

    //    public FrameworkElement FrozenGroupHeader
    //    {
    //        get { return (FrameworkElement)GetValue(FrozenGroupHeaderProperty); }
    //        set { SetValue(FrozenGroupHeaderProperty, value); }
    //    }

    //    private static void FrozenGroupHeaderChangedCallback(DependencyObject dependencyObject, 
    //        DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    //    {
    //        if(!(dependencyObject is StickyHeaderBehavior behavior))
    //            throw new ArgumentException("dependencyObject is not 'StickyHeaderBehavior'");
    //        if (!(dependencyPropertyChangedEventArgs.NewValue is TextBox headerTextBox))
    //            throw new ArgumentException("'FrozenGroupHeader' must be TextBox type");

    //        behavior._frozenGroupHeaderTextBox = headerTextBox;
    //        var binding = headerTextBox.GetBindingExpression(TextBox.TextProperty);
            
    //    }
    //}


    public class FreezeGroupHeader : DependencyObject
    {
        public static readonly DependencyProperty IsEnableProperty =
            DependencyProperty.RegisterAttached("IsEnable",
                typeof(bool),
                typeof(FreezeGroupHeader),
                new PropertyMetadata(false, IsEnableChangedCallback));

        public static bool GetIsEnable(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnableProperty);
        }
        public static void SetIsEnable(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnableProperty, value);
        }
        private static void IsEnableChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (!(dependencyObject is ItemsControl itemsControl))
                throw new ArgumentException($"DependencyObject must have type: 'ItemsControl'");
            if((bool)dependencyPropertyChangedEventArgs.NewValue)
                itemsControl.Loaded += ItemsControl_Loaded;
            else
                itemsControl.Loaded -= ItemsControl_Loaded;
        }

        private static void ItemsControl_Loaded(object sender, RoutedEventArgs e)
        {
            var scrollViewer = ((FrameworkElement)sender).FindChild<ScrollViewer>();
            if (scrollViewer == null)
                throw new ArgumentNullException(nameof(scrollViewer));
            scrollViewer.ScrollChanged += (o, ex) => ScrollView_ScrollChanged(sender, ex);
            ScrollView_ScrollChanged(sender, null);
        }

        private static void ScrollView_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var itemsControl = (ItemsControl)sender;
            if (itemsControl.HasItems)
            {
                HitTestResult hitTest = null;
                SetGroupHeader(itemsControl, null);
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
                    new PointHitTestParameters(new Point(itemsControl.ActualWidth / 2, 15)));
                GroupItem group = hitTest?.VisualHit.FindParent<GroupItem>();
                SetGroupHeader(itemsControl, group?.Content);
            }
        }


        public static readonly DependencyProperty GroupHeaderProperty =
            DependencyProperty.RegisterAttached("GroupHeader",
                typeof(object),
                typeof(FreezeGroupHeader),
                new PropertyMetadata(default(object)));

        public static object GetGroupHeader(DependencyObject obj)
        {
            return obj.GetValue(GroupHeaderProperty);
        }

        private static void SetGroupHeader(DependencyObject obj, object value)
        {
            obj.SetValue(GroupHeaderProperty, value);
        }
    }
}
