using System;

namespace JobApplicationTracker;

class Program
{
    // Programmets startpunkt
    static void Main()
    {
        JobManager manager = new JobManager();
        bool running = true;

        // Visar menyn tills användaren väljer att avsluta
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

            // Läser in användarens menyval
            string choice = Console.ReadLine() ?? "";

            // Väljer vilken funktion som ska köras
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
                    // Avslutar while-loopen
                    running = false;
                    Console.WriteLine("Programmet avslutas.");
                    break;

                default:
                    Console.WriteLine("Ogiltigt val. Välj 1–9.");
                    break;
            }
        }
    }

    // Lägger till en ny jobbansökan
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

        // Om inget datum anges används dagens datum
        if (string.IsNullOrWhiteSpace(dateInput))
        {
            applicationDate = DateTime.Today;
        }
        // Kontrollerar att datumet är giltigt och inte ligger i framtiden
        else if (!DateTime.TryParse(dateInput, out applicationDate) ||
                 applicationDate.Date > DateTime.Today)
        {
            Console.WriteLine("Ogiltigt datum. Ansökan avbröts.");
            return;
        }

        // Skapar ett nytt jobbansökningsobjekt
        JobApplication application = new JobApplication(
            company,
            position,
            ApplicationStatus.Applied,
            applicationDate,
            null,
            salary);

        // Lägger till ansökan via JobManager
        manager.AddJob(application);
    }

    // Uppdaterar statusen för en befintlig ansökan
    static void UpdateApplication(JobManager manager)
    {
        manager.ShowAll();

        // Avslutar metoden om listan är tom
        if (manager.Applications.Count == 0)
            return;

        Console.Write("Vilket ansökningsnummer vill du uppdatera? ");

        // Minus 1 eftersom listans index börjar på 0
        int index = ReadInt() - 1;

        Console.WriteLine("Välj den nya statusen:");
        ApplicationStatus status = ReadStatus();

        manager.UpdateStatus(index, status);
    }

    // Tar bort en jobbansökan
    static void RemoveApplication(JobManager manager)
    {
        manager.ShowAll();

        // Det finns inget att ta bort om listan är tom
        if (manager.Applications.Count == 0)
            return;

        Console.Write("Vilket ansökningsnummer vill du ta bort? ");

        // Omvandlar användarens nummer till ett listindex
        int index = ReadInt() - 1;

        manager.RemoveJob(index);
    }

    // Läser in och returnerar användarens valda status
    static ApplicationStatus ReadStatus()
    {
        // Frågar igen tills användaren väljer en giltig status
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

    // Läser in ett heltal och försöker igen om inmatningen är ogiltig
    static int ReadInt()
    {
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int number))
                return number;

            Console.Write("Skriv ett giltigt heltal: ");
        }
    }

    // Kontrollerar att ett heltal inte är negativt
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

    // Läser in text och kontrollerar att fältet inte är tomt
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