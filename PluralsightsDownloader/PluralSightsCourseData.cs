using PluralsightsDownloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluralsightsDownloader
{
    class PluralSightsCourseData
    {

        public class SkillPath
        {
            public string id { get; set; }
            public string title { get; set; }
            public bool retired { get; set; }
            public string url { get; set; }
            public string urlSlug { get; set; }
            public string iconUrl { get; set; }
        }

        public class Retired
        {
            public bool isRetired { get; set; }
            public string reason { get; set; }
            public string replacementId { get; set; }
        }

        public class Rating
        {
            public double average { get; set; }
            public int ratersCount { get; set; }
        }

        public class Clip
        {
            public string id { get; set; }
            public string title { get; set; }
            public string duration { get; set; }
            public string playerUrl { get; set; }
        }

        public class Module
        {
            public string id { get; set; }
            public string title { get; set; }
            public string duration { get; set; }
            public string playerUrl { get; set; }
            public List<Clip> clips { get; set; }
        }

        public class CourseImage
        {
            public object courseListUrl { get; set; }
            public object defaultUrl { get; set; }
            public object smallUrl { get; set; }
        }

        public class Author
        {
            public string id { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string displayName { get; set; }
        }

        public class RootObject
        {
            public string id { get; set; }
            public string publishedOn { get; set; }
            public string updatedOn { get; set; }
            public string title { get; set; }
            public string shortDescription { get; set; }
            public string description { get; set; }
            public string level { get; set; }
            public string duration { get; set; }
            public int popularityScore { get; set; }
            public bool hasTranscript { get; set; }
            public bool hasLearningCheck { get; set; }
            public string playerUrl { get; set; }
            public List<SkillPath> skillPaths { get; set; }
            public Retired retired { get; set; }
            public Rating rating { get; set; }
            public List<Module> modules { get; set; }
            public CourseImage courseImage { get; set; }
            public List<Author> authors { get; set; }
            public List<string> audiences { get; set; }
        }
    }

    public class DownloadData
    {
        public class Url
        {
            public int rank { get; set; }
            public string cdn { get; set; }
            public string url { get; set; }
        }

        public class RootObject
        {
            public List<Url> urls { get; set; }
        }
    }

    public class RPC
    {

        public class Course
        {
            public string title { get; set; }
            public string name { get; set; }
            public bool courseHasCaptions { get; set; }
            public bool supportsWideScreenVideoFormats { get; set; }
            public string timestamp { get; set; }
        }

        public class Payload
        {
            public Course course { get; set; }
        }

        public class RootObject
        {
            
            public Payload payload { get; set; }
            
        }

}
}

