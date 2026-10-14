public class Mountaineer : Person
{
    public string MountaineerId { get; set; }
    public int StudySessions { get; set; }

    public Mountaineer(
        string firstName,
        string lastName,
        string mountaineerId
    ) : base(firstName, lastName)
    {
        MountaineerId = mountaineerId;
    }

    public override string GetUniqueId()
    {
        return MountaineerId;
    }

    public override string Study()
    {
        StudySessions++;
        return "The mountaineer has completed climbing training.";
    }
}
