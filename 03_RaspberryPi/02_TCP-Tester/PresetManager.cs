using System;
using System.Collections.Generic;
using System.IO;

namespace TCP_Tester
{
    public class PresetManager
    {
        private byte[] outBuffer = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


        // Dictionary als Formatierungsgrundlage fuer die .csv Dateien. Anhand diesem Format werden die Daten eingelesen	
        private Dictionary<string, int> indices = new Dictionary<string, int>
        {
                { "MODE", 0 },
                { "LC", 1 },
                { "RC", 2 },
                { "LS", 3 },
                { "RS", 4 },
                { "LO", 5 },
                { "RO", 6 },
                { "LI", 7 },
                { "RI", 8 },
                { "LE", 9 },
                { "RE", 10 },
                { "AB", 11 },
                { "FI", 12 },
                { "LED", 13 }
        };

        // Nach .csv Dateien im uebergebenen Ordnerpfad suchen und die Namen zurueckgeben
        public string[] GetCsvFileNames(string folderPath)
        {
            return Directory.GetFiles(folderPath, "*.csv");
        }


        // Daten aus der uebergebenen .csv Datei auslesen	
        public byte[] LoadValuesFromCsv(string csvFilePath)
        {

            string[] lines;
            try
            {
                lines = File.ReadAllLines(csvFilePath);
            }
            catch (Exception e)
            {
                throw new Exception($"Fehler beim Lesen der CSV-Datei: {e.Message}");
            }

            // Jede Zeile separat bearbeiten
            foreach (var line in lines)
            {
                var parts = line.Split(',');

                if (parts.Length < 2)
                    throw new Exception("Falsches CSV-Format: Weniger als 2 Teile pro Zeile");

                var key = parts[0];
                if (!indices.ContainsKey(key))
                    throw new Exception($"Unbekannter Schlüssel: {key}");

                if (!byte.TryParse(parts[1], out var value))
                    throw new Exception($"Ungültiger Wert: {parts[1]}");

                outBuffer[indices[key]] = value;
                //Console.WriteLine(value);
            }
            return outBuffer;
        }
    }

}
