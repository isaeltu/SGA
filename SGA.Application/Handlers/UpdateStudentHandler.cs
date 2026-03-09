using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Common;
using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Users;
using SGA.Domain.ValueObjects.Users;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SGA.Application.Handlers
{
    public class UpdateStudentHandler : IRequestHandler<UpdateStudentCommand>
    {
        public readonly IStudentRepository _studentRepository;
        private readonly IValidator<UpdateStudentCommand> _validator;

        public UpdateStudentHandler(IStudentRepository studentRepository,
            IValidator<UpdateStudentCommand> validator
            )
        {
            _studentRepository = studentRepository;
            _validator = validator;
        }

        public async Task  Handle(UpdateStudentCommand request, CancellationToken ct)
        {
            // Validar comando
            var validatorResult = await _validator.ValidateAsync(request, ct);
            if (!validatorResult.IsValid)
            {
                var errors = string.Join("; ", validatorResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }

            // Buscar estudiante
            var student = await _studentRepository.GetByIdAsync(request.studentId, ct);
            if (student == null)
            {
                throw new KeyNotFoundException($"Student with ID {request.studentId} not found.");
            }

            var updatePerson = new Person(
                student.Person.RolId,
                request.email,
                request.firstName,
                request.lastName,
                student.Person.PhoneNumber.Value,
                student.Person.Cedula
                )

            {
                Id = student.Person.Id

            };

            var UpdateStudent = new Student(
                student.Person.Id,
                request.collegeId,
                request.period,
                request.CareerName
                )
            {
                Id = student.Id,
                Person = updatePerson,
                PersonId = student.PersonId,

            };

            await _studentRepository.UpdateAsync(UpdateStudent, ct);

            // Guardar cambios
            await _studentRepository.SaveChangeAsync(ct);
        
    }
    }
}
