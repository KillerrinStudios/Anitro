using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Anitro.Controls
{
    public sealed partial class ExpanderView : UserControl
    {
        public ExpanderView()
        {
            this.InitializeComponent();
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                LayoutRoot.DataContext = this;
            }
        }

        #region Header Template
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set
            {
                SetValue(HeaderTemplateProperty, value);
            }
        }
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(ExpanderView), new PropertyMetadata(new DataTemplate(), OnDataTemplatePropertyChanged));

        private static void OnDataTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExpanderView view = (d as ExpanderView);
        }
        #endregion

        #region Expanded
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set
            {
                SetValue(IsExpandedProperty, value);
            }
        }
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(ExpanderView), new PropertyMetadata(false, OnIsExpandedPropertyChanged));

        private static void OnIsExpandedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExpanderView view = (d as ExpanderView);
            view.HeaderButton.IsChecked = view.IsExpanded;
        }
        #endregion

        #region Content Template
        public DataTemplate ContentItemTemplate
        {
            get { return (DataTemplate)GetValue(ContentItemTemplateProperty); }
            set
            {
                SetValue(ContentItemTemplateProperty, value);
            }
        }
        public static readonly DependencyProperty ContentItemTemplateProperty =
            DependencyProperty.Register(nameof(ContentItemTemplate), typeof(DataTemplate), typeof(ExpanderView), new PropertyMetadata(new DataTemplate(), OnContentItemTemplatePropertyChanged));

        private static void OnContentItemTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExpanderView view = (d as ExpanderView);
            view.ContentListView.ItemTemplate = view.ContentItemTemplate;

        }
        #endregion

        #region Content Template
        public object ContentItemsSource
        {
            get { return (object)GetValue(ContentItemsSourceProperty); }
            set
            {
                SetValue(ContentItemsSourceProperty, value);
            }
        }
        public static readonly DependencyProperty ContentItemsSourceProperty =
            DependencyProperty.Register(nameof(ContentItemsSource), typeof(object), typeof(ExpanderView), new PropertyMetadata(new object(), OnContentItemsSourcePropertyChanged));

        private static void OnContentItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExpanderView view = (d as ExpanderView);
            view.ContentListView.ItemsSource = view.ContentItemsSource;
        }
        #endregion


        #region ItemClicked Command
        public RelayCommand<object> ItemClickedCommand
        {
            get { return (RelayCommand<object>)GetValue(ItemClickedCommandProperty); }
            set
            {
                if (value == null) return;
                SetValue(ItemClickedCommandProperty, value);
            }
        }
        public static readonly DependencyProperty ItemClickedCommandProperty =
            DependencyProperty.Register(nameof(ItemClickedCommand), typeof(RelayCommand<object>), typeof(ExpanderView), new PropertyMetadata(new RelayCommand<object>((s) => { })));

        private void ContentListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Debug.WriteLine("ExpanderView: Item Clicked");
            //if (ItemClickedCommand.CanExecute(e.ClickedItem))
            //    ItemClickedCommand.Execute(e.ClickedItem);
        }
        #endregion

        private void ContentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("ExpanderView: Selection Changed");
            if (ItemClickedCommand.CanExecute(ContentListView.SelectedItem))
                ItemClickedCommand.Execute(ContentListView.SelectedItem);
        }
    }
}
