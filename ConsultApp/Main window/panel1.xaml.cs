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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using System.Data.SqlClient;

namespace ConsultApp
{
    /// <summary>
    /// Логика взаимодействия для panel1.xaml
    /// </summary>
    public partial class panel1 : UserControl
    {
        SqlConnectionStringBuilder builder;
        int numberOfRoom;
        public panel1(int numberOfRoom)
        {
            InitializeComponent();
            this.numberOfRoom = numberOfRoom;
            builder = new SqlConnectionStringBuilder();
            builder.DataSource = @".\SQLEXPRESS";
            builder.InitialCatalog = "ConsultsDb";
            builder.IntegratedSecurity = true;
            MessageBox.Show(builder.ConnectionString);
            using (ConnectDB db = new ConnectDB(builder.ConnectionString))
            {
                User user = new User { Firstnsme = "Oleg", Lastname = "Romanov", Login = "roma_53w", Password = "1223", IsAdmin = false };
                db.Users.Add(user);
                db.SaveChanges();
                Room room = new Room { NumberRoom = 1, Roominess = 12, IsAvailable = true, ToArchive = false };
               // dataGridView1.DataSource = db.Users.Local.ToBindingList();
            }

        }

        public void SelectDataFromDB()
        {
            using(ConnectDB db=new ConnectDB())
            {
                //var doubleLess=db.Consultations.Join(db.Rooms,
                //    p=>p.RoomId, c=>c.RoomId)
            }
        }
    }
}
