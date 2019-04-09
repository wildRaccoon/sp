using MediatR;
using Moq;
using NUnit.Framework;
using sp.auth.app.account.commands.create;
using sp.auth.app.interfaces;
using System.Threading;
using System.Threading.Tasks;
using sp.auth.domain.account;
using sp.auth.test.utils;
using sp.auth.domain.account.events;
using sp.auth.domain.account.exceptions;

namespace sp.auth.test.app.commands.account
{
    [TestFixture]
    public class CreateAccountTest
    {

        [TestCase()]
        public void Success()
        {
            var testFixture = new QueryTestFixture();
            var mockFactory = new MockRepository(MockBehavior.Loose);

            var mediator = mockFactory.Create<IMediator>();

            mediator.Setup(s => s.Publish(It.IsAny<CreatedAccountDomainEvent>(), It.IsAny<CancellationToken>()))
                .Returns(Task.Run(() => { }));

            var ct = new CancellationToken();

            var hashService = mockFactory.Create<IHashService>();

            var cmd = new CreateAccountCommandHandler(mediator.Object, testFixture.Context, hashService.Object);

            var cmdArgs = new CreateAccountCommand("alias", "adf@email.com", "Asdfg!@21234");
            
            Assert.DoesNotThrowAsync(async () => await cmd.Handle(cmdArgs, ct));
        }

        [TestCase()]
        public void ExistWitTheSameAlias()
        {
            var testFixture = new QueryTestFixture();

            var dbContext = testFixture.Context;
            var mockFactory = new MockRepository(MockBehavior.Loose);

            dbContext.Add(new Account()
            {
                Alias = "alias"
            });
            dbContext.SaveChanges();

            var mediator = mockFactory.Create<IMediator>();
            var ct = new CancellationToken();

            var hashService = mockFactory.Create<IHashService>();

            var cmd = new CreateAccountCommandHandler(mediator.Object, dbContext, hashService.Object);

            var cmdArgs = new CreateAccountCommand("alias", "adf@email.com", "Asdfg!@21234");

            Assert.ThrowsAsync<UnableCreateAccountException>(async () => await cmd.Handle(cmdArgs, ct));
        }

        [TestCase()]
        public void ExistWitTheSameEmail()
        {

            var testFixture = new QueryTestFixture();

            var dbContext = testFixture.Context;
            var mockFactory = new MockRepository(MockBehavior.Loose);

            dbContext.Add(new Account()
            {
                Email = "adf@email.com"
            });
            dbContext.SaveChanges();

            var mediator = mockFactory.Create<IMediator>();
            var ct = new CancellationToken();

            var hashService = mockFactory.Create<IHashService>();

            var cmd = new CreateAccountCommandHandler(mediator.Object, dbContext, hashService.Object);

            var cmdArgs = new CreateAccountCommand("alias", "adf@email.com", "Asdfg!@21234");
            
            Assert.ThrowsAsync<UnableCreateAccountException>(async () => await cmd.Handle(cmdArgs, ct));
        }
    }
}
