namespace ConsumerAPI
{
    public class EmployeeReport
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }

        public EmployeeReport(Guid id, Guid employeeId, string name, string surName)
        {
            Id = id;
            Name = name;
            SurName = surName;
            EmployeeId = employeeId;
        }
    }
}
