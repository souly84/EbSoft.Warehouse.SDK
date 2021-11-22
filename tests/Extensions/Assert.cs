using Newtonsoft.Json.Linq;
using WebRequest.Elegant.Extensions;
using Xunit.Abstractions;

namespace EbSoft.Warehouse.SDK.UnitTests.Extensions
{
    public class Assert : Xunit.Assert
    {
        public static void EqualJson(
            string expectedJson,
            string actualJson,
            ITestOutputHelper output = null)
        {
            expectedJson = expectedJson.NoNewLines();
            actualJson = actualJson.NoNewLines();
            JObject expected = JObject.Parse(expectedJson);
            JObject actual = JObject.Parse(actualJson);
            if (output != null)
            {
                output.WriteLine("Expected:" + expectedJson);
                output.WriteLine("Actual:" + actualJson);
            }

            Equal(expected, actual, JToken.EqualityComparer);
        }

        public static void EqualJsonArrays(
            string expectedJson,
            string actualJson,
            ITestOutputHelper output = null)
        {
            expectedJson = expectedJson.NoNewLines();
            actualJson = actualJson.NoNewLines();
            JArray expected = JArray.Parse(expectedJson);
            JArray actual = JArray.Parse(actualJson);
            if (output != null)
            {
                output.WriteLine("Expected:" + expectedJson);
                output.WriteLine("Actual:" + actualJson);
            }

            Equal(expected, actual, JToken.EqualityComparer);
        }
    }
}
