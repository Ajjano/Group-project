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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConsultApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ConnectDB db = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //проверка наличия файла со строкой подключения
                if (File.Exists("config.dat"))
                {
                    // подключение к БД
                    db = ConnectionToDataBase();
                }
                else
                {
                    //создание окна "Настройка подключения к БД"
                    ConnectWindow connect = new ConnectWindow();
                    if (connect.ShowDialog() == true)
                    {
                        db = ConnectionToDataBase();
                    }
                    else
                        this.Close();

                }

 //***************************** тут код создания и вывода плиток **************************************

            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection string was break\nConnection to Data Base is missing\n" + ex.Message);
                this.Close();
            }
        }

        // метод дешифровки потока из файла
        private string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }

        //метод подключения к базе данных
        private ConnectDB ConnectionToDataBase()
        {
            ConnectDB connectDB = null;
            byte[] bytes = null;
            using (FileStream fsSource = new FileStream("config.dat", FileMode.Open, FileAccess.Read))
            {
                bytes = new byte[fsSource.Length];
                int numBytesToRead = (int)fsSource.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    // Read may return anything from 0 to numBytesToRead.
                    int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);
                    // Break when the end of the file is reached.
                    if (n == 0)
                        break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }
            }
            byte[] key = bytes.Skip(0).Take(32).ToArray();
            byte[] iv = bytes.Skip(32).Take(16).ToArray();
            byte[] data = bytes.Skip(48).Take(bytes.Length - 48).ToArray();
            // Decrypt the bytes to a string.
            string roundtrip = DecryptStringFromBytes_Aes(data, key, iv);
            ConnectionStringBuilder connectionString = null;
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(roundtrip)))
            {
                connectionString = (ConnectionStringBuilder)formatter.Deserialize(ms);
            }
            try
            {
                connectDB = new ConnectDB(connectionString.ConnectionString);
            }
            catch (SqlException aex)
            {
                MessageBox.Show(aex.Message);
            }
            return connectDB;
        }


        //для теста
        //private void But_Click(object sender, RoutedEventArgs e)
        //{
        //    NewConsult newConsult = new NewConsult();
        //    WrapPanel.Children.Add(newConsult);
        //}
    }
}
