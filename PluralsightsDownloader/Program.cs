using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PluralsightsDownloader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            HttpClient client = new HttpClient();
            FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("RedirectUrl", "/"),
                new KeyValuePair<string, string>("Username", ""),
                new KeyValuePair<string, string>("Password", ""),
                new KeyValuePair<string, string>("ShowCaptcha", "False"),
                new KeyValuePair<string, string>("ReCaptchaSiteKey", "6LeVIgoTAAAAAIhx_TOwDWIXecbvzcWyjQDbXsaV") // copied this from the website .. dont know whether this ever changes
            });
            var result = client.PostAsync("https://app.pluralsight.com/id/", content).Result;
            var resultContent = result.Content.ReadAsStringAsync().Result;
            if (resultContent.Contains("currentUser"))
            {
                // logged in
                DownloadCourse(client, "patterns-library");
            }
        }

        static void DownloadCourse(HttpClient client, string courseName)
        {
            
            var result = client.GetAsync("https://app.pluralsight.com/learner/content/courses/" + courseName).Result;
            var resultContent = result.Content.ReadAsStringAsync().Result;
            var resultData = Newtonsoft.Json.JsonConvert.DeserializeObject<PluralSightsCourseData.RootObject>(resultContent);

            string basePath = resultData.title.Replace(":","");
            Directory.CreateDirectory(basePath);

            var boundries = getVideoBoundries(client, courseName);
            for (int m = 23; m < resultData.modules.Count; m++)
            {
                string path = basePath+ "\\" + (m+1) + ". " + resultData.modules[m].title;
                Directory.CreateDirectory(path);
                
                for (int c = 0; c < resultData.modules[m].clips.Count; c++)
                {
                    DownloadClip(client, resultData.modules[m].clips[c], path  +"\\" + (c+1) + ". " + resultData.modules[m].clips[c].title +".mp4", boundries);
                    for (int i = 0; i < 60; i++)
                    {
                        System.Threading.Thread.Sleep(1000);
                        Console.WriteLine("Wait " + (60-i) + " Seconds");
                    }
                    
                }
            }
        }

        private static string getVideoBoundries(HttpClient client, string courseName)
        {
            
            var httpContent = new StringContent("{\"fn\":\"bootstrapPlayer\",\"payload\":{\"courseId\":\""+ courseName + "\"}}", Encoding.UTF8, "application/json");
            var result = client.PostAsync("https://app.pluralsight.com/player/functions/rpc", httpContent).Result;
            var resultContent = result.Content.ReadAsStringAsync().Result;
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<RPC.RootObject>(resultContent);
            return data.payload.course.supportsWideScreenVideoFormats ? "1280x720" : "1024x768";
        }

        static void DownloadClip(HttpClient client, PluralSightsCourseData.Clip clip, string localFilePath, string boundries)
        {

            var id = clip.id;
            string courseName = id.Split('|')[0];
            string author = id.Split('|')[1];
            string module = id.Split('|')[2];
            string clipName = id.Split('|')[3];
            string clipIndex = clip.playerUrl;
            clipIndex = clipIndex.Substring(clipIndex.IndexOf("clip=") + 5);
            clipIndex = clipIndex.Substring(0, clipIndex.IndexOf("&"));
            
            var httpContent = new StringContent("{\"author\":\""+ author + "\",\"includeCaptions\":false,\"clipIndex\":"+ clipIndex + ",\"courseName\":\"" + courseName + "\",\"locale\":\"en\",\"moduleName\":\""+ module + "\",\"mediaType\":\"mp4\",\"quality\":\""+ boundries + "\"}", Encoding.UTF8, "application/json");
            var result = client.PostAsync("https://app.pluralsight.com/video/clips/viewclip", httpContent).Result;
            var resultContent = result.Content.ReadAsStringAsync().Result;
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<DownloadData.RootObject>(resultContent);
            var file = client.GetByteArrayAsync(data.urls[0].url).Result;
            File.WriteAllBytes(localFilePath.Replace("?","").Replace(":","").Replace("w/","").Replace("/",""), file);
        }
        //static void OpenCourse(HttpClient client, string courseUrl)
        //{
        //    var result = client.GetAsync(courseUrl).Result;
        //    var resultContent = result.Content.ReadAsStringAsync().Result;
        //    var json = resultContent.Substring(resultContent.IndexOf("<script type=\"text/javascript\">__INITIAL_STATE__ = ") + "<script type=\"text/javascript\">__INITIAL_STATE__ = ".Length);
        //    json = json.Substring(0, json.IndexOf("</script>"));

        //}
    }
}
