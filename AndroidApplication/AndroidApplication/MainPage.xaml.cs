using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Xml;
using System.Collections.Generic;

namespace AndroidApplication
{
    public partial class MainPage : ContentPage
    {
        private async Task<string> GetContentFromUrl()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://partner.market.yandex.ru/pages/help/YML.xml");
            string content = await response.Content.ReadAsStringAsync();
            return content;
        }

        private List<string> GetOffersId(string content)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(content);
            var xRoot = xmlDoc.DocumentElement;
            var shopTag = xRoot.LastChild;
            var offersList = new List<string>();

            if (shopTag != null)
            {
                foreach (XmlNode node in shopTag.ChildNodes)
                {
                    if (node.Name == "offers")
                    {
                        foreach (XmlNode offer in node.ChildNodes)
                        {
                            offersList.Add(offer.Attributes.GetNamedItem("id").Value);
                        }
                    }
                }
            }

            return offersList;
        }

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            var content = await GetContentFromUrl();
            var offersIds = GetOffersId(content);
            var listView = new ListView();
            listView.ItemsSource = offersIds;

            var stackLayout = new StackLayout()
            {
                Children = { listView }
            };
            
            Content = stackLayout;
        }
    }
}
