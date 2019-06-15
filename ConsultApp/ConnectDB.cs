using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsultApp
{
    //Класс-контекст для создания/подкдючения БД
    class ConnectDB:DbContext
    {
        public ConnectDB() { }
        public ConnectDB(string connectString) : base(connectString) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
    }
    //Модель таблицы Users
    public class User
    {
        public int UserId { get; set; }
        public string Firstnsme { get; set; }
        public string Lastname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public virtual List<Order> Orders { get; set; }
        public virtual List<Consultation> Consultations { get; set; }
    }
    //Модель таблицы Rooms
    public class Room
    {
        public int RoomId { get; set; }
        public int NumberRoom { get; set; }
        public int Roominess { get; set; }
        public bool IsAvailable { get; set; }
        public bool ToArchive { get; set; }
        public virtual List<Order> Orders { get; set; }
        public virtual List<Consultation> Consultations { get; set; }
    }
    //Модель таблицы Orders
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime DateConsultation { get; set; }
        public DateTime DateResev { get; set; }
        public int NumberPair { get; set; }
        public int UserId { get; set; }
        public string Groups { get; set; }
        public int RoomId { get; set; }
        public int NumberStudents { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime? DateApproved { get; set; }
        public string Comments { get; set; }
        public bool ToArchive { get; set; }
        public User User { get; set; }
        public Room Room { get; set; }
    }
    //Модель таблицы Consultations
    public class Consultation
    {
        public int ConsultationId { get; set; }
        public DateTime DateConsultation { get; set; }
        public DateTime DateResev { get; set; }
        public int NumberPair { get; set; }
        public int UserId { get; set; }
        public string Groups { get; set; }
        public int RoomId { get; set; }
        public int NumberStudents { get; set; }
        public bool IsAvailable { get; set; }
        public bool ToArchive { get; set; }
        public Room Room { get; set; }
        public User User { get; set; }
    }
    //Статический класс хэширования хранимых паролей
    public static class Hashing
    {
        public static string GetHash(string message)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(message));
            return Convert.ToBase64String(hash);
        }
    }
}
