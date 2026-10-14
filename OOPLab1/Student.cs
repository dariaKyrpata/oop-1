public class Student : Person, IDrawable
{
    public int Course { get; set; }
    public string StudentId { get; set; }
    public decimal Scholarship { get; set; }
    public string Passport { get; set; }

    public int StudySessions { get; set; }
    public int DrawingsCount { get; set; }

    public Student(
        string firstName,
        string lastName,
        int course,
        string studentId,
        decimal scholarship,
        string passport
    ) : base(firstName, lastName)
    {
        Course = course;
        StudentId = studentId;
        Scholarship = scholarship;
        Passport = passport;
    }

    public override string GetUniqueId()
    {
        return StudentId;
    }

    public override string Study()
    {
        StudySessions++;
        return "The student has completed a study session.";
    }

    public string Draw()
    {
        DrawingsCount++;
        return "The student has completed a drawing.";
    }
}
