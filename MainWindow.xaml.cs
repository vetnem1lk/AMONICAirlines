using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AMONICAirlines
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AirlinesEntities contex;
        DispatcherTimer _timer;
        TimeSpan _time;
        int loginAttempts = 0;

        public MainWindow()
        {
            InitializeComponent();
            contex = new AirlinesEntities();
        }

        private void ExitButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {           
            this.Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var login = loginBox.Text;
            var pass = passBox.Password.ToString();
            var user = checkAuthorization(login, pass);
            if (user != null)
            {
                loginAttempts = 0;
                if (isActive(user))
                {
                    if (isAdmin(user))
                    {
                        AdministratorWindow adminWindow = new AdministratorWindow(user);
                        adminWindow.Show();
                    }                        
                    else
                    {
                        UserWindow userWindow = new UserWindow(user);
                        userWindow.Show();
                    }
                    this.Close();
                }
                else
                {
                    errorText.Text = "Вы заблокированы в системе!";
                }
            } 
            else
            {
                checkAttemts();
            }
        }

        private Users checkAuthorization(string login, string pass)
        {
            var result = (from u in contex.Users
                          where u.Email == login && u.Password == pass
                          select u).FirstOrDefault();
            
            return result;
        }
        private bool isActive(Users user)
        {
            return (bool)user.Active;
        }
        private bool isAdmin(Users user)
        {
            return user.RoleID == 1 ? true : false;
        }
        private void checkAttemts()
        {
            loginAttempts++;
            if (loginAttempts == 3)
            {
                _time = TimeSpan.FromSeconds(10);
                _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
                {
                    errorText.Text = "Введено максимальное количество попыток. Повторите через: " + _time.ToString("ss");
                    LoginButton.IsEnabled = false;
                    if (_time == TimeSpan.Zero)
                    {
                        _timer.Stop();
                        LoginButton.IsEnabled = true;
                        errorText.Text = "";
                        loginAttempts = 0;
                    }
                    _time = _time.Add(TimeSpan.FromSeconds(-1));
                }, Application.Current.Dispatcher);
                _timer.Start();
            }
        }

        private void loginBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            errorText.Text = "";
        }
    }
}
