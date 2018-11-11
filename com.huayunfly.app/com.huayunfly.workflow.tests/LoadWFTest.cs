using System;
using System.Activities;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Workflow.Activities;

namespace com.huayunfly.workflow.tests
{
    [TestClass]
    public class LoadWFTest
    {
        [TestMethod]
        public void TestLoadXaml()
        {
            string xamlName = "ActivityTemplate.xaml";
            string xamlFile = Path.Combine(Directory.GetCurrentDirectory(), xamlName);
            var inputs = new Dictionary<string, object>
            {
                {"token", "123456" },
                { "data", "你好"}
            };

            var output = ActivityWorker.InvokeWorkflowFile(xamlFile, inputs);
            Assert.AreEqual<string>(string.Empty, output);
        }

        [TestMethod]
        public void TestDynamicCreate()
        {
            var activity = ActivityWorker.CreateDynamicActivity("TestFlow");
            Assert.IsNotNull(activity);
            PlaceholderActivity placeHolder = new PlaceholderActivity(activity);
            string xaml = placeHolder.XAML;
            Assert.IsNotNull(xaml);
            var inputs = new Dictionary<string, object>
            {
                {"token", "123456" },
                { "data", "你好"}
            };
            var output = ActivityWorker.InvokeWorkflowXaml(xaml, inputs);
            Assert.AreEqual<string>("123456你好", output);

        }

        [TestMethod]
        public void TestActivityBuilderCreate()
        {
            var activity = ActivityWorker.CreateActivityFromBuilder("TestFlow");
            var inputs = new Dictionary<string, object>
            {
                {"token", "123456" },
                { "data", "你好"}
            };
            var output = ActivityWorker.InvokeWorkflow(activity, inputs);
            Assert.AreEqual<string>("123456你好", output);
        }
    }
}
