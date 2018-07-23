using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;

namespace AggregateGDPPopulation
{
    public class AggregatePopulationGDP
    {
        public void CalculateAggregate()
        {
            string[] line = File.ReadAllLines("../../../../AggregateGDPPopulation/data/datafile.csv");
            JObject readCCMap = JObject.Parse(File.ReadAllText("../../../../AggregateGDPPopulation/data/country-continent-map.json", Encoding.UTF8));
            Dictionary<string, string> CCDict = readCCMap.ToObject<Dictionary<string, string>>();
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

                if (SplitByComma[CountryIndex] != "European Union")
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
            var JSONOutput = Newtonsoft.Json.JsonConvert.SerializeObject(ContGDPPop);
            File.WriteAllText("../../../../AggregateGDPPopulation/data/output.json", JSONOutput);
        }
    }

    public class GDPPopClass
    {
        public float GDP_2012 { get; set; }
        public float POPULATION_2012 { get; set; }
    }
}
