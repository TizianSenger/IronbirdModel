using System;
using System.IO;
using System.Text;

namespace EurofighterInformationCenter
{
    internal class logger
    {
        public string userName = Environment.UserName;
        public string filePath = "";

        public void createLogFile()
        {
            try
            {
                string folderPath = $@"C:\Users\{userName}\Desktop\ApplicationLog\";
                string fileName = $"log_{DateTime.Now:yyyy_MM_dd}.txt";
                string combinedPath = Path.Combine(folderPath, fileName);

                filePath = combinedPath;

                if (File.Exists(combinedPath) == false)
                {
                    File.Create(combinedPath).Close();
                }
            }
            catch 
            { 
                //nothing here
            }
        }

        public void writeToLog(string message)
        {
            string logMessage = $"{DateTime.Now:T}: {message}{Environment.NewLine}";
            createLogFile();
            try
            {
                File.AppendAllText(filePath, logMessage + "\n  \n", Encoding.UTF8);
            }
            catch
            {
                //nothing here 
            }
        }
    }
}
