/* Quentin MOREAU
 * 
 * FichesDeCalculs
 * 
 * 
 * Android Application developped in C# with Xamarin
 * 
 * This is the main page.
 * The application opens on this page and displays the main menu.
 * From there, the user can navigate to different sections.
 * 
 */




using System;
using System.ComponentModel;
using Xamarin.Forms;

using System.Xml;
using System.Reflection;
using System.IO;

namespace FichesDeCalculs
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            //InitializeComponent();
            ScrollView scrollView = new ScrollView { Content = Build() };
            this.Content = scrollView;
        }


        public StackLayout Build()
        {
            /// reading xml file

            Assembly assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("FichesDeCalculs.architecture.xml");

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(stream);

            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/architecture/section");




            /// creating layout


            StackLayout layout = new StackLayout
            {
                Margin = new Thickness(10, 10, 10, 10),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };


            foreach (XmlNode section in xmlNodeList)
            {
                Button button = new Button
                {
                    Text = section.Attributes["label"].Value,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.FromHsla(layout.Children.Count*0.025,1,0.45), // colors change periodically
                    TextColor = Color.White
                };
                button.Clicked += OnButtonClicked;
                layout.Children.Add(button);
            }



            return layout;



        }




        async void OnButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new Section((sender as Button).Text));
        }
    }
}
