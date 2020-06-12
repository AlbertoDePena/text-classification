using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TextClassification.Contracts;

namespace TextClassification.Client.Components
{
    public class LabelsSelectorBase : ComponentBase
    {
        [Parameter]
        public EventCallback<Label> LabelSelected { get; set; }

        [Parameter]
        public IEnumerable<Label> Labels { get; set; }

        public IEnumerable<Label> DataSource { get; private set; }

        protected override async Task OnInitializedAsync() 
        {
            while (Labels == null)
            {
                await Task.Delay(millisecondsDelay: 1);
            }

            FilterLabels(filter: string.Empty);
        }

        public Task OnLabelSelectedAsync(Label label) 
            => LabelSelected.InvokeAsync(label);

        public void FilterLabels(string filter)
        {
            DataSource =
                string.IsNullOrWhiteSpace(filter) ?
                Labels?.ToList() :
                Labels?.Where(l => l.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
