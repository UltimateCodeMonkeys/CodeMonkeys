using CodeMonkeys.Configuration;

namespace CodeMonkeys.UnitTests.Configuration
{
    public class MockOptions : Options
    {
        public bool Prop1
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public int Prop2
        {
            get => GetValue<int>();
            set => SetValue(value);
        }
    }
}
