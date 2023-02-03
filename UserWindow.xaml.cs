using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AMONICAirlines
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        AirlinesEntities contex;
        Users user;
        DateTime startTime;
        DateTime endTime;
        public UserWindow(Users user)
        {
            InitializeComponent();
            contex = new AirlinesEntities();
            startTime = DateTime.Now;
            this.user = user;
            userBlock.Text += $" {user.Email}";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            endTime = DateTime.Now;
            TimeSpan span = endTime.Subtract(startTime);
            string time = $"{span.Minutes}:{span.Seconds}:{span.Milliseconds}";
            UserSessions currentSession = new UserSessions
            {
                UserID = user.ID,
                SessionTime = time
            };
            contex.UserSessions.Add(currentSession);
            contex.SaveChanges();
            MessageBox.Show("Время сессии " + time);
        }
    }
}
