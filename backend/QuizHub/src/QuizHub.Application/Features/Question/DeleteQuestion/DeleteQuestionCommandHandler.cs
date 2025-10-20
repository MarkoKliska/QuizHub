using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.Interfaces;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.Question.DeleteQuestion;

public sealed class DeleteQuestionCommandHandler(
    IQuestionRepository questionRepository,
    IUnitOfWork uow
) : IRequestHandler<DeleteQuestionCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteQuestionCommand command, CancellationToken ct)
    {
        var question = await questionRepository.GetByIdAsync(command.Id, ct);
        if (question == null || question.IsDeleted)
            return Result<Unit>.Failure("Question not found.");

        await questionRepository.DeleteAsync(command.Id, ct);
        await uow.SaveChangesAsync(ct);

        return Result<Unit>.Success(Unit.Value);
    }
}
