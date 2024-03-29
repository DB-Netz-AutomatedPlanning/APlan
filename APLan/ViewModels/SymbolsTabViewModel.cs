﻿using APLan.Commands;
using APLan.HelperClasses;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace APLan.ViewModels
{
    public class SymbolsTabViewModel : BaseViewModel
    {
        #region attributes
        public ObservableCollection<SymbolObject> items
        {
            get;
            set;
        }
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

        #endregion

        #region constructor
        public SymbolsTabViewModel()
        {
            loadSymbols();
            
        }
        #endregion

        #region logic
        private void ExecuteMouseDown(MouseEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                CustomCanvasSignal customSignal = new CustomCanvasSignal
                {
                    Source = (BitmapFrame)((Image)e.Source).Source,
                    Width = 10,
                    Height = 10
                };
                CustomProperites.SetScale(customSignal, 1);
                CustomProperites.SetRotation(customSignal, 0);
                DragDrop.DoDragDrop(customSignal, new DataObject(DataFormats.Serializable, customSignal), DragDropEffects.Move);
            }
        }

        /// <summary>
        /// load the signal symbols according to a list of names in the resources.
        /// </summary>
        private void loadSymbols()
        {
            ObservableCollection<SymbolObject> newItems = new ObservableCollection<SymbolObject>();
            string s = APLan.Properties.Resources.SymbolsList;
            string[] data = s.Split();
            foreach (string d in data)
            {
                if (d != "")
                {
                    string[] locAndName = d.Split(',');
                    
                    newItems.Add(new SymbolObject() { loc = new Uri("pack://application:,,," + locAndName[0]), Name = locAndName[1] });
                }
            }
            items = newItems;
        }

        
        #endregion
    }

}
