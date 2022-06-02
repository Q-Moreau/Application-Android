/* Quentin MOREAU
 * 
 * FichesDeCalculs
 * 
 * 
 * Android Application developped in C# with Xamarin
 * 
 * The application creates a page thanks to the architecture file.
 * From there, the user can enter values and calculate others according to the page he chose.
 * When calculating a value, the application calls a F# function set in a shared project.
 * 
 */




using System;
using System.Collections.Generic;

using Xamarin.Forms;

using System.Xml;
using System.Reflection;
using System.IO;

namespace FichesDeCalculs
{
    public class Page : ContentPage
    {
        public Page(string sectionLabelParameter, string pageLabelParameter)
        {
            sectionLabel = sectionLabelParameter;
            pageLabel = pageLabelParameter;
            ScrollView scrollView = new ScrollView { Content = BuildPage() };
            this.Content = scrollView;
        }

        private string sectionLabel;
        private string pageLabel;

        private List<Entry> entries = new List<Entry>();
        private List<Label> results = new List<Label>();





        public Grid BuildPage()
        {
            /// reading xml file

            Assembly assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("FichesDeCalculs.architecture.xml");

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(stream);

            XmlNodeList xmlNodeListEntry = xmlDocument.SelectNodes("/architecture/section[@label=\"" + sectionLabel + "\"]/page[@label=\""
                + pageLabel + "\"]/entry");
            XmlNodeList xmlNodeListDisplay = xmlDocument.SelectNodes("/architecture/section[@label=\"" + sectionLabel + "\"]/page[@label=\""
                + pageLabel + "\"]/display");




            /// creating grid


            Grid grid = new Grid
            {
                Margin = new Thickness(10, 10, 10, 10),
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                },
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            grid.RowDefinitions = new RowDefinitionCollection();




            // header

            grid.RowDefinitions.Add(new RowDefinition());

            Label header = new Label
            {
                Text = pageLabel,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.White,
                Scale = 2
            };

            grid.Children.Add(new BoxView { Color = Color.Red }, 0, 3, grid.RowDefinitions.Count - 1, grid.RowDefinitions.Count);
            grid.Children.Add(header, 0, 3, grid.RowDefinitions.Count - 1, grid.RowDefinitions.Count);

            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());



            // adding entries

            foreach (XmlNode entryNode in xmlNodeListEntry)
            {
                grid.RowDefinitions.Add(new RowDefinition());


                Label label = new Label
                {
                    Text = entryNode.Attributes["label"].Value + " : ",
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Center
                };

                Entry entry = new Entry
                {
                    Placeholder = entryNode.Attributes["label"].Value,
                    Keyboard = Keyboard.Numeric,
                    ClassId = entryNode.Attributes["label"].Value,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                entries.Add(entry);

                Label unitLabel = new Label
                {
                    Text = entryNode.Attributes["unit"].Value,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Center
                };
                grid.Children.Add(label, 0, grid.RowDefinitions.Count - 1);
                grid.Children.Add(entry, 1, grid.RowDefinitions.Count - 1);
                grid.Children.Add(unitLabel, 2, grid.RowDefinitions.Count - 1);
            }


            // calculate button

            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            Button button = new Button
            {
                Text = "Calculate",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = Color.Red,
                TextColor = Color.White
            };
            button.Clicked += OnCalculateButtonClicked;

            grid.Children.Add(button, 0, 3, grid.RowDefinitions.Count - 1, grid.RowDefinitions.Count);

            grid.RowDefinitions.Add(new RowDefinition());




            // adding displays

            foreach (XmlNode displayNode in xmlNodeListDisplay)
            {
                grid.RowDefinitions.Add(new RowDefinition());


                Label label = new Label
                {
                    Text = displayNode.Attributes["label"].Value + " : ",
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Center
                };
                Label result = new Label
                {
                    Text = " ",
                    ClassId = displayNode.Attributes["label"].Value,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Center
                };
                results.Add(result);



                grid.Children.Add(label, 0, grid.RowDefinitions.Count - 1);
                grid.Children.Add(result, 1, 3, grid.RowDefinitions.Count - 1, grid.RowDefinitions.Count);
            }




            return grid;



        }







        






        void OnCalculateButtonClicked(object sender, EventArgs args)
        {
            try
            {

                /// reading xml file

                Assembly assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("FichesDeCalculs.architecture.xml");

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(stream);

                foreach (Label result in results)
                {

                    XmlNode displayNode = xmlDocument.SelectNodes("/architecture/section[@label=\"" + sectionLabel + "\"]/page[@label=\""
                        + pageLabel + "\"]/display[@label=\"" + result.ClassId + "\"]")[0];





                    string calcul = displayNode.InnerText;

                    calcul = calcul.Trim();

                    for (int i = 0; i < entries.Count; i++)
                    {
                        calcul = calcul.Replace("#" + i.ToString(), entries[i].Text);
                    }


                    result.Text = EvaluationChaineDeCaracteres.evaluateString.evaluateString(calcul).ToString("0.###") + " " + displayNode.Attributes["unit"].Value;

                }


            }
            catch(Exception error)
            {
                (sender as Button).Text = error.Message; //"ERROR";
            }

        }
    }
}