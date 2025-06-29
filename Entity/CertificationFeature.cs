namespace qa_dotnet_cucumber.Entity
{
    public class CertificationFeature
    {
        public List<TestItem> TestItems { get; set; }
    }

    public class TestItem
    {
        public CertificationDetails CertificationDetailsToAdd { get; set; }
        public CertificationDetails CertificationDetailsToUpdate { get; set; }
        public CertificationDetails CertificationDetailsToDelete { get; set; }
    }

    public class CertificationDetails
    {
        public string CertificateOrAward { get; set; }
        public string CertifiedFrom { get; set; }
        public string Year { get; set; }
        public string ExpectedMessage { get; set; }
    }
}



