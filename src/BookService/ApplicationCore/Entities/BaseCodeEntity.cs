namespace Pillow.ApplicationCore.Entities
{
    public abstract class BaseCodeEntity : BaseEntity
    {
        public string Code { get; protected set; }
    }
}