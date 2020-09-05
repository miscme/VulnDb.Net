namespace VulnDb.Net.Models.Version
{
    public class VersionInformationOptions : IObjectOptions
    {
        /// <summary>
        /// If results returned should only be for vulnerable versions - value: true/false - defaults to true
        /// </summary>
        public bool Affected { get; set; }

        public VersionInformationOptions()
        {
            Affected = true;
        }

        public override string ToString()
        {
            return $@"&affected={Affected.ToString().ToLower()}";
        }
    }
}