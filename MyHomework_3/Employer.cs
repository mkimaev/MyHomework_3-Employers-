using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHomework_3
{
    [Serializable]
    class Employer : Person
    {
        private string company;
        public string Company
        {
            set { }
            get { company = "Poogle";
                  return company; }
        }
        public string Post { get; set; }
        public Employer() { }
        public Employer (string name, int age, string post) : base (name, age)
        {
            Post = post;
        }
        public void Info()
        {
            string s1 = String.Format("Имя: {0} | Возраст: {1} | Компания: {2} | Должность: {3} |", Name, Age, Company, Post);
            Console.WriteLine(new String('-', s1.Length));
            Console.WriteLine(s1);
        }

    }
}
