using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using XmlParser;

namespace XmlParserTests
{
    [TestClass]
    public class XmlDocumentWrapperUnitTest
    {
        [TestMethod]
        public void TestSaveXml()
        {
            var wrapper = new XmlDocumentWrapper();
            wrapper.LoadFrom(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("PARAMETERS", null),
                new KeyValuePair<string, string>("PARAMETERS_PARAMETER", null),
                new KeyValuePair<string, string>("PARAMETER:type", "6"),
                new KeyValuePair<string, string>("PARAMETER:prescision", "-1"),
                new KeyValuePair<string, string>("PARAMETER:readonly", "0"),
                new KeyValuePair<string, string>("PARAMETER:accuracy", "-1"),
                new KeyValuePair<string, string>("PARAMETER:valueType", "1"),
                new KeyValuePair<string, string>("PARAMETER:sysNameMeasureUnitBase", ""),
                new KeyValuePair<string, string>("PARAMETER:sysNameMeasureUnit", ""),
                new KeyValuePair<string, string>("PARAMETER_NAME", "ASSEMBLY_PARKING"),
                new KeyValuePair<string, string>("PARAMETER_COMMENT", "additional options"),
                new KeyValuePair<string, string>("PARAMETER_DEFAULT", "1"),
                new KeyValuePair<string, string>("PARAMETER_CATEGORIES", null),
                new KeyValuePair<string, string>("CATEGORIES_CATEGORY", "Ведомость работ"),
                new KeyValuePair<string, string>("CATEGORY:order", "2147483647"),
                new KeyValuePair<string, string>("CATEGORY:categoryOrder", "4"),
                new KeyValuePair<string, string>("PARAMETER_VALUES", null),
                new KeyValuePair<string, string>("VALUES_VALUE", null),
                new KeyValuePair<string, string>("VALUE_DATA", "1"),
                new KeyValuePair<string, string>("VALUE_COMMENT#1", "yes"),
                new KeyValuePair<string, string>("VALUES_VALUE", null),
                new KeyValuePair<string, string>("VALUE_DATA", "0"),
                new KeyValuePair<string, string>("VALUE_COMMENT#1", "no"),
                new KeyValuePair<string, string>("VALUES_VALUE", null),
                new KeyValuePair<string, string>("VALUE_DATA", "-1"),
                new KeyValuePair<string, string>("VALUE_COMMENT#1", "undefined")
            });
            var xml = wrapper.ToXml();
            Assert.AreEqual(xml, "﻿<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<PARAMETERS>\r\n  <PARAMETER type=\"6\" prescision=\"-1\" readonly=\"0\" accuracy=\"-1\" valueType=\"1\" sysNameMeasureUnitBase=\"\" sysNameMeasureUnit=\"\">\r\n    <NAME>ASSEMBLY_PARKING</NAME>\r\n    <COMMENT>additional options</COMMENT>\r\n    <DEFAULT>1</DEFAULT>\r\n    <CATEGORIES>\r\n      <CATEGORY order=\"2147483647\" categoryOrder=\"4\">Ведомость работ</CATEGORY>\r\n    </CATEGORIES>\r\n    <VALUES>\r\n      <VALUE>\r\n        <DATA>1</DATA>\r\n        <COMMENT>yes</COMMENT>\r\n      </VALUE>\r\n      <VALUE>\r\n        <DATA>0</DATA>\r\n        <COMMENT>no</COMMENT>\r\n      </VALUE>\r\n      <VALUE>\r\n        <DATA>-1</DATA>\r\n        <COMMENT>undefined</COMMENT>\r\n      </VALUE>\r\n    </VALUES>\r\n  </PARAMETER>\r\n</PARAMETERS>");
        }

        [TestMethod]
        public void TestLoadXml()
        {
            var fileName = "test.xml";
            var wrapper = new XmlDocumentWrapper();
            var parsed = wrapper.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
<PARAMETERS>
  <PARAMETER type=""6"" prescision=""-1"" readonly=""0"" accuracy=""-1"" valueType=""1"" sysNameMeasureUnitBase="""" sysNameMeasureUnit="""">
    <NAME>ASSEMBLY_PARKING</NAME>
    <COMMENT>additional options</COMMENT>
    <DEFAULT>1</DEFAULT>
    <CATEGORIES>
      <CATEGORY order=""2147483647"" categoryOrder=""4"">Ведомость работ</CATEGORY>
    </CATEGORIES>
    <VALUES>
      <VALUE>
        <DATA>1</DATA>
        <COMMENT>yes</COMMENT>
      </VALUE>
      <VALUE>
        <DATA>0</DATA>
        <COMMENT>no</COMMENT>
      </VALUE>
      <VALUE>
        <DATA>-1</DATA>
        <COMMENT>undefined</COMMENT>
      </VALUE>
    </VALUES>
  </PARAMETER>
</PARAMETERS>");
            var data = new object[parsed.Count, 2];
            for (var i = 0; i < parsed.Count; i++)
            {
                data[i, 0] = parsed[i].Key;
                data[i, 1] = parsed[i].Value;
            }
            Assert.IsTrue((string)data[0, 0] == "PARAMETERS" && data[0, 1] == null);
            Assert.IsTrue((string)data[1, 0] == "PARAMETERS_PARAMETER" && data[1, 1] == null);
            Assert.IsTrue((string)data[2, 0] == "PARAMETER:type" && (string)data[2, 1] == "6");
            Assert.IsTrue((string)data[3, 0] == "PARAMETER:prescision" && (string)data[3, 1] == "-1");
            Assert.IsTrue((string)data[4, 0] == "PARAMETER:readonly" && (string)data[4, 1] == "0");
            Assert.IsTrue((string)data[5, 0] == "PARAMETER:accuracy" && (string)data[5, 1] == "-1");
            Assert.IsTrue((string)data[6, 0] == "PARAMETER:valueType" && (string)data[6, 1] == "1");
            Assert.IsTrue((string)data[7, 0] == "PARAMETER:sysNameMeasureUnitBase" && (string)data[7, 1] == "");
            Assert.IsTrue((string)data[8, 0] == "PARAMETER:sysNameMeasureUnit" && (string)data[8, 1] == "");
            Assert.IsTrue((string)data[9, 0] == "PARAMETER_NAME" && (string)data[9, 1] == "ASSEMBLY_PARKING");
            Assert.IsTrue((string)data[10, 0] == "PARAMETER_COMMENT" && (string)data[10, 1] == "additional options");
            Assert.IsTrue((string)data[11, 0] == "PARAMETER_DEFAULT" && (string)data[11, 1] == "1");
            Assert.IsTrue((string)data[12, 0] == "PARAMETER_CATEGORIES" && (string)data[12, 1] == null);
            Assert.IsTrue((string)data[13, 0] == "CATEGORIES_CATEGORY" && (string)data[13, 1] == "Ведомость работ");
            Assert.IsTrue((string)data[14, 0] == "CATEGORY:order" && (string)data[14, 1] == "2147483647");
            Assert.IsTrue((string)data[15, 0] == "CATEGORY:categoryOrder" && (string)data[15, 1] == "4");
            Assert.IsTrue((string)data[16, 0] == "PARAMETER_VALUES" && (string)data[16, 1] == null);
            Assert.IsTrue((string)data[17, 0] == "VALUES_VALUE" && (string)data[17, 1] == null);
            Assert.IsTrue((string)data[18, 0] == "VALUE_DATA" && (string)data[18, 1] == "1");
            Assert.IsTrue((string)data[19, 0] == "VALUE_COMMENT#1" && (string)data[19, 1] == "yes");
            Assert.IsTrue((string)data[20, 0] == "VALUES_VALUE" && (string)data[20, 1] == null);
            Assert.IsTrue((string)data[21, 0] == "VALUE_DATA" && (string)data[21, 1] == "0");
            Assert.IsTrue((string)data[22, 0] == "VALUE_COMMENT#1" && (string)data[22, 1] == "no");
            Assert.IsTrue((string)data[23, 0] == "VALUES_VALUE" && (string)data[23, 1] == null);
            Assert.IsTrue((string)data[24, 0] == "VALUE_DATA" && (string)data[24, 1] == "-1");
            Assert.IsTrue((string)data[25, 0] == "VALUE_COMMENT#1" && (string)data[25, 1] == "undefined");
            Assert.IsTrue(File.Exists(fileName));
        }
    }
}
