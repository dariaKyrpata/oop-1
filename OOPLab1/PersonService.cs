using System;

public class PersonService
{
    private Person[] people = Array.Empty<Person>();

    public bool AddPerson(Person person)
    {
        if (person == null)
        {
            return false;
        }

        if (FindById(person.GetUniqueId()) != null)
        {
            return false;
        }

        Array.Resize(ref people, people.Length + 1);
        people[people.Length - 1] = person;

        return true;
    }

    public Person[] GetAll()
    {
        return (Person[])people.Clone();
    }

    public Person? FindById(string id)
    {
        foreach (Person person in people)
        {
            if (string.Equals(
                person.GetUniqueId(),
                id,
                StringComparison.OrdinalIgnoreCase))
            {
                return person;
            }
        }

        return null;
    }

    public Person[] FindByLastName(string lastName)
    {
        Person[] result = Array.Empty<Person>();

        foreach (Person person in people)
        {
            if (string.Equals(
                person.LastName,
                lastName,
                StringComparison.OrdinalIgnoreCase))
            {
                Array.Resize(ref result, result.Length + 1);
                result[result.Length - 1] = person;
            }
        }

        return result;
    }

    public bool DeleteById(string id)
    {
        for (int i = 0; i < people.Length; i++)
        {
            if (string.Equals(
                people[i].GetUniqueId(),
                id,
                StringComparison.OrdinalIgnoreCase))
            {
                for (int j = i; j < people.Length - 1; j++)
                {
                    people[j] = people[j + 1];
                }

                Array.Resize(ref people, people.Length - 1);
                return true;
            }
        }

        return false;
    }

    public Student[] GetFourthYearStudentsWithoutScholarship()
    {
        Student[] result = Array.Empty<Student>();

        foreach (Person person in people)
        {
            if (person is Student student &&
                student.Course == 4 &&
                student.Scholarship == 0)
            {
                Array.Resize(ref result, result.Length + 1);
                result[result.Length - 1] = student;
            }
        }

        return result;
    }

    public int CountFourthYearStudentsWithoutScholarship()
    {
        return GetFourthYearStudentsWithoutScholarship().Length;
    }

    public void ReplaceAll(Person[] newPeople)
    {
        people = (Person[])newPeople.Clone();
    }
}
