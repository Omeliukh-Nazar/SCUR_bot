using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCUR_bot
{
    public class GetRandomPhoto
    {
        public static string GetRandomPhotoOf(string name)
        {
            var list = ParseHtml($"https://www.google.com/search?q={name}&tbm=isch");
            return GetRandom(list);
        }
        private static List<string> ParseHtml(string url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);
            HtmlNodeCollection divs = doc.DocumentNode.SelectNodes("//img");
            var list = new List<string>();
            foreach (var item in divs)
            {
                list.Add(item.GetAttributeValue("data-src", ""));
            }
            return list;
        }
        private static string GetRandom(List<string> list)
        {
            Random rnd = new Random();
            int value = rnd.Next(0, list.Count);
            return list[value];
        }
    }

}
