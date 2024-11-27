/*------------------------------------------------*
  | Autor: Noah Gerstlauer                          |
  | Department: THGM-TL1                            |
  | Email: Noah.Gerstlauer@airbus.com               |
  | Date: 2023-08-21                                |
 *------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 Simple Hilfsklasse um Servopositions-Vorlagen aus .csv Dateien einzulesen. Die Inhalte der Dateien muessen dem Format des
 Dictionary (siehe unten) uebereinstimmen um korrekt eingelesen zu werden.
*/


namespace EF_SerialCommunication_Tester
{
	using System;
	using System.IO;
	using System.Collections.Generic;

	public class PresetManager
	{
		private byte[] outBuffer = new byte[16] { 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

		
		// Dictionary als Formatierungsgrundlage fuer die .csv Dateien. Anhand diesem Format werden die Daten eingelesen	
		private Dictionary<string, int> indices = new Dictionary<string, int>
		{
				{ "MODE", 1 },
				{ "LC", 2 },
				{ "RC", 3 },
				{ "LS", 4 },
				{ "RS", 5 },
				{ "LO", 6 },
				{ "RO", 7 },
				{ "LI", 8 },
				{ "RI", 9 },
				{ "LE", 10 },
				{ "RE", 11 },
				{ "AB", 12 },
				{ "FI", 13 },
				{ "LED", 14 }
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
