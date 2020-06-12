using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TextClassification.Contracts;

namespace TextClassification.Client
{
    public class AppBase : ComponentBase
    {
        private IReadOnlyList<TextSample> _textSamples;

        private int _skipTextSamples;

        private FiltersEnum _textSampleFilter;

        [Inject]
        public HttpClient HttpClient { get; set; }

        public IReadOnlyList<Label> Labels { get; private set; }

        public TextSample CurrentTextSample
        {
            get
            {
                var defaultValue = new TextSample();

                return _textSampleFilter switch
                {
                    FiltersEnum.ShowAll => _textSamples.Skip(_skipTextSamples).FirstOrDefault() ?? defaultValue,
                    FiltersEnum.WithLabels => _textSamples.Skip(_skipTextSamples).FirstOrDefault(ts => ts.Labels.Count() > 0) ?? defaultValue,
                    FiltersEnum.WithoutLabels => _textSamples.Skip(_skipTextSamples).FirstOrDefault(ts => ts.Labels.Count() == 0) ?? defaultValue,
                    _ => defaultValue,
                };
            }
        }

        protected override async Task OnInitializedAsync()
        {
            _textSamples = 
                await HttpClient.GetFromJsonAsync<TextSample[]>(
                    "sample-data/text-samples.json");
            
            Labels = 
                await HttpClient.GetFromJsonAsync<Label[]>(
                    "sample-data/labels.json");

            _textSampleFilter = FiltersEnum.ShowAll;
        }

        public void ShowNextTextSample()
        {
            _skipTextSamples++;

            if (_skipTextSamples == _textSamples.Count)
            {
                _skipTextSamples = 0;
            }
        }

        public void OnFilterSelected(ChangeEventArgs args)
        {
            _textSampleFilter = (FiltersEnum)Enum.Parse(typeof(FiltersEnum), args.Value.ToString(), ignoreCase: true);
            _skipTextSamples = 0;
        }

        public void AddLabel(Label label)
        {
            var textSample = _textSamples.FirstOrDefault(ts => ts.Id == CurrentTextSample.Id);

            if (textSample == null) return;

            var hasLabel = textSample.Labels.Any(l => l.Id == label.Id);

            if (hasLabel) return;

            var newLabels = textSample.Labels.ToList();

            newLabels.Add(label);

            textSample.Labels = newLabels;
        }

        public void RemoveLabel(Label label)
        {
            var textSample = _textSamples.FirstOrDefault(ts => ts.Id == CurrentTextSample.Id);

            if (textSample == null) return;

            var hasLabel = textSample.Labels.Any(l => l.Id == label.Id);

            if (!hasLabel) return;

            var newLabels = textSample.Labels.ToList();

            newLabels.Remove(label);

            textSample.Labels = newLabels;
        }
    }
}
