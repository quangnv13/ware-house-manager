using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WareHouse_Manager.Model;

namespace WareHouse_Manager.ViewModel
{
    public class LoginViewModel:BaseViewModel
    {
        public bool isLogin { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }

        private string _username;
        private string _password;
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }

        public LoginViewModel()
        {
            isLogin = false;
            Password = "";
            Username = "";
            PasswordChangedCommand = new RelayCommand<PasswordBox>(x => { return true; }, x => { Password = x.Password; });
            LoginCommand = new RelayCommand<Window>(x => { return true; }, x => { Login(x); });
            CloseCommand = new RelayCommand<Window>(x => { return true; }, x => { x.Close(); });
        }
        void Login(Window x)
        {
            if (DataProvider.Instance.DB.USERS.Where(r => r.ID == Username && r.PASSWORD == Password).Count() > 0)
            {
                isLogin = true;
                x.Close();
            }
            else
            {
                isLogin = false;
                MessageBox.Show("Sai tài khoản hoặc mật khẩu");
            }
        }
    }
}
