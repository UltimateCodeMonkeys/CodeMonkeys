using CodeMonkeys.Configuration;

namespace CodeMonkeys.UnitTests.Configuration
{
    public class MockOptions : Options
    {
        private bool _prop1;
        private int _prop2;

        public bool Prop1
        {
            get => _prop1;
            set => SetValue(ref _prop1, value);
        }

        public int Prop2
        {
            get => _prop2;
            set => SetValue(ref _prop2, value, false);
        }
    }
}
