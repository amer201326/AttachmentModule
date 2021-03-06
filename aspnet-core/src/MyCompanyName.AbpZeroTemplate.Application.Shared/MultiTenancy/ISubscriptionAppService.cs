using System.Threading.Tasks;
using Abp.Application.Services;

namespace MyCompanyName.AbpZeroTemplate.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
