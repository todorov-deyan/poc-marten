using System.Diagnostics;

using Marten;

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

        private readonly DocumentStore _store;

        public IssueTests(ITestOutputHelper output)
        {
            _output = output;
            _store = DocumentStore.For(x =>
            {
                x.Connection("Username=postgres;Password=secretp@ssword;Host=127.0.0.1;Port=5433;Database=postgres;Pooling=true;");
                x.Schema.For<Issue>()
                    .ForeignKey<User>(u => u.AssigneeId)
                    .ForeignKey<User>(u => u.OriginatorId);
            });
        }

        [Fact]
        public async Task CreateIssue()
        {
            var originator = new User { Id = Guid.NewGuid() , FirstName = "Ivan", LastName = "Ivanov", Role = "Supervisor"};
            var assignee = new User { Id = Guid.NewGuid() , FirstName = "Petar", LastName = "Petrov", Role = "Employee"};

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
                AssigneeId = assignee.Id,
                OriginatorId = originator.Id
            };

            await using var writeSession = _store.OpenSession();
            writeSession.Store(originator);
            writeSession.Store(assignee);
            await writeSession.SaveChangesAsync().ConfigureAwait(false);

            writeSession.Store(issue);
            await writeSession.SaveChangesAsync().ConfigureAwait(false);

            await using var readSession = _store.OpenSession();
            var dbIssue = await readSession.LoadAsync<Issue>(issue.Id).ConfigureAwait(false);

            _output.WriteLine(JsonConvert.SerializeObject(dbIssue, Formatting.Indented));

            dbIssue.ShouldNotBeNull();
        }

        [Fact]
        public async Task HasIssues()
        {
            await using var readSession = _store.OpenSession();
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
        public async Task ProcessConfigureAwait()
        {
            var resultWithOccupiedThread = await DemoAwaitWithOccupiedThread(10);
            var resultWithFreeThread = await DemoAwaitWithFreeThread(10);

            resultWithOccupiedThread.ShouldBeGreaterThan(0);
            resultWithFreeThread.ShouldBeGreaterThan(0);
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

    public class Issue
    {
        public Guid Id { get; set; }

        public Guid? AssigneeId { get; set; }

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

