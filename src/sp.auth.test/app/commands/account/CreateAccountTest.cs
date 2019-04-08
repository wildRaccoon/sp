using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Moq;
using NUnit.Framework;
using sp.auth.app.account.commands.create;
using sp.auth.app.infra.ef;
using sp.auth.app.interfaces;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using sp.auth.domain.account;
using System.Linq;
using System.Linq.Expressions;
using sp.auth.test.utils;
using sp.auth.domain.account.events;

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

            mediator.Setup(s => s.Publish(It.IsAny<CreatedAccountDomainEvent>(),It.IsAny<CancellationToken>()))
                    .Returns(Task.Run(() => {}));

            var ct = new CancellationToken();

            var hashService = mockFactory.Create<IHashService>();

            var cmd = new CreateAccountCommandHandler(mediator.Object,testFixture.Context,hashService.Object);

            var cmdArgs = new CreateAccountCommand("alias","adf@email.com","Asdfg!@21234");

            var result = cmd.Handle(cmdArgs,ct);
            result.Wait();

            Assert.IsTrue(result.Result);
        }

        [TestCase()]
        public void ExistWitTheSameAlias()
        {
            var testFixture = new QueryTestFixture();

            var dbContext = testFixture.Context;
            var mockFactory = new MockRepository(MockBehavior.Loose);

            dbContext.Add(new Account(){
                Alias = "alias"
            });
            dbContext.SaveChanges();

            var mediator = mockFactory.Create<IMediator>();
            var ct = new CancellationToken();

            var hashService = mockFactory.Create<IHashService>();

            var cmd = new CreateAccountCommandHandler(mediator.Object,dbContext,hashService.Object);

            var cmdArgs = new CreateAccountCommand("alias","adf@email.com","Asdfg!@21234");

            var result = cmd.Handle(cmdArgs,ct);
            result.Wait();

            Assert.IsFalse(result.Result);
        }

        [TestCase()]
        public void ExistWitTheSameEmail()
        {
            var testFixture = new QueryTestFixture();

            var dbContext = testFixture.Context;
            var mockFactory = new MockRepository(MockBehavior.Loose);

            dbContext.Add(new Account(){
                Email = "adf@email.com"
            });
            dbContext.SaveChanges();

            var mediator = mockFactory.Create<IMediator>();
            var ct = new CancellationToken();

            var hashService = mockFactory.Create<IHashService>();

            var cmd = new CreateAccountCommandHandler(mediator.Object,dbContext,hashService.Object);

            var cmdArgs = new CreateAccountCommand("alias","adf@email.com","Asdfg!@21234");

            var result = cmd.Handle(cmdArgs,ct);
            result.Wait();

            Assert.IsFalse(result.Result);
        }
    }
}
