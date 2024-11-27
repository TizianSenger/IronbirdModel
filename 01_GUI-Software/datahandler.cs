using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX;
using System.Drawing;

namespace EurofighterInformationCenter
{
    class datahandler
    {
        logger loggerInstance;
        private string[] data;
        public string userName = Environment.UserName;

        public string csvFilePath = "";

        public datahandler()
        {
            csvFilePath = $@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataForMainPage\ButtonsTextInformationCenter.csv";
            loggerInstance = new logger();
        }

        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THEM-TL1
        //  Date:       01.09.2023
        //
        //  Es wird eine Datei ausgelesen, und anschließend wird der String zurückgegeben
        //=======================================================================================
        public string ReadDataFromFile(string pathOfFile)
        {
            try
            {
                if (File.Exists(pathOfFile))
                {
                    using (StreamReader sr = new StreamReader(pathOfFile, Encoding.UTF8))
                    {
                        string content = sr.ReadToEnd();
                        return content;
                    }
                }
                else
                {
                    MessageBox.Show("File not found");
                    return null;
                }
            }

            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                return null;
            }
        }



        public Image[] loadPictures(string path)
        {
            try
            {
                List<Image> bilder = new List<Image>();

            for (int i = 1; i <= 10; i++)
            {
                string bildDateiPfad = Path.Combine(path, $"Folie{i}.jpg");

                if (File.Exists(bildDateiPfad))
                {
                    bilder.Add(Image.FromFile(bildDateiPfad));
                }
            }

            return bilder.ToArray();
            }

            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                return null;
            }
        }








        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       07.09.2023
        //
        //  In dieser Methode wird der Text für die labels im Ui ausgelesen, ebenso wie die
        //  Positon der label dies ermoglicht eine volle anpassung des UIs ohne die anwendung neu
        //  entwickeln zu müssen
        //=======================================================================================
        public string[] ReadLabelSettings(string filePath)
        {
            try
            {
                // Lese alle Zeilen aus der Textdatei
                string[] lines = File.ReadAllLines(filePath);
                return lines;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                return null;
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THEM-TL1
        //  Date:       07.09.2023
        //
        //  In dieser Methode wird der Text für die Buttons im UI ausgelesen, es wird Überprüft, 
        //  ob die Datei bereits eingelesen wurde, wenn das der fall ist, wird der aus dem Array 
        //  benötigte Eintrag zurückgegeben, wurde die datei noch nicht eingelesen, dann wird 
        //  diese eingelesen, und in einem Array abgespeichert
        //=======================================================================================
        public string ButtonTextRead(int numberOfButton)
        {
            try
            {
                if (data != null)
                {
                    if (numberOfButton <= 0 || numberOfButton > data.Length)
                    {
                        MessageBox.Show("Der angegebene Index liegt außerhalb des gültigen Bereichs");
                        return null;
                    }
                    else
                    {
                        return data[numberOfButton - 1];
                    }

                }
                else
                {
                    if (!File.Exists(csvFilePath))
                    {
                        MessageBox.Show($"Die Datei unter dem Pfad /n'{csvFilePath}' /n wurde nicht gefuden");
                    }
                    try
                    {

                        data = File.ReadAllLines(csvFilePath);

                        if (numberOfButton <= 0 || numberOfButton > data.Length)
                        {
                            MessageBox.Show("Der angegebene Index liegt außerhalb des gültigen Bereichs");
                            return null;
                        }
                        else
                        {
                            return data[numberOfButton - 1];
                        }
                    }
                    catch (Exception ex)
                    {
                        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        string severity = ex.GetType().Name;
                        loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                return null;
            }
        }
    }





    ////=======================================================================================
    ////  Author:     Tizian Senger           Department:     THEM-TL1
    ////  Date:       07.09.2023
    ////
    ////  In dieser Methode wird die DemoData.txt eingelesen, in dieser datei stehen für alle
    ////  100 Millisekunden die Servostellungen für alle elemente, skaliert auf 0 - 180
    ////  Methode derzeit nicht verwendet, da Demo funktion zum auslieferungszustand nicht 
    ////  gewünscht war 
    ////  Konsultieren Sie die Dokumentation für das hinzufügen der Demo Funktion
    ////=======================================================================================

    //public Dictionary<int, int[]> ReadDemoMovement(string pathOfFile)
    //{
    //    Dictionary<int, int[]> result = new Dictionary<int, int[]>();
    //    try
    //    {
    //        if (File.Exists(pathOfFile))
    //        {
    //            using (StreamReader sr = new StreamReader(pathOfFile, Encoding.UTF8))
    //            {
    //                // Überspringen der ersten Zeile (Überschriften)
    //                sr.ReadLine();

    //                while (!sr.EndOfStream)
    //                {
    //                    string line = sr.ReadLine();
    //                    string[] parts = line.Split('\t'); // Annahme, dass die Werte durch Tabs getrennt sind

    //                    if (parts.Length > 1) // Mindestens ein Schlüssel und ein Wert sollten vorhanden sein
    //                    {
    //                        int key = int.Parse(parts[0]);
    //                        int[] values = new int[parts.Length - 1];

    //                        for (int i = 1; i < parts.Length; i++)
    //                        {
    //                            values[i - 1] = int.Parse(parts[i]);
    //                        }

    //                        result[key] = values;
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            MessageBox.Show("File not found");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        MessageBox.Show("Bei dem Auslesen der Demo Datei ist ein Fehler aufgetreten: \n" + ex.Message);
    //    }
    //    //MessageBox.Show(result.ToString());
    //    return result;
    //}
}
