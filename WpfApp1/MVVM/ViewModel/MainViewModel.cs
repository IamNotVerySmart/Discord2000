using System;
using System.Collections.Generic;
using System.Linq;
using WpfApp1.Serverek;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.MVVM.Core;
using WpfApp1.MVVM.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace WpfApp1.MVVM.ViewModel
{
     public class MainViewModel
    {
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<string> Messages { get; set; }
        public RelayCommand CTS {  get; set; }
        public RelayCommand SMCommand { get; set; }

        public string Username { get; set; }
        public string Message { get; set; }

        private Server _server;
        public MainViewModel() 
        { 
            Users = new ObservableCollection<UserModel>();
            Messages = new ObservableCollection<String>();
            _server = new Server();
            _server.connectedEvent += UserConnected;
            _server.MsgEvent += UserMsg;
            _server.DisEvent += UserDis;
            CTS = new RelayCommand(o => _server.CTS(Username), o => !string.IsNullOrEmpty(Username));
            SMCommand = new RelayCommand(o => _server.SendMessageToServer(Message), o => !string.IsNullOrEmpty(Message));
        }

        private void UserMsg()
        {
            var msg = _server.PR.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
        }
        private void UserDis()
        {
            var uid = _server.PR.ReadMessage();
            var user = Users.Where(x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
        }

        private void UserConnected()
        {
            var user = new UserModel
            {
                UserName = _server.PR.ReadMessage(),
                UID = _server.PR.ReadMessage(),
            };
            if (!Users.Any(x => x.UID == user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }
    }
}
