using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TextClassification.Contracts;

namespace TextClassification.Client.Components
{
    public class LabelsSelectorBase : ComponentBase
    {
        private IReadOnlyList<Label> _labels;

        [Inject]
        public HttpClient Http { get; set; }

        [Parameter]
        public EventCallback<Label> LabelSelected { get; set; }

        public IReadOnlyList<Label> FilteredLabels;

        protected override async Task OnInitializedAsync()
        {
            _labels = await Http.GetJsonAsync<Label[]>("sample-data/labels.json");

            FilterLabels(filter: string.Empty);
        }

        public Task OnLabelSelectedAsync(Label label) => LabelSelected.InvokeAsync(label);

        public void OnFilterLabelChanged(ChangeEventArgs args) => FilterLabels(filter: args.Value as string);

        private void FilterLabels(string filter)
        {
            FilteredLabels =
                string.IsNullOrWhiteSpace(filter) ?
                _labels :
                _labels.Where(l => l.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
