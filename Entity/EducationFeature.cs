namespace qa_dotnet_cucumber.Entity
{
    public class EducationFeature
    {
        public TestData[] TestItems { get; set; }
    }

    public class TestData
    {
        public EducationDetails EducationDetails { get; set; }
        public EducationDetails EducationDetailsToUpdate { get; set; }
        public EducationDetails EducationDetailsToDelete { get; set; }
    }

    public class EducationDetails
    {
        public string CollegeUniversityName { get; set; }
        public string Country { get; set; }
        public string Title { get; set; }
        public string Degree { get; set; }
        public string YearOfGraduation { get; set; }
    }
}
