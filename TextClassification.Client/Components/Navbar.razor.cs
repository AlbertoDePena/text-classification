using Microsoft.AspNetCore.Components;
using System;

namespace TextClassification.Client.Components
{
    public class NavbarBase : ComponentBase
    {
        protected void DisplayTextSamplesDialog()
        {
            Console.WriteLine("DisplayTextSamplesDialog clicked");
        }

        protected void DisplayLabelsDialog()
        {
            Console.WriteLine("DisplayLabelsDialog clicked");
        }
    }
}
