using MediatR;
using UniLx.Domain.Data;
using UniLx.Shared.Abstractions;

namespace UniLx.Infra.Data.Database
{
    public class UnitOfWork(IMartenContext martenContext, IMediator mediator) : IUnitOfWork
    {
        private readonly List<Action<IDatabaseSession>> _commands = [];
        private List<Event>? _events;
        private IMediator _mediator = mediator;

        public void AddCommand(Action<IDatabaseSession> command) => _commands.Add(command);        

        public void AddEvents(List<Event>? events)
        {
            if (events?.Count > 0)
            {
                _events ??= [];
                _events.AddRange(events);
            }
        }

        public async Task Commit(CancellationToken cancellationToken = default)
        {
            using var session = martenContext.OpenSession();
            var adaptedSession = SessionAdapter.AdaptToDatabaseSession(session);

            foreach (var command in _commands)
            {
                command(adaptedSession);
            }

            await adaptedSession.SaveChangesAsync(cancellationToken);
            await PublishEvents(cancellationToken);
        }

        private async Task PublishEvents(CancellationToken cancellationToken)
        {
            if (_events is null || _events?.Count == 0)
                return;

            var notificationsTask = _events!.Select(async (notification) =>
            {
                await _mediator.Publish(notification, cancellationToken);
            });
            await Task.WhenAll(notificationsTask);
        }
    }
}
