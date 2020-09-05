namespace VulnDb.Net.Models.Classification
{
    public class ClassificationInformationOptions : IObjectOptions
    {
        public bool Category { get; set; }

        public ClassificationInformationOptions()
        {
            Category = false;
        }

        public override string ToString()
        {
            return $@"&category={Category.ToString().ToLower()}";
        }
    }
}