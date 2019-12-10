namespace TextClassification.Contracts
{
    public class Text
    {
        public long Id { get; set; }

        public string Value { get; set; }

        public Label[] Labels { get; set; }
    }
}