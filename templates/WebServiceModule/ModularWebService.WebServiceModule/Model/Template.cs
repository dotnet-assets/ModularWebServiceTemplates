namespace ModularWebService.WebServiceModule.Model;

internal class Template
{
    public Template(string someProperty)
        : this(default, someProperty)
    {
    }

    public Template(int id, string someProperty)
    {
        Id = id;
        SomeProperty = someProperty;
    }

    public int Id { get; private set; }

    public string SomeProperty { get; private set; }

    public void Update(string someProperty)
    {
        // TODO: Validate
        SomeProperty = someProperty;
    }

    // EF Core requires a default constructor
    private Template()
    {
        Id = default!;
        SomeProperty = default!;
    }
}