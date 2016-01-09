using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace Pyratron.PyraChat.UI.Helpers
{
    public static class ItemsControlHelper
    {
        public static readonly DependencyProperty ScrollToLastItemProperty =
            DependencyProperty.RegisterAttached("ScrollToLastItem",
                typeof(bool), typeof(ItemsControlHelper),
                new FrameworkPropertyMetadata(false, OnScrollToLastItemChanged));

        public static void SetScrollToLastItem(UIElement sender, bool value)
        {
            sender.SetValue(ScrollToLastItemProperty, value);
        }

        public static bool GetScrollToLastItem(UIElement sender)
        {
            return (bool)sender.GetValue(ScrollToLastItemProperty);
        }

        private static void OnScrollToLastItemChanged(DependencyObject sender,
                                DependencyPropertyChangedEventArgs e)
        {
            var itemsControl = sender as ItemsControl;

            if (itemsControl != null)
            {
                itemsControl.ItemContainerGenerator.StatusChanged +=
                         (s, a) => OnItemsChanged(itemsControl, s, a);
            }
        }

        static void OnItemsChanged(ItemsControl itemsControl, object sender, EventArgs e)
        {
            var generator = sender as ItemContainerGenerator;
            if (generator.Status == GeneratorStatus.ContainersGenerated)
            {
                if (itemsControl.Items.Count > 0)
                {
                    ScrollIntoView(itemsControl,
                     itemsControl.Items[itemsControl.Items.Count - 1]);
                }
            }
        }

        private static void ScrollIntoView(ItemsControl itemsControl, object item)
        {
            if (itemsControl.ItemContainerGenerator.Status ==
                GeneratorStatus.ContainersGenerated)
            {
                OnBringItemIntoView(itemsControl, item);
            }
            else
            {
                Func<object, object> onBringIntoView =
                        (o) => OnBringItemIntoView(itemsControl, item);
                itemsControl.Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
                      new DispatcherOperationCallback(onBringIntoView));
            }
        }

        private static object OnBringItemIntoView(ItemsControl itemsControl, object item)
        {
            var element = itemsControl.ItemContainerGenerator.
                     ContainerFromItem(item) as FrameworkElement;
            element?.BringIntoView();
            return null;
        }
    }
}
