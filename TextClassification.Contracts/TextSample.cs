using System.Linq;
using System.Collections.Generic;

namespace TextClassification.Contracts
{
    public class TextSample
    {
        public long Id { get; set; }

        public string Value { get; set; }

        public IEnumerable<Label> Labels { get; set; } = Enumerable.Empty<Label>();
    }
}