using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace AggregateGDPPopulation
{
    public class AggregatePopulationGDP
    {
        public async Task CalculateAggregate()
        {
            Task<string> linetask = ReadFileAsync("../../../../AggregateGDPPopulation/data/datafile.csv");
            Task<string> maptask = ReadFileAsync("../../../../AggregateGDPPopulation/data/country-continent-map.json");
            string line1 = await linetask;
            string readCCMap = await maptask;
            string[] line = line1.Split('\n');
            Dictionary<string, string> CCDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(readCCMap);
            Dictionary<string, GDPPopClass> ContGDPPop = new Dictionary<string, GDPPopClass>();
            string[] header = line[0].Replace("\"", "").Split(',');
            int CountryIndex = Array.IndexOf(header, "Country Name");
            int PopulationIndex = Array.IndexOf(header, "Population (Millions) 2012");
            int GDPIndex = Array.IndexOf(header, "GDP Billions (USD) 2012");
            string[] lines = line.Skip(1).ToArray();
            foreach (string item in lines)
            {
                string[] SplitByComma = item.Replace("\"", "").Split(',');
                GDPPopClass temp = new GDPPopClass();

                if (SplitByComma[CountryIndex] == "European Union")
                {
                    break;
                }
                else
                {
                    if (ContGDPPop.ContainsKey(CCDict[SplitByComma[CountryIndex]]))
                    {
                        ContGDPPop[CCDict[SplitByComma[CountryIndex]]].GDP_2012 += float.Parse(SplitByComma[GDPIndex]);
                        ContGDPPop[CCDict[SplitByComma[CountryIndex]]].POPULATION_2012 += float.Parse(SplitByComma[PopulationIndex]);
                    }
                    else
                    {
                        temp.GDP_2012 = float.Parse(SplitByComma[GDPIndex]);
                        temp.POPULATION_2012 = float.Parse(SplitByComma[PopulationIndex]);
                        ContGDPPop.Add(CCDict[SplitByComma[CountryIndex]], temp);
                    }
                }
            }
            var JSONOutput = Newtonsoft.Json.JsonConvert.SerializeObject(ContGDPPop, Formatting.Indented);
            WriteFileAsync("../../../../AggregateGDPPopulation/output/output.json", JSONOutput);
        }


        public async Task<string> ReadFileAsync(string filepath)
        {
            string data;
            using (StreamReader sr = new StreamReader(filepath))
            {
                data = await sr.ReadToEndAsync();
            }
            return data;
        }

        public async void WriteFileAsync(string filepath, string JSONOutput)
        {
            using (StreamWriter sw = new StreamWriter(filepath))
            {
                await sw.WriteAsync(JSONOutput);
            }
        }
    }

    public class GDPPopClass
    {
        public float GDP_2012 { get; set; }
        public float POPULATION_2012 { get; set; }
    }
}
