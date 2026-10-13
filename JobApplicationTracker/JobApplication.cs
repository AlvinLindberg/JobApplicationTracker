namespace JobApplicationTracker;

public class JobApplication
{
    public string CompanyName { get; set; }
    public string PositionTitle { get; set; }
    public ApplicationStatus Status { get; set; }
    public DateTime ApplicationDate { get; set; }
    public DateTime? ResponseDate { get; set; }
    public int SalaryExpectation { get; set; }

    public JobApplication(
        string companyName,
        string positionTitle,
        ApplicationStatus status,
        DateTime applicationDate,
        DateTime? responseDate,
        int salaryExpectation)
    {
        CompanyName = companyName;
        PositionTitle = positionTitle;
        Status = status;
        ApplicationDate = applicationDate;
        ResponseDate = responseDate;
        SalaryExpectation = salaryExpectation;
    }

    public int GetDaysSinceApplied()
    {
        return (DateTime.Today - ApplicationDate.Date).Days;
    }

    public string GetSummary()
    {
        return $"{CompanyName} | {PositionTitle} | {Status} | " +
               $"Ansökningsdatum: {ApplicationDate:yyyy-MM-dd} | " +
               $"Önskad lön: {SalaryExpectation:N0} kr";
    }
}