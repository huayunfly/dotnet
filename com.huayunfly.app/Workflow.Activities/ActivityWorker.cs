using Microsoft.VisualBasic.Activities;
using System;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;

namespace Workflow.Activities
{
    public static class ActivityWorker
    {
        public static string InvokeWorkflowFile(string xamlFile, Dictionary<string, object> inputs)
        {
            Activity wf;
            using (Stream strXaml = File.OpenRead(xamlFile))
            {
                var settings = new ActivityXamlServicesSettings() { CompileExpressions = true};
                wf = ActivityXamlServices.Load(strXaml, settings);
            }

            var outputs = WorkflowInvoker.Invoke(wf, inputs);
            return outputs["retData"]?.ToString()??string.Empty;
        }

        public static string InvokeWorkflowXaml(string xaml, Dictionary<string, object> inputs)
        {
            Activity wf;
            using (TextReader strReader = new StringReader(xaml))
            {
                wf = ActivityXamlServices.Load(strReader);
            }

            var outputs = WorkflowInvoker.Invoke(wf, inputs);
            return outputs["retValue"]?.ToString() ?? string.Empty;
        }

        public static string InvokeWorkflow(Activity activity, Dictionary<string, object> inputs)
        {
            var outputs = WorkflowInvoker.Invoke(activity, inputs);
            return outputs["retValue"]?.ToString() ?? string.Empty;
        }

        public static DynamicActivity CreateDynamicActivity(string name)
        {
            //Define the input argument for the activity
            var token = new InArgument<string>();
            var data = new InArgument<string>();
            var retData = new OutArgument<string>();

            //Create the activity, property, and implementation  
            DynamicActivity dynamicWorkflow = new DynamicActivity()
            {
                Name = name,
                Properties =
                {
                    new DynamicActivityProperty
                    {
                        Name = "token",
                        Type = typeof(InArgument<string>),
                        Value = token
                    },
                    new DynamicActivityProperty
                    {
                        Name = "data",
                        Type = typeof(InArgument<string>),
                        Value = data
                    },
                    new DynamicActivityProperty
                    {
                        Name = "retValue",
                        Type = typeof(OutArgument<string>),
                        Value = retData
                    }
                },
                Implementation = () =>
                {
                    Sequence seq = new Sequence();

                    Type type = Type.GetType("Workflow.Activities.ScheduleActivity", true, true);
                    object instance = Activator.CreateInstance(type);
                    // Get a property on the type that is stored in the property string
                    PropertyInfo prop = type.GetProperty("Token");
                    prop.SetValue(instance, new InArgument<string>(new VisualBasicValue<String>("token")), null);
                    prop = type.GetProperty("Data");
                    prop.SetValue(instance, new InArgument<string>("Mike"), null);
                    seq.Activities.Add((Activity)instance);
                    seq.Activities.Add(new Assign<string>
                    {
                        To = new ArgumentReference<string> { ArgumentName = "retValue" },
                        Value = new VisualBasicValue<string>("(token + data)")
                    });
                    return seq;
                }
                //new Assign<int>
                //{
                //    To = new ArgumentReference<int> { ArgumentName = "outValue" },
                //    Value = new VisualBasicValue<int>("(inValueA + inValueB) * 100")
                //}
            };
            return dynamicWorkflow;
        }

        public static ActivityBuilder CreateActivityBuilder(string name)
        {
            Sequence seq = new Sequence();

            Type type = Type.GetType("Workflow.Activities.ScheduleActivity", true, true);
            object instance = Activator.CreateInstance(type);
            // Get a property on the type that is stored in the property string
            PropertyInfo prop = type.GetProperty("Token");
            prop.SetValue(instance, new InArgument<string>(new VisualBasicValue<String>("token")), null);
            prop = type.GetProperty("Data");
            prop.SetValue(instance, new InArgument<string>("Mike"), null);
            seq.Activities.Add((Activity)instance);
            seq.Activities.Add(new Assign<string>
            {
                To = new ArgumentReference<string> { ArgumentName = "retValue" },
                Value = new VisualBasicValue<string>("(token + data)")
            });

            ActivityBuilder activityBuilder = new ActivityBuilder();
            activityBuilder.Name = name;
            activityBuilder.Properties.Add(new DynamicActivityProperty { Name = "token", Type = typeof(InArgument<string>) });
            activityBuilder.Properties.Add(new DynamicActivityProperty { Name = "data", Type = typeof(InArgument<string>) });
            activityBuilder.Properties.Add(new DynamicActivityProperty { Name = "retValue", Type = typeof(OutArgument<string>) });

            activityBuilder.Implementation = seq;
            return activityBuilder;
        }

        public static string GetXamlStringFromActivityBuilder(ActivityBuilder activityBuilder)
        {
            string xamlString = "";
            StringBuilder stringBuilder = new StringBuilder();
            StringWriter stringWriter = new StringWriter(stringBuilder);
            XamlSchemaContext xamlSchemaContext = new XamlSchemaContext();
            XamlXmlWriter xamlXmlWriter = new XamlXmlWriter(stringWriter, xamlSchemaContext);
            XamlWriter xamlWriter = ActivityXamlServices.CreateBuilderWriter(xamlXmlWriter);
            XamlServices.Save(xamlWriter, activityBuilder);
            xamlString = stringBuilder.ToString();
            return xamlString;
        }

        public static Activity CreateActivityFromBuilder(string name)
        {
            string xamlString = GetXamlStringFromActivityBuilder(CreateActivityBuilder(name));
            StringReader stringReader = new StringReader(xamlString);
            Activity activity = ActivityXamlServices.Load(stringReader);
            return activity;
        }
    }
}
