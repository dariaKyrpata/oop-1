using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using FileStorage;

public class ConsoleMenu
{
    private readonly PersonService service;
    private readonly PersonRepository repository;
    private readonly ITextFileStore fileStore;
    private readonly string filePath = "people.txt";

    public ConsoleMenu(
        PersonService service,
        PersonRepository repository,
        ITextFileStore fileStore)
    {
        this.service = service;
        this.repository = repository;
        this.fileStore = fileStore;
    }

    public void Run()
    {
        LoadData();

        while (true)
        {
            Console.WriteLine("\n=== OOP LAB 3.1 / VARIANT 14 ===");
            Console.WriteLine("1 - Add a person");
            Console.WriteLine("2 - Show all people");
            Console.WriteLine("3 - Search by last name");
            Console.WriteLine("4 - Search by unique ID");
            Console.WriteLine("5 - Delete by unique ID");
            Console.WriteLine("6 - Fourth-year students without scholarship");
            Console.WriteLine("7 - Perform a study session");
            Console.WriteLine("8 - Draw a picture");
            Console.WriteLine("9 - Load sample data");
            Console.WriteLine("0 - Exit");
            Console.Write("Choose an option: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1": AddPerson(); break;
                    case "2": ShowAll(); break;
                    case "3": SearchByLastName(); break;
                    case "4": SearchById(); break;
                    case "5": DeletePerson(); break;
                    case "6": ShowFourthYearStudents(); break;
                    case "7": Study(); break;
                    case "8": Draw(); break;
                    case "9": LoadSampleData(); break;
                    case "0":
                        SaveData();
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }

    private string ReadRequired(string message)
    {
        while (true)
        {
            Console.Write(message);
            string value = (Console.ReadLine() ?? "").Trim();

            if (!string.IsNullOrWhiteSpace(value))
                return value;

            Console.WriteLine("This field is required.");
        }
    }

    private string ReadName(string message)
    {
        while (true)
        {
            string value = ReadRequired(message);

            if (PersonValidator.IsValidName(value))
                return value;

            Console.WriteLine("Invalid name.");
        }
    }

    private string ReadId(string message)
    {
        while (true)
        {
            string value = ReadRequired(message);

            if (PersonValidator.IsValidUniqueId(value))
                return value;

            Console.WriteLine("Invalid ID.");
        }
    }

    private int ReadCourse()
    {
        while (true)
        {
            Console.Write("Course (1-6): ");

            if (int.TryParse(Console.ReadLine(), out int course)
                && PersonValidator.IsValidCourse(course))
                return course;

            Console.WriteLine("Invalid course.");
        }
    }

    private decimal ReadScholarship()
    {
        while (true)
        {
            Console.Write("Scholarship amount: ");

            string value = (Console.ReadLine() ?? "")
                .Replace(',', '.');

            if (decimal.TryParse(
                value,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out decimal scholarship)
                && PersonValidator.IsValidScholarship(scholarship))
                return scholarship;

            Console.WriteLine("Invalid scholarship amount.");
        }
    }

    private void AddPerson()
    {
        Console.WriteLine("1 - Student");
        Console.WriteLine("2 - Postman");
        Console.WriteLine("3 - Mountaineer");

        string type = ReadRequired("Person type: ");

        if (type != "1" && type != "2" && type != "3")
        {
            Console.WriteLine("Invalid person type.");
            return;
        }

        string firstName = ReadName("First name: ");
        string lastName = ReadName("Last name: ");

        Person person;

        if (type == "1")
        {
            int course = ReadCourse();

            string studentId;
            do
            {
                studentId = ReadRequired("Student ID (AB123456): ");
            }
            while (!ValidateStudentId(studentId));

            decimal scholarship = ReadScholarship();

            string passport;
            do
            {
                passport = ReadRequired("Passport (AB123456): ");
            }
            while (!ValidatePassport(passport));

            person = new Student(
                firstName, lastName, course,
                studentId, scholarship, passport);
        }
        else if (type == "2")
        {
            string id = ReadId("Employee ID: ");
            person = new Postman(firstName, lastName, id);
        }
        else
        {
            string id = ReadId("Mountaineer ID: ");
            person = new Mountaineer(firstName, lastName, id);
        }

        if (service.AddPerson(person))
        {
            SaveData();
            Console.WriteLine("Person added successfully.");
        }
        else
        {
            Console.WriteLine("A person with this ID already exists.");
        }
    }

    private bool ValidateStudentId(string value)
    {
        if (PersonValidator.IsValidStudentId(value))
            return true;

        Console.WriteLine("Invalid student ID format.");
        return false;
    }

    private bool ValidatePassport(string value)
    {
        if (PersonValidator.IsValidPassport(value))
            return true;

        Console.WriteLine("Invalid passport format.");
        return false;
    }

    private void ShowPerson(Person person)
    {
        Console.WriteLine("\nType: " + person.GetType().Name);
        Console.WriteLine("First name: " + person.FirstName);
        Console.WriteLine("Last name: " + person.LastName);
        Console.WriteLine("Unique ID: " + person.GetUniqueId());

if (person is Student student)
{
    Console.WriteLine("Course: " + student.Course);
    Console.WriteLine("Scholarship: " + student.Scholarship);
    Console.WriteLine("Passport: " + student.Passport);
    Console.WriteLine("Study sessions: " + student.StudySessions);
    Console.WriteLine("Drawings: " + student.DrawingsCount);
}
       
    }

    private void ShowAll()
    {
        Person[] people = service.GetAll();

        if (people.Length == 0)
        {
            Console.WriteLine("Database is empty.");
            return;
        }

        foreach (Person person in people)
            ShowPerson(person);
    }

    private void SearchByLastName()
    {
        string lastName = ReadName("Last name: ");
        Person[] people = service.FindByLastName(lastName);

        Console.WriteLine("Found: " + people.Length);

        foreach (Person person in people)
            ShowPerson(person);
    }

    private void SearchById()
    {
        string id = ReadRequired("Unique ID: ");
        Person? person = service.FindById(id);

        if (person == null)
            Console.WriteLine("Person not found.");
        else
            ShowPerson(person);
    }

    private void DeletePerson()
    {
        string id = ReadRequired("Unique ID to delete: ");

        if (service.DeleteById(id))
        {
            SaveData();
            Console.WriteLine("Person deleted.");
        }
        else
        {
            Console.WriteLine("Person not found.");
        }
    }

    private void ShowFourthYearStudents()
    {
        LoadData();

        Student[] students =
            service.GetFourthYearStudentsWithoutScholarship();

        Console.WriteLine(
            "Fourth-year students without scholarship: "
            + students.Length);

        foreach (Student student in students)
            ShowPerson(student);
    }

    private void Study()
    {
        string id = ReadRequired("Unique ID: ");
        Person? person = service.FindById(id);

        if (person == null)
        {
            Console.WriteLine("Person not found.");
            return;
        }

        Console.WriteLine(person.Study());
        SaveData();
    }

    private void Draw()
    {
        string id = ReadRequired("Unique ID: ");
        Person? person = service.FindById(id);

        if (person is IDrawable drawable)
        {
            Console.WriteLine(drawable.Draw());
            SaveData();
        }
        else
        {
            Console.WriteLine("This person cannot draw.");
        }
    }

    private void SaveData()
    {
        string[] records = repository.Serialize(service.GetAll());

        StringBuilder builder = new StringBuilder();

        foreach (string record in records)
        {
            builder.AppendLine(record);
        }

        fileStore.WriteAllText(filePath, builder.ToString());
    }

private void LoadData()
{
    if (!fileStore.FileExists(filePath))
        return;

    string content = fileStore.ReadAllText(filePath);

    if (string.IsNullOrWhiteSpace(content))
    {
        service.ReplaceAll(Array.Empty<Person>());
        return;
    }

    string normalized = content.Replace("\r\n", "\n").Trim();

    string[] records = System.Text.RegularExpressions.Regex.Split(
        normalized,
        @"(?<=;)\s*(?=Student |Postman |Mountaineer )"
    );

    Person[] people = repository.Deserialize(records);
    service.ReplaceAll(people);
}

    private void LoadSampleData()
    {
        if (service.GetAll().Length > 0)
        {
            Console.WriteLine("Sample data requires an empty database.");
            return;
        }

        Person[] samples =
        {
            new Student("Anna", "Kovalenko", 4, "KB123456", 0, "AB123456"),
            new Student("Ivan", "Petrenko", 4, "KB123457", 0, "AB123457"),
            new Student("Olena", "Shevchenko", 4, "KB123458", 1500, "AB123458"),
            new Student("Maria", "Bondarenko", 4, "KB123459", 0, "AB123459"),
            new Student("Petro", "Melnyk", 2, "KB123460", 0, "AB123460"),
            new Student("Sofia", "Tkachenko", 3, "KB123461", 2000, "AB123461"),
            new Postman("Andrii", "Moroz", "POST001"),
            new Mountaineer("Dmytro", "Savchenko", "MOUNT001")
        };

        foreach (Person person in samples)
            service.AddPerson(person);

        SaveData();
        Console.WriteLine("Eight sample people added.");
    }
}
