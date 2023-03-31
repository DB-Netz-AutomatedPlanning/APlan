using APLan.HelperClasses;
using System;

namespace APLan.ViewModels
{
    public class SymbolsTabViewModel : BaseViewModel
    {
        #region constructor
        public SymbolsTabViewModel()
        {
            loadSymbols();
        }
        #endregion

        #region logic
        /// <summary>
        /// load the signal symbols according to a list of names in the resources.
        /// </summary>
        private void loadSymbols()
        { 
            string s = APLan.Properties.Resources.SymbolsList;
            string[] data = s.Split();
            foreach (string d in data)
            {
                if (d != "")
                {
                    string[] locAndName = d.Split(',');

                    items.Add(new SymbolObject() { loc = new Uri("pack://application:,,," + locAndName[0]), Name = locAndName[1] });
                }
            }
        }
        #endregion
    }

}
