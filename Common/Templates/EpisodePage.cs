using HandlebarsDotNet;
using System;
using System.IO;
using System.Reflection;

namespace Common.Templates
{
    public static class EpisodePage
    {
        static EpisodePage()
        {   
            var dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var template = File.ReadAllText(Path.Combine(dir, @"Templates\EpisodePage.handlebars"));
            Template = Handlebars.Compile(template);
        }

        public static Func<object, string> Template {get; set;}
    }
}