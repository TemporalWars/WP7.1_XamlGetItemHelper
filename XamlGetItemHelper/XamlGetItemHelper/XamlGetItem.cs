using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BenScharbach.WP7.Helpers.Xaml.GetItem
{
    /// <summary>
    /// The <see cref="XamlGetItem"/> is used to retrieve some Xaml item from the visual-tree, using .Net built-in <see cref="VisualTreeHelper"/>
    /// and <see cref="ItemContainerGenerator"/> classes.
    /// </summary>
    /// <remarks>
    /// Learned 'VisualTreeHelper' requires the "Generated" items during xaml construction, which can vary because it is based on the number
    /// of items in some collection of (N).
    /// Ref: http://stackoverflow.com/questions/16375375/how-do-i-access-a-control-inside-a-xaml-datatemplate
    /// </remarks>
    public static class XamlGetItem
    {
        /// <summary>
        /// Used to retrieve a visual-item within a generated collection of visual items.
        /// The item is located by the given <paramref name="name"/>.
        /// 
        /// Supports: ListView, ListBox & ItemsControl.
        /// </summary>
        /// <remarks>
        /// Learned 'VisualTreeHelper' requires the "Generated" items during xaml construction, which can vary because it is based on the number
        /// of items in some collection of (N).
        /// Ref: http://stackoverflow.com/questions/16375375/how-do-i-access-a-control-inside-a-xaml-datatemplate
        /// 
        ///  * The ItemsControl is where the ItemContainerGenerator property resides.
        /// 
        /// </remarks>
        public static TCompare FindCollectionVisualItem<TCompare>(ItemsControl itemsControl, int index, string name) where TCompare : UIElement
        {
            if (itemsControl == null) throw new ArgumentNullException("itemsControl");
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            if (itemsControl.ItemContainerGenerator == null) return null;

            // get the generated visual item using the ItemContainerGenerator.
            var generatedItem = itemsControl.ItemContainerGenerator.ContainerFromIndex(index);
            // TODO: For testing.
            return FindVisualItem<TCompare>(generatedItem, name);
        }

        /// <summary>
        /// Used to retrieve a visual-item within a generated collection of visual items.
        /// The item is located by the given <paramref name="name"/>.
        /// 
        /// Supports: ListView, ListBox & ItemsControl.
        /// </summary>
        /// <remarks>
        /// Learned 'VisualTreeHelper' requires the "Generated" items during xaml construction, which can vary because it is based on the number
        /// of items in some collection of (N).
        /// Ref: http://stackoverflow.com/questions/16375375/how-do-i-access-a-control-inside-a-xaml-datatemplate
        /// 
        ///  * The ItemsControl is where the ItemContainerGenerator property resides.
        /// 
        /// </remarks>
        public static TCompare FindCollectionVisualItem<TCompare>(ItemsControl itemsControl, object item, string name) where TCompare : UIElement
        {
            if (itemsControl == null) throw new ArgumentNullException("itemsControl");
            if (item == null) throw new ArgumentNullException("item");
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            if (itemsControl.ItemContainerGenerator == null) return null;

            // get the generated visual item using the ItemContainerGenerator.
            var generatedItem = itemsControl.ItemContainerGenerator.ContainerFromItem(item);
            // TODO: For testing.
            return FindVisualItem<TCompare>(generatedItem, name);
        }

        /// <summary>
        /// Used to retrieve a visual-item within the visual-tree, using the built-in <see cref="VisualTreeHelper"/>.
        /// The item is located by the given <paramref name="name"/>.
        /// 
        /// Supports: Button, Canvas, Grid, ListBox, Rectangle, StackPanel, TextBox, TextBlock, CheckBox & RadioButton.
        /// </summary>
        /// <remarks>
        /// Learned 'VisualTreeHelper' requires the "Generated" items during xaml construction, which can vary because it is based on the number
        /// of items in some collection of (N).
        /// Ref: http://stackoverflow.com/questions/16375375/how-do-i-access-a-control-inside-a-xaml-datatemplate
        /// 
        /// * The FrameworkElement is where the Name property resides.
        /// 
        /// </remarks>
        public static TCompare FindVisualItem<TCompare>(DependencyObject visualObject, string name)
            where TCompare : UIElement
        {
            if (visualObject == null) throw new ArgumentNullException("visualObject");
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return DoFindVisualItem<TCompare>(visualObject, name);
        }

        #region Private Methods

        /// <summary>
        /// Used to retrieve a visual-item within the visual-tree, using the built-in <see cref="VisualTreeHelper"/>.
        /// The item is located by the given <paramref name="name"/>.
        /// 
        /// Supports: Button, Canvas, Grid, ListBox, Rectangle, StackPanel, TextBox, TextBlock, CheckBox & RadioButton.
        /// </summary>
        /// <remarks>
        /// Learned 'VisualTreeHelper' requires the "Generated" items during xaml construction, which can vary because it is based on the number
        /// of items in some collection of (N).
        /// Ref: http://stackoverflow.com/questions/16375375/how-do-i-access-a-control-inside-a-xaml-datatemplate
        /// 
        /// * The FrameworkElement is where the Name property resides.
        /// 
        /// </remarks>
        private static TCompare DoFindVisualItem<TCompare>(DependencyObject visualObject, string name)
            where TCompare : UIElement
        {
            // my example: Start at Grid -> get Stack, first child, then 2 children should be TextBlock -> get 2nd TextBlock.
            var childrenCount = VisualTreeHelper.GetChildrenCount(visualObject);
            for (var i = 0; i < childrenCount; i++)
            {
                // get the children in this Grid!
                var childVisual = VisualTreeHelper.GetChild(visualObject, i);
                var isCompare = childVisual as TCompare;
                if (isCompare == null)
                {
                    return FindVisualItem<TCompare>(childVisual, name);
                }

                // if control has given name, return.
                var controlFrameworkElement = isCompare as FrameworkElement;
                if (controlFrameworkElement == null) continue;
                if (controlFrameworkElement.Name == name) return isCompare;
            }
            return null;
        }

        #endregion

    }
}
