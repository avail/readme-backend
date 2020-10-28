using System;
using System.IO;

namespace backend.Services
{
    public class VisitorCountService
    {
        static string ms_visitorsFile = "visitors.txt";
        int m_visitorCount;
        object m_lock = new object();

        public string Visitors => $"{m_visitorCount}";

        public VisitorCountService()
        {
            if (File.Exists(ms_visitorsFile))
            {
                m_visitorCount = Convert.ToInt32(File.ReadAllText(ms_visitorsFile));
            }
            else
            {
                m_visitorCount = 0;
            }
        }

        public void Increase()
        {
            m_visitorCount++;

            lock (m_lock)
            {
                File.WriteAllText(ms_visitorsFile, $"{m_visitorCount}");
            }
        }
    }
}
