using System;
using System.Collections.Generic;
using System.Net;

namespace Exercise_3._6
{
    class Program
    {
        // lists containing allowed/disallowed urlpaths
        private static readonly List<string> AllowedUrls = new List<string>();
        private static List<string> DisallowedUrls = new List<string>();

        static void Main(string[] args)
        {

            Console.Write("Enter URL: ");
            string urlStr = Console.ReadLine();

            Uri url = new UriBuilder(urlStr).Uri;
            string host = url.Host;
            Uri robotsTxtUrl = new UriBuilder(host + "/robots.txt").Uri;

            try
            {
                // remember to identity the agent
                WebClient wc = new WebClient();
                wc.Headers.Add(HttpRequestHeader.UserAgent, "bhp-bot; CS student practice crawler; Developer: Bent H. Pedersen (bhp@easv.dk)");
                wc.Headers.Add(HttpRequestHeader.From, "bhp@easv.dk");

                // fetching the robots.txt document. Will throw exception if not exist.
                String robotstxt = wc.DownloadString(robotsTxtUrl.ToString());

                // split the document into a line-array based on line breaks.
                string[] lines = robotstxt.ToLower().Split("\n");
                
                int i = 0;
                while (i < lines.Length && !lines[i].Contains("user-agent: *"))
                {
                    // not the right section of the document so next line, please
                    i++;
                }

                for (int line = i + 1; line < lines.Length && !lines[line].Contains("user-agent"); line++)
                {
                    // split the line into command and path
                    string[] lineSegment = lines[line].Split(':');
                    if (lineSegment.Length == 2)
                    {
                        string command = lineSegment[0].Trim(); // remember to trim
                        string path = lineSegment[1].Trim();    // remember to trim

                        if (command == "allow") // add link to allowedUrls
                        {
                            AllowedUrls.Add(path);
                        }
                        else if (command == "disallow")  // add link to disallowedUrls
                        {
                            DisallowedUrls.Add(path);
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("No robots.txt so just crawl on!");
            }

            // print out all allowed urls
            Console.WriteLine("\nAllowed URLs:");
            foreach (var path in AllowedUrls)
            {
                Console.WriteLine(path);
            }

            // print out all dis-allowed urls
            Console.WriteLine("\nDisallowed URLs:");
            foreach (var path in DisallowedUrls)
            {
                Console.WriteLine(path);
            }
        }
    }
}
