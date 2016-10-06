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
    class Word
    {
        public string Name;
        public int Count;
        public Word() { Name = ""; Count = 0; }
        public Word(string str, int c = 0) { Name = str; Count = c; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<List<List<Word>>> AllFiles = new List<List<List<Word>>>();
            List<List<Word>> DocCollection;
            List<string> DocLst = new List<string>() { "doc1", "doc2", "doc3", "query1", "query2", "query3" };
            List<double> hdf;
            List<double> wdq;
            List<double> hdq;
            List<double> relevantList;
            
            //заполнение коллекции документов
            foreach (string s in DocLst)
            {
                DocCollection = new List<List<Word>>();
                //Запуск mystem для текущего файла
                //ProcessStartInfo procInfo = new ProcessStartInfo();
                //procInfo.FileName = "D://mystem.exe";
                //procInfo.Arguments = "-c -s -d -n --format xml D://" + s + ".txt D://out" + s + ".xml";
                //Process pr = Process.Start(procInfo);
                //pr.WaitForExit();
                //Загрузка полученного файла
                XDocument xdoc = XDocument.Load("D://out" + s + ".xml");
                List<Word> sentance; //все слова в предложении-документе, группированные и сортированные по встречаемости
                foreach (XElement xelem in xdoc.Element("html").Element("body").Elements("se"))
                {
                    //Действия с предложениями-документами
                    sentance = new List<Word>(from xe in xelem.Elements("w")
                                                  where xe.HasElements
                                                  group xe by xe.Element("ana").Attribute("lex").Value into g
                                                  let cc = g.Count()
                                                  orderby cc descending
                                                  select new Word
                                                  {
                                                      Name = g.Key,
                                                      Count = cc
                                                  });
                    //int sum = sentance.Sum(x => x.Count);
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
            for(int i = 0; i < 3; i++)
            {
                relevantList = new List<double>();
                hdf = new List<double>();
                double qhdf = 0;
                wdq = new List<double>();
                hdq = new List<double>();
                int sum = 0;

                for (int j = 0; j < AllFiles[i].Count; j++)
                {
                    int wdqSum = 0;
                    foreach (Word w in AllFiles[i][j])
                    {
                        sum += w.Count * w.Count;
                    }
                    foreach (Word w in AllFiles[i + 3][0])
                    {
                        Word tmp = AllFiles[i][j].Find(x => x.Name == w.Name);
                        wdqSum += (tmp == null ? 0 : tmp.Count) * w.Count;
                    }
                    wdq.Add(wdqSum);
                    hdf.Add(Math.Sqrt(sum));
                }
                foreach (Word w in AllFiles[i + 3][0])
                {
                    qhdf += w.Count * w.Count;
                }
                foreach(var d in hdf)
                {
                    hdq.Add(d * qhdf);
                }
                for(int j = 0; j < wdq.Count; j++)
                {
                    relevantList.Add(wdq[j]/hdq[j]);
                    Console.WriteLine(relevantList[j]);
                }
                Console.WriteLine("++++++++++++++++++++++++++++++");
            }
            Console.ReadLine();

        }
    }
}