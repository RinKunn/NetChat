using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace NetChat.Desktop.View.Behaviors
{
    public class DisplayItemBehavior : Behavior<ItemsControl>
    {
        public static DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex",
                typeof(int),
                typeof(DisplayItemBehavior),
                new FrameworkPropertyMetadata(
                    new PropertyChangedCallback(OnSelectedIndexChanged)));


        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue == -1) return;
            DisplayItemBehavior behavior = (DisplayItemBehavior)d;
            int targetIndex = (int)e.NewValue;
            if (targetIndex < 0 || targetIndex >= behavior.AssociatedObject.Items.Count)
                throw new ArgumentOutOfRangeException(nameof(SelectedIndex));

            var targetVisualContainer = (FrameworkElement)behavior
                .AssociatedObject
                .ItemContainerGenerator
                .ContainerFromIndex(targetIndex);
            targetVisualContainer.BringIntoView();
            
        }
    }
}
