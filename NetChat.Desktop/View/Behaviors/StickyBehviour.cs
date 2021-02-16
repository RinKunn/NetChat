using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat.Desktop.View.Behaviors
{
    public class StickyBehviour : Behavior<ListView>
    {
        private ScrollViewer _scrollViewer;
        private TextBlock frozenGroupHeader;


        private GroupItem GetFirstVisibleGroupItem(ItemsControl listview)
        {
            HitTestResult hitTest = VisualTreeHelper.HitTest(listview, new Point(5, 5));
            GroupItem group = FindUpVisualTree<GroupItem>(hitTest.VisualHit);
            return group;
        }

        private static T FindChild<T>(DependencyObject parent, string name = null)
                where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                Console.WriteLine("{0}/{2} item has {1}", i, child.GetType(), childrenCount);
                if (name != null && ((FrameworkElement)child).Name != name)
                    continue;

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
        private static T FindUpVisualTree<T>(DependencyObject initial, string name = null) where T : DependencyObject
        {
            DependencyObject current = initial;
            if (name == null)
            {
                while (current != null && current.GetType() != typeof(T))
                {
                    current = VisualTreeHelper.GetParent(current);
                }
            }
            else
            {
                while (current != null && current.GetType() != typeof(T) && ((FrameworkElement)current).Name != name)
                {
                    current = VisualTreeHelper.GetParent(current);
                }
            }


            return current as T;
        }

        private void GetListViewMargins(ItemsControl listview)
        {
            if (listview.HasItems)
            {
                object o = listview.Items[0];
                ListViewItem firstItem = (ListViewItem)listview.ItemContainerGenerator.ContainerFromItem(o);
                Console.WriteLine("====HasItems ");
                if (firstItem != null)
                {
                    GroupItem group = FindUpVisualTree<GroupItem>(firstItem);
                    Point p = group.TranslatePoint(new Point(0, 0), listview);
                    _listviewHeaderHeight = p.Y; // height of columnheader
                    _listviewSideMargin = p.X; // listview borders
                    Console.WriteLine("_listviewHeaderHeight = {0}, _listviewSideMargin = {1}",
                        _listviewHeaderHeight, _listviewSideMargin);
                }
            }
        }

        private void UpdateFrozenGroupHeader()
        {
            if (this.AssociatedObject.HasItems)
            {
                // Text of frozenGroupHeader
                GroupItem group = GetFirstVisibleGroupItem(this.AssociatedObject);
                if (group != null)
                {
                    object data = group.Content;
                    string str = data.GetType().GetProperty("Name").GetValue(data, null) as string;
                    int count = (data.GetType().GetProperty("ItemCount").GetValue(data, null) as int?).Value;
                    Console.WriteLine("---str = {0} from {1}", str, count);
                    this.frozenGroupHeader.Text = str;  // slight hack
                }
                this.frozenGroupHeader.Visibility = Visibility.Visible;
            }
            else
                this.frozenGroupHeader.Visibility = Visibility.Collapsed;
        }

        private void listview1_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            UpdateFrozenGroupHeader();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer = FindChild<ScrollViewer>(AssociatedObject);
            _scrollViewer.ScrollChanged += listview1_ScrollChanged;
            if (_scrollViewer == null)
                throw new ArgumentNullException(nameof(frozenGroupHeader));
            Console.WriteLine("_scrollViewer inited = {0}", _scrollViewer != null);

            frozenGroupHeader = FindChild<TextBlock>(
                VisualTreeHelper.GetParent(AssociatedObject),
                "frozenGroupHeader");

            if (frozenGroupHeader == null)
                throw new ArgumentNullException(nameof(frozenGroupHeader));
            Console.WriteLine("frozenGroupHeader inited = {0}", frozenGroupHeader != null);

            // Position frozen header
            GetListViewMargins(this.AssociatedObject);

            Thickness margin = this.frozenGroupHeader.Margin;
            margin.Top = _listviewHeaderHeight;
            margin.Right = SystemParameters.VerticalScrollBarWidth + _listviewSideMargin;
            margin.Left = _listviewSideMargin;

            this.frozenGroupHeader.Margin = margin;

            UpdateFrozenGroupHeader();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
        }

        private double _listviewHeaderHeight;
        private double _listviewSideMargin;
    }
}
