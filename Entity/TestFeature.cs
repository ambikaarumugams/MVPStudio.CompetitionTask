namespace qa_dotnet_cucumber.Entity
{
    public class TestFeature
    {
        public Scenario[] Scenarios { get; set; }

        public class Scenario
        {
            public string ScenarioName { get; set; }
            public TestData[] TestItems { get; set; }
        }

        public class TestData
        {
            public EducationDetails EducationDetails { get; set; }
            public EducationDetails EducationDetailsToUpdate { get; set; }
           // public MessageDetails MessageDetails { get; set; }
        }

        public class EducationDetails
        {
            public string CollegeUniversityName { get; set; }
            public string Country { get; set; }
            public string Title { get; set; }
            public string Degree { get; set; }
            public string YearOfGraduation { get; set; }
        }

        //public class MessageDetails
        //{
        //    public bool IsError { get; set; } 
        //    public string Message { get; set; }
        //}

    }
}
