using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APLan.Commands;


namespace APLan.HelperClasses
{
    public class CommandItem : EventArgs
    {
        private RelayCommand relayCommand;

        public RelayCommand TheRelayCommand
        {
            get { return relayCommand; }
            set { relayCommand = value; }
        }
        public override String ToString()
        {
            return cmdText;
        }

        public CommandItem(RelayCommand rcmd, string txt, object parameter, string cat, int seq)
        {
            relayCommand = rcmd;
            cmdText = txt;
            parm = parameter;
            timeStamp = DateTime.Now;
            category = cat;
            sequence = seq;
        }


        private DateTime timeStamp;

        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set
            {
                timeStamp = value;

            }
        }
        private string category;
        public string Category
        {
            get { return category; }
            set { category = value; }
        }
        private string cmdText;

        public string CmdText
        {
            get { return cmdText; }
            set
            {
                cmdText = value;
                // OnPropertyChanged("CmdText");
            }
        }
        private object parm;

        public object Parm
        {
            get { return parm; }
            set
            {
                parm = value;
                //OnPropertyChanged("Parm");
            }
        }
       

        private int time;


        public int Time
        {
            get { return time; }
            set { time = value; }
        }

        private int sequence = 0;

        public int Sequence { get { return sequence; } }
    }
}
