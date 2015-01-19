using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Newtonsoft.Json;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Text;
using Eniction.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Eniction
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        Task myREST = null;
        EnictionData _theData;

        public MainPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            this.NavigationCacheMode = NavigationCacheMode.Required;

            _theData = new EnictionData();
            this.DataContext = _theData; 
            _theData.Ritme = "Comfort";
            _theData.Temp = 65f;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void GetPrediction_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> vector = new Dictionary<string, string>() 
                    {
                        { "Elec now", "15" },
                        { "Elec -5", "5" },
                        { "Elec -10", "7" },
                        { "Elec -15", "8" },
                        { "Elec -20", "9" },
                        { "Elec -25", "9" },
                        { "Ritme", "0" },
                    };
            // Invoke Service Call to prediction engine
            if (myREST == null || myREST.Status != TaskStatus.Running)
            {
                myREST = InvokeRequestResponseService(_theData, vector,1);
            }
        }
        private void GetPrediction_Click2(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> vector = new Dictionary<string, string>() 
                    {
                            { "Elec now", "15" },
                            { "Elec -5", "20" },
                            { "Elec -10", "15" },
                            { "Elec -15", "5" },
                            { "Elec -20", "7" },
                            { "Elec -25", "8" },
                            { "Ritme", "0" },
                    };
            // Invoke Service Call to prediction engine
            if (myREST == null || myREST.Status != TaskStatus.Running)
            {
                myREST = InvokeRequestResponseService(_theData, vector,2);
            }
        }
        private void GetPrediction_Click3(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> vector = new Dictionary<string, string>() 
                    {
                            { "Elec now", "5" },
                            { "Elec -5", "7" },
                            { "Elec -10", "8" },
                            { "Elec -15", "9" },
                            { "Elec -20", "9" },
                            { "Elec -25", "10" },
                            { "Ritme", "0" },
                    };
            // Invoke Service Call to prediction engine
            if (myREST == null || myREST.Status != TaskStatus.Running)
            {
                myREST = InvokeRequestResponseService(_theData, vector,3);
            }
        }
        static async Task InvokeRequestResponseService(EnictionData theData, Dictionary<string, string> featureVector, int button)
        {
            using (var client = new HttpClient())
            {
                ScoreData scoreData = new ScoreData()
                {
                    FeatureVector = featureVector,
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };

                ScoreRequest scoreRequest = new ScoreRequest()
                {
                    Id = "score00001",
                    Instance = scoreData
                };

                const string apiKey = "INSERT YOUR API KEY HERE"; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/f17b03bd6f88427b869f89a389b5a87d/services/f938656e4eab4cfb84cff2a38d0c77f2/score");
                var json_object = JsonConvert.SerializeObject(scoreRequest);
                HttpContent content = new StringContent(json_object.ToString(), Encoding.UTF8);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync("", content);
                if (response.IsSuccessStatusCode)
                {
                    //string result = await response.Content.ReadAsStringAsync();
                    string jsonMessage;
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        jsonMessage = new StreamReader(responseStream).ReadToEnd();
                    }
                    var responseMessage= JsonConvert.DeserializeObject<List<string>>(jsonMessage);
                    switch(button)
                    {
                        case 1:
                            theData.Label = responseMessage[7];
                            break;
                        case 2:
                            theData.Label2 = responseMessage[7];
                            break;
                        case 3:
                            theData.Label3 = responseMessage[7];
                            break;
                    }
                    theData.Confidence = Double.Parse(responseMessage[8]);
                    theData.Temp= responseMessage[7] == "Slapen" ? 45f : 115f;
                    // 
                    //Console.WriteLine("Result: {0}", result);
                }
                else
                {
                    //Console.WriteLine("Failed with status code: {0}", response.StatusCode);
                }
            }
        }
    }
}
