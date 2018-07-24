using System;
using Xunit;
using Newtonsoft.Json.Linq;
using System.IO;
using AggregateGDPPopulation;


namespace AggregateGDPPopulation.Tests
{
    public class GDP_Population_Aggregate
    {
        [Fact]
        public async void Aggregate_Testcase()
        {
            AggregatePopulationGDP aggregate = new AggregatePopulationGDP();
            await aggregate.CalculateAggregate();
            var actual2 = await aggregate.ReadFileAsync("../../../../AggregateGDPPopulation/data/output.json");
            var expected2 = await aggregate.ReadFileAsync("../../../expected-output.json");
            //var actual1 = await actual2;
            //var expected1 = await expected2;
            JObject actual = JObject.Parse(actual2);
            JObject expected = JObject.Parse(expected2);
            Assert.Equal(expected, actual);
        }
    }
}
