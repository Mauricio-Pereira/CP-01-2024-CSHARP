namespace CP_01___2024;

public class SfCard
{
    public int ID { get; set; }
    public string NAME { get; set; }
    public string DESCRIPTION { get; set; }

    public SfCard()
    {
    }

    public SfCard(string name, string description)
    {
        NAME = name;
        DESCRIPTION = description;
    }

    public SfCard(string name)
    {
        NAME = name;
    }
}