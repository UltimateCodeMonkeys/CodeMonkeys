using CodeMonkeys.Configuration;

namespace CodeMonkeys.UnitTests.Configuration
{
    public class MockOptionsConsumer : OptionsConsumer<MockOptions>
    {
        private readonly IOptionsConsumerForwarder _mock;

        public MockOptionsConsumer(
            IOptionsConsumerForwarder mock, 
            MockOptions options)

            : base(options)
        {
            _mock = mock;
        }

        protected override void OnOptionsChanged(MockOptions options) => 
            _mock.OnOptionsChanged(options);
    }
}
