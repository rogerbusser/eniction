using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Eniction
{
    public class ScoreData
    {
        public Dictionary<string, string> FeatureVector { get; set; }
        public Dictionary<string, string> GlobalParameters { get; set; }
    }

    public class ScoreRequest
    {
        public string Id { get; set; }
        public ScoreData Instance { get; set; }
    }

    //public class ScoreResponse
    //{
    //    public string Now { get; set; }
    //    public string E05 { get; set; }
    //    public string E10 { get; set; }
    //    public string E15 { get; set; }
    //    public string E20 { get; set; }
    //    public string E25 { get; set; }
    //    public string Ritme { get; set; }
    //    public string Label { get; set; }
    //    public string Probability { get; set; }
    //}

    public class ScoreResponse
    {
        public List<string> Params {get;set;}
    }

}
