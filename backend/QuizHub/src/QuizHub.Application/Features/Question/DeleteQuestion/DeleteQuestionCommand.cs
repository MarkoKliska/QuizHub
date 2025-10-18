using MediatR;
using QuizHub.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizHub.Application.Features.Question.DeleteQuestion;

public sealed record DeleteQuestionCommand(Guid Id) 
    : IRequest<Result<Unit>>;
