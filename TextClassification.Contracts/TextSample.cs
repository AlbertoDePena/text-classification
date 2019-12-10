namespace TextClassification.Contracts
{
    public class TextSample
    {
        public long Id { get; set; }

        public string Value { get; set; }

        public Label[] Labels { get; set; } = new Label[0];
    }
}