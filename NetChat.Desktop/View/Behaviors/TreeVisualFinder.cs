using System;
using System.Windows;
using System.Windows.Media;

namespace NetChat.Desktop.View.Behaviors
{
    public static class TreeVisualFinder
    {
        public static T FindChild<T>(this DependencyObject parent)
                where T : DependencyObject
        {
            if (parent == null) return null;

            T foundChild = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (!(child is T))
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

        public static T FindChild<T>(this DependencyObject parent, string name)
                where T : DependencyObject
        {
            if (parent == null) return null;
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            T foundChild = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (!(child is T) || ((FrameworkElement)child).Name != name)
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

        public static T FindParent<T>(this DependencyObject initial)
            where T : DependencyObject
        {
            DependencyObject current = initial;
            while (current != null && current.GetType() != typeof(T))
            {
                current = VisualTreeHelper.GetParent(current);
            }
            return current as T;
        }

        public static T FindParent<T>(this DependencyObject initial, string name = null)
            where T : DependencyObject
        {
            DependencyObject current = initial;
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            while (current != null
                && current.GetType() != typeof(T)
                && ((FrameworkElement)current).Name != name)
            {
                current = VisualTreeHelper.GetParent(current);
            }

            return current as T;
        }
    }
}
