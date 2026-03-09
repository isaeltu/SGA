using MediatR;
using SGA.Application.DTOs.Users;

namespace SGA.Application.Queries
{
    public sealed record GetStudentByIdQuery(int StudentId) : IRequest<StudentDto?>;
}
