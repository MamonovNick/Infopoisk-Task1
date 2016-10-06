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
        static List<List<List<Word>>> AllFiles = new List<List<List<Word>>>();
        static List<List<Word>> DocCollection;
        static List<string> DocLst = new List<string>() { "doc1", "doc2", "doc3", "query1", "query2", "query3" };
        static List<Word> idf;
        static List<double> hdf;
        static List<double> wdq;
        static List<double> hdq;
        static List<double> relevantList;

        static void Main(string[] args)
        {
            //заполнение коллекции документов
            foreach (string s in DocLst)
            {
                getParseFile(s);
            }
            //Работа с Коллекцией AllFiles
            for(int i = 0; i < 3; i++)
            {
                getIdf(i);
                getRes(false, i);
                getRes(true, i);
            }
            Console.ReadLine();

        }

        static void getParseFile(string s)
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
                foreach (var i in sentance)
                {
                    Console.WriteLine("{0} - {1}", i.Name, i.Count);
                }
                Console.WriteLine("--------------------------------");
                DocCollection.Add(sentance);
            }
            AllFiles.Add(DocCollection);
        }

        static void getIdf(int i)
        {
            idf = new List<Word>();
            for (int j = 0; j < AllFiles[i].Count; j++)
            {
                foreach (Word w in AllFiles[i][j])
                {
                    int index = idf.FindIndex(x => x.Name == w.Name);
                    if (index == -1)
                        idf.Add(new Word(w.Name, 1));
                    else
                        idf[index].Count++;
                }
            }
        }

        static void getRes(bool b, int i)
        {
            relevantList = new List<double>();
            hdf = new List<double>();
            double qhdf = 0;
            wdq = new List<double>();
            hdq = new List<double>();
            int sum = 0;
            int df = 0;
            int qf = 0;

            Console.WriteLine("==================================");
            Console.WriteLine(b ? "With Idf" : "Without Idf");
            for (int j = 0; j < AllFiles[i].Count; j++)
            {
                int wdqSum = 0;
                foreach (Word w in AllFiles[i][j])
                {
                    df = b ? w.Count * idf.Find(x => x.Name == w.Name).Count : w.Count;
                    sum += df * df;
                }
                foreach (Word w in AllFiles[i + 3][0])
                {
                    Word tmp = AllFiles[i][j].Find(x => x.Name == w.Name);
                    if (tmp != null)
                    {
                        int CN = idf.Find(x => x.Name == w.Name).Count;
                        qf = b ? w.Count * CN : w.Count;
                        wdqSum += (b ? tmp.Count * CN : tmp.Count) * qf;
                    }
                }
                wdq.Add(wdqSum);
                hdf.Add(Math.Sqrt(sum));
            }
            foreach (Word w in AllFiles[i + 3][0])
            {
                Word tmp = idf.Find(x => x.Name == w.Name);
                if (tmp != null)
                {
                    qf = b ? w.Count * tmp.Count : w.Count;
                    qhdf += qf * qf;
                }
            }
            foreach (var d in hdf)
            {
                hdq.Add(d * qhdf);
            }
            for (int j = 0; j < wdq.Count; j++)
            {
                relevantList.Add(wdq[j] / hdq[j]);
                Console.WriteLine(relevantList[j]);
            }
            Console.WriteLine("++++++++++++++++++++++++++++++");
        }
    }
}