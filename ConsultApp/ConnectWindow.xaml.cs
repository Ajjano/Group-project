using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
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

namespace ConsultApp
{
    /// <summary>
    /// Interaction logic for ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : Window
    {
        ConnectionStringBuilder connectionString = null;
        public ConnectWindow()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // создание строки подключения к БД
                connectionString = new ConnectionStringBuilder();
                // проверка способа авторизации в базе данных
                if (radioButton2.IsChecked == true)
                {
                    // sql - авторизация
                    if (textBox1.Text != "" && textBox2.Text != "" && passBox.Password != "")
                    {
                        connectionString.DataSource = textBox1.Text + @"\SQLEXPRESS";
                        connectionString.InitialCatalog = "ConsultDB";
                        connectionString.IntegratedSecurity = false;
                        connectionString.UserID = textBox2.Text;
                        connectionString.Password = passBox.Password;
                    }
                    else
                        return;
                }
                else
                {
                    connectionString.DataSource = @".\SQLEXPRESS";
                    connectionString.InitialCatalog = "ConsultDB";
                    connectionString.IntegratedSecurity = true;
                }

                // проверка авторизации, подключение или создание базы
                using(ConnectDB db = new ConnectDB(connectionString.ConnectionString))
                {
                    var room = db.Rooms.FirstOrDefault();
                }

                
                if (radioButton3.IsChecked == true)
                {
                    // заполнение первичными данными таблицы Rooms 
                    using (ConnectDB db = new ConnectDB(connectionString.ConnectionString))
                    {
                        Room room1 = new Room { NumberRoom = 1, Roominess = 18, IsAvailable = true, ToArchive = false };
                        Room room2 = new Room { NumberRoom = 2, Roominess = 18, IsAvailable = true, ToArchive = false };
                        Room room3 = new Room { NumberRoom = 3, Roominess = 18, IsAvailable = true, ToArchive = false };
                        Room room4 = new Room { NumberRoom = 4, Roominess = 18, IsAvailable = true, ToArchive = false };
                        Room room5 = new Room { NumberRoom = 5, Roominess = 18, IsAvailable = true, ToArchive = false };
                        Room room6 = new Room { NumberRoom = 6, Roominess = 18, IsAvailable = true, ToArchive = false };
                        Room room7 = new Room { NumberRoom = 7, Roominess = 18, IsAvailable = true, ToArchive = false };
                        Room room8 = new Room { NumberRoom = 8, Roominess = 18, IsAvailable = true, ToArchive = false };
                        Room room9 = new Room { NumberRoom = 9, Roominess = 18, IsAvailable = true, ToArchive = false };
                        Room room10 = new Room { NumberRoom = 10, Roominess = 18, IsAvailable = true, ToArchive = false };
                        Room room11 = new Room { NumberRoom = 11, Roominess = 18, IsAvailable = true, ToArchive = false };
                        Room room12 = new Room { NumberRoom = 12, Roominess = 18, IsAvailable = true, ToArchive = false };
                        Room room13 = new Room { NumberRoom = 13, Roominess = 18, IsAvailable = true, ToArchive = false };
                        Room room14 = new Room { NumberRoom = 14, Roominess = 18, IsAvailable = true, ToArchive = false };
                        Room room15 = new Room { NumberRoom = 15, Roominess = 18, IsAvailable = true, ToArchive = false };
                        db.Rooms.Add(room1);
                        db.Rooms.Add(room2);
                        db.Rooms.Add(room3);
                        db.Rooms.Add(room4);
                        db.Rooms.Add(room5);
                        db.Rooms.Add(room6);
                        db.Rooms.Add(room7);
                        db.Rooms.Add(room8);
                        db.Rooms.Add(room9);
                        db.Rooms.Add(room10);
                        db.Rooms.Add(room11);
                        db.Rooms.Add(room12);
                        db.Rooms.Add(room13);
                        db.Rooms.Add(room14);
                        db.Rooms.Add(room15);
                        db.SaveChanges();
                    }
                }

                // сериализация строки подключения
                BinaryFormatter formatter = new BinaryFormatter();
                string txt = "";
                using (MemoryStream ms = new MemoryStream())
                {
                    formatter.Serialize(ms, connectionString);
                    byte[] brr = ms.ToArray();
                    txt = Encoding.ASCII.GetString(brr);
                }

                // шифрование и запись в файл сериализованной строки подключения
                using (Aes myAes = Aes.Create())
                {
                    byte[] encrypted = EncryptStringToBytes_Aes(txt, myAes.Key, myAes.IV);
                    byte[] full = new byte[48 + encrypted.Length];
                    Array.Copy(myAes.Key, 0, full, 0, 32);
                    Array.Copy(myAes.IV, 0, full, 32, 16);
                    Array.Copy(encrypted, 0, full, 48, encrypted.Length);
                    using (BinaryWriter bw = new BinaryWriter(File.Open("config.dat", FileMode.Create)))
                    {
                        bw.Write(full);
                    }
                }
                this.DialogResult = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Can not connect to the Data Base \n" + ex.Message + "\nEnter correct login and password");
            }
        }

        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            if(radioButton1.IsChecked==true)
            {
                textBox1.IsEnabled = false;
                textBox2.IsEnabled = false;
                passBox.IsEnabled = false;
            }
            else
            {
                textBox1.IsEnabled = true;
                textBox2.IsEnabled = true;
                passBox.IsEnabled = true;
            }
        }

        private void radioButton3_Checked(object sender, RoutedEventArgs e)
        {
            if (radioButton3.IsChecked == true)
                radioButton4.IsChecked = false;
            else
                radioButton4.IsChecked = true;
        }
        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (System.IO.MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }
    }
}
