namespace MultitenantService.Contracts
{
    public interface ITenant
    {
        public string TenantId { get; set; }
    }
}
