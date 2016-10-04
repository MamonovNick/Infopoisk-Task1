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
        String Name;
        int count;
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<Dictionary<string, int>> Docs = new List<Dictionary<string, int>>();
            //List<string> DocLst = new List<string>() { "doc1", "doc2", "doc3", "query1", "query2", "query3" };
            List<string> DocLst = new List<string>() { "doc3" };
            foreach (string s in DocLst)
            {
                Dictionary<string, int> Doc = new Dictionary<string, int>();

                //ProcessStartInfo procInfo = new ProcessStartInfo();
                //procInfo.FileName = "D://mystem.exe";
                //procInfo.Arguments = "-c -s -d -n --format xml D://"+ s +".txt D://out"+ s +".xml";

                //Process pr = Process.Start(procInfo);
                //pr.WaitForExit();

                XDocument xdoc = XDocument.Load("D://out" + s + ".xml");

                Document doc_tmp = new Document();

                foreach (XElement xelem in xdoc.Element("html").Element("body").Elements("se"))
                {

                    //Действия с предложениями
                    var smth = from xe in xelem.Elements("w")
                               group xe by xe.Element("ana").Attribute("lex").Value into g
                               let cc = g.Count()
                               orderby cc descending
                               select new
                               {
                                   Name = g.Key,
                                   Count = cc,
                                   //Names = from p in g select p
                               };

                    foreach (var i in smth)
                    {
                        Console.WriteLine("{0} - {1}", i.Name, i.Count);
                    }
                    Console.WriteLine("--------------------------------");
                    //foreach (XElement xelem2 in xelem.Elements("w"))
                    //{

                    //    //XAttribute xattr = xelem2.Attribute("lex");
                    //    //Console.WriteLine(xattr);
                    //    Console.WriteLine(xelem2.Element("ana").Attribute("lex").Value);
                    //}

                }

                //XmlDocument xDoc = new XmlDocument();
                //xDoc.Load("D://out" + s + ".xml");
                //// получаем корневой элемент
                //XmlNode xRoot = xDoc.DocumentElement.FirstChild;
                //// обход всех узлов в корневом элементе
                //foreach (XmlNode xnode in xRoot)
                //{
                //    //<se>
                //    foreach (XmlNode childnode in xnode.ChildNodes)
                //    {
                //        //<w>
                //        foreach (XmlNode childnode2 in childnode.ChildNodes)
                //        {
                //            //<ana>
                //            if (childnode2.Attributes != null)
                //            {
                //                XmlNode attr = childnode2.Attributes.GetNamedItem("lex");
                //                if (attr != null)
                //                {
                //                    Console.WriteLine(attr.Value);
                //                    Doc.Add(attr.Value, 1);
                //                }
                //            }
                //        }
                //    }
                //}
                //Docs.Add(Doc);
                //Doc = null;
            }
            //Console.WriteLine();
            Console.ReadLine();
        }   
    }
}