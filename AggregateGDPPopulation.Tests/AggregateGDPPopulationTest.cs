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
        public void Aggregate_Testcase()
        {
            AggregatePopulationGDP aggregate = new AggregatePopulationGDP();
            aggregate.CalculateAggregate();
            var actual1 = File.ReadAllText("../../../../AggregateGDPPopulation/data/output.json");
            var expected1 = File.ReadAllText("../../../expected-output.json");
            JObject actual = JObject.Parse(actual1);
            JObject expected = JObject.Parse(expected1);
            Assert.Equal(expected, actual);
        }
    }
}
