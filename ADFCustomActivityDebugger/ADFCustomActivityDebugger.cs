using Microsoft.Azure.Management.DataFactories.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core = Microsoft.Azure.Management.DataFactories.Core;
using CoreModels = Microsoft.Azure.Management.DataFactories.Core.Models;
using System.Reflection;
using Microsoft.Azure.Management.DataFactories.Runtime;

namespace Azure.DataFactory
{
    public class ADFCustomActivityDebugger
    {
        string _vsProjectPath;
        string _configName;
        string _pipelineName;
        string _activityName;
        List<LinkedService> _adfLinkedServices;
        List<Dataset> _adfDataSets;
        List<Pipeline> _adfPipelines;
        JObject _configFileJson;
        public DateTime SliceStart { get; set; }
        public DateTime SliceEnd { get; set; }

        public ADFCustomActivityDebugger(string vsProjectPath, string pipelineName, string activityName, string configName)
        {
            _vsProjectPath = vsProjectPath;
            _pipelineName = pipelineName;
            _activityName = activityName;
            _configName = configName;
        }
        public ADFCustomActivityDebugger(string vsProjectPath, string pipelineName, string activityName) : this(vsProjectPath, pipelineName, activityName, null) { }


        public IDictionary<string, string> ExecuteActivity(IDotNetActivity activity)
        {
            Dictionary<string, string> ret = null;

            ret = (Dictionary<string, string>)activity.Execute(_adfLinkedServices, _adfDataSets, ActivityToDebug, new ADFCustomActivityConsoleLogger());

            return ret;
        }

        private Activity ActivityToDebug
        {
            get
            {
                return _adfPipelines.Single(p => p.Name == _pipelineName).Properties.Activities.Single(a => a.Name == _activityName);
            }
        }
        public void LoadADFEnvironment()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(_vsProjectPath);
            string schema;
            string adfType;

            _adfLinkedServices = new List<LinkedService>();
            _adfDataSets = new List<Dataset>();
            _adfPipelines = new List<Pipeline>();

            ReadConfig();

            foreach (FileInfo fileInfo in dirInfo.GetFiles("*.json", SearchOption.TopDirectoryOnly))
            {
                using (StreamReader file = File.OpenText(fileInfo.FullName))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        JObject jsonObj = (JObject)JToken.ReadFrom(reader);

                        if (jsonObj["$schema"] != null)
                        {
                            schema = jsonObj["$schema"].ToString();
                            adfType = schema.Substring(schema.LastIndexOf("/") + 1);

                            switch (adfType)
                            {
                                case "Microsoft.DataFactory.Pipeline.json": // ADF Pipeline
                                    _adfPipelines.Add(ADFPipelineFromJson(jsonObj));
                                    break;
                                case "Microsoft.DataFactory.Table.json": // ADF Table/Dataset
                                    _adfDataSets.Add(ADFDatasetFromJson(jsonObj));
                                    break;
                                case "Microsoft.DataFactory.LinkedService.json":
                                    _adfLinkedServices.Add(ADFLinkedServiceFromJson(jsonObj));
                                    break;
                                default:
                                    Console.WriteLine("{0} does not seem to belong to any know ADF Json-Schema and is ignored!", fileInfo.FullName);
                                    break;
                            }
                        }
                    }
                }
            }

            foreach (Pipeline pipeline in _adfPipelines)
            {
                pipeline.Properties.Datasets = _adfDataSets;

                foreach (Activity act in pipeline.Properties.Activities)
                {
                    // TODO 
                    // map Inputs
                    // map Outputs
                }
            }
        }

        private void ReadConfig()
        {
            if (ConfigFilePath != null)
            {
                using (StreamReader file = File.OpenText(ConfigFilePath.FullName))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        _configFileJson = (JObject)JToken.ReadFrom(reader);
                    }
                }
            }
        }

        private FileInfo ConfigFilePath
        {
            get
            {
                if (!string.IsNullOrEmpty(_configName))
                {
                    FileInfo ret = new FileInfo(_vsProjectPath.TrimEnd('\\') + "\\" + _configName);

                    if (ret.Exists)
                        return ret;

                    else
                        return null;
                }
                else
                    return null;
            }
        }

        public JObject ConfigFile
        {
            get
            {
                if (ConfigFilePath != null)
                {
                    if (_configFileJson == null)
                    {
                        ReadConfig();
                    }
                }
                else
                {
                    return new JObject();
                }

                return _configFileJson;
            }
        }


        public LinkedService ADFLinkedServiceFromJson(JObject jsonObject)
        {
            Type dynClass;
            MethodInfo dynMethod;

            string objectType = "LinkedService";

            MapConfigElements(ref jsonObject);
            MapSlices(ref jsonObject);

            dynClass = new Core.DataFactoryManagementClient().GetType();
            dynMethod = dynClass.GetMethod("DeserializeInternal" + objectType + "Json", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            CoreModels.LinkedService internalObject = (CoreModels.LinkedService)dynMethod.Invoke(this, new object[] { jsonObject.ToString() });

            dynClass = Type.GetType(dynClass.AssemblyQualifiedName.Replace("Core.DataFactoryManagementClient", "Conversion." + objectType + "Converter"));
            ConstructorInfo constructor = dynClass.GetConstructor(Type.EmptyTypes);
            object classObject = constructor.Invoke(new object[] { });
            dynMethod = dynClass.GetMethod("ToWrapperType", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            LinkedService ret = (LinkedService)dynMethod.Invoke(classObject, new object[] { internalObject });

            return ret;
        }
        public Dataset ADFDatasetFromJson(JObject jsonObject)
        {
            Type dynClass;
            MethodInfo dynMethod;

            string objectType = "Dataset";

            MapConfigElements(ref jsonObject);
            MapSlices(ref jsonObject);

            dynClass = new Core.DataFactoryManagementClient().GetType();
            dynMethod = dynClass.GetMethod("DeserializeInternal" + objectType + "Json", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            CoreModels.Dataset internalObject = (CoreModels.Dataset)dynMethod.Invoke(this, new object[] { jsonObject.ToString() });

            dynClass = Type.GetType(dynClass.AssemblyQualifiedName.Replace("Core.DataFactoryManagementClient", "Conversion." + objectType + "Converter"));
            ConstructorInfo constructor = dynClass.GetConstructor(Type.EmptyTypes);
            object classObject = constructor.Invoke(new object[] { });
            dynMethod = dynClass.GetMethod("ToWrapperType", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            Dataset ret = (Dataset)dynMethod.Invoke(classObject, new object[] { internalObject });

            return ret;
        }
        public Pipeline ADFPipelineFromJson(JObject jsonObject)
        {
            Type dynClass;
            MethodInfo dynMethod;

            string objectType = "Pipeline";

            MapConfigElements(ref jsonObject);
            MapSlices(ref jsonObject);

            dynClass = new Core.DataFactoryManagementClient().GetType();
            dynMethod = dynClass.GetMethod("DeserializeInternal" + objectType + "Json", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            CoreModels.Pipeline internalObject = (CoreModels.Pipeline)dynMethod.Invoke(this, new object[] { jsonObject.ToString() });

            dynClass = Type.GetType(dynClass.AssemblyQualifiedName.Replace("Core.DataFactoryManagementClient", "Conversion." + objectType + "Converter"));
            ConstructorInfo constructor = dynClass.GetConstructor(Type.EmptyTypes);
            object classObject = constructor.Invoke(new object[] { });
            dynMethod = dynClass.GetMethod("ToWrapperType", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            Pipeline ret = (Pipeline)dynMethod.Invoke(classObject, new object[] { internalObject });

            return ret;
        }

        public void MapConfigElements(ref JObject jsonObject)
        {
            JProperty jProp;
            JToken find;
            string objectName = jsonObject["name"].ToString();

            foreach (JToken jToken in jsonObject.Descendants())
            {
                if (jToken is JProperty)
                {
                    jProp = (JProperty)jToken;

                    if (jProp.Value is JValue)
                    {
                        if (jProp.Value.ToString() == "<config>")
                        {
                            // get all Config-settings for the current file
                            foreach (JToken result in ConfigFile.SelectTokens(string.Format("$.{0}.[*]", objectName)))
                            {
                                // try to select the token specified in the config in the file
                                // this logic is necessary as the config might contain JSONPath wildcards
                                find = jProp.Root.SelectToken(result["name"].ToString());

                                if (find != null) // token was found
                                    if (find.Path == jProp.Path) // found token has the same path as the original token
                                    {
                                        jProp.Value = result["value"];
                                        break;
                                    }
                            }

                            // the jProp.Value must be updated with the value from the config file - otherwise we throw an error
                            if (jProp.Value.ToString() == "<config>")
                            {
                                throw new KeyNotFoundException("No Config-Setting could be found for \"" + objectName + "\" and \"name\": \"" + jProp.Path + "\" (or any matching wildcard)");
                            }
                        }
                    }
                }
            }
        }

        public void MapSlices(ref JObject jsonObject)
        {
            JProperty jProp;
            string objectName = jsonObject["name"].ToString();

            Regex regex = new Regex(@"\$\$Text.Format\('(.*)',(.*)\)");

            string textTemplate;
            string textParameters;

            List<string> parameters;
            List<object> arguments;

            string oldText;
            string newText;
            Dictionary<string, string> partitionBy = new Dictionary<string, string>(); ;


            foreach (JToken jToken in jsonObject.Descendants())
            {
                if (jToken is JProperty)
                {
                    jProp = (JProperty)jToken;

                    // map all Values that are like "$$Text.Format(..., SliceStart)"
                    if (jProp.Value is JValue)
                    {
                        Match match = regex.Match(jProp.Value.ToString());
                        if (match.Groups.Count == 3)
                        {
                            textTemplate = match.Groups[1].Value;
                            textParameters = match.Groups[2].Value;

                            parameters = textParameters.Split(',').Select(p => p.Trim()).ToList();
                            arguments = new List<object>(parameters.Count);

                            for (int i = 0; i < parameters.Count; i++)
                            {
                                arguments.Add(new object());
                            }

                            if (parameters.Contains("SliceEnd"))
                            {
                                arguments[parameters.IndexOf("SliceEnd")] = SliceEnd;
                            }

                            if (parameters.Contains("SliceStart"))
                            {
                                arguments[parameters.IndexOf("SliceStart")] = SliceStart;
                            }

                            jProp.Value = new JValue(string.Format(textTemplate, arguments.ToArray()));
                        }
                    }

                    // map all Values that have a partitionedBy clause
                    if (jProp.Name == "partitionedBy")
                    {
                        partitionBy = new Dictionary<string, string>();
                        foreach (JToken part in jProp.Value)
                        {
                            oldText = "{" + part["name"] + "}";

                            switch (part["value"]["date"].ToString())
                            {
                                case "SliceStart":
                                    newText = string.Format("{0:" + part["value"]["format"] + "}", SliceStart);
                                    break;
                                case "SliceEnd":
                                    newText = string.Format("{0:" + part["value"]["format"] + "}", SliceEnd);
                                    break;
                                default:
                                    throw new Exception("PartitionedBy currently only works with 'SliceStart' and 'SliceEnd'");
                            }

                            partitionBy.Add(oldText, newText);
                        }
                    }
                }
            }

            string newObjectJson = jsonObject.ToString();

            foreach (KeyValuePair<string, string> kvp in partitionBy)
            {
                newObjectJson = newObjectJson.Replace(kvp.Key, kvp.Value);
            }
            jsonObject = JObject.Parse(newObjectJson);
        }
    }
}
