namespace CSBEF.Module.UserManagement.Models.Return {
    public class ChangeStatusHubSyncModel {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool Status { get; set; }
        public string Name { get; set; }
    }
}