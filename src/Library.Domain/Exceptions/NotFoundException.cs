namespace Library.Domain.Exceptions
{
    public class NotFoundException : DomainException
    {
        public string EntityName { get; }
        public object EntityId { get; }

        public NotFoundException(string entityName, object entityId)
            : base($"{entityName} con ID '{entityId}' no encontrado.")
        {
            EntityName = entityName;
            EntityId = entityId;
        }
    }
}