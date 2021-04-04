using Domain.Entities;
using Domain.Enum;
using Services.BaseService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FanService
{
    public interface IFanService : IBaseService<Fan>
    {
        FanSpeed changeFanSpeed(Fan fan);

        FanDirection changeFanDirection(Fan fan);
    }
}
