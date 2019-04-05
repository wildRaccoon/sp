using System;
using System.Linq;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using sp.auth.app.commands.account.create;
using sp.auth.app.interfaces;
using sp.auth.domain.account;
using System.Threading.Tasks;
using sp.auth.app.interfaces.queries;

namespace sp.auth.console
{
    class Program
    {
        static void Main(string[] args)
        {
            var sc = new ServiceCollection();

            sc.AddMediatR(typeof(CreateAccountCommandHandler));

            var factory = new MockRepository(MockBehavior.Loose);

            var repoAcc = factory.Create<IRepository<Account>>();
            sc.AddSingleton<IRepository<Account>>(repoAcc.Object);

            var query = factory.Create<IUniqueAccountQuery>();
            sc.AddSingleton<IUniqueAccountQuery>(query.Object);

            repoAcc.Setup(s => s.Add(It.IsAny<Account>()));
            repoAcc.Setup(s => s.SaveAsync()).Returns(Task.FromResult(true));

            var hash = factory.Create<IHashService>();
            sc.AddSingleton<IHashService>(hash.Object);

            hash.Setup(s => s.Encode(It.IsAny<string>())).Returns("hash");

            var sp = sc.BuildServiceProvider();

            var mediator = sp.GetService<IMediator>();

            var command = new CreateAccountCommand("a","b","aA,1aaaaa");
            var validator = new CreateAccountCommandValidator();
            
            var resValidation =  validator.Validate(command);

            if(!resValidation.IsValid)
            {
                foreach( var err in resValidation.Errors )
                {
                    Console.WriteLine($"Error {err.ErrorCode}:{err.ErrorMessage}");
                }
            }
            else
            {
                Console.WriteLine($"Command: Valid");
            }

            var res = mediator.Send(command);

            res.Wait();

            Console.WriteLine($"Command Res: {res.Result}");
        }
    }
}
