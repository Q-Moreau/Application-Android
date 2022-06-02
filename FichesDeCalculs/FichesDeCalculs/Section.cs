/* Quentin MOREAU
 * 
 * FichesDeCalculs
 * 
 * 
 * Android Application developped in C# with Xamarin
 * 
 * This is the section page.
 * The application creates a section page thanks to the architecture file.
 * From there, the user can choose a page according to the section he chose.
 * 
 */




using System;

using Xamarin.Forms;

using System.Xml;
using System.Reflection;
using System.IO;

namespace FichesDeCalculs
{
    public class Section : ContentPage
    {
        public Section(string sectionLabelParameter)
        {
            sectionLabel = sectionLabelParameter;
            ScrollView scrollView = new ScrollView { Content = BuildSection() };
            this.Content = scrollView;
        }

        private string sectionLabel;




        public StackLayout BuildSection()
        {
            /// reading xml file

            Assembly assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("FichesDeCalculs.architecture.xml");

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(stream);

            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/architecture/section[@label=\"" + sectionLabel + "\"]/page");




            /// creating layout


            StackLayout layout = new StackLayout
            {
                Margin = new Thickness(10, 10, 10, 10),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };


            foreach (XmlNode page in xmlNodeList)
            {
                Button button = new Button
                {
                    Text = page.Attributes["label"].Value,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.FromHsla(layout.Children.Count*0.025, 1, 0.45),
                    TextColor = Color.White
                };
                button.Clicked += OnButtonClicked;
                layout.Children.Add(button);
            }



            return layout;



        }




        async void OnButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new Page(sectionLabel, (sender as Button).Text));
        }
    }
}