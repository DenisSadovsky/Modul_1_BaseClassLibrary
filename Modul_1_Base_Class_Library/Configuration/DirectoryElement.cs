using System;
using System.Configuration;

namespace Modul_1_Base_Class_Library.Configuration
{
    class DirectoryElement : ConfigurationElement
    {
        [ConfigurationProperty("path", IsRequired = true, IsKey = true)]
        public string Path => (string)this["path"];
    }
}
