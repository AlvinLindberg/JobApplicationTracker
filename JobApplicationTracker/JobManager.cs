using System;
using System.Collections.Generic;
using System.Linq;

namespace JobApplicationTracker;

public class JobManager
{
    public List<JobApplication> Applications { get; } = new();

    // Lägger till en ny ansökan.
    public void AddJob(JobApplication application)
    {
        Applications.Add(application);
        Console.WriteLine("Ansökan har lagts till!");
    }

    // Visar samtliga ansökningar.
    public void ShowAll()
    {
        if (Applications.Count == 0)
        {
            Console.WriteLine("Det finns inga ansökningar.");
            return;
        }

        for (int i = 0; i < Applications.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Applications[i].GetSummary()}");
        }
    }

    // Filtrerar ansökningar efter status med LINQ.
    public void ShowByStatus(ApplicationStatus status)
    {
        var filtered = Applications
            .Where(a => a.Status == status)
            .ToList();

        if (!filtered.Any())
        {
            Console.WriteLine("Inga ansökningar hittades.");
            return;
        }

        foreach (var application in filtered)
        {
            Console.WriteLine(application.GetSummary());
        }
    }

    // Sorterar efter ansökningsdatum med LINQ.
    public void ShowSortedByDate()
    {
        var sorted = Applications
            .OrderBy(a => a.ApplicationDate)
            .ToList();

        foreach (var application in sorted)
        {
            Console.WriteLine(application.GetSummary());
        }
    }

    // Visar statistik med LINQ.
    public void ShowStatistics()
    {
        Console.WriteLine("\n--- STATISTIK ---");
        Console.WriteLine($"Totalt antal ansökningar: {Applications.Count}");
        if (Applications.Any())
        {
            var highestSalary = Applications
                .OrderByDescending(a => a.SalaryExpectation)
                .First();

            Console.WriteLine(
                $"Högsta önskade lön: {highestSalary.SalaryExpectation:N0} kr " +
                $"hos {highestSalary.CompanyName}");
        }

        foreach (ApplicationStatus status in Enum.GetValues<ApplicationStatus>())
        {
            int count = Applications.Count(a => a.Status == status);
            Console.WriteLine($"{status}: {count}");
        }

        var answered = Applications
            .Where(a => a.ResponseDate.HasValue)
            .ToList();

        if (answered.Any())
        {
            double averageDays = answered.Average(
                a => (a.ResponseDate!.Value - a.ApplicationDate).TotalDays);

            Console.WriteLine($"Genomsnittlig svarstid: {averageDays:F1} dagar");
        }
        else
        {
            Console.WriteLine("Ingen svarstid kan beräknas ännu.");
        }

        var bySalary = Applications
            .OrderByDescending(a => a.SalaryExpectation);

        Console.WriteLine("\nAnsökningar sorterade efter önskad lön:");

        foreach (var application in bySalary)
        {
            Console.WriteLine(
                $"{application.CompanyName}: {application.SalaryExpectation:N0} kr");
        }
    }

    // Visar ansökningar utan svar som är äldre än 14 dagar.
    public void ShowUnansweredOlderThan14Days()
    {
        var unanswered = Applications
            .Where(a => !a.ResponseDate.HasValue &&
                        a.GetDaysSinceApplied() > 14)
            .OrderBy(a => a.ApplicationDate)
            .ToList();

        if (!unanswered.Any())
        {
            Console.WriteLine("Inga obesvarade ansökningar äldre än 14 dagar.");
            return;
        }

        foreach (var application in unanswered)
        {
            Console.WriteLine(
                $"{application.GetSummary()} | " +
                $"{application.GetDaysSinceApplied()} dagar sedan ansökan");
        }
    }

    // Uppdaterar status på en ansökan.
    public void UpdateStatus(int index, ApplicationStatus newStatus)
    {
        if (index < 0 || index >= Applications.Count)
        {
            Console.WriteLine("Ogiltigt ansökningsnummer.");
            return;
        }

        JobApplication application = Applications[index];
        application.Status = newStatus;

        if (newStatus == ApplicationStatus.Applied)
        {
            application.ResponseDate = null;
        }
        else
        {
            application.ResponseDate = DateTime.Today;
        }

        Console.WriteLine("Statusen har uppdaterats!");
    }

    // Tar bort en ansökan.
    public void RemoveJob(int index)
    {
        if (index < 0 || index >= Applications.Count)
        {
            Console.WriteLine("Ogiltigt ansökningsnummer.");
            return;
        }

        Applications.RemoveAt(index);
        Console.WriteLine("Ansökan har tagits bort.");
    }
}