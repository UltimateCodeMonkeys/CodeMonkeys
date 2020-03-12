using CodeMonkeys.Core.Dialogs;
using CodeMonkeys.Dialogs;
using CodeMonkeys.Dialogs.Xamarin.Forms;

using NUnit.Framework;

namespace CodeMonkeys.UnitTests.Dialogs
{
    [TestFixture]
    public class DialogServiceExtensions
    {
        private IDialogService _service;

        [SetUp]
        public void Setup()
        {
            _service = new DialogService(new DialogOptions());
        }
    }
}
