using Newtonsoft.Json.Linq;
using System.Configuration;


namespace MyReqnrollProject.Utilities.GetEnvironementData
{
    /// <summary>
    /// Class to retrieve environment data form config files
    /// </summary>
    public class GetEnvironementData
    {
        /// <summary>
        /// static method to retrieve value of required parameter
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetEnvData(string data = "WebURL")
        {
            string key;
            try
            {
                String? env;
                string currentdirectory = Directory.GetParent(System.Environment.CurrentDirectory)?.Parent?.Parent?.FullName;
                string? jsonstring = File.ReadAllText(currentdirectory + "/Configurations/Environment.json");
                var json = JToken.Parse(jsonstring);
                Object? obj = json?.SelectToken("Default")?.Value<object>("TargetEnvironemnt");
                env = obj?.ToString();
                obj = json?.SelectToken(env)?.Value<object>(data);
                key = obj?.ToString();
            }
            catch (NullReferenceException e)
            {
                throw new Exception("Null Reference error occured in GetEnvData Method",e);
            }
            catch (Exception e)
            {
                throw new Exception("Error occured in the GetEnvironmentData-->GetEnvData() method, please check the parameters name",e);
            }

            return key;
        }
    }
}
