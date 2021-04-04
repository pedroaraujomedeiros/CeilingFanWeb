using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Fan entity
    /// </summary>
    public class Fan
    {
        /// <summary>
        /// Fan Id
        /// </summary>
        public int FanId { get; set; }

        /// <summary>
        /// Description of Fan
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Current direction of fan
        /// </summary>
        public FanDirection Direction { get; set; } = FanDirection.FORWARD;

        /// <summary>
        /// Current speed of fan
        /// </summary>
        public FanSpeed Speed { get; set; } = FanSpeed.SPEED_0;


        /// <summary>
        /// When Fan was created
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Last update
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

    }
}
