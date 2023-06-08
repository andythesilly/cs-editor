using System.Configuration;

namespace Editor
{
    public class UISettings : ConfigurationSection
    {
        [ConfigurationProperty("fontSize", DefaultValue = 10)]
        public int FontSize
        {
            get { return (int)this["fontSize"]; }
            set { this["fontSize"] = value; }
        }

        [ConfigurationProperty("fontFamily", DefaultValue = "Consolas")]
        public string FontFamily
        {
            get { return (string)this["fontFamily"]; }
            set { this["fontFamily"] = value; }
        }

        [ConfigurationProperty("wrap", DefaultValue = false)]
        public bool Wrap
        {
            get { return (bool)this["wrap"]; }
            set { this["wrap"] = value; }
        }
    }
}
