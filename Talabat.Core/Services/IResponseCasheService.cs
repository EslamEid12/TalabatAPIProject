using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Services
{
    public interface IResponseCasheService
    {
        Task CasheResponseAsync(string CashKey, object response, TimeSpan timeToLive);
        Task<string?> GetCashedResponseAsync(string CashKey);
    }
}
