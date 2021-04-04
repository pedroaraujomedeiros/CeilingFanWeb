using Domain.Entities;
using Domain.Enum;
using Infra.UnitOfWork;
using Services.BaseService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FanService
{
    public class FanService : BaseService<Fan> , IFanService
    {
        #region [Fields]


        #endregion

        #region [Constructor]

        public FanService(IUnitOfWork uow) : base(uow)
        {

        }


        #endregion

        #region [Methods]

        public override Fan Insert<TValidator>(Fan instance, string[] ruleSet = null)
        {
            instance.CreatedAt = DateTime.Now;
            return base.Update<TValidator>(instance, ruleSet);
        }

        public override Fan Update<TValidator>(Fan instance, string[] ruleSet = null)
        {
            instance.UpdatedAt = DateTime.Now;
            return base.Update<TValidator>(instance, ruleSet);
        }

        /// <summary>
        /// Change fan direction
        /// </summary>
        /// <param name="fan"></param>
        public FanDirection changeFanDirection(Fan fan)
        {
            switch (fan.Direction)
            {
                case FanDirection.FORWARD:
                    fan.Direction = FanDirection.REVERSE;
                    break;
                case FanDirection.REVERSE:
                    fan.Direction = FanDirection.FORWARD;
                    break;
                default:
                    fan.Direction = FanDirection.FORWARD;
                    break;
            }

            return fan.Direction;
        }


        /// <summary>
        /// Change fan speed
        /// </summary>
        /// <param name="fan"></param>
        public FanSpeed changeFanSpeed(Fan fan)
        {
            switch (fan.Speed)
            {
                case FanSpeed.SPEED_0:
                    fan.Speed = FanSpeed.SPEED_1;
                    break;
                case FanSpeed.SPEED_1:
                    fan.Speed = FanSpeed.SPEED_2;
                    break;
                case FanSpeed.SPEED_2:
                    fan.Speed = FanSpeed.SPEED_3;
                    break;
                case FanSpeed.SPEED_3:
                    fan.Speed = FanSpeed.SPEED_0;
                    break;
                default:
                    fan.Speed = FanSpeed.SPEED_0;
                    break;
            }

            return fan.Speed;
        }

        #endregion
    }
}
