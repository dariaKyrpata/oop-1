public class Postman : Person
{
    public string EmployeeId { get; set; }
    public int StudySessions { get; set; }

    public Postman(
        string firstName,
        string lastName,
        string employeeId
    ) : base(firstName, lastName)
    {
        EmployeeId = employeeId;
    }

    public override string GetUniqueId()
    {
        return EmployeeId;
    }

    public override string Study()
    {
        StudySessions++;
        return "The postman has completed delivery training.";
    }
}
