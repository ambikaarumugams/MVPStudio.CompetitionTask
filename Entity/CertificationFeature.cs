namespace qa_dotnet_cucumber.Entity
{
    public class CertificationFeature
    {
         public List<Scenario> Scenarios { get; set; }

        public class Scenario
        {
            public string ScenarioName { get; set; }
            public List<TestItem> TestItems { get; set; }
        }

        public class TestItem
        {
            public CertificationDetailsToAdd CertificationDetailsToAdd { get; set; }
            public object CertificationDetailsToUpdate { get; set; }
            public object CertificationDetailsToDelete { get; set; }
        }

        public class CertificationDetailsToAdd
        {
            public string CertificateOrAward { get; set; }
            public string CertifiedFrom { get; set; }
            public string Year { get; set; }
        }

    }
}
