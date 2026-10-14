using System;
using System.Text.Json;

public class PersonRepository
{
    private readonly JsonSerializerOptions options = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public string[] Serialize(Person[] people)
    {
        string[] result = new string[people.Length];

        for (int i = 0; i < people.Length; i++)
        {
            Person person = people[i];

            string type;
            string data;

            if (person is Student student)
            {
                type = "Student";
                data = JsonSerializer.Serialize(student, options);
            }
            else if (person is Postman postman)
            {
                type = "Postman";
                data = JsonSerializer.Serialize(postman, options);
            }
            else if (person is Mountaineer mountaineer)
            {
                type = "Mountaineer";
                data = JsonSerializer.Serialize(mountaineer, options);
            }
            else
            {
                throw new NotSupportedException(
                    "Unsupported person type."
                );
            }

            result[i] = type + " " + person.GetUniqueId()
                + Environment.NewLine
                + data + ";";
        }

        return result;
    }

    public Person[] Deserialize(string[] records)
    {
        Person[] people = new Person[records.Length];
        int count = 0;

        foreach (string record in records)
        {
            if (string.IsNullOrWhiteSpace(record))
                continue;

            string normalized = record.Replace("\r\n", "\n");

            int separator = normalized.IndexOf('\n');

            if (separator < 0)
            {
                throw new FormatException(
                    "Invalid record format."
                );
            }

            string header = normalized.Substring(0, separator).Trim();
            string json = normalized.Substring(separator + 1).Trim();

            if (json.EndsWith(";"))
                json = json.Substring(0, json.Length - 1).Trim();

            Person? person = null;

            if (header.StartsWith("Student "))
            {
                person = JsonSerializer.Deserialize<Student>(
                    json, options
                );
            }
            else if (header.StartsWith("Postman "))
            {
                person = JsonSerializer.Deserialize<Postman>(
                    json, options
                );
            }
            else if (header.StartsWith("Mountaineer "))
            {
                person = JsonSerializer.Deserialize<Mountaineer>(
                    json, options
                );
            }

            if (person == null)
            {
                throw new FormatException(
                    "Unknown or invalid person record."
                );
            }

            people[count] = person;
            count++;
        }

        Array.Resize(ref people, count);

        return people;
    }
}
