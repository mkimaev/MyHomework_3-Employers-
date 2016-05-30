using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyHomework_3
{
    class Menu
    {
        public Dictionary<string, Employer> Data_Emp { get; set; }
        public string config;
        public string Config
        {
            get
            {
                using (FileStream fstream = new FileStream("option.ini", FileMode.OpenOrCreate))
                {
                    byte[] array = new byte[fstream.Length];
                    fstream.Read(array, 0, array.Length);
                    string conf = Encoding.Default.GetString(array);
                    config = conf;
                }
                return config;
            }
            protected set { }
        }
        public void Show ()
        {
            if (Config == "binary")
            {
                BinaryFormatter bf2 = new BinaryFormatter();
                using (FileStream fs = new FileStream("emps.dat", FileMode.OpenOrCreate))
                {
                    Dictionary<string, Employer> data = new Dictionary<string, Employer>();
                    data = (Dictionary<string, Employer>)bf2.Deserialize(fs);
                    Data_Emp = data;
                }
            }

            else if (Config != "binary")
            {
                XmlSerializer xs_2 = new XmlSerializer(typeof(Dictionary<string, Employer>));
                using (FileStream fs_2 = new FileStream("emps.xml", FileMode.OpenOrCreate))
                {
                    Dictionary<string, Employer> data_xml = new Dictionary<string, Employer>();
                    xs_2.Serialize(fs_2, data_xml);
                }
            }

            Dictionary<string, Employer> employers = new Dictionary<string, Employer>();
            employers = Data_Emp;

            while (true)
            {
                Console.WriteLine("\nБаза данных о сотрудниках фирмы Google_Ukraine\n");
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
                            Console.WriteLine("Введите должность:");
                            string post2 = Console.ReadLine();
                            employers.Add(id2, new Employer(name2, age2, post2));
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("\nДобавлена запись");
                            employers[id2].Info();
                            Thread.Sleep(1000);
                            Console.ResetColor();
                            Console.Clear();
                            break;
                        case "info":
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("\nВыполнение запроса...");
                            Thread.Sleep(1100);
                            Console.Clear();
                            foreach (KeyValuePair<string, Employer> p in employers)
                            {
                                p.Value.Info();
                            }
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("\nКол-во сотрудников: {0}", employers.Count);
                            Console.ResetColor();
                            break;
                        case "del":
                            Console.Clear();
                            foreach (KeyValuePair<string, Employer> p in employers)
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.Write("id  " + p.Key);
                                Console.ResetColor();
                                p.Value.Info();
                            }
                            Console.WriteLine("\nВведите ID для удаления:");
                            string num = Console.ReadLine();
                            employers.Remove(num);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("ID {0} удалена из списка", num);
                            Console.ResetColor();
                            Thread.Sleep(1000);
                            Console.Clear();

                            break;
                        case "info_id":
                            foreach (KeyValuePair<string, Employer> p in employers)
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.Write("id  " + p.Key + " - ");
                                Console.ResetColor();
                                Console.WriteLine(p.Value.Post);
                            }
                            Console.WriteLine("\nВведите ID сотрудника:");
                            string id3 = Console.ReadLine();
                            employers[id3].Info();
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
                            empl_2 = employers;
                            if (Config == "binary")
                            {
                                BinaryFormatter bf = new BinaryFormatter();
                                using (FileStream fs = new FileStream("emps.dat", FileMode.OpenOrCreate))
                                {
                                    bf.Serialize(fs, empl_2);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("Данные сохранены (сериализованы через bin)\n");
                                    Console.WriteLine("Сеанс окончен! До свидания!\n");
                                }
                            }
                            else if (Config == "xml")
                            {
                                XmlSerializer xs = new XmlSerializer(typeof(Dictionary<string, Employer>));
                                using (FileStream fs_2 = new FileStream("emps.xml", FileMode.OpenOrCreate))
                                {
                                    xs.Serialize(fs_2, empl_2);
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                                    Console.WriteLine("Данные сохранены (сериализованы через xml)\n");
                                    Console.WriteLine("Сеанс окончен! До свидания!\n");
                                }
                            }
                            else { Console.WriteLine("ошибка"); }
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
    }
}
