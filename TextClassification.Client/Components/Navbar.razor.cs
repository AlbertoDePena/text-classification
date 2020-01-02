using Microsoft.AspNetCore.Components;
using System;

namespace TextClassification.Client.Components
{
    public class NavbarBase : ComponentBase
    {
        public void DisplayTextSamplesDialog()
        {
            Console.WriteLine("DisplayTextSamplesDialog clicked");
        }

        public void DisplayLabelsDialog()
        {
            Console.WriteLine("DisplayLabelsDialog clicked");
        }
    }
}
