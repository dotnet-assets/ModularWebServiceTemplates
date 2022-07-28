namespace MyPetProject.SomeModule.Model;

internal class Entity
{
    public Entity(string someProperty)
        : this(default, someProperty)
    {
    }

    public Entity(int id, string someProperty)
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
    private Entity()
    {
        Id = default!;
        SomeProperty = default!;
    }
}