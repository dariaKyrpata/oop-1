using System.Text.RegularExpressions;

public static class PersonValidator
{
    public static bool IsValidName(string value)
    {
        return !string.IsNullOrWhiteSpace(value)
            && Regex.IsMatch(value, @"^[\p{L}][\p{L}' -]*$");
    }

    public static bool IsValidCourse(int course)
    {
        return course >= 1 && course <= 6;
    }

    public static bool IsValidStudentId(string studentId)
    {
        return !string.IsNullOrWhiteSpace(studentId)
            && Regex.IsMatch(studentId, @"^[A-Z]{2}\d{6}$");
    }

    public static bool IsValidPassport(string passport)
    {
        return !string.IsNullOrWhiteSpace(passport)
            && Regex.IsMatch(passport, @"^[A-Z]{2}\d{6}$");
    }

    public static bool IsValidScholarship(decimal scholarship)
    {
        return scholarship >= 0;
    }

    public static bool IsValidUniqueId(string id)
    {
        return !string.IsNullOrWhiteSpace(id)
            && Regex.IsMatch(id, @"^[A-Za-z0-9-]{3,20}$");
    }

    public static bool IsValidStudent(Student student)
    {
        return IsValidName(student.FirstName)
            && IsValidName(student.LastName)
            && IsValidCourse(student.Course)
            && IsValidStudentId(student.StudentId)
            && IsValidPassport(student.Passport)
            && IsValidScholarship(student.Scholarship);
    }
}
