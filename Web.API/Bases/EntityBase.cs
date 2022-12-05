namespace Web.API.Bases
{
    public class EntityBase
    {
        public EntityBase()
        {
            Id = Guid.NewGuid();
            IsActive = true;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public void Activate()
        {
            IsActive = true;
        }
        public void DeActivate()
        {
            IsActive = false;
        }
    }
}
