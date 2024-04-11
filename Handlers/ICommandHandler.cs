namespace GuestApi.Handlers
{
    public interface ICommandHandler<TCommand>
    {
        void Handle(TCommand command);
    }
}
