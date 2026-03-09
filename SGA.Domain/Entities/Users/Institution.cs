using SGA.Domain.Base;
using SGA.Domain.Entities.Transportation;
using SGA.Domain.Entities.Trips;

namespace SGA.Domain.Entities.Users
{
    public class Institution : BaseEntity<int>, IAuditEntity
    {
        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public bool IsActive { get; private set; } = true;

        public ICollection<Bus> Buses { get; set; } = new List<Bus>();
        public ICollection<College> Colleges { get; set; } = new List<College>();
        public ICollection<Department> Departments { get; set; } = new List<Department>();
        public ICollection<Person> People { get; set; } = new List<Person>();
        public ICollection<Route> Routes { get; set; } = new List<Route>();
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();

        protected Institution() { }

        public Institution(string code, string name, string createdBy)
        {
            Code = code.Trim();
            Name = name.Trim();
            IsActive = true;
            SetCreationInfo(createdBy);
        }

        public void Deactivate(string modifiedBy)
        {
            IsActive = false;
            SetModificationInfo(modifiedBy);
        }

        public void Activate(string modifiedBy)
        {
            IsActive = true;
            SetModificationInfo(modifiedBy);
        }

        public void UpdateDetails(string code, string name, string modifiedBy)
        {
            Code = code.Trim();
            Name = name.Trim();
            SetModificationInfo(modifiedBy);
        }
    }
}