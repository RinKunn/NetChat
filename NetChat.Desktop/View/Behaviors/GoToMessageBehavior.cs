using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace NetChat.Desktop.View.Behaviors
{

    public static class Helper
    {
        public static T FindChild<T>(this DependencyObject parent)
                where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (!(child is T childType))
                {
                    foundChild = FindChild<T>(child);
                    if (foundChild != null) break;
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }
            return foundChild;
        }
    }

    public class GoToMessageBehavior : Behavior<ItemsControl>
    {
        private INotifyCollectionChanged _notifier;
        private ScrollViewer _scrollViewer;

        protected override void OnAttached()
        {
            base.OnAttached();
            _notifier = AssociatedObject.Items;
            _notifier.CollectionChanged += ItemsControl_CollectionChanged;
            LastVisibleIndex = -1;

            AssociatedObject.Loaded += (o, e) =>
            {
                _scrollViewer = AssociatedObject.FindChild<ScrollViewer>();
                _scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
            };

            AssociatedObject.Unloaded -= (o, e) =>
            {
                _scrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
            };
        }


        private void ItemsControl_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var itemCollection = (ItemCollection)sender;
            
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                int oldLastIndex = e.NewStartingIndex - 1;

                if (oldLastIndex == -1)
                {
                    LastVisibleIndex = itemCollection.Count - 1;

                }
                else
                {
                    FrameworkElement lastItemBeforeAdding = (FrameworkElement)AssociatedObject.ItemContainerGenerator.ContainerFromIndex(oldLastIndex);
                    if (ItemIsVisible(lastItemBeforeAdding, AssociatedObject))
                    {
                        LastVisibleIndex = itemCollection.Count - 1;
                    }
                }
                
            }
            else if(e.Action == NotifyCollectionChangedAction.Reset && itemCollection.Count > 0)
            {
                LastVisibleIndex = itemCollection.Count - 1;
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.HorizontalChange != 0) return;
            for (int i = AssociatedObject.Items.Count - 1; i > 0; i--)
            {
                FrameworkElement elm = (FrameworkElement)AssociatedObject.ItemContainerGenerator.ContainerFromIndex(i);
                if (ItemIsVisible(elm, AssociatedObject))
                {
                    LastVisibleIndex = i;
                    break;
                }
            }
        }

        private bool ItemIsVisible(FrameworkElement element, FrameworkElement container)
        {
            if (!element.IsVisible)
                return false;

            Rect bounds = element.TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth*0.5, element.ActualHeight*0.5));
            var rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
            return /*rect.Contains(bounds.TopLeft); || */ rect.Contains(bounds.BottomRight);
        }



        public int LastVisibleIndex
        {
            get { return (int)GetValue(LastVisibleIndexProperty); }
            set 
            {
                if (value == (int)GetValue(LastVisibleIndexProperty)) return;
                SetValue(LastVisibleIndexProperty, value);
                if (value >= 0) SetValue(LastVisibleItemProperty, AssociatedObject.Items[value]);
            }
        }

        public static DependencyProperty LastVisibleIndexProperty =
            DependencyProperty.Register("LastVisibleIndex",
                typeof(int),
                typeof(GoToMessageBehavior),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnLastVisibleIndexChanged)));

        private static void OnLastVisibleIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GoToMessageBehavior behavior = (GoToMessageBehavior)d;
            int newValue = (int)e.NewValue;
            if (newValue > 0 && newValue != (int)e.OldValue)
            {
                FrameworkElement item = (FrameworkElement)behavior.AssociatedObject?.ItemContainerGenerator?.ContainerFromIndex(newValue);
                item?.BringIntoView();
            }
        }


        public object LastVisibleItem
        {
            get { return GetValue(LastVisibleItemProperty); }
            set 
            {
                if (object.ReferenceEquals(value, GetValue(LastVisibleItemProperty))) return;
                SetValue(LastVisibleItemProperty, value);
                if(value != null) SetValue(LastVisibleIndexProperty, AssociatedObject.Items.IndexOf(value));
            }
        }

        public static DependencyProperty LastVisibleItemProperty =
            DependencyProperty.Register("LastVisibleItem",
                typeof(object),
                typeof(GoToMessageBehavior));

    }
}
