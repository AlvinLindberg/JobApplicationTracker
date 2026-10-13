using System;

namespace JobApplicationTracker;

class Program
{
    static void Main()
    {
        JobManager manager = new JobManager();
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n===== JOB APPLICATION TRACKER =====");
            Console.WriteLine("1. Lägg till ny ansökan");
            Console.WriteLine("2. Visa alla ansökningar");
            Console.WriteLine("3. Filtrera efter status");
            Console.WriteLine("4. Sortera efter datum");
            Console.WriteLine("5. Visa statistik");
            Console.WriteLine("6. Uppdatera status");
            Console.WriteLine("7. Ta bort ansökan");
            Console.WriteLine("8. Visa obesvarade ansökningar äldre än 14 dagar");
            Console.WriteLine("9. Avsluta");
            Console.Write("Välj alternativ: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    AddApplication(manager);
                    break;

                case "2":
                    manager.ShowAll();
                    break;

                case "3":
                    manager.ShowByStatus(ReadStatus());
                    break;

                case "4":
                    manager.ShowSortedByDate();
                    break;

                case "5":
                    manager.ShowStatistics();
                    break;

                case "6":
                    UpdateApplication(manager);
                    break;

                case "7":
                    RemoveApplication(manager);
                    break;

                case "8":
                    manager.ShowUnansweredOlderThan14Days();
                    break;

                case "9":
                    running = false;
                    Console.WriteLine("Programmet avslutas.");
                    break;

                default:
                    Console.WriteLine("Ogiltigt val. Välj 1–9.");
                    break;
            }
        }
    }

    static void AddApplication(JobManager manager)
    {
        Console.Write("Företag: ");
        string company = ReadRequiredText();

        Console.Write("Tjänst: ");
        string position = ReadRequiredText();

        Console.Write("Önskad månadslön i kronor: ");
        int salary = ReadNonNegativeInt();

        Console.Write("Ansökningsdatum (ÅÅÅÅ-MM-DD), Enter = idag: ");
        string dateInput = Console.ReadLine() ?? "";

        DateTime applicationDate;

        if (string.IsNullOrWhiteSpace(dateInput))
        {
            applicationDate = DateTime.Today;
        }
        else if (!DateTime.TryParse(dateInput, out applicationDate) ||
                 applicationDate.Date > DateTime.Today)
        {
            Console.WriteLine("Ogiltigt datum. Ansökan avbröts.");
            return;
        }

        JobApplication application = new JobApplication(
            company,
            position,
            ApplicationStatus.Applied,
            applicationDate,
            null,
            salary);

        manager.AddJob(application);
    }

    static void UpdateApplication(JobManager manager)
    {
        manager.ShowAll();

        if (manager.Applications.Count == 0)
            return;

        Console.Write("Vilket ansökningsnummer vill du uppdatera? ");
        int index = ReadInt() - 1;

        Console.WriteLine("Välj den nya statusen:");
        ApplicationStatus status = ReadStatus();

        manager.UpdateStatus(index, status);
    }

    static void RemoveApplication(JobManager manager)
    {
        manager.ShowAll();

        if (manager.Applications.Count == 0)
            return;

        Console.Write("Vilket ansökningsnummer vill du ta bort? ");
        int index = ReadInt() - 1;

        manager.RemoveJob(index);
    }

    static ApplicationStatus ReadStatus()
    {
        while (true)
        {
            Console.WriteLine("1. Applied");
            Console.WriteLine("2. Interview");
            Console.WriteLine("3. Offer");
            Console.WriteLine("4. Rejected");
            Console.Write("Välj status: ");

            switch (Console.ReadLine())
            {
                case "1":
                    return ApplicationStatus.Applied;
                case "2":
                    return ApplicationStatus.Interview;
                case "3":
                    return ApplicationStatus.Offer;
                case "4":
                    return ApplicationStatus.Rejected;
                default:
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                    break;
            }
        }
    }

    static int ReadInt()
    {
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int number))
                return number;

            Console.Write("Skriv ett giltigt heltal: ");
        }
    }

    static int ReadNonNegativeInt()
    {
        while (true)
        {
            int number = ReadInt();

            if (number >= 0)
                return number;

            Console.Write("Lönen kan inte vara negativ. Försök igen: ");
        }
    }

    static string ReadRequiredText()
    {
        while (true)
        {
            string input = Console.ReadLine() ?? "";

            if (!string.IsNullOrWhiteSpace(input))
                return input.Trim();

            Console.Write("Fältet får inte vara tomt. Försök igen: ");
        }
    }
}