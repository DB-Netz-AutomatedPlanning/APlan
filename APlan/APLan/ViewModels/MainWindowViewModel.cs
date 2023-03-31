using APLan.Commands;
using APLan.HelperClasses;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Media;
using APLan.Model.CustomObjects;

namespace APLan.ViewModels
{
    public class MainWindowViewModel :BaseViewModel
    {
        #region Fields
        private RelayCommand _KeyDownForMainWindow;
        #endregion
        #region Properties
        public RelayCommand KeyDownForMainWindow
        {
            get
            {
                return _KeyDownForMainWindow ??= new RelayCommand(
                   x =>
                   {
                       ExecuteKeyDownPressed((KeyEventArgs)x);
                   });
            }
        }
        #endregion
        #region command logic
        /// <summary>
        /// apply keydown on the main window.
        /// </summary>
        /// <param name="e"></param>
        public void ExecuteKeyDownPressed(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    deleteSelected();
                    break;
                case Key.Escape:
                    CancelSelected();
                    ResetToolSelection();
                    removeHelperItemsFromCanvas();
                    DrawViewModel.resetPointers();
                    System.Windows.Application.Current.Resources["arrow"] = System.Windows.Input.Cursors.Arrow;
                    break;
            }
        }
        #endregion
        #region logic
        /// <summary>
        /// delet selected items.
        /// </summary>
        public void deleteSelected()
        {
            for (int i = 0; i < selected.Count; i++)
            {
                if (selected[i].GetType() != typeof(System.Windows.Shapes.Path) && selected[i].GetType() != typeof(CustomSignal))
                {

                    Canvas c = (Canvas)LogicalTreeHelper.GetParent(selected[i]);
                    if (c != null && c.Children.Contains(selected[i]))
                    {
                        c.Children.Remove(selected[i]);
                        toBeStored.Remove((UIElement)selected[i]);
                        selected.Remove(selected[i]);
                        i--;
                    }
                    else // it is a loaded object
                    {
                        if (selected[i] is CustomCanvasSignal)
                        {
                            ((ObservableCollection<CanvasObjectInformation>)findItemControlParent(selected[i]).ItemsSource).Remove((CanvasObjectInformation)((CustomCanvasSignal)selected[i]).DataContext);
                            // loadedObjects.Remove((CanvasObjectInformation)((CustomCanvasSignal)selected[i]).DataContext);
                        }
                        else if (selected[i] is CustomCanvasText)
                        {
                            ((ObservableCollection<CanvasObjectInformation>)findItemControlParent(selected[i]).ItemsSource).Remove((CanvasObjectInformation)((CustomCanvasText)selected[i]).DataContext);
                            // loadedObjects.Remove((CanvasObjectInformation)((CustomCanvasText)selected[i]).DataContext);
                            toBeStored.Remove(selected[i]);
                        }

                    }
                }
                else if (selected[i].GetType() == typeof(System.Windows.Shapes.Path))
                {
                    //if ((findItemControlParent(selected[i]).ItemsSource).GetType() == typeof(ObservableCollection<CustomPolyLine>))
                    //{
                    //    ((ObservableCollection<CustomPolyLine>)findItemControlParent(selected[i]).ItemsSource).Remove((CustomPolyLine)((Polyline)selected[i]).DataContext);
                    //}
                    //else if ((findItemControlParent(selected[i]).ItemsSource).GetType() == typeof(ObservableCollection<CustomArc>))
                    //{
                    //    ((ObservableCollection<CustomArc>)findItemControlParent(selected[i]).ItemsSource).Remove((CustomArc)((System.Windows.Shapes.Ellipse)selected[i]).DataContext);
                    //}

                    ItemsControl c = findItemControlParent(selected[i]);


                }
            }

        }
        /// <summary>
        /// find the itemControl that contains a selection to delete the objects from the corresponding bounded list.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public ItemsControl findItemControlParent(UIElement e)
        {
            ItemsControl c = null;
            e = (UIElement)VisualTreeHelper.GetParent(e);
            while (e != null)
            {

                //if (e is Canvas && !Double.IsNaN(((Canvas)e).Width))
                //{
                //    return (Canvas)e;
                //}
                e = (UIElement)VisualTreeHelper.GetParent(e);

                if (e is ItemsControl)
                {
                    return (ItemsControl)e;
                    //((ItemsControl)e).ItemsSource = null;
                    // ((ObservableCollection<CanvasObjectInformation>)((ItemsControl)e).ItemsSource).Remove((CanvasObjectInformation)temp.DataContext);
                }
            }
            return c;
        }
        public void childRemover(UIElement element)
        {
            if (element != null)
            {
                var canvas = HelperClasses.VisualTreeHelpers.FindAncestor<Canvas>(element);
                if (canvas != null && canvas.Children.Contains(element))
                {
                    canvas.Children.Remove(element);
                }
            }
        }
        /// <summary>
        /// remove temporary items. like rectangles,lines.. used in drawing.
        /// </summary>
        public void removeHelperItemsFromCanvas()
        {
            childRemover(DrawViewModel.indicationLine);
            childRemover(DrawViewModel.selectionRectangle);
        }
        #endregion
    }
}
