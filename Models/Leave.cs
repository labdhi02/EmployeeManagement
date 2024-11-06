namespace EmployeeManagementSystem.Models
{
    public class Leave
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime LeaveDate { get; set; }
        public string LeaveType { get; set; }
        public string Reason { get; set; }
        public Employee Employee { get; set; }
    }
}
