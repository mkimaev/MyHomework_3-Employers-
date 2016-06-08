using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyHomework_3
{
    [Serializable]
    public class Employer : Person
    {
        [XmlElement]
        public string Company { get; set; }
        public string Post { get; set; }
        public Employer() { }
        public Employer (string name, int age, string company,string post) : base (name, age)
        {
            Post = post;
            Company = company;
        }
    }
}
