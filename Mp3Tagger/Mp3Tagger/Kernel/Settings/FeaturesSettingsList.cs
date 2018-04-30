using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Mp3Tagger.Kernel.Interfaces;

namespace Mp3Tagger.Kernel.Settings
{
    public class FeaturesSettingsList : List<ISettings>, IXmlSerializable
    {
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement("FeaturesSettings");
            while (reader.IsStartElement("ISettings"))
            {
                Type type = Type.GetType(reader.GetAttribute("AssemblyQualifiedName"));
                XmlSerializer serial = new XmlSerializer(type);

                reader.ReadStartElement("ISettings");
                this.Add((ISettings)serial.Deserialize(reader));
                reader.ReadEndElement();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (ISettings settings in this)
            {
                writer.WriteStartElement("ISettings");
                writer.WriteAttributeString("AssemblyQualifiedName", settings.GetType().AssemblyQualifiedName);
                XmlSerializer xmlSerializer = new XmlSerializer(settings.GetType());
                xmlSerializer.Serialize(writer, settings);
                writer.WriteEndElement();
            }
        }
    }
}
