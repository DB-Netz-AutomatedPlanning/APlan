using APLan.Commands;
using APLan.Model.CustomObjects;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
namespace APLan.HelperClasses
{
    /// <summary>
    /// object to contain information about Signals to be loaded on the signals tab.
    /// </summary>
    public class SymbolObject
    {
        private RelayCommand _mouseDownCommand;
        public RelayCommand MouseDownCommand
        {
            get
            {
                if (_mouseDownCommand == null) return _mouseDownCommand = new RelayCommand(param => ExecuteMouseDown((MouseEventArgs)param));
                return _mouseDownCommand;
            }
            set { _mouseDownCommand = value; }
        }

        public string Name { get; set; }
        public Uri loc { get; set; }

        private void ExecuteMouseDown(MouseEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                CustomCanvasSignal customSignal = new CustomCanvasSignal
                {
                    Source = (System.Windows.Media.Imaging.BitmapFrame)((Image)e.Source).Source,
                    Width = 10,
                    Height = 10
                };
                CustomProperites.SetScale(customSignal, 1);
                CustomProperites.SetRotation(customSignal, 0);
                DragDrop.DoDragDrop(customSignal, new DataObject(DataFormats.Serializable, customSignal), DragDropEffects.Move);
            }
        }

    }
}
