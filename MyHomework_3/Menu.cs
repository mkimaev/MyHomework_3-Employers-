using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace MyHomework_3
{
    
    public class Menu
    {
        public Dictionary<string, Employer> Data_Emp { get; set; }
        private string config;
        public string Config
        {
            get
            {
                FileInfo fi = new FileInfo("option.ini");
                if (fi.Exists)
                {
                    using (FileStream fstream = new FileStream("option.ini", FileMode.OpenOrCreate))
                    {
                        byte[] array = new byte[fstream.Length];
                        fstream.Read(array, 0, array.Length);
                        string conf = Encoding.Default.GetString(array);
                        config = conf;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ошибка загрузки!\nНе найден конфигурационный файл \"option.ini\". ");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Выполняется создание конфигурационного файла option.ini, пожалуйста подождите...");
                    Thread.Sleep(4000);
                    using (FileStream fstream2 = new FileStream("option.ini", FileMode.CreateNew))
                    {
                        string text = "binary";
                        byte[] array = Encoding.Default.GetBytes(text);
                        fstream2.Write(array, 0, array.Length);
                        config = text;
                    }
                    Console.WriteLine("Конфигурационный файл option.ini (value = binary) создан!");
                    Console.ResetColor();
                }
                return config;
            }
        }
        public void Show ()
        {
            Data_Emp = new Dictionary<string, Employer>();
            FileInfo fi2 = new FileInfo("emps.dat");
            FileInfo fi3 = new FileInfo("pers.xml");
            if (fi2.Exists | fi3.Exists)
            {
                if (Config == "binary")
                {
                    BinaryFormatter bf2 = new BinaryFormatter();
                    using (FileStream fs = new FileStream("emps.dat", FileMode.Open))
                    {
                        Dictionary<string, Employer> data = new Dictionary<string, Employer>();
                        data = (Dictionary<string, Employer>)bf2.Deserialize(fs);
                        Data_Emp = data;
                    }
                }
                else if (Config == "xml")
                {
                    //    XmlSerializer xs_2 = new XmlSerializer(typeof(Dictionary<string, Employer>));
                    //    using (FileStream fs_2 = new FileStream("emps.xml", FileMode.OpenOrCreate))
                    //    {
                    //        Dictionary<string, Employer> data_xml = new Dictionary<string, Employer>();
                    //        xs_2.Serialize(fs_2, data_xml);
                    //    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("В конфигурационном файле option.ini указаны некорректные данные - {0}", Config);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Выполняется запись по умолчанию (binary) в конфигурационный файл option.ini...");
                    Thread.Sleep(2000);
                    using (FileStream fstream2 = new FileStream("option.ini", FileMode.Truncate))
                    {
                        string text = "binary";
                        byte[] array = Encoding.Default.GetBytes(text);
                        fstream2.Write(array, 0, array.Length);
                    }
                    Console.WriteLine("Конфигурационные данные перезаписаны!");
                    Console.ResetColor();
                    Show();
                }
            }
            else
            {
                Console.WriteLine("Отсутвует файл для десеарилации emps.dat или pers.xml. Данные с последнего сеанса не загружены!\n");
            }

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\nКол-во сотрудников в базе: {0}", Data_Emp.Count);
                Console.ResetColor();
                Console.WriteLine("\nБаза данных о сотрудниках фирмы\n");
                Console.Write("Введите команду: ");
                try
                {
                    string command = Console.ReadLine();
                    Console.Clear();
                    switch (command.ToLower())
                    {
                        case "add":
                            Console.WriteLine("Введите уникальный № ID:");
                            string id2 = Console.ReadLine();
                            Console.WriteLine("Введите имя:");
                            string name2 = Console.ReadLine();
                            Console.WriteLine("Введите возраст:");
                            int age2 = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Введите название фирмы:");
                            string company2 = Console.ReadLine();
                            Console.WriteLine("Введите должность:");
                            string post2 = Console.ReadLine();
                            Data_Emp.Add(id2, new Employer(name2, age2, company2, post2));
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("\nДобавлена запись");
                            ShowInfo_ID(id2);
                            Thread.Sleep(2000);
                            Console.ResetColor();
                            Console.Clear();
                            break;
                        case "info":
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("\nВыполнение запроса...");
                            Thread.Sleep(1100);
                            Console.Clear();
                            ShowInfo();
                            Console.ResetColor();
                            break;
                        case "del":
                            Console.Clear();
                            ShowInfo();
                            Console.WriteLine("\nВведите ID для удаления:");
                            string num = Console.ReadLine();
                            Data_Emp.Remove(num);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("ID {0} удалена из списка", num);
                            Console.ResetColor();
                            break;
                        case "info_id":
                            foreach (KeyValuePair<string, Employer> p in Data_Emp)
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.Write("id  " + p.Key + " - ");
                                Console.ResetColor();
                                Console.WriteLine(p.Value.Post);
                            }
                            Console.WriteLine("\nВведите ID сотрудника:");
                            string id3 = Console.ReadLine();
                            ShowInfo_ID(id3);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nКоманда выполнена");
                            Console.ResetColor();
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\nДоступные команды:\n \nadd - добавить запись \ndel - удалить запись ");
                            Console.WriteLine("info - посмотреть записи о всех сотрудниках \ninfo_id - посмотреть запись о конктретном сотруднике");
                            Console.WriteLine("exit - выйти из программы. Данные сохранятся автоматически");
                            Console.WriteLine("exit* - выйти из программы без сохранения");
                            Console.ResetColor();
                            break;
                        case "exit":
                            Dictionary<string, Employer> empl_2 = new Dictionary<string, Employer> ();
                            empl_2 = /*employers*/ Data_Emp;
                            if (Config == "binary")
                            {
                                BinaryFormatter bf = new BinaryFormatter();
                                using (FileStream fs = new FileStream("emps.dat", FileMode.OpenOrCreate))
                                {
                                    bf.Serialize(fs, empl_2);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("Данные сохранены (сериализованы через BinaryFormatter)");
                                    Console.WriteLine("Сеанс окончен! До свидания!\n");
                                }
                            }
                            else if (Config == "xml")
                            {
                                XmlSerializer xs = new XmlSerializer(typeof(Dictionary<string, Employer>));
                                using (FileStream fs_2 = new FileStream("pers.xml", FileMode.OpenOrCreate))
                                {
                                    xs.Serialize(fs_2, empl_2);
                                    Console.WriteLine("Данные сохранены (сериализованы через XmlSerializer)");
                                    Console.WriteLine("Сеанс окончен! До свидания!\n");
                                }
                            }
                            return;
                        case "exit*":
                            Console.WriteLine("Сеанс окончен! До свидания!\n");
                            return;
                    }
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Ошибка ввода \n" + e);
                    Console.ResetColor();
                }
            }
        }
        public void ShowInfo()
        {
            foreach (KeyValuePair<string, Employer> person in Data_Emp)
            {
                string s1 = String.Format("Id {4} | Имя: {0} | Возраст: {1} | Компания: {2} | Должность: {3} |", person.Value.Name, person.Value.Age, person.Value.Company, person.Value.Post, person.Key);
                Console.WriteLine(new String('-', s1.Length));
                Console.WriteLine(s1);
            }
        }
        public void ShowInfo_ID(string idm)
        {
                string s1 = String.Format("Имя: {0} | Возраст: {1} | Компания: {2} | Должность: {3} |", Data_Emp[idm].Name, Data_Emp[idm].Age, Data_Emp[idm].Company, Data_Emp[idm].Post);
                Console.WriteLine(new String('-', s1.Length));
                Console.WriteLine(s1);
        }
    }

}
