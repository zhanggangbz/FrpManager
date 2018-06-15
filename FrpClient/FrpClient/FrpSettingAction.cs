using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FrpClient
{
    class FrpSettingAction
    {
        public static void AddFrpSettingToConfig(FrpSettingModel _frp)
        {
            XmlDocument xmldoc = GetXMLRootElement();

            XmlElement elementroot = (XmlElement)xmldoc.SelectSingleNode("Frps");

            if(elementroot != null)
            {

                XmlElement newfrpNode = xmldoc.CreateElement("frp");

                AddXMLChildNode(xmldoc, newfrpNode, "id", _frp.ID);

                AddXMLChildNode(xmldoc, newfrpNode, "name", _frp.Name);

                AddXMLChildNode(xmldoc, newfrpNode, "cffile", _frp.ConfigFile);

                AddXMLChildNode(xmldoc, newfrpNode, "logfile", _frp.LogFile);

                AddXMLChildNode(xmldoc, newfrpNode, "autorun", _frp.AutoRun.ToString());

                elementroot.AppendChild(newfrpNode);

                xmldoc.Save("config.xml");
            }
        }

        public static void DelFrpSettingFromConfig(FrpSettingModel _frp)
        {
            XmlDocument xmldoc = GetXMLRootElement();

            XmlElement elementroot = (XmlElement)xmldoc.SelectSingleNode("Frps");

            if (elementroot != null)
            {
                string xpathstr = "frp[id='{0}']";

                XmlNode tempNode = elementroot.SelectSingleNode(string.Format(xpathstr, _frp.ID));

                if (tempNode != null)
                {
                    elementroot.RemoveChild(tempNode);
                }

                xmldoc.Save("config.xml");
            }
        }

        public static void EditFrpSettingToConfig(FrpSettingModel _frp)
        {
            XmlDocument xmldoc = GetXMLRootElement();

            XmlElement elementroot = (XmlElement)xmldoc.SelectSingleNode("Frps");

            if (elementroot != null)
            {
                string xpathstr = "frp[id='{0}']";

                XmlElement itemNode = (XmlElement)elementroot.SelectSingleNode(string.Format(xpathstr, _frp.ID));

                if (itemNode != null)
                {
                    XmlNode tempNode = itemNode.SelectSingleNode("id/text()");
                    if (tempNode != null)
                    {
                        tempNode.Value = _frp.ID;
                    }
                    else
                    {
                        AddXMLChildNode(xmldoc, itemNode, "id", _frp.ID);
                    }

                    tempNode = itemNode.SelectSingleNode("name/text()");
                    if (tempNode != null)
                    {
                        tempNode.Value = _frp.Name;
                    }
                    else
                    {
                        AddXMLChildNode(xmldoc, itemNode, "name", _frp.Name);
                    }

                    tempNode = itemNode.SelectSingleNode("cffile/text()");
                    if (tempNode != null)
                    {
                        tempNode.Value = _frp.ConfigFile;
                    }
                    else
                    {
                        AddXMLChildNode(xmldoc, itemNode, "cffile", _frp.ConfigFile);
                    }

                    tempNode = itemNode.SelectSingleNode("logfile/text()");
                    if (tempNode != null)
                    {
                        tempNode.Value = _frp.LogFile;
                    }
                    else
                    {
                        AddXMLChildNode(xmldoc, itemNode, "logfile", _frp.LogFile);
                    }

                    tempNode = itemNode.SelectSingleNode("autorun/text()");
                    if (tempNode != null)
                    {
                        tempNode.Value = _frp.AutoRun.ToString();
                    }
                    else
                    {
                        AddXMLChildNode(xmldoc, itemNode, "autorun", _frp.AutoRun.ToString());
                    }
                }

                xmldoc.Save("config.xml");
            }
        }

        public static List<FrpSettingModel> GetAllFrpSetting()
        {
            List<FrpSettingModel> list = new List<FrpSettingModel>();

            XmlDocument xmldoc = GetXMLRootElement();

            XmlElement elementroot = (XmlElement)xmldoc.SelectSingleNode("Frps");

            if (elementroot != null)
            {

                XmlNodeList allFrp = xmldoc.SelectNodes("Frps/frp");

                foreach (XmlNode item in allFrp)
                {
                    try
                    {
                        FrpSettingModel ftpitem = new FrpSettingModel();

                        XmlNode tempNode = item.SelectSingleNode("id/text()");
                        if (tempNode != null)
                        {
                            ftpitem.ID = tempNode.Value;
                        }

                        tempNode = item.SelectSingleNode("name/text()");
                        if (tempNode != null)
                        {
                            ftpitem.Name = tempNode.Value;
                        }

                        tempNode = item.SelectSingleNode("cffile/text()");
                        if (tempNode != null)
                        {
                            ftpitem.ConfigFile = tempNode.Value;
                        }

                        tempNode = item.SelectSingleNode("logfile/text()");
                        if (tempNode != null)
                        {
                            ftpitem.LogFile = tempNode.Value;
                        }

                        tempNode = item.SelectSingleNode("autorun/text()");
                        if (tempNode != null)
                        {
                            try
                            {
                                ftpitem.AutoRun = bool.Parse(tempNode.Value);
                            }
                            catch
                            {
                                ftpitem.AutoRun = false;
                            }
                        }

                        list.Add(ftpitem);
                    }
                    catch { }

                }
            }

            return list;
        }

        private static void AddXMLChildNode(XmlDocument _doc, XmlElement _node, string cnodename, string cnodevalue)
        {
            XmlElement newnode = _doc.CreateElement(cnodename);
            XmlText newnodevalue = _doc.CreateTextNode(cnodevalue);
            newnode.AppendChild(newnodevalue);
            _node.AppendChild(newnode);
        }

        private static XmlDocument GetXMLRootElement()
        {
            if (!File.Exists("config.xml"))
            {
                XElement xElement = new XElement(new XElement("Frps"));

                //需要指定编码格式，否则在读取时会抛：根级别上的数据无效。 第 1 行 位置 1异常
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UTF8Encoding(false);
                settings.Indent = true;
                XmlWriter xw = XmlWriter.Create("config.xml", settings);
                xElement.Save(xw);
                //写入文件
                xw.Flush();
                xw.Close();
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("config.xml");
            return xmlDoc;
        }
    }
}
