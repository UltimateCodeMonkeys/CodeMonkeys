namespace CodeMonkeys.UnitTests.Configuration
{
    public interface IOptionsConsumerForwarder
    {
        void OnOptionsChanged(MockOptions options);
    }
}
