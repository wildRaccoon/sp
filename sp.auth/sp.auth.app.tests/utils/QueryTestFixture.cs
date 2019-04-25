using System;
using sp.auth.app.infra.ef;

namespace sp.auth.app.tests.utils
{
    public class QueryTestFixture : IDisposable
    {
        public AuthDataContext Context { get; private set; }

        public QueryTestFixture()
        {
            Context = TestDbContextFactory.Create();
        }

        public void Dispose()
        {
            TestDbContextFactory.Destroy(Context);
        }
    }
}