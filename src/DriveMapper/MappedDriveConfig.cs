using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DriveMapper
{
    public static class MappedDriveConfig
    {
        public static IEnumerable<MappedDrivesConfigElement> GetAllConfigItems()
        {
            var mappedDrivesConfigSection = (MappedDrivesConfigSection)ConfigurationManager.GetSection("MappedDrivesConfig");

            return mappedDrivesConfigSection.MappedDrivesConfigCollection
                .Cast<MappedDrivesConfigElement>()
                .ToList();
        }
    }

    public class MappedDrivesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public MappedDrivesConfigCollection MappedDrivesConfigCollection
        {
            get { return (MappedDrivesConfigCollection)this[""]; }
            set { this[""] = value; }
        }
    }

    public class MappedDrivesConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MappedDrivesConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MappedDrivesConfigElement)element).DriveLetter;
        }
    }

    public class MappedDrivesConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("DriveLetter", IsKey = true, IsRequired = true)]
        public string DriveLetter
        {
            get { return (string)base["DriveLetter"]; }
            set { base["DriveLetter"] = value; }
        }

        [ConfigurationProperty("Label", IsRequired = true)]
        public string Label
        {
            get { return (string)base["Label"]; }
            set { base["Label"] = value; }
        }

        [ConfigurationProperty("Share", IsRequired = true)]
        public string Share
        {
            get { return (string)base["Share"]; }
            set { base["Share"] = value; }
        }

        [ConfigurationProperty("Username", IsRequired = false)]
        public string Username
        {
            get { return (string)base["Username"]; }
            set { base["Username"] = value; }
        }

        [ConfigurationProperty("Password", IsRequired = false)]
        public string Password
        {
            get { return (string)base["Password"]; }
            set { base["Password"] = value; }
        }

        [ConfigurationProperty("Persistent", IsRequired = true)]
        public bool Persistent
        {
            get { return (bool)base["Persistent"]; }
            set { base["Persistent"] = value; }
        }
    }
}
