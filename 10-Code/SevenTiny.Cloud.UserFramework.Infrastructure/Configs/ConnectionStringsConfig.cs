﻿using SevenTiny.Bantina.Bankinate.Attributes;
using SevenTiny.Bantina.Configuration;
using System.Collections.Generic;
using System.Linq;


namespace SevenTiny.Cloud.UserFramework.Infrastructure.Configs
{
    [ConfigName("ConnectionStrings")]
    public class ConnectionStringsConfig : ConfigBase<ConnectionStringsConfig>
    {
        [Column]
        public string Key { get; set; }
        [Column]
        public string Value { get; set; }

        private static Dictionary<string, string> _configs;

        public static string Get(string key)
        {
            if (_configs == null)
            {
                _configs = Configs.ToDictionary(t => t.Key, v => v.Value);
            }
            if (_configs.ContainsKey(key))
            {
                return _configs[key];
            }
            return string.Empty;
        }
    }
}
