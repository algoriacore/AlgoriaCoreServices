namespace AlgoriaCore.Domain.Interfaces
{
    public interface IEntity<TType>
        //where TType : struct
    {
        TType Id { get; set; }

        bool IsTransient();
        bool MustHaveTenant();
        bool MayHaveTenant();
    }
}
