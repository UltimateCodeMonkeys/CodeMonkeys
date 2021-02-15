using CodeMonkeys.DependencyInjection.DryIoC;
using CodeMonkeys.MVVM;
using CodeMonkeys.UnitTests.MVVM.ViewModels;

using NUnit.Framework;

using System;
using System.Threading.Tasks;

namespace CodeMonkeys.UnitTests.MVVM
{
    [TestFixture]
    public class ViewModelFactoryTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            var container = DryFactory
                .CreateInstance();

            container.RegisterType<ViewModel>();
            container.RegisterType<ViewModelWithModel>();
            container.RegisterType<ViewModelWithoutInterface>();

            ViewModelFactory.Configure(
                container);
        }

        [Test]
        public void Resolve_ReturnsViewModel()
        {
            var viewModel = ViewModelFactory.Resolve(
                typeof(ViewModel));

            Assert.IsNotNull(viewModel);
        }

        [Test]
        public void Resolve_ViewModelTypeNull_ThrowsArgumentNullException()
        {
            var ex = Assert.Throws<TypeLoadException>(() => 
                ViewModelFactory.Resolve(null));

            Assert.That(
                ex.InnerException, 
                Is.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Resolve_ViewModelTypeNotBasedOnInterface_ThrowsInvalidOperationException()
        {
            var ex = Assert.Throws<TypeLoadException>(() =>
                ViewModelFactory.Resolve(typeof(ViewModelWithoutInterface)));

            Assert.That(
                ex.InnerException,
                Is.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Resolve_ViewModelNotRegistered_ThrowsTypeLoadException()
        {
            Assert.Throws<TypeLoadException>(() =>
                ViewModelFactory.Resolve(typeof(NotRegisteredViewModel)));
        }




        [Test]
        public async Task ResolveAsync_ReturnsViewModel()
        {
            var viewModel = await ViewModelFactory.ResolveAsync(
                typeof(ViewModel));

            Assert.IsNotNull(viewModel);
        }

        [Test]
        public void ResolveAsync_ViewModelTypeNull_ThrowsArgumentNullException()
        {
            var ex = Assert.ThrowsAsync<TypeLoadException>(() =>
                ViewModelFactory.ResolveAsync(null));

            Assert.That(
                ex.InnerException,
                Is.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ResolveAsync_ViewModelTypeNotBasedOnInterface_ThrowsInvalidOperationException()
        {
            var ex = Assert.ThrowsAsync<TypeLoadException>(() =>
                ViewModelFactory.ResolveAsync(typeof(ViewModelWithoutInterface)));

            Assert.That(
                ex.InnerException,
                Is.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ResolveAsync_ViewModelNotRegistered_ThrowsTypeLoadException()
        {
            Assert.ThrowsAsync<TypeLoadException>(() =>
                ViewModelFactory.ResolveAsync(typeof(NotRegisteredViewModel)));
        }




        [Test]
        public void ResolveWithModel_ReturnsViewModel()
        {
            var viewModel = ViewModelFactory.Resolve(
                typeof(ViewModelWithModel),
                new Model());

            Assert.IsNotNull(viewModel);
        }

        [Test]
        public void ResolveWithModel_ViewModelTypeNull_ThrowsArgumentNullException()
        {
            var ex = Assert.Throws<TypeLoadException>(() =>
                ViewModelFactory.Resolve(null, new Model()));

            Assert.That(
                ex.InnerException,
                Is.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ResolveWithModel_ViewModelTypeNotBasedOnInterface_ThrowsInvalidOperationException()
        {
            var ex = Assert.Throws<TypeLoadException>(() =>
                ViewModelFactory.Resolve(typeof(ViewModelWithoutInterface), new Model()));

            Assert.That(
                ex.InnerException,
                Is.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ResolveWithModel_ViewModelNotRegistered_ThrowsTypeLoadException()
        {
            Assert.Throws<TypeLoadException>(() =>
                ViewModelFactory.Resolve(typeof(NotRegisteredViewModel), new Model()));
        }




        [Test]
        public async Task ResolveAsyncWithModel_ReturnsViewModel()
        {
            var viewModel = await ViewModelFactory.ResolveAsync(
                typeof(ViewModelWithModel),
                new Model());

            Assert.IsNotNull(viewModel);
        }

        [Test]
        public void ResolveAsyncWithModel_ViewModelTypeNull_ThrowsArgumentNullException()
        {
            var ex = Assert.ThrowsAsync<TypeLoadException>(() =>
                ViewModelFactory.ResolveAsync(null, new Model()));

            Assert.That(
                ex.InnerException,
                Is.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ResolveAsyncWithModel_ViewModelTypeNotBasedOnInterface_ThrowsInvalidOperationException()
        {
            var ex = Assert.ThrowsAsync<TypeLoadException>(() =>
                ViewModelFactory.ResolveAsync(typeof(ViewModelWithoutInterface), new Model()));

            Assert.That(
                ex.InnerException,
                Is.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ResolveAsyncWithModel_ViewModelNotRegistered_ThrowsTypeLoadException()
        {
            Assert.ThrowsAsync<TypeLoadException>(() =>
                ViewModelFactory.ResolveAsync(typeof(NotRegisteredViewModel), new Model()));
        }
    }
}
