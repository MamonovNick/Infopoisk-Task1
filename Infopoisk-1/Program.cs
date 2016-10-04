using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;

namespace Infopoisk_1
{
    class Document
    {
        public String Name;
        public int Count;
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<List<List<Document>>> AllFiles = new List<List<List<Document>>>();
            List<List<Document>> DocCollection;
            List<string> DocLst = new List<string>() { "doc1", "doc2", "doc3", "query1", "query2", "query3" };

            foreach (string s in DocLst)
            {
                DocCollection = new List<List<Document>>();
                //Запуск mystem для текущего файла
                ProcessStartInfo procInfo = new ProcessStartInfo();
                procInfo.FileName = "D://mystem.exe";
                procInfo.Arguments = "-c -s -d -n --format xml D://" + s + ".txt D://out" + s + ".xml";
                Process pr = Process.Start(procInfo);
                pr.WaitForExit();
                //Загрузка полученного файла
                XDocument xdoc = XDocument.Load("D://out" + s + ".xml");

                List<Document> sentance;

                foreach (XElement xelem in xdoc.Element("html").Element("body").Elements("se"))
                {
                    //Действия с предложениями
                    sentance = new List<Document>(from xe in xelem.Elements("w")
                                                  where xe.HasElements
                                                  group xe by xe.Element("ana").Attribute("lex").Value into g
                                                  let cc = g.Count()
                                                  orderby cc descending
                                                  select new Document
                                                  {
                                                      Name = g.Key,
                                                      Count = cc
                                                  });

                    foreach (var i in sentance)
                    {
                        Console.WriteLine("{0} - {1}", i.Name, i.Count);
                    }
                    Console.WriteLine("--------------------------------");
                    DocCollection.Add(sentance);
                }
                AllFiles.Add(DocCollection);
            }
            //Работа с Коллекцией AllFiles
            Console.ReadLine();
        }   
    }
}