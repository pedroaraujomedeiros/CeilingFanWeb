using Domain.Entities;
using Infra.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FanService
{
    /// <summary>
    /// Fan Service Factory
    /// </summary>
    public class FanServiceFactory
    {
        public static IFanService createInstance(IUnitOfWork uow)
        {
            return new FanService(uow);
        }
    }
}
