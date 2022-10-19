using System.Diagnostics;
using System.Linq.Expressions;
using Marten;
using Marten.AspNetCore;
using Marten.Linq;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using Shouldly;

using Xunit;
using Xunit.Abstractions;

namespace PocMarten.Tests
{
    public class IssueTests
    {
        private static readonly Random Random = new();

        private readonly ITestOutputHelper _output;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        private readonly DocumentStore _documentStore;
        private readonly IDocumentSession _documentSession;
        private readonly IQuerySession _querySession;

        public IssueTests(ITestOutputHelper output)
        {
            _output = output;
            _documentStore = DocumentStore.For(x =>
            {
                x.Connection("Username=postgres;Password=secretp@ssword;Host=127.0.0.1;Port=5433;Database=postgres;Pooling=true;");
                x.Schema.For<Issue>()
                    .ForeignKey<User>(u => u.AssignerId)
                    .ForeignKey<User>(u => u.OriginatorId);
                x.Policies.ForAllDocuments(m =>
                {
                    m.Metadata.CorrelationId.Enabled = true;
                    m.Metadata.LastModifiedBy.Enabled = true;
                });
            });

            _documentSession = _documentStore.OpenSession();
            _querySession = _documentStore.QuerySession();
        }

        [Fact]
        public async Task CreateIssue() 
        {
            var originator = new User { Id = Guid.NewGuid() , FirstName = "Ivan", LastName = "Ivanov", Role = "Supervisor"};
            var assigner = new User { Id = Guid.NewGuid() , FirstName = "Petar", LastName = "Petrov", Role = "Employee"};

            var issue = new Issue
            {
                Title = "Bad Problem",
                IsOpen = true,
                Description = "Need help fast!",
                Opened = DateTimeOffset.UtcNow,
                Tasks =
                {
                    new IssueTask("Investigate","Do some troubleshooting")
                },
                AssignerId = assigner.Id,
                OriginatorId = originator.Id
            };

            await using var writeSession = _documentStore.OpenSession();

            var docs = new List<object>
            {
                originator,
                assigner,
                issue
            };

            writeSession.Store(docs.ToArray());
            await writeSession.SaveChangesAsync().ConfigureAwait(false);

            await using var readSession = _documentStore.OpenSession();
            var dbIssue = await readSession.LoadAsync<Issue>(issue.Id).ConfigureAwait(false);

            _output.WriteLine(JsonConvert.SerializeObject(dbIssue, Formatting.Indented));

            dbIssue.ShouldNotBeNull();
        }

        [Fact]
        public async Task HasIssues()
        {
            await using var readSession = _documentStore.OpenSession();
            var issues =
                await readSession
                .Query<Issue>()
                .Where(x => x.IsOpen)
                .OrderByDescending(x => x.Opened)
                .ToListAsync()
                .ConfigureAwait(false);

            issues.ShouldNotBeNull();
        }

        [Fact]
        public async Task Query()
        {
            await using var readSession = _documentStore.OpenSession();

            var openIssues = await readSession.Query<Issue>()
                .Where(x => x.IsOpen)
                .OrderByDescending(x => x.Opened)
                .Take(10)
                .ToListAsync().ConfigureAwait(false);

            var userIds = openIssues
                .Where(x => x.AssignerId.HasValue)
                .Select(x => x.AssignerId.Value)
                .Distinct()
                .ToArray();

            var users = await readSession
                .LoadManyAsync<User>(userIds)
                .ConfigureAwait(false);

            users.ShouldNotBeNull();
        }

        [Fact]
        public async Task QueryOptimized()
        {
            await using var readSession = _documentStore.OpenSession();

            var users = new Dictionary<Guid, User>();

            var openIssues = await readSession.Query<Issue>()
                .Where(x => x.IsOpen)
                .OrderByDescending(x => x.Opened)
                .Take(10)
                .Include(x => x.AssignerId, users)
                .ToListAsync()
                .ConfigureAwait(false);

            openIssues.ShouldNotBeNull();
        }

        [Fact]
        public async Task ProcessConfigureAwait()
        {
            var resultWithOccupiedThread = await DemoAwaitWithOccupiedThread(10);
            var resultWithFreeThread = await DemoAwaitWithFreeThread(10);

            resultWithOccupiedThread.ShouldBeGreaterThan(0);
            resultWithFreeThread.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task QueryCompiled()
        {
            await using var session = _documentStore.QuerySession();

            var result = session.QueryAsync(new IssueById(Guid.Parse("0183d617-b3c4-4135-b0c8-12798d78bde9")));

            result.ShouldNotBeNull();
        }

        private async Task<int> DemoAwaitWithOccupiedThread(int loop)
        {
            var result = 0;

            for (var i = 0; i < loop; i++)
            {
                _output.WriteLine((SynchronizationContext.Current == null).ToString());
                await Task.Delay(10);
                _output.WriteLine((SynchronizationContext.Current == null).ToString());

                result += i;
            }

            return result;
        }

        private async Task<int> DemoAwaitWithFreeThread(int loop)
        {
            var result = 0;

            for (var i = 0; i < loop; i++)
            {
                _output.WriteLine((SynchronizationContext.Current == null).ToString());
                await Task.Delay(10).ConfigureAwait(false);
                _output.WriteLine((SynchronizationContext.Current == null).ToString());

                result += i;
            }

            return result;
        }
    }

    public class IssueView
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
    }

    public class IssueById : ICompiledListQuery<Issue, IssueView>
    {
        public IssueById(Guid id)
        {
            Id = id; 
        }
        
        public Guid Id { get; set; }
        
        public Expression<Func<IMartenQueryable<Issue>, IEnumerable<IssueView>>> QueryIs()
        {
            return q => q
                .Where(x => x.Id == Id)
                .OrderBy(x => x.Opened)
                .Select(x => new IssueView
                {
                    Id = x.Id,
                    Title = x.Title
                });
        }
    }

    public class Issue
    {
        public Guid Id { get; set; }

        public Guid? AssignerId { get; set; }

        public Guid? OriginatorId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsOpen { get; set; }

        public DateTimeOffset Opened { get; set; }

        public IList<IssueTask> Tasks { get; set; } = new List<IssueTask>();
    }

    public class IssueTask
    {
        public IssueTask(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTimeOffset? Started { get; set; }

        public DateTimeOffset Finished { get; set; }
    }

    public class User
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }
    }
}

